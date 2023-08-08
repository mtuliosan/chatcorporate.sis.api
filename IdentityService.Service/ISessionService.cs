using IdentityService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Service
{
    public interface ISessionService
    {
        Task<AppSession> GetActiveSessionAsync();
    }
}
