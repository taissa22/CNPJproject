using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Constants;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.External;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System.Collections.Generic;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories.Implementations
{
    public class ComplementoDeAreaEnvolvidaRepository : IComplementoDeAreaEnvolvidaRepository
    {
        private IDatabaseContext DatabaseContext { get; }

        private ILogger<IComplementoDeAreaEnvolvidaRepository> Logger { get; }

        private IUsuarioAtualProvider UsuarioAtual { get; }

        public ComplementoDeAreaEnvolvidaRepository(IDatabaseContext databaseContext, ILogger<IComplementoDeAreaEnvolvidaRepository> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        private IQueryable<ComplementoDeAreaEnvolvida> ObterBase(TipoProcesso tipoProcesso, string pesquisa, ComplementoDeAreaEnvolvidaSort sort, bool ascending)
        {
            string logName = "Complemento Area Envolvida";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Ordenar {logName}"));
            IQueryable<ComplementoDeAreaEnvolvida> query = DatabaseContext.ComplementoDeAreasEnvolvidas.AsNoTracking();

            switch (sort)
            {
                case ComplementoDeAreaEnvolvidaSort.Id:
                    query = query.SortBy(x => x.Id, ascending);
                    break;

                case ComplementoDeAreaEnvolvidaSort.Nome:
                    query = query.SortBy(x => x.Nome, ascending);
                    break;

                case ComplementoDeAreaEnvolvidaSort.Ativo:
                    query = query.SortBy(x => x.Ativo, ascending);
                    break;

                default:
                    query = query.SortBy(x => x.Id, ascending);
                    break;
            }

            query = query.WhereIfNotNull(x => x.Nome.ToUpper().Contains(pesquisa.ToString().ToUpper()), pesquisa);

            bool temPermissao = false;

            switch (tipoProcesso.Id)
            {
                case 2:
                    temPermissao = UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_COMPLEMENTO_AREA_ENVOLVIDA_TRABALHISTA);
                    break;

                case 3:
                    temPermissao = UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_COMPLEMENTO_AREA_ENVOLVIDA_ADMINISTRATIVO);
                    break;

                case 5:
                    temPermissao = UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_COMPLEMENTO_AREA_ENVOLVIDA_TRIBUTARIO);
                    break;

                case 9:
                    temPermissao = UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_COMPLEMENTO_AREA_ENVOLVIDA_CIVEL_ESTRATATEGICO);
                    break;

                case 13:
                    temPermissao = UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_COMPLEMENTO_AREA_ENVOLVIDA_CRIMINAL);
                    break;
            }

            return query.Where(x => temPermissao && x.TipoProcesso.Id == tipoProcesso.Id);
        }

        public CommandResult<IReadOnlyCollection<ComplementoDeAreaEnvolvida>> Obter(TipoProcesso tipoProcesso, string pesquisa, ComplementoDeAreaEnvolvidaSort sort, bool ascending)
        {
            string logName = "Complemento De Area Envolvida";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var resultado = ObterBase(tipoProcesso, pesquisa, sort, ascending).ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<ComplementoDeAreaEnvolvida>>.Valid(resultado);
        }

        public CommandResult<PaginatedQueryResult<ComplementoDeAreaEnvolvida>> ObterPaginado(TipoProcesso tipoProcesso, string pesquisa, ComplementoDeAreaEnvolvidaSort sort, bool ascending, int pagina, int quantidade)
        {
            string logName = "Complemento De Area Envolvida";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var listaBase = ObterBase(tipoProcesso, pesquisa, sort, ascending);

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
            var total = listaBase.Count();

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Quantidade de registros para saltar"));
            var skip = Pagination.PagesToSkip(quantidade, total, pagina);

            var resultado = new PaginatedQueryResult<ComplementoDeAreaEnvolvida>()
            {
                Total = total,
                Data = listaBase.Skip(skip).Take(quantidade).ToArray()
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<PaginatedQueryResult<ComplementoDeAreaEnvolvida>>.Valid(resultado);
        }
    }
}