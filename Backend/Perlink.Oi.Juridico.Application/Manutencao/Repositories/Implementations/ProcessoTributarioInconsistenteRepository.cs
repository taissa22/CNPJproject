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
using Perlink.Oi.Juridico.Infra.ValueObjects;
using System.Collections.Generic;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories.Implementations
{
    internal class ProcessoTributarioInconsistenteRepository : IProcessoTributarioInconsistenteRepository
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IProcessoTributarioInconsistenteRepository> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public ProcessoTributarioInconsistenteRepository(IDatabaseContext databaseContext, ILogger<IProcessoTributarioInconsistenteRepository> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        private IQueryable<ProcessoTributarioInconsistente> ObterBase(ProcessoTributarioInconsistenteSort sort, bool ascending)
        {
            string logName = "ProcessoTributarioInconsistente";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Ordenar {logName}"));
            IQueryable<ProcessoTributarioInconsistente> query = DatabaseContext.ProcessoTributarioInconsistentes.AsNoTracking();

            switch (sort)
            {
                case ProcessoTributarioInconsistenteSort.TipoProcesso:
                    query = query.SortBy(a => a.TipoProcesso, ascending);
                    break;

                case ProcessoTributarioInconsistenteSort.CodigoProcesso:
                    query = query.SortBy(a => a.CodigoProcesso, ascending);
                    break;

                case ProcessoTributarioInconsistenteSort.NumeroProcesso:
                    query = query.SortBy(a => a.NumeroProcesso, ascending);
                    break;
        
                case ProcessoTributarioInconsistenteSort.Estado:
                    query = query.SortBy(a => a.Estado, ascending);
                    break;

                case ProcessoTributarioInconsistenteSort.ComarcaOrgao:
                    query = query.SortBy(a => a.ComarcaOrgao, ascending);
                    break;

                case ProcessoTributarioInconsistenteSort.VaraMunicipio:
                    query = query.SortBy(a => a.VaraMunicipio, ascending);
                    break;

                case ProcessoTributarioInconsistenteSort.EmpresadoGrupo:
                    query = query.SortBy(a => a.EmpresadoGrupo, ascending);
                    break;

                case ProcessoTributarioInconsistenteSort.Escritorio:
                    query = query.SortBy(a => a.Escritorio, ascending);
                    break;

                case ProcessoTributarioInconsistenteSort.Objeto:
                    query = query.SortBy(a => a.Objeto, ascending);
                    break;
                case ProcessoTributarioInconsistenteSort.ValorTotalCorrigido:
                    query = query.SortBy(a => a.ValorTotalCorrigido, ascending);
                    break;
                case ProcessoTributarioInconsistenteSort.ValorTotalPago:
                    query = query.SortBy(a => a.ValorTotalPago, ascending);
                    break;
            }
           
            return query;
        }

        public CommandResult<PaginatedQueryResult<ProcessoTributarioInconsistente>> ObterPaginado(int pagina, int quantidade, ProcessoTributarioInconsistenteSort sort, bool ascending)
        {
            string logName = "ProcessoTributarioInconsistente";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));
          
            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var listaBase = ObterBase(sort, ascending);

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
            var total = listaBase.Count();

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Quantidade de registros para saltar"));
            var skip = Pagination.PagesToSkip(quantidade, total, pagina);

            var resultado = new PaginatedQueryResult<ProcessoTributarioInconsistente>()
            {
                Total = total,
                Data = listaBase.Skip(skip).Take(quantidade).ToArray()
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<PaginatedQueryResult<ProcessoTributarioInconsistente>>.Valid(resultado);
        }

        public CommandResult<IReadOnlyCollection<ProcessoTributarioInconsistente>> Obter(ProcessoTributarioInconsistenteSort sort, bool ascending)
        {          

            string logName = "ProcessoTributarioInconsistente";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var resultado = ObterBase(sort, ascending).ToArray();


            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<ProcessoTributarioInconsistente>>.Valid(resultado);
        }
    }
}
