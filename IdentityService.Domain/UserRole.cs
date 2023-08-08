using System;

namespace IdentityService.Domain
{
    public class UserRole
    {
        public Guid Id { get; set; }
        public Guid IdRole { get; set; }
        public Guid IdUser { get; set; }
        public Guid IdAudience { get; set; }
        public string Value { get; set; }
        public string Name { get; set; }
    }
}
