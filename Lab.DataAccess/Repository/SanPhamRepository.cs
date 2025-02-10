using Lab.DataAccess.Data;
using Lab.DataAccess.Repository.IRepository;
using Lab.Models;
using Lab.Models.DTOs.SanPham;
using Lab.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab.DataAccess.Repository
{
    public class SanPhamRepository : Repository<tblSanPham>, ISanPhamRepository
    {
        private ApplicationDbContext _db;
        public SanPhamRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public async Task<bool> Update(int id,SanPhamDTO sanPhamDTO)
        {
            tblSanPham? sanPham = await _db.SanPhams.FirstOrDefaultAsync(x => x.MaSanPham == id);
            if (sanPham == null)
            {
                return false;
            }
            sanPham.TenSanPham = sanPhamDTO.TenSanPham;
            sanPham.DonGia = sanPhamDTO.DonGia;
            sanPham.SoLuong = sanPhamDTO.SoLuong;

            _db.Update(sanPham);

            await _db.SaveChangesAsync();

            return true;
        }
        public async Task<IEnumerable<SanPhamVM>> GetAllAsyncVM()
        {
            var sanPhamVMs = _db.SanPhams.Select(x => new SanPhamVM
            {
                MaSanPham = x.MaSanPham,
                TenSanPham = x.TenSanPham,
                DonGia = String.Format(x.DonGia.ToString(), "0:0n"),
                SoLuong = x.SoLuong
            });

            return sanPhamVMs;
        }
    }
}
