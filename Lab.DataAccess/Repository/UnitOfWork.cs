using Lab.DataAccess.Data;
using Lab.DataAccess.Repository.IRepository;
using Lab.Models;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<NguoiDungUngDung> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        // Main constructor
        private readonly ApplicationDbContext _db;
        public INguoiDungUngDungRepository NguoiDungs { get; private set; }
        public ISanPhamRepository SanPhams { get; private set; }
        public IPaymentRepository Payments { get; private set; }
        public IRoleRepository Roles {get; private set;}
        public UnitOfWork(ApplicationDbContext db, JWTRepository jwt, IConfiguration configuration, UserManager<NguoiDungUngDung> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _jwt = jwt;
            _configuration = configuration;
            _userManager = userManager;
            _roleManager = roleManager;
            this.NguoiDungs = new NguoiDungUngDungRepository(_db, _jwt, userManager);
            this.SanPhams = new SanPhamRepository(_db);
            this.Payments = new PaymentRepository(_db);
            this.Roles = new RoleRepository(_db, _userManager, _roleManager);
        }

        //public async Task SaveAsync()
        //{
        //    await _db.SaveChangesAsync();
        //}
    }
}
