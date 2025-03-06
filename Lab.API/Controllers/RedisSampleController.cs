using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace Lab.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedisSampleController : ControllerBase
    {
        private readonly IDistributedCache _cache;

        public RedisSampleController(IDistributedCache cache)
        {
            _cache = cache;
        }

        [HttpGet("{key}")]
        public async Task<IActionResult> Get(string key)
        {
            var value = await _cache.GetStringAsync(key);
            if (value == null)
            {
                return NotFound();
            }

            return Ok(value);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] KeyValuePair<string, string> data)
        {
            var options = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(30)) // Thay đổi thời gian hết hạn nếu sử dụng lại
                .SetAbsoluteExpiration(TimeSpan.FromHours(1)); // Một thời gian cụ thể để hết hạn

            await _cache.SetStringAsync(data.Key, data.Value, options);

            return CreatedAtAction(nameof(Get), new { key = data.Key }, data);
        }
    }
}
