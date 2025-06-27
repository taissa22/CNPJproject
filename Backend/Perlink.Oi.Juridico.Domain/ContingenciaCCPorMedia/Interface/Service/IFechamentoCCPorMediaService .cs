using Perlink.Oi.Juridico.Domain.ContingenciaCCPorMedia.DTO;
using Perlink.Oi.Juridico.Domain.ContingenciaCCPorMedia.Entity;
using Shared.Domain.Interface.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.ContingenciaCCPorMedia.Interface.Service
{
    public interface IFechamentoCCPorMediaService : IBaseCrudService<FechamentoCivelConsumidorPorMedia, long>
    {
        Task<IEnumerable<FechamentoContingenciaCCPorMediaDTO>> ListarFechamentos(string dataInicio, string dataFim, int quantidade, int pagina);
        Task<int> TotalFechamentos(string dataInicio, string dataFim);
    }
}
