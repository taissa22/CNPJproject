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
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories.Implementations
{
    class TipoProcedimentoRepository : ITipoProcedimentoRepository
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<ITipoProcedimentoRepository> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public TipoProcedimentoRepository(IDatabaseContext databaseContext, ILogger<TipoProcedimentoRepository> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        private IQueryable<Procedimento> ObterBase(TipoProcesso tipoProcesso, TipoProcedimentoSort sort, bool ascending, string pesquisa = null)
        {
            string logName = "Tipo de Procedimento";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Ordenar {logName}"));
            IQueryable<Procedimento> query = DatabaseContext.Procedimentos.AsNoTracking();

            switch (sort)
            {
                case TipoProcedimentoSort.Codigo:
                    query = query.SortBy(x => x.Codigo, ascending);
                    break;
                case TipoProcedimentoSort.Ativo:
                break;
                case TipoProcedimentoSort.TipoParticipacao1:
                    query = query.Where(x => x.TipoDeParticipacao1 != null).SortBy(x => x.TipoDeParticipacao1.Descricao, ascending);                    
                    break;
                case TipoProcedimentoSort.TipoParticipacao2:
                    query = query.Where(x => x.TipoDeParticipacao2 != null).SortBy(x => x.TipoDeParticipacao2.Descricao, ascending);
                    break;
                case TipoProcedimentoSort.EhOrgao1:
                    query = query.SortBy(x => x.IndOrgao1.GetValueOrDefault() , ascending);
                    break;
                case TipoProcedimentoSort.EhOrgao2:
                    query = query.SortBy(x => x.IndOrgao2.GetValueOrDefault(), ascending);
                    break;        
                case TipoProcedimentoSort.PoloPassivoUnico:                   
                        query = query.SortBy(x => x.IndPoloPassivoUnico, ascending);                    
                    break;
                case TipoProcedimentoSort.Provisionado:                    
                     query = query.SortBy(x => x.IndProvisionado, ascending);                   
                    break;
                case TipoProcedimentoSort.Descricao:
                default:
                    query = query.SortBy(x => x.Descricao, ascending);
                    break;
            }

            var visualizaCivelAdministrativo = UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_PROCEDIMENTO_CIVEL_ADMINISTRATIVO);
            var visualizaTrabalhistaAdministrativo = UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_PROCEDIMENTO_TRABALHISTA_ADMINISTRATIVO);
            var visualizaTributarioAdministrativo = UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_PROCEDIMENTO_TRIBUTARIO_ADMINISTRATIVO);
            var visualizaAdministrativo = UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_PROCEDIMENTO_ADMINISTRATIVO);
            var visualizaCriminalAdministrativo = UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_PROCEDIMENTO_CRIMINAL_ADMINISTRATIVO);    


            if (tipoProcesso == TipoProcesso.ADMINISTRATIVO)
            {
                query = query.Where(x => visualizaAdministrativo && x.IndAdministrativo.GetValueOrDefault());
            }

            if (tipoProcesso == TipoProcesso.TRABALHISTA_ADMINISTRATIVO)
            {
                query = query.Where(x => visualizaTrabalhistaAdministrativo && x.IndTrabalhistaAdm.GetValueOrDefault());
            }

            if (tipoProcesso == TipoProcesso.TRIBUTARIO_ADMINISTRATIVO)
            {
                query = query.Where(x => visualizaTributarioAdministrativo && x.IndTributario.GetValueOrDefault());
            }

            if (tipoProcesso == TipoProcesso.CIVEL_ADMINISTRATIVO)
            {
                query = query.Where(x => visualizaCivelAdministrativo && x.IndCivelAdm);
            }

            if (tipoProcesso == TipoProcesso.CRIMINAL_ADMINISTRATIVO)
            {
                query = query.Where(x => visualizaCriminalAdministrativo && x.IndCriminalAdm);
            } 

            return query.WhereIfNotNull(x => x.Descricao .ToUpper().Contains(pesquisa.ToString().ToUpper()), pesquisa);
        }

        public CommandResult<PaginatedQueryResult<Procedimento>> ObterPaginado(TipoProcesso tipoProcesso, int pagina, int quantidade,
            TipoProcedimentoSort sort, bool ascending, string pesquisa = null)
        {
            string logName = "Tipo de Procedimento";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_TIPO_PROCEDIMENTO))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_TIPO_PROCEDIMENTO, UsuarioAtual.Login));
                return CommandResult<PaginatedQueryResult<Procedimento>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var listaBase = ObterBase(tipoProcesso, sort, ascending, pesquisa);

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
            var total = listaBase.Count();

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Quantidade de registros para saltar"));
            var skip = Pagination.PagesToSkip(quantidade, total, pagina);

            var resultado = new PaginatedQueryResult<Procedimento>()
            {
                Total = total,
                Data = listaBase.Skip(skip).Take(quantidade).ToArray()
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<PaginatedQueryResult<Procedimento>>.Valid(resultado);
        }

        public CommandResult<IReadOnlyCollection<Procedimento>> Obter(TipoProcesso tipoProcesso, TipoProcedimentoSort sort, bool ascending, string pesquisa = null)
        {
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_TIPO_PROCEDIMENTO))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_TIPO_PROCEDIMENTO, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<Procedimento>>.Forbidden();
            }

            string logName = "Tipo de Procedimento";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var resultado = ObterBase(tipoProcesso, sort, ascending, pesquisa).ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<Procedimento>>.Valid(resultado);
        }

        public CommandResult<IReadOnlyCollection<Procedimento>> ObterTodos()
        {
            string logName = "Tipo de Procedimento";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var result = DatabaseContext.Procedimentos
                                        .AsNoTracking()
                                        .ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<Procedimento>>.Valid(result);
        }

        public CommandResult<IEnumerable<TipoProcesso>> ObterComboboxTipoProcesso()
        {
            List<TipoProcesso> tiposProcesso = new List<TipoProcesso>();

            if (UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_PROCEDIMENTO_ADMINISTRATIVO))
                tiposProcesso.Add(TipoProcesso.ADMINISTRATIVO);
            if (UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_PROCEDIMENTO_TRABALHISTA_ADMINISTRATIVO))
                tiposProcesso.Add(TipoProcesso.TRABALHISTA_ADMINISTRATIVO);
            if (UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_PROCEDIMENTO_TRIBUTARIO_ADMINISTRATIVO))
                tiposProcesso.Add(TipoProcesso.TRIBUTARIO_ADMINISTRATIVO);
            if (UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_PROCEDIMENTO_CRIMINAL_ADMINISTRATIVO))
                tiposProcesso.Add(TipoProcesso.CRIMINAL_ADMINISTRATIVO);
            if (UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_PROCEDIMENTO_CIVEL_ADMINISTRATIVO))
                tiposProcesso.Add(TipoProcesso.CIVEL_ADMINISTRATIVO);          

            return CommandResult<IEnumerable<TipoProcesso>>.Valid(tiposProcesso);

        }

    }
}
