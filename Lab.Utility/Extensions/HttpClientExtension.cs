using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Lab.Models;
using Lab.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Net.Http;

namespace Lab.Utility.Extensions
{
    public static class HttpClientExtension
    {
        private static readonly HttpClient httpClient = new HttpClient();
        public static async Task<T?> GetFromApiAsync<T>(this HttpClient httpClient, string url, IHttpContextAccessor httpContextAccessor, bool requiredAuth = true)
        {
            await SetAuthorizationHeader(httpClient, httpContextAccessor, requiredAuth);
            var response = await httpClient.GetAsync(url);
            return await HandleResponse<T>(response);
        }

        public static async Task<ResponseAPI<dynamic>?> DeleteFromApiAsync(this HttpClient httpClient, string url, IHttpContextAccessor httpContextAccessor, bool requiredAuth = true)
        {
            await SetAuthorizationHeader(httpClient, httpContextAccessor, requiredAuth);
            var response = await httpClient.DeleteAsync(url);
            return await HandleResponse(response);
        }

        public static async Task<ResponseAPI<dynamic>?> PostToApiAsync<U>(this HttpClient httpClient, string url, U content, IHttpContextAccessor httpContextAccessor, bool requiredAuth = true)
        {
            await SetAuthorizationHeader(httpClient, httpContextAccessor, requiredAuth);
            var jsonContent = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(url, jsonContent);
            return await HandleResponse(response);
        }

        public static async Task<ResponseAPI<dynamic>?> PutToApiAsync<U>(this HttpClient httpClient, string url, U content, IHttpContextAccessor httpContextAccessor, bool requiredAuth = true)
        {
            await SetAuthorizationHeader(httpClient, httpContextAccessor, requiredAuth);
            var jsonContent = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync(url, jsonContent);
            return await HandleResponse(response);
        }

        public static async Task<ResponseAPI<dynamic>?> PatchToApiAsync<U>(this HttpClient httpClient, string url, U content, IHttpContextAccessor httpContextAccessor, bool requiredAuth = true)
        {
            await SetAuthorizationHeader(httpClient, httpContextAccessor, requiredAuth);
            var jsonContent = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
            var method = new HttpMethod("PATCH");
            var request = new HttpRequestMessage(method, url) { Content = jsonContent };
            var response = await httpClient.SendAsync(request);
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
            var response = await httpClient.PostAsync($"https://localhost:7094/api/TruyCap/RefreshToken", content);

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

            return jwtToken.ValidTo < DateTime.UtcNow;
        }

        private static async Task<ResponseAPI<dynamic>?> HandleResponse(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ResponseAPI<dynamic>>(jsonResponse);
            }

            throw new HttpRequestException($"Error: {response.StatusCode}, Message: {await response.Content.ReadAsStringAsync()}");
        }
        private static async Task<T?> HandleResponse<T>(HttpResponseMessage response)
        {
            string jsonResponse = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<T>(jsonResponse);
            }

            var errorMessage = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Error: {response.StatusCode}, Message: {errorMessage}");
        }
    }
}
