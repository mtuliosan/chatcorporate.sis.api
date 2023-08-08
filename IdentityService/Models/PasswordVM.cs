using System.ComponentModel.DataAnnotations;

namespace IdentityService.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class PasswordVM
    {
        /// <summary>
        /// 
        /// </summary>
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "Informe a senha atual")]
        public string CurrentPassword { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string NewPassword { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Compare("NewPassword", ErrorMessage = "A senha é diferente da repetição")]
        public string RepeatNewPassword { get; set; }
    }
}
