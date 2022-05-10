using CheckBox.Constants;
using CheckBox.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheckBox.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = new LoginViewModel();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                AppConstants.UserId = await Xamarin.Essentials.SecureStorage.GetAsync(nameof(AppConstants.UserId));
            }
            catch (System.Exception)
            {
                throw;
            }

            if (AppConstants.UserId != null)
            {
                await Shell.Current.GoToAsync($"//{nameof(GalleryPage)}");
            }
            else
            {
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }
        }
    }
}