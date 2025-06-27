using Perlink.Oi.Juridico.Application.ControleDeAcesso.ViewModel;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity;
using Shared.Application.Interface;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Application.ControleDeAcesso.Interface {
    public interface IParametroAppService : IBaseCrudAppService<ParametroViewModel, Parametro, string> {
        IResultadoApplication<ParametroViewModel> RecuperarPorNome(string codigoDoParametro);
        IResultadoApplication<IList<ParametroViewModel>> CarregarConfiguracao();
    }
}