using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IdentityService.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class UserVM
    {
        string email, name;
        /// <summary>
        /// 
        /// </summary>
        public Guid? Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get => email; set => email = value?.Trim().ToLower(); }
        /// <summary>
        /// 
        /// </summary>
        //[Password(ErrorMessage = "A senha deve conter no mínimo 6 caracteres, 1 letra maiúscula, 1 letra minúscula, 1 número e 1 caracter especial")]
        public string Password { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Compare("Password", ErrorMessage = "A senha é diferente da repetição")]
        public string Repeat { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "Informe o nome")]
        public string Name { get => name; set => name = value?.Trim(); }
        /// <summary>
        /// 
        /// </summary>
        public LoginAudienceVM Audience { get; set; }
        public string roleId { get; set; }
        public string roleName { get; set; }
    }
}
