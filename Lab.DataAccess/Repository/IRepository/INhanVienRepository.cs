﻿using Lab.Models;
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
        Task Update(int id, NhanVienDTO objDTO);
        Task SetRefreshToken(int id, string refreshToken);
    }
}
