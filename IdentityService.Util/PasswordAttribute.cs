using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace IdentityService.Util
{
    public  class PasswordAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null) return false;

            var passworod = value as string;
            bool isValid = passworod.Length >= 5
                           && Regex.IsMatch(passworod, "[A-Z]")
                           && Regex.IsMatch(passworod, "[a-z]")
                           && Regex.IsMatch(passworod, "\\d")
                           && Regex.IsMatch(passworod, "\\W");

            return isValid;
        }
    }
}
