using Perlink.Oi.Juridico.Domain.ControleDeAcesso.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.ControleDeAcesso.Interface
{
    public interface IPerfilAppService 
    {
        Task<PerfilDTO> ObterDetalhePerfil(string codigoPerfil);
        Task<IList<PerfilDTO>> ListarGestoresDefault();
        Task Atualizar(PerfilDTO perfil);
        Task<PerfilDTO> Criar();
        Task Salvar(PerfilDTO perfilDTO);
        Task<byte[]> Exportar(string id, bool exportarPerfil);
    }
}
