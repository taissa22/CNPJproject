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
    internal class TipoDocumentoService : ITipoDocumentoService
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<ITipoDocumentoService> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public TipoDocumentoService(IDatabaseContext context, ILogger<ITipoDocumentoService> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = context;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        public CommandResult Criar(CriarTipoDocumentoCommand command)
        {
            string entityName = "Tipo de documento";
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

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_TIPO_DOCUMENTO))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_TIPO_DOCUMENTO, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                TipoPrazo tipoPrazo = null;
                if (command.CodTipoPrazo.HasValue)
                {
                    tipoPrazo = DatabaseContext.TiposPrazos.FirstOrDefault(x => x.Id == command.CodTipoPrazo);
                }

                TipoProcesso tipoProcesso = TipoProcesso.PorId(command.CodTipoProcesso);

                if (DatabaseContext.TiposDocumentos.Any(x => x.Descricao.ToLower() == command.Descricao.ToLower() && x.CodTipoProcesso == command.CodTipoProcesso))
                {
                    string mensagem = "Já exite um tipo de documento cadastrado com esta descrição.";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                Logger.LogInformation(Infra.Extensions.Logs.CriandoEntidade(entityName));
                TipoDocumento tipoDocumento = TipoDocumento.Criar(command.Descricao, command.Ativo, tipoProcesso,
                    command.MarcadoCriacaoProcesso, command.IndPrioritarioFila, command.IndRequerDatAudiencia,
                    command.IndDocumentoProtocolo, tipoPrazo, command.IndDocumentoApuracao, command.IndEnviarAppPreposto);

                if (tipoDocumento.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(tipoDocumento.Notifications.ToNotificationsString());
                }

                DatabaseContext.Add(tipoDocumento);
                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade(entityName));

                if (command.IdEstrategico.HasValue)
                {
                    TipoDocumentoMigracao tipoDocumentoMigracaoConsumidor = TipoDocumentoMigracao.CriarTipoDocumentoMigracao(tipoDocumento.Id, command.IdEstrategico.Value);
                    DatabaseContext.Add(tipoDocumentoMigracaoConsumidor);

                }

                if (command.IdConsumidor.HasValue)
                {
                    TipoDocumentoMigracaoEstrategico tipoDocumentoMigracaoEstrategico = TipoDocumentoMigracaoEstrategico.CriarTipoDocumentoMigracaoEstrategico(command.IdConsumidor.Value, tipoDocumento.Id);
                    DatabaseContext.Add(tipoDocumentoMigracaoEstrategico);
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

        public CommandResult Atualizar(AtualizarTipoDocumentoCommand command)
        {
            string entityName = "Tipo de documento";
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

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_TIPO_DOCUMENTO))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_TIPO_DOCUMENTO, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, command.Id));
                TipoDocumento tipoDocumento = DatabaseContext.TiposDocumentos.FirstOrDefault(x => x.Id == command.Id);

                if (DatabaseContext.TiposDocumentos.Any(x => x.Descricao.ToLower() == command.Descricao.ToLower()
                    && x.Id != tipoDocumento.Id && x.CodTipoProcesso == tipoDocumento.CodTipoProcesso))
                {
                    string mensagem = "Já exite um tipo de documento cadastrado com esta descrição.";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }


                if (tipoDocumento is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.Id));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.Id));
                }

                TipoPrazo tipoPrazo = null;
                if (command.CodTipoPrazo.HasValue)
                {
                    tipoPrazo = DatabaseContext.TiposPrazos.FirstOrDefault(x => x.Id == command.CodTipoPrazo);
                }

                Logger.LogInformation(Infra.Extensions.Logs.AtualizandoEntidade(entityName, command.Id));
                tipoDocumento.Atualizar(command.Descricao, command.Ativo, command.MarcadoCriacaoProcesso,
                    command.IndPrioritarioFila, command.IndRequerDatAudiencia, command.IndDocumentoProtocolo,
                    tipoPrazo, command.IndDocumentoApuracao, command.IndEnviarAppPreposto);

                if (tipoDocumento.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(tipoDocumento.Notifications.ToNotificationsString());
                }

                TipoDocumentoMigracao tipoDocumentoMigracaoConsumidor = DatabaseContext.TipoDocumentoMigracao.FirstOrDefault(x => x.CodTipoDocCivelConsumidor == tipoDocumento.Id);

                if (tipoDocumentoMigracaoConsumidor != null)
                {
                    DatabaseContext.Remove(tipoDocumentoMigracaoConsumidor);
                }

                if (command.IdEstrategico.HasValue)
                {
                    var migTipoDocumento = TipoDocumentoMigracao.CriarTipoDocumentoMigracao(tipoDocumento.Id, command.IdEstrategico.Value);
                    DatabaseContext.Add(migTipoDocumento);
                }

                TipoDocumentoMigracaoEstrategico tipoDocumentoMigracaoEstrategico = DatabaseContext.TipoDocumentoMigracaoEstrategico.FirstOrDefault(x => x.CodTipoDocCivelEstrategico == tipoDocumento.Id);

                if (tipoDocumentoMigracaoEstrategico != null)
                {
                    DatabaseContext.Remove(tipoDocumentoMigracaoEstrategico);
                }

                if (command.IdConsumidor.HasValue)
                {
                    var migTipoDocumento = TipoDocumentoMigracaoEstrategico.CriarTipoDocumentoMigracaoEstrategico(command.IdConsumidor.Value, tipoDocumento.Id);
                    DatabaseContext.Add(migTipoDocumento);
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

        public CommandResult Remover(int id)
        {
            string entityName = "Tipo de documento";
            string commandName = $"Removendo {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try
            {
                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_TIPO_DOCUMENTO))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_TIPO_DOCUMENTO, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, id));
                TipoDocumento tipoDocumento = DatabaseContext.TiposDocumentos.FirstOrDefault(x => x.Id == id);

                if (tipoDocumento is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, id));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, id));
                }

                string commandRemocao = $"Validar remoção {entityName}";
                Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandRemocao));

                if (DatabaseContext.Protocolos.Any(p => p.CodTipoDocumento == tipoDocumento.Id))
                {
                    string mensagem = "Não será possível excluir o Tipo de Documento selecionado, pois se encontra relacionado com Protocolos.";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                if (DatabaseContext.DocumentosProcessos.Any(x => x.TipoDocumento.Id == tipoDocumento.Id))
                {
                    string mensagem = "Não será possível excluir o Tipo de Documento selecionado, pois se encontra relacionado com Documentos dos processos.";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                if (DatabaseContext.AndamentosProcessosDocumentos.Any(x => x.CodTipoDocumento == tipoDocumento.Id))
                {
                    string mensagem = "Não será possível excluir o Tipo de Documento selecionado, pois se encontra relacionado com Documentos dos andamentos dos processos.";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                if (DatabaseContext.ApuracaoProcessos.Any(x => x.TipoDocumentoId == tipoDocumento.Id))
                {
                    string mensagem = "Não será possível excluir o Tipo de Documento selecionado, pois se encontra relacionado com Apuração do Processo.";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                TipoDocumentoMigracao tipoDocumentoMigracaoConsumidor = DatabaseContext.TipoDocumentoMigracao.FirstOrDefault(x => x.CodTipoDocCivelConsumidor == tipoDocumento.Id);

                if (tipoDocumentoMigracaoConsumidor != null)
                {
                    DatabaseContext.Remove(tipoDocumentoMigracaoConsumidor);
                }

                TipoDocumentoMigracaoEstrategico tipoDocumentoMigracaoEstrategico = DatabaseContext.TipoDocumentoMigracaoEstrategico.FirstOrDefault(x => x.CodTipoDocCivelEstrategico == tipoDocumento.Id);

                if (tipoDocumentoMigracaoEstrategico != null)
                {
                    DatabaseContext.Remove(tipoDocumentoMigracaoEstrategico);
                }

                Logger.LogInformation(Infra.Extensions.Logs.RemovendoEntidade(entityName, id));
                DatabaseContext.Remove(tipoDocumento);


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
