using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public static async Task<T?> GetFromApiAsync<T>(this HttpClient httpClient, string url, IHttpContextAccessor httpContextAccessor, bool requiredAuth = true)
        {
            if (requiredAuth)
            {
                var accessToken = await RefreshAccessToken(httpContextAccessor);
                if (!string.IsNullOrEmpty(accessToken))
                {
                    // Set Authorization header with token
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                }
                else
                {
                    throw new HttpRequestException("Unable to retrieve a valid token.");
                }
            }
            HttpResponseMessage response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var jsonObject = JObject.Parse(jsonResponse);
                return JsonConvert.DeserializeObject<T>(jsonObject["data"]?.ToString());
            }

            var errorMessage = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Error: {response.StatusCode}, Message: {errorMessage}");
        }

        private static async Task<string?> RefreshAccessToken(IHttpContextAccessor httpContextAccessor)
        {
            var accessToken = httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
            var refreshToken = httpContextAccessor.HttpContext?.Session.GetString("RefreshToken");

            if (!string.IsNullOrEmpty(accessToken) && !IsTokenExpired(accessToken))
            {
                return accessToken;
            }

            if (string.IsNullOrEmpty(refreshToken))
            {
                return null;
            }

            var httpClient = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(new { RefreshToken = refreshToken }), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("https://localhost:7094/api/TruyCap/RefreshToken", content);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var newTokenResponse = JsonConvert.DeserializeObject<TokenVM>(jsonResponse);

                if (newTokenResponse != null)
                {
                    httpContextAccessor.HttpContext.Session.SetString("AccessToken", newTokenResponse.AccessToken);
                    httpContextAccessor.HttpContext.Session.SetString("RefreshToken", newTokenResponse.RefreshToken);
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
    }
}
