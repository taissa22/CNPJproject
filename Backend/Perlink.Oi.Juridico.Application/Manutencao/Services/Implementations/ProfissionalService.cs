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
using EnumEstado = Perlink.Oi.Juridico.Infra.Enums.EstadoEnum;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services.Implementations
{
    internal class ProfissionalService : IProfissionalService
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IProfissionalService> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }
        public ProfissionalService(IDatabaseContext databaseContext, ILogger<IProfissionalService> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        public CommandResult Atualizar(AtualizarProfissionalCommand command)
        {
            string entityName = "Profissional";
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

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_PROFISSIONAL))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_PROFISSIONAL, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                if (!command.PessoaJuridica && !CPF.IsValidForSisjur(command.Documento))
                {
                    if (DatabaseContext.Profissionais.Any(x => x.CPF == command.Documento && x.Nome == command.Nome && x.Id != command.Id))
                    {
                        string message = "Já existe um Profissional com o mesmo CPF e com o mesmo nome.";
                        Logger.LogInformation(message);
                        return CommandResult.Invalid(message);
                    }
                }

                if (command.PessoaJuridica && !CNPJ.IsValidForSisjur(command.Documento))
                {
                    if (DatabaseContext.Profissionais.Any(x => x.CNPJ == command.Documento && x.Nome == command.Nome && x.Id != command.Id))
                    {
                        string message = "Já existe um Profissional com o mesmo CNPJ e com o mesmo nome.";
                        Logger.LogInformation(message);
                        return CommandResult.Invalid(message);
                    }
                }

                if (CPF.IsValidForSisjur(command.Documento))
                {
                    if (DatabaseContext.Profissionais.Any(x => x.CPF == command.Documento && x.Id != command.Id))
                    {
                        string message = "Já existe um Profissional com o mesmo CPF.";
                        Logger.LogInformation(message);
                        return CommandResult.Invalid(message);
                    }
                }

                if (CNPJ.IsValidForSisjur(command.Documento))
                {
                    if (DatabaseContext.Profissionais.Any(x => x.CNPJ == command.Documento && x.Id != command.Id))
                    {
                        string message = "Já existe um Profissional com o mesmo CNPJ.";
                        Logger.LogInformation(message);
                        return CommandResult.Invalid(message);
                    }
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, command.Id));
                Profissional profissional = DatabaseContext.Profissionais.FirstOrDefault(x => x.Id == command.Id);

                if (profissional is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.Id));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.Id));
                }

                Logger.LogInformation(Infra.Extensions.Logs.AtualizandoEntidade(entityName, command.Id));
                if (command.PessoaJuridica)
                {
                    profissional.Atualizar(
                        command.Nome,
                        CNPJ.FromString(command.Documento),
                        command.Endereco,
                        command.EnderecosAdicionais,
                        command.Bairro,
                        EnumEstado.PorId(command.Estado),
                        command.Cidade,
                        command.CEP,
                        command.Email,
                        Telefone.FromNullableString(command.Telefone),
                        Telefone.FromNullableString(command.Celular),
                        Telefone.FromNullableString(command.Fax),
                        command.TelefonesAdicionais,
                        command.Contador,
                        command.ContadorPex,
                        command.Advogado,
                        command.NumeroOAB,
                        EnumEstado.PorId(command.EstadoOAB));
                }
                else
                {
                    profissional.Atualizar(
                        command.Nome,
                        CPF.FromString(command.Documento),
                        command.Endereco,
                        command.EnderecosAdicionais,
                        command.Bairro,
                        EnumEstado.PorId(command.Estado),
                        command.Cidade,
                        command.CEP,
                        command.Email,
                        Telefone.FromNullableString(command.Telefone),
                        Telefone.FromNullableString(command.Celular),
                        Telefone.FromNullableString(command.Fax),
                        command.TelefonesAdicionais,
                        command.Contador,
                        command.ContadorPex,
                        command.Advogado,
                        command.NumeroOAB,
                        EnumEstado.PorId(command.EstadoOAB));
                }

                if (profissional.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(profissional.Notifications.ToNotificationsString());
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

        public CommandResult Criar(CriarProfissionalCommand command)
        {
            string entityName = "Profissional";
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

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_PROFISSIONAL))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_PROFISSIONAL, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                if (!command.PessoaJuridica && !CPF.IsValidForSisjur(command.Documento))
                {
                    if (DatabaseContext.Profissionais.Any(x => x.CPF == command.Documento && x.Nome == command.Nome))
                    {
                        string message = "Já existe um Profissional com o mesmo CPF e com o mesmo nome.";
                        Logger.LogInformation(message);
                        return CommandResult.Invalid(message);
                    }
                }

                if (command.PessoaJuridica && !CNPJ.IsValidForSisjur(command.Documento))
                {
                    if (DatabaseContext.Profissionais.Any(x => x.CNPJ == command.Documento && x.Nome == command.Nome))
                    {
                        string message = "Já existe um Profissional com o mesmo CNPJ e com o mesmo nome.";
                        Logger.LogInformation(message);
                        return CommandResult.Invalid(message);
                    }
                }

                if (CPF.IsValidForSisjur(command.Documento))
                {
                    if (DatabaseContext.Profissionais.Any(x => x.CPF == command.Documento))
                    {
                        string message = "Já existe um Profissional com o mesmo CPF.";
                        Logger.LogInformation(message);
                        return CommandResult.Invalid(message);  
                    }
                }

                if (CNPJ.IsValidForSisjur(command.Documento))
                {
                    if (DatabaseContext.Profissionais.Any(x => x.CNPJ == command.Documento))
                    {
                        string message = "Já existe um Profissional com o mesmo CNPJ.";
                        Logger.LogInformation(message);
                        return CommandResult.Invalid(message);
                    }
                }

                Logger.LogInformation(Infra.Extensions.Logs.CriandoEntidade(entityName));
                Profissional profissional;
                if (command.PessoaJuridica)
                {
                    profissional = Profissional.CriarPessoaJuridica(
                       command.Nome,
                        CNPJ.FromString(command.Documento),
                        command.Endereco,
                        command.EnderecosAdicionais,
                        command.Bairro,
                        EnumEstado.PorId(command.Estado),
                        command.Cidade,
                        command.CEP,
                        command.Email,
                        Telefone.FromNullableString(command.Telefone),
                        Telefone.FromNullableString(command.Celular),
                        Telefone.FromNullableString(command.Fax),
                        command.TelefonesAdicionais,
                        command.Contador,
                        command.ContadorPex,
                        command.Advogado,
                        command.NumeroOAB,
                        EnumEstado.PorId(command.EstadoOAB));
                }
                else
                {
                    profissional = Profissional.CriarPessoaFisica(
                        command.Nome,
                        CPF.FromString(command.Documento),
                        command.Endereco,
                        command.EnderecosAdicionais,
                        command.Bairro,
                        EnumEstado.PorId(command.Estado),
                        command.Cidade,
                        command.CEP,
                        command.Email,
                        Telefone.FromNullableString(command.Telefone),
                        Telefone.FromNullableString(command.Celular),
                        Telefone.FromNullableString(command.Fax),
                        command.TelefonesAdicionais,
                        command.Contador,
                        command.ContadorPex,
                        command.Advogado,
                        command.NumeroOAB,
                        EnumEstado.PorId(command.EstadoOAB));
                }

                if (profissional.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(profissional.Notifications.ToNotificationsString());
                }

                DatabaseContext.Add(profissional);
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
            string entityName = "Profissional";
            string commandName = $"Removendo {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try
            {
                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_PROFISSIONAL))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_PROFISSIONAL, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, id));
                Profissional profissional = DatabaseContext.Profissionais.FirstOrDefault(x => x.Id == id);

                if (profissional is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, id));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, id));
                }

                string commandRemocao = $"Validar remoção {entityName}";
                Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandRemocao));
                ValidarExclusao(profissional);
                
                Logger.LogInformation(Infra.Extensions.Logs.RemovendoEntidade(entityName, id));
                DatabaseContext.Remove(profissional);

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

        private void ValidarExclusao(Profissional profissional) {
            string message = "O Profissional selecionado se encontra relacionado a um processo.";
            if (DatabaseContext.Processos.Any(x => x.EscritorioId == profissional.Id)) {
                Logger.LogInformation($"Profissional '{ profissional.Id }' vinculado a 'Processo como escritório principal'");
                throw new InvalidOperationException(message);
            }

            if (DatabaseContext.Processos.Any(x => x.EscritorioAcompanhanteId == profissional.Id)) {
                Logger.LogInformation($"Profissional '{ profissional.Id }' vinculado a 'Processo como escritório acompanhante'");
                throw new InvalidOperationException(message);
            }

            if (DatabaseContext.Processos.Any(x => x.ContadorId == profissional.Id)) {
                Logger.LogInformation($"Profissional '{ profissional.Id }' vinculado a 'Processo como contador'");
                throw new InvalidOperationException(message);
            }

            if (DatabaseContext.EscritoriosDosUsuarios.Any(x => x.EscritorioId == profissional.Id)) {
                Logger.LogInformation($"Profissional '{ profissional.Id }' vinculado a 'Processo como usuário internet'");
                throw new InvalidOperationException(message);
            }

            if (DatabaseContext.PagamentosProcesso.Any(x => x.Profissional.Id == profissional.Id)) {
                Logger.LogInformation($"Profissional '{ profissional.Id }' vinculado a 'Pagamentos do processo'");
                throw new InvalidOperationException(message);
            }

            if (DatabaseContext.DespesasDosProfissionais.Any(x => x.ProfissionalId == profissional.Id)) {
                Logger.LogInformation($"Profissional '{ profissional.Id }' vinculado a 'Despesas dos profissionais'");
                throw new InvalidOperationException(message);
            }

            if (DatabaseContext.Fornecedores.Any(x => x.EscritorioId == profissional.Id)) {
                Logger.LogInformation($"Profissional '{ profissional.Id }' vinculado a 'Fornecedores'");
                throw new InvalidOperationException(message);
            }

            if (DatabaseContext.Fornecedores.Any(x => x.ProfissionalId == profissional.Id)) {
                Logger.LogInformation($"Profissional '{ profissional.Id }' vinculado a 'Fornecedores SAP'");
                throw new InvalidOperationException(message);
            }

            if (DatabaseContext.AdvogadosDosAutores.Any(x => x.ProfissionalId == profissional.Id)) {
                Logger.LogInformation($"Profissional '{ profissional.Id }' vinculado a 'Advogados do Autor'");
                throw new InvalidOperationException(message);
            }

            if (DatabaseContext.AudienciasDosProcessos.Any(x => x.EscritorioId == profissional.Id)) {
                Logger.LogInformation($"Profissional '{ profissional.Id }' vinculado a 'Audiências do processo'");
                throw new InvalidOperationException(message);
            }

            if (DatabaseContext.Protocolos.Any(x => x.ProfissionalId == profissional.Id)) {
                Logger.LogInformation($"Profissional '{ profissional.Id }' vinculado a 'Protocolos'");
                throw new InvalidOperationException(message);
            }

            if (DatabaseContext.EscritoriosEstados.Any(x => x.Profissional.Id == profissional.Id)) {
                Logger.LogInformation($"Profissional '{ profissional.Id }' vinculado a 'Escritórios do Estado'");
                throw new InvalidOperationException(message);
            }
        }

    }
}
