using System;

namespace IdentityService.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class UserRoleEditVM
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid IdRole { get; set; }
        public Guid IdUser { get; set; }
        public Guid IdAudience { get; set; }
    }
}
