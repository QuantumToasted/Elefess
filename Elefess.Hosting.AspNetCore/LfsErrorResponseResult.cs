using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Elefess.Models;
using Microsoft.AspNetCore.Http;

namespace Elefess.Hosting.AspNetCore;

internal sealed class LfsErrorResponseResult(HttpStatusCode statusCode, LfsErrorResponse error) : IResult
{
    private static readonly JsonSerializerOptions JsonOptions = new() { Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin) };

    public LfsErrorResponseResult(HttpStatusCode statusCode, string message, Uri? documentationUri = null, string? requestId = null)
        : this(statusCode, new LfsErrorResponse { Message = message, DocumentationUri = documentationUri, RequestId = requestId })
    { }

    public Task ExecuteAsync(HttpContext httpContext)
    {
        httpContext.Response.StatusCode = (int) statusCode;
        httpContext.Response.ContentType = LfsUtil.Constants.Headers.Values.CONTENT_TYPE;
        
        if (statusCode == HttpStatusCode.Unauthorized)
        {
            httpContext.Response.Headers.Append(LfsUtil.Constants.Headers.Names.LFS_AUTHENTICATE, LfsUtil.Constants.Headers.Values.LFS_AUTHENTICATE_VALUE);
        }

        return httpContext.Response.WriteAsync(JsonSerializer.Serialize(error, JsonOptions));
    }
}