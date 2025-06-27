using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.ControleDeAcesso.DTO
{
    public class PerfilPermissaoDTO
    {
        public virtual string Codigo { get; set; }

        public virtual string Descricao { get; set; }
        public virtual string Caminho { get; set; }
        public virtual string Janela { get; set; }
        public virtual string CodigoMenu { get; set; }
        public virtual bool Associado { get; set; }       
    }
}
