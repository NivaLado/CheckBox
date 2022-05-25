namespace CheckBox.Constants
{
	public class AppConstants
	{
		public static string UserDirectory => $"{LoginMethod}_{UserId}";

		public static string AppName = "CheckBoxDevelopment";
		public static string AlbumFolderFormat = "MMddyyyyHHmmss";
		public static string UserId;
		public static string LoginMethod;

		// OAuth
		// For Google login, configure at https://console.developers.google.com/
		public static string iOSClientId = "";
		public static string AndroidClientId = "337584034380-ab807m8o05armaav9gsj2clv8khb8a5t.apps.googleusercontent.com";

		// These values do not need changing
		public static string Scope = "https://www.googleapis.com/auth/userinfo.email https://www.googleapis.com/auth/userinfo.profile";
		public static string AuthorizeUrl = "https://accounts.google.com/o/oauth2/auth";
		public static string AccessTokenUrl = "https://www.googleapis.com/oauth2/v4/token";
		public static string UserInfoUrl = "https://www.googleapis.com/oauth2/v2/userinfo";

		// Set these to reversed iOS/Android client ids, with :/oauth2redirect appended
		public static string iOSRedirectUrl = "";
		public static string AndroidRedirectUrl = "com.googleusercontent.apps.337584034380-ab807m8o05armaav9gsj2clv8khb8a5t:/oauth2redirect";
	}
}