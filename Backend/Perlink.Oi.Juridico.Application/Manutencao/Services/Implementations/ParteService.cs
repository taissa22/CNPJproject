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
    internal class ParteService : IParteService
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IParteService> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public ParteService(IDatabaseContext databaseContext, ILogger<IParteService> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        public CommandResult Atualizar(AtualizarParteCommand command)
        {
            string entityName = "Parte";
            string commandName = $"Atualizar {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try
            {
                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, command.Id));
                Parte parte = DatabaseContext.Partes.FirstOrDefault(x => x.Id == command.Id);

                //CA17
                if (command.PessoaJuridica && parte.TipoParte == TipoParte.PESSOA_FISICA && DatabaseContext.Processos.Any(x => x.EmpresaDoGrupo.Id == command.Id))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.ValidandoRegras());
                    return CommandResult.Invalid("A alteração do tipo da parte não poderá ser realizada, pois ela e encontra relacionada a um processo.");
                }
                else if (!command.PessoaJuridica && parte.TipoParte == TipoParte.PESSOA_JURIDICA && DatabaseContext.Processos.Any(x => x.EmpresaDoGrupo.Id == command.Id))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.ValidandoRegras());
                    return CommandResult.Invalid("A alteração do tipo da parte não poderá ser realizada, pois ela e encontra relacionada a um processo.");
                }

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
                    if (DatabaseContext.Partes.Any(x => x.CPF == command.Documento && x.Nome == command.Nome && x.Id != command.Id))
                    {
                        string message = "Já existe uma parte com o mesmo CPF e com o mesmo nome.";
                        Logger.LogInformation(message);
                        return CommandResult.Invalid(message);
                    }
                }

                if (command.PessoaJuridica && !CNPJ.IsValidForSisjur(command.Documento))
                {
                    if (DatabaseContext.Partes.Any(x => x.CNPJ == command.Documento && x.Nome == command.Nome && x.Id != command.Id))
                    {
                        string message = "Já existe uma parte com o mesmo CNPJ e com o mesmo nome.";
                        Logger.LogInformation(message);
                        return CommandResult.Invalid(message);
                    }
                }

                if (CPF.IsValidForSisjur(command.Documento))
                {
                    if (DatabaseContext.Partes.Any(x => x.CPF == command.Documento && x.Id != command.Id))
                    {
                        string message = "Já existe uma parte com o mesmo CPF.";
                        Logger.LogInformation(message);
                        return CommandResult.Invalid(message);
                    }
                }

                if (CNPJ.IsValidForSisjur(command.Documento))
                {
                    if (DatabaseContext.Partes.Any(x => x.CNPJ == command.Documento && x.Id != command.Id))
                    {
                        string message = "Já existe uma parte com o mesmo CNPJ.";
                        Logger.LogInformation(message);
                        return CommandResult.Invalid(message);
                    }
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, command.Id));
                 parte = DatabaseContext.Partes.FirstOrDefault(x => x.Id == command.Id);

                if (parte is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.Id));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.Id));
                }

                Logger.LogInformation(Infra.Extensions.Logs.AtualizandoEntidade(entityName, command.Id));
                if (command.PessoaJuridica)
                {               

                    parte.AtualizarPJ(
                        command.Nome,
                        CNPJ.FromString(command.Documento),
                        EstadoEnum.PorId(command.EstadoId),
                        command.Endereco,
                        command.CEP,
                       command.Cidade,
                        command.Bairro,
                        command.EnderecosAdicionais,
                        Telefone.FromNullableString(command.Telefone),
                        Telefone.FromNullableString(command.Celular),
                        command.TelefonesAdicionais,
                        command.DataCartaFianca, command.ValorCartaFianca);
                }
                else
                {    
                    parte.AtualizarPF(
                        command.Nome,
                        CPF.FromString(command.Documento),
                        EstadoEnum.PorId(command.EstadoId),
                        command.Endereco,
                        command.CEP,
                        command.Cidade,
                        command.Bairro,
                        command.EnderecosAdicionais,
                        Telefone.FromNullableString(command.Telefone),
                        Telefone.FromNullableString(command.Celular),
                        command.TelefonesAdicionais,
                        command.CarteiraTrabalho,
                        command.DataCartaFianca,
                        command.ValorCartaFianca);
                }

                if (parte.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(parte.Notifications.ToNotificationsString());
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


        public CommandResult Criar(CriarParteCommand command)
        {
            string entityName = "Parte";
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

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_PARTE))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_PARTE, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                if (UsuarioAtual.TemPermissaoPara(Permissoes.ALTERAR_CARTA_FIANCA) && command.DataCartaFianca is null || command.ValorCartaFianca <= 0)
                {
                    return CommandResult.Invalid("É necessário informar o valor e data da carta fiança.");
                }

                Logger.LogInformation(Infra.Extensions.Logs.CriandoEntidade(entityName));
                Parte parte;
                if (command.PessoaJuridica && CNPJ.IsValidForSisjur(command.Documento))
                {
                    //CA14
                    Parte partePJ = DatabaseContext.Partes.FirstOrDefault(x => x.CNPJ == command.Documento);              

                    if (partePJ != null )
                    {
                        if (DatabaseContext.Partes.Any(x => x.CNPJ == command.Documento && x.Nome == command.Nome.ToUpper()))
                        {
                            string msg = "Já existe uma parte cadastrada com este CNPJ e o mesmo nome.";

                            Logger.LogInformation(Infra.Extensions.Logs.ComandoInvalido(msg));
                            return CommandResult.Invalid(msg);
                        }
                        else if (!command.Documento.Equals("11111111111111") && !command.Documento.Equals("99999999999999"))
                        {
                            string msg = "Já existe uma parte cadastrada com este CNPJ.";
                            Logger.LogInformation(Infra.Extensions.Logs.ComandoInvalido(msg));
                            return CommandResult.Invalid(msg);
                        }                        
                    }     
              
                    parte = Parte.CriarPessoaJuridica(
                        command.Nome,
                        CNPJ.FromString(command.Documento), EstadoEnum.PorId(command.EstadoId), DataString.FromNullableString(command.Endereco),
                        command.CEP, command.Cidade,
                       command.Bairro,command.EnderecosAdicionais,
                        Telefone.FromNullableString(command.Telefone), Telefone.FromNullableString(command.Celular),
                        command.TelefonesAdicionais, command.DataCartaFianca, command.ValorCartaFianca);
                }
                else if (CPF.IsValidForSisjur(command.Documento))
                {
                    //CA15
                    Parte partePF = DatabaseContext.Partes.FirstOrDefault(x => x.CPF == command.Documento);              

                    if (partePF != null)
                    {
                        if (DatabaseContext.Partes.Any(x => x.CPF == command.Documento && x.Nome == command.Nome.ToUpper()))
                        {
                            string msg = "Já existe uma parte cadastrada com este CPF e o mesmo nome.";
                            Logger.LogInformation(Infra.Extensions.Logs.ComandoInvalido(msg));
                            return CommandResult.Invalid(msg);
                        }

                        else if (!command.Documento.Equals("11111111111") && !command.Documento.Equals("99999999999"))
                        {
                            string msg = "Já existe uma parte cadastrada com este CPF.";
                            Logger.LogInformation(Infra.Extensions.Logs.ComandoInvalido(msg));
                            return CommandResult.Invalid(msg);
                        }
                    }                                  

                    parte = Parte.CriarPessoaFisica(
                        command.Nome,
                        CPF.FromString(command.Documento), EstadoEnum.PorId(command.EstadoId), command.Endereco,
                        command.CEP, command.Cidade,
                        command.Bairro, command.EnderecosAdicionais,
                        Telefone.FromNullableString(command.Telefone), Telefone.FromNullableString(command.Celular),
                        command.TelefonesAdicionais, command.CarteiraTrabalho, command.DataCartaFianca, command.ValorCartaFianca);
                }
                else 
                {
                    string msg = "Documento informado inválido.";
                    Logger.LogInformation(Infra.Extensions.Logs.ComandoInvalido(msg));
                    return CommandResult.Invalid(msg);
                }
                //CA16
                if (DatabaseContext.Fornecedores.Any(x => x.CNPJ == command.Documento))
                {
                    string msg = $"Já existe uma parte cadastrada com este CNPJ(Fornecedor) - {command.Documento}";

                    Logger.LogInformation(Infra.Extensions.Logs.ComandoInvalido(msg));
                    return CommandResult.Invalid(msg);
                }                   

                if (parte.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(parte.Notifications.ToNotificationsString());
                }

                DatabaseContext.Add(parte);
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

            string entityName = "Parte";
            string commandName = $"Removendo {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try
            {
                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_PARTE))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_PARTE, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, id));
                Parte parte = DatabaseContext.Partes.FirstOrDefault(x => x.Id == id);

                if (parte is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, id));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, id));
                }

                if (DatabaseContext.Processos.Any(x => x.EmpresaDoGrupo.Id == parte.Id))
                {
                    string msg = $"Empresa relacionada (Parte) com processo.";

                    Logger.LogInformation(Infra.Extensions.Logs.ComandoInvalido(msg));
                    return CommandResult.Invalid(msg);
                }

                if (DatabaseContext.Processos.Any(x => x.Orgao.Id == parte.Id))
                {
                    string msg = $"Orgao relacionado (Parte) com processo.";

                    Logger.LogInformation(Infra.Extensions.Logs.ComandoInvalido(msg));
                    return CommandResult.Invalid(msg);
                }

                if (DatabaseContext.ValoresProcesso.Any(x => x.COD_PARTE_EFETUOU == parte.Id || x.COD_PARTE_LEVANTOU == parte.Id))
                {
                    string msg = $"Valor relacionado (Parte) com processo.";

                    Logger.LogInformation(Infra.Extensions.Logs.ComandoInvalido(msg));
                    return CommandResult.Invalid(msg);
                }

                if (DatabaseContext.PagamentosProcesso.Any(x => x.Parte.Id == parte.Id))
                {
                    string msg = $"Pagamento do processo (Parte) relacionado com processo.";

                    Logger.LogInformation(Infra.Extensions.Logs.ComandoInvalido(msg));
                    return CommandResult.Invalid(msg);
                }

                if (DatabaseContext.PagamentosObjetosProcesso.Any(x => x.Parte.Id == parte.Id))
                {
                    string msg = $"Pagamento do processo (Parte) tributário relacionado com processo.";

                    Logger.LogInformation(Infra.Extensions.Logs.ComandoInvalido(msg));
                    return CommandResult.Invalid(msg);
                }

                if (DatabaseContext.Estabelecimentos.Any(x => x.Id == parte.Id))
                {
                    string msg = $"Estabelecimento relacionado com processo.";

                    Logger.LogInformation(Infra.Extensions.Logs.ComandoInvalido(msg));
                    return CommandResult.Invalid(msg);
                }

                if (DatabaseContext.Lotes.Any(x => x.EmpresaDoGrupo.Id == parte.Id))
                {
                    string msg = $"Lote (Parte) relacionado com processo.";

                    Logger.LogInformation(Infra.Extensions.Logs.ComandoInvalido(msg));
                    return CommandResult.Invalid(msg);
                }

                if (DatabaseContext.AlocacoesPrepostos.Any(x => x.EmpresaDoGrupo.Id == parte.Id))
                {
                    string msg = $"Lote (Parte) relacionado com processo.";

                    Logger.LogInformation(Infra.Extensions.Logs.ComandoInvalido(msg));
                    return CommandResult.Invalid(msg);
                }

                if (DatabaseContext.PartesProcessos.Any(x => x.Parte.Id == parte.Id))
                {
                    string msg = $"Parte relacionado com processo.";

                    Logger.LogInformation(Infra.Extensions.Logs.ComandoInvalido(msg));
                    return CommandResult.Invalid(msg);
                }


                Logger.LogInformation(Infra.Extensions.Logs.RemovendoEntidade(entityName, id));
                DatabaseContext.Remove(parte);

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
