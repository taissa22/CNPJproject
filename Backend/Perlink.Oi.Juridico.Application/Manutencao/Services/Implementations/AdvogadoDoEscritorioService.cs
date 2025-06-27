using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;
using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Constants;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services.Implementations
{
    internal class AdvogadoDoEscritorioService : IAdvogadoDoEscritorioService
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IAdvogadoDoEscritorioService> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public AdvogadoDoEscritorioService(IDatabaseContext databaseContext, ILogger<IAdvogadoDoEscritorioService> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        public CommandResult Criar(CriarAdvogadoDoEscritorioCommand command)
        {
            string entityName = "Advogado Do Escritorio";
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

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ESCRITORIO))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ESCRITORIO, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }


                if (command.EhContato)
                {
                    var contato = DatabaseContext.AdvogadosDosEscritorios.FirstOrDefault(x => x.EhContato && x.EscritorioId == command.EscritorioId);
                    if (contato != null)
                    {
                        contato.AtualizarContato(false);
                    }
                }


                var sequencial = DatabaseContext.Escritorios.Where(x => x.Id == command.EscritorioId).Max(x => x.SeqAdvogado);
               

                if ((sequencial == 0) || (sequencial == null))
                {
                    sequencial = 1;
                }
                else
                {
                    sequencial += 1;
                }


                var taCadastrado = DatabaseContext.AdvogadosDosEscritorios.Where(x => x.EstadoId == command.Estado && x.NumeroOAB == command.NumeroOAB && x.EscritorioId == command.EscritorioId).Any();
                if (taCadastrado)
                {
                    string retorno = "Advogado já cadastrado para este escritório";
                    Logger.LogInformation(retorno);
                    return CommandResult.Invalid(retorno);
                }
                        
                Logger.LogInformation(Infra.Extensions.Logs.CriandoEntidade(entityName));
                AdvogadoDoEscritorio advogado = AdvogadoDoEscritorio.Criar(sequencial.GetValueOrDefault(), command.EscritorioId,  command.Nome.ToUpper(),command.EhContato,command.Telefone, command.TelefoneDDD,command.Email,command.Estado,command.NumeroOAB);

                if (advogado.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(advogado.Notifications.ToNotificationsString());
                }
              
                DatabaseContext.Add(advogado);
                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade(entityName));

                DatabaseContext.SaveChanges();

                Escritorio escritorio = DatabaseContext.Escritorios.FirstOrDefault(x => x.Id == command.EscritorioId);
                escritorio.AtualizarSequecialAdvogado(sequencial.GetValueOrDefault());
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

        public CommandResult<PaginatedQueryResult<AdvogadoDoEscritorio>> Atualizar(AtualizarAdvogadoDoEscritorioCommand command)
        {
            string entityName = "Advogado do Escritório";
            string commandName = $"Atualizar {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try
            {
                command.Validate();
                if (command.Invalid)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.ComandoInvalido(commandName));
                    return CommandResult<PaginatedQueryResult<AdvogadoDoEscritorio>>.Invalid(command.Notifications.ToNotificationsString());

                }

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ESCRITORIO))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ESCRITORIO, UsuarioAtual.Login));
                    return CommandResult<PaginatedQueryResult<AdvogadoDoEscritorio>>.Invalid(command.Notifications.ToNotificationsString());

                }              

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, $"{command.Id}, {command.Nome}"));
                AdvogadoDoEscritorio advogado = DatabaseContext.AdvogadosDosEscritorios.FirstOrDefault(x => x.Id == command.Id && x.EscritorioId == command.EscritorioId);

                if (advogado is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, $"{command.Id}, {command.Nome}"));
                    return CommandResult<PaginatedQueryResult<AdvogadoDoEscritorio>>.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, $"{command.Id}, {command.Nome}"));
                }


                var taCadastrado = DatabaseContext.AdvogadosDosEscritorios.Where(x => x.EstadoId == command.Estado && x.NumeroOAB == command.NumeroOAB && x.EscritorioId == command.EscritorioId && x.Id != command.Id).Any();
                if (taCadastrado)
                {
                    string retorno = "Advogado já cadastrado para este escritório";
                    Logger.LogInformation(retorno);
                    return CommandResult<PaginatedQueryResult<AdvogadoDoEscritorio>>.Invalid(retorno);
                }

                if (command.EhContato)
                {
                    var contato = DatabaseContext.AdvogadosDosEscritorios.FirstOrDefault(x => x.EhContato && (x.Id == command.Id) && x.EscritorioId == command.EscritorioId);
                    if (contato != null)
                    {
                        contato.AtualizarContato(false);
                    }
                }

                Logger.LogInformation(Infra.Extensions.Logs.AtualizandoEntidade(entityName, $"{command.Id}, {command.Nome}"));
                advogado.Atualizar(command.EscritorioId, command.Id,command.Nome.ToUpper(),command.EhContato,command.Telefone, command.TelefoneDDD, command.Email, command.Estado, command.NumeroOAB);

                if (advogado.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida($"{command.Id}, {command.Nome}"));
                    return CommandResult<PaginatedQueryResult<AdvogadoDoEscritorio>>.Invalid(advogado.Notifications.ToNotificationsString());
                }

                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade($"{command.Id}, {command.Nome}"));
                DatabaseContext.SaveChanges();               

                Logger.LogInformation(Infra.Extensions.Logs.OperacaoFinalizada(commandName));
                return CommandResult<PaginatedQueryResult<AdvogadoDoEscritorio>>.Valid(null);                   
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, Infra.Extensions.Logs.OperacaoComErro(commandName));
                return CommandResult<PaginatedQueryResult<AdvogadoDoEscritorio>>.Invalid(ex.Message);
            }
        }

        public CommandResult Remover(int Id, int escritorioId)
        {
            string entityName = "advogado";
            string commandName = $"Removendo {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try
            {
                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ESCRITORIO))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ESCRITORIO, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, $"{Id}"));
                AdvogadoDoEscritorio advogado = DatabaseContext.AdvogadosDosEscritorios.FirstOrDefault(x => x.Id == Id && x.EscritorioId == escritorioId);

                if (advogado is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, $"{Id}"));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, $"{Id}"));
                }

                var temProcesso = DatabaseContext.Processos.Where(x => x.EscritorioId == escritorioId && x.AdvogadoId == Id).Any();
                if (temProcesso)
                {
                    string retorno = "Não é possível excluir esse advogado, pois ele está relacionado com Processos";
                    Logger.LogInformation(retorno);
                    return CommandResult.Invalid(retorno);   
                }

                
                var temAudiencia = DatabaseContext.AudienciasDosProcessos.Where(x => x.EscritorioId == escritorioId && x.AdvogadoEscritorioId == Id).Any();
                if (temAudiencia)
                {
                    string retorno = "Não é possível excluir esse advogado, pois ele está relacionado com Audiência Processos";
                    Logger.LogInformation(retorno);
                    return CommandResult.Invalid(retorno);
                }

                Logger.LogInformation(Infra.Extensions.Logs.RemovendoEntidade(entityName, $"{Id}"));
                DatabaseContext.Remove(advogado);

                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade($"{Id}"));
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