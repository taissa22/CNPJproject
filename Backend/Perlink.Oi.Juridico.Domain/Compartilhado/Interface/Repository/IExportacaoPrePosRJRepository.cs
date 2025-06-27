using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Domain.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository
{
    public interface IExportacaoPrePosRJRepository : IBaseCrudRepository<ExportacaoPrePosRJ, long>
    {
        Task<IEnumerable<ExportacaoPrePosRJ>> ListarExportacaoPrePosRj(DateTime? dataExtracao, int pagina, int qtd);
        Task<int> QuantidadeTotal(DateTime? dataExtracao);
    }
}
