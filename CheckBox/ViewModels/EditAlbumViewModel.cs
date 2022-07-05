using CheckBox.Models;
using CheckBox.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.CommunityToolkit.Extensions;

namespace CheckBox.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class EditAlbumViewModel : AlbumViewModel
    {
        private int itemId;
        private List<Images> originalImages;
        private List<string> imagesToRemove;
        private Album selectedAlbum;

        public Command UpdateCommand { get; set; }

        public Command OpenImageCommand { get; set; }

        public EditAlbumViewModel() : base()
        {
            originalImages = new List<Images>();
            imagesToRemove = new List<string>();
            UpdateCommand = new Command(OnUpdate);
            OpenImageCommand = new Command<Images >(OnImageSelected);
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

        public async void OnUpdate()
        {
            // TODO: Fake send if were no changes
            var newImages = Images.Where(i => originalImages.All(o => o.ImageName != i.ImageName)).ToList();

            Album newAlbum = new Album()
            {
                Id = selectedAlbum.Id,
                FolderName = FolderName,
                Title = Name,
                Description = Description,
                Images = newImages,
                ImagesToRemove = imagesToRemove,
                CreationTime = DateTime.UtcNow,
                ThumbnailName = selectedAlbum.ThumbnailName
            };

            await CheckBoxService.UpdateAlbumAsync(newAlbum);

            await Shell.Current.GoToAsync("..");
        }

        public override void OnRemove(Images image)
        {
            base.OnRemove(image);

            if (originalImages.Contains(image))
            {
                imagesToRemove.Add(image.ImageName);
            }

            if (image.IsThumbnail) selectedAlbum.ThumbnailName = Images.FirstOrDefault()?.ImageName;
        }

        public async void LoadItemId(int itemId)
        {
            try
            {
                // TODO: Merge this into one query
                selectedAlbum = await CheckBoxService.GetAlbumAsync(itemId);
                originalImages = await CheckBoxService.GetImages(itemId);

                foreach (var image in originalImages)
                {
                    if (image.ImageName == selectedAlbum.ThumbnailName) image.IsThumbnail = true;

                    image.ImagePath = CheckBoxService.GetImageFullPath(selectedAlbum.FolderName, image.ImageName);
                    ImageCount++;
                    Images.Add(image);
                }

                FolderName = selectedAlbum.FolderName;
                Name = selectedAlbum.Title;
                Description = selectedAlbum.Description;
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to Load Item");
            }
        }

        async void OnImageSelected(Images item)
        {
            if (item == null)
                return;
        }
    }
}
