using BookStoreManager.Interface;
using BookStoreModel;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BookStoreApplication.Controller
{
    public class UserController : ControllerBase
    {
        private readonly IUserManager userManager;

        public UserController(IUserManager userManager)
        {
            this.userManager = userManager;
        }

        [HttpPost]
        [Route("api/register")]
        public IActionResult Register([FromBody] RegisterModel register)
        {
            try
            {
                var result = this.userManager.Register(register);
                if (result == "User Registeration is Successfull")
                {
                    return this.Ok(new ResponseModel<string> { Status = true, Message = result });
                }
                else
                {
                    return this.BadRequest(new ResponseModel<string> { Status = false, Message = "EmailId is Already Exist! Try with different EmailId" });
                }
            }
            catch(Exception ex)
            {
                return this.NotFound(new ResponseModel<string>() { Status = false, Message = ex.Message });
            }
        }

        [HttpPost]
        [Route("api/login")]
        public IActionResult Login([FromBody] LoginModel loginModel)
        {
            try
            {
                var result = this.userManager.Login(loginModel);
                string tokenstring = this.userManager.GenerateToken(loginModel.EmailId);
                if (result == "Login Successfull")
                {
                    return this.Ok(new ResponseModel<string> { Status = true, Message = result +  tokenstring });
                }
                else
                {
                    return this.BadRequest(new ResponseModel<string> { Status = false, Message = "Login Unsuccessfull" });
                }
            }
            catch (Exception ex)
            {
                return this.NotFound(new ResponseModel<string>() { Status = false, Message = ex.Message });
            }
        }

        [HttpPost]
        [Route("api/forgotPassword")]
        public IActionResult ForgotPassword(string EmailId)
        {
            try
            {
                string result = this.userManager.ForgotPassword(EmailId);
                if (result == "Password Reset Link send to your Mail Successfully!")
                {
                    return this.Ok(new ResponseModel<string>() { Status = true, Message = result });
                }
                else
                {
                    return this.BadRequest(new ResponseModel<string>() { Status = false, Message = result });
                }
            }
            catch (Exception ex)
            {
                return this.NotFound(new ResponseModel<string>() { Status = false, Message = ex.Message });
            }
        }

        [HttpPut]
        [Route("api/resetPassword")]
        public IActionResult ResetPassword([FromBody] ResetPasswordModel resetPassword)
        {
            try
            {
                bool result = this.userManager.ResetPassword(resetPassword);
                if (result == true)
                {
                    return this.Ok(new ResponseModel<string>() { Status = true, Message = "Password Reset Successfully!" });
                }
                else
                {
                    return this.BadRequest(new ResponseModel<string>() { Status = false, Message = "Password Reset Failed!" });
                }
            }
            catch (Exception ex)
            {
                return this.NotFound(new ResponseModel<string>() { Status = false, Message = ex.Message });
            }

        }
    }
}
