using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Constants;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Validators;
using Perlink.Oi.Juridico.Infra.ValueObjects;
using System;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services.Implementations
{
    internal class FielDepositarioService : IFielDepositarioService
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IFielDepositarioService> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public FielDepositarioService(IDatabaseContext context, ILogger<IFielDepositarioService> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = context;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        public CommandResult CriarFielDepositario(CriarFielDepositarioCommand command)
        {
            string entityName = "Fiel depositário";
            string commandName = $"Criar {entityName}";           
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try
            {
                command.Validate();
                if (command.Invalid) {
                    Logger.LogInformation(Infra.Extensions.Logs.ComandoInvalido(commandName));
                    return CommandResult.Invalid(command.Notifications.ToNotificationsString());
                }


                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_FIEL_DEPOSITARIO))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_FIEL_DEPOSITARIO, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }
    

                if (CPF.IsValidForSisjur(command.Cpf))
                {
                    FielDepositario fielDepositario = DatabaseContext
                                                     .FieisDepositarios
                                                     .FirstOrDefault(x => x.Cpf.Equals(command.Cpf));

                    if (fielDepositario != null)
                    {
                        string message = "Já existe um fiel depositário cadastrado com o mesmo CPF.";                    
                        Logger.LogInformation(message);
                        return CommandResult.Invalid(message);
                    }

                        Logger.LogInformation(Infra.Extensions.Logs.CriandoEntidade(entityName));
                        fielDepositario = FielDepositario.Criar(CPF.FromString(command.Cpf), command.Nome);

                        Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade(entityName));
                        DatabaseContext.Add(fielDepositario);
                        DatabaseContext.SaveChangesAsync();

                        Logger.LogInformation(Infra.Extensions.Logs.OperacaoFinalizada(commandName));
                        return CommandResult.Valid();
                }   
                else
                {
                    Logger.LogInformation("CPF inválido");
                    return CommandResult.Invalid("CPF inválido");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, Infra.Extensions.Logs.OperacaoComErro(commandName));
                return CommandResult.Invalid(ex.Message);
            }
        }

        public CommandResult ExcluirFielDepositario(int id)
        {
            string entityName = "Fiel depositário";
            string commandName = $"Excluir {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));         

            try
            {
                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_FIEL_DEPOSITARIO)) {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_FIEL_DEPOSITARIO, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }


                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, id));
                FielDepositario fielDepositario = DatabaseContext
                                                 .FieisDepositarios
                                                 .FirstOrDefault(x => x.Id == id);

                if (fielDepositario is null) {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, id));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, id));
                } else if (DatabaseContext.Lancamentos.Where(x => x.FielDepositario.Id == id).Count() > 0) {
                    Logger.LogInformation($"O Fiel Depositário selecionado se encontra relacionado com um lançamento de processo.");
                    return CommandResult.Invalid("O Fiel Depositário selecionado se encontra relacionado com um lançamento de processo.");
                }

                Logger.LogInformation(Infra.Extensions.Logs.RemovendoEntidade(entityName, id));
                DatabaseContext.Remove(fielDepositario);

                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade(entityName));
                DatabaseContext.SaveChangesAsync();

                Logger.LogInformation(Infra.Extensions.Logs.OperacaoFinalizada(commandName));
                return CommandResult.Valid();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, Infra.Extensions.Logs.OperacaoComErro(commandName));
                return CommandResult.Invalid(ex.Message);
            }
        }

        public CommandResult AtualizarFielDepositario(AtualizarFielDepositarioCommand command)
        {
            string entityName = "Fiel depositário";
            string commandName = $"Excluir {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));


            try
            {

                command.Validate();
                if (command.Invalid) {
                    Logger.LogInformation(Infra.Extensions.Logs.ComandoInvalido(commandName));
                    return CommandResult.Invalid(command.Notifications.ToNotificationsString());
                }

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_FIEL_DEPOSITARIO))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_FIEL_DEPOSITARIO, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, command.Id));
                FielDepositario fielDepositario = DatabaseContext
                                                 .FieisDepositarios
                                                 .FirstOrDefault(x => x.Id == command.Id);

                if (fielDepositario is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.Id));
                    return CommandResult.Invalid("Não encontrado fiel depositário.");
                }   
                else if (DatabaseContext.FieisDepositarios.Any(x => x.Cpf.Equals(command.Cpf) && x.Id != command.Id))
                {                
                    string message = "Já existe um fiel depositário cadastrado com o mesmo CPF.";
                    Logger.LogInformation(message);
                    return CommandResult.Invalid(message);
                }
                else if (!CPF.IsValidForSisjur(command.Cpf))
                {
                    Logger.LogInformation($"CPF inválido");
                    return CommandResult.Invalid("CPF inválido");
                }
                Logger.LogInformation(Infra.Extensions.Logs.AtualizandoEntidade(entityName, command.Id));
                fielDepositario.AtualizarFielDepositario(command.Nome, CPF.FromString(command.Cpf));

                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade(entityName));
                DatabaseContext.SaveChangesAsync();

                Logger.LogInformation(Infra.Extensions.Logs.OperacaoFinalizada(commandName));
                return CommandResult.Valid();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, Infra.Extensions.Logs.OperacaoComErro(commandName));
                throw;
            }
        }
    }
}