using System;
using System.Collections.Generic;

namespace IdentityService.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class AudienceEditVM
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid? Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<UserRoleEditVM> Roles { get; set; }
    }
}
