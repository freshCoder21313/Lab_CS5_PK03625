using Lab.DataAccess.Repository.IRepository;
using Lab.Services.AI.HuggingFace;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AIController : ControllerBase
    {
        private readonly HuggingFaceService _huggingFaceService;
        private readonly IUnitOfWork _unit;

        public AIController(HuggingFaceService huggingFaceService, IUnitOfWork unitOfWork)
        {
            _huggingFaceService = huggingFaceService;
            _unit = unitOfWork;
        }
        [HttpPost("DynamicChatWithAI")]
        public async Task<IActionResult> DynamicChatWithAI([FromBody] string userQuestion)
        {
            // 1. Lấy dữ liệu từ CSDL
            var products = await _unit.SanPhams.GetAllAsync();

            // 2. Tạo mô tả sản phẩm từ CSDL
            var productDescriptions = string.Join("\n", products.Select(p =>
                $"{p.TenSanPham}: {p.SoLuong}, Price: {p.DonGia} USD."
            ));

            // 3. Tạo Prompt động
            var prompt =
                $"Dưới đây là danh sách sản phẩm có sẵn. Trả lời câu hỏi của người dùng dựa trên thông tin này:\n\n" +
                $"Sản phẩm:\n{productDescriptions}\n\n" +
                $"Câu hỏi của người dùng: \"{userQuestion}\"";

            // 4. Gửi prompt đến AI
            var aiResponse = await _huggingFaceService.GenerateTextAsync("gpt2", prompt);

            // 5. Phản hồi kết quả với người dùng
            return Ok(new { AIResponse = aiResponse });
        }

        [HttpPost("ChatWithData")]
        public async Task<IActionResult> ChatWithData([FromBody] string userQuestion)
        {
            // Lấy dữ liệu từ bảng Products
            var products = await _unit.SanPhams.GetAllAsync();

            // Tạo một prompt cơ bản cho AI với thông tin từ CSDL
            var productDetails = string.Join("\n", products.Select(p => $"{p.TenSanPham}: {p.SoLuong} (Price: {p.DonGia} USD)"));

            var prompt = $"Here is the product information:\n{productDetails}\n\nUser question: {userQuestion}";
            var aiResponse = await _huggingFaceService.GenerateTextAsync("gpt2", prompt);

            return Ok(new { AIResponse = aiResponse });
        }

        [HttpPost("GenerateText")]
        public async Task<IActionResult> GenerateText([FromBody] string prompt)
        {
            // Sử dụng model GPT-2 từ Hugging Face
            var model = "gpt2"; // Model này hỗ trợ text generation
            var result = await _huggingFaceService.GenerateTextAsync(model, prompt);

            return Ok(new { AIResponse = result });
        }
        [HttpPost("TranslateText")]
        public async Task<IActionResult> TranslateText([FromQuery] string targetLanguage, [FromBody] string text)
        {
            var model = "facebook/nllb-200-3.3B"; // Model dịch thuật
            var result = await _huggingFaceService.GenerateTextAsync(model, text);

            return Ok(new { TranslatedText = result });
        }

    }

}
