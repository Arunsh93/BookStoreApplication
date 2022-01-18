using BookStoreManager.Interface;
using BookStoreModel;
using BookStoreRepository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStoreManager.Manager
{
    public class UserManager : IUserManager
    {
        private IUserRepository userRepository;

        public UserManager(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public string Register(RegisterModel register)
        {
            try
            {
               return this.userRepository.Register(register);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string Login(LoginModel loginModel)
        {
            try
            {
                return this.userRepository.Login(loginModel);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string ForgotPassword(string EmailId)
        {
            try
            {
                return this.userRepository.ForgotPassword(EmailId);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }         
        }

        public bool ResetPassword(ResetPasswordModel resetPassword)
        {
            try
            {
                return this.userRepository.ResetPassword(resetPassword);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
