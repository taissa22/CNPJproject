using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Shared.Domain.Interface.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Interface.Repository
{
    public interface IGrupoCorrecoesGarantiasRepository : IBaseCrudRepository<GrupoCorrecaoGarantia, long>
    {
        Task<IEnumerable<ComboboxDTO>> RecuperarComboboxGrupoCorrecao(long tipoProcesso);
    }
}
