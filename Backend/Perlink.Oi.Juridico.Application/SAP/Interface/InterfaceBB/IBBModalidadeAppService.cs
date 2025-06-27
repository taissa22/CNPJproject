using Perlink.Oi.Juridico.Application.SAP.ViewModel.InterfaceBB;
using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO.InterfaceBB;
using Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB;
using Shared.Application.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.SAP.Interface.InterfaceBB
{
    public interface IBBModalidadeAppService : IBaseCrudAppService<BBModalidadeViewModel, BBModalidade, long>
    {
        Task<IPagingResultadoApplication<ICollection<BBModalidadeViewModel>>> ConsultarBBModalidade(BBModalidadeFiltroDTO filtroDTO);
        Task<IResultadoApplication> AlterarBBModalidade(BBModalidadeViewModel objeto);
        Task<IResultadoApplication> CadastrarBBModalidade(BBModalidadeViewModel objeto);
        Task<IResultadoApplication> ExcluirBBModalidade(long codigo);
        Task<IResultadoApplication<byte[]>> ExportarBBModalidade(BBModalidadeFiltroDTO filtroDTO);

    }
}
