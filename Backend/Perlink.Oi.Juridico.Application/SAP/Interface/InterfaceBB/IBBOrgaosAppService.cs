using Perlink.Oi.Juridico.Application.SAP.ViewModel.InterfaceBB;
using Perlink.Oi.Juridico.Domain.SAP.DTO.InterfaceBB;
using Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB;
using Shared.Application.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.SAP.Interface.InterfaceBB {
    public interface IBBOrgaosAppService : IBaseCrudAppService<BBOrgaosViewModel, BBOrgaos, long> {
        Task<IResultadoApplication<BBOrgaosFiltrosViewModel>> CarregarFiltros();
        Task<IPagingResultadoApplication<ICollection<BBOrgaosViewModel>>> ConsultarBBOrgaos(ConsultaBBOrgaosDTO consultaBBOrgaosDTO);
        Task<IResultadoApplication<byte[]>> ExportarBBOrgaos(ConsultaBBOrgaosDTO consultaBBOrgaosDTO);
        Task<IResultadoApplication> SalvarBBOrgaos(BBOrgaosViewModel viewModel);
        Task<IResultadoApplication> ExcluirBBOrgaos(long id);
    }
}
