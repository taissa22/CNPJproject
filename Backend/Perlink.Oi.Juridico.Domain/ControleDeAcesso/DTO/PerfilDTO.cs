using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Domain.ControleDeAcesso.DTO
{
    public class PerfilDTO
    {
        /// <summary>
        /// O nome é o id do registro
        /// </summary>
        public string NomeId { get; set; }
        public string Descricao { get; set; }
        public string TipoUsuario { get; set; }
        public string GestorDefaultId { get; set; }
        public string GestorDefault { get; set; }
        public bool PerfilWeb { get; set; }
        public bool Restrito { get; set; }
        public bool Ativo { get; set; }
        public virtual int QuantidadeAssociadaUsuario { get; set; }
        public virtual int QuantidadeDisponivelUsuario { get; set; }
        public virtual int QuantidadeAssociadaPermissao { get; set; }
        public virtual int QuantidadeDisponivelPermissao { get; set; }
        public virtual bool tipoQuery { get; set; }
        public virtual bool mudancasUsuario { get; set; }
        public virtual bool mudancasPermissoes { get; set; }
        public virtual bool mudancasPerfilUsuario { get; set; }
        public ICollection<PerfilPermissaoDTO> Permissoes { get; set; }
        public ICollection<UsuariosPerfilDTO> UsuariosPerfil { get; set; }
    }
}
