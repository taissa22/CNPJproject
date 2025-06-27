using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Domain.Interface.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service
{
    public interface IEscritorioService :IBaseCrudService<Profissional, long>
    {
        Task<ICollection<EscritorioDTO>> RecuperarAreaCivilConsumidor();
        Task<ICollection<EscritorioDTO>> RecuperarProcon();
        Task<ICollection<EscritorioDTO>> RecuperarAreaCivelEstrategico();

        Task<ICollection<EscritorioDTO>> RecuperarEscritorioFiltro(long codigoTipoProcesso);

        Task<ICollection<EscritorioDTO>> RecuperarEscritorioTrabalhistaFiltro(bool ehEscritorio, string codUsuario);
        Task<ICollection<EscritorioDTO>> RecuperarEscritorioTrabalhista();
    }
}
