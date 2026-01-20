using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text;
using Elefess.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Elefess.Hosting.AspNetCore;

/// <summary>
/// Various extension methods for mapping Git LFS endpoints to an <see cref="IEndpointConventionBuilder"/>.
/// </summary>
public static class EndpointRouteBuilderExtensions
{
    extension(IEndpointRouteBuilder builder)
    {
        /// <summary>
        /// Maps a <c>POST</c> request for the Git LFS batch object endpoint.
        /// </summary>
        /// <param name="route">The route to the endpoint. Defaults to <c>/objects/batch</c>, the default in the API spec.</param>
        /// <returns></returns>
        public IEndpointConventionBuilder MapGitLfsBatch(string route = "/objects/batch")
        {
            return builder.MapPost(route, PostObjectsBatch);

            static async Task<IResult> PostObjectsBatch(HttpContext context,
                [FromServices] ILfsAuthenticator authenticator,
                [FromServices] ILfsTransferSelector selector,
                [FromServices] ILfsObjectManager objectManager,
                [FromServices] ILoggerFactory loggerFactory,
                CancellationToken cancellationToken,
                [FromBody] LfsBatchTransferRequest request)
            {
                var logger = loggerFactory.CreateLogger(nameof(PostObjectsBatch));

                // TODO: Separate this back into an IEndpointFilter?
                // We will need to read the response body TWICE if so. (Formerly BasicAuthEndpointFilter)

                #region IEndpointFilter

                const string basicHeaderValueStart = "Basic";

                if (context.Request.Headers.Authorization.FirstOrDefault() is not { } headerValue)
                    return new LfsErrorResponseResult(HttpStatusCode.Unauthorized, "Missing Authorization header.");

                if (!headerValue.StartsWith(basicHeaderValueStart))
                    return new LfsErrorResponseResult(HttpStatusCode.Unauthorized, "Invalid Authorization header.");

                headerValue = headerValue[
                    (headerValue.IndexOf(basicHeaderValueStart, StringComparison.Ordinal) + basicHeaderValueStart.Length + 1)..];

                if (!TryDecodeCredentials(headerValue, out var id, out var password))
                    return new LfsErrorResponseResult(HttpStatusCode.Unauthorized, "Invalid Authorization header.");

                try
                {
                    await authenticator.AuthenticateAsync(id, password, request.Operation, cancellationToken);
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "Failed to authenticate user.");
                    return new LfsErrorResponseResult(HttpStatusCode.Unauthorized, ex.Message);
                }

                if (context.Request.Headers.Accept.FirstOrDefault()?.StartsWith(LfsUtil.Constants.Headers.Values.ACCEPT) != true)
                {
                    return new LfsErrorResponseResult(HttpStatusCode.BadRequest,
                        $"Invalid Accept header start - expected \"{LfsUtil.Constants.Headers.Values.ACCEPT}\", got \"{context.Request.Headers.Accept}\".");
                }

                if (context.Request.ContentType?.StartsWith(LfsUtil.Constants.Headers.Values.CONTENT_TYPE) != true)
                {
                    return new LfsErrorResponseResult(HttpStatusCode.UnsupportedMediaType,
                        $"Invalid Content-Type header start - expected \"{LfsUtil.Constants.Headers.Values.CONTENT_TYPE}\", got \"{context.Request.ContentType}\".");
                }

                if (context.RequestServices.GetService<ILfsRequestValidator>() is { } validator)
                {
                    try
                    {
                        await validator.ValidateAsync(request);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Failed to validate LfsBatchTransferRequest.");
                        return new LfsErrorResponseResult(HttpStatusCode.UnprocessableEntity, ex.Message);
                    }
                }

                #endregion

                LfsTransferAdapter selectedTransferAdapter;
                try
                {
                    selectedTransferAdapter = await selector.SelectTransferAsync(request.TransferAdapters, cancellationToken);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Failed to select appropriate transfer type.");
                    return new LfsErrorResponseResult(HttpStatusCode.UnprocessableEntity, ex.Message);
                }

                var responseObjects = await objectManager.CreateObjectsAsync(request.Objects.ToList(), request.Operation, cancellationToken);

                return new LfsTransferResponseResult(selectedTransferAdapter, responseObjects, request.HashAlgorithm);

                static bool TryDecodeCredentials(string headerValue,
                    [NotNullWhen(true)] out string? id,
                    [NotNullWhen(true)] out string? password)
                {
                    (id, password) = (null, null);

                    try
                    {
                        var base64Bytes = Convert.FromBase64String(headerValue);
                        var decoded = Encoding.UTF8.GetString(base64Bytes);
                        var split = decoded.Split(':', 2, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                        (id, password) = (split[0], split[1]);
                        return true;
                    }
                    catch // TODO: logging?
                    {
                        return false;
                    }
                }
            }
        }
    }
}