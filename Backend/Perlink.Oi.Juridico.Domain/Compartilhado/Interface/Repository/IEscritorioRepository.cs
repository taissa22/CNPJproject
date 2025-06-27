using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Domain.Interface.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository
{
    public interface IEscritorioRepository : IBaseCrudRepository<Profissional, long>
    {
        Task<ICollection<EscritorioDTO>> RecuperarAreaCivilConsumidor();

        Task<ICollection<EscritorioDTO>> RecuperarAreaProcon();

        Task<ICollection<EscritorioDTO>> RecuperarEscritorioFiltro(long codigoTipoProcesso);

        Task<ICollection<EscritorioDTO>> RecuperarAreaCivelEstrategico();

        Task<ICollection<EscritorioDTO>> RecuperarEscritorioTrabalhistaFiltro();

        Task<ICollection<EscritorioDTO>> RecuperarEscritorioTrabalhista();

        Task<ICollection<EscritorioDTO>> RecuperarEscritorioTrabalhistaFiltroUsuarioEscritorio(string codUsuario);
    }
}
