using Perlink.Oi.Juridico.Application.Manutencao.Results.Assunto;
using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.ValueObjects;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories
{
   public interface IAssuntoRepository
    {
        #region Civel Consumidor

        CommandResult<PaginatedQueryResult<AssuntoCommandResult>> ObterPaginadoDoCivelConsumidor(int pagina, int quantidade,
            AssuntoCivelConsumidorSort sort, bool ascending, string descricao);

        CommandResult<IReadOnlyCollection<AssuntoCommandResult>> ObterDoCivelConsumidor(AssuntoCivelConsumidorSort sort, 
            bool ascending, string descricao);

        CommandResult<IReadOnlyCollection<Assunto>> ObterDescricaoDeParaCivelConsumidor();

        #endregion Civel Consumidor

        #region Civel Estrategico

        CommandResult<PaginatedQueryResult<AssuntoEstrategicoCommandResult>> ObterPaginadoDoCivelEstrategico(int pagina, int quantidade,
            AssuntoCivelEstrategicoSort sort, bool ascending, string descricao);

        CommandResult<IReadOnlyCollection<AssuntoEstrategicoCommandResult>> ObterDoCivelEstrategico(AssuntoCivelEstrategicoSort sort,
            bool ascending, string descricao);

        CommandResult<IReadOnlyCollection<Assunto>> ObterDescricaoDeParaCivelEstrategico();

        #endregion Civel Estrategico
    }
}
