using CheckBox.ViewModels;
using Plugin.Media;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheckBox.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewAlbumPage : ContentPage
    {
        public NewAlbumPage()
        {
            InitializeComponent();
            BindingContext = new NewAlbumViewModel();
        }

        protected async override void OnAppearing()
        {
            await CrossMedia.Current.Initialize();
        }
    }
}