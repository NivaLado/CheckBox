using CheckBox.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CheckBox.Services
{
    public interface ICheckBoxService
    {
        Task<int> RegisterNewUser(string email, ushort authorizationMethod, string password, string name = null, string surname = null);

        Task<int> RegisterOrGetUserForGoogle(string email, ushort authorizationMethod, string googleId, string name, string surname);

        Task<int> Login(string email, string password);

        Task<bool> AddAlbumAsync(Album item);

        Task DeleteAlbumAsync(int albumId);

        Task<Album> GetAlbumAsync(int albumId);

        Task<List<Album>> GetAlbumsAsync(int userId);

        Task<List<Images>> GetImages(int albumId);

        // TODO: Move to helpers instead of service

        string GetImageFullPath(string folderName, string imageName);
    }
}
