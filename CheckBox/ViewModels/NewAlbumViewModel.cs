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
        private string albumName;

        public ObservableCollection<string> Checks { get; set; }

        public NewAlbumViewModel()
        {
            SaveCommand = new Command(OnSave); //, ValidateSave);
            CancelCommand = new Command(OnCancel);
            PhotoCommand = new Command(async () => await TakePhotoAsync());
            PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();

            Checks = new ObservableCollection<string>();
            Checks.Add("https://dummyimage.com/1000x1000/46bf13/c20000.jpg");
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

        public Command PhotoCommand { get; }

        private async Task TakePhotoAsync()
        {
            try
            {
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    Console.WriteLine("No Camera", ":( No camera available.", "OK");
                    return;
                }
                var userDirectory = AppConstants.UserDirectory;
                albumName ??= DateTime.UtcNow.ToString(AppConstants.AlbumFolderFormat);
                var albumFolder = Path.Combine(albumName, DateTime.UtcNow.ToString(AppConstants.AlbumFolderFormat));
                var fileName = string.Concat(Guid.NewGuid().ToString().Replace("-", ""), $".jpg");

                var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    SaveToAlbum = true,
                    Directory = userDirectory,
                    CompressionQuality = 80,
                    PhotoSize = PhotoSize.Medium,
                    MaxWidthHeight = 2000,
                    DefaultCamera = CameraDevice.Rear,
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
                FolderName = albumName,
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
