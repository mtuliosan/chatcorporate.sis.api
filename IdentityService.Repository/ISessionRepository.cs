using IdentityService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Repository
{
    public interface ISessionRepository
    {
        Task<AppSession> GetOrCreateActiveSession();
    }
}
