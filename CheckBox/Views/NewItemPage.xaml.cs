using CheckBox.Models;
using CheckBox.ViewModels;
using Xamarin.Forms;

namespace CheckBox.Views
{
    public partial class NewItemPage : ContentPage
    {
        public Item Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();
            BindingContext = new NewItemViewModel();
        }
    }
}