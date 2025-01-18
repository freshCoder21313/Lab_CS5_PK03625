using Lab.DataAccess.Data;
using Lab.Models;
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
        private readonly ApplicationDbContext _db;
        public DbInitializer(ApplicationDbContext db)
        {
            _db = db;
        }

        public void Initializer()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
                if (_db.NhanViens.Count() == 0)
                {
                    InitNhanVien_Lab2();
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

        public void InitNhanVien_Lab2()
        {
            var objs = new List<tblNhanVien>();
            for (int i = 0; i < 10; i++)
            {
                tblNhanVien blankObject = new tblNhanVien
                {
                    HoTen = RandomData_DB.Instance.rdName(),
                    SoDienThoai = RandomData_DB.Instance.RandomPhone(),
                    NgaySinh = (RandomData_DB.Instance.RandomBirthDate())
                };
                objs.Add(blankObject);
            }
            _db.NhanViens.AddRange(objs);
            _db.SaveChanges();
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
                    DonGia = rd.NextDouble() * 1000000
                };
                objs.Add(blankObject);
            }
            _db.SanPhams.AddRange(objs);
            _db.SaveChanges();
        }
    }
}
