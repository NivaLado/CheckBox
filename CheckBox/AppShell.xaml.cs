using CheckBox.Views;
using Xamarin.Forms;

namespace CheckBox
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));

            Routing.RegisterRoute(nameof(NewAlbumPage), typeof(NewAlbumPage));
        }

    }
}
