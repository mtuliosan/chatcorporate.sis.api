using System.ComponentModel.DataAnnotations;

namespace IdentityService.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ResetPasswordVM
    {
        /// <summary>
        /// 
        /// </summary>
        public string Email { get; set; }

        public string NewPassword { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Compare("NewPassword", ErrorMessage = "A senha é diferente da repetição")]
        public string RepeatNewPassword { get; set; }
    }
}
