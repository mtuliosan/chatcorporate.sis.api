using System;
using System.Collections.Generic;

namespace IdentityService.Domain
{
    public class Audience
    {
        string key;

        public Audience() { }

        public Audience(Guid? id)
        {
            Id = id;
        }

        public Audience(string name, string key)
        {
            Name = name;
            Key = key;
        }

        public Audience(Guid? id, string name, string key)
        {
            Id = id;
            Name = name;
            Key = key;
        }

        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Key { get => key; set => key = value?.Trim(); }
        public List<UserRole> Roles { get; set; } = new List<UserRole>();
    }
}
