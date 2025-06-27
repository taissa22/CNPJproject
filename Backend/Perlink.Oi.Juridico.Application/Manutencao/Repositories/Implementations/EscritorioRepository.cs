using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Constants;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.External;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System.Collections.Generic;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories.Implementations
{
    internal class EscritorioRepository : IEscritorioRepository
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IEscritorioRepository> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public EscritorioRepository(IDatabaseContext databaseContext, ILogger<IEscritorioRepository> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        private IQueryable<Escritorio> ObterBase(string estado, int areaAtuacao, EscritorioSort sort, bool ascending, string pesquisa)
        {
            string logName = "Escritório";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Ordenar {logName}"));

            var civel = areaAtuacao == (int)TipoProcessoEnum.CivelConsumidor;
            var juizado = areaAtuacao == (int)TipoProcessoEnum.JuizadoEspecial;
            var regulatorio = areaAtuacao == (int)TipoProcessoEnum.Administrativo;
            var trabalhista = areaAtuacao == (int)TipoProcessoEnum.Trabalhista;
            var tributario = areaAtuacao == (int)TipoProcessoEnum.TributarioAdministrativo;
            var civelAdm = areaAtuacao == (int)TipoProcessoEnum.CivelAdministrativo;
            var civelEstrategico = areaAtuacao == (int)TipoProcessoEnum.CivelEstrategico;
            var criminalAdm = areaAtuacao == (int)TipoProcessoEnum.CriminalAdministrativo;
            var criminalJudicial = areaAtuacao == (int)TipoProcessoEnum.CriminalJudicial;
            var pex = areaAtuacao == (int)TipoProcessoEnum.Pex;
            var procon = areaAtuacao == (int)TipoProcessoEnum.Procon;

            IQueryable<Escritorio> query = (from a in DatabaseContext.Escritorios
                                            where (areaAtuacao <= 0 ||

                                                     (juizado && a.IndAreaJuizado == juizado) ||
                                                     (civel && a.IndAreaCivel == civel) ||
                                                     (regulatorio && a.IndAreaRegulatoria == regulatorio) ||
                                                     (tributario && a.IndAreaTributaria == tributario) ||
                                                     (trabalhista && a.IndAreaTrabalhista == trabalhista) ||
                                                     (procon && a.IndAreaProcon == procon) ||
                                                     (pex && a.IndAreaPEX == pex) ||
                                                     (criminalAdm && a.IndAreaCriminalAdministrativo == criminalAdm) ||
                                                     (criminalJudicial && a.IndAreaCriminalJudicial == criminalJudicial) ||
                                                     (civelEstrategico && a.IndAreaCriminalJudicial == civelEstrategico) ||
                                                     (tributario && a.IndAreaTributaria == tributario)
                                                  )
                                            select a).AsNoTracking();

            switch (sort)
            {
                case EscritorioSort.Id:
                    query = query.SortBy(a => a.Id, ascending);
                    break;

                case EscritorioSort.Nome:
                    query = query.SortBy(a => a.Nome, ascending);
                    break;

                case EscritorioSort.CPFCPNJ:
                    query = query.SortBy(a => a.CPF, ascending);
                    break;

                case EscritorioSort.Ativo:
                    query = query.SortBy(a => a.Ativo, ascending);
                    break;

                case EscritorioSort.TipoPessoa:
                    query = query.SortBy(a => a.TipoPessoaValor, ascending);
                    break;
            }

            query = query.WhereIfNotNull(x => x.Nome.ToUpper().Contains(pesquisa.ToUpper()), pesquisa);
            query = query.WhereIfNotNull(x => x.EstadoId == estado, estado);

            return query;
        }

        public CommandResult<PaginatedQueryResult<Escritorio>> ObterPaginado(string estado, int areaAtuacao, int pagina, int quantidade, EscritorioSort sort, bool ascending, string pesquisa)
        {
            string logName = "Escritório";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ESCRITORIO))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ESCRITORIO, UsuarioAtual.Login));
                return CommandResult<PaginatedQueryResult<Escritorio>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var listaBase = ObterBase(estado, areaAtuacao, sort, ascending, pesquisa);

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
            var total = listaBase.Count();

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Quantidade de registros para saltar"));
            var skip = Pagination.PagesToSkip(quantidade, total, pagina);

            var resultado = new PaginatedQueryResult<Escritorio>()
            {
                Total = total,
                Data = listaBase.Skip(skip).Take(quantidade).ToArray()
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<PaginatedQueryResult<Escritorio>>.Valid(resultado);
        }

        public CommandResult<IReadOnlyCollection<Escritorio>> Obter(string estado, int areaAtuacao, EscritorioSort sort, bool ascending, string pesquisa)
        {
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ESCRITORIO))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ESCRITORIO, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<Escritorio>>.Forbidden();
            }

            string logName = "Escritório";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var resultado = ObterBase(estado, areaAtuacao, sort, ascending, pesquisa).ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<Escritorio>>.Valid(resultado);
        }
    }
}