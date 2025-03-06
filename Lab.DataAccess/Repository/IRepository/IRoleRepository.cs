using Lab.Models;
using Lab.Models.DTOs.Role;
using Lab.Models.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Lab.DataAccess.Repository.IRepository
{
    public interface IRoleRepository : IRepository<IdentityRole>
    {
        Task<ResponseAPI<dynamic>> CreateRole(CreateRoleDTO createRoleDto);
        Task<ResponseAPI<List<RoleVM>>> GetRoles();
        Task<ResponseAPI<dynamic>> DeleteRole(string id);
        Task<ResponseAPI<dynamic>> AssignRole(RoleAssignDTO roleAssignDto);
    }
}