using Perlink.Oi.Juridico.Domain.ControleDeAcesso.DTO;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity;
using Shared.Domain.Interface.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.ControleDeAcesso.Interface.Repository
{
    public interface IPerfilRepository : IBaseCrudRepository<Usuario, string>
    {
        Task<PerfilDTO> ObterDetalhePerfil(string codigoPerfil);
        Task<List<PerfilDTO>> ObterGestoresAprovadoresAtivos();
        Task<IList<PerfilPermissaoDTO>> ObterPermissoesAssociadasAoPerfil(string id);
        Task<IList<UsuariosPerfilDTO>> ObterUsuariosAssociadosAoPerfil(string id);
        IList<UsuariosPerfilDTO> ObterTodosUsuariosDisponiveis();
        IList<PerfilPermissaoDTO> ObterTodasPermissoesDisponiveis();
        Task<PerfilDTO> CriarNovoUsuarioPerfil();
        bool ExistePerfil(string NomePerfil);
    }
}
