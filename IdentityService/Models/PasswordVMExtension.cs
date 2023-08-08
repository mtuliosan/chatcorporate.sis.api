using IdentityService.Domain;

namespace IdentityService.Models
{
    /// <summary>
    /// 
    /// </summary>
    public static class PasswordVMExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="passwordVM"></param>
        /// <returns></returns>
        public static Password Map(this PasswordVM passwordVM) => new Password
        {
            CurrentPassword = passwordVM?.CurrentPassword,
            Email = passwordVM?.Email,
            NewPassword = passwordVM?.NewPassword
        };
    }
}
