using IdentityService.Domain;
using System.Linq;

namespace IdentityService.Models
{
    /// <summary>
    /// 
    /// </summary>
    public static class UserVMExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userVM"></param>
        /// <returns></returns>
        public static User Map(this UserVM userVM) => new User
        {
            Audience = new Audience { Id = userVM?.Audience.Id },
            Email = userVM?.Email,
            Id = userVM?.Id,
            Name = userVM?.Name,
            Password = userVM?.Password
        };
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public static UserEdit Map(this UserEditVM vm)
        {
            return new UserEdit
            {
                Id = vm?.Id,
                Email = vm?.Email,
                Name = vm?.Name,
                Audiences = vm?.Audiences.Select(x =>
                    new Audience
                    {
                        Id = x.Id,
                        Roles = x.Roles.Select(r =>
                        new UserRole
                        {
                            IdRole = r.IdRole,
                            IdAudience = r.IdAudience,
                            IdUser = r.IdUser
                        }).ToList()
                    }
                ).ToList()
            };
        }
    }
}
