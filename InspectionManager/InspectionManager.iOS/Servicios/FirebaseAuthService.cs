using System;
using System.Threading;
using System.Threading.Tasks;
using Firebase.Auth;
using Foundation;
using InspectionManager.iOS.Servicios;
using InspectionManager.Servicios;
using Xamarin.Auth;
using Xamarin.Forms;

[assembly: Dependency(typeof(FirebaseAuthService))]
namespace InspectionManager.iOS.Servicios
{
    public class FirebaseAuthService: IFirebaseAuthService
    {
        public static string KEY_AUTH = "auth";

        public FirebaseAuthService()
        {
        }

        public string GetAuthKey()
        {
            return KEY_AUTH;
        }

        public string GetUserEmail()
        {
            var user = Auth.DefaultInstance.CurrentUser;
            return user.Uid;
        }

        public bool IsUserSigned()
        {
            var user = Auth.DefaultInstance.CurrentUser;
            return user != null;
        }

        public async Task<string> SignIn(string username, string password)
        {
            try
            {
                var result = await Auth.DefaultInstance.SignInWithPasswordAsync(username, password);
                Console.WriteLine(result);
                return await result.User.GetIdTokenAsync();
            }catch(Exception e)
            {
                return string.Empty;
            }
        }

        public void SignInWithGoogle()
        {
            throw new NotImplementedException();
        }

        public Task<bool> SignInWithGoogle(string token)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> LogOut()
        {
            NSError error;
            var signedOut = Auth.DefaultInstance.SignOut(out error);

            if (!signedOut)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> SignUp(string username, string password)
        {
            try
            {
                await Auth.DefaultInstance.CreateUserAsync(username, password);
                return true;
            }catch(Exception e)
            {
                return false;
            }
        }
    }
}
