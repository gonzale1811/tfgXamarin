using System;
using System.Threading.Tasks;
using Android.Gms.Extensions;
using InspectionManager.Servicios;

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

        public string GetUserId()
        {
            return Firebase.Auth.FirebaseAuth.GetInstance(MainActivity.app).CurrentUser.Uid;
        }

        public bool IsUserSigned()
        {
            var user = Firebase.Auth.FirebaseAuth.GetInstance(MainActivity.app).CurrentUser;
            var signedIn = user != null;
            return signedIn;
        }

        public async Task<bool> SignIn(string username, string password)
        {
            try
            {
                await Firebase.Auth.FirebaseAuth.GetInstance(MainActivity.app).SignInWithEmailAndPasswordAsync(username, password);
                return true;
            }
            catch (Exception e)
            {
                return false;
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
            try
            {
                Firebase.Auth.FirebaseAuth.GetInstance(MainActivity.app).SignOut();
                return true;
            }catch(Exception e)
            {
                return false;
            }
        }

        public async Task<bool> SignUp(string username, string password)
        {
            try
            {
                await Firebase.Auth.FirebaseAuth.GetInstance(MainActivity.app).CreateUserWithEmailAndPasswordAsync(username, password);
                return true;
            }catch(Exception e)
            {
                return false;
            }
        }
    }
}
