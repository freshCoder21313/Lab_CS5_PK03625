﻿using Lab.Services.Redis.IServices;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Lab.Services.Redis
{
    public class RedisCacheService : IRedisCacheService
    {
        public readonly IDistributedCache? _cache;
        public RedisCacheService(IDistributedCache? cache)
        {
            _cache = cache;
        }
        public T? GetData<T>(string key)
        {
            var data = _cache?.GetString(key);
            if (data is null)
            {
                return default;
            }
            return JsonSerializer.Deserialize<T>(data);
        }

        public void SetData<T>(string key, T data)
        {
            var options = new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            };

            _cache?.SetString(key, JsonSerializer.Serialize(data), options);
        }
    }
}
