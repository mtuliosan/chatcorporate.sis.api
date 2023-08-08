using IdentityService.Domain;
using IdentityService.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityService.Service
{
    public interface IAudienceService
    {
        Task<Audience> GetAsync(Guid? idAudience);
        Task<string> GetKeyAsync(Guid? idAudience);
        public Task<IEnumerable<Audience>> ListAsync();
        Task<Guid> AddAsync(Audience audience);
        Task UpdateAsync(Audience audience, Guid id);
    }
    public class AudienceService : IAudienceService
    {
        private readonly IAudienceRepository repository;

        public AudienceService(IAudienceRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<Audience>> ListAsync() => await repository.ListAsync();
        public async Task<Audience> GetAsync(Guid? idAudience) => await repository.GetAsync(idAudience);
        public async Task<string> GetKeyAsync(Guid? idAudience) => await repository.GetKeyAsync(idAudience);

        public async Task<Guid> AddAsync(Audience audience)
        {
            audience.Id = Guid.NewGuid();
            await repository.AddAsync(audience);

            return audience.Id.Value;
        }

        public async Task UpdateAsync(Audience audience, Guid id) => await repository.UpdateAsync(audience, id);
    }
}
