using CheckBox.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheckBox.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GalleryPage : ContentPage
    {
        GalleryViewModel _viewModel;

        public GalleryPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new GalleryViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}