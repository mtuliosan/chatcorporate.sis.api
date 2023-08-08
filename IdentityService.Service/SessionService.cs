using IdentityService.Domain;
using IdentityService.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Service
{
    public class SessionService : ISessionService
    {
        private readonly ISessionRepository _repository;

        public SessionService(ISessionRepository repository)
        {
            this._repository = repository;
        }

        public async Task<AppSession> GetActiveSessionAsync()
        {
            return await _repository.GetOrCreateActiveSession();
        }
    }
}
