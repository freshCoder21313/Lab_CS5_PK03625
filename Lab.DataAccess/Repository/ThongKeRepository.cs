using Dapper;
using Lab.DataAccess.Repository.IRepository;
using Lab.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Lab.DataAccess.Repository
{
    public class ThongKeRepository : IThongKeRepository
    {
        private readonly string _connectionString;
        public ThongKeRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectString")!;
        }

        public async Task<ResponseAPI<List<tblSanPham>?>> GetAllSanPhamAsync()
        {
            ResponseAPI<List<tblSanPham>?> response = new ResponseAPI<List<tblSanPham>?>(); 
            try
            {

                using (var connection = new SqlConnection(_connectionString))
                {
                    List<tblSanPham>? sanPhams = (await connection.QueryAsync<tblSanPham>("SELECT * FROM SanPhams")).ToList();
                    if (sanPhams.Count == 0 || sanPhams is null)
                    {
                        response.Status = 204;
                    }
                    else
                    {
                        response.Status = 200;
                        response.Data = sanPhams;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Status = 400;
                response.Success = false;
                response.Message = ex.Message;
                throw;
            }
            return response;
        }

        public async Task<ResponseAPI<tblSanPham>?> GetSanPhamByIdAsync(int id)
        {
            ResponseAPI<tblSanPham>? response = new ResponseAPI<tblSanPham>();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    tblSanPham? sanPham = await connection.QueryFirstAsync<tblSanPham?>($"SELECT * FROM SanPhams WHERE MaSanPham = {id}");
                    if (sanPham is null)
                    {
                        response.Status = 204;
                    }
                    else
                    {
                        response.Status = 200;
                        response.Data = sanPham;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Status = 400;
                response.Success = false;
                response.Message = ex.Message;
                throw;
            }
            return response;
        }
    }
}
