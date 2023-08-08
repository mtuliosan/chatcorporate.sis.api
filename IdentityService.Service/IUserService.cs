using IdentityService.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityService.Service
{
    public interface IUserService
    {
        Task<Guid?> AddAsync(User user);
        Task ChangePasswordAsync(Password password);
        Task<User> LoginAsync(User user);
        Task<bool> ResetPasswordAsync(Password password, Guid resetTicket);
        Task<bool> ResetPasswordWithoutTicketAsync(Password password);
        Task<User> GetDetailsUser(string userId);
        Task<(string UserName, Guid ResetTicket, DateTime ResetTicketExp)> NotifyResetPasswordAsync(string email);
        Task<IEnumerable<UserEdit>> ListAsync();
        Task UpdateAsync(UserEdit userEdit);
        Task ValidateResetTicketAsync(Guid resetTicket);
        Task<User> LoginWithKeyAsync(string key, string secret);
    }
}
