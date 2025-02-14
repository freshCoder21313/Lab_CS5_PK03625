using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        INhanVienRepository NhanViens { get; }
        ISanPhamRepository SanPhams { get; }
        //Task SaveAsync();
    }
}
