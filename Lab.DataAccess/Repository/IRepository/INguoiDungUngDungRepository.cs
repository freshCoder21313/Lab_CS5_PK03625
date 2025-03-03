using Lab.Models;
using Lab.Models.DTOs.NguoiDungUngDung;
using Lab.Models.DTOs.NhanVien;
using Lab.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab.DataAccess.Repository.IRepository
{
    public interface INguoiDungUngDungRepository : IRepository<NguoiDungUngDung>
    {
        Task<ResponseAPI<dynamic>> AddAsyncByDTO(NhanVienDTO objDTO);
        Task Update(NhanVienDTO objDTO);
        Task<ResponseAPI<TokenVM>> RegisterAsync(RegisterDto request);
        Task<ResponseAPI<TokenVM>> LoginAsync(LoginDto login);
        Task<NguoiDungUngDung?> GetUserByIdAsync(string userId);
        Task<List<UserDetailDto>> GetUsersAsync();
    }
}
