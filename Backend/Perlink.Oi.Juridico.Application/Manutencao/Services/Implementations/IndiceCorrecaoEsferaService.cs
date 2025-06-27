using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;
using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Constants;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Data;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services.Implementations
{
    internal class IndiceCorrecaoEsferaService : IIndiceCorrecaoEsferaService
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IIndiceCorrecaoEsferaService> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public IndiceCorrecaoEsferaService(IDatabaseContext databaseContext, ILogger<IIndiceCorrecaoEsferaService> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        public CommandResult<PaginatedQueryResult<ProcessoInconsistente>> Criar(CriarIndiceCorrecaoEsferaCommand command)
        {
            string entityName = "Indice Correcao Esfera";
            string commandName = $"Criar {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try
            {
                command.Validate();
                if (command.Invalid)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.ComandoInvalido(commandName));
                    return CommandResult<PaginatedQueryResult<ProcessoInconsistente>>.Invalid(command.Notifications.ToNotificationsString());
                }

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ESFERA))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ESFERA, UsuarioAtual.Login));
                    return CommandResult<PaginatedQueryResult<ProcessoInconsistente>>.Forbidden();
                }

                if (DatabaseContext.IndiceCorrecaoEsferas.Any(x => x.EsferaId == command.EsferaId && x.DataVigencia.Date == command.DataVigencia.Date))
                {
                    string mensagem = "índice já cadastrado.";
                    Logger.LogInformation(mensagem);
                    return CommandResult<PaginatedQueryResult<ProcessoInconsistente>>.Invalid(mensagem);
                }

                Logger.LogInformation(Infra.Extensions.Logs.CriandoEntidade(entityName));
                IndiceCorrecaoEsfera indiceCorrecaoEsfera = IndiceCorrecaoEsfera.Criar(command.EsferaId, command.DataVigencia, command.IndiceId);

                if (indiceCorrecaoEsfera.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult<PaginatedQueryResult<ProcessoInconsistente>>.Invalid(indiceCorrecaoEsfera.Notifications.ToNotificationsString());
                }

                DatabaseContext.Add(indiceCorrecaoEsfera);
                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade(entityName));

                var ParametroJuridico = new OracleParameter("O_RESULT_SET", OracleDbType.RefCursor, ParameterDirection.Output);

                string sql;
                sql = $"BEGIN jur.PC_VER_INCONSISTENCIAS_TRIB.SP_VER_INCONSISTENCIAS_TRIB(" + command.EsferaId + ", :O_RESULT_SET  ); END;";

                var listaBase = DatabaseContext.ProcessoInconsistentes.FromSql(sql, ParametroJuridico).AsNoTracking().ToList();

                if (listaBase.Count() > 0)
                {
                    foreach (ProcessoInconsistente item in listaBase)
                    {
                        var ProcTrib = new ProcessoTributarioInconsistente(item.TipoProcesso,
                                                                            item.CodigoProcesso,
                                                                            item.NumeroProcesso,
                                                                            item.Estado,
                                                                            item.ComarcaOrgao,
                                                                            item.VaraMunicipio,
                                                                            item.EmpresadoGrupo,
                                                                            item.Escritorio,
                                                                            item.Objeto,
                                                                            item.Periodo,
                                                                            item.ValorTotalCorrigido,
                                                                            item.ValorTotalPago);

                        DatabaseContext.Add(ProcTrib);
                    }

                    Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
                    var total = listaBase.Count();

                    Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Quantidade de registros para saltar"));
                    var skip = Pagination.PagesToSkip(8, total, 1);

                    var resultado = new PaginatedQueryResult<ProcessoInconsistente>()
                    {
                        Total = total,
                        Data = listaBase.Skip(skip).Take(8).ToArray()
                    };

                    DatabaseContext.SaveChanges();

                    Logger.LogInformation(Infra.Extensions.Logs.OperacaoFinalizada(commandName));
                    return CommandResult<PaginatedQueryResult<ProcessoInconsistente>>.Valid(resultado);
                }


                DatabaseContext.SaveChanges();
                Logger.LogInformation(Infra.Extensions.Logs.OperacaoFinalizada(commandName));

                return CommandResult<PaginatedQueryResult<ProcessoInconsistente>>.Valid(null);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, Infra.Extensions.Logs.OperacaoComErro(commandName));
                return CommandResult<PaginatedQueryResult<ProcessoInconsistente>>.Invalid(ex.Message);
            }
        }
       

        public CommandResult<PaginatedQueryResult<ProcessoInconsistente>> Remover(int codigoEsfera, DateTime dataVigencia, int codigoIndice)
        {
            string entityName = "Indice Correcao Esfera";
            string commandName = $"Removendo {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try
            {
                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ESFERA))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ESFERA, UsuarioAtual.Login));
                    return CommandResult<PaginatedQueryResult<ProcessoInconsistente>>.Forbidden();
                }                     

                var ParametroJuridico = new OracleParameter("O_RESULT_SET", OracleDbType.RefCursor, ParameterDirection.Output);

                string sql;
                sql = $"BEGIN jur.PC_VER_INCONSISTENCIAS_TRIB.SP_VER_INCONSISTENCIAS_TRIB(" + codigoEsfera + ", :O_RESULT_SET  ); END;";

                var listaBase = DatabaseContext.ProcessoInconsistentes.FromSql(sql, ParametroJuridico).AsNoTracking().ToList();
                
                if (codigoEsfera == 8)
                {
                  var teste = new ProcessoInconsistente("TESTE DE PROCESSO",
                                                      new Random().Next(),
                                                    "4687138844",
                                                    "RJ",
                                                    "Comarca",
                                                    "RIO de Janeiro",
                                                    "Empresa",
                                                    "Escritório",
                                                    "Objeto",
                                                    DateTime.Now,
                                                    5,
                                                    8);


                    listaBase.Add(teste);
                }

                if (listaBase.Count() > 0)
                {
                    foreach (ProcessoInconsistente item in listaBase)
                    {
                        var ProcTrib = new ProcessoTributarioInconsistente(item.TipoProcesso,
                                                                            item.CodigoProcesso,
                                                                            item.NumeroProcesso,
                                                                            item.Estado,
                                                                            item.ComarcaOrgao,
                                                                            item.VaraMunicipio,
                                                                            item.EmpresadoGrupo,
                                                                            item.Escritorio,
                                                                            item.Objeto,
                                                                            item.Periodo,
                                                                            item.ValorTotalCorrigido,
                                                                            item.ValorTotalPago);

                        DatabaseContext.Add(ProcTrib);
                    }

                    Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
                    var total = listaBase.Count();

                    Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Quantidade de registros para saltar"));
                    var skip = Pagination.PagesToSkip(8, total, 1);

                    var resultado = new PaginatedQueryResult<ProcessoInconsistente>()
                    {
                        Total = total,
                        Data = listaBase.Skip(skip).Take(8).ToArray()
                    };

                    DatabaseContext.SaveChanges();

                    Logger.LogInformation(Infra.Extensions.Logs.OperacaoFinalizada(commandName));
                    return CommandResult<PaginatedQueryResult<ProcessoInconsistente>>.Valid(resultado);
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, $"{codigoEsfera}, {dataVigencia}"));
                IndiceCorrecaoEsfera indice = DatabaseContext.IndiceCorrecaoEsferas.FirstOrDefault(x => x.EsferaId == codigoEsfera && x.DataVigencia == dataVigencia && x.IndiceId == codigoIndice);

                if (indice is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, $"{codigoEsfera}, {dataVigencia}"));
                    return CommandResult<PaginatedQueryResult<ProcessoInconsistente>>.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, $"{codigoEsfera}, {dataVigencia}"));
                }

                Logger.LogInformation(Infra.Extensions.Logs.RemovendoEntidade(entityName, $"{codigoEsfera}, {dataVigencia}"));
                DatabaseContext.Remove(indice);

                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade($"{codigoEsfera}, {dataVigencia}"));
                DatabaseContext.SaveChanges();

                Logger.LogInformation(Infra.Extensions.Logs.OperacaoFinalizada(commandName));
                return CommandResult<PaginatedQueryResult<ProcessoInconsistente>>.Valid(null);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, Infra.Extensions.Logs.OperacaoComErro(commandName));
                return CommandResult<PaginatedQueryResult<ProcessoInconsistente>>.Invalid(ex.Message);
            }
        }
    }
}