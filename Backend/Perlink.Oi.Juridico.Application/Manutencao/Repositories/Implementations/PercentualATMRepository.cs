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
    public class PercentualATMRepository : IPercentualATMRepository
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IPercentualATMRepository> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }
        private IParametroJuridicoProvider ParametroJuridico { get; }

        public PercentualATMRepository(IDatabaseContext databaseContext, IParametroJuridicoProvider parametroJuridico, ILogger<IPercentualATMRepository> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
            ParametroJuridico = parametroJuridico;
        }

        public CommandResult<PaginatedQueryResult<PercentualATM>> ObterPaginadoPercentualATM(int pagina, int quantidade,
             PercentualATMSort sort, bool ascending, DateTime dataVigencia,  int codTipoProcesso)
        {
            string logName = "% ATM";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_PERCENTUAL_ATM))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_PERCENTUAL_ATM, UsuarioAtual.Login));
                return CommandResult<PaginatedQueryResult<PercentualATM>>.Forbidden();
            }

            if (dataVigencia.Equals(DateTime.MinValue.AddYears(1900)))
            {
                dataVigencia = this.RecuperarUltimaVigenciaCadastrada(codTipoProcesso);
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var listaBase = ObterBase(sort, ascending, dataVigencia, codTipoProcesso);

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
            var total = listaBase.Count();

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Quantidade de registros para saltar"));
            var skip = Pagination.PagesToSkip(quantidade, total, pagina);

            var resultado = new PaginatedQueryResult<PercentualATM>()
            {
                Total = total,
                Data = listaBase.Skip(skip).Take(quantidade).ToArray()
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<PaginatedQueryResult<PercentualATM>>.Valid(resultado);
        }

        public CommandResult<IReadOnlyCollection<PercentualATM>> Obter(PercentualATMSort sort, bool ascending, DateTime dataVigencia, int codTipoProcesso)
        {
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_PERCENTUAL_ATM))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_PERCENTUAL_ATM, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<PercentualATM>>.Forbidden();
            }

            string logName = "% ATM";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            if (dataVigencia.Equals(DateTime.MinValue))
            {
                dataVigencia = this.RecuperarUltimaVigenciaCadastrada(codTipoProcesso);
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var resultado = ObterBase(sort, ascending, dataVigencia, codTipoProcesso).ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<PercentualATM>>.Valid(resultado);
        }

        private IQueryable<PercentualATM> ObterBase(PercentualATMSort sort, bool ascending, DateTime dataVigencia, int codTipoProcesso)
        {
            IQueryable<PercentualATM> query = DatabaseContext.PercentualATM.AsNoTracking();

            string logName = "% de ATM";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Ordenar {logName}"));
            switch (sort)
            {
                case PercentualATMSort.Percentual:
                    query = query.SortBy(a => a.Percentual, ascending).ThenSortBy(a => a.EstadoId, ascending);
                    break;

                default:
                    query = query.SortBy(x => x.EstadoId, ascending).ThenSortBy(x => x.EstadoId, ascending);
                    break;
            }

            return query.Where(x => x.DataVigencia == dataVigencia && x.CodTipoProcesso == codTipoProcesso);
        }

        public CommandResult<bool> ExistePercentualParaVigencia(DateTime dataVigencia, int codTipoProcesso)
        {
            return CommandResult<bool>.Valid(DatabaseContext.PercentualATM
                                                            .AsNoTracking()
                                                            .Any(x => x.DataVigencia == dataVigencia && x.CodTipoProcesso == codTipoProcesso));
        }

        public PercentualATM RecuperarVigenciaParaUF(string estado, DateTime dataVigencia, int codTipoProcesso)
        {
            var percentualATM = DatabaseContext.PercentualATM.FirstOrDefault(x => x.DataVigencia == dataVigencia && 
                                                                                                                        x.EstadoId == estado && 
                                                                                                                        x.CodTipoProcesso == codTipoProcesso);
            return percentualATM;
        }

        public DateTime RecuperarUltimaVigenciaCadastrada(int codTipoProcesso)
        {
            var ultimaVigencia = DatabaseContext.PercentualATM.Where(x => x.CodTipoProcesso == codTipoProcesso).OrderByDescending(x => x.DataVigencia).FirstOrDefault();
            if (ultimaVigencia != null)
                return ultimaVigencia.DataVigencia;
            return DateTime.Now;
        }

        public CommandResult<IReadOnlyCollection<DateTime>> ObterComboVigencias(int codTipoProcesso)
        {
            return CommandResult<IReadOnlyCollection<DateTime>>.Valid(DatabaseContext.PercentualATM.Where(x => x.CodTipoProcesso == codTipoProcesso).Select(x => x.DataVigencia).Distinct().OrderByDescending(x => x).ToArray());
        }
    }
}