﻿using System;
using System.Threading.Tasks;
using InspectionManager.Modelo;

namespace InspectionManager.Servicios
{
    public interface IFirebaseAuthService
    {
        string GetAuthKey();

        bool IsUserSigned();

        Task<bool> SignUp(Inspector inspector);

        Task<bool> SignIn(string username, string password);

        void SignInWithGoogle();

        Task<bool> SignInWithGoogle(string token);

        string GetUserId();

        Task<bool> SignOut();
    }
}