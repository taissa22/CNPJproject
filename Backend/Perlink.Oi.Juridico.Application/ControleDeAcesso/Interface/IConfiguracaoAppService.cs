using Perlink.Oi.Juridico.Application.ControleDeAcesso.ViewModel;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity;
using Shared.Application.Interface;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Application.ControleDeAcesso.Interface {
    public interface IConfiguracaoAppService {
        IResultadoApplication<ConfiguracaoViewModel> CarregarConfiguracao();
    }
}