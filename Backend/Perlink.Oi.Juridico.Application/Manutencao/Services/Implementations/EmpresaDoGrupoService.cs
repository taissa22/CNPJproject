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
using Perlink.Oi.Juridico.Infra.ValueObjects;
using System;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services.Implementations
{
    internal class EmpresaDoGrupoService : IEmpresaDoGrupoService {
        private IDatabaseContext Context { get; }
        private ILogger<IEmpresaDoGrupoService> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public EmpresaDoGrupoService(IDatabaseContext databaseContext, ILogger<IEmpresaDoGrupoService> logger, IUsuarioAtualProvider usuarioAtual) {
            Context = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        /// <summary>
        /// Cria uma nova empresa
        /// </summary>
        /// <param name="command">Dto da empresa que irá ser criada</param>
        public CommandResult Criar(CriarEmpresaDoGrupoCommand command) {
            string entityName = "Empresa do Grupo";
            string commandName = $"Criar {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_EMPRESA_DO_GRUPO)) {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_EMPRESA_DO_GRUPO, UsuarioAtual.Login));
                return CommandResult.Forbidden();
            }

            try {
                command.Validate();

                if (command.Invalid) {
                    Logger.LogInformation(Infra.Extensions.Logs.ComandoInvalido(commandName));
                    return CommandResult.Invalid(command.Notifications.ToNotificationsString());
                }

                if (Context.EmpresasDoGrupo.Any(x => x.Nome.ToUpper() == command.Nome.ToUpper())) {
                    string message = "Já existe uma empresa do grupo cadastrada com o mesmo nome";
                    Logger.LogInformation(message);
                    return CommandResult.Invalid(message);
                }

                var regional = Context.Regionais.FirstOrDefault(x => x.Id == command.Regional);
                if (regional is null) {
                    string message = $"Regional não encontrado '{command.Regional}'.";
                    Logger.LogInformation(message);
                    return CommandResult.Invalid(message);
                }

                var empresaCentralizadora = Context.EmpresasCentralizadoras.FirstOrDefault(x => x.Codigo == command.EmpresaCentralizadora);
                if (empresaCentralizadora is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.EmpresaCentralizadora));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.EmpresaCentralizadora));
                }

                var empresaSap = Context.EmpresasSap.FirstOrDefault(x => x.Codigo == command.EmpresaSap);
                if (empresaSap is null) {
                    string message = $"Empresa SAP não encontrada '{command.EmpresaSap}'.";
                    Logger.LogInformation(message);
                    return CommandResult.Invalid(message);
                }

                Logger.LogInformation(Infra.Extensions.Logs.CriandoEntidade(entityName));                

                EmpresaDoGrupo empresaDoGrupo = EmpresaDoGrupo.Criar(DataString.FromString(command.Nome), DataString.FromString(command.Cnpj), command.Regional, DataString.FromString(command.CentroSap), command.EmpresaCentralizadora, command.EmpresaSap,
                    DataString.FromString(command.Endereco), DataString.FromString(command.Bairro), EstadoEnum.PorId(command.Estado), DataString.FromString(command.Cidade), DataString.FromString(command.Cep),
                    DataString.FromString(command.TelefoneDDD), DataString.FromString(command.Telefone), DataString.FromString(command.FaxDDD), DataString.FromString(command.Fax), command.Fornecedor, command.CentroCusto, command.GeraArquivoBB, command.InterfaceBB, command.EmpRecuperanda, command.EmpTrio);

                if (empresaDoGrupo.HasNotifications) {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(empresaDoGrupo.Notifications.ToNotificationsString());
                }

                Context.Add(empresaDoGrupo);
                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade(entityName));

                Context.SaveChanges();
                Logger.LogInformation(Infra.Extensions.Logs.OperacaoFinalizada(commandName));

                return CommandResult.Valid();
            } catch (Exception ex) {
                Logger.LogError(ex, Infra.Extensions.Logs.OperacaoComErro(commandName));
                return CommandResult.Invalid(ex.Message);
            }
        }

        /// <summary>
        /// Atualiza uma empresa do grupo
        /// </summary>
        /// <param name="command">Dto com os dados a serem atualizado</param>
        public CommandResult Atualizar(AtualizarEmpresaDoGrupoCommand command) {
            string entityName = "Empresa do Grupo";
            string commandName = $"Atualizar {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));            

            try {

                command.Validate();
                if (command.Invalid)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.ComandoInvalido(commandName));
                    return CommandResult.Invalid(command.Notifications.ToNotificationsString());
                }

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_EMPRESA_DO_GRUPO))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_EMPRESA_DO_GRUPO, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, command.Id));
                EmpresaDoGrupo empresa = Context.EmpresasDoGrupo
                                                .FirstOrDefault(x => x.Id == command.Id);                       

                if (empresa is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.Id));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.Id));
                }

                #region Trata a regra de validação do nome

                string commandValidacaoNome = $"Validar duplicidade de nome para {entityName}";
                Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandValidacaoNome));

                command.Nome = command.Nome.Padronizar();

                if (Context.EmpresasDoGrupo.Any(x => x.Nome == command.Nome && x.Id != command.Id)) {
                    string message = "Já existe uma Empresa com o mesmo Nome.";
                    Logger.LogInformation(message);
                    return CommandResult.Invalid(message);
                }

                var regional = Context.Regionais.FirstOrDefault(x => x.Id == command.Regional);
                if (regional is null) {
                    string message = $"Regional não encontrado '{command.Regional}'.";
                    Logger.LogInformation(message);
                    return CommandResult.Invalid(message);
                }

                var empresaCentralizadora = Context.EmpresasCentralizadoras.FirstOrDefault(x => x.Codigo == command.EmpresaCentralizadora);
                if (empresaCentralizadora is null)
                {
                    string message = $"Empresa Centralizadora não encontrada '{command.EmpresaCentralizadora}'.";
                    Logger.LogInformation(message);
                    return CommandResult.Invalid(message);
                }

                var empresaSap = Context.EmpresasSap.FirstOrDefault(x => x.Codigo == command.EmpresaSap);
                if (empresaSap is null) {
                    string message = $"Empresa SAP não encontrada '{command.EmpresaSap}'.";
                    Logger.LogInformation(message);
                    return CommandResult.Invalid(message);
                }

                #endregion Trata a regra de validação do nome

                if ((empresa.Regional != null && empresa.Regional.Id != command.Regional) || (command.Regional > 0  && empresa.Regional is null))
                    this.AtualizaRegionalProcessosEmpresa(empresa.Id, command.Regional);

                empresa.Atualizar(DataString.FromString(command.Nome), DataString.FromString(command.Cnpj), command.Regional, DataString.FromNullableString(command.CentroSap), command.EmpresaCentralizadora, command.EmpresaSap, DataString.FromNullableString(command.Endereco),
                    DataString.FromNullableString(command.Bairro), command.Estado, DataString.FromNullableString(command.Cidade), DataString.FromNullableString(command.Cep), DataString.FromString(command.TelefoneDDD), DataString.FromNullableString(command.Telefone),
                    DataString.FromString(command.FaxDDD), DataString.FromNullableString(command.Fax), command.Fornecedor, command.CentroCusto, command.GeraArquivoBB, command.InterfaceBB, command.EmpRecuperanda, command.EmpTrio);

                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade(entityName));
                Context.SaveChanges();

                Logger.LogInformation(Infra.Extensions.Logs.OperacaoFinalizada(commandName));
                return CommandResult.Valid();
            } catch (Exception ex) {
                Logger.LogError(ex, Infra.Extensions.Logs.OperacaoComErro(commandName));
                return CommandResult.Invalid(ex.Message);
            }
        }

        /// <summary>
        /// Deleta uma empresa do grupo
        /// </summary>
        /// <param name="id">Identificador da empresa</param>
        public CommandResult Remover(int id) {
            string entityName = "Empresa do Grupo";
            string commandName = $"Remover {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try { 

                string todasRestricoes = string.Empty;

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(commandName, id));
                EmpresaDoGrupo empresa = Context.EmpresasDoGrupo
                                                 .FirstOrDefault(x => x.Id == id);

                if (empresa is null) {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, id));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, id));
                }

                if (Context.Processos.Any(x => x.EmpresaDoGrupo.Id == id))
                {
                    todasRestricoes += "Empresa do grupo do processo; ";      
                    string message = "Não será possível excluir a Empresa do Grupo selecionada, pois encontra-se relacionada com processos.";
                    Logger.LogInformation(message);
                }

                string commandRemocao = $"Validar remoção {entityName}";
                Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandRemocao));

                if (Context.ValoresProcesso.Any(x => x.COD_PARTE_EFETUOU.Equals(id) || x.COD_PARTE_LEVANTOU.Equals(id))) {
                    todasRestricoes += "Valores do processo; ";
                    string message = "Não será possível excluir a Empresa do Grupo selecionada, pois encontra-se relacionada com Valores Processo";
                    Logger.LogInformation(message);            
                }

                if (Context.PagamentosProcesso.Any(x => x.Parte.Id == id))
                {
                    todasRestricoes += "Pagamentos do processo; ";
                    string message = "Não será possível excluir a Empresa do Grupo selecionada, pois se encontra relacionada com Pagamento Processo";
                    Logger.LogInformation(message);

                }

                if (Context.PagamentosObjetosProcesso.Any(x => x.Parte.Id == id))
                {
                    todasRestricoes += "Pagamento objeto do processo; ";
                    string message = "Não será possível excluir a Empresa do Grupo selecionada, pois encontra-se relacionada com Pagamentos Objetos Processo";
                    Logger.LogInformation(message);

                }

                if (Context.ProcessosConexos.Any(x => x.EmpresaDoGrupo.Id == id))
                {
                    todasRestricoes += "Processo conexo";
                    string message = "Empresa vinculada a processo conexo";
                    Logger.LogInformation(message);

                }

                if (!string.IsNullOrEmpty(todasRestricoes)) {
                    string message = $"Não será possível excluir a Empresa do Grupo selecionada, pois se encontra relacionada com {todasRestricoes}";
                    return CommandResult.Invalid(message);
                }

                Logger.LogInformation(Infra.Extensions.Logs.RemovendoEntidade(entityName, id));
                Context.Remove(empresa);

                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade(entityName));
                Context.SaveChangesAsync();

                Logger.LogInformation(Infra.Extensions.Logs.OperacaoFinalizada(commandName));
                return CommandResult.Valid();

            } catch (Exception ex) {
                Logger.LogError(ex, Infra.Extensions.Logs.OperacaoComErro(commandName));
                return CommandResult.Invalid(ex.Message);
            }
        }

        /// <summary>
        /// Caso a regional da empresa seja alterada, modifica a regional de todos os processos vinculados a ela.
        /// </summary>
        /// <param name="empresa">Empresa do grupo atualizada.</param>
        private void AtualizaRegionalProcessosEmpresa(int empresaId, int regionalId) {
            Regional regional = Context.Regionais
                                       .FirstOrDefault(x => x.Id == regionalId);

            string message = "Atualizando a regional dos processos vinculados à empresa.";
            Logger.LogInformation(message);
            Context.Processos
              .Where(x => x.EmpresaDoGrupo.Id == empresaId)
              .ToList()
              .ForEach(x => x.AtualizarRegional(regional));
        }
    }
}