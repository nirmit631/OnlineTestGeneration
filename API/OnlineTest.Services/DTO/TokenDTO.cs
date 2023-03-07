using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTest.Services.DTO
{
    public class TokenDTO
    {
        public TokenDTO()
        {
            Refresh_Token = string.Empty;
            Username = string.Empty;
            Password = string.Empty;
        }
        public string Grant_Type { get; set; }
        public string Refresh_Token { get; set; }

        //[Required]
        public string Username { get; set; }

        //[Required]
        public string Password { get; set; }
    }
}
