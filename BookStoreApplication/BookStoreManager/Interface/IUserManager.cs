using BookStoreModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStoreManager.Interface
{
    public interface IUserManager
    {
        string Register(RegisterModel register);

        string Login(LoginModel loginModel);

        string ForgotPassword(string EmailId);

        bool ResetPassword(ResetPasswordModel resetPassword);
    }
}
