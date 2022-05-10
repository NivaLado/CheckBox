using CheckBox.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CheckBox.Services
{
    public class CheckBoxService : ICheckBoxService
    {
        public CheckBoxService()
        {

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
