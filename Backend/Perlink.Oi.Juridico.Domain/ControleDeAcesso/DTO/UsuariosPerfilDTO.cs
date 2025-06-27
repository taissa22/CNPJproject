using System;

namespace Perlink.Oi.Juridico.Domain.ControleDeAcesso.DTO
{
    public class UsuariosPerfilDTO
    {
        public virtual string Codigo { get; set; }
        public virtual string Nome { get; set; }
        public virtual string Email { get; set; }
        public virtual bool Associado { get; set; }
        public virtual bool Ativo { get; set; }
        public virtual DateTime? DataUltimoAcesso { get; set; }
    }
   
}
