using CheckBox.Views;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using Xamarin.Auth;
using Xamarin.Forms;
using CheckBox.Models;
using CheckBox.Constants;
using Google.Apis.Auth;
using CheckBox.Enums;

namespace CheckBox.ViewModels
{
	public class LoginViewModel : BaseViewModel
	{
		private bool showUserNotExistNotification;

		public bool ShowUserNotExistNotification
		{
			get => showUserNotExistNotification;
			set => SetProperty(ref showUserNotExistNotification, value);
		}

        public string Email { get; set; }

		public string Password { get; set; }

		public Command LoginCommand { get; }

		public Command GoogleCommand { get; }

		public Command FacebookCommand { get; }

		public Command RegisterCommand { get; }

		public LoginViewModel()
		{
			LoginCommand = new Command(OnLoginClicked, CanLogin);
			GoogleCommand = new Command(OnGoogleClicked);
			RegisterCommand = new Command(OnRegisterClicked);
		}

		public void ShowThatUserExist (bool userExist)
        {
			ShowUserNotExistNotification = userExist;
		}

		private async void OnRegisterClicked()
        {
			await Shell.Current.GoToAsync($"{nameof(RegistrationPage)}");
		}

		private async void OnLoginClicked()
        {
			var id = await CheckBoxService.Login(Email, Password);
            if (id > 0)
            {
				await LocalStorageService.Login(id, (ushort)AuthorizationMethod.Internal);
				await Shell.Current.GoToAsync($"//{nameof(GalleryPage)}");
			}
			else
            {
				// TODO: Probably wrong credentials error message
				ShowThatUserExist(true);
			}
        }

		private bool CanLogin()
        {
			if (Email == null || Password == null)
				return false;
			else
				return true;
        }

		private void OnGoogleClicked()
		{
			string clientId = null;
			string redirectUri = null;

			switch (Device.RuntimePlatform)
			{
				case Device.iOS:
					clientId = AppConstants.iOSClientId;
					redirectUri = AppConstants.iOSRedirectUrl;
					break;

				case Device.Android:
					clientId = AppConstants.AndroidClientId;
					redirectUri = AppConstants.AndroidRedirectUrl;
					break;
			}

			var authenticator = new OAuth2Authenticator(
				clientId,
				null,
				AppConstants.Scope,
				new Uri(AppConstants.AuthorizeUrl),
				new Uri(redirectUri),
				new Uri(AppConstants.AccessTokenUrl),
				null,
				true);

			authenticator.Completed += OnAuthCompleted;
			authenticator.Error += OnAuthError;

			AuthenticationState.Authenticator = authenticator;

			var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
			presenter.Login(authenticator);
		}

		async void OnAuthCompleted(object sender, AuthenticatorCompletedEventArgs e)
		{
			if (sender is OAuth2Authenticator authenticator) 
			{
				authenticator.Completed -= OnAuthCompleted;
				authenticator.Error -= OnAuthError;
			}

			User user = null;
			if (e.IsAuthenticated)
			{
				// If the user is authenticated, request their basic user data from Google
				// UserInfoUrl = https://www.googleapis.com/oauth2/v2/userinfo
				var request = new OAuth2Request("GET", new Uri(AppConstants.UserInfoUrl), null, e.Account);
				var response = await request.GetResponseAsync();
				if (response != null)
				{
					// Deserialize the data and store it in the account store
					// The users email address will be used to identify data in SimpleDB
					string userJson = await response.GetResponseTextAsync();
					user = JsonConvert.DeserializeObject<User>(userJson);
				}

				// Validate JWT
				//var idToken = e.Account.Properties["id_token"];
				//var refreshToken = e.Account.Properties["refresh_token"];
				//var experisIn = e.Account.Properties["expires_in"];
				//var validPayload = await GoogleJsonWebSignature.ValidateAsync(idToken);

				if (user != null)
				{
					int id = await CheckBoxService.RegisterOrGetUserForGoogle(
						user.Email, 
						(ushort)AuthorizationMethod.Google,
						user.Id,
						user.GivenName,
						user.FamilyName);

					if (id > 0)
                    {
						await LocalStorageService.Login(id, (ushort)AuthorizationMethod.Google);
						await Shell.Current.GoToAsync($"//{nameof(GalleryPage)}");
                    }
					else
                    {
						ShowThatUserExist(true);
					}
				}
			}
		}

		void OnAuthError(object sender, AuthenticatorErrorEventArgs e)
		{
			if (sender is OAuth2Authenticator authenticator)
			{
				authenticator.Completed -= OnAuthCompleted;
				authenticator.Error -= OnAuthError;
			}

			Debug.WriteLine("Authentication error: " + e.Message);
		}
	}
}
