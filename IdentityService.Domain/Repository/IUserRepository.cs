using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace IdentityService.Domain.Repository
{
    public interface IUserRepository
    {
        Task AddAsync(User user, string salt);
        Task ChangePasswordAsync(Password password);
        Task<User> GetUserByEmailAsync(string email);
        Task<IEnumerable<UserRole>> GetRolesAsync(Guid? userId, Guid? audienceId);
        Task NotifyResetPasswordAsync(string email, Guid resetTicket, DateTime resetTicketExp);
        Task<bool> ResetPasswordAsync(Password password, Guid? resetTicket);
        Task<bool> ResetPasswordAsync(Password password);
        Task<bool> ExistsAsync(string email);
        Task<IEnumerable<UserEdit>> ListAsync();
        Task UpdateAsync(UserEdit userEdit);
        Task<ResetTicket> GetResetTicketAsync(Guid? resetTicket);
        Task RemoveResetTicketAsync(Guid? resetTicket);
    }
}
