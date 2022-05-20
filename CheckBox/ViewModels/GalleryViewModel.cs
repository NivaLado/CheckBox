using CheckBox.Constants;
using CheckBox.Models;
using CheckBox.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CheckBox.ViewModels
{
    public class GalleryViewModel : BaseViewModel
    {
        private GalleryItem _selectedItem;

        public ObservableCollection<GalleryItem> Items { get; }

        public Command LoadItemsCommand { get; }

        public Command AddCommand { get; }

        // TODO: Change logout place
        public Command LogoutCommand { get; }

        public GalleryViewModel()
        {
            Title = "Gallery";
            Items = new ObservableCollection<GalleryItem>();
            LoadItemsCommand = new Command(ExecuteLoadItemsCommand);
            AddCommand = new Command(ExecuteAddCommand);
            LogoutCommand = new Command(ExecuteLogoutCommand);
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

        private void ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                var items = new ObservableCollection<GalleryItem>() 
                {
                    new GalleryItem() { Id = 0, Title = "Name 0", Url = "https://dummyimage.com/100x100/942e94/1aff00.png" },
                    new GalleryItem() { Id = 1, Title = "Name 1", Url = "https://dummyimage.com/200x200/304294/677d64.jpg" },
                    new GalleryItem() { Id = 2, Title = "Name 2", Url = "https://dummyimage.com/300x300/943131/677d64.jpg" },
                    new GalleryItem() { Id = 3, Title = "Name 3", Url = "https://dummyimage.com/500x500/46bf13/677d64.jpg" },
                    new GalleryItem() { Id = 4, Title = "Name 4", Url = "https://dummyimage.com/1000x1000/46bf13/c20000.jpg" },
                    new GalleryItem() { Id = 5, Title = "Name 5", Url = "https://dummyimage.com/100x100/942e94/1aff00.png" },
                    new GalleryItem() { Id = 6, Title = "Name 6", Url = "https://dummyimage.com/200x200/304294/677d64.jpg" },
                    new GalleryItem() { Id = 7, Title = "Name 7", Url = "https://dummyimage.com/300x300/943131/677d64.jpg" }
                };

                foreach (var item in items)
                {
                    Items.Add(item);
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

        public GalleryItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        async void OnItemSelected(GalleryItem item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item.Id}");
        }
    }
}
