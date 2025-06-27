using Perlink.Oi.Juridico.Domain.ContingenciaPex.DTO;
using Perlink.Oi.Juridico.Domain.ContingenciaPex.Entity;
using Shared.Domain.Interface.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.ContingenciaPex.Interface.Service
{
    public interface IFechamentoPexMediaService : IBaseCrudService<FechamentoPexMedia, long>
    {
        Task<IEnumerable<FechamentoContingenciaPexMediaDTO>> ListarFechamentos(string dataInicio, string dataFim, int quantidade, int pagina);
        Task<int> TotalFechamentos(string dataInicio, string dataFim);        
    }
}
