using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity;
using Shared.Domain.Interface.Service;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Domain.ControleDeAcesso.Interface.Service
{
    public interface IParametroService : IBaseCrudService<Parametro, string>
    {
        Parametro RecuperarPorNome(string codigoDoParametro);
        IList<Parametro> CarregarConfiguracao();
    }
}
