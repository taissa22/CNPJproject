using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Constants;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services.Implementations
{
    internal class PrepostosService : IPrepostosService
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IPrepostosService> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public PrepostosService(IDatabaseContext databaseContext, ILogger<IPrepostosService> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        public CommandResult Criar(CriarPrepostoCommand command)
        {
            string entityName = "Preposto";
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

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_PREPOSTO))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_PREPOSTO, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                if (!string.IsNullOrEmpty(command.UsuarioId) && DatabaseContext.Prepostos.Any(x => x.UsuarioId == command.UsuarioId))
                {
                    string mensagem = "O usuário selecionado já está cadastrado para outro preposto";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                var matriculaFormatada = command.Matricula != null && command.Matricula != String.Empty ? command.Matricula.ToUpper().Trim() : null;

                if (matriculaFormatada != null)
                {
                    var prepostoComMatriculaExistente = DatabaseContext.Prepostos.FirstOrDefault(x => x.Matricula.ToUpper().Trim() == matriculaFormatada &&
                                                                                                 x.Ativo == true);

                    if (prepostoComMatriculaExistente != null && command.Ativo)
                    {
                        string mensagem = "Existe um outro preposto ativo com a mesma matrícula com o nome " + prepostoComMatriculaExistente.Nome + ". Favor verificar.";
                        Logger.LogInformation(mensagem);
                        return CommandResult.Invalid(mensagem);
                    }
                }

                Logger.LogInformation(Infra.Extensions.Logs.CriandoEntidade(entityName));

                Preposto preposto = Preposto.Criar(command.Nome.ToUpper().Trim(),
                                                   command.Ativo,
                                                   command.EhCivelEstrategico,
                                                   command.EhCivel,
                                                   command.EhTrabalhista,
                                                   command.EhJuizado,
                                                   command.UsuarioId,
                                                   command.EhProcon,
                                                   command.EhPex,
                                                   command.EhEscritorio,
                                                   matriculaFormatada);

                if (preposto.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(preposto.Notifications.ToNotificationsString());
                }

                DatabaseContext.Add(preposto);
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

        public CommandResult Atualizar(AtualizarPrepostoCommand command)
        {
            string entityName = "Preposto";
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

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_PREPOSTO))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_PREPOSTO, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, $"{command.Id}, {command.Nome}"));
                Preposto preposto = DatabaseContext.Prepostos.FirstOrDefault(x => x.Id == command.Id);

                if (preposto is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, $"{command.Id}, {command.Nome}"));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, $"{command.Id}, {command.Nome}"));
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade("Alocação Preposto", $"{preposto}"));

                var inativando = preposto.Ativo && preposto.Ativo != command.Ativo;

                if (inativando)
                {
                    bool alocadoEmAgenda = DatabaseContext.AlocacoesPrepostos.Where(x => x.PrepostoId == command.Id && x.DataAlocacao > System.DateTime.Now.Date).AsNoTracking().Any();

                    if (alocadoEmAgenda)
                    {
                        string mensagem = "O preposto não pode ser Inativado, pois está relacionado a Alocação Preposto";
                        Logger.LogInformation(mensagem);
                        return CommandResult.Invalid(mensagem);
                    }
                }

                if (inativando)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade("Audiência Processo", $"{preposto}"));

                    var alocadoEmAudiencia = DatabaseContext.AudienciasDosProcessos.Where(x => x.Preposto.Id == preposto.Id && x.DataAudiencia > System.DateTime.Now.Date).AsNoTracking().Any();

                    if (alocadoEmAudiencia)
                    {
                        string mensagem = "O preposto não pode ser Inativado, pois está relacionado a Audiência Processo";
                        Logger.LogInformation(mensagem);
                        return CommandResult.Invalid(mensagem);
                    }
                }

                if (!string.IsNullOrEmpty(command.UsuarioId) && DatabaseContext.Prepostos.Any(x => x.Id != command.Id && x.UsuarioId == command.UsuarioId))
                {
                    string mensagem = "O usuário selecionado já está cadastrado para outro preposto";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                var matriculaFormatada = command.Matricula != null && command.Matricula != String.Empty ? command.Matricula.ToUpper().Trim() : null;

                if (matriculaFormatada != null)
                {
                    var prepostoComMatriculaExistente = DatabaseContext.Prepostos.FirstOrDefault(x => x.Matricula.ToUpper().Trim() == matriculaFormatada &&
                                                                                             x.Id != command.Id &&
                                                                                             x.Ativo == true);
                    if (prepostoComMatriculaExistente != null && command.Ativo)
                    {
                        string mensagem = "Existe um outro preposto ativo com a mesma matrícula com o nome " + prepostoComMatriculaExistente.Nome + ". Favor verificar.";
                        Logger.LogInformation(mensagem);
                        return CommandResult.Invalid(mensagem);
                    }
                }

                Logger.LogInformation(Infra.Extensions.Logs.AtualizandoEntidade(entityName, $"{command.Id}, {command.Nome}"));

                preposto.Atualizar(command.Nome.ToUpper().Trim(),
                                   command.Ativo,
                                   command.EhCivelEstrategico,
                                   command.EhCivel,
                                   command.EhTrabalhista,
                                   command.EhJuizado,
                                   command.UsuarioId,
                                   command.EhProcon,
                                   command.EhPex,
                                   command.EhEscritorio,
                                   matriculaFormatada);

                if (preposto.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida($"{command.Id}, {command.Nome}"));
                    return CommandResult.Invalid(preposto.Notifications.ToNotificationsString());
                }

                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade($"{command.Id}, {command.Nome}"));
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

        public CommandResult Remover(int prepostoId)
        {
            string entityName = "Preposto";
            string commandName = $"Removendo {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try
            {
                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_PREPOSTO))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_PREPOSTO, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade("Alocação Preposto", $"{prepostoId}"));
                bool alocado = DatabaseContext.AlocacoesPrepostos.Where(x => x.PrepostoId == prepostoId && x.DataAlocacao > DateTime.Today).Any();

                if (alocado)
                {
                    string mensagem = "O preposto não pode ser excluído, pois está relacionado a Alocação Preposto";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade("Audiência Processo", $"{prepostoId}"));
                alocado = DatabaseContext.AudienciasDosProcessos.Any(x => x.Preposto.Id == prepostoId);

                if (alocado)
                {
                    string mensagem = "O preposto não pode ser excluído, pois está relacionado a Audiência Processo";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, $"{prepostoId}"));
                Preposto preposto = DatabaseContext.Prepostos.FirstOrDefault(x => x.Id == prepostoId);

                if (preposto is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, $"{prepostoId}"));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, $"{prepostoId}"));
                }

                Logger.LogInformation(Infra.Extensions.Logs.RemovendoEntidade(entityName, $"{prepostoId}"));
                DatabaseContext.Remove(preposto);

                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade($"{prepostoId}"));
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