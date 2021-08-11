using System;
using System.Threading.Tasks;
using InspectionManager.Modelo;

namespace InspectionManager.Servicios
{
    public class FirebaseAuthService: IFirebaseAuthService
    {
        public FirebaseAuthService()
        {
        }

        public string GetAuthKey()
        {
            throw new NotImplementedException();
        }

        public string GetUserId()
        {
            throw new NotImplementedException();
        }

        public bool IsUserSigned()
        {
            throw new NotImplementedException();
        }

        public Task<bool> SignIn(string username, string password)
        {
            throw new NotImplementedException();
        }

        public void SignInWithGoogle()
        {
            throw new NotImplementedException();
        }

        public Task<bool> SignInWithGoogle(string token)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SignOut()
        {
            throw new NotImplementedException();
        }

        public Task<bool> SignUp(Inspector inspector)
        {
            throw new NotImplementedException();
        }
    }
}
