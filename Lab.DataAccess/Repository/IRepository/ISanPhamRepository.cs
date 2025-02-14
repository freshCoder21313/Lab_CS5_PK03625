using Lab.Models;
using Lab.Models.DTOs.NhanVien;
using Lab.Models.DTOs.SanPham;
using Lab.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Lab.DataAccess.Repository.IRepository
{
    public interface ISanPhamRepository : IRepository<tblSanPham>
    {
        Task<ResponseAPI<SanPhamVM>> GetAsyncVM(Expression<Func<tblSanPham, bool>> filter, string? includeProperties = null, bool tracked = false);
        Task<ResponseAPI<List<SanPhamVM>>> GetAllAsyncVM(Expression<Func<tblSanPham, bool>>? filter = null, string? includeProperties = null);
        Task<ResponseAPI<dynamic>> AddAsyncByDTO(SanPhamDTO objDTO);
        Task<ResponseAPI<dynamic>> UpdateByDTO(SanPhamDTO sanPhamDTO);
        Task<ResponseAPI<dynamic>> DeleteById(int? id);
    }
}
