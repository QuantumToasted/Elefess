using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Elefess;
using Elefess.Models;
using Microsoft.AspNetCore.Http;

namespace Elefess.Hosting.AspNetCore;

internal sealed class LfsErrorResponseResult : IResult
{
    private readonly HttpStatusCode _statusCode;
    private readonly LfsErrorResponse _error;

    public LfsErrorResponseResult(HttpStatusCode statusCode, LfsErrorResponse error)
    {
        _statusCode = statusCode;
        _error = error;
    }
    
    public LfsErrorResponseResult(HttpStatusCode statusCode, string message, Uri? documentationUri = null, string? requestId = null)
        : this(statusCode, new LfsErrorResponse(message, documentationUri, requestId))
    { }

    public Task ExecuteAsync(HttpContext httpContext)
    {
        httpContext.Response.StatusCode = (int) _statusCode;
        httpContext.Response.ContentType = LfsUtil.Constants.Headers.Values.CONTENT_TYPE;
        
        if (_statusCode == HttpStatusCode.Unauthorized)
        {
            httpContext.Response.Headers.Add(LfsUtil.Constants.Headers.Names.LFS_AUTHENTICATE, LfsUtil.Constants.Headers.Values.LFS_AUTHENTICATE_VALUE);
        }
        
        return httpContext.Response.WriteAsync(JsonSerializer.Serialize(_error, 
            new JsonSerializerOptions{Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin)}));
    }
}