using CheckBox.Constants;
using CheckBox.Models;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CheckBox.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class EditAlbumViewModel : BaseViewModel
    {
        private int itemId;
        private string name;
        private string description;
        private int imageCount;
        private string folderName;
        private List<Images> originalImages;

        public ObservableCollection<Images> Images { get; set; }

        public EditAlbumViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            RemoveCommand = new Command<Images>(OnRemove);
            PickPhotoCommand = new Command(async () => await PickFromGallery());
            TakePhotoCommand = new Command(async () => await TakePhotoAsync());
            PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();

            Images = new ObservableCollection<Images>();
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
                    imageCount++;
                    Images.Add(image);
                }

                folderName = album.FolderName;
                Name = album.Title;
                Description = album.Description;
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to Load Item");
            }
        }

        public string PhotoPath { get; set; }

        private bool ValidateSave()
        {
            return !string.IsNullOrWhiteSpace(name)
                && !string.IsNullOrWhiteSpace(description)
                && imageCount > 0 && imageCount <= 5;
        }

        public int ImageCount
        {
            get => imageCount;
            set => SetProperty(ref imageCount, value);
        }

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public Command SaveCommand { get; }

        public Command CancelCommand { get; }

        public Command RemoveCommand { get; }

        public Command TakePhotoCommand { get; }

        public Command PickPhotoCommand { get; set; }

        private async Task PickFromGallery()
        {
            try
            {
                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    throw new AccessViolationException("No Pick photo supported");
                }

                var fileName = string.Concat(Guid.NewGuid().ToString().Replace("-", ""), $".jpg");

                var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                {
                    MaxWidthHeight = SettingConstants.MaxWidthHeight,
                    PhotoSize = SettingConstants.PhotoSize,
                    CompressionQuality = SettingConstants.CompressionQuality,
                    RotateImage = SettingConstants.RotateImage
                });

                AddFile(file);
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                Console.WriteLine($"CapturePhotoAsync THREW: {fnsEx.Message}");
            }
            catch (PermissionException pEx)
            {
                Console.WriteLine($"CapturePhotoAsync THREW: {pEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CapturePhotoAsync THREW: {ex.Message}");
            }
        }

        private async Task TakePhotoAsync()
        {
            try
            {
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    Console.WriteLine("No Camera", ":( No camera available.", "OK");
                    return;
                }

                var fileName = string.Concat(Guid.NewGuid().ToString().Replace("-", ""), $".jpg");

                var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    SaveMetaData = false,
                    AllowCropping = SettingConstants.AllowCropping,
                    DefaultCamera = SettingConstants.DefaultCamera,
                    MaxWidthHeight = SettingConstants.MaxWidthHeight,
                    SaveToAlbum = SettingConstants.SaveToAlbum,
                    PhotoSize = SettingConstants.PhotoSize,
                    CompressionQuality = SettingConstants.CompressionQuality,
                    RotateImage = SettingConstants.RotateImage,
                    Name = fileName
                });

                AddFile(file);
                Console.WriteLine($"CapturePhotoAsync COMPLETED: {PhotoPath}");
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                Console.WriteLine($"CapturePhotoAsync THREW: {fnsEx.Message}");
            }
            catch (PermissionException pEx)
            {
                Console.WriteLine($"CapturePhotoAsync THREW: {pEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CapturePhotoAsync THREW: {ex.Message}");
            }
        }

        private void OnRemove(Images image)
        {
            Images.Remove(image);
            ImageCount--;
        }

        private async void OnCancel()
        {
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            Album newAlbum = new Album()
            {
                FolderName = folderName,
                Title = Name,
                Description = Description,
                Images = Images.ToList(),
                CreationTime = DateTime.UtcNow
            };

            await CheckBoxService.AddAlbumAsync(newAlbum);

            await Shell.Current.GoToAsync("..");
        }

        private void AddFile(MediaFile file)
        {
            if (file != null)
            {
                Images.Add(new Images() { ImagePath = file?.Path });
                ImageCount++;
            }
        }
    }
}
