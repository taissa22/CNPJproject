using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Application.Factories.TipoAudienciaResult;
using Perlink.Oi.Juridico.Application.Manutencao.Results.TiposAudiencias;
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
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories.Implementations
{
    internal class TipoAudienciaRepository : ITipoAudienciaRepository
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<ITipoAudienciaRepository> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public TipoAudienciaRepository(IDatabaseContext databaseContext, ILogger<ITipoAudienciaRepository> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }


        private IQueryable<TipoAudienciaCommandResult> ObterBase(TipoProcesso tipoProcesso, TipoAudienciaSort sort, bool ascending, string pesquisa = null)
        {
            string logName = "Tipos de audiência";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Ordenar {logName}"));
            //IQueryable<TipoAudiencia> query = DatabaseContext.TipoAudiencias.AsNoTracking();
            IQueryable<TipoAudienciaCommandResult> query = TipoAudienciaFactory.TipoAudienciaCommandResult(tipoProcesso)
                .CreateQuery(DatabaseContext).GerarQuery(sort, ascending, pesquisa);

            switch (sort)
            {
                case TipoAudienciaSort.Codigo:
                    query = query.SortBy(a => a.CodigoTipoAudiencia, ascending);
                    break;

                case TipoAudienciaSort.Sigla:
                    query = query.SortBy(x => x.Sigla != null, ascending).ThenSortBy(x => x.Sigla, ascending);
                    break;

                case TipoAudienciaSort.Ativo:
                    query = query.SortBy(x => x.Ativo, ascending).ThenSortBy(a => a.CodigoTipoAudiencia, ascending);                   
                    break;

                case TipoAudienciaSort.Descricao:
                default:
                    query = query.SortBy(a => a.Descricao, ascending);
                    break;
            }

            query = query.WhereIfNotNull(x => x.Descricao.ToUpper().Contains(pesquisa.ToString().ToUpper()), pesquisa);

            if (tipoProcesso != TipoProcesso.NAO_DEFINIDO)
            {
                switch (tipoProcesso.Id)
                {
                    case 1:
                        query = query.Where(x => x.EhCivelConsumidor);
                        break;
                    case 2:
                        query = query.Where(x => x.EhTrabalhista);
                        break;
                    case 3:
                        query = query.Where(x => x.EhAdministrativo);
                        break;
                    case 4:
                        query = query.Where(x => x.EhTributarioAdmin);
                        break;
                    case 5:
                        query = query.Where(x => x.EhTributarioJud);
                        break;
                    case 6:
                        query = query.Where(x => x.EhTrabalhistaAdmin);
                        break;
                    case 7:
                        query = query.Where(x => x.EhJuizado);
                        break;
                    case 9:
                        query = query.Where(x => x.EhCivelEstrategico);
                        break;
                    case 12:
                        query = query.Where(x => x.EhCivelAdmin);
                        break;
                    case 14:
                        query = query.Where(x => x.EhCriminalAdmin);
                        break;
                    case 15:
                        query = query.Where(x => x.EhCriminalJud);
                        break;
                    case 17:
                        query = query.Where(x => x.EhProcon);
                        break;
                    case 18:
                        query = query.Where(x => x.EhPex);
                        break;
                }
            }
            else
            {
                var visualizaCivelConsumidor = UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_AUDIENCIA_CIVEL_CONSUMIDOR);
                var visualizaCivelEstrategico = UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_AUDIENCIA_CIVEL_ESTRATIGICO);
                var visualizaTrabalhista = UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_AUDIENCIA_TRABALHISTA);
                var visualizaTrabalhistaAdministrativo = UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_AUDIENCIA_TRABALHISTA_ADMINISTRATIVO);
                var visualizaTributarioAdministrativo = UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_AUDIENCIA_TRIB_ADMINISTRATIVO);
                var visualizaTributarioJudicial = UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_AUDIENCIA_TRIB_JUDICIAL);
                var visualizaAdministrativo = UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_AUDIENCIA_ADMINISTRATIVO);
                var visualizaCivelAdministrativo = UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_AUDIENCIA_CIVEL_ADMINISTRATIVO);
                var visualizaCriminalJudicial = UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_AUDIENCIA_CRIMINAL_JUDICIAL);
                var visualizaCriminalAdministrativo = UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_AUDIENCIA_CRIMINAL_ADMINISTRATIVO);
                var visualizaJuizadoEspecial = UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_AUDIENCIA_JUIZADO_ESPECIAL);
                var visualizaProcon = UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_AUDIENCIA_PROCON);
                var visualizaPex = UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_AUDIENCIA_PEX);


                query = query.Where(x => (visualizaCivelConsumidor && x.EhCivelConsumidor) ||
                                         (visualizaCivelEstrategico && x.EhCivelEstrategico) ||
                                         (visualizaTrabalhista && x.EhTrabalhista) ||
                                         (visualizaTrabalhistaAdministrativo && x.EhTrabalhistaAdmin) ||
                                         (visualizaTributarioAdministrativo && x.EhTributarioAdmin) ||
                                         (visualizaTributarioJudicial && x.EhTributarioJud) ||
                                         (visualizaAdministrativo && x.EhAdministrativo) ||
                                         (visualizaCivelAdministrativo && x.EhCivelAdmin) ||
                                         (visualizaCriminalJudicial && x.EhCriminalJud) ||
                                         (visualizaCriminalAdministrativo && x.EhCriminalAdmin) ||
                                         (visualizaJuizadoEspecial && x.EhJuizado) ||
                                         (visualizaProcon && x.EhProcon) ||
                                         (visualizaPex && x.EhPex));
            }

            return query;
        }

        public CommandResult<PaginatedQueryResult<TipoAudienciaCommandResult>> ObterPaginado(int pagina, int quantidade, TipoAudienciaSort sort, bool ascending, TipoProcesso tipoProcesso, string pesquisa = null)
        {

            string logName = "Tipos de audiência";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_TIPO_AUDIENCIA))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_TIPO_AUDIENCIA, UsuarioAtual.Login));
                return CommandResult<PaginatedQueryResult<TipoAudienciaCommandResult>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var listaBase = ObterBase(tipoProcesso, sort, ascending, pesquisa);

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
            var total = listaBase.Count();

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Quantidade de registros para saltar"));
            var skip = Pagination.PagesToSkip(quantidade, total, pagina);

            var resultado = new PaginatedQueryResult<TipoAudienciaCommandResult>()
            {
                Total = total,
                Data = listaBase.Skip(skip).Take(quantidade).ToArray()
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<PaginatedQueryResult<TipoAudienciaCommandResult>>.Valid(resultado);
        }

        public CommandResult<IReadOnlyCollection<TipoAudienciaCommandResult>> Obter(TipoAudienciaSort sort, bool ascending, TipoProcesso tipoProcesso, string pesquisa = null)
        {
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_TIPO_AUDIENCIA))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_TIPO_AUDIENCIA, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<TipoAudienciaCommandResult>>.Forbidden();
            }

            string logName = "Tipos de audiência";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var resultado = ObterBase(tipoProcesso, sort, ascending, pesquisa).ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<TipoAudienciaCommandResult>>.Valid(resultado);
        }


        public CommandResult<IEnumerable<TipoProcesso>> ObterComboboxTipoProcesso()
        {
            List<TipoProcesso> tiposProcesso = new List<TipoProcesso>();

            if (UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_AUDIENCIA_CIVEL_CONSUMIDOR))
                tiposProcesso.Add(TipoProcesso.CIVEL_CONSUMIDOR);
            if (UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_AUDIENCIA_CIVEL_ESTRATIGICO))
                tiposProcesso.Add(TipoProcesso.CIVEL_ESTRATEGICO);
            if (UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_AUDIENCIA_TRABALHISTA))
                tiposProcesso.Add(TipoProcesso.TRABALHISTA);
            if (UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_AUDIENCIA_TRABALHISTA_ADMINISTRATIVO))
                tiposProcesso.Add(TipoProcesso.TRABALHISTA_ADMINISTRATIVO);
            if (UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_AUDIENCIA_TRIB_ADMINISTRATIVO))
                tiposProcesso.Add(TipoProcesso.TRIBUTARIO_ADMINISTRATIVO);
            if (UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_AUDIENCIA_TRIB_JUDICIAL))
                tiposProcesso.Add(TipoProcesso.TRIBUTARIO_JUDICIAL);
            if (UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_AUDIENCIA_ADMINISTRATIVO))
                tiposProcesso.Add(TipoProcesso.ADMINISTRATIVO);
            if (UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_AUDIENCIA_CIVEL_ADMINISTRATIVO))
                tiposProcesso.Add(TipoProcesso.CIVEL_ADMINISTRATIVO);
            if (UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_AUDIENCIA_CRIMINAL_JUDICIAL))
                tiposProcesso.Add(TipoProcesso.CRIMINAL_JUDICIAL);
            if (UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_AUDIENCIA_CRIMINAL_ADMINISTRATIVO))
                tiposProcesso.Add(TipoProcesso.CRIMINAL_ADMINISTRATIVO);
            if (UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_AUDIENCIA_JUIZADO_ESPECIAL))
                tiposProcesso.Add(TipoProcesso.JEC);
            if (UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_AUDIENCIA_PROCON))
                tiposProcesso.Add(TipoProcesso.PROCON);
            if (UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_AUDIENCIA_PEX))
                tiposProcesso.Add(TipoProcesso.PEX);

            return CommandResult<IEnumerable<TipoProcesso>>.Valid(tiposProcesso);
        }
    }
}
