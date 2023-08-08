using System;
using System.ComponentModel.DataAnnotations;

namespace IdentityService.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class RoleVM
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid? Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "Informe o valor da role")]
        public string Value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "Informe o nome da role")]
        public string Name { get; set; }
    }
}
