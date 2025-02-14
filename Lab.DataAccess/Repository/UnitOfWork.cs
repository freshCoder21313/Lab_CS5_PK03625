using Lab.DataAccess.Data;
using Lab.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public INhanVienRepository NhanViens { get; private set; }
        public ISanPhamRepository SanPhams { get; private set; }
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            this.NhanViens = new NhanVienRepository(_db);
            this.SanPhams = new SanPhamRepository(_db);
        }

        //public async Task SaveAsync()
        //{
        //    await _db.SaveChangesAsync();
        //}
    }
}
