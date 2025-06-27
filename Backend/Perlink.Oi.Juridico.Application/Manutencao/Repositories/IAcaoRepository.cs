using Perlink.Oi.Juridico.Application.Manutencao.Results.Acoes;
using Perlink.Oi.Juridico.Application.Manutencao.Results.Assunto;
using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.ValueObjects;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories
{
    public interface IAcaoRepository
    {
        #region Civel Estratégico

        CommandResult<PaginatedQueryResult<AcoesEstrategicoCommandResult>> ObterPaginadoDoCivelEstrategico(int pagina, int quantidade,
           AcaoCivelEstrategicoSort sort, bool ascending, string pesquisa = null);

        CommandResult<IReadOnlyCollection<AcoesEstrategicoCommandResult>> ObterDoCivelEstrategico(AcaoCivelEstrategicoSort sort, bool ascending, string pesquisa = null);

        #endregion Civel Estratégico

        #region Civel Consumidor

        CommandResult<PaginatedQueryResult<AcoesCommandResult>> ObterPaginadoDoCivelConsumidor(int pagina, int quantidade,
            AcaoCivelConsumidorSort sort, bool ascending, string pesquisa);

        CommandResult<IReadOnlyCollection<AcoesCommandResult>> ObterDoCivelConsumidor(AcaoCivelConsumidorSort sort, bool ascending, string pesquisa);
        CommandResult<IReadOnlyCollection<Acao>> ObterDescricaoDeParaConsumidor();

        #endregion Civel Consumidor

        #region Trabalhista

        CommandResult<PaginatedQueryResult<Acao>> ObterPaginadoDoTrabalhista(int pagina, int quantidade,AcaoTrabalhistaSort sort, bool ascending, string pesquisa);

        CommandResult<IReadOnlyCollection<Acao>> ObterDoTrabalhista(AcaoTrabalhistaSort sort, bool ascending, string pesquisa);

        #endregion Trabalhista

        #region Tributaria Judicial

        CommandResult<PaginatedQueryResult<Acao>> ObterPaginadoDoTributarioJudicial(int pagina, int quantidade,
            AcaoTributarioJudicialSort sort, bool ascending, string pesquisa);

        CommandResult<IReadOnlyCollection<Acao>> ObterDoTributarioJudicial(AcaoTributarioJudicialSort sort, bool ascending, string pesquisa);

        CommandResult<IReadOnlyCollection<Acao>> ObterDescricaoDeParaCivelEstrategico();

        #endregion Tributaria Judicial
    }
}