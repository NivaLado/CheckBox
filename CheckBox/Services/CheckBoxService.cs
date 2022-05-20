using CheckBox.Constants;
using CheckBox.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SharedLayer.Dto;

namespace CheckBox.Services
{
    public class CheckBoxService : ICheckBoxService
    {
        static readonly HttpClient httpClient = new HttpClient() { BaseAddress = new Uri(RouteConstants.DomainName) };
        public CheckBoxService()
        {

        }

        public async Task<int> Login(string email, string password)
        {
            RegisterDto registerDto = new RegisterDto()
            {
                Email = email,
                Password = password
            };
            var json = JsonConvert.SerializeObject(registerDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(RouteConstants.Login, content);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                int id = JsonConvert.DeserializeObject<int>(responseBody);
                return id;
            }

            return 0;
        }

        public async Task<int> RegisterNewUser(
            string email, 
            ushort authorizationMethod, 
            string password, 
            string name = null, 
            string surname = null)
        {
            RegisterDto registerDto = new RegisterDto()
            {
                Email = email,
                Password = password,
                AuthorizationMethod = authorizationMethod,
                Name = name,
                Surname = surname
            };
            var json = JsonConvert.SerializeObject(registerDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(RouteConstants.RegisterUrl, content);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                int id = JsonConvert.DeserializeObject<int>(responseBody);
                return id;
            }

            return 0;
        }

        public async Task<int> RegisterOrGetUserForGoogle(string email, ushort authorizationMethod, string googleId, string name, string surname)
        {
            RegisterDto registerDto = new RegisterDto()
            {
                Email = email,
                AuthorizationMethod = authorizationMethod,
                VendorId = googleId,
                Name = name,
                Surname = surname
            };

            var json = JsonConvert.SerializeObject(registerDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(RouteConstants.GoogleLogin, content);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                int id = JsonConvert.DeserializeObject<int>(responseBody);
                return id;
            }

            return 0;
        }

        public Task<bool> AddAlbumAsync(Album item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAlbumAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Album> GetAlbumAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Album>> GetAlbumsAsync(bool forceRefresh = false)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateItemAsync(Album item)
        {
            throw new NotImplementedException();
        }
    }
}
