using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Domain
{
    public class AppSession
    {
        public Guid Session { get; set; }
        public Guid ApiKey { get; set; }
        public string Token { get; set; }
        public DateTime CreateAt { get; set; }
        public bool IsActived { get; set; }
    }
}
