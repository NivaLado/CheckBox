using CheckBox.ViewModels;
using Plugin.Media;
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
    }
}