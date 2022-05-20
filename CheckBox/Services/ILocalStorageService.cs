using CheckBox.Enums;
using System.Threading.Tasks;

namespace CheckBox.Services
{
    public interface ILocalStorageService
    {
        Task Login(int userId, ushort loginMethod);

        void Logout();
    }
}
