using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity;
using Shared.Domain.Interface.Repository;

namespace Perlink.Oi.Juridico.Domain.ControleDeAcesso.Interface.Repository
{
    public interface IParametroRepository : IBaseCrudRepository<Parametro, string>
    {
        Parametro RecuperarPorNome(string codigoDoParametro);
    }
}
