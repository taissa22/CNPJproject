using Perlink.Oi.Juridico.Application.Compartilhado.ViewModel.General;
using Perlink.Oi.Juridico.Application.SAP.ViewModel.InterfaceBB;
using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO.InterfaceBB;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB;
using Shared.Application.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.SAP.Interface.InterfaceBB
{
    public interface IBBStatusParcelasAppService : IBaseCrudAppService<BBStatusParcelasViewModel, BBStatusParcelas, long>
    {
        Task<IResultadoApplication<byte[]>> ExportarBBStatusParcelas(DescriptionFilterViewModel filtroDTO);
        Task<IPagingResultadoApplication<ICollection<BBStatusParcelasViewModel>>> ConsultarBBStatusParcelas(DescriptionFilterViewModel filtroDTO);
        Task<IResultadoApplication> SalvarBBStatusParcelas(BBStatusParcelaInclusaoEdicaoDTO inclusaoEdicao);
        Task<IResultadoApplication> ExcluirBBStatusParcelas(long id);
    }
}