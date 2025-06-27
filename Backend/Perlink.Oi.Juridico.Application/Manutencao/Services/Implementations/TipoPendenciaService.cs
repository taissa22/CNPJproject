using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Constants;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services.Implementations
{
    internal class TipoPendenciaService: ITipoPendenciaService
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<ITipoPendenciaService> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public TipoPendenciaService(IDatabaseContext databaseContext, ILogger<ITipoPendenciaService> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        public CommandResult Criar(CriarTipoPendenciaCommand command)
        {
            string entityName = "Tipo de Pendência";
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

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_TIPO_PENDENCIA))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_TIPO_PENDENCIA, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }                            

                Logger.LogInformation(Infra.Extensions.Logs.CriandoEntidade(entityName));
                TipoPendencia tipoPendencia = TipoPendencia.Criar(DataString.FromString(command.Descricao));

                if (tipoPendencia.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(tipoPendencia.Notifications.ToNotificationsString());
                }

                DatabaseContext.Add(tipoPendencia);
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

        public CommandResult Atualizar(AtualizarTipoPendenciaCommand command)
        {
            string entityName = "Tipo de Pendência";
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

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_TIPO_PENDENCIA))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_TIPO_PENDENCIA, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, command.Id));
                TipoPendencia tipoPendencia = DatabaseContext.TipoPendencias.FirstOrDefault(a => a.Id == command.Id);

                if (tipoPendencia is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.Id));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.Id));
                }

                Logger.LogInformation(Infra.Extensions.Logs.AtualizandoEntidade(entityName, command.Id));
                tipoPendencia.Atualizar(command.Descricao);

                if (tipoPendencia.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(tipoPendencia.Notifications.ToNotificationsString());
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

        public CommandResult Remover(int tipoPendenciaId)
        {
            string entityName = "Tipo de Pendência";
            string commandName = $"Removendo {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try
            {
                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_TIPO_PENDENCIA))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_TIPO_PENDENCIA, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, tipoPendenciaId));
                TipoPendencia tipoPendencia = DatabaseContext.TipoPendencias.FirstOrDefault(a => a.Id == tipoPendenciaId);

                if (tipoPendencia is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, tipoPendenciaId));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, tipoPendenciaId));
                }

                string commandRemocao = $"Validar remoção {entityName}";
                Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandRemocao));
                if (DatabaseContext.PendenciasProcessos.Any(x => x.TipoPendencia == tipoPendencia)) {
                    string mensagem = "Não é possível excluir o Tipo de Pendência, pois ele está relacionado com Pendências do Processo";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }                    

                Logger.LogInformation(Infra.Extensions.Logs.RemovendoEntidade(entityName, tipoPendenciaId));
                DatabaseContext.Remove(tipoPendencia);

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
