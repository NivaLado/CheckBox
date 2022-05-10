using CheckBox.Views;
using Xamarin.Forms;

namespace CheckBox
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            // TODO: Remove this two routes
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));

            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(GalleryPage), typeof(GalleryPage));
            Routing.RegisterRoute(nameof(NewAlbumPage), typeof(NewAlbumPage));
        }
    }
}
