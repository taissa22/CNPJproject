using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity;
using Shared.Domain.Interface.Repository;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Domain.ControleDeAcesso.Interface.Repository
{
    public interface IJanelaRepository : IBaseCrudRepository<Janela, string>
    {
        List<string> FindJanelas(string codMenu);
    }
}
