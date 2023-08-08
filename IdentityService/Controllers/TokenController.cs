using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using IdentityService.App.Models;
using IdentityService.Service;
using IdentityService.Util;
using System;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly ISessionService _sessionService;
        private readonly IAudienceService audienceService;
        private readonly ITokenService tokenService;
        private IDistributedCache _cache;

        public TokenController(IUserService userService, IAudienceService audienceService, ITokenService tokenService, IDistributedCache cache, ISessionService sessionService)
        {
            this.userService = userService;
            this.audienceService = audienceService;
            this.tokenService = tokenService;
            _cache = cache;
            this._sessionService = sessionService;

        }

        /// <summary>
        /// Retorna um JWT para a aplicação solicitada.
        /// </summary>
        /// <param name="loginVM">Informe o usuário, a senha e a audiência (aplicação) que deseja acessar</param>
        /// <returns>JWT com as roles para o usuário</returns>
        [HttpPost]
        public  async Task<IActionResult> Login(GenerateTokenVM loginVM)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { messages = ModelState.ErroMessages() });

            var regUser = await  userService.LoginWithKeyAsync(loginVM.key, loginVM.secret);

            if (regUser != null)
            {
                var session = await _sessionService.GetActiveSessionAsync();

                regUser.Audience.Key = audienceService.GetKeyAsync(regUser.Audience.Id).Result;
                var newToken = tokenService.Create(regUser, session, 4);

                _cache.Set(loginVM.CacheKey, Encoding.UTF8.GetBytes(newToken), new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddHours(4)
                });
                return Ok(new
                {
                    token = newToken
                });
            }

            return BadRequest(new { messages = new string[] { "Usuário ou senha inválidos" } });
        }
    }
}