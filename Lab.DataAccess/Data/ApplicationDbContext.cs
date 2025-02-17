using Lab.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options): base(options) { }
        public DbSet<tblNhanVien> NhanViens { get; set; }
        public DbSet<tblSanPham> SanPhams { get; set; }
        public DbSet<DonHang> DonHangs { get; set; }
        //public DbSet<GioHang> GioHangs { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
