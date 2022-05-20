using CheckBox.Constants;
using CheckBox.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheckBox.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty(nameof(UserExist), nameof(UserExist))]
    public partial class LoginPage : ContentPage
    {
        public string UserExist { get; set; }

        LoginViewModel _viewModel;

        public LoginPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new LoginViewModel();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            bool.TryParse(UserExist, out bool result);
            _viewModel.ShowThatUserExist(result);

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