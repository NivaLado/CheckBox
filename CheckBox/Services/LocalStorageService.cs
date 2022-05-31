using CheckBox.Constants;
using System.Threading.Tasks;

namespace CheckBox.Services
{
    public class LocalStorageService : ILocalStorageService
    {
        public async Task Login(int userId, ushort loginMethod)
        {
            AppConstants.UserId = userId;
            AppConstants.LoginMethod = loginMethod.ToString();
            await Xamarin.Essentials.SecureStorage.SetAsync(nameof(AppConstants.UserId), AppConstants.UserId.ToString());
            await Xamarin.Essentials.SecureStorage.SetAsync(nameof(AppConstants.LoginMethod), AppConstants.LoginMethod);
        }

        public void Logout()
        {
            AppConstants.UserId = 0;
            AppConstants.LoginMethod = string.Empty;
            Xamarin.Essentials.SecureStorage.Remove(nameof(AppConstants.UserId));
            Xamarin.Essentials.SecureStorage.Remove(nameof(AppConstants.LoginMethod));
        }
    }
}
