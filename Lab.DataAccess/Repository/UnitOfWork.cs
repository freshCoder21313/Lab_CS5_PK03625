using Lab.DataAccess.Data;
using Lab.DataAccess.Repository.IRepository;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        // Sub constructor
        private readonly JWTRepository _jwt;
        private readonly IConfiguration _configuration;
        // Main constructor
        private readonly ApplicationDbContext _db;
        public INguoiDungUngDungRepository NguoiDungs { get; private set; }
        public ISanPhamRepository SanPhams { get; private set; }
        public UnitOfWork(ApplicationDbContext db, JWTRepository jwt, IConfiguration configuration)
        {
            _db = db;
            _jwt = jwt;
            _configuration = configuration;
            this.NguoiDungs = new NguoiDungUngDungRepository(_db, _jwt);
            this.SanPhams = new SanPhamRepository(_db);
        }

        //public async Task SaveAsync()
        //{
        //    await _db.SaveChangesAsync();
        //}
    }
}
