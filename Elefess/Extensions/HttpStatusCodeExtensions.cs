using System.Net;

namespace Elefess;

internal static class HttpStatusCodeExtensions
{
    extension(HttpStatusCode)
    {
        public static HttpStatusCode UnprocessableEntity => (HttpStatusCode)422;
    }
}