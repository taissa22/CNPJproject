using Perlink.Oi.Juridico.Domain.ControleDeAcesso.DTO;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity;
using Shared.Domain.Interface.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.ControleDeAcesso.Interface.Service
{
    public interface IPerfilService : IBaseService<Usuario, string>
    {
        Task<PerfilDTO> ObterDetalhePerfil(string codigoPerfil);
        Task<IList<PerfilDTO>> ObterGestoresAprovadoresAtivos();
        Task<IList<PerfilPermissaoDTO>> ObterPermissoesAssociadasAoPerfil(string codigoPerfil);
        Task<IList<UsuariosPerfilDTO>> ObterUsuariosAssociadasAoPerfil(string codigoPerfil);
        Task AtualizarPerfil(PerfilDTO perfil);
        Task<PerfilDTO> CriarNovoUsuarioPerfil();
        Task SalvarUsuarioPerfil(PerfilDTO perfilDTO);
        bool ExistePerfil(string NomePerfil);

    }
}
