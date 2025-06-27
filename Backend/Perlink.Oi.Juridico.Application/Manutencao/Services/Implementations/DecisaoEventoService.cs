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
    internal class DecisaoEventoService : IDecisaoEventoService
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IDecisaoEventoService> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public DecisaoEventoService(IDatabaseContext databaseContext, ILogger<IDecisaoEventoService> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        public CommandResult Criar(CriarDecisaoEventoCommand command)
        {
            string entityName = "Decisao Evento";
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

                bool perdapotencial = (command.PerdaPotencial != null) && ( (command.PerdaPotencial.ToUpper() == "RE") || (command.PerdaPotencial.ToUpper() == "PR") || (command.PerdaPotencial.ToUpper() == "PO"));

                if (command.RiscoPerda.GetValueOrDefault() && !perdapotencial )
                {
                    Logger.LogInformation(Infra.Extensions.Logs.ComandoInvalido(commandName));
                    return CommandResult.Invalid("Risco de perda potencial não informado.");
                }

                if (command.DecisaoDefault)
                {
                    DecisaoEvento decisaodefault = DatabaseContext.DecisaoEventos.FirstOrDefault(x => x.EventoId == command.EventoId && x.DecisaoDefault);
                    if (decisaodefault != null)
                    {
                        decisaodefault.AtualizarDecisaoDefault(decisaodefault.Id.GetValueOrDefault(), decisaodefault.EventoId, false);
                        DatabaseContext.SaveChanges();
                    }
                }

                Logger.LogInformation(Infra.Extensions.Logs.CriandoEntidade(entityName));
                int? sequencialEvento = DatabaseContext.Eventos.FirstOrDefault(x => x.Id == command.EventoId).SequencialDecisao.GetValueOrDefault();
                if (sequencialEvento == 0)
                {
                    int? maxDecisaoId = DatabaseContext.DecisaoEventos.Where(x => x.EventoId == command.EventoId).Max(x => x.Id).GetValueOrDefault();
                    if (maxDecisaoId > 0)
                    {
                        sequencialEvento = maxDecisaoId;
                    }
                }

                DecisaoEvento decisaoevento = DecisaoEvento.Criar(sequencialEvento + 1, command.EventoId, command.Descricao.ToUpper(), command.RiscoPerda, command.PerdaPotencial,command.DecisaoDefault, command.ReverCalculo);                               

                DatabaseContext.Add(decisaoevento);
                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade(entityName));

                DatabaseContext.SaveChanges();
                Logger.LogInformation(Infra.Extensions.Logs.OperacaoFinalizada(commandName));

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, $"{command.EventoId},'' "));
                Evento evento = DatabaseContext.Eventos.FirstOrDefault(x => x.Id == command.EventoId);

                if (evento is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, $"{evento.Id}, {evento.Nome}"));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, $"{evento.Id}, {evento.Nome}"));
                }

                Logger.LogInformation(Infra.Extensions.Logs.AtualizandoEntidade(entityName, $"{evento.Id}, {evento.Nome}"));
                evento.Atualizar(evento.Id, evento.Nome.ToUpper(), true, evento.NotificarViaEmail, evento.EhPrazo.GetValueOrDefault(), evento.Ativo,
                                evento.EhTrabalhistaAdm, evento.EhCivel, evento.EhCivelEstrategico, evento.EhRegulatorio, evento.InstanciaId,
                                evento.PreencheMulta);

                if (evento.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida($"{evento.Id}, {evento.Nome}"));
                    return CommandResult.Invalid(evento.Notifications.ToNotificationsString());
                }

                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade($"{evento.Id}, {evento.Nome}"));
                DatabaseContext.SaveChanges();


                return CommandResult.Valid();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, Infra.Extensions.Logs.OperacaoComErro(commandName));
                return CommandResult.Invalid(ex.Message);
            }
        }

        public CommandResult Atualizar(AtualizarDecisaoEventoCommand command)
        {
            string entityName = "Decisao Evento";
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
               

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, $"{command.Id}, {command.Descricao}"));
                DecisaoEvento decisaoevento = DatabaseContext.DecisaoEventos.FirstOrDefault(x => x.Id == command.Id && x.EventoId == command.EventoId);

                if (decisaoevento is null) 
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, $"{command.Id}, {command.Descricao}"));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, $"{command.Id}, {command.Descricao}"));
                }


                if (command.DecisaoDefault)
                {
                    DecisaoEvento decisaodefault = DatabaseContext.DecisaoEventos.FirstOrDefault(x => x.EventoId == command.EventoId && x.DecisaoDefault);
                    if (decisaodefault != null)
                    {
                        decisaodefault.AtualizarDecisaoDefault(decisaodefault.Id.GetValueOrDefault(), decisaodefault.EventoId, false);
                        DatabaseContext.SaveChanges();
                    }
                }


                Logger.LogInformation(Infra.Extensions.Logs.AtualizandoEntidade(entityName, $"{command.Id}, {command.Descricao}"));
                decisaoevento.Atualizar(command.Id,command.EventoId,command.Descricao, command.RiscoPerda, command.PerdaPotencial,command.DecisaoDefault, command.ReverCalculo); 

                if (decisaoevento.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida($"{command.Id}, {command.Descricao}"));
                    return CommandResult.Invalid(decisaoevento.Notifications.ToNotificationsString());
                }

                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade($"{command.Id}, {command.Descricao}"));
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

        public CommandResult Remover(int decisaoId, int eventoId)
        {
            string entityName = "Decisão Evento";
            string commandName = $"Removendo {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try
            {
                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_EVENTO))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_EVENTO, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, $"{decisaoId}"));
                DecisaoEvento decisao = DatabaseContext.DecisaoEventos.FirstOrDefault(x => x.Id == decisaoId && x.EventoId == eventoId);

                if (decisao is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, $"{decisaoId}"));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, $"{decisaoId}"));
                }


                bool existe;

                existe = DatabaseContext.AndamentosDosProcessos.Where(x => x.Evento.Id == eventoId && x.DecisaoId == decisaoId).Count() > 0;

                if (existe)
                {
                    string mensagem = "Não é possível excluir o Evento, pois ele está relacionado com Andamento Processo";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }
                else
                {
                    existe = DatabaseContext.AndamentosPedidos.Where(x => x.EventoId == eventoId && x.DecisaoId == decisaoId).Count() > 0;

                    if (existe)
                    {
                        string mensagem = "Não é possível excluir o Evento, pois ele está relacionado com Andamento Pedido Processo";
                        Logger.LogInformation(mensagem);
                        return CommandResult.Invalid(mensagem);
                    }
                    else
                    {
                        existe = DatabaseContext.FaseProcessos.Where(x => x.EventoId == eventoId && x.DecisaoId == decisaoId).Count() > 0;

                        if (existe)
                        {
                            string mensagem = "Não é possível excluir o Evento, pois ele está relacionado com Fase Processo";
                            Logger.LogInformation(mensagem);
                            return CommandResult.Invalid(mensagem);
                        }
                        else
                        {
                            existe = DatabaseContext.DecisaoObjetoProcessos.Where(x => x.EventoId == eventoId && x.DecisaoId == decisaoId).Count() > 0;

                            if (existe)
                            {
                                string mensagem = "Não é possível excluir o Evento, pois ele está relacionado com Decisão Objeto Processos";
                                Logger.LogInformation(mensagem);
                                return CommandResult.Invalid(mensagem);
                            }
                            else
                            {
                                existe = DatabaseContext.ColunasRelatorioEficienciaEventos.Where(x => x.EventoId == eventoId && x.DecisaoId == decisaoId).Count() > 0;

                                if (existe)
                                {
                                    string mensagem = "Não é possível excluir o Evento, pois ele está relacionado com Colunas Relatório Eficiência Eventos";
                                    Logger.LogInformation(mensagem);
                                    return CommandResult.Invalid(mensagem);
                                }
                                else
                                {
                                    Evento evento = DatabaseContext.Eventos.FirstOrDefault(x => x.Id == decisao.EventoId);


                                    if (evento.EhCivel.GetValueOrDefault())
                                    {
                                        existe = DatabaseContext.MigracaoEventosCivelConsumidor.Where(x => x.EventoCivelConsumidorId == eventoId && x.DecisaoEventoCivelConsumidorId == decisaoId).Count() > 0;

                                        if (existe)
                                        {
                                            string mensagem = "Não é possível excluir o Evento, pois ele está relacionado com Migração Evento Civel Consumidor";
                                            Logger.LogInformation(mensagem);
                                            return CommandResult.Invalid(mensagem);
                                        }
                                    }

                                    if (evento.EhCivelEstrategico)
                                    {
                                        existe = DatabaseContext.MigracaoEventosCivelConsumidor.Where(x => x.EventoCivelEstrategicoId == eventoId && x.DecisaoEventoCivelEstrategicoId == decisaoId).Count() > 0;

                                        if (existe)
                                        {
                                            string mensagem = "Não é possível excluir o Evento, pois ele está relacionado com Migração Evento Civel Consumidor";
                                            Logger.LogInformation(mensagem);
                                            return CommandResult.Invalid(mensagem);
                                        }
                                        else
                                        {
                                            existe = DatabaseContext.MigracaoEventosCivelEstrategico.Where(x => x.EventoCivelEstrategicoId == eventoId && x.DecisaoCivelEstrategicoId == decisaoId).Count() > 0;

                                            if (existe)
                                            {
                                                string mensagem = "Não é possível excluir o Evento, pois ele está relacionado com Migração Evento Civel Estrátegico";
                                                Logger.LogInformation(mensagem);
                                                return CommandResult.Invalid(mensagem);
                                            }
                                        }
                                    }

                                    
                                }
                            }
                        }
                    }

                }


                Logger.LogInformation(Infra.Extensions.Logs.RemovendoEntidade(entityName, $"{decisaoId}"));
                DatabaseContext.Remove(decisao);

                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade($"{decisaoId}"));
                DatabaseContext.SaveChanges();

                EventoPossuiDecisao(DatabaseContext.DecisaoEventos.Where(x => x.EventoId == eventoId).Count() > 0, eventoId);

                Logger.LogInformation(Infra.Extensions.Logs.OperacaoFinalizada(commandName));
                return CommandResult.Valid();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, Infra.Extensions.Logs.OperacaoComErro(commandName));
                return CommandResult.Invalid(ex.Message);
            }
        }

        private void EventoPossuiDecisao(bool possui, int eventoId)
        {
            string entityName = "Evento";
            string commandName = $"Atualizando {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            Evento evento = DatabaseContext.Eventos.FirstOrDefault(x => x.Id == eventoId);
                      
            Logger.LogInformation(Infra.Extensions.Logs.AtualizandoEntidade(entityName, $"{evento.Id}, {evento.Nome}"));
            evento.Atualizar(evento.Id, evento.Nome.ToUpper(), possui, evento.NotificarViaEmail, evento.EhPrazo.GetValueOrDefault(), evento.Ativo,
                            evento.EhTrabalhistaAdm, evento.EhCivel, evento.EhCivelEstrategico, evento.EhRegulatorio, evento.InstanciaId,
                            evento.PreencheMulta);            

            Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade($"{evento.Id}, {evento.Nome}"));
            DatabaseContext.SaveChanges();
        }
    }
}