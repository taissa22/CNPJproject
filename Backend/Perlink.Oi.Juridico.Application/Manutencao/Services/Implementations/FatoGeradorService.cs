using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Constants;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services.Implementations
{
    internal class FatoGeradorService : IFatoGeradorService
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IFatoGeradorService> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public FatoGeradorService(IDatabaseContext databaseContext, ILogger<IFatoGeradorService> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        public CommandResult Criar(CriarFatoGeradorCommand command)
        {
            string entityName = "Fato Gerador";
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
                
                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_FATO_GERADOR ))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_FATO_GERADOR, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                
                Logger.LogInformation(Infra.Extensions.Logs.CriandoEntidade(entityName));
                FatoGerador fato = FatoGerador.Criar(command.Nome,command.Ativo);
                
                if (fato.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(fato.Notifications.ToNotificationsString());
                }

                DatabaseContext.Add(fato);
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

        public CommandResult Atualizar(AtualizarFatoGeradorCommand command)
        {
            string entityName = "Fato Gerador";
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

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_FATO_GERADOR))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_FATO_GERADOR, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, $"{command.Id}, {command.Nome}"));
                FatoGerador fato = DatabaseContext.FatosGeradores.FirstOrDefault(x => x.Id == command.Id);

                if (fato is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, $"{command.Id}, {command.Nome}"));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, $"{command.Id}, {command.Nome}"));
                }

                Logger.LogInformation(Infra.Extensions.Logs.AtualizandoEntidade(entityName, $"{command.Id}, {command.Nome}"));


                fato.Atualizar(command.Id, command.Nome, command.Ativo);               

                if (fato.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida($"{command.Id}, {command.Nome}"));
                    return CommandResult.Invalid(fato.Notifications.ToNotificationsString());
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
        public CommandResult Remover(int id)
        {
            string entityName = "Fato Gerador";
            string commandName = $"Removendo {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try
            {
                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_FATO_GERADOR))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_FATO_GERADOR, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, $"{id}"));
                FatoGerador fato = DatabaseContext.FatosGeradores.FirstOrDefault(x => x.Id == id);

                if (fato is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, $"{id}"));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, $"{id}"));
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, $"{id}"));
                bool TemProcesso = DatabaseContext.Processos.Where(x => x.FatoGeradorId == id).Count() > 0;

                if (TemProcesso)
                {
                    string mensagem = "Não é possível excluir o Fato Gerador, pois ele está relacionado com a Processo.";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, $"{id}"));
                bool TemFatoProcesso = DatabaseContext.FatoGeradorProcessos.Where(x => x.FatoGeradorId == id).Count() > 0;

                if (TemFatoProcesso)
                {
                    string mensagem = "Não é possível excluir o Fato Gerador, pois ele está relacionado com a Fato Gerador Processo.";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                Logger.LogInformation(Infra.Extensions.Logs.RemovendoEntidade(entityName, $"{id}"));
                DatabaseContext.Remove(fato);

                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade($"{id}"));
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