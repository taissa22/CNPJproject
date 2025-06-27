using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Application.AlteracaoBloco.ViewModel;
using Perlink.Oi.Juridico.Domain.AlteracaoBloco.Entity;
using Shared.Application.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.AlteracaoBloco.Interface
{
    public interface IAlteracaoEmBlocoAppService : IBaseCrudAppService<AlteracaoEmBlocoViewModel, AlteracaoEmBloco, long>
    {
        Task<IResultadoApplication> Upload(IFormFile arquivo, long codigoTipoProcesso);
        Task<IResultadoApplication<AlteracaoEmBlocoDownloadViewModel>> BaixarPlanilhaCarregada(long codigoAlteracaoEmBloco);
        Task<IResultadoApplication<AlteracaoEmBlocoDownloadViewModel>> BaixarPlanilhaResultado(long codigoAlteracaoEmBloco);
        Task<IResultadoApplication> RemoverAgendamentoComStatusAgendado(long id);
        Task<IPagingResultadoApplication<ICollection<AlteracaoEmBlocoViewModel>>> ListarAgendamentos(int index = 1, int count = 5);
        Task<IResultadoApplication> ExpurgoAlteracaoEmBloco(ILogger logger);
        Task<IResultadoApplication> ProcessarAgendamentos(ILogger logger);
    }
}
