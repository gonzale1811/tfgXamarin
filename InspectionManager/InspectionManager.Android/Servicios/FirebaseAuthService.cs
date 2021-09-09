using System;
using System.Threading.Tasks;
using Android.Gms.Extensions;
using InspectionManager.Droid.Servicios;
using InspectionManager.Servicios;
using Xamarin.Forms;

[assembly: Dependency(typeof(FirebaseAuthService))]
namespace InspectionManager.Droid.Servicios
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
            return Firebase.Auth.FirebaseAuth.GetInstance(MainActivity.app).CurrentUser.Email;
        }

        public bool IsUserSigned()
        {
            var user = Firebase.Auth.FirebaseAuth.GetInstance(MainActivity.app).CurrentUser;
            var signedIn = user != null;
            return signedIn;
        }

        public async Task<string> SignIn(string username, string password)
        {
            try
            {
                var result = await Firebase.Auth.FirebaseAuth.GetInstance(MainActivity.app).SignInWithEmailAndPasswordAsync(username, password);
                var token = await result.User.GetIdToken(false);
                return token.ToString();
            }
            catch (Exception)
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

        public Task<bool> LogOut()
        {
            try
            {
                Firebase.Auth.FirebaseAuth.GetInstance(MainActivity.app).SignOut();
                return Task.FromResult(true);
            }catch (Exception)
            {
                return Task.FromResult(false);
            }
        }

        public async Task<bool> SignUp(string username, string password)
        {
            try
            {
                await Firebase.Auth.FirebaseAuth.GetInstance(MainActivity.app).CreateUserWithEmailAndPasswordAsync(username, password);
                return true;
            }catch(Exception)
            {
                return false;
            }
        }
    }
}
