using System;
using System.Collections.Generic;

namespace IdentityService.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class UserEditVM
    {
        string email, name;
        /// <summary>
        /// 
        /// </summary>
        public Guid? Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Email { get => email; set => email = value?.Trim().ToLower(); }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get => name; set => name = value?.Trim(); }
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<AudienceEditVM> Audiences { get; set; }
    }
}
