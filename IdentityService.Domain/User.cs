using System;
using System.Collections.Generic;
using IdentityService.Util;

namespace IdentityService.Domain
{
    public class User
    {

        private string _groupId;
        private string _email;
        public User() { }

        public User(string email, string password, Audience audience)
        {
            Email = email;
            Password = password;
            Audience = audience;
            RoleId = null;
            RoleName = null;
        }

        public User(string email, string password, Audience audience, string salt,
                    string name, string roleId, string roleName) {

            Email = email;
            Password = password;
            Audience = audience;
            Salt = salt;
            Name = name;
            RoleId = roleId;
            RoleName = roleName;
        }

        public Guid? Id { get; set; }
        public string Email 
        { 
            get => _email;
            set => _email = value;
        }
        public string Password { get; set; }
        public Audience Audience { get; set; }
        public string Salt { get; set; }
        public string Name { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }

        public string firstname { get; set; }
        public string lastname { get; set; }
        public string avatar { get; set; }
        public List<Guid> roles { get; set; }
        public string about { get; set; }        
        public string occupation { get; set; }
        public string phone { get; set; }

        public string GroupId
        {
            get => string.IsNullOrEmpty(_groupId) ? "" : _groupId;
            set => _groupId = value;
        }

        public string UserId
        {
            get => _email.Split('@')[0];
        }

        public void ClearPassword()
        {
            Password = null;
            Salt = null;
        }

        public bool CheckPassword(string password)
        {

            var salt = default(byte[]);
            if (!string.IsNullOrWhiteSpace(Salt))
                salt = Convert.FromBase64String(Salt);

            return Extensions.Hash(password, salt).Hash == Password;
        }
    }
}
