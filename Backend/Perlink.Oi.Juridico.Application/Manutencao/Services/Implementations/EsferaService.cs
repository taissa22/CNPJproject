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
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services.Implementations
{
    internal class EsferaService : IEsferaService
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IEsferaService> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public EsferaService(IDatabaseContext databaseContext, ILogger<IEsferaService> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        public CommandResult Criar(CriarEsferaCommand command)
        {
            string entityName = "Esfera";
            string commandName = $"Criar {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try
            {
                command.Validate();
                if (command.Invalid)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.ComandoInvalido(commandName));
                    return CommandResult.Invalid(command.Notifications.ToNotificationsString());
                }

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ESFERA))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ESFERA, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.CriandoEntidade(entityName));
                Esfera esfera = Esfera.Criar(command.Nome, command.CorrigePrincipal, command.CorrigeMultas, command.CorrigeJuros);

                if (esfera.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(esfera.Notifications.ToNotificationsString());
                }

                DatabaseContext.Add(esfera);
                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade(entityName));

                DatabaseContext.SaveChanges();
                Logger.LogInformation(Infra.Extensions.Logs.OperacaoFinalizada(commandName));

                return CommandResult.Valid();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, Infra.Extensions.Logs.OperacaoComErro(commandName));
                return CommandResult.Invalid(ex.Message);
            }
        }

        public CommandResult<PaginatedQueryResult<ProcessoInconsistente>> Atualizar(AtualizarEsferaCommand command)
        {
            string entityName = "Esfera";
            string commandName = $"Atualizar {entityName}";
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
                    return CommandResult<PaginatedQueryResult<ProcessoInconsistente>>.Invalid(command.Notifications.ToNotificationsString());

                }

                var ParametroJuridico = new OracleParameter("O_RESULT_SET", OracleDbType.RefCursor, ParameterDirection.Output);

                string sql;
                sql = $"BEGIN jur.PC_VER_INCONSISTENCIAS_TRIB.SP_VER_INCONSISTENCIAS_TRIB(" + command.Id + ", :O_RESULT_SET  ); END;";

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

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, $"{command.Id}, {command.Nome}"));
                Esfera esfera = DatabaseContext.Esferas.FirstOrDefault(x => x.Id == command.Id);

                if (esfera is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, $"{command.Id}, {command.Nome}"));
                    return CommandResult<PaginatedQueryResult<ProcessoInconsistente>>.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, $"{command.Id}, {command.Nome}"));
                }

                Logger.LogInformation(Infra.Extensions.Logs.AtualizandoEntidade(entityName, $"{command.Id}, {command.Nome}"));
                esfera.Atualizar(command.Nome, command.CorrigePrincipal, command.CorrigeMultas, command.CorrigeJuros);

                if (esfera.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida($"{command.Id}, {command.Nome}"));
                    return CommandResult<PaginatedQueryResult<ProcessoInconsistente>>.Invalid(esfera.Notifications.ToNotificationsString());
                }

                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade($"{command.Id}, {command.Nome}"));
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

        public CommandResult Remover(int esferaId)
        {
            string entityName = "Esfera";
            string commandName = $"Removendo {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try
            {
                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ESFERA))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ESFERA, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, $"{esferaId}"));
                Esfera esfera = DatabaseContext.Esferas.FirstOrDefault(x => x.Id == esferaId);

                if (esfera is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, $"{esferaId}"));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, $"{esferaId}"));
                }

                if (DatabaseContext.Processos.Any(x => x.EsferaId == esferaId))
                {
                    string message = "Não será possível excluir o registro de esfera selecionado, pois se encontra relacionado com Processo.";
                    Logger.LogInformation(message);
                    return CommandResult.Invalid(message);
                }

                if (DatabaseContext.IndiceCorrecaoEsferas.Any(x => x.EsferaId == esferaId))
                {
                    string message = "Não será possível excluir o registro de esfera selecionado, pois se encontra relacionado com Índice de Correção de Esferas.";
                    Logger.LogInformation(message);
                    return CommandResult.Invalid(message);
                }

                Logger.LogInformation(Infra.Extensions.Logs.RemovendoEntidade(entityName, $"{esferaId}"));
                DatabaseContext.Remove(esfera);

                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade($"{esferaId}"));
                DatabaseContext.SaveChanges();

                Logger.LogInformation(Infra.Extensions.Logs.OperacaoFinalizada(commandName));
                return CommandResult.Valid();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, Infra.Extensions.Logs.OperacaoComErro(commandName));
                return CommandResult.Invalid(ex.Message);
            }
        }
    }
}