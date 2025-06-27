using Perlink.Oi.Juridico.Domain.ContingenciaCCPorMedia.DTO;
using Perlink.Oi.Juridico.Domain.ContingenciaCCPorMedia.Entity;
using Shared.Domain.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.ContingenciaCCPorMedia.Interface.Repository
{
    public interface IFechamentoCCPorMediaRepository : IBaseCrudRepository<FechamentoCivelConsumidorPorMedia, long>
    {
        Task<IEnumerable<FechamentoContingenciaCCPorMediaDTO>> ListarFechamentos(string dataInicio, string dataFim, int quantidade, int pagina);
        Task<int> TotalFechamentos(string dataInicio, string dataFim);
        Task<DateTime> MaxDataExecucao(long codigo);

    }
}
