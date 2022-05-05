using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using CheckBox.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CheckBox.Droid
{
	[Activity(Label = "CustomUrlSchemeInterceptorActivity", NoHistory = true, LaunchMode = LaunchMode.SingleTop)]
	[IntentFilter(
	new[] { Intent.ActionView },
	Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
	DataSchemes = new[] { "com.googleusercontent.apps.337584034380-ab807m8o05armaav9gsj2clv8khb8a5t" },
	DataPath = "/oauth2redirect")]
	public class CustomUrlSchemeInterceptorActivity : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Convert Android.Net.Url to Uri
			var uri = new Uri(Intent.Data.ToString());

			// Load redirectUrl page
			AuthenticationState.Authenticator.OnPageLoading(uri);

			Finish();
		}
	}
}