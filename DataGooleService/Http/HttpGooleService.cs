using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AdaDanaService.Dtos;
using AdaDanaService.Models;

namespace AdaDanaService.DataGooleService.Http
{
    public class HttpGooleService : IHttpGooleService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HttpGooleService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        public async Task<UserToken> SendLoginByGoleId(GooleIdDto gooleIdDto)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(gooleIdDto),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync($"{_configuration["GoleService"]}", httpContent);
            Console.WriteLine(response);
            var content = await response.Content.ReadAsStringAsync();
            var allContent = JsonSerializer.Deserialize<ResponseGooleIdDto>(content);
            if (allContent != null)
            {
                Console.WriteLine($"Username: {allContent.Username}");
                Console.WriteLine($"Token: {allContent.Token}");
            }


            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync POST to GoleService success !");
                return new UserToken { Token = allContent.Token };
            }
            else
            {
                Console.WriteLine("--> Sync POST to GoleService failed");
                return new UserToken { Message = "Failed to sync" };
            }
        }
    }
}