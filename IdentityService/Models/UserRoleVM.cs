using System;

namespace IdentityService.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class UserRoleVM
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid IdRole { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid IdUser { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid IdAudience { get; set; }
    }
}
