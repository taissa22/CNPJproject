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
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories.Implementations
{
    public class MunicipioRepository : IMunicipioRepository
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IMunicipioRepository> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public MunicipioRepository(IDatabaseContext databaseContext, ILogger<IMunicipioRepository> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        private IQueryable<Municipio> ObterBase(MunicipioSort sort, bool ascending, string estadoId, int? municipioId)
        {
            string logName = nameof(Municipio);
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Ordenar {logName}"));
            IQueryable<Municipio> query = DatabaseContext.Municipios.AsNoTracking();

            switch (sort)
            {
                case MunicipioSort.Nome:
                    query = query.SortBy(a => a.Nome, ascending);
                    break;

                case MunicipioSort.Codigo:
                    query = query.SortBy(a => a.Id, ascending);
                    break;

                case MunicipioSort.Estado:
                    query = query.SortBy(a => a.EstadoId, ascending);
                    break;

                default:
                    query = query.SortBy(a => a.Nome, ascending);
                    break;
            }


            return query.Where(x => x.EstadoId == estadoId).WhereIfNotNull(x => x.Id == municipioId, municipioId);
        }


        public CommandResult<PaginatedQueryResult<Municipio>> ObterPaginado(int pagina, int quantidade, string estadoId, int? municipioId, MunicipioSort sort, bool ascending)
        {
            string logName = nameof(Municipio);
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ESTADO))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ESTADO, UsuarioAtual.Login));
                return CommandResult<PaginatedQueryResult<Municipio>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var listaBase = ObterBase(sort, ascending, estadoId, municipioId);

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
            var total = listaBase.Count();

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Quantidade de registros para saltar"));
            var skip = Pagination.PagesToSkip(quantidade, total, pagina);

            var resultado = new PaginatedQueryResult<Municipio>()
            {
                Total = total,
                Data = listaBase.Skip(skip).Take(quantidade).ToArray()
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<PaginatedQueryResult<Municipio>>.Valid(resultado);
        }

        public CommandResult<IReadOnlyCollection<Municipio>> ObterTodos(string estadoId)
        {
            string logName = nameof(Municipio);
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ESTADO))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ESTADO, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<Municipio>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var listaBase = ObterBase(MunicipioSort.Nome, true, estadoId,null);
       
            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<Municipio>>.Valid(listaBase.ToArray());
        }
    }
}
