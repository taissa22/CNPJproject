using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity;
using Shared.Domain.Interface.Repository;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Domain.ControleDeAcesso.Interface.Repository
{
    public interface IUsuarioGrupoRepository : IBaseRepository<UsuarioGrupo, string>
    {
        List<string> ObterGruposUsuario(string usuario);
        UsuarioGrupo ObterUsuarioAssociadoPerfil(string codigoUsuario, string codigoUsuarioDelegacao);
        void CriarUsuarioAssociadoPerfil(UsuarioGrupo usuarioGrupo);
        void RemoverUsuarioAssociadoPerfil(UsuarioGrupo usuarioGrupo);
        void SaveChanges();
    }
}
