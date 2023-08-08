using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IdentityService.Domain;
using IdentityService.Models;
using IdentityService.Service;
using IdentityService.Util;

namespace IdentityService.Controllers
{
    /// <summary>
    ///
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrator")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService roleService;
        /// <summary>
        ///
        /// </summary>
        /// <param name="roleService"></param>
        public RoleController(IRoleService roleService) => this.roleService = roleService;

        /// <summary>
        /// Retorna todas as roles.
        /// </summary>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<IActionResult> ListAsync() => Ok(await roleService.ListAsync());

        /// <summary>
        /// Cria uma nova role
        /// </summary>
        /// <param name="roleVM"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddAsync(RoleVM roleVM)
        {
            if (!ModelState.IsValid)
            {
                var messages = ModelState.ErroMessages();
                return BadRequest(new { messages });
            }

            var role = new Role(roleVM.Value, roleVM.Name);
            var id = await roleService.AddAsync(role);
            return Ok(new { id });
        }

        /// <summary>
        /// Atualiza os dados de uma role
        /// </summary>
        /// <param name="roleVM"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> UpdateAsync(RoleVM roleVM, Guid? id)
        {
            if (!ModelState.IsValid || !id.HasValue)
            {
                var messages = ModelState.ErroMessages() ?? new List<string>();
                messages.Add("Informe o id da role");
                return BadRequest(new { messages });
            }

            var role = new Role(id, roleVM.Value, roleVM.Name);
            await roleService.UpdateAsync(role, id.Value);
            return Ok();
        }
    }
}