using System;
using System.Collections.Generic;

namespace IdentityService.Domain
{
    public class UserEdit
    {
        public Guid? Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public List<Audience> Audiences { get; set; } = new List<Audience>();
    }
}
