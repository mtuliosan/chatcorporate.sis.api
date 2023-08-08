using System;
using System.ComponentModel.DataAnnotations;

namespace IdentityService.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class AudienceVM
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid? Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "Informe o nome da aplicaçao")]
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [MinLength(16, ErrorMessage = "A chave deve contar ao menos 16 caracteres")]
        public string Key { get; set; }
    }
}
