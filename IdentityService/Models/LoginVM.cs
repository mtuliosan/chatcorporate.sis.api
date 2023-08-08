using Microsoft.Extensions.Caching.Distributed;
using IdentityService.Util;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IdentityService.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class LoginVM
    {

        /// <summary>
        /// 
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "Audiência inválida")]
        public LoginAudienceVM Audience { get; set; }


        public string CacheKey 
        { 
            get => Crypto.ComputerSha256Hash(Email + Password); 
        }
    }
}
