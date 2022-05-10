using CheckBox.Views;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using Xamarin.Auth;
using Xamarin.Forms;
using CheckBox.Models;
using CheckBox.Constants;

namespace CheckBox.ViewModels
{
    public class LoginViewModel : BaseViewModel
	{
		public Command LoginCommand { get; }

		public Command GoogleCommand { get; }

		public Command RegisterCommand { get; }

		public Command ApiCommand { get; }

		public LoginViewModel()
		{
			LoginCommand = new Command(OnLoginClicked);
			ApiCommand = new Command(OnApiClicked);
			GoogleCommand = new Command(OnGoogleClicked);
			RegisterCommand = new Command(OnRegisterClicked);
		}

		private void OnRegisterClicked()
        {

        }

		private void OnApiClicked()
        {

        }

		private async void OnLoginClicked()
        {
			await Xamarin.Essentials.SecureStorage.SetAsync(nameof(AppConstants.UserId), "Test");
			await Shell.Current.GoToAsync($"//{nameof(GalleryPage)}");
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

				if (user != null)
				{
					await Shell.Current.GoToAsync(nameof(NewItemPage));
				}

				//await store.SaveAsync(account = e.Account, AppConstant.Constants.AppName);
				//await DisplayAlert("Email address", user.Email, "OK");
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
