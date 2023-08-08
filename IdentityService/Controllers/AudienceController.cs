using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IdentityService.Models;
using IdentityService.Service;
using IdentityService.Util;
using IdentityService.Domain;

namespace IdentityService.Controllers
{
    /// <summary>
    ///
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrator")]
    public class AudienceController : ControllerBase
    {
        private readonly IAudienceService audienceService;
        /// <summary>
        ///
        /// </summary>
        /// <param name="audienceService"></param>
        public AudienceController(IAudienceService audienceService) => this.audienceService = audienceService;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<IActionResult> ListAsync() => Ok(await audienceService.ListAsync());

        /// <summary>
        ///
        /// </summary>
        /// <param name="audienceVM"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddAsync(AudienceVM audienceVM)
        {
            if (!ModelState.IsValid)
            {
                var messages = ModelState.ErroMessages();
                return BadRequest(new { messages });
            }

            var audience = new Audience(audienceVM.Key, audienceVM.Name);
            var id = await audienceService.AddAsync(audience);

            return Ok(new { id });
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="audienceVM"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> UpdateAsync(AudienceVM audienceVM, Guid? id)
        {
            if (!ModelState.IsValid || !id.HasValue)
            {
                var messages = ModelState.ErroMessages() ?? new List<string>();
                messages.Add("Informe o id da aplicação");

                return BadRequest(new { messages });
            }

            var audience = new Audience { Id = id, Key = audienceVM.Key, Name = audienceVM.Name };
            await audienceService.UpdateAsync(audience, id.Value);
            return Ok();
        }

    }
}
