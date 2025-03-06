using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Lab.DataAccess.Data;
using Lab.DataAccess.Repository.IRepository;
using Lab.Models;
using Lab.Models.DTOs.Role;
using Lab.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Lab.DataAccess.Repository
{
    public class RoleRepository : Repository<IdentityRole>, IRoleRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<NguoiDungUngDung> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleRepository(ApplicationDbContext db, UserManager<NguoiDungUngDung> userManager, RoleManager<IdentityRole> roleManager) 
            : base(db)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // Phương thức AssignRole
        public async Task<ResponseAPI<dynamic>> AssignRole(RoleAssignDTO roleAssignDto)
        {
            var response = new ResponseAPI<dynamic>();

            try
            {
                var user = await _userManager.FindByIdAsync(roleAssignDto.UserId) ?? throw new Exception("Không tìm thấy người dùng.");
                var role = await _roleManager.FindByIdAsync(roleAssignDto.RoleId) ?? throw new Exception("Không tìm thấy vai trò.");
                var result = await _userManager.AddToRoleAsync(user, role.Name!);

                if (result.Succeeded)
                {
                    response.Success = true;
                    response.Message = "Gán vai trò thành công.";
                }
                else
                {
                    var error = result.Errors.FirstOrDefault()?.Description;
                    throw new Exception(error ?? "Đã có lỗi xảy ra khi gán vai trò.");
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }

        // Phương thức CreateRole
        public async Task<ResponseAPI<dynamic>> CreateRole(CreateRoleDTO createRoleDto)
        {
            var response = new ResponseAPI<dynamic>();

            try
            {
                if (string.IsNullOrEmpty(createRoleDto.RoleName))
                {
                    throw new Exception("Tên vai trò không được để trống.");
                }

                var roleExist = await _roleManager.RoleExistsAsync(createRoleDto.RoleName);
                if (roleExist)
                {
                    throw new Exception("Vai trò đã tồn tại.");
                }

                var roleResult = await _roleManager.CreateAsync(new IdentityRole(createRoleDto.RoleName));
                if (roleResult.Succeeded)
                {
                    response.Success = true;
                    response.Message = "Tạo vai trò thành công.";
                }
                else
                {
                    var error = roleResult.Errors.FirstOrDefault()?.Description;
                    throw new Exception(error ?? "Đã có lỗi xảy ra khi tạo vai trò.");
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }

        // Phương thức DeleteRole
        public async Task<ResponseAPI<dynamic>> DeleteRole(string id)
        {
            var response = new ResponseAPI<dynamic>();

            try
            {
                var role = await _roleManager.FindByIdAsync(id);
                if (role == null)
                {
                    throw new Exception("Không tìm thấy vai trò.");
                }

                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    response.Success = true;
                    response.Message = "Xóa vai trò thành công.";
                }
                else
                {
                    var error = result.Errors.FirstOrDefault()?.Description;
                    throw new Exception(error ?? "Đã có lỗi xảy ra khi xóa vai trò.");
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }

        // Phương thức GetRoles
        public async Task<ResponseAPI<List<RoleVM>>> GetRoles()
        {
            var response = new ResponseAPI<List<RoleVM>>();

            try
            {
                var roles = await _roleManager.Roles.ToListAsync(); // Lấy danh sách roles trước

                var roleVMs = new List<RoleVM>();
                foreach (var role in roles)
                {
                    var totalUsers = (await _userManager.GetUsersInRoleAsync(role.Name!)).Count; // Xử lý từng vai trò
                    roleVMs.Add(new RoleVM
                    {
                        Id = role.Id,
                        Name = role.Name,
                        TotalUsers = totalUsers
                    });
                }
                
                response.Data = roleVMs;
                response.Success = true;

                response.Message = "Lấy danh sách vai trò thành công.";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
