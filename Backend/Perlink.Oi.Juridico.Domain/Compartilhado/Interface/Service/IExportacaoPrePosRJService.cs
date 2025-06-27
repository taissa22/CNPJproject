using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Application.Interface;
using Shared.Domain.Interface.Service;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service
{
    public interface IExportacaoPrePosRJService : IBaseCrudService<ExportacaoPrePosRJ, long>
    {
        Task<ExportacaoPrePosRJ> InserirDados(ExportacaoPrePosRJ model);
        Task <IEnumerable<ExportacaoPrePosRJ>> ListarExportacaoPrePosRj(DateTime? dataExtracao, int pagina, int qtd);
        Task<int> ObterQuantidadeTotal(DateTime? dataExtracao);
        Task<DownloadExportacaoPrePosRjDTO> DownloadExportacaoPrePosRj(long idExtracao, ICollection<string> tiposProcessos);
    }
}
