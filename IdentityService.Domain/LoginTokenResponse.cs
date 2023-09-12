using System;
using System.Collections.Generic;
using IdentityService.Util;

namespace IdentityService.Domain
{
    public class LoginToken 
    {
        public User user{ get; set; }
        public string accessToken {get;set;}
        public string tokenType {get;set;}
    }
}