using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountManager.Domain.Dto
{
    public class LoginResponseDTO
    {
        public UserResponseDTO User { get; set; }
        public string Token { get; set; }
    }
}
