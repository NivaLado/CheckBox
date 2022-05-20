using CheckBox.Constants;
using CheckBox.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheckBox.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrationPage : ContentPage
    {
        public RegistrationPage()
        {
            InitializeComponent();
            BindingContext = new RegistrationViewModel();
        }
    }
}