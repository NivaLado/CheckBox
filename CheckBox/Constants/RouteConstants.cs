namespace CheckBox.Constants
{
    public static class RouteConstants
    {
        private const string Localhost = "http://10.0.2.2:52236";
        private const string LocalIIS = "http://10.0.2.2:90";
        private const string Production = "http://5.101.122.153";

        public const string DomainName = Production;
        public const string RegisterUrl = "api/AppUser";
        public const string GoogleLogin = "api/AppUser/google";
        public const string Login = "api/AppUser/login";

        public const string GetAlbum = "api/Gallery/{0}";
        public const string AddAlbum = "api/Gallery";
        public const string DeleteAlbum = "api/Gallery/{0}";
        public const string UpdateAlbum = "api/Gallery";
        public const string GetAlbums = "api/Gallery/byUserId/{0}";

        public const string GetImages = "api/Gallery/images/{0}";
        public const string DeleteImages = "api/Gallery/images";
    }
}