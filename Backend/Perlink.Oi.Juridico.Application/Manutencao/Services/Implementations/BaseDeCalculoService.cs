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
    internal class BaseDeCalculoService: IBaseDeCalculoService
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IBaseDeCalculoService> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public BaseDeCalculoService(IDatabaseContext databaseContext, ILogger<IBaseDeCalculoService> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }        

        public CommandResult Criar(CriarBaseDeCalculoCommand command)
        {
            string entityName = "Base de Cálculo";
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

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_BASE_DE_CALCULO))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_BASE_DE_CALCULO, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.CriandoEntidade(entityName));
                BaseDeCalculo baseDeCalculo = BaseDeCalculo.Criar(command.Descricao);

                if (baseDeCalculo.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(baseDeCalculo.Notifications.ToNotificationsString());
                }

                DatabaseContext.Add(baseDeCalculo);
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

        public CommandResult Atualizar(AtualizarBaseDeCalculoCommand command)
        {
            string entityName = "Base de Cálculo";
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

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_BASE_DE_CALCULO))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_BASE_DE_CALCULO, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, command.Codigo));
                BaseDeCalculo baseDeCalculo = DatabaseContext.BasesDeCalculo.FirstOrDefault(a => a.Codigo == command.Codigo);

                if (baseDeCalculo is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.Codigo));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.Codigo));
                }

                if (!baseDeCalculo.IndBaseInicial && command.IndBaseInicial)
                {
                    BaseDeCalculo baseDeCalculoInicial = DatabaseContext.BasesDeCalculo.FirstOrDefault(a => a.IndBaseInicial == true);

                    if (baseDeCalculoInicial != null)
                    {
                        baseDeCalculoInicial.DesmarcarBaseInicial();                        
                    }
                }

                Logger.LogInformation(Infra.Extensions.Logs.AtualizandoEntidade(entityName, command.Codigo));
                baseDeCalculo.Atualizar(command.Descricao, command.IndBaseInicial);

                if (baseDeCalculo.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(baseDeCalculo.Notifications.ToNotificationsString());
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

        public CommandResult Remover(int codigoBaseDeCalculo)
        {
            string entityName = "Base De Cálculo";
            string commandName = $"Removendo {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try
            {
                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_BASE_DE_CALCULO))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_BASE_DE_CALCULO, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, codigoBaseDeCalculo));
                BaseDeCalculo baseDeCalculo = DatabaseContext.BasesDeCalculo.FirstOrDefault(a => a.Codigo == codigoBaseDeCalculo);

                if (baseDeCalculo is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, codigoBaseDeCalculo));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, codigoBaseDeCalculo));
                }

                string commandRemocao = $"Validar remoção {entityName}";
                Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandRemocao));                
                if (baseDeCalculo.IndBaseInicial)
                {
                    string mensagem = "O registro selecionado possui a indicação de Base de Cálculo Inicial, transfira essa indicação para outro registro antes de excluí-lo.";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                if (DatabaseContext.PartesPedidosProcesso.Any(x => x.CodBaseCalculo == codigoBaseDeCalculo))
                {
                    string mensagem = "Não é possível excluir essa Base de Cálculo, pois ela está relacionada com Partes de Pedidos do Processo.";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }
 
                Logger.LogInformation(Infra.Extensions.Logs.RemovendoEntidade(entityName, codigoBaseDeCalculo));
                DatabaseContext.Remove(baseDeCalculo);

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
