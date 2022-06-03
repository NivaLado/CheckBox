using CheckBox.Views;
using Xamarin.Forms;

namespace CheckBox
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(RegistrationPage), typeof(RegistrationPage));
            Routing.RegisterRoute(nameof(GalleryPage), typeof(GalleryPage));
            Routing.RegisterRoute(nameof(NewAlbumPage), typeof(NewAlbumPage));
        }
    }
}
