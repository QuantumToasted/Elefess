using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Elefess;
using Elefess.Models;
using Microsoft.AspNetCore.Http;

namespace Elefess.Hosting.AspNetCore;

internal sealed class LfsTransferResponseResult : IResult
{
    private readonly LfsBatchTransferResponse _transfer;

    public LfsTransferResponseResult(LfsBatchTransferResponse transfer)
    {
        _transfer = transfer;
    }
    
    public LfsTransferResponseResult(LfsTransfer transfer, IReadOnlyCollection<LfsResponseObject> objects, string? hashAlgorithm = null)
        : this(new LfsBatchTransferResponse(transfer, objects, hashAlgorithm))
    { }
    
    public Task ExecuteAsync(HttpContext httpContext)
    {
        httpContext.Response.StatusCode = (int) HttpStatusCode.OK;
        httpContext.Response.ContentType = LfsUtil.Constants.Headers.Values.CONTENT_TYPE;
        
        return httpContext.Response.WriteAsync(JsonSerializer.Serialize(_transfer,
            new JsonSerializerOptions{Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin)}));
    }
}