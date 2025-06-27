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
    internal class EstabelecimentoService : IEstabelecimentoService
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IEstabelecimentoService> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public EstabelecimentoService(IDatabaseContext databaseContext, ILogger<IEstabelecimentoService> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        public CommandResult Criar(CriarEstabelecimentoCommand command)
        {
            string entityName = "Estabelecimento";
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

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ESTABELECIMENTO))
                {
                    Logger.LogInformation(string.Format("Permissao Negada", UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }
                
                Logger.LogInformation(Infra.Extensions.Logs.Obtendo(entityName));                

                if (DatabaseContext.Estabelecimentos
                                   .Any(x => x.CNPJ.Equals(command.Cnpj)
                                        || x.Nome.Equals(command.Nome.ToUpper())))
                {
                    string message = $"Já existe um estabelecimento cadastrado com o mesmo nome ou cnpj.";
                    Logger.LogInformation(message);
                    return CommandResult.Invalid(message);
                }
                else if (string.IsNullOrEmpty(command.Cnpj) || string.IsNullOrEmpty(command.Nome))
                {
                    Logger.LogInformation($"Nome ou Cnpj não informados.");
                    return CommandResult.Invalid($"Nome ou Cnpj não informados.");
                }


                if (string.IsNullOrEmpty(command.Estado))
                {
                    string message = $"Estado inválido '{command.Estado}'.";
                    Logger.LogInformation(message);
                    command.Estado = null;
                };

                Logger.LogInformation(Infra.Extensions.Logs.CriandoEntidade(entityName));
                Estabelecimento estabelecimento = Estabelecimento.Criar(DataString.FromString(command.Nome), CNPJ.FromString(command.Cnpj), DataString.FromString(command.Endereco), DataString.FromString(command.Bairro), DataString.FromString(command.Cidade), DataString.FromString(command.Cep),
                    EstadoEnum.PorId(command.Estado), Telefone.FromNullableString(command.Telefone), Telefone.FromNullableString(command.Celular));

                
                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade(entityName));
                DatabaseContext.Add(estabelecimento);
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

        public CommandResult Atualizar(AtualizarEstabelecimentoCommand command)
        {
            string entityName = "Estabelecimento";
            string commandName = $"Atualizando {entityName}";

            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ESTABELECIMENTO))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ESTABELECIMENTO, UsuarioAtual.Login));
                return CommandResult.Forbidden();
            }

            try
            {
                command.Validate();
                if (command.Invalid)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.ComandoInvalido(commandName));
                    return CommandResult.Invalid(command.Notifications.ToNotificationsString());
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, command.Id));
                Estabelecimento estabelecimento = DatabaseContext.Estabelecimentos.FirstOrDefault(x => x.Id == command.Id);

                if (estabelecimento is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.Id));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.Id));
                }

                if (DatabaseContext.Estabelecimentos.Any(x => x.Nome == command.Nome.ToUpper() && x.Id != command.Id))
                {
                    string message = "Já existe outro estabelecimento com este nome.";
                    Logger.LogInformation(message);
                    return CommandResult.Invalid(message);
                }

                Logger.LogInformation(Infra.Extensions.Logs.AtualizandoEntidade(entityName, command.Id));
                estabelecimento.Atualizar(DataString.FromString(command.Nome), CNPJ.FromString(command.Cnpj), DataString.FromString(command.Endereco), DataString.FromString(command.Bairro),
                    DataString.FromString(command.Cidade), DataString.FromString(command.Cep), EstadoEnum.PorId(command.Estado), Telefone.FromNullableString(command.Telefone), Telefone.FromNullableString(command.Celular));

                if (estabelecimento.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(estabelecimento.Notifications.ToNotificationsString());
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
            string entityName = "Estabelecimento";
            string commandName = $"Removendo {entityName}";

            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ESTABELECIMENTO))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ESTABELECIMENTO, UsuarioAtual.Login));
                return CommandResult.Forbidden();
            }

            try
            {
                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, id));
                Estabelecimento estabelecimento = DatabaseContext.Estabelecimentos.FirstOrDefault(x => x.Id == id);

                if (estabelecimento is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, id));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, id));
                }             

                string commandRemocao = $"Validar remoção {entityName}";
                Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandRemocao));
                
                if (DatabaseContext.Processos.Any(x => x.Estabelecimento.Id == id))
                {
                    string mensagem  = "O Estabelecimento selecionado se encontra relacionado a um processo.";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                if (estabelecimento.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(estabelecimento.Notifications.ToNotificationsString());
                }

                Logger.LogInformation(Infra.Extensions.Logs.RemovendoEntidade(entityName, id));
                DatabaseContext.Remove(estabelecimento);

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