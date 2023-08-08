using System.Net;

namespace IdentityService.Domain
{
    public class RoleNotFoundException : CustomException
    {
        public RoleNotFoundException(string message, HttpStatusCode statusCode) : base(message, statusCode) { }
    }
}
