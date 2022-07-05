using CheckBox.Models;
using CheckBox.ViewModels;
using FFImageLoading.Forms;
using Plugin.Media;
using System;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheckBox.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditAlbumPage : ContentPage
    {
        public EditAlbumPage()
        {
            InitializeComponent();
            BindingContext = new EditAlbumViewModel();
        }

        protected async override void OnAppearing()
        {
            await CrossMedia.Current.Initialize();
        }

        void OnImageSelected(object sender, EventArgs args)
        {
            var imageSender = (CachedImage)sender;
            TapGestureRecognizer tapGesture = (TapGestureRecognizer)imageSender.GestureRecognizers[0];
            // watch the monkey go from color to black&white!
            Navigation.ShowPopup(new ImagePopupPage());
        }
    }
}