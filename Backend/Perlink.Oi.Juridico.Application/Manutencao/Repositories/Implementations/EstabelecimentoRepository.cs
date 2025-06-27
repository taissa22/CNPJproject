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
using System.Collections.Generic;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories.Implementations
{
    internal class EstabelecimentoRepository : IEstabelecimentoRepository
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IEstabelecimentoRepository> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public EstabelecimentoRepository(IDatabaseContext context, ILogger<IEstabelecimentoRepository> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = context;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        private IQueryable<Estabelecimento> ObterBase(EstabelecimentoSort sort, bool ascending, string nome)
        {
            IQueryable<Estabelecimento> query = DatabaseContext.Estabelecimentos.AsNoTracking();

            string logName = "Estabelecimentos";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Ordenar {logName}"));
            switch (sort)
            {
                case EstabelecimentoSort.CNPJ:
                    query = query.SortBy(a => a.CNPJ, ascending);
                    break;

                case EstabelecimentoSort.Endereco:
                    if (ascending)
                    {
                        query = query.OrderBy(a => a.Endereco == null ? 0 : 1).ThenBy(a => a.Endereco);
                    }
                    else
                    {
                        query = query.OrderByDescending(a => a.Endereco == null ? 0 : 1).ThenByDescending(a => a.Endereco);
                    }
                    break;

                case EstabelecimentoSort.Bairro:
                    if (ascending)
                    {
                        query = query.OrderBy(a => a.Bairro == null ? 0 : 1).ThenBy(a => a.Bairro);
                    }
                    else
                    {
                        query = query.OrderByDescending(a => a.Bairro == null ? 0 : 1).ThenByDescending(a => a.Bairro);
                    }
                    break;

                case EstabelecimentoSort.Cidade:
                    if (ascending)
                    {
                        query = query.OrderBy(a => a.Cidade == null ? 0 : 1).ThenBy(a => a.Cidade);
                    }
                    else
                    {
                        query = query.OrderByDescending(a => a.Cidade == null ? 0 : 1).ThenByDescending(a => a.Cidade);
                    }
                    break;

                case EstabelecimentoSort.CEP:
                    if (ascending)
                    {
                        query = query.OrderBy(a => a.CEP == null ? 0 : 1).ThenBy(a => a.Id);
                    }
                    else
                    {
                        query = query.OrderByDescending(a => a.CEP == null ? 0 : 1).ThenByDescending(a => a.CEP);
                    }
                    break;

                case EstabelecimentoSort.Estado:
                    if (ascending)
                    {
                        query = query.OrderBy(a => a.EstadoId == null ? 0 : 1).ThenBy(a => a.EstadoId);
                    }
                    else
                    {
                        query = query.OrderByDescending(a => a.EstadoId == null ? 0 : 1).ThenByDescending(a => a.EstadoId);
                    }
                    break;

                case EstabelecimentoSort.Nome:
                default:
                    query = query.SortBy(x => x.Nome, ascending);
                    break;
            }

            return query.WhereIfNotNull(x => x.Nome.ToString().ToUpper().Contains(nome.ToString().ToUpper()), nome);
        }

        public CommandResult<PaginatedQueryResult<Estabelecimento>> ObterPaginado(int pagina, int quantidade,
            EstabelecimentoSort sort, bool ascending, string nome)
        {
            string logName = "Estabelecimentos";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ESTABELECIMENTO))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ESTABELECIMENTO, UsuarioAtual.Login));
                return CommandResult<PaginatedQueryResult<Estabelecimento>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var listaBase = ObterBase(sort, ascending, nome);

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
            var total = listaBase.Count();

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Quantidade de registros para saltar"));
            var skip = Pagination.PagesToSkip(quantidade, total, pagina);

            var resultado = new PaginatedQueryResult<Estabelecimento>()
            {
                Total = total,
                Data = listaBase.Skip(skip).Take(quantidade).ToArray()
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<PaginatedQueryResult<Estabelecimento>>.Valid(resultado);
        }

        public CommandResult<IReadOnlyCollection<Estabelecimento>> Obter(EstabelecimentoSort sort, bool ascending, string nome)
        {
            string logName = "Estabelecimentos";

            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Ordenar {logName}"));

            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ESTABELECIMENTO))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ESTABELECIMENTO, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<Estabelecimento>>.Forbidden();
            }

            var resultado = ObterBase(sort, ascending, nome).ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<Estabelecimento>>.Valid(resultado);
        }
    }
}