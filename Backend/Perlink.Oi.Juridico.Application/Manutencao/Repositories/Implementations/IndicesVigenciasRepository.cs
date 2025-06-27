using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Constants;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.External;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories.Implementations
{
    public class IndicesVigenciasRepository : IIndicesVigenciasRepository
    {
        public IndicesVigenciasRepository(IDatabaseContext databaseContext, ILogger<IIndicesVigenciasRepository> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IIndicesVigenciasRepository> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public CommandResult<bool> InserirIndiceVivencia(IndiceCorrecaoProcesso indiceVigencia)
        {
            throw new NotImplementedException();
        }

        public CommandResult<PaginatedQueryResult<IndiceCorrecaoProcesso>> ObterPaginado(int pagina, int quantidade, IndicesVigenciaSort sort, bool ascending, int tipoProcesso, string pesquisa = null, int vigencia = 0, bool exportar = false)
        {
            var teste = DatabaseContext.IndiceVigencias.AsNoTracking();
            string logName = "Indice";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_INDICE))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_INDICE, UsuarioAtual.Login));
                return CommandResult<PaginatedQueryResult<IndiceCorrecaoProcesso>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var listaBase = ObterBase(sort, ascending,tipoProcesso, pesquisa, vigencia);

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
            var total = listaBase.Count();

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Quantidade de registros para saltar"));
            var skip = Pagination.PagesToSkip(quantidade, total, pagina);

            var resultado = new PaginatedQueryResult<IndiceCorrecaoProcesso>()
            {
                Total = total,
                Data = listaBase.Skip(skip).Take(quantidade).ToArray()
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<PaginatedQueryResult<IndiceCorrecaoProcesso>>.Valid(resultado);
        }

        private IQueryable<IndiceCorrecaoProcesso> ObterBase(IndicesVigenciaSort sort, bool ascending, DateTime inicio, DateTime fim, int tipoProcesso, string pesquisa = null, int vigencia = 0)
        {
            string logName = "Indice";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Ordenar {logName}"));
            IQueryable<IndiceCorrecaoProcesso> query = DatabaseContext.IndiceVigencias
                .Where(s => s.ProcessoId == tipoProcesso)
                .Where(s => s.DataVigencia <= fim.AddDays(1))
                .Where(s => s.DataVigencia >= inicio)
                .Include(x => x.Indice).AsNoTracking();
            if(vigencia > 0)
            {
                query = query.Where(d => d.IndiceId == vigencia);
            }

            switch (sort)
            {
                case IndicesVigenciaSort.Indice:
                    query = query.SortBy(a => a.Indice, ascending);
                    break;

                default:
                    query = query.SortBy(a => a.DataVigencia, ascending);
                    break;
            }

            return query.WhereIfNotNull(x => x.ProcessoId.ToString().ToUpper().Contains(pesquisa.ToString().ToUpper()), pesquisa);
        }

        private IQueryable<IndiceCorrecaoProcesso> ObterBase(IndicesVigenciaSort sort, bool ascending, int tipoProcesso, string pesquisa = null, int vigencia = 0)
        {
            string logName = "Indice";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Ordenar {logName}"));
            IQueryable<IndiceCorrecaoProcesso> query = DatabaseContext.IndiceVigencias
                .Where(s => s.ProcessoId == tipoProcesso)
                .Include(x => x.Indice).AsNoTracking();
            if (vigencia > 0)
            {
                query = query.Where(d => d.IndiceId == vigencia);
            }
            switch (sort)
            {
                case IndicesVigenciaSort.Indice:
                    query = query.SortBy(a => a.Indice, ascending);
                    break;

                default:
                    query = query.SortBy(a => a.DataVigencia, ascending);
                    break;
            }

            return query.WhereIfNotNull(x => x.ProcessoId.ToString().ToUpper().Contains(pesquisa.ToString().ToUpper()), pesquisa);
        }

        public CommandResult<PaginatedQueryResult<IndiceCorrecaoProcesso>> ObterPaginado(int pagina, int quantidade, IndicesVigenciaSort sort, bool ascending, DateTime DataInicio, DateTime DataFim, int tipoProcesso, string pesquisa = null, int vigencia = 0, bool exportar = false)
        {
            string logName = "Indice";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_INDICE))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_INDICE, UsuarioAtual.Login));
                return CommandResult<PaginatedQueryResult<IndiceCorrecaoProcesso>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var listaBase = ObterBase(sort, ascending,DataInicio,DataFim,tipoProcesso,pesquisa, vigencia);

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
            var total = listaBase.Count();

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Quantidade de registros para saltar"));
            var skip = Pagination.PagesToSkip(quantidade, total, pagina);

            var resultado = new PaginatedQueryResult<IndiceCorrecaoProcesso>()
            {
                Total = total,
                Data = listaBase.Skip(skip).Take(quantidade).ToArray()
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<PaginatedQueryResult<IndiceCorrecaoProcesso>>.Valid(resultado);
        }

        public CommandResult<IReadOnlyCollection<IndiceCorrecaoProcesso>> ObterTodos()
        {

            string logName = "Indices";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var result = DatabaseContext.IndiceVigencias.Include(s => s.Indice)
                                        .AsNoTracking()
                                        .ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<IndiceCorrecaoProcesso>>.Valid(result);
        }

        public CommandResult<IReadOnlyCollection<Indice>> ObterInces(int tipoProcesso)
        {
            string logName = "Indices";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            if(tipoProcesso == 0)
            {
               var list =  DatabaseContext.Indices
                                            .AsNoTracking()
                                            .ToArray();
                Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
                return CommandResult<IReadOnlyCollection<Indice>>.Valid(list);
            }
            var result = DatabaseContext.IndiceVigencias.Include(d => d.Indice).Where(p => p.ProcessoId == tipoProcesso)
                                            .AsNoTracking()
                                            .ToArray();
                                            
            var lst = result.Select(d => d.Indice).Distinct().ToList();


            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<Indice>>.Valid(lst);
        }

        CommandResult<IQueryable<IndiceCorrecaoProcesso>> IIndicesVigenciasRepository.ObterBase(IndicesVigenciaSort sort, bool ascending, DateTime inicio, DateTime fim, int tipoProcesso, string pesquisa, int vigencia)
        {
            string logName = "Indice";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_INDICE))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_INDICE, UsuarioAtual.Login));
                return CommandResult<IQueryable<IndiceCorrecaoProcesso>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var listaBase = ObterBase(sort, ascending, inicio, fim, tipoProcesso, pesquisa, vigencia);


            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IQueryable<IndiceCorrecaoProcesso>>.Valid(listaBase);
        }

        CommandResult<IQueryable<IndiceCorrecaoProcesso>> IIndicesVigenciasRepository.ObterBase(IndicesVigenciaSort sort, bool ascending, int tipoProcesso, string pesquisa, int vigencia)
        {
            string logName = "Indice";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_INDICE))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_INDICE, UsuarioAtual.Login));
                return CommandResult<IQueryable<IndiceCorrecaoProcesso>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var listaBase = ObterBase(sort, ascending, tipoProcesso, pesquisa, vigencia);


            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IQueryable<IndiceCorrecaoProcesso>>.Valid(listaBase);
        }
    }
}
