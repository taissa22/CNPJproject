using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.ControleDeAcesso.DTO
{
    public class LoginDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string GrantType { get; set; }
        public string RefreshToken { get; set; }
    }
}
