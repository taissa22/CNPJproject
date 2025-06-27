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
    internal class TipoOrientacaoJuridicaService : ITipoOrientecaoJuridicaService
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<ITipoOrientecaoJuridicaService> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public TipoOrientacaoJuridicaService(IDatabaseContext databaseContext, ILogger<ITipoOrientecaoJuridicaService> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        public CommandResult Criar(CriarTipoOrientecaoJuridicaCommand command)
        {
            string entityName = "TipoOrientecaoJuridica";
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
                TipoOrientacaoJuridica tipoOrientacaojuridica = TipoOrientacaoJuridica.Criar(command.Descricao);

                if (tipoOrientacaojuridica.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(tipoOrientacaojuridica.Notifications.ToNotificationsString());
                }

                DatabaseContext.Add(tipoOrientacaojuridica);
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

        public CommandResult Atualizar(AtualizarTipoOrientecaoJuridicaCommand command)
        {
            string entityName = "TipoOrientecaoJuridica";
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

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ORIENTACAO_JURIDICA))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ORIENTACAO_JURIDICA, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, $"{command.Codigo}"));
                TipoOrientacaoJuridica tipoOrientacaojuridica = DatabaseContext.TiposOrientacoesJuridicas.FirstOrDefault(x => x.Id == command.Codigo);

                if (tipoOrientacaojuridica is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, $"{command.Codigo}"));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, $"{command.Codigo}"));
                }

                Logger.LogInformation(Infra.Extensions.Logs.AtualizandoEntidade(entityName, $"{command.Codigo}"));
                tipoOrientacaojuridica.Atualizar(command.Descricao);

                if (tipoOrientacaojuridica.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida($"{command.Codigo}"));
                    return CommandResult.Invalid(tipoOrientacaojuridica.Notifications.ToNotificationsString());
                }

                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade($"{command.Codigo}"));
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

        public CommandResult Remover(int codigo)
        {
            string entityName = "Indice";
            string commandName = $"Removendo {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try
            {
                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ORIENTACAO_JURIDICA))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ORIENTACAO_JURIDICA, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }
                                
                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, $"{codigo}"));
                TipoOrientacaoJuridica tipoOrientacaojuridica = DatabaseContext.TiposOrientacoesJuridicas.FirstOrDefault(x => x.Id == codigo);

                if (tipoOrientacaojuridica is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, $"{codigo}"));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, $"{codigo}"));
                }

                if (DatabaseContext.OrientacoesJuridicas.Any(x => x.TipoOrientacaoJuridica.Id == codigo))
                {
                    string mensagem = $"Não é possível excluir o Tipo de Pendência, pois ele está relacionado com Orientação Jurídica.";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                Logger.LogInformation(Infra.Extensions.Logs.RemovendoEntidade(entityName, $"{codigo}"));
                DatabaseContext.Remove(tipoOrientacaojuridica);

                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade($"{codigo}"));
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
