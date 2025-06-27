using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.ControleDeAcesso.Security
{
    public class TokenSeguranca
    {
        public Decimal Id { get; set; }
        public string CodigoUsuario { get; set; }
        public string Token { get; set; }
        public string Ip { get; set; }
        public DateTime DataDeCriacao { get; set; }
    }
}
