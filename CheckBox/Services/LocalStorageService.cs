using CheckBox.Constants;
using CheckBox.Enums;
using System.Threading.Tasks;

namespace CheckBox.Services
{
    public class LocalStorageService : ILocalStorageService
    {
        public async Task Login(int userId, ushort loginMethod)
        {
            await Xamarin.Essentials.SecureStorage.SetAsync(nameof(AppConstants.UserId), userId.ToString());
            await Xamarin.Essentials.SecureStorage.SetAsync(nameof(AppConstants.LoginMethod), loginMethod.ToString());
        }

        public void Logout()
        {
            Xamarin.Essentials.SecureStorage.Remove(nameof(AppConstants.UserId));
            Xamarin.Essentials.SecureStorage.Remove(nameof(AppConstants.LoginMethod));
        }
    }
}
