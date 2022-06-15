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

        readonly LoginViewModel _viewModel;

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
                int.TryParse(await Xamarin.Essentials.SecureStorage.GetAsync(nameof(AppConstants.UserId)), out int userId);
                AppConstants.UserId = userId;
            }
            catch (System.Exception)
            {
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }

            if (AppConstants.UserId > 0)
            {
                AppConstants.LoginMethod = await Xamarin.Essentials.SecureStorage.GetAsync(nameof(AppConstants.LoginMethod));
                await Shell.Current.GoToAsync($"//{nameof(GalleryPage)}");
            }
            else
            {
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }
        }
    }
}