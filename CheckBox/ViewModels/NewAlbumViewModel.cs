using CheckBox.Models;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CheckBox.ViewModels
{
    public class NewAlbumViewModel : BaseViewModel
    {
        private string name;
        private string description;

        public ObservableCollection<string> ChecksFileLocation { get; }

        public string Test { get; set; }

        public NewAlbumViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            PhotoCommand = new Command(async () => await TakePhotoAsync());
            PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();

            ChecksFileLocation = new ObservableCollection<string>();
            ChecksFileLocation.Add("/data/user/0/com.companyname.checkbox/cache/822b028f37bc4ce7b9432897f156004b.jpg");
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
                var photo = await MediaPicker.CapturePhotoAsync();
                await LoadPhotoAsync(photo);
                ChecksFileLocation.Add(PhotoPath);
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

        async Task LoadPhotoAsync(FileResult photo)
        {
            // canceled
            if (photo == null)
            {
                PhotoPath = null;
                return;
            }
            // save the file into local storage
            var newFile = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
            using (var stream = await photo.OpenReadAsync())
            using (var newStream = File.OpenWrite(newFile))
                await stream.CopyToAsync(newStream);

            PhotoPath = newFile;
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

            // Gather Id from result
            // await DataStore.AddItemAsync(newItem);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}
