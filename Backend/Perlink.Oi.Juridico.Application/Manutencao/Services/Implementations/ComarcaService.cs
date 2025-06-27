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
    internal class ComarcaService : IComarcaService
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IComarcaService> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public ComarcaService(IDatabaseContext databaseContext, ILogger<IComarcaService> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        public CommandResult Criar(CriarComarcaCommand command)
        {
            string entityName = "Comarca";
            string commandName = $"Criar {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try
            {
                command.Validate();
                command.Nome = command.Nome.RemoveAccents().ToUpper().Trim();
                if (command.Invalid)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.ComandoInvalido(commandName));
                    return CommandResult.Invalid(command.Notifications.ToNotificationsString());
                }

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_COMARCA))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_COMARCA, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Estado estado = DatabaseContext.Estados.FirstOrDefault(x => x.Id == command.EstadoId);

                Comarca comarca = Comarca.Criar(command.Nome, estado, command.EscritorioCivelId, command.EscritorioTrabalhistaId,
                                               command.ProfissionalCivelEstrategicoId, command.ComarcaBBId);

                Logger.LogInformation(Infra.Extensions.Logs.CriandoEntidade(entityName));

                if (comarca.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(comarca.Notifications.ToNotificationsString());
                }

                if (DatabaseContext.Comarcas.Any(c => c.Estado.Id == command.EstadoId && c.Nome.RemoveAccents().ToUpper().Trim() == command.Nome))
                {
                    string mensagem = "Já existe uma comarca com o mesmo nome para o estado.";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                DatabaseContext.Add(comarca);
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

        public CommandResult Atualizar(AtualizarComarcaCommand command)
        {
            string entityName = "Comarca";
            string commandName = $"Atualizar {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try
            {
                command.Validate();
                command.Nome = command.Nome.RemoveAccents().ToUpper().Trim();
                if (command.Invalid)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.ComandoInvalido(commandName));
                    return CommandResult.Invalid(command.Notifications.ToNotificationsString());
                }

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_COMARCA))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_COMARCA, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }


                Comarca comarca = DatabaseContext.Comarcas.FirstOrDefault(x => x.Id == command.Id);
                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, command.Id));

                if (comarca is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.Id));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.Id));
                }

                if (DatabaseContext.Comarcas.Any(x => x.Nome.RemoveAccents().ToUpper().Trim() == command.Nome && x.Estado.Id == command.EstadoId && x.Id != command.Id))
                {
                    string mensagem = "Já existe uma comarca com o mesmo nome para o estado.";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                Estado estado = DatabaseContext.Estados.FirstOrDefault(x => x.Id == command.EstadoId);

                comarca.Atualizar(command.Nome, estado, command.EscritorioCivelId, command.EscritorioTrabalhistaId,
                command.ProfissionalCivelEstrategicoId, command.ComarcaBBId);

                Logger.LogInformation(Infra.Extensions.Logs.AtualizandoEntidade(entityName, command.Id));

                if (comarca.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(comarca.Notifications.ToNotificationsString());
                }
                DatabaseContext.SaveChanges();
                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade(entityName));

                var a =  DatabaseContext.ExecuteSqlInterpolated($"BEGIN JUR.SP_AVALIA_RISCO_PERDA('COMARCA', {comarca.Id}, ''); END;");

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
            string entityName = nameof(Comarca);
            string commandName = $"Removendo {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try
            {
                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_COMARCA))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_COMARCA, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                if (DatabaseContext.Processos.Any(p => p.ComarcaId == id))
                {
                    string mensagem = "Não será possivel excluir a comarca selecionada, pois se encontra relacionada com um " + nameof(Processo) + ".";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                if (DatabaseContext.GrupoJuizadoVaras.Any(p => p.ComarcaId == id))
                {
                    string mensagem = "Não será possivel excluir a comarca selecionada, pois se encontra relacionada com um " + nameof(GrupoJuizadoVara) + ".";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                if (DatabaseContext.ProcessoCpfCnpjs.Any(p => p.ComarcaId == id))
                {
                    string mensagem = "Não será possivel excluir a comarca selecionada, pois se encontra relacionada com um " + nameof(ProcessoCpfCnpj) + ".";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                if (DatabaseContext.AlocacoesPrepostos.Any(p => p.ComarcaId == id))
                {
                    string mensagem = "Não será possivel excluir a comarca selecionada, pois se encontra relacionada com um " + nameof(AlocacaoPreposto) + ".";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                if (DatabaseContext.ProcessosConexos.Any(p => p.ComarcaId == id))
                {
                    string mensagem = "Não será possivel excluir a comarca selecionada, pois se encontra relacionada com um " + nameof(ProcessoConexo) + ".";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                if (DatabaseContext.Protocolos.Any(p => p.ComarcaId == id))
                {
                    string mensagem = "Não será possivel excluir a comarca selecionada, pois se encontra relacionada com um " + nameof(Protocolo) + ".";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                Comarca comarca = DatabaseContext.Comarcas.FirstOrDefault(x => x.Id == id);
                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, id));

                if (comarca is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, id));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, id));
                }

                DatabaseContext.Varas.Where(x => x.ComarcaId == comarca.Id).ToList().ForEach(vara =>
                {
                    DatabaseContext.Remove(vara);
                    Logger.LogInformation(Infra.Extensions.Logs.RemovendoEntidade(nameof(Vara), $"{vara.VaraId}, {vara.ComarcaId}, {vara.TipoVaraId}"));
                });

                string commandRemocao = $"Validar remoção {entityName}";
                Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandRemocao));

                DatabaseContext.Remove(comarca);
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
