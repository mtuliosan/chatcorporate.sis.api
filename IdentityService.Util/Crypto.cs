using System.Security.Cryptography;
using System.Text;

namespace IdentityService.Util
{
    public static class Crypto
    {
        public static string ComputerSha256Hash(string rawData)
        {
            using SHA256 sha256Hasj = SHA256.Create();
            var bytes = sha256Hasj.ComputeHash(Encoding.UTF8.GetBytes(rawData));

            StringBuilder builder   = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++) { builder.Append(bytes[i].ToString("x2")); }

            return builder.ToString();
        }

    }
}