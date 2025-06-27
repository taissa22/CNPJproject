using Perlink.Oi.Juridico.Application.ContingenciaPex.ViewModel;
using Perlink.Oi.Juridico.Domain.ContingenciaPex.Entity;
using Shared.Application.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.ContingenciaPex.Interface
{
    public interface IFechamentoPexMediaAppService : IBaseCrudAppService<FechamentoContingenciaPexMediaViewModel, FechamentoPexMedia, long>
    {
        Task<IPagingResultadoApplication<IEnumerable<FechamentoContingenciaPexMediaViewModel>>> ListarFechamentos(string dataInicio, string dataFim, int quantidade, int pagina);
        Task<(byte[] arquivo, string nomeArquivo)> ObterArquivo(int fechamentoId, DateTime dataFechamento, DateTime dataGeracao);
    }
}
