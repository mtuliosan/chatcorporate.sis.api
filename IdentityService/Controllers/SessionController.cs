using IdentityService.Domain;
using IdentityService.Domain.Config;
using IdentityService.Models;
using IdentityService.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.Text;

namespace IdentityService.Controllers
{
    /// <summary>
    ///
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SessionController : ControllerBase
    {
        private ISessionService _sessionService;
        private IDistributedCache _cache;
        private string _token;

        public SessionController(ISessionService sessionService, IDistributedCache cache, IOptions<IdentityServerConfigs> identityConfig)
        {
            _sessionService = sessionService;
            _cache = cache;
            _token = identityConfig.Value.Token;
        }

        /// <summary>
        /// Obter Sessao Ativa, caso nao tenha sera criada uma nova sessao.
        /// </summary>
        /// <returns>Dados da Sessão</returns>
        [HttpGet]
        public async Task<ActionResult> GetActiveSession()
        {
            var cachedToken = await _cache.GetAsync(_token);
            if (cachedToken is not null)
            {
                var session = Encoding.UTF8.GetString(cachedToken);
                return Ok(Newtonsoft.Json.JsonConvert.DeserializeObject<AppSession>(session));
            }
            
            var appSession = await _sessionService.GetActiveSessionAsync();
           
            _cache.Set(_token, Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(appSession)), new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddHours(24)
            });

            return Ok(appSession);

        }

    }
}
