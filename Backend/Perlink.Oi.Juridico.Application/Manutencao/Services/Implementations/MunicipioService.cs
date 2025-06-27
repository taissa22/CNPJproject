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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services.Implementations
{
    public class MunicipioService : IMunicipioService
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IMunicipioService> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }
        public MunicipioService(IDatabaseContext databaseContext, ILogger<IMunicipioService> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        public CommandResult Criar(CriarMunicipioCommand command)
        {
            string entityName = nameof(Municipio);
            string commandName = $"Criar {entityName}";

            try
            {
                Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

                command.Validate();
                command.Nome = command.Nome.ToUpper().Trim();
                if (command.Invalid)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.ComandoInvalido(commandName));
                    return CommandResult.Invalid(command.Notifications.ToNotificationsString());
                }

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ESTADO))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ESTADO, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                if (DatabaseContext.Municipios.Any(x => x.Nome.ToUpper().Trim() == command.Nome && x.EstadoId == command.EstadoId))
                {
                    string mensagem = "Já existe um municipio cadastrado com este nome.";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                Municipio municipio = Municipio.Criar(command.EstadoId, command.Nome);

                Logger.LogInformation(Infra.Extensions.Logs.CriandoEntidade(entityName));

                if (municipio.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(municipio.Notifications.ToNotificationsString());
                }

                DatabaseContext.Add(municipio);
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


        public CommandResult Atualizar(AtualizarMunicipioCommand command)
        {
            string entityName = nameof(Municipio);
            string commandName = $"Atualizar {entityName}";

            try
            {
                Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

                command.Validate();
                command.Nome = command.Nome.ToUpper().Trim();
                if (command.Invalid)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.ComandoInvalido(commandName));
                    return CommandResult.Invalid(command.Notifications.ToNotificationsString());
                }

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ESTADO))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ESTADO, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

               

                Municipio municipio = DatabaseContext.Municipios.FirstOrDefault(x => x.Id == command.Id);
                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, command.Id));

                if (municipio is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.Id));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.Id));
                }

                if (DatabaseContext.Municipios.Any(x => x.Nome.ToUpper().Trim() == command.Nome && municipio.Nome.ToUpper().Trim() != command.Nome && command.EstadoId == x.EstadoId))
                {
                    string mensagem = "Já existe um municipio cadastrado com este nome.";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                municipio.Atualizar(command.EstadoId, command.Nome);

                Logger.LogInformation(Infra.Extensions.Logs.AtualizandoEntidade(entityName, command.Id));

                if (municipio.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(municipio.Notifications.ToNotificationsString());
                }

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



        public CommandResult Remover(int id)
        {
            string entityName = nameof(Municipio);
            string commandName = $"Removendo {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try
            {
                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ESTADO))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ESTADO, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Municipio municipio = DatabaseContext.Municipios.FirstOrDefault(x => x.Id == id);
                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, id));

                if (municipio is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, id));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, id));
                }

                if (DatabaseContext.Processos.Any(p =>
                p.MunicipioId == id
                && p.EstadoId == municipio.EstadoId
                && p.TipoProcessoId == TipoProcesso.TRIBUTARIO_ADMINISTRATIVO.Id))
                {
                    string mensagem = "Não é possível excluir esse município, pois ele está relacionado com Processo " + TipoProcesso.TRIBUTARIO_ADMINISTRATIVO.Nome + ".";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                if (DatabaseContext.Processos.Any(p =>
                p.MunicipioId == id
                && p.EstadoId == municipio.EstadoId
                && p.TipoProcessoId == TipoProcesso.TRABALHISTA_ADMINISTRATIVO.Id))
                {
                    string mensagem = "Não é possível excluir esse município, pois ele está relacionado com Processo " + TipoProcesso.TRABALHISTA_ADMINISTRATIVO.Nome + ".";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                if (DatabaseContext.Processos.Any(p =>
                p.MunicipioId == id
                && p.EstadoId == municipio.EstadoId
                && p.TipoProcessoId == TipoProcesso.CIVEL_ADMINISTRATIVO.Id))
                {
                    string mensagem = "Não é possível excluir esse município, pois ele está relacionado com Processo " + TipoProcesso.CIVEL_ADMINISTRATIVO.Nome + ".";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                if (DatabaseContext.Processos.Any(p =>
                p.MunicipioId == id
                && p.EstadoId == municipio.EstadoId
                && p.TipoProcessoId == TipoProcesso.CRIMINAL_ADMINISTRATIVO.Id))
                {
                    string mensagem = "Não é possível excluir esse município, pois ele está relacionado com Processo " + TipoProcesso.CRIMINAL_ADMINISTRATIVO.Nome + ".";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                if (DatabaseContext.Processos.Any(p =>
                p.MunicipioId == id
                && p.EstadoId == municipio.EstadoId
                && p.TipoProcessoId == TipoProcesso.DENUNCIA_FISCAL.Id))
                {
                    string mensagem = "Não é possível excluir esse município, pois ele está relacionado com Processo " + TipoProcesso.DENUNCIA_FISCAL.Nome + ".";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                string commandRemocao = $"Validar remoção {entityName}";
                Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandRemocao));

                DatabaseContext.Remove(municipio);
                Logger.LogInformation(Infra.Extensions.Logs.RemovendoEntidade(entityName, id));

                DatabaseContext.SaveChanges();
                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade(entityName));

                Logger.LogInformation(Infra.Extensions.Logs.OperacaoFinalizada(commandName));
                return CommandResult.Valid();
            }
            catch (Exception ex)
            {
                if (ex.HResult == -2146233088 && ex.InnerException != null && ex.InnerException.HResult == -2147467259)
                {
                    string mensagem = "Não é possível excluir esse municipio, pois ele está relacionado com alguma tabela.";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }
                Logger.LogError(ex, Infra.Extensions.Logs.OperacaoComErro(commandName));
                return CommandResult.Invalid(ex.Message);
            }
        }

    }
}
