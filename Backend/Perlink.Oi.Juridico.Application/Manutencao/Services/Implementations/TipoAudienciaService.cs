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
    internal class TipoAudienciaService : ITipoAudienciaService
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<ITipoAudienciaService> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public TipoAudienciaService(IDatabaseContext databaseContext, ILogger<ITipoAudienciaService> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        public CommandResult Criar(CriarTipoAudienciaCommand command)
        {
            string entityName = "Tipo de Audiência";
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

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_TIPO_AUDIENCIA))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_TIPO_AUDIENCIA, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                TipoProcesso tipoProcesso = TipoProcesso.PorId(command.TipoProcesso);

                
                var ehCivelConsumidor = tipoProcesso == TipoProcesso.CIVEL_CONSUMIDOR;
                var ehCivelEstrategico = tipoProcesso == TipoProcesso.CIVEL_ESTRATEGICO;
                var ehTrabalhista = tipoProcesso == TipoProcesso.TRABALHISTA;
                var ehTrabalhistaAdministrativo = tipoProcesso == TipoProcesso.TRABALHISTA_ADMINISTRATIVO;
                var ehTributarioAdministrativo = tipoProcesso == TipoProcesso.TRIBUTARIO_ADMINISTRATIVO;
                var ehTributarioJudicial = tipoProcesso == TipoProcesso.TRIBUTARIO_JUDICIAL;
                var ehCivelAdministrativo = tipoProcesso == TipoProcesso.CIVEL_ADMINISTRATIVO;
                var ehAdministrativo = tipoProcesso == TipoProcesso.ADMINISTRATIVO;
                var ehCriminalJudicial = tipoProcesso == TipoProcesso.CRIMINAL_JUDICIAL;
                var ehCriminalAdministrativo = tipoProcesso == TipoProcesso.CRIMINAL_ADMINISTRATIVO;
                var ehJuizadoEspecial = tipoProcesso == TipoProcesso.JEC;
                var ehProcon = tipoProcesso == TipoProcesso.PROCON;
                var ehPex = tipoProcesso == TipoProcesso.PEX;

                if (DatabaseContext.TipoAudiencias.Any(x => x.Descricao == command.Descricao && ((ehCivelConsumidor && x.Eh_CivelConsumidor) ||
                                                        (ehCivelEstrategico && x.Eh_CivelEstrategico) ||
                                                        (ehTrabalhista && x.Eh_Trabalhista) ||
                                                        (ehTrabalhistaAdministrativo && x.Eh_TrabalhistaAdministrativo) ||
                                                        (ehTributarioAdministrativo && x.Eh_TributarioAdministrativo) ||
                                                        (ehTributarioJudicial && x.Eh_TributarioJudicial) ||
                                                        (ehCivelAdministrativo && x.Eh_CivelAdministrativo) ||
                                                        (ehAdministrativo && x.Eh_Administrativo) ||
                                                        (ehCriminalJudicial && x.Eh_CriminalJudicial) ||
                                                        (ehCriminalAdministrativo && x.Eh_CriminalAdministrativo) ||
                                                        (ehJuizadoEspecial && x.Eh_JuizadoEspecial) ||
                                                        (ehProcon && x.Eh_Procon) ||
                                                        (ehPex && x.Eh_Pex))))
                {

                    string mensagem = $"Já existe um Tipo de Audiência com a descrição informada para esse Tipo de Processo.";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                Logger.LogInformation(Infra.Extensions.Logs.CriandoEntidade(entityName));
                TipoAudiencia tipoAudiencia = TipoAudiencia.Criar(command.Descricao, command.Ativo, tipoProcesso, command.Sigla,command.LinkVirtual);

                if (tipoAudiencia.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(tipoAudiencia.Notifications.ToNotificationsString());
                }

                DatabaseContext.Add(tipoAudiencia);
                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade(entityName));

                if (command.IdConsumidor.HasValue)
                {
                    MigracaoTipoAudienciaEstrategico migracaoTipoAudienciaEstrategico = MigracaoTipoAudienciaEstrategico.CriarMigracaoTipoAudienciaEstrategico(tipoAudiencia.Id, command.IdConsumidor.Value);
                    DatabaseContext.Add(migracaoTipoAudienciaEstrategico);
                }

                if (command.IdEstrategico.HasValue)
                {
                    MigracaoTipoAudienciaConsumidor migracaoTipoAudienciaConsumidor = MigracaoTipoAudienciaConsumidor.CriarMigracaoTipoAudienciaConsumidor(tipoAudiencia.Id, command.IdEstrategico.Value);
                    DatabaseContext.Add(migracaoTipoAudienciaConsumidor);
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

        public CommandResult Atualizar(AtualizarTipoAudienciaCommand command)
        {
            string entityName = "Tipos de Audiência";
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

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_TIPO_AUDIENCIA))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_TIPO_AUDIENCIA, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, command.CodigoTipoAudiencia));
                TipoAudiencia tipoAudiencia = DatabaseContext.TipoAudiencias.FirstOrDefault(a => a.Id == command.CodigoTipoAudiencia);

                if (tipoAudiencia is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.CodigoTipoAudiencia));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.CodigoTipoAudiencia));
                }

                TipoProcesso tipoProcesso = tipoAudiencia.TipoProcesso;

                var ehCivelConsumidor = tipoProcesso == TipoProcesso.CIVEL_CONSUMIDOR;
                var ehCivelEstrategico = tipoProcesso == TipoProcesso.CIVEL_ESTRATEGICO;
                var ehTrabalhista = tipoProcesso == TipoProcesso.TRABALHISTA;
                var ehTrabalhistaAdministrativo = tipoProcesso == TipoProcesso.TRABALHISTA_ADMINISTRATIVO;
                var ehTributarioAdministrativo = tipoProcesso == TipoProcesso.TRIBUTARIO_ADMINISTRATIVO;
                var ehTributarioJudicial = tipoProcesso == TipoProcesso.TRIBUTARIO_JUDICIAL;
                var ehCivelAdministrativo = tipoProcesso == TipoProcesso.CIVEL_ADMINISTRATIVO;
                var ehAdministrativo = tipoProcesso == TipoProcesso.ADMINISTRATIVO;
                var ehCriminalJudicial = tipoProcesso == TipoProcesso.CRIMINAL_JUDICIAL;
                var ehCriminalAdministrativo = tipoProcesso == TipoProcesso.CRIMINAL_ADMINISTRATIVO;
                var ehJuizadoEspecial = tipoProcesso == TipoProcesso.JEC;
                var ehProcon = tipoProcesso == TipoProcesso.PROCON;
                var ehPex = tipoProcesso == TipoProcesso.PEX;

                if (DatabaseContext.TipoAudiencias.Any(x => x.Descricao == command.Descricao && x.Id != tipoAudiencia.Id
                                                        && ((ehCivelConsumidor && x.Eh_CivelConsumidor) ||
                                                        (ehCivelEstrategico && x.Eh_CivelEstrategico) ||
                                                        (ehTrabalhista && x.Eh_Trabalhista) ||
                                                        (ehTrabalhistaAdministrativo && x.Eh_TrabalhistaAdministrativo) ||
                                                        (ehTributarioAdministrativo && x.Eh_TributarioAdministrativo) ||
                                                        (ehTributarioJudicial && x.Eh_TributarioJudicial) ||
                                                        (ehCivelAdministrativo && x.Eh_CivelAdministrativo) ||
                                                        (ehAdministrativo && x.Eh_Administrativo) ||
                                                        (ehCriminalJudicial && x.Eh_CriminalJudicial) ||
                                                        (ehCriminalAdministrativo && x.Eh_CriminalAdministrativo) ||
                                                        (ehJuizadoEspecial && x.Eh_JuizadoEspecial) ||
                                                        (ehProcon && x.Eh_Procon) ||
                                                        (ehPex && x.Eh_Pex))))
                {
                    string mensagem = $"Já existe um Tipo de Audiência com a descrição informada para esse Tipo de Processo.";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                Logger.LogInformation(Infra.Extensions.Logs.AtualizandoEntidade(entityName, command.CodigoTipoAudiencia));
                tipoAudiencia.Atualizar(command.Descricao, command.Ativo, command.Sigla,command.LinkVirtual);

                if (tipoAudiencia.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(tipoAudiencia.Notifications.ToNotificationsString());
                }

                MigracaoTipoAudienciaEstrategico migracaoTipoAudienciaEstrategico = DatabaseContext.MigracaoTipoAudienciaEstrategico.FirstOrDefault(x => x.CodTipoAudienciaCivelEstrat == tipoAudiencia.Id);

                if (migracaoTipoAudienciaEstrategico != null)
                {
                    DatabaseContext.Remove(migracaoTipoAudienciaEstrategico);
                }

                if (command.IdConsumidor.HasValue)
                {
                    var migTipoAudiencia = MigracaoTipoAudienciaEstrategico.CriarMigracaoTipoAudienciaEstrategico(tipoAudiencia.Id, command.IdConsumidor.Value);
                    DatabaseContext.Add(migTipoAudiencia);
                }

                MigracaoTipoAudienciaConsumidor migracaoTipoAudienciaConsumidor = DatabaseContext.MigracaoTipoAudienciaConsumidor.FirstOrDefault(x => x.CodTipoAudCivel == tipoAudiencia.Id);

                if (migracaoTipoAudienciaConsumidor != null)
                {
                    DatabaseContext.Remove(migracaoTipoAudienciaConsumidor);
                }

                if (command.IdEstrategico.HasValue)
                {
                    var migTipoAudiencia = MigracaoTipoAudienciaConsumidor.CriarMigracaoTipoAudienciaConsumidor(tipoAudiencia.Id, command.IdEstrategico.Value);
                    DatabaseContext.Add(migTipoAudiencia);
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

        public CommandResult Remover(int TipoAudienciaId)
        {
            string entityName = "Tipos de Audiência";
            string commandName = $"Removendo {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try
            {
                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_TIPO_AUDIENCIA))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_TIPO_AUDIENCIA, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, TipoAudienciaId));
                TipoAudiencia tipoAudiencia = DatabaseContext.TipoAudiencias.FirstOrDefault(a => a.Id == TipoAudienciaId);

                if (tipoAudiencia is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, TipoAudienciaId));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, TipoAudienciaId));
                }

                string commandRemocao = $"Validar remoção {entityName}";
                Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandRemocao));
                if (DatabaseContext.AudienciasDosProcessos.Any(x => x.TipoAudiencia == tipoAudiencia))
                {
                    string mensagem = "Não será possível excluir o Tipo de Audiência selecionado, pois se encontra relacionado com uma audiência do processo";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                MigracaoTipoAudienciaEstrategico migracaoTipoAudienciaEstrategico = DatabaseContext.MigracaoTipoAudienciaEstrategico.FirstOrDefault(x => x.CodTipoAudienciaCivelEstrat == tipoAudiencia.Id);

                if (migracaoTipoAudienciaEstrategico != null)
                {
                    DatabaseContext.Remove(migracaoTipoAudienciaEstrategico);
                }

                MigracaoTipoAudienciaConsumidor migracaoTipoAudienciaConsumidor = DatabaseContext.MigracaoTipoAudienciaConsumidor.FirstOrDefault(x => x.CodTipoAudCivel == tipoAudiencia.Id);

                if (migracaoTipoAudienciaConsumidor != null)
                {
                    DatabaseContext.Remove(migracaoTipoAudienciaConsumidor);
                }

                Logger.LogInformation(Infra.Extensions.Logs.RemovendoEntidade(entityName, TipoAudienciaId));
                DatabaseContext.Remove(tipoAudiencia);

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
