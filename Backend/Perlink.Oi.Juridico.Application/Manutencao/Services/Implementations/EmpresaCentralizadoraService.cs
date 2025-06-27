using Microsoft.EntityFrameworkCore;
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
    internal class EmpresaCentralizadoraService : IEmpresaCentralizadoraService
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IEmpresaDoGrupoService> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public EmpresaCentralizadoraService(IDatabaseContext databaseContext, ILogger<IEmpresaDoGrupoService> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        public CommandResult Criar(CriarEmpresaCentralizadoraCommand command)
        {
            string entityName = "Empresa Centralizadora";
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

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_EMPRESA_CENTRALIZADORA))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.ComandoInvalido(commandName));
                    return CommandResult<bool>.Forbidden();
                }

                if (DatabaseContext.EmpresasCentralizadoras.Any(x => x.Nome == command.Nome))
                {
                    string message = "Já existe outra Empresa Centralizadora com este nome.";
                    Logger.LogInformation(message);
                    return CommandResult.Invalid(message);
                }

                Logger.LogInformation(Infra.Extensions.Logs.CriandoEntidade(entityName));
                EmpresaCentralizadora empresaCentralizadora =
                    EmpresaCentralizadora.Criar(DataString.FromString(command.Nome), ConveniosDoDTO(command.Convenios));

                if (empresaCentralizadora.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(empresaCentralizadora.Notifications.ToNotificationsString());
                }

                DatabaseContext.Add(empresaCentralizadora);
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

        public CommandResult Atualizar(AtualizarEmpresaCentralizadoraCommand command)
        {
            string entityName = "Empresa Centralizadora";
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

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_EMPRESA_CENTRALIZADORA))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.ComandoInvalido(commandName));
                    return CommandResult<bool>.Forbidden();
                }

                if (DatabaseContext.EmpresasCentralizadoras.Any(x => x.Nome == command.Nome && x.Codigo != command.Codigo))
                {
                    string message = "Já existe outra Empresa Centralizadora com este nome.";
                    Logger.LogInformation(message);
                    return CommandResult.Invalid(message);
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, command.Codigo));
                EmpresaCentralizadora empresaCentralizadora = DatabaseContext.EmpresasCentralizadoras.FirstOrDefault(x => x.Codigo == command.Codigo);

                if (empresaCentralizadora is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.Codigo));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.Codigo));
                }

                Logger.LogInformation(Infra.Extensions.Logs.AtualizandoEntidade(entityName, command.Codigo));
                empresaCentralizadora.Atualizar(DataString.FromString(command.Nome), ConveniosDoDTO(command.Convenios));

                if (empresaCentralizadora.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(empresaCentralizadora.Notifications.ToNotificationsString());
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

        private IEnumerable<Convenio> ConveniosDoDTO(IEnumerable<ConvenioDTO> convenios)
        {
            return convenios.Select(convenio => Convenio.Criar(
                        EstadoEnum.PorId(convenio.EstadoId), convenio.Codigo, CNPJ.FromString(convenio.CNPJ),
                        convenio.BancoDebito, convenio.AgenciaDebito, DataString.FromString(convenio.DigitoAgenciaDebito),
                        DataString.FromString(convenio.ContaDebito), convenio.MCI, convenio.AgenciaDepositaria,
                        DataString.FromString(convenio.DigitoAgenciaDepositaria))).ToArray();
        }

        public CommandResult Remover(int codigo)
        {
            string entityName = "Empresa Centralizadora";
            string commandName = $"Removendo {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try
            {
                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_EMPRESA_CENTRALIZADORA))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_EMPRESA_CENTRALIZADORA, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, codigo));
                EmpresaCentralizadora empresaCentralizadora = DatabaseContext.EmpresasCentralizadoras.FirstOrDefault(x => x.Codigo == codigo);

                if (empresaCentralizadora is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, codigo));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, codigo));
                }

                if (DatabaseContext.EmpresasDoGrupo.Any(x => x.EmpresaCentralizadora == empresaCentralizadora))
                {
                    empresaCentralizadora.AddNotification("Relacionamento", "Empresa do Grupo");
                }

                // TODO: EF.Property
                // TODO: IgnoreQueryFilters()
                if (DatabaseContext.EmpresasDoGrupo.IgnoreQueryFilters()
                    .Where(x => EF.Property<string>(x, "TipoParteValor") == "C")
                    .Any(x => x.EmpresaCentralizadora == empresaCentralizadora))
                {
                    empresaCentralizadora.AddNotification("Relacionamento", "Empresa do Grupo");
                }

                if (DatabaseContext.FechamentosTrabalhistas.Any(x => x.CodigoEmpresaCentralizadora == empresaCentralizadora.Codigo))
                {
                    empresaCentralizadora.AddNotification("Relacionamento", "Fechamentos Trabalhistas");
                }

                if (DatabaseContext.FechamentosCiveis.Any(x => x.CodigoEmpresaCentralizadora == empresaCentralizadora.Codigo))
                {
                    empresaCentralizadora.AddNotification("Relacionamento", "Fechamentos Cíveis");
                }

                if (DatabaseContext.FechamentosProcessosJuizados.Any(x => x.CodigoEmpresaCentralizadora == empresaCentralizadora.Codigo))
                {
                    empresaCentralizadora.AddNotification("Relacionamento", "Fechamentos Processos Juizados");
                }

                // TODO: Notes on US70587;

                if (empresaCentralizadora.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(empresaCentralizadora.Notifications.ToNotificationsString());
                }

                Logger.LogInformation(Infra.Extensions.Logs.RemovendoEntidade(entityName, codigo));
                DatabaseContext.Remove(empresaCentralizadora);

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