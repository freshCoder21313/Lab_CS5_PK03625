using Lab.Models;
using Lab.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab.DataAccess.Repository.IRepository
{
    public interface IThongKeRepository
    {
        Task<ResponseAPI<List<tblSanPham>?>> GetAllSanPhamAsync();
        Task<ResponseAPI<tblSanPham>> GetSanPhamByIdAsync(int id);
    }
}
