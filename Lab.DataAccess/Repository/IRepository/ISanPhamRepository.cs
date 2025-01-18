using Lab.Models;
using Lab.Models.DTOs.SanPham;
using Lab.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab.DataAccess.Repository.IRepository
{
    public interface ISanPhamRepository : IRepository<tblSanPham>
    {
        Task<bool> Update(int id, SanPhamDTO sanPhamDTO);
        Task<IEnumerable<SanPhamVM>> GetAllAsyncVM();
    }
}
