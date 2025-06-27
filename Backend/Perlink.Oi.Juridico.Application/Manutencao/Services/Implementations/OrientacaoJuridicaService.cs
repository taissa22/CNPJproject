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
    internal class OrientacaoJuridicaService : IOrientacaoJuridicaService
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IOrientacaoJuridicaService> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public OrientacaoJuridicaService(IDatabaseContext databaseContext, ILogger<IOrientacaoJuridicaService> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        public CommandResult Criar(CriarOrientacaoJuridicaCommand command)
        {
            string entityName = "OrientacaoJuridica";
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

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ORIENTACAO_JURIDICA))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ORIENTACAO_JURIDICA, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }
                                
                Logger.LogInformation(Infra.Extensions.Logs.CriandoEntidade(entityName));

                OrientacaoJuridica orientacao = OrientacaoJuridica.Criar(command.CodTipoOrientacaoJuridica, command.Nome.ToUpper()
                    , command.Descricao.ToUpper(), command.PalavraChave.ToUpper(), command.EhTrabalhista, command.Ativo);

                if (orientacao.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(orientacao.Notifications.ToNotificationsString());
                }

                DatabaseContext.Add(orientacao);
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

        public CommandResult Atualizar(AtualizarOrientacaoJuridicaCommand command)
        {
            string entityName = "OrientacaoJuridica";
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

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, command.CodOrientacaoJuridica));
                OrientacaoJuridica orientacao = DatabaseContext.OrientacoesJuridicas.FirstOrDefault(x => x.CodOrientacaoJuridica == command.CodOrientacaoJuridica);

                if (orientacao is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.CodOrientacaoJuridica));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.CodOrientacaoJuridica));
                }

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ORIENTACAO_JURIDICA))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ORIENTACAO_JURIDICA, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.AtualizandoEntidade(entityName, command.CodOrientacaoJuridica));
                orientacao.Atualizar(command.CodOrientacaoJuridica, command.CodTipoOrientacaoJuridica, command.Nome.ToUpper()
                    , command.Descricao.ToUpper(), command.PalavraChave.ToUpper(), command.EhTrabalhista, command.Ativo);

                if (orientacao.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(orientacao.Notifications.ToNotificationsString());
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

        public CommandResult Remover(int id)
        {
            string entityName = "OrientacaoJuridica";
            string commandName = $"Removendo {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try
            {
                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, id));
                OrientacaoJuridica orientacao = DatabaseContext.OrientacoesJuridicas.FirstOrDefault(x => x.CodOrientacaoJuridica == id);

                Logger.LogInformation(Infra.Extensions.Logs.ValidanoPermissao(UsuarioAtual.Login));
                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ORIENTACAO_JURIDICA))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ORIENTACAO_JURIDICA, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                if (orientacao is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, id));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, id));
                }

                if (orientacao.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(orientacao.Notifications.ToNotificationsString());
                }

                if (DatabaseContext.PartesPedidosProcesso.Any(x => x.CodOrientacaoJuridica == id))
                {
                    string mensagem = "Não é possível excluir a Orientação Jurídica, pois ela está relacionada com Partes do Pedido do Processo.";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                Logger.LogInformation(Infra.Extensions.Logs.RemovendoEntidade(entityName, id));
                DatabaseContext.Remove(orientacao);

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
