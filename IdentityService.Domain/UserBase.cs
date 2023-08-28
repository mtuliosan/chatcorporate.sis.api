using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Domain
{
    public class UserBase
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string occupation { get; set; }
        public string avatar { get; set; }

 
    }
}