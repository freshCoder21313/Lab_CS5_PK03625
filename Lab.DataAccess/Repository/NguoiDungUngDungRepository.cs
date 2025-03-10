﻿using Lab.DataAccess.Data;
using Lab.DataAccess.Repository.IRepository;
using Lab.Models;
using Lab.Models.DTOs.NguoiDungUngDung;
using Lab.Models.DTOs.NhanVien;
using Lab.Models.ViewModels;
using Lab.Utility.SharedData;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Lab.DataAccess.Repository
{
    public class NguoiDungUngDungRepository : Repository<NguoiDungUngDung>, INguoiDungUngDungRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly PasswordHasher<NguoiDungUngDung> _passwordHasher;
        private readonly IJWTRepository _jwt;

        private readonly UserManager<NguoiDungUngDung> _userManager;
        public NguoiDungUngDungRepository(ApplicationDbContext db, IJWTRepository jwt, UserManager<NguoiDungUngDung> userManager) : base(db)
        {
            _db = db;
            _passwordHasher = new PasswordHasher<NguoiDungUngDung>();
            _jwt = jwt;
            _userManager = userManager;
        }

        public async Task<ResponseAPI<dynamic>> AddAsyncByDTO(NhanVienDTO objDTO)
        {
            NguoiDungUngDung NguoiDungUngDung = new NguoiDungUngDung
            {
                HoTen = objDTO.HoTen,
                SoDienThoai = objDTO.SoDienThoai,
                NgaySinh = objDTO.NgaySinh,
                VaiTro = objDTO.VaiTro ?? ConstantsValue.RoleCustomer
            };

            if (objDTO.TenDangNhap == NguoiDungUngDung.UserName && (await _db.Users.AnyAsync(nv => nv.UserName == objDTO.TenDangNhap)))
            {
                return new ResponseAPI<dynamic>
                {
                    Status = 200,
                    Success = false,
                    Message = "Tên đăng nhập đã có trong hệ thống"
                };
            }

            _db.Users.Add(NguoiDungUngDung);

            await _db.SaveChangesAsync();

            return new ResponseAPI<dynamic>
            {
                Status = 200,
                Success = true,
                Message = $"Đã thêm dữ liệu nhân viên mang mã: {NguoiDungUngDung.Id}" // Assuming Id is generated by the DB
            };
        }
        public async Task<ResponseAPI<dynamic>> DangKyAsync(DangKy dangKyDTO)
        {
            if (await _db.Users.AnyAsync(u => u.UserName == dangKyDTO.UserName || u.Email == dangKyDTO.Email))
            {
                return new ResponseAPI<dynamic>
                {
                    Status = 400,
                    Success = false,
                    Message = "Tên đăng nhập hoặc email đã tồn tại trong hệ thống."
                };
            }

            var newUser = new NguoiDungUngDung
            {
                UserName = dangKyDTO.UserName,
                Email = dangKyDTO.Email,
                HoTen = dangKyDTO.HoTen,
                SoDienThoai = dangKyDTO.SoDienThoai,
                NgaySinh = dangKyDTO.NgaySinh,
                GioiTinh = dangKyDTO.GioiTinh,
                DiaChi = dangKyDTO.DiaChi,
                LinkAnh = dangKyDTO.LinkAnh ?? "https://example.com/default-avatar.png"
            };

            var result = await _userManager.CreateAsync(newUser, dangKyDTO.Password);

            if (!result.Succeeded)
            {
                return new ResponseAPI<dynamic>
                {
                    Status = 500,
                    Success = false,
                    Message = "Đăng ký thất bại",
                    Data = result.Errors.Select(e => e.Description)
                };
            }

            await _userManager.AddToRoleAsync(newUser, ConstantsValue.RoleCustomer);

            return new ResponseAPI<dynamic>
            {
                Status = 201,
                Success = true,
                Message = "Đăng ký thành công",
                Data = newUser.Id
            };
        }

        public async Task Update(NhanVienDTO objDTO)
        {
            NguoiDungUngDung? infoGoc = await _db.Users.FirstOrDefaultAsync(x => x.Id == objDTO.Id.ToString()); // Assuming Id is string and maps to Id
            if (infoGoc == null)
            {
                return;
            }
            infoGoc.HoTen = objDTO.HoTen;
            infoGoc.NgaySinh = objDTO.NgaySinh;
            infoGoc.SoDienThoai = objDTO.SoDienThoai;
            infoGoc.UserName = objDTO.TenDangNhap;

            _db.Users.Update(infoGoc);

            await _db.SaveChangesAsync();
        }

        public async Task<ResponseAPI<TokenVM>> RegisterAsync(RegisterDto request)
        {
            ResponseAPI<TokenVM> response = new ResponseAPI<TokenVM>
            {
                Status = 400,
                Success = false
            };
            try
            {
                if (await _db.Users.AnyAsync(u => u.Email == request.Email))
                {
                    throw new Exception("Tài khoản đã tồn tại.");
                }

                var user = new NguoiDungUngDung
                {
                    Email = request.Email,
                    HoTen = request.FullName,
                    UserName = request.Email,  // Or any other logic for username
                    VaiTro = ConstantsValue.RoleCustomer // Default Role
                };

                user.PasswordHash = _passwordHasher.HashPassword(user, request.Password!); // Hash the password

                _db.Users.Add(user);
                await _db.SaveChangesAsync();

                // Assign default role
                // Assuming SD.RoleUser exists
                response =  new ResponseAPI<TokenVM>
                {
                    Status = 200,
                    Success = true,
                    Message = "Đã đăng kí tài khoản thành công!"
                };
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                throw;
            }
            return response;
        }

        public async Task<ResponseAPI<TokenVM>> LoginAsync(LoginDto login)
        {
            ResponseAPI<TokenVM> response = new ResponseAPI<TokenVM>
            {
                Status = 401,
                Success = false,
                Message = "Mật khẩu hoặc email không hợp lệ."
            };

            try
            {
                var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == login.Email);

                if (user == null)
                {
                    throw new Exception("Không tìm thấy tài khoản người dùng");
                }

                var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash!, login.Password!);

                if (passwordVerificationResult == PasswordVerificationResult.Failed)
                {
                    throw new Exception("Xác nhận mật khẩu thất bại.");
                }

                // Generate Token
                var token = _jwt.GenerateToken(user);

                response = new ResponseAPI<TokenVM>
                {
                    Status = 200,
                    Success = true,
                    Message = "Đăng nhập thành công.",
                    Data = token
                };
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                throw;
            }
            return response;
        }

        public async Task<NguoiDungUngDung?> GetUserByIdAsync(string userId)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<List<UserDetailDto>> GetUsersAsync()
        {
            return await _db.Users.Select(u => new UserDetailDto
            {
                Id = u.Id,
                Email = u.Email,
                FullName = u.HoTen,
                //Roles = _userManager.GetRolesAsync(u).Result.ToArray(), // Removed usermanager depedency 
                PhoneNumber = u.PhoneNumber,
                PhoneNumberConfirmed = u.PhoneNumberConfirmed,
                AccessFailedCount = u.AccessFailedCount
            }).ToListAsync();
        }
    }
}
