using Perlink.Oi.Juridico.Application.Compartilhado.ViewModel;
using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Application.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.Compartilhado.Interface
{
    public interface IExportacaoPrePosRJAppService : IBaseCrudAppService<ExportacaoPrePosRJViewModel, ExportacaoPrePosRJ, long>
    {
        Task<IResultadoApplication<ExportacaoPrePosRJViewModel>> InserirDados(ExportacaoPrePosRJViewModel viewModel);
        Task<IResultadoApplication<ICollection<ExportacaoPrePosRJViewModel>>> ListarExportacaoPrePosRj(DateTime? dataExtracao, int pagina, int qtd);
        Task<IResultadoApplication<ExportacaoPrePosRJViewModel>> ExpurgarExportacaoPrePosRj(long idExtracao, bool naoExpurgar);
        Task<IResultadoApplication<ExportacaoPrePosRJListaViewModel>> DownloadExportacaoPrePosRj(long idExtracao, ICollection<string> tiposProcessos);
    }
}