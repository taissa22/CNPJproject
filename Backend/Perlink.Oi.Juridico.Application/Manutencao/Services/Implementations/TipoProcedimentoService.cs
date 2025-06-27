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
using System;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services.Implementations
{
    internal class TipoProcedimentoService : ITipoProcedimentoService
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<ITipoProcedimentoService> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public TipoProcedimentoService(IDatabaseContext databaseContext, ILogger<ITipoProcedimentoService> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        public CommandResult Criar(CriarTipoProcedimentoCommand command)
        {
            string entityName = "Tipo de procedimento";
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

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_TIPO_PROCEDIMENTO))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_TIPO_PROCEDIMENTO, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                TipoDeParticipacao tipoParticipacao1 = DatabaseContext.TiposDeParticipacoes.FirstOrDefault(x => x.Codigo == command.CodTipoParticipacao1);
                TipoDeParticipacao tipoParticipacao2 = DatabaseContext.TiposDeParticipacoes.FirstOrDefault(x => x.Codigo == command.CodTipoParticipacao2);

                TipoProcesso tipoProcesso = TipoProcesso.PorId(command.CodTipoProcesso);

                bool existeMesmaDescricao = false;

                switch (tipoProcesso.Id)
                {
                    case 3: //Administrativo
                        existeMesmaDescricao = DatabaseContext.Procedimentos.Any(x => x.Descricao.ToUpper() == command.Descricao.ToUpper() && x.IndAdministrativo.GetValueOrDefault());
                        break;

                    case 4: //Tributario Administrativo
                        existeMesmaDescricao = DatabaseContext.Procedimentos.Any(x => x.Descricao.ToUpper() == command.Descricao.ToUpper() && x.IndTributario.GetValueOrDefault());
                        break;

                    case 6: // Trabalhista Administrativo
                        existeMesmaDescricao = DatabaseContext.Procedimentos.Any(x => x.Descricao.ToUpper() == command.Descricao.ToUpper() && x.IndTrabalhistaAdm.GetValueOrDefault());
                        break;

                    case 12: //Cível Administrativo
                        existeMesmaDescricao = DatabaseContext.Procedimentos.Any(x => x.Descricao.ToUpper() == command.Descricao.ToUpper() && x.IndCivelAdm);
                        break;

                    case 14: //Criminal Administrativo
                        existeMesmaDescricao = DatabaseContext.Procedimentos.Any(x => x.Descricao.ToUpper() == command.Descricao.ToUpper() && x.IndCriminalAdm);
                        break;
                }

                if (existeMesmaDescricao)
                {
                    string mensagem = $"Já existe um procedimento com o mesmo nome para o tipo de processo selecionado.";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                Logger.LogInformation(Infra.Extensions.Logs.CriandoEntidade(entityName));
                Procedimento procedimento = Procedimento.Criar(command.Descricao, command.IndAtivo, tipoParticipacao1, tipoParticipacao2, command.IndOrgao1.GetValueOrDefault(), command.IndOrgao2.GetValueOrDefault(),
                    command.IndProvisionado.GetValueOrDefault(), command.IndPoloPassivoUnico.GetValueOrDefault(), tipoProcesso);

                if (procedimento.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(procedimento.Notifications.ToNotificationsString());
                }

                DatabaseContext.Add(procedimento);
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

        public CommandResult Atualizar(AtualizarTipoProcedimentoCommand command)
        {
            string entityName = "Tipo de procedimento";
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

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_TIPO_PROCEDIMENTO))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_TIPO_PROCEDIMENTO, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                TipoDeParticipacao tipoParticipacao1 = DatabaseContext.TiposDeParticipacoes.FirstOrDefault(x => x.Codigo == command.CodTipoParticipacao1);
                TipoDeParticipacao tipoParticipacao2 = DatabaseContext.TiposDeParticipacoes.FirstOrDefault(x => x.Codigo == command.CodTipoParticipacao2);

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, command.Codigo));
                Procedimento procedimento = DatabaseContext.Procedimentos.FirstOrDefault(x => x.Codigo == command.Codigo);

                bool existeMesmaDescricao = false;

                switch (procedimento.TipoProcesso.Id)
                {
                    case 3: //Administrativo
                        existeMesmaDescricao = DatabaseContext.Procedimentos.Any(x => x.Descricao.ToUpper() == command.Descricao.ToUpper() && x.IndAdministrativo.GetValueOrDefault() && x.Codigo != procedimento.Codigo);
                        break;

                    case 4: //Tributario Administrativo
                        existeMesmaDescricao = DatabaseContext.Procedimentos.Any(x => x.Descricao.ToUpper() == command.Descricao.ToUpper() && x.IndTributario.GetValueOrDefault() && x.Codigo != procedimento.Codigo);
                        break;

                    case 6: // Trabalhista Administrativo
                        existeMesmaDescricao = DatabaseContext.Procedimentos.Any(x => x.Descricao.ToUpper() == command.Descricao.ToUpper() && x.IndTrabalhistaAdm.GetValueOrDefault() && x.Codigo != procedimento.Codigo);
                        break;

                    case 12: //Cível Administrativo
                        existeMesmaDescricao = DatabaseContext.Procedimentos.Any(x => x.Descricao.ToUpper() == command.Descricao.ToUpper() && x.IndCivelAdm && x.Codigo != procedimento.Codigo);
                        break;

                    case 14: //Criminal Administrativo
                        existeMesmaDescricao = DatabaseContext.Procedimentos.Any(x => x.Descricao.ToUpper() == command.Descricao.ToUpper() && x.IndCriminalAdm && x.Codigo != procedimento.Codigo);
                        break;
                }

                if (existeMesmaDescricao)
                {
                    string mensagem = $"Já existe um procedimento com o mesmo nome para o tipo de processo selecionado.";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                if (procedimento.IndAdministrativo.GetValueOrDefault() || procedimento.IndTrabalhistaAdm.GetValueOrDefault() || procedimento.IndTributario.GetValueOrDefault())
                {
                    if (procedimento.CodTipoParticipacao1 != command.CodTipoParticipacao1
                        && DatabaseContext.PartesProcessos.Any(x => x.Processo.Procedimento.Codigo == procedimento.Codigo && x.TipoParticipacaoId == procedimento.CodTipoParticipacao1))
                    {
                        string mensagem = "O 1º Tipo de Participação não pode ser alterado, pois este procedimento já está sento utilizado em um processo.";
                        Logger.LogInformation(mensagem);
                        return CommandResult.Invalid(mensagem);
                    }

                    if (procedimento.CodTipoParticipacao2 != command.CodTipoParticipacao2
                        && DatabaseContext.PartesProcessos.Any(x => x.Processo.Procedimento.Codigo == procedimento.Codigo && x.TipoParticipacaoId == procedimento.CodTipoParticipacao2))
                    {
                        string mensagem = "O 2º Tipo de Participação não pode ser alterado, pois este procedimento já está sento utilizado em um processo.";
                        Logger.LogInformation(mensagem);
                        return CommandResult.Invalid(mensagem);
                    }

                    //TODO: Weriton - Fazer mapeamento Parte_processo_Base e Orgao_Processo e retirar IgnoreQueryFilters

                    if (procedimento.IndOrgao1 != command.IndOrgao1
                        && DatabaseContext.PartesProcessos.IgnoreQueryFilters().Any(x => x.Processo.Procedimento.Codigo == procedimento.Codigo && x.TipoParticipacaoId == procedimento.CodTipoParticipacao1 && x.Parte.TipoParteValor == TipoOrgao.DEMAIS_TIPOS.Valor))
                    {
                        string mensagem = "O indicativo de é orgão do 1º Tipo de Participação não pode ser alterado, pois este procedimento já está sendo utilizado em um processo.";
                        Logger.LogInformation(mensagem);
                        return CommandResult.Invalid(mensagem);
                    }

                    //TODO: Weriton - Fazer mapeamento Parte_processo_Base e Orgao_Processo e retirar IgnoreQueryFilters
                    if (procedimento.IndOrgao2 != command.IndOrgao2
                        && DatabaseContext.PartesProcessos.IgnoreQueryFilters().Any(x => x.Processo.Procedimento.Codigo == procedimento.Codigo && x.TipoParticipacaoId == procedimento.CodTipoParticipacao2 && x.Parte.TipoParteValor == TipoOrgao.DEMAIS_TIPOS.Valor))
                    {
                        string mensagem = "O indicativo de é orgão do 2º Tipo de Participação não pode ser alterado, pois este procedimento já está sendo utilizado em um processo.";
                        Logger.LogInformation(mensagem);
                        return CommandResult.Invalid(mensagem);
                    }
                }

                if (procedimento is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.Codigo));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.Codigo));
                }

                Logger.LogInformation(Infra.Extensions.Logs.AtualizandoEntidade(entityName, command.Codigo));
                procedimento.Atualizar(command.Descricao, command.IndAtivo, tipoParticipacao1, tipoParticipacao2, command.IndOrgao1.GetValueOrDefault(), command.IndOrgao2.GetValueOrDefault(),
                    command.IndProvisionado.GetValueOrDefault(), command.IndPoloPassivoUnico.GetValueOrDefault());

                if (procedimento.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(procedimento.Notifications.ToNotificationsString());
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

        public CommandResult Remover(int codigo)
        {
            string entityName = "Tipo de procedimento";
            string commandName = $"Removendo {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try
            {
                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_TIPO_PROCEDIMENTO))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_TIPO_PROCEDIMENTO, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, codigo));
                Procedimento procedimento = DatabaseContext.Procedimentos.FirstOrDefault(x => x.Codigo == codigo);

                if (procedimento is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, codigo));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, codigo));
                }

                string commandRemocao = $"Validar remoção {entityName}";
                Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandRemocao));

                if (DatabaseContext.Processos.Any(p => p.Procedimento.Codigo == codigo))
                {
                    string mensagem = "Não será possível excluir o Tipo de Procedimento selecionado, pois se encontra relacionado a um processo.";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                Logger.LogInformation(Infra.Extensions.Logs.RemovendoEntidade(entityName, codigo));
                DatabaseContext.Remove(procedimento);

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
