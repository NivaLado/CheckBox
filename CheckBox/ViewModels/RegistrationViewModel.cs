using CheckBox.Views;
using Xamarin.Forms;
using CheckBox.Enums;

namespace CheckBox.ViewModels
{
    public class RegistrationViewModel : BaseViewModel
	{
        public string Email { get; set; }

		public string Password { get; set; }

		public string RepeatPassword { get; set; }

		public Command RegisterCommand { get; }

		public Command CancelCommand { get; }

		public RegistrationViewModel()
		{
			RegisterCommand = new Command(OnRegisterClicked, CanRegister);
			CancelCommand = new Command(OnCancelCommand);
		}

		private async void OnCancelCommand()
        {
			await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
		}

		private async void OnRegisterClicked()
        {
			var userId = await CheckBoxService.RegisterNewUser(Email, (ushort)AuthorizationMethod.Internal, Password);
            if (userId > 0)
            {
				await LocalStorageService.Login(userId, (ushort)AuthorizationMethod.Internal);
				await Shell.Current.GoToAsync($"//{nameof(GalleryPage)}");
            }
			else
            {
				await Shell.Current.GoToAsync($"//{nameof(LoginPage)}?UserExist={true}");
			}
        }

		private bool CanRegister()
        {
			if (Email == null || Password == null || RepeatPassword == null || (Password != RepeatPassword))
				return false;
			else
				return true;
        }
	}
}
