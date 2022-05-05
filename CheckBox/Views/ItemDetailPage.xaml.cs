using CheckBox.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace CheckBox.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}