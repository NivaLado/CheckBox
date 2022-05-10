using CheckBox.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CheckBox.Services
{
    public interface ICheckBoxService
    {
        Task<bool> AddAlbumAsync(Album item);
        Task<bool> UpdateItemAsync(Album item);
        Task<bool> DeleteAlbumAsync(string id);
        Task<Album> GetAlbumAsync(string id);
        Task<IEnumerable<Album>> GetAlbumsAsync(bool forceRefresh = false);
    }
}
