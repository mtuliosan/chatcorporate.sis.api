using System.ComponentModel.DataAnnotations;

namespace IdentityService.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class EmailVM
    {
        /// <summary>
        /// 
        /// </summary>
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Compare("Email", ErrorMessage = "O email é diferente da repetição")]
        public string RepeatEmail { get; set; }
    }
}
