using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Application.Manutencao.Results.Acoes;
using Perlink.Oi.Juridico.Application.Manutencao.Results.Assunto;
using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Constants;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.External;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories.Implementations
{
    internal class AcaoRepository : IAcaoRepository
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IAcaoRepository> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public AcaoRepository(IDatabaseContext databaseContext, ILogger<IAcaoRepository> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        #region Civel Estratégico

        private IQueryable<AcoesEstrategicoCommandResult> ObterBaseDoCivelEstrategico(AcaoCivelEstrategicoSort sort, bool ascending, string pesquisa)
        {
            string logName = "Ações - Cível Estratégico";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Ordenar {logName}"));          

            var listaEstrategicoMigracao = DatabaseContext.Acoes.Where(x => x.EhCivelConsumidor).Select(y => new { y.Id, y.Descricao, y.Ativo }).AsNoTracking().ToArray();

            IQueryable<AcoesEstrategicoCommandResult> query = from a in DatabaseContext.Acoes.AsNoTracking()
                                                   join ma in DatabaseContext.MigracaoAcao on a.Id equals ma.CodAcaoEstrategico into LeftJoinMa
                                                   from ma in LeftJoinMa.DefaultIfEmpty()
                                                   where a.EhCivelEstrategico
                                                   select new AcoesEstrategicoCommandResult(
                                                       a.Id,
                                                       a.Descricao,
                                                       a.Ativo,
                                                       (int?)ma.CodAcaoConsumidor == null ? false : listaEstrategicoMigracao.FirstOrDefault(z => z.Id == ma.CodAcaoConsumidor) != null ? listaEstrategicoMigracao.FirstOrDefault(z => z.Id == ma.CodAcaoConsumidor).Ativo : false,                                                       
                                                       a.EhCivelEstrategico,
                                                       a.EhCivelConsumidor,
                                                       a.EhTrabalhista,
                                                       a.EhTributaria,
                                                       (int?)ma.CodAcaoConsumidor,
                                                       (int?)ma.CodAcaoConsumidor == null ? null : listaEstrategicoMigracao.FirstOrDefault(z => z.Id == ma.CodAcaoConsumidor).Descricao

                                                   );


            switch (sort)
            {
                case AcaoCivelEstrategicoSort.Id:
                    query = query.SortBy(a => a.Id, ascending);
                    break;

                case AcaoCivelEstrategicoSort.Ativo:
                    if (ascending)
                    {
                        query = query.OrderBy(a => a.Ativo).ThenBy(a => a.Id);
                    }
                    else
                    {
                        query = query.OrderByDescending(a => a.Ativo).ThenByDescending(a => a.Id);
                    }
                    break;

                case AcaoCivelEstrategicoSort.Descricao:
                default:
                    query = query.SortBy(a => a.Descricao, ascending);
                    break;
            }

            return query.WhereIfNotNull(x => x.Descricao.ToString().ToUpper().Contains(pesquisa.ToString().ToUpper()), pesquisa);
        }

        public CommandResult<PaginatedQueryResult<AcoesEstrategicoCommandResult>> ObterPaginadoDoCivelEstrategico(int pagina, int quantidade, AcaoCivelEstrategicoSort sort, bool ascending, string pesquisa)
        {
            
            string logName = "Ações - Cível Estratégico";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ACAO_CIVEL_ESTRATEGICO))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ACAO_CIVEL_ESTRATEGICO, UsuarioAtual.Login));
                return CommandResult<PaginatedQueryResult<AcoesEstrategicoCommandResult>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var listaBase = ObterBaseDoCivelEstrategico(sort, ascending, pesquisa);

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
            var total = listaBase.Count();

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Quantidade de registros para saltar"));
            var skip = Pagination.PagesToSkip(quantidade, total, pagina);

            var resultado = new PaginatedQueryResult<AcoesEstrategicoCommandResult>()
            {
                Total = total,
                Data = listaBase.Skip(skip).Take(quantidade).ToArray()
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<PaginatedQueryResult<AcoesEstrategicoCommandResult>>.Valid(resultado);
        }    

        public CommandResult<IReadOnlyCollection<AcoesEstrategicoCommandResult>> ObterDoCivelEstrategico(AcaoCivelEstrategicoSort sort, bool ascending, string pesquisa)
        {
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ACAO_CIVEL_ESTRATEGICO)) {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ACAO_CIVEL_ESTRATEGICO, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<AcoesEstrategicoCommandResult>>.Forbidden();
            }

            string logName = "Ações - Cível Estratégico";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));
        
            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var resultado = ObterBaseDoCivelEstrategico(sort, ascending, pesquisa).ToArray();           

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<AcoesEstrategicoCommandResult>>.Valid(resultado);
        }

        #endregion Civel Estratégico

        #region Civel Consumidor

        private IQueryable<AcoesCommandResult> ObterBaseDoCivelConsumidor(AcaoCivelConsumidorSort sort, bool ascending, string pesquisa)
        {
            var listaEstrategicoMigracao = DatabaseContext.Acoes.Where(x => x.EhCivelEstrategico).Select(y => new { y.Id, y.Descricao, y.Ativo }).AsNoTracking().ToArray(); 

            IQueryable<AcoesCommandResult> query = from a in DatabaseContext.Acoes.AsNoTracking().Include(x => x.NaturezaAcaoBB)
                                                     join ma in DatabaseContext.MigracaoAcaoConsumidor on a.Id equals ma.CodAcaoCivel into LeftJoinMa
                                                     from ma in LeftJoinMa.DefaultIfEmpty()
                                                     where a.EhCivelConsumidor
                                                     select new AcoesCommandResult(
                                                         a.Id,
                                                         a.Descricao,
                                                         a.Ativo,
                                                         (int?)ma.CodAcaoCivelEstrategico == null ? false : listaEstrategicoMigracao.FirstOrDefault(z => z.Id == ma.CodAcaoCivelEstrategico) != null ? listaEstrategicoMigracao.FirstOrDefault(z => z.Id == ma.CodAcaoCivelEstrategico).Ativo : false,                                                                                                            
                                                         a.NaturezaAcaoBB,
                                                         a.EhCivelEstrategico,
                                                         a.EhCivelConsumidor,
                                                         a.EhTrabalhista,
                                                         a.EhTributaria,
                                                         (int?)ma.CodAcaoCivelEstrategico,
                                                         (int?)ma.CodAcaoCivelEstrategico == null ? null : listaEstrategicoMigracao.FirstOrDefault(z => z.Id == ma.CodAcaoCivelEstrategico).Descricao                                                       ,
                                                         a.EnviarAppPreposto
                                                   
                                                     );

          


            string logName = "Ações - Cível Consumidor";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Ordenar {logName}"));

            switch (sort)
            {
                case AcaoCivelConsumidorSort.Id:
                    query = query.SortBy(a => a.Id, ascending);
                    break;

                case AcaoCivelConsumidorSort.Nomenatureza:
                    if (ascending)
                    {
                        query = query.OrderBy(a => a.NaturezaAcaoBB.Nome == null ? 0 : 1).ThenBy(a => a.NaturezaAcaoBB.Nome);
                    }
                    else
                    {
                        query = query.OrderByDescending(a => a.NaturezaAcaoBB.Nome == null ? 0 : 1).ThenByDescending(a => a.NaturezaAcaoBB.Nome);
                    }
                    break;

                case AcaoCivelConsumidorSort.Descricao:
                default:
                    query = query.SortBy(a => a.Descricao, ascending);
                    break;
            }

            return query.WhereIfNotNull(x => x.Descricao.ToUpper().ToString().Contains(pesquisa.ToString().ToUpper()), pesquisa);
        }


        public CommandResult<PaginatedQueryResult<AcoesCommandResult>> ObterPaginadoDoCivelConsumidor(int pagina, int quantidade, AcaoCivelConsumidorSort sort, bool ascending, string pesquisa)
        {
            string logName = "Ações - Cível Consumidor";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ACAO_CIVEL_CONSUMIDOR))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ACAO_CIVEL_CONSUMIDOR, UsuarioAtual.Login));
                return CommandResult<PaginatedQueryResult<AcoesCommandResult>>.Forbidden();
            }
            
            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var listaBase = ObterBaseDoCivelConsumidor(sort, ascending, pesquisa);

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
            var total = listaBase.Count();

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Quantidade de registros para saltar"));
            var skip = Pagination.PagesToSkip(quantidade, total, pagina);

            var resultado = new PaginatedQueryResult<AcoesCommandResult>()
            {
                Total = total,
                Data = listaBase.Skip(skip).Take(quantidade).ToArray()
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<PaginatedQueryResult<AcoesCommandResult>>.Valid(resultado);
        }

        public CommandResult<IReadOnlyCollection<AcoesCommandResult>> ObterDoCivelConsumidor(AcaoCivelConsumidorSort sort, bool ascending, string pesquisa)
        {
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ACAO_CIVEL_CONSUMIDOR)) {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ACAO_CIVEL_CONSUMIDOR, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<AcoesCommandResult>>.Forbidden();
            }

            string logName = "Ações - Cível Consumidor";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));       

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var resultado = ObterBaseDoCivelConsumidor(sort, ascending, pesquisa).ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<AcoesCommandResult>>.Valid(resultado);
        }

        #endregion Civel Consumidor

        #region Trabalhista

        private IQueryable<Acao> ObterBaseDoTrabalhista(AcaoTrabalhistaSort sort, bool ascending, string pesquisa)
        {
            string logName = "Ações - Trabalhista";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Ordenar {logName}"));
            IQueryable<Acao> query = DatabaseContext.Acoes.AsNoTracking().Where(x => x.EhTrabalhista);

            switch (sort)
            {
                case AcaoTrabalhistaSort.Id:
                    query = query.SortBy(a => a.Id, ascending);
                    break;

                case AcaoTrabalhistaSort.Descricao:
                default:
                    query = query.SortBy(a => a.Descricao, ascending);
                    break;                    
            }           

            return query.WhereIfNotNull(x => x.Descricao.ToString().ToUpper().Contains(pesquisa.ToString().ToUpper()), pesquisa);
        }

        public CommandResult<PaginatedQueryResult<Acao>> ObterPaginadoDoTrabalhista(int pagina, int quantidade, AcaoTrabalhistaSort sort, bool ascending, string pesquisa)
        {
            string logName = "Ações - Trabalhista";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ACAO_TRABALHISTA))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ACAO_TRABALHISTA, UsuarioAtual.Login));
                return CommandResult<PaginatedQueryResult<Acao>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var listaBase = ObterBaseDoTrabalhista(sort, ascending, pesquisa);

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
            var total = listaBase.Count();

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Quantidade de registros para saltar"));
            var skip = Pagination.PagesToSkip(quantidade, total, pagina);

            var resultado = new PaginatedQueryResult<Acao>()
            {
                Total = total,
                Data = listaBase.Skip(skip).Take(quantidade).ToArray()
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<PaginatedQueryResult<Acao>>.Valid(resultado);
        }

        public CommandResult<IReadOnlyCollection<Acao>> ObterDoTrabalhista(AcaoTrabalhistaSort sort, bool ascending, string pesquisa)
        {
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ACAO_TRABALHISTA)) {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ACAO_TRABALHISTA, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<Acao>>.Forbidden();
            }

            string logName = "Ações - Trabalhista";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));      

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var resultado = ObterBaseDoTrabalhista(sort, ascending, pesquisa).ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<Acao>>.Valid(resultado);
        }

        #endregion Trabalhista

        #region Tributario Judicial

        private IQueryable<Acao> ObterBaseDoTributarioJudicial(AcaoTributarioJudicialSort sort, bool ascending, string pesquisa)
        {
            string logName = "Ações - Tributário Judicial";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Ordenar {logName}"));
            IQueryable<Acao> query = DatabaseContext.Acoes.AsNoTracking().Where(x => x.EhTributaria);

            switch (sort)
            {
                case AcaoTributarioJudicialSort.Id:                    
                    query = query.SortBy(a => a.Id, ascending);                    
                    break;

                case AcaoTributarioJudicialSort.Descricao:
                default:                    
                    query = query.SortBy(a => a.Descricao, ascending);                    
                    break;
            }
      
            return query.WhereIfNotNull(x => x.Descricao.ToString().ToUpper().Contains(pesquisa.ToString().ToUpper()), pesquisa);
        }

        public CommandResult<PaginatedQueryResult<Acao>> ObterPaginadoDoTributarioJudicial(int pagina, int quantidade, AcaoTributarioJudicialSort sort, bool ascending, string pesquisa)
        {
            string logName = "Ações - Tributário Judicial";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ACAO_TRIBUTARIA_JUDICIAL))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ACAO_TRIBUTARIA_JUDICIAL, UsuarioAtual.Login));
                return CommandResult<PaginatedQueryResult<Acao>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var listaBase = ObterBaseDoTributarioJudicial(sort, ascending, pesquisa);

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
            var total = listaBase.Count();

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Quantidade de registros para saltar"));
            var skip = Pagination.PagesToSkip(quantidade, total, pagina);

            var resultado = new PaginatedQueryResult<Acao>()
            {
                Total = total,
                Data = listaBase.Skip(skip).Take(quantidade).ToArray()
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<PaginatedQueryResult<Acao>>.Valid(resultado);
        }           
      
        public CommandResult<IReadOnlyCollection<Acao>> ObterDoTributarioJudicial(AcaoTributarioJudicialSort sort, bool ascending, string pesquisa)
        {
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ACAO_TRIBUTARIA_JUDICIAL)) {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ACAO_TRIBUTARIA_JUDICIAL, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<Acao>>.Forbidden();
            }

            string logName = "Ações - Tributário Judicial";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));      

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var resultado = ObterBaseDoTributarioJudicial(sort, ascending, pesquisa).ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<Acao>>.Valid(resultado);
        }

        public CommandResult<IReadOnlyCollection<Acao>> ObterDescricaoDeParaCivelEstrategico()
        {
            IQueryable<Acao> query = DatabaseContext.Acoes.AsNoTracking().Where(x => x.EhCivelEstrategico);


            return CommandResult<IReadOnlyCollection<Acao>>.Valid(query.ToArray());
        }

        public CommandResult<IReadOnlyCollection<Acao>> ObterDescricaoDeParaConsumidor()
        {
            IQueryable<Acao> query = DatabaseContext.Acoes.AsNoTracking().Where(x => x.EhCivelConsumidor);


            return CommandResult<IReadOnlyCollection<Acao>>.Valid(query.ToArray());
        }

        #endregion Tributario Judicial
    }
}