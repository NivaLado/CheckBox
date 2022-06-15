using CheckBox.Constants;
using CheckBox.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SharedLayer.Dto;
using System.IO;
using System.Linq;

namespace CheckBox.Services
{
    public class CheckBoxService : ICheckBoxService
    {
        private static readonly HttpClient httpClient = new HttpClient() { BaseAddress = new Uri(RouteConstants.DomainName) };

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

        public async Task<bool> AddAlbumAsync(Album album)
        {
            using (var content = new MultipartFormDataContent())
            {
                List<FileStream> fileStream = new List<FileStream>();
                foreach(var check in album.CheckPath)
                {
                    fileStream.Add(new FileStream(check, FileMode.Open));
                    var streamContent = new StreamContent(fileStream.Last());
                    streamContent.Headers.Add("Content-Type", "application/octet-stream");
                    streamContent.Headers.Add("Content-Disposition", "form-data; name=\"file\"; filename=\"" + Path.GetFileName(check) + "\"");
                    content.Add(streamContent, "files", Path.GetFileName(check));
                }

                var albumDto = new AlbumDto() 
                { 
                    Title = album.Title,
                    Description = album.Description,
                    FolderName = album.FolderName,
                    UserId = AppConstants.UserId,
                    CreationTime = DateTime.UtcNow,
                    ThumbnailName = Path.GetFileName(album.CheckPath.First()) // Todo: Make more elegant thumnail picking
                };

                var stringContent = new StringContent(JsonConvert.SerializeObject(albumDto));
                stringContent.Headers.Add("Content-Disposition", "form-data; name=\"json\"");
                content.Add(stringContent, "json");

                var response = await httpClient.PostAsync(RouteConstants.AddAlbum, content);

                foreach (var steam in fileStream)
                {
                    steam.Dispose();
                }
            }

            return await Task.FromResult(true);
        }

        public Task<Album> GetAlbumAsync(int albumId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Album>> GetAlbumsAsync(int userId)
        {
            var url = string.Format(RouteConstants.GetAlbums, userId);
            var response = await httpClient.GetAsync(url);
            var responseMessage = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<Album>>(responseMessage);

            return result;
        }

        public Task<bool> DeleteAlbumAsync(int albumId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateItemAsync(Album item)
        {
            throw new NotImplementedException();
        }
    }
}
