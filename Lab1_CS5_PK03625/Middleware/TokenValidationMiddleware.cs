using Lab.DataAccess.Repository;
using Lab.Models;
using Lab.Services.Redis;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Lab.API.Middleware
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var endpoint = context.GetEndpoint();

            if (endpoint?.Metadata?.GetMetadata<AuthorizeAttribute>() != null)
            {
                // Chỉ kiểm tra cho yêu cầu có Authorization header
                if (context.Request.Headers.TryGetValue("Authorization", out var token))
                {
                    ResponseAPI<dynamic> response;
                    // Lấy dịch vụ từ DI
                    var tokenService = context.RequestServices.GetRequiredService<TokenService>();
                    var jwtRepo = context.RequestServices.GetRequiredService<JWTRepository>();

                    // Giải mã token để lấy userId
                    var tokenValue = token.ToString().Replace("Bearer ", "");

                    string? userId = (jwtRepo.TakeDataTokenAsync(tokenValue)).FindFirstValue(ClaimTypes.NameIdentifier) ?? null;

                    if (String.IsNullOrEmpty(userId))
                    {
                        response = new ResponseAPI<dynamic>
                        {
                            Status = 401,
                            Success = false,
                            Message = "Token không hợp lệ."
                        };
                    }
                    // Kiểm tra token có bị hủy hay không
                    if (await tokenService.IsTokenRevokedAsync(userId!))
                    {
                        context.Response.StatusCode = 401; // Unauthorized
                        context.Response.ContentType = "application/json"; // Đặt kiểu nội dung là JSON

                        response = new ResponseAPI<dynamic>
                        {
                            Status = 401,
                            Success = false,
                            Message = "Token đã bị hủy."
                        };

                        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                        return; // Dừng việc xử lý tiếp theo
                    }
                }

            }
            // Gọi middleware tiếp theo trong pipeline
            await _next(context);
        }
    }
}
