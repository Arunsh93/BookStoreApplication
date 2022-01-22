using BookStoreModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStoreRepository.Interface
{
    public interface IUserRepository
    {
        string Register(RegisterModel register);

        string Login(LoginModel loginModel);

        string ForgotPassword(string EmailId);

        bool ResetPassword(ResetPasswordModel resetPassword);

        string GenerateToken(string EmailId);
    }
}
