using CheckBox.Services;
using Xamarin.Forms;

namespace CheckBox
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<CheckBoxService>();
            DependencyService.Register<LocalStorageService>();

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
