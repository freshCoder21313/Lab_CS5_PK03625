using Lab.DataAccess.Data;
using Lab.DataAccess.Repository.IRepository;
using Lab.Models;
using Lab.Models.DTOs.NhanVien;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab.DataAccess.Repository
{
    public class NhanVienRepository : Repository<tblNhanVien>, INhanVienRepository
    {
        private ApplicationDbContext _db;
        public NhanVienRepository(ApplicationDbContext db) : base (db)
        {
            _db = db;
        }

        public async Task<ResponseAPI<dynamic>> AddAsyncByDTO(NhanVienDTO objDTO)
        {

            tblNhanVien tblNhanVien = new tblNhanVien
            {
                HoTen = objDTO.HoTen,
                SoDienThoai = objDTO.SoDienThoai,
                NgaySinh = objDTO.NgaySinh,
                TenDangNhap = objDTO.TenDangNhap,
                MatKhau = objDTO.MatKhau,
                VaiTro = objDTO.VaiTro
            };

            if (objDTO.TenDangNhap == tblNhanVien.TenDangNhap && (await _db.NhanViens.AnyAsync(nv => nv.TenDangNhap == objDTO.TenDangNhap)))
            {
                return new ResponseAPI<dynamic>
                {
                    Status = 200,
                    Success = false,
                    Message = "Tên đăng nhập đã có trong hệ thống"
                };
            }

            await _db.NhanViens.AddAsync(tblNhanVien);

            await _db.SaveChangesAsync();

            return new ResponseAPI<dynamic>
            {
                Status = 200,
                Success = true,
                Message = $"Đã thêm dữ liệu nhân viên mang mã: {tblNhanVien.MaNhanVien}"
            };
        }

        public async Task Update(NhanVienDTO objDTO)
        {
            tblNhanVien? infoGoc = await _db.NhanViens.FirstOrDefaultAsync(x => x.MaNhanVien == objDTO.MaNhanVien);
            if (infoGoc == null)
            {
                return;
            }
            infoGoc.HoTen = objDTO.HoTen;
            infoGoc.NgaySinh = objDTO.NgaySinh;
            infoGoc.SoDienThoai = objDTO.SoDienThoai;
            infoGoc.TenDangNhap = objDTO.TenDangNhap;
            infoGoc.MatKhau = objDTO.MatKhau;
            infoGoc.VaiTro = objDTO.VaiTro;

            _db.Update(infoGoc);

            await _db.SaveChangesAsync();
        }
    }
}
