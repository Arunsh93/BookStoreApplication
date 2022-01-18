using System;
using System.Collections.Generic;
using System.Text;

namespace BookStoreModel
{
    public class RegisterModel
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string EmailId { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
    }
}
