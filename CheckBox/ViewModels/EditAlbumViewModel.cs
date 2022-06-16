using CheckBox.Models;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace CheckBox.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class EditAlbumViewModel : AlbumViewModel
    {
        private int itemId;
        private List<Images> originalImages;

        public EditAlbumViewModel() : base()
        {
            originalImages = new List<Images>();
        }

        public int ItemId
        {
            get
            {
                return itemId;
            }
            set
            {
                itemId = value;
                LoadItemId(value);
            }
        }

        public async void LoadItemId(int itemId)
        {
            try
            {
                var album = await CheckBoxService.GetAlbumAsync(itemId);
                originalImages = await CheckBoxService.GetImages(itemId);

                foreach (var image in originalImages)
                {
                    image.ImagePath = CheckBoxService.GetImageFullPath(album.FolderName, image.ImageName);
                    ImageCount++;
                    Images.Add(image);
                }

                FolderName = album.FolderName;
                Name = album.Title;
                Description = album.Description;
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to Load Item");
            }
        }
    }
}
