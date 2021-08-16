using System;
using System.Threading.Tasks;

namespace InspectionManager.Servicios
{
    public interface IFirebaseAuthService
    {
        string GetAuthKey();

        bool IsUserSigned();

        Task<bool> SignUp(string username, string password);

        Task<string> SignIn(string username, string password);

        void SignInWithGoogle();

        Task<bool> SignInWithGoogle(string token);

        string GetUserId();

        Task<bool> LogOut();
    }
}
