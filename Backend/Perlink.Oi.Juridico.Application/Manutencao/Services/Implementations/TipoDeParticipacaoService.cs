using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Constants;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services.Implementations
{
    internal class TipoDeParticipacaoService : ITipoDeParticipacaoService
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<ITipoDeParticipacaoService> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public TipoDeParticipacaoService(IDatabaseContext databaseContext, ILogger<ITipoDeParticipacaoService> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        public CommandResult Criar(CriarTipoDeParticipacaoCommand command)
        {
            string entityName = "Tipo de Participacao";
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

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_TIPO_DE_PARTICIPACAO))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_TIPO_DE_PARTICIPACAO, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                if (DatabaseContext.TiposDeParticipacoes.Any(x => x.Descricao.ToUpper() == command.Descricao.ToUpper()))
                {
                    string mensagem = "Já existe um tipo de participação cadastrado com essa descrição.";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                Logger.LogInformation(Infra.Extensions.Logs.CriandoEntidade(entityName));
                TipoDeParticipacao acao = TipoDeParticipacao.Criar(command.Descricao);

                if (acao.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(acao.Notifications.ToNotificationsString());
                }

                DatabaseContext.Add(acao);
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

        public CommandResult Atualizar(AtualizarTipoDeParticipacaoCommand command)
        {
            string entityName = "Tipo de Participacao";
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

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_TIPO_DE_PARTICIPACAO))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_TIPO_DE_PARTICIPACAO, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                if (DatabaseContext.TiposDeParticipacoes.Any(x => x.Descricao.ToUpper() == command.Descricao.ToUpper() && x.Codigo != command.Codigo))
                {
                    string mensagem = "Já existe um tipo de participação cadastrado com essa descrição.";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, command.Codigo));
                TipoDeParticipacao tipoDeParticipacao = DatabaseContext.TiposDeParticipacoes.FirstOrDefault(a => a.Codigo == command.Codigo);

                if (tipoDeParticipacao is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.Codigo));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.Codigo));
                }

                Logger.LogInformation(Infra.Extensions.Logs.AtualizandoEntidade(entityName, command.Codigo));
                tipoDeParticipacao.Atualizar(command.Descricao);

                if (tipoDeParticipacao.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(tipoDeParticipacao.Notifications.ToNotificationsString());
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

        public CommandResult Remover(int CodigoTipoDeParticipacao)
        {
            string entityName = "Tipo de Participação";
            string commandName = $"Removendo {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try
            {
                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_TIPO_DE_PARTICIPACAO))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_TIPO_DE_PARTICIPACAO, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, CodigoTipoDeParticipacao));
                TipoDeParticipacao tipoDeParticipacao = DatabaseContext.TiposDeParticipacoes.FirstOrDefault(a => a.Codigo == CodigoTipoDeParticipacao);

                if (tipoDeParticipacao is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, CodigoTipoDeParticipacao));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, CodigoTipoDeParticipacao));
                }

                string commandRemocao = $"Validar remoção {entityName}";
                Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandRemocao));
                if (DatabaseContext.PartesProcessos.Any(x => x.TipoParticipacaoId == CodigoTipoDeParticipacao))
                {
                    string mensagem = "Não será possível excluir o Tipo de Participação selecionado, pois ele se encontra relacionado com Partes do Processo.";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                if (DatabaseContext.Procedimentos.Any(x => x.CodTipoParticipacao1 == CodigoTipoDeParticipacao || x.CodTipoParticipacao2 == CodigoTipoDeParticipacao))
                {
                    string mensagem = "Não será possível excluir o Tipo de Participação selecionado, pois ele se encontra relacionado com Procedimento.";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                Logger.LogInformation(Infra.Extensions.Logs.RemovendoEntidade(entityName, CodigoTipoDeParticipacao));
                DatabaseContext.Remove(tipoDeParticipacao);

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
