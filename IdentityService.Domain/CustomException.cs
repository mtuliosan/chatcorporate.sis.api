using System;
using System.Net;

namespace IdentityService.Domain
{
    public class CustomException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public CustomException(string message, HttpStatusCode statusCode) : base(message) => StatusCode = statusCode;
    }
}
