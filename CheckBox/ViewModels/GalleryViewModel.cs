using CheckBox.Constants;
using CheckBox.Models;
using CheckBox.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Xamarin.Forms;

namespace CheckBox.ViewModels
{
    public class GalleryViewModel : BaseViewModel
    {
        private Album _selectedItem;

        public ObservableCollection<Album> Albums { get; }

        public Command LoadItemsCommand { get; }

        public Command AddCommand { get; }

        public Command RemoveCommand { get; }

        public Command OpenGalleryCommand { get; }

        // TODO: Change logout place
        public Command LogoutCommand { get; }

        public GalleryViewModel()
        {
            Title = "Gallery";
            Albums = new ObservableCollection<Album>();
            LoadItemsCommand = new Command(ExecuteLoadItemsCommand);
            AddCommand = new Command(ExecuteAddCommand);
            LogoutCommand = new Command(ExecuteLogoutCommand);
            RemoveCommand = new Command<int>(OnRemove);
            OpenGalleryCommand = new Command<Album>(OnItemSelected);
        }

        private async void OnRemove(int albumId)
        {
            // call service to remove album.
        }

        private async void ExecuteLogoutCommand()
        {
            LocalStorageService.Logout();
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }

        private async void ExecuteAddCommand()
        {
            await Shell.Current.GoToAsync(nameof(NewAlbumPage));
        }

        private async void ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Albums.Clear();
                var items = await CheckBoxService.GetAlbumsAsync(AppConstants.UserId);
                //new ObservableCollection<GalleryItem>() 
                //{
                //    new GalleryItem() { Id = 0, Title = "Title 0", Url = "https://dummyimage.com/100x100/942e94/1aff00.png" },
                //    new GalleryItem() { Id = 1, Title = "Title 1", Url = "https://dummyimage.com/200x200/304294/677d64.jpg" },
                //    new GalleryItem() { Id = 2, Title = "Title 2", Url = "https://dummyimage.com/300x300/943131/677d64.jpg" },
                //    new GalleryItem() { Id = 3, Title = "Title 3", Url = "https://dummyimage.com/500x500/46bf13/677d64.jpg" },
                //    new GalleryItem() { Id = 4, Title = "Title 4", Url = "https://dummyimage.com/1000x1000/46bf13/c20000.jpg" },
                //    new GalleryItem() { Id = 5, Title = "Title 5", Url = "https://dummyimage.com/100x100/942e94/1aff00.png" },
                //    new GalleryItem() { Id = 6, Title = "Title 6", Url = "https://dummyimage.com/200x200/304294/677d64.jpg" },
                //    new GalleryItem() { Id = 7, Title = "Title 7", Url = "https://dummyimage.com/300x300/943131/677d64.jpg" }
                //};

                foreach (var item in items)
                {
                    Albums.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
        }

        public Album SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        async void OnItemSelected(Album item)
        {
            if (item == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(EditAlbumPage)}?{nameof(EditAlbumViewModel.ItemId)}={item.Id}");
        }
    }
}
