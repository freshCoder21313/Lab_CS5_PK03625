
using Lab.DataAccess.Data;
using Lab.DataAccess.DbInitializer;
using Lab.DataAccess.Repository;
using Lab.DataAccess.Repository.IRepository;
using Lab.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

namespace Lab_CS5_PK03625
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
                #region Format thêm comment lên môi action
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"; 
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile); 
                c.IncludeXmlComments(xmlPath);
                #endregion

                #region Thêm mid cho bảo m?t
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Vui lòng nhập mã Token: ",
                    Name = "Xác thuc: ",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
                #endregion
            });

            builder.Services.AddScoped<IDbInitializer, DbInitializer>();

            builder.Services.AddScoped<IJWTRepository, JWTRepository>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            #region //Cấu hình mã Secret
            builder.Services.Configure<AppSetting>(builder.Configuration.GetSection("AppSettings"));
            var secretKey = builder.Configuration["AppSettings:SecretKey"];
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(option =>
                {
                    option.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),
                        ClockSkew = TimeSpan.Zero
                    };
                });
            #endregion

            builder.Services.AddAuthentication();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
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
