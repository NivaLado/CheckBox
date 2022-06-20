using CheckBox.Constants;
using CheckBox.Models;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CheckBox.ViewModels
{
    public class AlbumViewModel : BaseViewModel
    {
        private int imageCount;
        private string name;
        private string description;

        private bool ValidateSave()
        {
            return !string.IsNullOrWhiteSpace(name)
                && !string.IsNullOrWhiteSpace(description)
                && ImageCount > 0 && ImageCount <= 5;
        }

        public string FolderName { get; set; }

        public ObservableCollection<Images> Images { get; set; }

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

        public int ImageCount
        {
            get => imageCount;
            set => SetProperty(ref imageCount, value);
        }

        public Command SaveCommand { get; }

        public Command CancelCommand { get; }

        public Command RemoveCommand { get; }

        public Command TakePhotoCommand { get; }

        public Command PickPhotoCommand { get; set; }

        public AlbumViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            RemoveCommand = new Command<Images>(OnRemove);
            PickPhotoCommand = new Command(async () => await PickFromGallery());
            TakePhotoCommand = new Command(async () => await TakePhotoAsync());
            PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();

            Images = new ObservableCollection<Images>();
        }

        private async void OnSave()
        {
            Album newAlbum = new Album()
            {
                FolderName = FolderName,
                Title = Name,
                Description = Description,
                Images = Images.ToList(),
                CreationTime = DateTime.UtcNow
            };

            await CheckBoxService.AddAlbumAsync(newAlbum);

            await Shell.Current.GoToAsync("..");
        }

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

                FolderName ??= DateTime.UtcNow.ToString(AppConstants.AlbumFolderFormat);
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
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CapturePhotoAsync THREW: {ex.Message}");
            }
        }

        public virtual void OnRemove(Images image)
        {
            Images.Remove(image);
            ImageCount--;
        }


        private async void OnCancel()
        {
            await Shell.Current.GoToAsync("..");
        }

        private void AddFile(MediaFile file)
        {
            if (file != null)
            {
                Images.Add(new Images() { ImagePath = file.Path, ImageName = Path.GetFileName(file.Path) });
                ImageCount++;
            }
        }
    }
}
