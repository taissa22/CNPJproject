using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Data;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Constants;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.External;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.ValueObjects;
using System.Collections.Generic;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories.Implementations {

    public class EmpresaDoGrupoRepository : IEmpresaDoGrupoRepository {
        private IDatabaseContext DatabaseContext { get; }

        private ILogger<IEmpresaDoGrupoRepository> Logger { get; }

        private IUsuarioAtualProvider UsuarioAtual { get; }

        public EmpresaDoGrupoRepository(IDatabaseContext databaseContext, ILogger<IEmpresaDoGrupoRepository> logger, IUsuarioAtualProvider usuarioAtual) {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        private IQueryable<EmpresaDoGrupo> ObterBase(EmpresaDoGrupoSort sort, bool ascending, DataString? nome, CNPJ? cnpj, DataString? centroSap) {
            IQueryable<EmpresaDoGrupo> query = DatabaseContext.EmpresasDoGrupo;

            switch (sort) {
                case EmpresaDoGrupoSort.Estado:
                    if (ascending) {
                        query = query.OrderBy(x => x.EstadoId == null ? 0 : 1).ThenBy(x => x.EstadoId);
                    } else {
                        query = query.OrderByDescending(x => x.EstadoId == null ? 0 : 1).ThenByDescending(x => x.EstadoId);
                    }
                    break;

                case EmpresaDoGrupoSort.EmpresaCentralizadora:
                    if (ascending) {
                        query = query.OrderBy(x => x.EmpresaCentralizadora.Nome == null ? 0 : 1).ThenBy(x => x.EmpresaCentralizadora.Nome);
                    } else {
                        query = query.OrderByDescending(x => x.EmpresaCentralizadora.Nome == null ? 0 : 1).ThenByDescending(x => x.EmpresaCentralizadora.Nome);
                    }
                    break;

                case EmpresaDoGrupoSort.CodCentroSap:
                    if (ascending) {
                        query = query.OrderBy(x => x.CodCentroSap == null ? 0 : 1).ThenBy(x => x.CodCentroSap);
                    } else {
                        query = query.OrderByDescending(x => x.CodCentroSap == null ? 0 : 1).ThenByDescending(x => x.CodCentroSap);
                    }
                    break;

                case EmpresaDoGrupoSort.CNPJ:
                    if (ascending) {
                        query = query.OrderBy(x => x.CNPJ == null ? 0 : 1).ThenBy(x => x.CNPJ).OrderBy(x => x.CNPJ == null ? 0 : 1).ThenBy(x => x.CNPJ);
                    } else {
                        query = query.OrderByDescending(x => x.CNPJ == null ? 0 : 1).ThenByDescending(x => x.CNPJ).OrderByDescending(x => x.CNPJ == null ? 0 : 1).ThenByDescending(x => x.CNPJ);
                    }
                    break;
                case EmpresaDoGrupoSort.EmpRecuperanda:
                    if (ascending) {
                        query = query.OrderBy(x => x.EmpRecuperanda == null ? 0 : 1).ThenBy(x => x.EmpRecuperanda);
                    } else { 
                        query = query.OrderByDescending(x => x.EmpRecuperanda == null ? 0 : 1).ThenByDescending(x => x.EmpRecuperanda);
                    }
                    break;
                case EmpresaDoGrupoSort.Nome:
                default:
                    query = query.SortBy(x => x.Nome, ascending);
                    break;
            }

            return query
                .AsNoTracking()
                .WhereIfNotNull(x => x.Nome.ToUpper().Contains(nome.ToString()), nome)
                .WhereIfNotNull(x => x.CNPJ.ToUpper().Equals(cnpj.ToString()), cnpj)
                .WhereIfNotNull(x => x.CodCentroSap.ToUpper().Contains(centroSap.ToString()), centroSap);
        }

        public CommandResult<IReadOnlyCollection<EmpresaDoGrupo>> Obter(EmpresaDoGrupoSort sort, bool ascending, DataString? nome, CNPJ? cnpj, DataString? centroSap) {

            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_EMPRESA_DO_GRUPO)) {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_EMPRESA_DO_GRUPO, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<EmpresaDoGrupo>>.Forbidden();
            }

            string logName = "Empresas do Grupo";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var resultado = ObterBase(sort, ascending, nome, cnpj, centroSap).ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<EmpresaDoGrupo>>.Valid(resultado);
        }

        public CommandResult<PaginatedQueryResult<EmpresaDoGrupo>> ObterPaginado(EmpresaDoGrupoSort sort, bool ascending, int pagina, int quantidade, DataString? nome, CNPJ? cnpj, DataString? centroSap) {
            string logName = "Empresas do Grupo";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_EMPRESA_DO_GRUPO)) {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_EMPRESA_DO_GRUPO, UsuarioAtual.Login));
                return CommandResult<PaginatedQueryResult<EmpresaDoGrupo>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var listaBase = ObterBase(sort, ascending, nome, cnpj, centroSap);

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
            var total = listaBase.Count();

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Quantidade de registros para saltar"));
            var skip = Pagination.PagesToSkip(quantidade, total, pagina);

            var resultado = new PaginatedQueryResult<EmpresaDoGrupo>() {
                Total = total,
                Data = listaBase.Skip(skip).Take(quantidade).ToArray()
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<PaginatedQueryResult<EmpresaDoGrupo>>.Valid(resultado);
        }

        public CommandResult<bool> Existe(DataString nome, int? id) {
            string logName = "Partes";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Verificar {logName}"));
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_EMPRESA_DO_GRUPO)) {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_EMPRESA_DO_GRUPO, UsuarioAtual.Login));
                return CommandResult<bool>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));

            bool resultado = DatabaseContext.Partes
                // TODO: .IgnoreQueryFilters()
                .IgnoreQueryFilters()
                .WhereIfNotNull(x => x.Id != id, id)
                .Any(x => x.Nome == nome.ToString());

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<bool>.Valid(resultado);
        }

        public CommandResult<IEnumerable<string>> ObterNomesPorCNPJ(CNPJ cnpj) {
            string logName = "Empresas do Grupo por CNPJ";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Buscando {logName}"));
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_EMPRESA_DO_GRUPO)) {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_EMPRESA_DO_GRUPO, UsuarioAtual.Login));
                return CommandResult<IEnumerable<string>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var empresasComMesmoCNPJ = DatabaseContext.EmpresasDoGrupo
                .Where(x => x.CNPJ == cnpj.ToString())
                .Select(x => x.Nome)
                .ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IEnumerable<string>>.Valid(empresasComMesmoCNPJ);
        }
    }
}