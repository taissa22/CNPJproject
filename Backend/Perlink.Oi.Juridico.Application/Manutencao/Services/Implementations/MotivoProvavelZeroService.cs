using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Constants;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Validators.PrimitiveValidators;
using Perlink.Oi.Juridico.Infra.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services.Implementations
{
    internal class MotivoProvavelZeroService : IMotivoProvavelZeroService
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IMotivoProvavelZeroService> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public MotivoProvavelZeroService(IDatabaseContext databaseContext, ILogger<IMotivoProvavelZeroService> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        public CommandResult Criar(CriarMotivoProvavelZeroCommand command)
        {
            string entityName = "MotivoProvavelZero";
            string commandName = $"Criar {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try
            {
                command.Validate();
                command.Descricao = command.Descricao.ToUpper().Trim();

                if (command.Invalid)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.ComandoInvalido(commandName));
                    return CommandResult.Invalid(command.Notifications.ToNotificationsString());
                }

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_MOTIVO_PROVAVEL_ZERO))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_MOTIVO_PROVAVEL_ZERO, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }
               
                MotivoProvavelZero MotivoProvavelZero = MotivoProvavelZero.Criar(command.Descricao);

                Logger.LogInformation(Infra.Extensions.Logs.CriandoEntidade(entityName));

                if (MotivoProvavelZero.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(MotivoProvavelZero.Notifications.ToNotificationsString());
                }           

                DatabaseContext.Add(MotivoProvavelZero);
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

        public CommandResult Atualizar(AtualizarMotivoProvavelZeroCommand command)
        {
            string entityName = "MotivoProvavelZero";
            string commandName = $"Atualizar {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try
            {
                command.Validate();
                command.Descricao = command.Descricao.ToUpper().Trim();

                if (command.Invalid)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.ComandoInvalido(commandName));
                    return CommandResult.Invalid(command.Notifications.ToNotificationsString());
                }

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_MOTIVO_PROVAVEL_ZERO))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_MOTIVO_PROVAVEL_ZERO, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                MotivoProvavelZero MotivoProvavelZero = DatabaseContext.MotivoProvavelZeros.FirstOrDefault(x => x.Id == command.Id);
                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, command.Id));

                if (MotivoProvavelZero is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.Id));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.Id));
                }                
               
                MotivoProvavelZero.Atualizar(command.Id,  command.Descricao );

                Logger.LogInformation(Infra.Extensions.Logs.AtualizandoEntidade(entityName, command.Id));

                if (MotivoProvavelZero.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(MotivoProvavelZero.Notifications.ToNotificationsString());
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
            string entityName = nameof(MotivoProvavelZero);
            string commandName = $"Removendo {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try
            {
                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_MOTIVO_PROVAVEL_ZERO))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_MOTIVO_PROVAVEL_ZERO, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                MotivoProvavelZero MotivoProvavelZero = DatabaseContext.MotivoProvavelZeros.FirstOrDefault(x => x.Id == id);
                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, id));

                if (DatabaseContext.PartesPedidosProcesso.Any(p => p.CodMotivoProvavelZero == id))
                {
                    string mensagem = "Não será possível excluir " + MotivoProvavelZero.Descricao + ", pois se encontra relacionada com a tabela Parte Pedido Processo.";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }


                if (MotivoProvavelZero is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, id));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, id));
                }
               
                string commandRemocao = $"Validar remoção {entityName}";
                Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandRemocao));

                DatabaseContext.Remove(MotivoProvavelZero);
                Logger.LogInformation(Infra.Extensions.Logs.RemovendoEntidade(entityName, id));

                DatabaseContext.SaveChanges();
                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade(entityName));

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
