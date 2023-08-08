using System;
using System.ComponentModel.DataAnnotations;

namespace IdentityService.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class LoginAudienceVM
    {
        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "Audiência inválida")]
        public Guid? Id { get; set; }
    }
}
