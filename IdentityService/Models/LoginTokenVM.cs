using Microsoft.Extensions.Caching.Distributed;
using IdentityService.Util;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IdentityService.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class LoginTokenVM
    {
        public string accessToken { get; set; }
    }
}