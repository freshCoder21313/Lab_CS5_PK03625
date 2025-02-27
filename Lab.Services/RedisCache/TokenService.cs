using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Lab.Services.Redis
{
    public class TokenService
    {
        private readonly IDistributedCache _cache;

        public TokenService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task StoreTokenAsync(string userId, string token, TimeSpan? expiration = null)
        {
            // Lưu token vào Redis với userId đã mã hóa như key
            await _cache.SetStringAsync($"user_{userId}", token, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromHours(1),
            });
        }

        public async Task<string?> GetTokenAsync(string userId)
        {
            return await _cache.GetStringAsync($"user_{userId}");
        }

        public async Task RemoveTokenAsync(string userId)
        {
            await _cache.RemoveAsync($"user_{userId}");
        }

        public async Task<bool> IsTokenRevokedAsync(string userId)
        {
            var storedToken = await _cache.GetStringAsync($"user_{userId}");
            return storedToken == null || storedToken == "revoked";
        }

        public async Task RevokeTokenAsync(string userId)
        {
            await _cache.RemoveAsync($"user_{userId}");
            /*await _cache.SetStringAsync($"user_{userId}", "revoked", new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1), // Thời gian hết hạn để revoked
            });*/
        }
    }
}
