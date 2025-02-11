using Lab.Models;
using Lab.Models.DTOs.NhanVien;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab.DataAccess.Repository.IRepository
{
    public interface INhanVienRepository : IRepository<tblNhanVien>
    {
        Task Update(NhanVienDTO objDTO);
    }
}
