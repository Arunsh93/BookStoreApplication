using BookStoreModel;
using BookStoreRepository.Interface;
using Experimental.System.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;

namespace BookStoreRepository.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration configuration;

        SqlConnection sqlConnection;

        public UserRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string Register(RegisterModel register)
        {
            try
            {
                sqlConnection = new SqlConnection(this.configuration.GetConnectionString("BookStoreDBConnection"));
                using (sqlConnection)
                {
                    SqlCommand sqlCommand = new SqlCommand("Sp_Registration", sqlConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    sqlCommand.Parameters.AddWithValue("@FullName", register.FullName);
                    sqlCommand.Parameters.AddWithValue("@EmailId", register.EmailId);
                    sqlCommand.Parameters.AddWithValue("@Password", register.Password);
                    sqlCommand.Parameters.AddWithValue("@PhoneNumber", register.PhoneNumber);

                    sqlConnection.Open();

                    int result = sqlCommand.ExecuteNonQuery();

                    if (result == -1)
                    {
                        return "EmailId is Already Exist! Try with different EmailId";
                    }
                    else
                    {
                        return "User Registeration is Successfull";
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        public string Login(LoginModel loginModel)
        {
            try
            {
                sqlConnection = new SqlConnection(this.configuration.GetConnectionString("BookStoreDBConnection"));
                using (sqlConnection)
                {
                    SqlCommand sqlCommand = new SqlCommand("Sp_Login", sqlConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    sqlConnection.Open();

                    sqlCommand.Parameters.AddWithValue("@EmailId", loginModel.EmailId);
                    sqlCommand.Parameters.AddWithValue("@Password", loginModel.Password);

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    if (sqlDataReader.HasRows == true)
                    {
                        return "Login Successfull";
                    }
                    else
                    {
                        return "Login Unsuccessfull";
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        public string ForgotPassword(string EmailId)
        {
            try
            {
                sqlConnection = new SqlConnection(this.configuration.GetConnectionString("BookStoreDBConnection"));
                using (sqlConnection)
                {
                    SqlCommand sqlCommand = new SqlCommand("Sp_ForgotPassword", sqlConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@EmailId", EmailId);

                    sqlConnection.Open();

                    var result = Convert.ToInt32(sqlCommand.ExecuteScalar());
                    if(result == 1)
                    {
                        MailMessage mail = new MailMessage();
                        SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                        mail.From = new MailAddress(this.configuration["Credentials:EmailId"]);
                        mail.To.Add(EmailId);
                        mail.Subject = "Test Mail of Book Store Application for Forgot Password";
                        SendMSMQ();
                        mail.Body = ReceiveMSMQ();
                        SmtpServer.Port = 587;
                        SmtpServer.Credentials = new System.Net.NetworkCredential(this.configuration["Credentials:EmailId"], this.configuration["Credentials:Password"]);
                        SmtpServer.EnableSsl = true;

                        SmtpServer.Send(mail);
                        return "Password Reset Link send to your Mail Successfully!";
                    }
                    else
                    {
                        return "Failed! to Send a Mail";
                    }
                    
                    
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        public void SendMSMQ()
        {
            MessageQueue msgqueue;
            if (MessageQueue.Exists(@".\Private$\BookStore"))
            {
                msgqueue = new MessageQueue(@".\Private$\BookStore");
            }
            else
            {
                msgqueue = MessageQueue.Create(@".\Private$\BookStore");
            }
            msgqueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
            string body = "This is Password reset link for Book Store Application";
            msgqueue.Label = "Mail Body";
            msgqueue.Send(body);
        }

        public string ReceiveMSMQ()
        {
            MessageQueue msgqueue = new MessageQueue(@".\Private$\BookStore");
            var recievemsg = msgqueue.Receive();
            recievemsg.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
            return recievemsg.Body.ToString();
        }

        public bool ResetPassword(ResetPasswordModel resetPassword)
        {
            try
            {
                sqlConnection = new SqlConnection(this.configuration.GetConnectionString("BookStoreDBConnection"));
                using (sqlConnection)
                {
                    SqlCommand sqlCommand = new SqlCommand("Sp_ResetPassword", sqlConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@EmailId", resetPassword.EmailId);
                    sqlCommand.Parameters.AddWithValue("@NewPassword", resetPassword.NewPassword);

                    sqlConnection.Open();

                    int result = Convert.ToInt32(sqlCommand.ExecuteScalar());

                    if(result == 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        public string GenerateToken(string EmailId)
        {
            byte[] key = Convert.FromBase64String(this.configuration["SecretKey"]);
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, EmailId)
            }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
            };
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);
            return handler.WriteToken(token);
        }
    }
}
