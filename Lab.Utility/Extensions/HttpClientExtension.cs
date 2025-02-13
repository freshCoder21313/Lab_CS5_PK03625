using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Lab.Models.ViewModels;
using System.IdentityModel.Tokens.Jwt;

namespace Lab.Utility.Extensions
{
    public static class HttpClientExtension
    {
        private static readonly HttpClient httpClient = new HttpClient();

        public static async Task<T?> GetFromApiAsync<T>(this HttpClient httpClient, string url, IHttpContextAccessor httpContextAccessor, bool requiredAuth = true)
        {
            await SetAuthorizationHeader(httpClient, httpContextAccessor, requiredAuth);

            HttpResponseMessage response = await httpClient.GetAsync(url);
            return await HandleResponse<T>(response);
        }
        public static async Task<string?> DeleteFromApiAsync(this HttpClient httpClient, string url, IHttpContextAccessor httpContextAccessor, bool requiredAuth = true)
        {
            await SetAuthorizationHeader(httpClient, httpContextAccessor, requiredAuth);

            HttpResponseMessage response = await httpClient.DeleteAsync(url);
            return await HandleResponse(response);
        }

        public static async Task<string?> PostToApiAsync<U>(this HttpClient httpClient, string url, U content, IHttpContextAccessor httpContextAccessor, bool requiredAuth = true)
        {
            await SetAuthorizationHeader(httpClient, httpContextAccessor, requiredAuth);

            var jsonContent = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await httpClient.PostAsync(url, jsonContent);
            return await HandleResponse(response);
        }

        public static async Task<string?> PutToApiAsync<U>(this HttpClient httpClient, string url, U content, IHttpContextAccessor httpContextAccessor, bool requiredAuth = true)
        {
            await SetAuthorizationHeader(httpClient, httpContextAccessor, requiredAuth);

            var jsonContent = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await httpClient.PutAsync(url, jsonContent);
            return await HandleResponse(response);
        }

        public static async Task<string?> PatchToApiAsync<U>(this HttpClient httpClient, string url, U content, IHttpContextAccessor httpContextAccessor, bool requiredAuth = true)
        {
            await SetAuthorizationHeader(httpClient, httpContextAccessor, requiredAuth);

            var jsonContent = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
            var method = new HttpMethod("PATCH");
            var request = new HttpRequestMessage(method, url) { Content = jsonContent };
            HttpResponseMessage response = await httpClient.SendAsync(request);
            return await HandleResponse(response);
        }

        private static async Task SetAuthorizationHeader(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, bool requiredAuth)
        {
            if (requiredAuth)
            {
                var accessToken = await RefreshAccessToken(httpContextAccessor);
                if (!string.IsNullOrEmpty(accessToken))
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                }
                else
                {
                    throw new HttpRequestException("Unable to retrieve a valid token.");
                }
            }
        }

        private static async Task<string?> RefreshAccessToken(IHttpContextAccessor httpContextAccessor)
        {
            var accessToken = httpContextAccessor.HttpContext?.Session.GetString(SD.AccessToken);
            var refreshToken = httpContextAccessor.HttpContext?.Session.GetString(SD.RefreshToken);

            if (!string.IsNullOrEmpty(accessToken) && !IsTokenExpired(accessToken))
            {
                return accessToken;
            }

            if (string.IsNullOrEmpty(refreshToken))
            {
                return null;
            }

            var content = new StringContent(JsonConvert.SerializeObject(new { RefreshToken = refreshToken }), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("https://localhost:7094/api/TruyCap/RefreshToken", content);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var newTokenResponse = JsonConvert.DeserializeObject<TokenVM>(jsonResponse);

                if (newTokenResponse != null)
                {
                    httpContextAccessor.HttpContext.Session.SetString(SD.AccessToken, newTokenResponse.AccessToken);
                    httpContextAccessor.HttpContext.Session.SetString(SD.RefreshToken, newTokenResponse.RefreshToken);
                    return newTokenResponse.AccessToken;
                }
            }

            return null;
        }

        private static bool IsTokenExpired(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            if (jwtToken == null)
            {
                return true;
            }

            var expiration = jwtToken.ValidTo;
            return expiration < DateTime.UtcNow;
        }
        private static async Task<string?> HandleResponse(HttpResponseMessage response)
        {
            string jsonResponse = await response.Content.ReadAsStringAsync();
            //Console.WriteLine($"Response Content: {jsonResponse}"); // Log nội dung phản hồi

            if (response.IsSuccessStatusCode)
            {
                return jsonResponse; // Trả về nội dung JSON trực tiếp
            }

            var errorMessage = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Error: {response.StatusCode}, Message: {errorMessage}");
        }
        private static async Task<T?> HandleResponse<T>(HttpResponseMessage response)
        {
            string jsonResponse = await response.Content.ReadAsStringAsync();
            //Console.WriteLine($"Response Content: {jsonResponse}"); // Log nội dung phản hồi

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    if (typeof(T) == typeof(string))
                    {
                        return (T)(object)jsonResponse;
                    }
                    var jsonObject = JObject.Parse(jsonResponse);

                    if (jsonObject["data"] != null)
                    {
                        return JsonConvert.DeserializeObject<T>(jsonObject["data"]?.ToString());
                    }
                    else
                    {
                        // Trường hợp không có "data", trả về phản hồi trực tiếp
                        return JsonConvert.DeserializeObject<T>(jsonResponse);
                    }
                }
                catch (JsonReaderException ex)
                {
                    Console.WriteLine("Gặp lỗi khi phân tích cú pháp JSON: " + ex.Message);
                    return JsonConvert.DeserializeObject<T>(jsonResponse);
                }
            }

            var errorMessage = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Error: {response.StatusCode}, Message: {errorMessage}");
        }


    }
}
