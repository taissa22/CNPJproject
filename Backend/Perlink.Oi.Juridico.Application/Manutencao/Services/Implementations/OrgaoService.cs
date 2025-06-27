using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Application.Manutencao.Commands.Dto;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Constants;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services.Implementations
{
    internal class OrgaoService : IOrgaoService
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IOrgaoService> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public OrgaoService(IDatabaseContext databaseContext, ILogger<IOrgaoService> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        public CommandResult Criar(CriarOrgaoCommand command)
        {
            string entityName = "Orgao";
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

                var tipoOrgao = TipoOrgao.PorValor(command.TipoOrgao);

                if (!UsuarioAtual.TemPermissaoPara(PermissaoPorTipoOrgao(tipoOrgao)))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(PermissaoPorTipoOrgao(tipoOrgao), UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                HugeDataString nome = HugeDataString.FromString(command.Nome);
                if (DatabaseContext.Orgaos.Any(x => x.Nome == nome))
                {
                    string message = "Já existe outro órgão com este nome.";
                    Logger.LogInformation(message);
                    return CommandResult.Invalid(message);
                }

                Logger.LogInformation(Infra.Extensions.Logs.CriandoEntidade(entityName));
                var competencias = command.Competencias.Select(competencia => Competencia.Criar(DataString.FromString(competencia)));
                Orgao orgao = Orgao.Criar(nome, Telefone.FromNullableString(command.Telefone), tipoOrgao, competencias);

                if (orgao.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(orgao.Notifications.ToNotificationsString());
                }

                DatabaseContext.Add(orgao);
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

        public CommandResult Atualizar(AtualizarOrgaoCommand command)
        {
            string entityName = "Orgao";
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

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, command.Id));
                Orgao orgao = DatabaseContext.Orgaos.FirstOrDefault(x => x.Id == command.Id);

                if (orgao is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.Id));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.Id));
                }

                if (!UsuarioAtual.TemPermissaoPara(PermissaoPorTipoOrgao(orgao.TipoOrgao)))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(PermissaoPorTipoOrgao(orgao.TipoOrgao), UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                if (DatabaseContext.Orgaos.Any(x => x.Nome == command.Nome && x.Id != command.Id))
                {
                    string message = "Já existe outro Órgão com este nome.";
                    Logger.LogInformation(message);
                    return CommandResult.Invalid(message);
                }

                Logger.LogInformation(Infra.Extensions.Logs.AtualizandoEntidade(entityName, command.Id));
                orgao.Atualizar(
                    HugeDataString.FromString(command.Nome),
                    Telefone.FromNullableString(command.Telefone),
                    CompetenciasDoDTO(command.Competencias, orgao));

                if (orgao.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(orgao.Notifications.ToNotificationsString());
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
            string entityName = "Orgao";
            string commandName = $"Removendo {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try
            {
                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, id));
                Orgao orgao = DatabaseContext.Orgaos.FirstOrDefault(x => x.Id == id);

                Logger.LogInformation(Infra.Extensions.Logs.ValidanoPermissao(UsuarioAtual.Login));
                if (!UsuarioAtual.TemPermissaoPara(PermissaoPorTipoOrgao(orgao.TipoOrgao)))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(PermissaoPorTipoOrgao(orgao.TipoOrgao), UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                if (orgao is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, id));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, id));
                }

                if (DatabaseContext.ProcessosConexos.Any(x => x.OrgaoId == id))
                {
                    orgao.AddNotification("Relacionamento", "Processo Conexo");
                }

                if (orgao.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(orgao.Notifications.ToNotificationsString());
                }

                Logger.LogInformation(Infra.Extensions.Logs.RemovendoEntidade(entityName, id));
                DatabaseContext.Remove(orgao);

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

        private string PermissaoPorTipoOrgao(TipoOrgao tipoOrgao)
        {
            if (tipoOrgao == TipoOrgao.CRIMINAL_ADMINISTRATIVO)
            {
                return Permissoes.ACESSAR_ORGAOS_CRIMINAL_ADMINISTRATIVO;
            }

            if (tipoOrgao == TipoOrgao.CIVEL_ADMINISTRATIVO)
            {
                return Permissoes.ACESSAR_ORGAOS_CIVEL_ADMINISTRATIVO;
            }

            if (tipoOrgao == TipoOrgao.DEMAIS_TIPOS)
            {
                return Permissoes.ACESSAR_ORGAOS_DEMAIS_TIPOS;
            }

            return Permissoes.PERMISSAO_NEGADA;
        }

        private static IEnumerable<Competencia> CompetenciasDoDTO(IEnumerable<CompetenciaDTO> competencias, Orgao orgao)
        {
            return competencias.Select(command =>
            {
                if (command.Sequencial is null)
                {
                    return Competencia.Criar(DataString.FromString(command.Nome));
                }
                var competencia = orgao.Competencias.Single(c => c.Sequencial == command.Sequencial);
                competencia.Atualizar(DataString.FromString(command.Nome));
                return competencia;
            }).ToArray();
        }
    }
}
