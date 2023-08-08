using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace IdentityService.Util
{
    public static class Extensions
    {
        public static Hasher Hash(this string text, byte[] salt)
        {
            if (salt == null)
            {
                salt = new byte[16];

                using (var rng = RandomNumberGenerator.Create())
                    rng.GetBytes(salt);
            }

            var hasher = new Hasher
            {
                Hash = Convert.ToBase64String(
                KeyDerivation.Pbkdf2(
                    password: text,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 10000,
                    numBytesRequested: 32)
                ),
                Salt = Convert.ToBase64String(salt)
            };

            return hasher;
        }
        public static Hasher Hash(this string text, string base64Salt)
        {
            var salt = default(byte[]);

            if (!string.IsNullOrWhiteSpace(base64Salt))
                salt = Convert.FromBase64String(base64Salt);

            return Hash(text, salt);
        }
        public static bool IsEmail(this string text) => Regex.IsMatch($"{text}", @"[^\s]+@[^\s]+\.[^\s]+");
        public static List<string>? ErroMessages(this ModelStateDictionary model) => model?.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();

        public static bool AnySafe<T>(this IEnumerable<T> enumerable, Func<T, bool>? predicate = null)
        {
            return enumerable != null && (predicate == null ? enumerable.Any() : enumerable.Any(predicate));
        }

        public static IEnumerable<IEnumerable<T>> Lists<T>(this IEnumerable<T> enumerable, int size)
        {
            var list = enumerable.ToList();
            var lists = new List<IEnumerable<T>>();

            for (int i = 0; i < list.Count(); i += size)
                lists.Add(list.GetRange(i, Math.Min(size, list.Count - i)));

            return lists;
        }
    }
}