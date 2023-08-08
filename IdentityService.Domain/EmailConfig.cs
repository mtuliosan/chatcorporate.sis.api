﻿using System.Collections.Generic;

namespace IdentityService.Domain
{
    public class EmailConfig
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string From { get; set; }
        public string Displayname { get; set; }
        public List<string> To { get; set; }
    }
}
