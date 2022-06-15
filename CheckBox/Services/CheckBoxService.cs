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
                foreach(var image in album.Images)
                {
                    fileStream.Add(new FileStream(image.ImagePath, FileMode.Open));
                    var streamContent = new StreamContent(fileStream.Last());
                    streamContent.Headers.Add("Content-Type", "application/octet-stream");
                    streamContent.Headers.Add("Content-Disposition", "form-data; name=\"file\"; filename=\"" + Path.GetFileName(image.ImagePath) + "\"");
                    content.Add(streamContent, "files", Path.GetFileName(image.ImagePath));
                }

                var albumDto = new AlbumDto() 
                { 
                    Title = album.Title,
                    Description = album.Description,
                    FolderName = album.FolderName,
                    UserId = AppConstants.UserId,
                    ThumbnailName = Path.GetFileName(album.Images.First().ImagePath) // Todo: Make more elegant thumnail picking
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

        public async Task<bool> UpdateAlbumAsync(Album album)
        {
            using (var content = new MultipartFormDataContent())
            {
                List<FileStream> fileStream = new List<FileStream>();
                foreach (var image in album.Images)
                {
                    fileStream.Add(new FileStream(image.ImagePath, FileMode.Open));
                    var streamContent = new StreamContent(fileStream.Last());
                    streamContent.Headers.Add("Content-Type", "application/octet-stream");
                    streamContent.Headers.Add("Content-Disposition", "form-data; name=\"file\"; filename=\"" + Path.GetFileName(image.ImagePath) + "\"");
                    content.Add(streamContent, "files", Path.GetFileName(image.ImagePath));
                }

                var albumDto = new AlbumDto()
                {
                    Title = album.Title,
                    Description = album.Description,
                    FolderName = album.FolderName,
                    UserId = AppConstants.UserId,
                    ThumbnailName = Path.GetFileName(album.Images.First().ImagePath) // Todo: Make more elegant thumnail picking
                };

                var stringContent = new StringContent(JsonConvert.SerializeObject(albumDto));
                stringContent.Headers.Add("Content-Disposition", "form-data; name=\"json\"");
                content.Add(stringContent, "json");

                var response = await httpClient.PutAsync(RouteConstants.UpdateAlbum, content);

                foreach (var steam in fileStream)
                {
                    steam.Dispose();
                }
            }

            return await Task.FromResult(true);
        }

        public async Task<Album> GetAlbumAsync(int albumId)
        {
            var url = string.Format(RouteConstants.GetAlbum, albumId);
            var response = await httpClient.GetAsync(url);
            var responseMessage = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Album>(responseMessage);

            return result;
        }

        public async Task<List<Album>> GetAlbumsAsync(int userId)
        {
            var url = string.Format(RouteConstants.GetAlbums, userId);
            var response = await httpClient.GetAsync(url);
            var responseMessage = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<Album>>(responseMessage);

            return result;
        }

        public async Task DeleteAlbumAsync(int albumId)
        {
            var url = string.Format(RouteConstants.DeleteAlbum, albumId);
            await httpClient.DeleteAsync(url);
        }

        public async Task<List<Images>> GetImages(int albumId)
        {
            var url = string.Format(RouteConstants.GetImages, albumId);
            var response = await httpClient.GetAsync(url);
            var responseMessage = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<Images>>(responseMessage);

            return result;
        }

        public string GetImageFullPath(string folderName, string imageName)
        {
            return AppConstants.UserDirectory + $"{folderName}/{imageName}";
        }
    }
}
