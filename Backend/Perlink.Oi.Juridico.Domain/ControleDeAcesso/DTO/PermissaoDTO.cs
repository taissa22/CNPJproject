using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.ControleDeAcesso.DTO
{
    public class PermissaoDTO
    {
        public string Aplicacao { get; set; }
        public string CodigoUsuario { get; set; }
        public string Janela { get; set; }
        public string Menu { get; set; }
    }
}
