using System;

namespace IdentityService.Domain
{
    public class Role
    {
        public Role(string value, string name)
        {
            Value = value;
            Name = name;
        }

        public Role(Guid? id, string value, string name)
        {
            Id = id;
            Value = value;
            Name = name;
        }
        public Role() { }

        public Guid? Id { get; set; }
        public string Value { get; set; }
        public string Name { get; set; }
    }
}
