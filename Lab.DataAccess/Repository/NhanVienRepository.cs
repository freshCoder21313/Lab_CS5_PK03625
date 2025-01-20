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
        public async Task Update(int id, NhanVienDTO objDTO)
        {
            tblNhanVien? infoGoc = await _db.NhanViens.FirstOrDefaultAsync(x => x.MaNhanVien == id);
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
        }
    }
}
