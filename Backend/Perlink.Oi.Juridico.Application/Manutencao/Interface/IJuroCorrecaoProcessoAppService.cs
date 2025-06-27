using Perlink.Oi.Juridico.Application.Manutencao.ViewModel.JurosCorrecaoProcesso;
using Perlink.Oi.Juridico.Domain.Manutencao.JurosCorrecaoProcesso.Entity;
using Shared.Application.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.Manutencao.Interface
{
    public interface IJuroCorrecaoProcessoAppService : IBaseCrudAppService<JuroCorrecaoProcessoViewModel, JuroCorrecaoProcesso, long>
    {
        Task<IResultadoApplication<ICollection<JuroCorrecaoProcessoViewModel>>> ObterJuroCorrecaoProcessoPorFiltro(VigenciaCivilFiltrosViewModel viewModel);

        Task<IResultadoApplication<byte[]>> ExportarJuroCorrecaoProcesso(VigenciaCivilFiltrosViewModel viewModel);

        Task<IResultadoApplication> CadastrarJurosCorrecaoProcesso(JuroCorrecaoProcessoInputViewModel viewModel);

        Task<IResultadoApplication> EditarJuroCorrecaoProcesso(JuroCorrecaoProcessoInputViewModel viewModel);

        Task<IResultadoApplication> ExcluirJuroCorrecaoProcesso(long? codigo, DateTime? dataVigencia);
    }
}
