using IdentityService.Util;

namespace IdentityService.App.Models
{
    public class GenerateTokenVM
    {
        public string key { get; set; }
        public string secret { get; set; }

        public string CacheKey
        {
            get => Crypto.ComputerSha256Hash(key + secret);
        }
    }
}
