using Lab.Services.AI.HuggingFace.Response;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

public class HuggingFaceService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public HuggingFaceService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["HuggingFace:ApiKey"];
    }
    public async Task<string> GenerateTextAsync(string model, string input)
    {
        var requestBody = new
        {
            inputs = input
        };

        // Đặt Header Authorization và Content-Type
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _apiKey);
        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

        var response = await _httpClient.PostAsJsonAsync($"https://api-inference.huggingface.co/models/{model}", requestBody);

        if (!response.IsSuccessStatusCode)
        {
            var errorResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error from API: {response.StatusCode}, {errorResponse}");
            throw new Exception($"Error: {response.StatusCode}, {errorResponse}");
        }

        var jsonResponse = await response.Content.ReadAsStringAsync();

        // Kiểm tra JSON phản hồi và xử lý linh hoạt
        using (JsonDocument document = JsonDocument.Parse(jsonResponse))
        {
            if (document.RootElement.ValueKind == JsonValueKind.Array)
            {
                foreach (var element in document.RootElement.EnumerateArray())
                {
                    if (element.TryGetProperty("generated_text", out JsonElement generatedText))
                    {
                        return generatedText.GetString() ?? "No response generated.";
                    }
                }
            }
        }

        throw new Exception("Unexpected JSON Format.");
    }

}
