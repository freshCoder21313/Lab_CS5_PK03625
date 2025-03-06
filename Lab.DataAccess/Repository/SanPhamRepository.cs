using Azure;
using Lab.DataAccess.Data;
using Lab.DataAccess.Repository.IRepository;
using Lab.Models;
using Lab.Models.DTOs.NhanVien;
using Lab.Models.DTOs.SanPham;
using Lab.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Lab.DataAccess.Repository
{
    public class SanPhamRepository(ApplicationDbContext db) : Repository<tblSanPham>(db), ISanPhamRepository
    {
        private readonly ApplicationDbContext _db = db;

        public async Task<ResponseAPI<SanPhamVM>> GetAsyncVM(Expression<Func<tblSanPham, bool>> filter, string? includeProperties = null, bool tracked = false)
        {
            ResponseAPI<SanPhamVM> responseSpVM = new ResponseAPI<SanPhamVM>();

            tblSanPham? tblSanPham = await base.GetAsync(filter, includeProperties, tracked);

            if (tblSanPham == null)
            {
                responseSpVM.Status = 204;
                responseSpVM.Message = "Không tìm thấy sản phẩm";
                return responseSpVM;
            }

            responseSpVM.Data = new SanPhamVM
            {
                MaSanPham = tblSanPham.MaSanPham,
                TenSanPham = tblSanPham.TenSanPham,
                SoLuong = tblSanPham.SoLuong,
                DonGia = tblSanPham.DonGia
            };

            return responseSpVM;
        }
        public async Task<ResponseAPI<List<SanPhamVM>>> GetAllAsyncVM(Expression<Func<tblSanPham, bool>>? filter = null, string? includeProperties = null)
        {
            List<tblSanPham> tblSanPhams = (await base.GetAllAsync(filter, includeProperties)).ToList();

            var sanPhamVMs = tblSanPhams.Select(x => new SanPhamVM
            {
                MaSanPham = x.MaSanPham,
                TenSanPham = x.TenSanPham,
                DonGia = x.DonGia,
                SoLuong = x.SoLuong
            }).ToList();

            return new ResponseAPI<List<SanPhamVM>>
            {
                Status = sanPhamVMs.Any() ? 200 : 204,
                Success = true,
                Data = sanPhamVMs,
                Message = sanPhamVMs.Any() ? "Danh sách sản phẩm đã được lấy thành công." : "Không tìm thấy sản phẩm trong phạm vi giá."
            };
        }

        public async Task<ResponseAPI<dynamic>> AddAsyncByDTO(SanPhamDTO objDTO)
        {
            tblSanPham tblSanPham = new tblSanPham
            {
                TenSanPham = objDTO.TenSanPham,
                SoLuong = objDTO.SoLuong,
                DonGia = objDTO.DonGia
            };

            await _db.SanPhams.AddAsync(tblSanPham);

            await _db.SaveChangesAsync();

            return new ResponseAPI<dynamic>
            {
                Status = 200,
                Success = true,
                Message = $"Đã thêm dữ liệu sản phẩm mang mã: {tblSanPham.MaSanPham}"
            };
        }

        public async Task<ResponseAPI<dynamic>> UpdateByDTO(SanPhamDTO sanPhamDTO)
        {
            tblSanPham? sanPham = await _db.SanPhams.FirstOrDefaultAsync(x => x.MaSanPham == sanPhamDTO.MaSanPham);

            if (sanPham == null)
            {
                return new ResponseAPI<dynamic>
                {
                    Status = 200,
                    Success = false,
                    Message = "Không tìm thấy sản phẩm để cập nhập thông tin."
                };
            }
            sanPham.TenSanPham = sanPhamDTO.TenSanPham;
            sanPham.DonGia = sanPhamDTO.DonGia;
            sanPham.SoLuong = sanPhamDTO.SoLuong;

            _db.Update(sanPham);

            await _db.SaveChangesAsync();

            return new ResponseAPI<dynamic>
            {
                Status = 200,
                Success = true,
                Message = "Sản phẩm đã cập nhập thành công."
            };
        }
        public async Task<ResponseAPI<dynamic>> DeleteById(int? id)
        {
            ResponseAPI<dynamic> responseSpVM = new ResponseAPI<dynamic>();

            tblSanPham? tblSanPham = await base.GetAsync(sp => sp.MaSanPham == id);

            if (tblSanPham == null)
            {
                responseSpVM.Status = 204;
                responseSpVM.Success = false;
                responseSpVM.Message = "Không tìm thấy sản phẩm muốn xóa.";
                return responseSpVM;
            }

            base.Remove(tblSanPham);

            await _db.SaveChangesAsync();

            responseSpVM.Status = 204;
            responseSpVM.Success = true;
            responseSpVM.Message = $"Sản phẩm mang mã {id} đã xóa thành công.";

            return responseSpVM;
        }
    }
}
