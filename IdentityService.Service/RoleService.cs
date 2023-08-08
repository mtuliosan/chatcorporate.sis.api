using IdentityService.Domain;
using IdentityService.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityService.Service
{
    public interface IRoleService
    {
        Task<IEnumerable<Role>> ListAsync();
        Task<Guid> AddAsync(Role role);
        Task UpdateAsync(Role role, Guid id);
    }

    public class RoleService : IRoleService
    {
        private readonly IRoleRepository repository;

        public RoleService(IRoleRepository repository) => this.repository = repository;

        public async Task<IEnumerable<Role>> ListAsync() => await repository.ListAsync();

        public async Task<Guid> AddAsync(Role role)
        {
            role.Id = Guid.NewGuid();
            await repository.AddAsync(role);

            return role.Id.Value;
        }

        public async Task UpdateAsync(Role role, Guid id) => await repository.UpdateAsync(role, id);
    }
}