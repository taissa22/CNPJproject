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
    internal class TipoPrazoService : ITipoPrazoService
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<ITipoPrazoService> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public TipoPrazoService(IDatabaseContext databaseContext, ILogger<ITipoPrazoService> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        public CommandResult Criar(CriarTipoPrazoCommand command)
        {
            string entityName = "Tipo de Prazo";
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

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_TIPO_PRAZO))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_TIPO_PRAZO, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                TipoProcessoManutencao tipoProcesso = TipoProcessoManutencao.PorId(command.TipoProcesso);

                if ((bool)command.EhDocumento && tipoProcesso != TipoProcessoManutencao.CRIMINAL_ADMINISTRATIVO && tipoProcesso != TipoProcessoManutencao.CRIMINAL_JUDICIAL)
                {
                    string mensagem = $"Tipo de processo diferente do esperado para tipo prazo de documento";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                if ((bool)command.EhServico && tipoProcesso != TipoProcessoManutencao.CIVEL_CONSUMIDOR && tipoProcesso != TipoProcessoManutencao.JEC && tipoProcesso != TipoProcessoManutencao.PROCON)
                {
                    string mensagem = $"Tipo de processo diferente do esperado para tipo prazo de serviço";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                var ehCivelConsumidor = tipoProcesso == TipoProcessoManutencao.CIVEL_CONSUMIDOR;
                var ehCivelEstrategico = tipoProcesso == TipoProcessoManutencao.CIVEL_ESTRATEGICO;
                var ehTrabalhista = tipoProcesso == TipoProcessoManutencao.TRABALHISTA;
                var ehTributarioAdministrativo = tipoProcesso == TipoProcessoManutencao.TRIBUTARIO_ADMINISTRATIVO;
                var ehTributarioJudicial = tipoProcesso == TipoProcessoManutencao.TRIBUTARIO_JUDICIAL;
                var ehAdministrativo = tipoProcesso == TipoProcessoManutencao.ADMINISTRATIVO;
                var ehCriminalJudicial = tipoProcesso == TipoProcessoManutencao.CRIMINAL_JUDICIAL;
                var ehCriminalAdministrativo = tipoProcesso == TipoProcessoManutencao.CRIMINAL_ADMINISTRATIVO;
                var ehJuizadoEspecial = tipoProcesso == TipoProcessoManutencao.JEC;
                var ehProcon = tipoProcesso == TipoProcessoManutencao.PROCON;
                var ehPexJuizado = tipoProcesso == TipoProcessoManutencao.PEX_JUIZADO;
                var ehPexConsumidor = tipoProcesso == TipoProcessoManutencao.PEX_CONSUMIDOR;

                if (DatabaseContext.TiposPrazos.Any(x => x.Descricao == command.Descricao && ((ehCivelConsumidor && (bool)x.Eh_Civel_Consumidor) ||
                                                        (ehCivelEstrategico && x.Eh_Civel_Estrategico) ||
                                                        (ehTrabalhista && (bool)x.Eh_Trabalhista) ||
                                                        (ehTributarioAdministrativo && (bool)x.Eh_Tributario_Administrativo) ||
                                                        (ehTributarioJudicial && (bool)x.Eh_Tributario_Judicial) ||
                                                        (ehAdministrativo && x.Eh_Administrativo) ||
                                                        (ehCriminalJudicial && x.Eh_Criminal_Judicial) ||
                                                        (ehCriminalAdministrativo && x.Eh_Criminal_Administrativo) ||
                                                        (ehJuizadoEspecial && (bool)x.Eh_Juizado_Especial) ||
                                                        (ehProcon && x.Eh_Procon) ||
                                                        (ehPexJuizado && x.Eh_Pex_Juizado) ||
                                                        (ehPexConsumidor && x.Eh_Pex_Consumidor)))) {

                    string mensagem = $"A descrição <b>{command.Descricao}</b> já está cadastrada para o tipo de processo <b>{tipoProcesso.Descricao}</b>.";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                Logger.LogInformation(Infra.Extensions.Logs.CriandoEntidade(entityName));
                TipoPrazo tipoPrazo = TipoPrazo.Criar(command.Descricao, command.Ativo, tipoProcesso, command.EhServico, command.EhDocumento);

                if (tipoPrazo.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(tipoPrazo.Notifications.ToNotificationsString());
                }

                DatabaseContext.Add(tipoPrazo);
                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade(entityName));

                if (command.IdConsumidor.HasValue)
                {
                    MigracaoTipoPrazoEstrategico migracaoTipoPrazoEstrategico = MigracaoTipoPrazoEstrategico.CriarMigracaoTipoPrazoEstrategico(tipoPrazo.Id, command.IdConsumidor.Value);
                    DatabaseContext.Add(migracaoTipoPrazoEstrategico);
                }

                if (command.IdEstrategico.HasValue)
                {
                    MigracaoTipoPrazoConsumidor migracaoTipoPrazoConsumidor = MigracaoTipoPrazoConsumidor.CriarMigracaoTipoPrazoConsumidor(tipoPrazo.Id, command.IdEstrategico.Value);
                    DatabaseContext.Add(migracaoTipoPrazoConsumidor);
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

        public CommandResult Atualizar(AtualizarTipoPrazoCommand command)
        {
            string entityName = "Tipos de Prazo";
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

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_TIPO_PRAZO))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_TIPO_PRAZO, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, command.Id));
                TipoPrazo tipoPrazo = DatabaseContext.TiposPrazos.FirstOrDefault(a => a.Id == command.Id);

                if (tipoPrazo is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.Id));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.Id));
                }

                Logger.LogInformation(Infra.Extensions.Logs.AtualizandoEntidade(entityName, command.Id));
                tipoPrazo.Atualizar(command.Descricao, command.Ativo, command.EhServico, command.EhDocumento);

                if (tipoPrazo.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(tipoPrazo.Notifications.ToNotificationsString());
                }

                MigracaoTipoPrazoEstrategico migracaoTipoPrazoEstrategico = DatabaseContext.MigracaoTipoPrazoEstrategico.FirstOrDefault(x => x.CodTipoPrazoCivelEstrat == tipoPrazo.Id);

                if (migracaoTipoPrazoEstrategico != null)
                {
                    DatabaseContext.Remove(migracaoTipoPrazoEstrategico);
                }

                if (command.IdConsumidor.HasValue)
                {
                    var migTipoPrazo = MigracaoTipoPrazoEstrategico.CriarMigracaoTipoPrazoEstrategico(tipoPrazo.Id, command.IdConsumidor.Value);
                    DatabaseContext.Add(migTipoPrazo);
                }

                MigracaoTipoPrazoConsumidor migracaoTipoPrazoConsumidor = DatabaseContext.MigracaoTipoPrazoConsumidor.FirstOrDefault(x => x.CodTipoPrazoCivel == tipoPrazo.Id);

                if (migracaoTipoPrazoConsumidor != null)
                {
                    DatabaseContext.Remove(migracaoTipoPrazoConsumidor);
                }

                if (command.IdEstrategico.HasValue)
                {
                    var migTipoPrazo = MigracaoTipoPrazoConsumidor.CriarMigracaoTipoPrazoConsumidor(tipoPrazo.Id, command.IdEstrategico.Value);
                    DatabaseContext.Add(migTipoPrazo);
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

        public CommandResult Remover(int tipoPrazoId)
        {
            string entityName = "Tipos de Prazo";
            string commandName = $"Removendo {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try
            {
                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_TIPO_PRAZO))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_TIPO_PRAZO, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, tipoPrazoId));
                TipoPrazo tipoPrazo = DatabaseContext.TiposPrazos.FirstOrDefault(a => a.Id == tipoPrazoId);

                if (tipoPrazo is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, tipoPrazoId));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, tipoPrazoId));
                }

                string commandRemocao = $"Validar remoção {entityName}";
                Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandRemocao));
                if (DatabaseContext.PrazosDoProcesso.Any(x => x.TipoPrazo == tipoPrazo))
                {
                    string mensagem = "Não será possível excluir o Tipo de Prazo selecionado, pois se encontra relacionado com um prazo do processo";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                MigracaoTipoPrazoEstrategico migracaoTipoPrazoEstrategico = DatabaseContext.MigracaoTipoPrazoEstrategico.FirstOrDefault(x => x.CodTipoPrazoCivelEstrat == tipoPrazo.Id);

                if (migracaoTipoPrazoEstrategico != null)
                {
                    DatabaseContext.Remove(migracaoTipoPrazoEstrategico);
                }

                MigracaoTipoPrazoConsumidor migracaoTipoPrazoConsumidor = DatabaseContext.MigracaoTipoPrazoConsumidor.FirstOrDefault(x => x.CodTipoPrazoCivel == tipoPrazo.Id);

                if (migracaoTipoPrazoConsumidor != null)
                {
                    DatabaseContext.Remove(migracaoTipoPrazoConsumidor);
                }

                Logger.LogInformation(Infra.Extensions.Logs.RemovendoEntidade(entityName, tipoPrazoId));
                DatabaseContext.Remove(tipoPrazo);

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
