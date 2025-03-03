using Lab.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Lab.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<NguoiDungUngDung> // IdentityDbContext<...>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { } // DbContextOptions<ApplicationDbContext>

        public DbSet<NguoiDungUngDung> NguoiDungUngDungs { get; set; }
        public DbSet<tblSanPham> SanPhams { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
