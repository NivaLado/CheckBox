namespace CheckBox.Constants
{
    public static class RouteConstants
    {
        public const string DomainName = "http://10.0.2.2:52236/";
        public const string RegisterUrl = "api/AppUser";
        public const string GoogleLogin = "api/AppUser/google";
        public const string Login = "api/AppUser/login";

        public const string GetAlbum = "api/Gallery/{0}";
        public const string AddAlbum = "api/Gallery";
        public const string DeleteAlbum = "api/Gallery/{0}";
        public const string UpdateAlbum = "api/Gallery";
        public const string GetAlbums = "api/Gallery/byUserId/{0}";
    }
}