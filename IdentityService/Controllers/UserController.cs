using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using IdentityService.Core;
using IdentityService.Domain;
using IdentityService.Domain.Config;
using IdentityService.Models;
using IdentityService.Service;
using IdentityService.Util;
using System;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Controllers
{
    /// <summary>
    ///
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly ISessionService sessionService;
        private readonly IAudienceService audienceService;
        private readonly ITokenService tokenService;
        private readonly IMailerService mailerService;
        private IDistributedCache _cache;

        /// <summary>
        ///
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="audienceService"></param>
        /// <param name="tokenService"></param>
        /// <param name="mailerService"></param>
        /// <param name="configuration"></param>
        public UserController(IUserService userService, IAudienceService audienceService, ITokenService tokenService, ISessionService sessionService,
                                IMailerService mailerService, IDistributedCache cache)
        {
            this.userService = userService;
            this.audienceService = audienceService;
            this.tokenService = tokenService;
            this.mailerService = mailerService;
            _cache = cache;
            this.sessionService = sessionService;
        }

        /// <summary>
        /// Registra um novo usuário no sistema
        /// </summary>
        /// <param name="userVM">Dados do novo usuário</param>
        /// <returns></returns>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserVM userVM)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { messages = ModelState.ErroMessages() });

            var user = new User
            {
                Email = userVM.Email,
                Name = userVM.Name,
                Password = userVM.Password,
                RoleName = userVM.roleName,
                RoleId = userVM.roleId,
                Audience = new Audience() { Id = userVM.Audience?.Id }
            };

            Guid? idUser = await userService.AddAsync(user);

            return Ok();
        }

        [HttpGet("me")]
        public async Task<ActionResult<User>> GetDatailUserLogged()
        {
            var userIdLogged = User.FindFirst("UId")?.Value;
            User? regUser = null;
            if (userIdLogged != null) 
                regUser = await userService.GetDetailsUser(userIdLogged);

            if (regUser != null)
            {
                return Ok(regUser);
            }

            return NotFound(new { messages = new string[] { "Usuário não encontrado." } });

        }


        /// <summary>
        /// Retorna um JWT para a aplicação solicitada.
        /// </summary>
        /// <param name="loginVM">Informe o usuário, a senha e a audiência (aplicação) que deseja acessar</param>
        /// <returns>JWT com as roles para o usuário</returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { messages = ModelState.ErroMessages() });

                var user = new User(loginVM.Email, loginVM.Password, new Audience(loginVM.Audience.Id));
                var cachedToken = await _cache.GetAsync(loginVM.CacheKey);
                //if (cachedToken is not null)
                   // return Ok(new { token = Encoding.UTF8.GetString(cachedToken) });

                var regUser = await userService.LoginAsync(user);

                if (regUser != null)
                {
                    var session = await sessionService.GetActiveSessionAsync();

                    regUser.Audience.Key = await audienceService.GetKeyAsync(regUser.Audience.Id);
                    var token = tokenService.Create(regUser, session, 4);

                    _cache.Set(loginVM.CacheKey, Encoding.UTF8.GetBytes(token), new DistributedCacheEntryOptions
                    {
                        AbsoluteExpiration = DateTime.Now.AddHours(4)
                    });
                    return Ok(new { token });
                }
                return BadRequest(new { messages = new string[] { "Usuário ou senha inválidos" } });
            }
            catch (Exception ex)
            {
                return BadRequest(new { messages = new string[] { ex.Message } });
            }
        }

        /// <summary>
        /// Altera senha do usuário
        /// </summary>
        /// <param name="passwordVM"></param>
        /// <returns></returns>
        [HttpPost("changepassword")]
        public async Task<IActionResult> ChangePassword(PasswordVM passwordVM)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { messages = ModelState.ErroMessages() });

            var password = passwordVM.Map();

            await userService.ChangePasswordAsync(password);

            return Ok();
        }

        /// <summary>
        /// Reset da senha baseado em um reset ticket enviado por email.
        /// </summary>
        /// <param name="resetPasswordVM">Nova senha e repetição</param>
        /// <param name="resetTicket">Reset ticket recebido por email</param>
        /// <returns></returns>
        [HttpPost("resetpassword/{resetTicket:Guid}")]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordVM resetPasswordVM, Guid resetTicket)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { messages = ModelState.ErroMessages() });

            var password = new Password { NewPassword = resetPasswordVM.NewPassword };
            await userService.ResetPasswordAsync(password, resetTicket);

            return Ok();
        }

        /// <summary>
        /// Reset da senha.
        /// </summary>
        /// <param name="resetPasswordVM">Nova senha e repetição</param>
        /// <returns></returns>
        [HttpPost("resetpassword")]
        public async Task<IActionResult> ResetPasswordWithoutTicketAsync([FromBody] ResetPasswordVM resetPasswordVM)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { messages = ModelState.ErroMessages() });

            var password = new Password { NewPassword = resetPasswordVM.NewPassword };
            password.Email = resetPasswordVM.Email;
            await userService.ResetPasswordWithoutTicketAsync(password);

            return Ok();
        }

        /// <summary>
        /// Lista todos os usuários registrados
        /// </summary>
        /// <returns>Lista de usuários</returns>
        //[Authorize("admin")]
        [HttpGet("list")]
        public async Task<IActionResult> ListAsync() => Ok(await userService.ListAsync());


        
        /// <summary>
        /// Atualiza os dados do usuário
        /// </summary>
        /// <param name="userEditVM">Dados do usuário</param>
        /// <param name="userId">Id do usuário</param>
        /// <returns></returns>
        //[Authorize("admin")]
        [HttpPut("update/{userId:Guid}")]
        public async Task<IActionResult> Update(UserEditVM userEditVM, Guid? userId)
        {
            await userService.UpdateAsync(userEditVM.Map());
            return Ok();
        }


    }
}
