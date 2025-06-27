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

namespace Perlink.Oi.Juridico.Application.Manutencao.Services.Implementations
{
    internal class JurosVigenciasCiveisService : IJurosVigenciasCiveisService
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IJurosVigenciasCiveisService> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public JurosVigenciasCiveisService(IDatabaseContext databaseContext, ILogger<IJurosVigenciasCiveisService> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        public CommandResult Criar(CriarJurosVigenciasCiveisCommand command)
        {
            string entityName = "Juros Vigências Cíveis";
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

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_JUROS_VIGENCIAS_CIVEIS))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_JUROS_VIGENCIAS_CIVEIS, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                TipoProcesso tipoProcesso = TipoProcesso.PorId(command.CodTipoProcesso);

                if (DatabaseContext.JurosCorrecoesProcessos.Any(x => x.TipoProcesso == tipoProcesso && x.DataVigencia.Date >= command.DataVigencia.Date))
                {
                    string mensagem = $"Não é permitido incluir vigências com data menor ou igual a de maior data já cadastrada.";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                Logger.LogInformation($"Criando Juros Correção Processo com a chave: Código tipo de proceso: {command.CodTipoProcesso} e Data de início de vigência: {command.DataVigencia}");
                JurosCorrecaoProcesso jurosCorrecaoProcesso = JurosCorrecaoProcesso.Criar(tipoProcesso, command.DataVigencia, command.ValorJuros);

                if (jurosCorrecaoProcesso.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(jurosCorrecaoProcesso.Notifications.ToNotificationsString());
                }

                DatabaseContext.Add(jurosCorrecaoProcesso);
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

        public CommandResult Atualizar(AtualizarJurosVigenciasCiveisCommand command)
        {
            string entityName = "Juros Vigências Cíveis";
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

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_JUROS_VIGENCIAS_CIVEIS))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_JUROS_VIGENCIAS_CIVEIS, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                TipoProcesso tipoProcesso = TipoProcesso.PorId(command.CodTipoProcesso);

                Logger.LogInformation($"Obterndo Juros Correção Processo com a chave: Código tipo de proceso: {command.CodTipoProcesso} e Data de início de vigência: {command.DataVigencia}");
                JurosCorrecaoProcesso jurosCorrecaoProcesso = DatabaseContext.JurosCorrecoesProcessos.FirstOrDefault(x => x.DataVigencia == command.DataVigencia && x.TipoProcesso == tipoProcesso);

                if (jurosCorrecaoProcesso is null)
                {
                    string mensagem = $"Juros Correção Processo não encontrado para a chave: Código tipo de proceso: {command.CodTipoProcesso} e Data de início de vigência: {command.DataVigencia}";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                Logger.LogInformation($"Atualizando Juros Correção Processo com a chave: Código tipo de proceso: {command.CodTipoProcesso} e Data de início de vigência: {command.DataVigencia}");
                jurosCorrecaoProcesso.Atualizar(command.ValorJuros);

                if (jurosCorrecaoProcesso.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(jurosCorrecaoProcesso.Notifications.ToNotificationsString());
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

        public CommandResult Remover(int codigoTipoProcesso, DateTime dataVigencia)
        {
            string entityName = "Juros Vigências Cíveis";
            string commandName = $"Removendo {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try
            {
                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_JUROS_VIGENCIAS_CIVEIS))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_JUROS_VIGENCIAS_CIVEIS, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                TipoProcesso tipoProcesso = TipoProcesso.PorId(codigoTipoProcesso);

                Logger.LogInformation($"Obterndo Juros Correção Processo com a chave: Código tipo de proceso: {codigoTipoProcesso} e Data de início de vigência: {dataVigencia}");
                JurosCorrecaoProcesso jurosCorrecaoProcesso = DatabaseContext.JurosCorrecoesProcessos.FirstOrDefault(x => x.DataVigencia == dataVigencia && x.TipoProcesso == tipoProcesso);

                if (jurosCorrecaoProcesso is null)
                {
                    string mensagem = $"Juros Correção Processo não encontrado para a chave: Código tipo de proceso: {codigoTipoProcesso} e Data de início de vigência: {dataVigencia}";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                string commandRemocao = $"Validar remoção {entityName}";
                Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandRemocao));                

                Logger.LogInformation($"Removendo Juros Correção Processo com a chave: Código tipo de proceso: {codigoTipoProcesso} e Data de início de vigência: {dataVigencia}");
                DatabaseContext.Remove(jurosCorrecaoProcesso);

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