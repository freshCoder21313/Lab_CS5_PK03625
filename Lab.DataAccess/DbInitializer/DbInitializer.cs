using Lab.DataAccess.Data;
using Lab.Models;
using Lab.Utility.SharedData;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<NguoiDungUngDung> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        public DbInitializer(UserManager<NguoiDungUngDung> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext db)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _db = db;
        }

        public void Initializer()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                    CreateRolesAndAdminUser();
                }
                if (_db.SanPhams.Count() == 0)
                {
                    InitSanPham_Lab2();
                }
            } catch(Exception ex)
            {
                Console.WriteLine("Gặp lỗi: " + ex.Message);
                Console.WriteLine("Liên hệ nhà phát triển qua số sau để được hỗ trợ: 0347-018-582.");
            }
        }

        private void CreateRolesAndAdminUser()
        {
            if (!_roleManager.RoleExistsAsync(ConstantsValue.RoleCustomer).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(ConstantsValue.RoleCustomer)).GetAwaiter().GetResult();
                //_roleManager.CreateAsync(new IdentityRole(Constants.RoleStaff)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(ConstantsValue.RoleAdmin)).GetAwaiter().GetResult();

                _userManager.CreateAsync(new NguoiDungUngDung
                {
                    UserName = "admin@dotnetmastery.com",
                    Email = "admin@dotnetmastery.com",
                    HoTen = "Bhrugen Patel",
                    GioiTinh = true,
                    PhoneNumber = "1112223333",
                    DiaChi = "test 123 Ave",
                    LinkAnh = "https://i.pinimg.com/control/564x/6a/9c/77/6a9c77e0b1c7e5571ea5b5a350af0248.jpg",
                }, "Admin123*@").GetAwaiter().GetResult();

                var user = _db.NguoiDungUngDungs.FirstOrDefault(u => u.Email == "admin@dotnetmastery.com");
                _userManager.AddToRoleAsync(user!, ConstantsValue.RoleAdmin).GetAwaiter().GetResult();


                //SeedUsers();
                //SeedProductCategories();
                //SeedProducts();
                //SeedCombos();
                //SeedComboDetails();
                //SeedProductImages();
                //SeedProductVideos();
                //SeedOrders();
                //SeedFavorites();
                //SeedComments();
                //SeedOrderDetails();
            }

            if (!_db.NguoiDungUngDungs.Any(x => x.Email == "customer66@gmail.com"))
            {
                _userManager.CreateAsync(new NguoiDungUngDung
                {
                    UserName = "customer66@gmail.com",
                    Email = "customer66@gmail.com",
                    HoTen = "Bhrugen Patel",
                    GioiTinh = true,
                    PhoneNumber = "1112223333",
                    DiaChi = "test 123 Ave",
                    LinkAnh = "https://i.pinimg.com/control/564x/6a/9c/77/6a9c77e0b1c7e5571ea5b5a350af0248.jpg",
                }, "Admin123*@").GetAwaiter().GetResult();

                var user = _db.NguoiDungUngDungs.FirstOrDefault(u => u.Email == "email@gmail.com");
                _userManager.AddToRoleAsync(user!, ConstantsValue.RoleCustomer).GetAwaiter().GetResult();

                _db.SaveChanges();
            }
        }
        public void InitSanPham_Lab2()
        {
            var objs = new List<tblSanPham>();
            Random rd = new Random();
            for (int i = 0; i < 10; i++)
            {
                tblSanPham blankObject = new tblSanPham
                {
                    TenSanPham = RandomData_DB.Instance.RandomProductName(),
                    SoLuong = rd.Next(10,100),
                    DonGia = (decimal)(rd.NextDouble() * 1000000)
                };
                objs.Add(blankObject);
            }
            _db.SanPhams.AddRange(objs);
            _db.SaveChanges();
        }
    }
}
