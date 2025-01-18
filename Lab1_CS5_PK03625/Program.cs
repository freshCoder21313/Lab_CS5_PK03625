
using Lab.DataAccess.Data;
using Lab.DataAccess.DbInitializer;
using Lab.DataAccess.Repository;
using Lab.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Lab2_CS5_PK03625
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectString")));

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            // Đăng ký Swagger
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" }); // Thêm đường dẫn tới file XML để hiển thị comment
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"; 
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile); 
                c.IncludeXmlComments(xmlPath);
            });

            builder.Services.AddScoped<IDbInitializer, DbInitializer>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            SeedDatabaes();

            app.MapControllers();

            app.Run();

            void SeedDatabaes()
            {
                using (var seedScope = app.Services.CreateScope())
                {
                    var dbInitializer = seedScope.ServiceProvider.GetRequiredService<IDbInitializer>();
                    try
                    {
                        dbInitializer.Initializer();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error!");
                    }
                }
            }
        }
    }
}
