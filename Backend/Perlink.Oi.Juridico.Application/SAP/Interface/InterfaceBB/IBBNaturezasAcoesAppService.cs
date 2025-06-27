using Perlink.Oi.Juridico.Application.Compartilhado.ViewModel.General;
using Perlink.Oi.Juridico.Application.SAP.ViewModel.InterfaceBB;
using Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB;
using Shared.Application.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.SAP.Interface.InterfaceBB
{
    public interface IBBNaturezasAcoesAppService : IBaseCrudAppService<BBNaturezasAcoesViewModel, BBNaturezasAcoes, long>
    {
        Task<IPagingResultadoApplication<ICollection<BBNaturezasAcoesViewModel>>> ConsultarBBNaturezasAcoes(DescriptionFilterViewModel filtroDTO);
        
        Task<IResultadoApplication<byte[]>> ExportarBBNaturezasAcoes(DescriptionFilterViewModel filtroDTO);
        
        Task<IResultadoApplication> AlterarBBNaturezasAcoes(BBNaturezasAcoesViewModel objeto);
        
        Task<IResultadoApplication> CadastrarBBNaturezasAcoes(BBNaturezasAcoesViewModel objeto);
        
        Task<IResultadoApplication> ExcluirBBNaturezasAcoes(long id);
    }
}