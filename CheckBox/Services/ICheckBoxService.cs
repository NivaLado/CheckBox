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
        Task<bool> UpdateItemAsync(Album item);
        Task<bool> DeleteAlbumAsync(string id);
        Task<Album> GetAlbumAsync(string id);
        Task<IEnumerable<Album>> GetAlbumsAsync(bool forceRefresh = false);
    }
}
