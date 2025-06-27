using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services.Implementations
{
    internal class ComplementoDeAreaEnvolvidaService : IComplementoDeAreaEnvolvidaService
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IComplementoDeAreaEnvolvidaService> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public ComplementoDeAreaEnvolvidaService(IDatabaseContext databaseContext, ILogger<IComplementoDeAreaEnvolvidaService> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        public CommandResult Criar(CriarComplementoDeAreaEnvolvidaCommand command)
        {
            string entityName = "Complemento de Area Envolvida";
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

                if (DatabaseContext.ComplementoDeAreasEnvolvidas.Any(x => x.Nome == command.Nome && x.TipoProcesso.Id == command.TipoProcessoId))
                {
                    string message = "Já existe um complemento com este nome.";
                    Logger.LogInformation(message);
                    return CommandResult.Invalid(message);
                }

                var complementoDeAreaEnvolvida = ComplementoDeAreaEnvolvida.Criar(command.Nome, command.Ativo, TipoProcesso.PorId(command.TipoProcessoId));

                if (complementoDeAreaEnvolvida.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(complementoDeAreaEnvolvida.Notifications.ToNotificationsString());
                }

                DatabaseContext.Add(complementoDeAreaEnvolvida);
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

        public CommandResult Atualizar(AtualizarComplementoDeAreaEnvolvidaCommand command)
        {
            string entityName = "Complemento de Area Envolvida";
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

                var complementoDeAreaEnvolvida = DatabaseContext.ComplementoDeAreasEnvolvidas.FirstOrDefault(x => x.Id == command.Id);

                if (complementoDeAreaEnvolvida is null)
                {
                    var mensagem = Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.Id);
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                if (DatabaseContext.ComplementoDeAreasEnvolvidas.Any(x => x.Id != command.Id && x.TipoProcesso.Id == complementoDeAreaEnvolvida.TipoProcesso.Id && x.Nome == command.Nome))
                {
                    string message = "Já existe um complemento com este nome.";
                    Logger.LogInformation(message);
                    return CommandResult.Invalid(message);
                }

                complementoDeAreaEnvolvida.Atualizar(command.Id, command.Nome, command.Ativo);

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

        public CommandResult Remover(int complementoDeAreaEnvolvidaId)
        {
            string entityName = "Complemento de Area Envolvida";
            string commandName = $"Removendo {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try
            {
                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, complementoDeAreaEnvolvidaId));
                ComplementoDeAreaEnvolvida complementoDeAreaEnvolvida = DatabaseContext.ComplementoDeAreasEnvolvidas.FirstOrDefault(x => x.Id == complementoDeAreaEnvolvidaId);

                if (complementoDeAreaEnvolvida is null)
                {
                    var mensagem = Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, complementoDeAreaEnvolvidaId);
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                if (DatabaseContext.Processos.Any(x => x.ComplementoDeAreaEnvolvidaId == complementoDeAreaEnvolvida.Id))
                {
                    string message = "Não é possível excluir esse complemento de área envolvida, pois ele está registrado em algum processo";
                    Logger.LogInformation(message);
                    return CommandResult.Invalid(message);
                }

                Logger.LogInformation(Infra.Extensions.Logs.RemovendoEntidade(entityName, complementoDeAreaEnvolvidaId));
                DatabaseContext.Remove(complementoDeAreaEnvolvida);

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