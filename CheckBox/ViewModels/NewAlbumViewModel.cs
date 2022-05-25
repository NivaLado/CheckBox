using CheckBox.Constants;
using CheckBox.Models;
using PCLStorage;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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
        private string uniquePhotoName;
        public ObservableCollection<CheckItem> Checks { get; }

        public NewAlbumViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            PhotoCommand = new Command(async () => await TakePhotoAsync());
            PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();

            var tempCheckItem = new CheckItem() { Url = "https://dummyimage.com/1000x1000/46bf13/c20000.jpg" };

            Checks = new ObservableCollection<CheckItem>();
            //Checks.Add(tempCheckItem);
            //Checks.Add(tempCheckItem);
            //Checks.Add(tempCheckItem);
            //Checks.Add(tempCheckItem);
            //Checks.Add(tempCheckItem);
            //Checks.Add(tempCheckItem);
            //Checks.Add(tempCheckItem);
            //Checks.Add(tempCheckItem);
            //Checks.Add(tempCheckItem);
            //Checks.Add(tempCheckItem);
            //Checks.Add(tempCheckItem);
            //Checks.Add(tempCheckItem);
            //Checks.Add(tempCheckItem);
            //Checks.Add(tempCheckItem);
            //Checks.Add(tempCheckItem);
            //Checks.Add(tempCheckItem);
            //Checks.Add(tempCheckItem);
            //Checks.Add(tempCheckItem);
            //Checks.Add(tempCheckItem);
            //Checks.Add(tempCheckItem);
            //Checks.Add(tempCheckItem);
            //Checks.Add(tempCheckItem);
            //Checks.Add(tempCheckItem);
            //Checks.Add(tempCheckItem);
            //Checks.Add(tempCheckItem);
            //Checks.Add(tempCheckItem);
            //Checks.Add(tempCheckItem);
            //Checks.Add(tempCheckItem);
            //Checks.Add(tempCheckItem);
            //Checks.Add(tempCheckItem);
            //Checks.Add(tempCheckItem);
            //Checks.Add(tempCheckItem);
            //Checks.Add(tempCheckItem);
            //Checks.Add(tempCheckItem);
            //Checks.Add(tempCheckItem);
            //Checks.Add(tempCheckItem);
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
                var albumFolder = Path.Combine(AppConstants.UserDirectory, DateTime.UtcNow.ToString(AppConstants.AlbumFolderFormat));
                //var cacheFolder = PCLStorage.FileSystem.Current.LocalStorage;
                //var path = Path.Combine(cacheFolder.Path, albumFolder);
                var fileName = string.Concat(Guid.NewGuid().ToString().Replace("-", ""), $".jpg");

                var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    SaveToAlbum = true,
                    Directory = albumFolder,
                    CompressionQuality = 1,
                    CustomPhotoSize = 50,
                    PhotoSize = PhotoSize.MaxWidthHeight,
                    MaxWidthHeight = 2000,
                    DefaultCamera = CameraDevice.Rear,
                    Name = fileName
                });

                //await LoadPhotoAsync(file.Path, fileName);

                var imageStream = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    file.Dispose();
                    return stream;
                });


                Checks.Add(new CheckItem() { MyProperty = imageStream });
               
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

        async Task LoadPhotoAsync(string filePath, string fileName)
        {
            // canceled
            if (filePath == null)
            {
                PhotoPath = null;
                return;
            }
            
            IFolder folderPath;
            var albumFolder = Path.Combine(DateTime.UtcNow.ToString(AppConstants.AlbumFolderFormat), AppConstants.UserDirectory);
            var cacheFolder = PCLStorage.FileSystem.Current.LocalStorage;
            var path = Path.Combine(cacheFolder.Path, albumFolder);
            // Create or open cacheFolder
            if (cacheFolder.CheckExistsAsync(path).Result == ExistenceCheckResult.FolderExists)
            {
                folderPath = cacheFolder.GetFolderAsync(path).Result;
            }
            else
            {
                folderPath = await cacheFolder.CreateFolderAsync(path, CreationCollisionOption.ReplaceExisting);
            }

            // create a file, overwriting any existing file
            IFile file = await folderPath.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

            // populate the file with image data
            var photoStream = File.ReadAllBytes(filePath);
            using (Stream stream = await file.OpenAsync(PCLStorage.FileAccess.ReadAndWrite))
            {
                var imageStream = ImageSource.FromStream(() =>
                {
                    return stream;
                });
                Checks.Add(new CheckItem() { MyProperty = imageStream });

                stream.Write(photoStream, 0, photoStream.Length);
                stream.Dispose();
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
                Id = Guid.NewGuid().ToString(),
                Name = Name,
                Description = Description
            };

            await Shell.Current.GoToAsync("..");
        }
    }
}
