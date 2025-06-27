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
using System;
using System.Collections.Generic;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories.Implementations
{
    internal class JurosVigenciasCiveisRepository : IJurosVigenciasCiveisRepository
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IJurosVigenciasCiveisRepository> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public JurosVigenciasCiveisRepository(IDatabaseContext databaseContext, ILogger<IJurosVigenciasCiveisRepository> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }


        private IQueryable<JurosCorrecaoProcesso> ObterBase(TipoProcesso tipoProcesso, DateTime dataInicial, DateTime dataFinal, JurosVigenciasCiveisSort sort, bool ascending)
        {
            string logName = "Juros Vigências Cíveis";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Ordenar {logName}"));
            IQueryable<JurosCorrecaoProcesso> query = DatabaseContext.JurosCorrecoesProcessos.AsNoTracking().Where(x => x.TipoProcesso == tipoProcesso && x.DataVigencia.Date >= dataInicial.Date && x.DataVigencia.Date <= dataFinal.Date);

            switch (sort)
            {
                case JurosVigenciasCiveisSort.ValorJuros:
                    query = query.SortBy(a => a.ValorJuros, ascending);
                    break;

                case JurosVigenciasCiveisSort.InicioVigencia:
                default:
                    query = query.SortBy(a => a.DataVigencia, ascending);
                    break;
            }

            return query;
        }

        public CommandResult<PaginatedQueryResult<JurosCorrecaoProcesso>> ObterPaginado(TipoProcesso tipoProcesso, DateTime dataInicial, DateTime dataFinal, int pagina, int quantidade,
            JurosVigenciasCiveisSort sort, bool ascending)
        {
            string logName = "Juros Vigências Cíveis";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_JUROS_VIGENCIAS_CIVEIS))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_JUROS_VIGENCIAS_CIVEIS, UsuarioAtual.Login));
                return CommandResult<PaginatedQueryResult<JurosCorrecaoProcesso>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var listaBase = ObterBase(tipoProcesso, dataInicial, dataFinal, sort, ascending);

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
            var total = listaBase.Count();

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Quantidade de registros para saltar"));
            var skip = Pagination.PagesToSkip(quantidade, total, pagina);

            var resultado = new PaginatedQueryResult<JurosCorrecaoProcesso>()
            {
                Total = total,
                Data = listaBase.Skip(skip).Take(quantidade).ToArray()
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<PaginatedQueryResult<JurosCorrecaoProcesso>>.Valid(resultado);
        }

        public CommandResult<IReadOnlyCollection<JurosCorrecaoProcesso>> Obter(TipoProcesso tipoProcesso, DateTime dataInicial, DateTime dataFinal, JurosVigenciasCiveisSort sort, bool ascending)
        {
            string logName = "Juros Vigências Cíveis";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_JUROS_VIGENCIAS_CIVEIS))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_JUROS_VIGENCIAS_CIVEIS, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<JurosCorrecaoProcesso>>.Forbidden();
            }            

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var resultado = ObterBase(tipoProcesso, dataInicial, dataFinal, sort, ascending).ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<JurosCorrecaoProcesso>>.Valid(resultado);
        }

        public CommandResult<IEnumerable<TipoProcesso>> obterParaComboboxTipoProcesso()
        {
            List<TipoProcesso> tiposProcesso = new List<TipoProcesso>();

            if (UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_JUROS_CIVEL_CONSUMIDOR))
                tiposProcesso.Add(TipoProcesso.CIVEL_CONSUMIDOR);
            if (UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_JUROS_CIVEL_ESTRATEGICO))
                tiposProcesso.Add(TipoProcesso.CIVEL_ESTRATEGICO);           

            return CommandResult<IEnumerable<TipoProcesso>>.Valid(tiposProcesso);
        }

    }
}