using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using IdentityService.Domain;
using IdentityService.Domain.Config;
using IdentityService.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using IdentityService.Util;

namespace IdentityService.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository repository;
        private readonly IAudienceRepository audienceRepository;

        public UserService(IUserRepository repository, IAudienceRepository audienceRepository)
        {
            this.repository = repository;
            this.audienceRepository = audienceRepository;
        }

        public async Task<User> GetDetailsUser(string userId) {
            return  await repository.GetUserByUserIdAsync(userId);
        }

        public async Task<Guid?> AddAsync(User user)
        {
            if (await repository.ExistsAsync(user.Email))
                throw new CustomException("Usuário já existe", HttpStatusCode.BadRequest);

            var hasher = user.Password.Hash(default(byte[]));

            user.Id = Guid.NewGuid();
            user.Password = hasher.Hash;

            await repository.AddAsync(user, hasher.Salt);

            if (!string.IsNullOrEmpty(user.RoleId) && !string.IsNullOrEmpty(user.RoleName))
                await repository.AddRole(Guid.Parse(user.RoleId), (Guid)user.Id, (Guid)user.Audience.Id, user.RoleName);

            return user.Id;
        }

        public async Task<User> LoginAsync(User user)
        {
            var registeredUser = await repository.GetUserByEmailAsync(user.Email);

            if (registeredUser != null)
            {
                var hash = user.Password.Hash(registeredUser.Salt);
                var isLoggedIn = hash.Hash == registeredUser.Password;

                if (isLoggedIn)
                {
                    registeredUser.ClearPassword();
                    registeredUser.Audience = user.Audience;
                    registeredUser.Audience.Key = await audienceRepository.GetKeyAsync(user.Audience.Id);
                    registeredUser.Audience.Roles.AddRange(await repository.GetRolesAsync(registeredUser.Id, registeredUser.Audience.Id));

                    if (!registeredUser.Audience.Roles.Any())
                        throw new RoleNotFoundException("Usuário não possui roles.", HttpStatusCode.BadRequest);

                    return registeredUser;
                }
            }

            throw new CustomException("Usuário ou senha inválidos.", HttpStatusCode.NotFound);
        }

        public async Task ChangePasswordAsync(Password password)
        {
            var registeredUser = await repository.GetUserByEmailAsync(password.Email);

            if (registeredUser != null)
            {
                var hasher = password.CurrentPassword.Hash(registeredUser.Salt);

                if (hasher.Hash == registeredUser.Password)
                {
                    var newPassHasher = password.NewPassword.Hash(default(byte[]));
                    password.NewPassword = newPassHasher.Hash;
                    password.Salt = newPassHasher.Salt;
                    password.CurrentPassword = hasher.Hash;

                    await repository.ChangePasswordAsync(password);

                    return;
                }
            }

            throw new CustomException("Usuário ou senha inválidos.", HttpStatusCode.NotFound);
        }

        public async Task<bool> ResetPasswordAsync(Password password, Guid resetTicket)
        {
            await ValidateResetTicketAsync(resetTicket);

            var hasher = password.NewPassword.Hash(default(byte[]));
            password.NewPassword = hasher.Hash;
            password.Salt = hasher.Salt;

            return await repository.ResetPasswordAsync(password, resetTicket);
        }

        public async Task<bool> ResetPasswordAsync(Password password)
        {
            var hasher = password.NewPassword.Hash(default(byte[]));
            password.NewPassword = hasher.Hash;
            password.Salt = hasher.Salt;

            return await repository.ResetPasswordAsync(password);
        }

        public async Task<(string UserName, Guid ResetTicket, DateTime ResetTicketExp)> NotifyResetPasswordAsync(string email)
        {
            var user = await repository.GetUserByEmailAsync(email);

            if (user == null)
                throw new CustomException("Usuário inválido.", HttpStatusCode.NotFound);

            var resetTicket = Guid.NewGuid();
            var resetTicketExp = DateTime.Now.AddHours(4);

            await repository.NotifyResetPasswordAsync(email, resetTicket, resetTicketExp);

            return (user.Name, resetTicket, resetTicketExp);
        }

        public async Task<IEnumerable<UserEdit>> ListAsync()
        {
            return await repository.ListAsync();
        }

        public async Task UpdateAsync(UserEdit userEdit)
        {
            if (!(userEdit != null && userEdit.Id.HasValue))
                return;

            await repository.UpdateAsync(userEdit);
        }

        public async Task ValidateResetTicketAsync(Guid resetTicket)
        {
            var ticket = (await repository.GetResetTicketAsync(resetTicket)) ?? throw new CustomException("Chave não encontrada", HttpStatusCode.BadRequest);

            if (ticket?.ExpireDate < DateTime.Now)
            {
                await repository.RemoveResetTicketAsync(resetTicket);
                throw new CustomException("Chave expirada", HttpStatusCode.BadRequest);
            }
        }

        public async Task<bool> ResetPasswordWithoutTicketAsync(Password password)
        {
            var hasher = password.NewPassword.Hash(default(byte[]));
            password.NewPassword = hasher.Hash;
            password.Salt = hasher.Salt;

            return await repository.ResetPasswordAsync(password);
        }

        public async Task<User> LoginWithKeyAsync(string key, string secret)
        {
            var email = $"{key}@chatcorporate-intenal-api.com.br";
            var user = new User(email, secret, new Audience(Guid.Parse(key)));
            var registeredUser = await repository.GetUserByEmailAsync(email);

            if (registeredUser != null)
            {
                var hash = user.Password.Hash(registeredUser.Salt);
                var isLoggedIn = hash.Hash == registeredUser.Password;

                if (isLoggedIn)
                {
                    registeredUser.ClearPassword();
                    registeredUser.Audience = user.Audience;
                    registeredUser.Audience.Roles.AddRange(await repository.GetRolesAsync(registeredUser.Id, registeredUser.Audience.Id));

                    if (!registeredUser.Audience.Roles.Any())
                        throw new RoleNotFoundException("Usuário não possui roles.", HttpStatusCode.BadRequest);

                    return registeredUser;
                }
            }

            throw new CustomException("invalid key or secret", HttpStatusCode.NotFound);
        }

       
    }
}
