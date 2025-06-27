using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Application.Manutencao.Commands.Dto;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Constants;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services.Implementations
{
    internal class UsuarioOperacaoRetroativaService : IUsuarioOperacaoRetroativaService
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IUsuarioOperacaoRetroativaService> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public UsuarioOperacaoRetroativaService(IDatabaseContext databaseContext, ILogger<IUsuarioOperacaoRetroativaService> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        public CommandResult Criar(CriarUsuarioOperacaoRetroativaCommand command)
        {
            string entityName = "Usuario Operação Retroativa";
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

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_OPERACOES_RETROATIVAS))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_OPERACOES_RETROATIVAS, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                if (DatabaseContext.UsuarioOperacaoRetroativas.Any(x => x.CodUsuario == command.CodUsuario))
                {
                    string message = "Usuario já cadastrado.";
                    Logger.LogInformation(message);
                    return CommandResult.Invalid(message);
                }

                Logger.LogInformation(Infra.Extensions.Logs.CriandoEntidade(entityName));
                UsuarioOperacaoRetroativa usuarioOperacaoRetroativa = UsuarioOperacaoRetroativa.Criar(command.CodUsuario, command.LimiteAlteracao, 2);

                if (usuarioOperacaoRetroativa.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(usuarioOperacaoRetroativa.Notifications.ToNotificationsString());
                }

                DatabaseContext.Add(usuarioOperacaoRetroativa);
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

        public CommandResult Atualizar(AtualizarUsuarioOperacaoRetroativaCommand command)
        {
            string entityName = "Usuario Operacao Retroativa";
            string commandName = $"Atualizar {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try
            {
                command.Validate();
                if (command.Invalid)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.ComandoInvalido(commandName));
                    return CommandResult.Invalid(command.Notifications.ToNotificationsString());
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, command.CodUsuario));
                UsuarioOperacaoRetroativa operacao = DatabaseContext.UsuarioOperacaoRetroativas.FirstOrDefault(x => x.CodUsuario == command.CodUsuario);

                if (operacao is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.CodUsuario));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.CodUsuario));
                }

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_OPERACOES_RETROATIVAS))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_OPERACOES_RETROATIVAS, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.AtualizandoEntidade(entityName, command.CodUsuario));
                operacao.Atualizar(command.CodUsuario, command.LimiteAlteracao, 2);

                if (operacao.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(operacao.Notifications.ToNotificationsString());
                }

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

        public CommandResult Remover(string codUsuario)
        {
            string entityName = "Usuario Operacao Retroativa";
            string commandName = $"Removendo {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try
            {
                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, codUsuario));
                UsuarioOperacaoRetroativa operacao = DatabaseContext.UsuarioOperacaoRetroativas.FirstOrDefault(x => x.CodUsuario == codUsuario);

                Logger.LogInformation(Infra.Extensions.Logs.ValidanoPermissao(UsuarioAtual.Login));
                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_OPERACOES_RETROATIVAS))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_OPERACOES_RETROATIVAS, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                if (operacao is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, codUsuario));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, codUsuario));
                }              

                if (operacao.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(operacao.Notifications.ToNotificationsString());
                }

                Logger.LogInformation(Infra.Extensions.Logs.RemovendoEntidade(entityName, codUsuario));
                DatabaseContext.Remove(operacao);

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

      
    }
}
