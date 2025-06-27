using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Interface.Repository;
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
    internal class EventoService : IEventoService
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IEventoService> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }
        private readonly IParametroRepository parametroRepository;

        public EventoService(IDatabaseContext databaseContext, ILogger<IEventoService> logger, IParametroRepository parametroRepository, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
            this.parametroRepository = parametroRepository;
        }

        private int TipoProcesso(CriarEventoCommand command)
        {
            return command.EhCivel ? 1 : (command.EhRegulatorio ? 3 : (command.EhTrabalhistaAdm ? 6 : (command.EhCivelEstrategico ? 9 : (command.EhTrabalhista ? 2 : command.EhTributarioAdm ? 4 : command.EhTributarioJudicial ? 5 : 0))));
        }

        private int TipoProcesso(AtualizarEventoCommand command)
        {
            return command.EhCivel.GetValueOrDefault() ? 1 : (command.EhRegulatorio.GetValueOrDefault() ? 3 : (command.EhTrabalhistaAdm.GetValueOrDefault() ? 6 : (command.EhCivelEstrategico ? 9 : (command.EhTrabalhista.GetValueOrDefault() ? 2 : command.EhTributarioAdm ? 4 : command.EhTributarioJudicial ? 5 : 0))));
        }

        public CommandResult Criar(CriarEventoCommand command)
        {
            string entityName = "Evento";
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


                if (!UsuarioAtual.TemPermissaoPara(PermissaoPorTipoProcesso(TipoProcessoManutencao.PorId(TipoProcesso(command)))))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(PermissaoPorTipoProcesso(TipoProcessoManutencao.PorId(TipoProcesso(command))), UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Evento evento;

                Logger.LogInformation(Infra.Extensions.Logs.CriandoEntidade(entityName));
                if (TipoProcessoManutencao.PorId(TipoProcesso(command)) == TipoProcessoManutencao.TRABALHISTA)
                {
                    evento = Evento.CriarTrabalhista(command.Nome.ToUpper(),
                                             command.PossuiDecisao,
                                             command.EhPrazo,
                                             command.ReverCalculo,
                                             command.FinalizacaoEscritorio,
                                             command.FinalizacaoContabil,
                                             command.AlterarExcluir,
                                             command.Ativo);
                }
                else
                {
                    if ((TipoProcessoManutencao.PorId(TipoProcesso(command)) == TipoProcessoManutencao.TRIBUTARIO_ADMINISTRATIVO) || (TipoProcessoManutencao.PorId(TipoProcesso(command)) == TipoProcessoManutencao.TRIBUTARIO_JUDICIAL))
                    {
                        evento = Evento.CriarTributario(command.Nome.ToUpper(),
                                                        command.PossuiDecisao,
                                                        command.EhPrazo,
                                                        command.AtualizaEscritorio,
                                                        TipoProcesso(command),
                                                        command.Ativo);
                    }
                    else
                    {
                        evento = Evento.Criar(command.Nome.ToUpper(),
                                                 command.PossuiDecisao,
                                                 command.NotificarViaEmail,
                                                 command.EhPrazo,
                                                 command.Ativo,
                                                 command.EhTrabalhistaAdm,
                                                 command.EhCivel,
                                                 command.EhCivelEstrategico,
                                                 command.EhRegulatorio,
                                                 command.InstanciaId,
                                                 command.PreencheMulta);

                    }
                }

                if (evento.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(evento.Notifications.ToNotificationsString());
                }

                DatabaseContext.Add(evento);
                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade(entityName));

                if (command.EhCivel && command.IdEstrategico.HasValue)
                {
                    var migEventoConsumidor = MigracaoEventoCivelConsumidor.CriarMigracaoEventoCivelConsumidor(evento.Id, command.IdEstrategico.Value, null, null);
                    DatabaseContext.Add(migEventoConsumidor);
                }

                if (command.EhCivelEstrategico && command.IdConsumidor.HasValue)
                {
                    var migEventoEstrategico = MigracaoEventoCivelEstrategico.CriarMigracaoEventoCivelEstrategico(evento.Id, command.IdConsumidor.Value, null, null);
                    DatabaseContext.Add(migEventoEstrategico);
                }

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

        private string PermissaoPorTipoProcesso(TipoProcessoManutencao tipoProcesso)
        {
            switch (tipoProcesso.Id)
            {
                case 1: return Permissoes.ACESSAR_EVENTO_CIVEL_CONSUMIDOR;
                case 2: return Permissoes.ACESSAR_EVENTO_TRABALHISTA;
                case 3: return Permissoes.ACESSAR_EVENTO_ADMINSTRATIVO;
                case 4: return Permissoes.ACESSAR_EVENTO_TRIBUTARIO_ADMINISTRATIVO;
                case 5: return Permissoes.ACESSAR_EVENTO_TRIBUTARIO_JUDICIAL;
                case 6: return Permissoes.ACESSAR_EVENTO_TRABALHISTA_ADMINISTRATIVO;
                case 9: return Permissoes.ACESSAR_EVENTO_CIVEL_ESTRATEGICO;
                default: return "";
            }
        }

        public CommandResult Atualizar(AtualizarEventoCommand command)
        {
            string entityName = "Evento";
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


                if (!UsuarioAtual.TemPermissaoPara(PermissaoPorTipoProcesso(TipoProcessoManutencao.PorId(TipoProcesso(command)))))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(PermissaoPorTipoProcesso(TipoProcessoManutencao.PorId(TipoProcesso(command))), UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, $"{command.Id}, {command.Nome}"));
                Evento evento = DatabaseContext.Eventos.FirstOrDefault(x => x.Id == command.Id);

                if (evento is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, $"{command.Id}, {command.Nome}"));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, $"{command.Id}, {command.Nome}"));
                }

                if (command.CodTipoProcesso == TipoProcessoManutencao.CIVEL_ESTRATEGICO.Id && command.Ativo == false && evento.Id == Convert.ToInt32(parametroRepository.RecuperarPorNome("EV_DISTRIBUICAO").Conteudo))
                {
                    return CommandResult.Invalid("Não é possível inativar um evento de Distribuição");
                }

                if (evento.AlterarExcluir == false && evento.EhTrabalhista == true)
                {
                    return CommandResult.Invalid("Você não pode editar esse evento porque ele está configurado para não editar");
                }

                Logger.LogInformation(Infra.Extensions.Logs.AtualizandoEntidade(entityName, $"{command.Id}, {command.Nome}"));

                if (command.CodTipoProcesso == TipoProcessoManutencao.TRABALHISTA.Id)
                {
                    evento.AtualizarTrabalhista(command.Id,
                                                command.Nome.ToUpper(),
                                                command.PossuiDecisao,
                                                command.EhPrazo,
                                                command.ReverCalculo,
                                                command.FinalizacaoEscritorio,
                                                command.FinalizacaoContabil,
                                                command.AlterarExcluir);
                }
                else if (command.CodTipoProcesso == TipoProcessoManutencao.TRIBUTARIO_ADMINISTRATIVO.Id || command.CodTipoProcesso == TipoProcessoManutencao.TRIBUTARIO_JUDICIAL.Id)
                {
                    evento.AtualizarTributario(command.Id,
                                               command.Nome.ToUpper(),
                                               command.PossuiDecisao,
                                               command.EhPrazo,
                                               command.AtualizaEscritorio,
                                               command.CodTipoProcesso,
                                               command.Ativo);
                }
                else
                {
                    evento.Atualizar(command.Id, command.Nome.ToUpper(), command.PossuiDecisao, command.NotificarViaEmail, command.EhPrazo, command.Ativo,
                                 command.EhTrabalhistaAdm, command.EhCivel, command.EhCivelEstrategico, command.EhRegulatorio, command.InstanciaId,
                                 command.PreencheMulta);
                }


                if (evento.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida($"{command.Id}, {command.Nome}"));
                    return CommandResult.Invalid(evento.Notifications.ToNotificationsString());
                }

                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade($"{command.Id}, {command.Nome}"));

                if (command.EhCivel.HasValue)
                {
                    var migracaoCivelConsumidor = DatabaseContext.MigracaoEventosCivelConsumidor.FirstOrDefault(x => x.EventoCivelConsumidorId == command.Id);

                    if (migracaoCivelConsumidor != null)
                    {
                        DatabaseContext.Remove(migracaoCivelConsumidor);
                    }

                    if (command.IdEstrategico.HasValue)
                    {
                        var migEventoConsumidor = MigracaoEventoCivelConsumidor.CriarMigracaoEventoCivelConsumidor(evento.Id, command.IdEstrategico.Value, null, null);
                        DatabaseContext.Add(migEventoConsumidor);
                    }
                }

                if (command.EhCivelEstrategico)
                {
                    var migracaoCivelEstategico = DatabaseContext.MigracaoEventosCivelEstrategico.FirstOrDefault(x => x.EventoCivelEstrategicoId == command.Id);

                    if (migracaoCivelEstategico != null)
                    {
                        DatabaseContext.Remove(migracaoCivelEstategico);
                    }
                    if (command.IdConsumidor.HasValue)
                    {
                        var migEventoEstrategico = MigracaoEventoCivelEstrategico.CriarMigracaoEventoCivelEstrategico(evento.Id, command.IdConsumidor.Value, null, null);
                        DatabaseContext.Add(migEventoEstrategico);
                    }
                }

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

        public CommandResult AtualizarDependente(AtualizarEventoDependenteCommand command)
        {
            string entityName = "Evento";
            string commandName = $"Atualizar {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));
            //return CommandResult.Valid();
            try
            {
                command.Validate();
                if (command.Invalid)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.ComandoInvalido(commandName));
                    return CommandResult.Invalid(command.Notifications.ToNotificationsString());
                }

                if (!UsuarioAtual.TemPermissaoPara(PermissaoPorTipoProcesso(TipoProcessoManutencao.TRABALHISTA)))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(PermissaoPorTipoProcesso(TipoProcessoManutencao.TRABALHISTA), UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                for (int i = 0; i < command.Lista.Count; i++)
                {
                    EventoDependente evento;

                    if (command.Lista[i].Selecionado)
                    {
                        Logger.LogInformation(Infra.Extensions.Logs.CriandoEntidade(entityName));
                        evento = EventoDependente.Criar(command.Lista[i].Id, command.EventoId);

                        DatabaseContext.Add(evento);
                        Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade(entityName));
                    }
                    else
                    {
                        Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, $"{command.Lista[i].Id}, {command.Lista[i].Label}"));
                        evento = DatabaseContext.EventosDependentes.FirstOrDefault(x => x.EventoId == command.Lista[i].Id && x.EventoDependenteId == command.EventoId);

                        if (evento is null)
                        {
                            Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, $"{command.Lista[i].Id}, {command.Lista[i].Label}"));
                            return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, $"{command.Lista[i].Id}, {command.Lista[i].Label}"));
                        }

                        Logger.LogInformation(Infra.Extensions.Logs.RemovendoEntidade(entityName, $"{command.EventoId}"));
                        DatabaseContext.Remove(evento);
                    }

                    if (evento.HasNotifications)
                    {
                        Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida($"{command.Lista[i].Id}, {command.Lista[i].Label}"));
                        return CommandResult.Invalid(evento.Notifications.ToNotificationsString());
                    }
                }

                // Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade($"{command.Id}, {command.Nome}"));
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

        public CommandResult Remover(int tipoProcesso, int eventoId)
        {
            string entityName = "Evento";
            string commandName = $"Removendo {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try
            {
                if (!UsuarioAtual.TemPermissaoPara(PermissaoPorTipoProcesso(TipoProcessoManutencao.PorId(tipoProcesso))))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(PermissaoPorTipoProcesso(TipoProcessoManutencao.PorId(tipoProcesso)), UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, $"{eventoId}"));
                Evento evento = DatabaseContext.Eventos.FirstOrDefault(x => x.Id == eventoId);

                if (evento is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, $"{eventoId}"));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, $"{eventoId}"));
                }

                if (evento.Id == Convert.ToInt32(parametroRepository.RecuperarPorNome("EV_DISTRIBUICAO").Conteudo))
                {
                    return CommandResult.Invalid("Não é possível excluir um evento de Distribuição");
                }

                if (evento.AlterarExcluir == false && evento.EhTrabalhista == true)
                {
                    return CommandResult.Invalid("Você não pode excluir esse evento porque ele está configurado para não excluir");
                }


                bool existe;
                existe = DatabaseContext.DecisaoEventos.Where(x => x.EventoId == eventoId).Count() > 0;

                if (existe)
                {
                    string mensagem = "Não é possível excluir o Evento, pois ele está relacionado com Decisão Evento";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }
                else
                {

                    existe = DatabaseContext.AndamentosDosProcessos.Where(x => x.Evento.Id == eventoId).Count() > 0;

                    if (existe)
                    {
                        string mensagem = "Não é possível excluir o Evento, pois ele está relacionado com Andamento Processo";
                        Logger.LogInformation(mensagem);
                        return CommandResult.Invalid(mensagem);
                    }
                    else
                    {
                        existe = DatabaseContext.FaseProcessos.Where(x => x.EventoId == eventoId).Count() > 0;

                        if (existe)
                        {
                            string mensagem = "Não é possível excluir o Evento, pois ele está relacionado com Fase Processo";
                            Logger.LogInformation(mensagem);
                            return CommandResult.Invalid(mensagem);
                        }
                        else
                        {
                            existe = DatabaseContext.EventosDependentes.Where(x => x.EventoId == eventoId).Count() > 0;

                            if (existe)
                            {
                                string mensagem = "Não é possível excluir o Evento, pois ele está relacionado com Eventos Dependentes";
                                Logger.LogInformation(mensagem);
                                return CommandResult.Invalid(mensagem);
                            }
                            else
                            {
                                existe = DatabaseContext.ColunasRelatorioEficienciaEventos.Where(x => x.EventoId == eventoId).Count() > 0;

                                if (existe)
                                {
                                    string mensagem = "Não é possível excluir o Evento, pois ele está relacionado com Colunas Relatório Eficiência Eventos";
                                    Logger.LogInformation(mensagem);
                                    return CommandResult.Invalid(mensagem);
                                }
                                //Removido na US 162437
                                //else
                                //{
                                //    if (evento.EhCivel.GetValueOrDefault())
                                //    {
                                //        existe = DatabaseContext.MigracaoEventosCivelConsumidor.Where(x => x.EventoCivelConsumidorId == eventoId).Count() > 0;

                                //        if (existe)
                                //        {
                                //            string mensagem = "Não é possível excluir o Evento, pois ele está relacionado com Migração Evento Civel Consumidor";
                                //            Logger.LogInformation(mensagem);
                                //            return CommandResult.Invalid(mensagem);
                                //        }
                                //        else
                                //        {
                                //            existe = DatabaseContext.MigracaoEventosCivelEstrategico.Where(x => x.EventoCivelConsumidorId == eventoId).Count() > 0;

                                //            if (existe)
                                //            {
                                //                string mensagem = "Não é possível excluir o Evento, pois ele está relacionado com Migração Evento Civel Estratégico";
                                //                Logger.LogInformation(mensagem);
                                //                return CommandResult.Invalid(mensagem);
                                //            }
                                //        }

                                //    }

                                //    if (evento.EhCivelEstrategico)
                                //    {
                                //        existe = DatabaseContext.MigracaoEventosCivelConsumidor.Where(x => x.EventoCivelEstrategicoId == eventoId).Count() > 0;

                                //        if (existe)
                                //        {
                                //            string mensagem = "Não é possível excluir o Evento, pois ele está relacionado com Migração Evento Civel Consumidor";
                                //            Logger.LogInformation(mensagem);
                                //            return CommandResult.Invalid(mensagem);
                                //        }
                                //        else
                                //        {
                                //            existe = DatabaseContext.MigracaoEventosCivelEstrategico.Where(x => x.EventoCivelEstrategicoId == eventoId).Count() > 0;

                                //            if (existe)
                                //            {
                                //                string mensagem = "Não é possível excluir o Evento, pois ele está relacionado com Migração Evento Civel Estrátegico";
                                //                Logger.LogInformation(mensagem);
                                //                return CommandResult.Invalid(mensagem);
                                //            }
                                //        }
                                //    }
                                //}

                            }
                        }

                    }
                }

                if (tipoProcesso == 1)
                {
                    var migracaoCivelConsumidor = DatabaseContext.MigracaoEventosCivelConsumidor.FirstOrDefault(x => x.EventoCivelConsumidorId == evento.Id);

                    if (migracaoCivelConsumidor != null)
                    {
                        DatabaseContext.Remove(migracaoCivelConsumidor);
                    }
                }

                if (tipoProcesso == 9)
                {
                    var migracaoCivelEstrategico = DatabaseContext.MigracaoEventosCivelEstrategico.FirstOrDefault(x => x.EventoCivelEstrategicoId == evento.Id);

                    if (migracaoCivelEstrategico != null)
                    {
                        DatabaseContext.Remove(migracaoCivelEstrategico);
                    }
                }

                Logger.LogInformation(Infra.Extensions.Logs.RemovendoEntidade(entityName, $"{eventoId}"));
                DatabaseContext.Remove(evento);

                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade($"{eventoId}"));
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