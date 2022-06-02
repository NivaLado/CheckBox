using CheckBox.Constants;
using CheckBox.Models;
using PCLStorage;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CheckBox.ViewModels
{
    public class NewAlbumViewModel : BaseViewModel
    {
        private string name;
        private string description;
        private string folderName;

        public ObservableCollection<string> Checks { get; set; }

        public NewAlbumViewModel()
        {
            SaveCommand = new Command(OnSave); //, ValidateSave);
            CancelCommand = new Command(OnCancel);
            PickPhotoCommand = new Command(async () => await PickFromGallery());
            TakePhotoCommand = new Command(async () => await TakePhotoAsync());
            PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();

            Checks = new ObservableCollection<string>();
        }

        public string PhotoPath { get; set; }

        private bool ValidateSave()
        {
            return !string.IsNullOrWhiteSpace(name)
                && !string.IsNullOrWhiteSpace(description);
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

                Checks.Add(file?.Path);
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

                folderName ??= DateTime.UtcNow.ToString(AppConstants.AlbumFolderFormat);
                var fileName = string.Concat(Guid.NewGuid().ToString().Replace("-", ""), $".jpg");

                var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    AllowCropping = SettingConstants.AllowCropping,
                    DefaultCamera = SettingConstants.DefaultCamera,
                    MaxWidthHeight = SettingConstants.MaxWidthHeight,
                    SaveToAlbum = SettingConstants.SaveToAlbum,
                    PhotoSize = SettingConstants.PhotoSize,
                    CompressionQuality = SettingConstants.CompressionQuality,
                    RotateImage = SettingConstants.RotateImage,
                    Name = fileName
                });

                Checks.Add(file?.Path);
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
                CheckPath = Checks.ToList(),
                CreationTime = DateTime.UtcNow
            };

            await CheckBoxService.AddAlbumAsync(newAlbum);

            await Shell.Current.GoToAsync("..");
        }
    }
}
