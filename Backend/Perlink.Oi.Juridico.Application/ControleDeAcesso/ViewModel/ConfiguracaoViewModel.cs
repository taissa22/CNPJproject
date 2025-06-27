using Shared.Application.ViewModel;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Application.ControleDeAcesso.ViewModel
{
    public class ConfiguracaoViewModel : BaseViewModel<string>
    {
        public IList<ParametroViewModel> Parametros { get; set; }
    }
}