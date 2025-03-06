using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab.DataAccess.Repository.IRepository;
using Lab.Models.DTOs.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lab.API.Areas.Manager
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class RoleController : ControllerBase
    {
        private readonly IUnitOfWork _unit;
        public RoleController(IUnitOfWork unitOfWork) {
            _unit = unitOfWork;
        } 
        
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleDTO createRoleDto) {
            return Ok(await _unit.Roles.CreateRole(createRoleDto));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetRoles() {
            return Ok(await _unit.Roles.GetRoles());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(string id) {
            return Ok(await _unit.Roles.DeleteRole(id));
        }
        [HttpPost]
        public async Task<IActionResult> AssignRole([FromBody] RoleAssignDTO roleAssignDto) {
            return Ok(await _unit.Roles.AssignRole(roleAssignDto));
        }
    }
}