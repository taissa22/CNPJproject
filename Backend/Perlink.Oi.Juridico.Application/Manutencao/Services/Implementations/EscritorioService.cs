using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Constants;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services.Implementations
{
    internal class EscritorioService : IEscritorioService
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IEscritorioService> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public EscritorioService(IDatabaseContext databaseContext, ILogger<IEscritorioService> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        public CommandResult<Escritorio> Criar(CriarEscritorioCommand command)
        {
            string entityName = "Escritorio";
            string commandName = $"Criar {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try
            {
                command.Validate();
                if (command.Invalid)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.ComandoInvalido(commandName));
                    return CommandResult<Escritorio>.Invalid(command.Notifications.ToNotificationsString());
                }

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ESCRITORIO))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ESCRITORIO, UsuarioAtual.Login));
                    return CommandResult<Escritorio>.Forbidden();
                }

                if (command.TipoPessoaValor == "J")
                {
                    Logger.LogInformation(Infra.Extensions.Logs.ValidandoDuplicidades());

                    var TaCadastrado = DatabaseContext.Escritorios.Where(x => x.CNPJ == command.CNPJ).Any();
                    if (TaCadastrado)
                    {
                        Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida("Escritorio"));
                        return CommandResult<Escritorio>.Invalid("Já existe um Escritório cadastrado com esse CNPJ");
                    }
                }
                else
                {
                    var TaCadastrado = DatabaseContext.Escritorios.Where(x => x.CPF == command.CPF).Any();
                    if (TaCadastrado)
                    {
                        Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida("Escritorio"));
                        return CommandResult<Escritorio>.Invalid("Já existe um Escritório cadastrado com esse CPF");
                    }
                }

                Logger.LogInformation(Infra.Extensions.Logs.CriandoEntidade(entityName));
                Escritorio escritorio = Escritorio.Criar(command.Nome,
                                                         (command.Ativo!=null? command.Ativo.Value:true),
                                                         command.Endereco,
                                                         command.CivelEstrategico,
                                                         command.TipoPessoaValor,
                                                         command.CPF,
                                                         command.CEP,
                                                         command.Cidade,
                                                         command.Email,
                                                         command.EstadoId,
                                                         command.Bairro,
                                                         command.IndAdvogado,
                                                         command.IndAreaCivel,
                                                         command.IndAreaJuizado,
                                                         command.IndAreaRegulatoria,
                                                         command.IndAreaTrabalhista,
                                                         command.IndAreaTributaria,
                                                         command.IndAreaCivelAdministrativo,
                                                         command.IndAreaCriminalAdministrativo,
                                                         command.IndAreaCriminalJudicial,
                                                         command.IndAreaPEX,
                                                         command.IndAreaProcon,
                                                         command.AlertaEm,
                                                         command.CodProfissionalSAP,
                                                         command.Site,
                                                         command.EhContadorPex.GetValueOrDefault() ,
                                                         command.CNPJ,
                                                         command.Telefone,
                                                         command.TelefoneDDD,
                                                         command.Celular,
                                                         command.CelularDDD,
                                                         command.Fax,
                                                         command.FaxDDD,
                                                         command.selecionadosJec,
                                                         command.selecionadosCivelConsumidor,
                                                         command.EnviarAppPreposto);

                if (escritorio.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult<Escritorio>.Invalid(escritorio.Notifications.ToNotificationsString());
                }

                DatabaseContext.Add(escritorio);
                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade(entityName));

                DatabaseContext.SaveChanges();

                var listaIncluir = new List<EscritorioEstado>();

                if (escritorio.IndAreaJuizado == true)
                {
                    var listaGravarJec = escritorio.selecionadosJec.Where(x => x.Selecionado);
                    foreach (var item in listaGravarJec)
                    {
                        var novo = EscritorioEstado.Criar(escritorio.Id, item.Id, (int)TipoProcessoEnum.JuizadoEspecial);
                        listaIncluir.Add(novo);
                    }
                }

                
                if (escritorio.IndAreaCivel == true)
                {
                    var listaCivelGravar = escritorio.selecionadosCivelConsumidor.Where(x => x.Selecionado);
                    foreach (var item in listaCivelGravar)
                    {
                        var novo = EscritorioEstado.Criar(escritorio.Id, item.Id, (int)TipoProcessoEnum.CivelConsumidor);
                        listaIncluir.Add(novo);
                    }
                }

                listaIncluir.ForEach(x => DatabaseContext.Add(x));
                DatabaseContext.SaveChanges();

                Logger.LogInformation(Infra.Extensions.Logs.OperacaoFinalizada(commandName));

                return CommandResult<Escritorio>.Valid(escritorio);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, Infra.Extensions.Logs.OperacaoComErro(commandName));
                return CommandResult<Escritorio>.Invalid(ex.Message);
            }
        }

        public CommandResult<Escritorio> Atualizar(AtualizarEscritorioCommand command)
        {
            string entityName = "Escritorio";
            string commandName = $"Atualizar {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try
            {
                command.Validate();
                if (command.Invalid)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.ComandoInvalido(commandName));
                    return CommandResult<Escritorio>.Invalid(command.Notifications.ToNotificationsString());
                }

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ESCRITORIO))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ESCRITORIO, UsuarioAtual.Login));
                    return CommandResult<Escritorio>.Invalid(command.Notifications.ToNotificationsString());
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, $"{command.Id}, {command.Nome}"));
                Escritorio escritorio = DatabaseContext.Escritorios.FirstOrDefault(x => x.Id == command.Id);

                if (escritorio is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, $"{command.Id}, {command.Nome}"));
                    return CommandResult<Escritorio>.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, $"{command.Id}, {command.Nome}"));
                }

                if (command.TipoPessoaValor == "J")
                {
                    Logger.LogInformation(Infra.Extensions.Logs.ValidandoDuplicidades());

                    var TaCadastrado = DatabaseContext.Escritorios.Where(x => x.CNPJ == command.CNPJ && x.Id != command.Id).Any();
                    if (TaCadastrado)
                    {
                        Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida("Escritorio"));
                        return CommandResult<Escritorio>.Invalid("Já existe um Escritório cadastrado com esse CNPJ");
                    }
                }
                else
                {
                    var TaCadastrado = DatabaseContext.Escritorios.Where(x => x.CPF == command.CPF && x.Id != command.Id).Any();
                    if (TaCadastrado)
                    {
                        Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida("Escritorio"));
                        return CommandResult<Escritorio>.Invalid("Já existe um Escritório cadastrado com esse CPF");
                    }
                }

                if (escritorio.IndAreaCivel.GetValueOrDefault() && (!command.IndAreaCivel))
                {
                    if (DatabaseContext.Processos.Where(x => x.EscritorioId == escritorio.Id && x.TipoProcessoId == (int)TipoProcessoEnum.CivelConsumidor).Any())
                    {
                        string retorno = "Não é possível desmarcar: Civel Consumidor, pois existem processos associados para esta área.";
                        Logger.LogInformation(retorno);
                        return CommandResult<Escritorio>.Invalid(retorno);
                    }
                }

                if (escritorio.CivelEstrategico.GetValueOrDefault() && (!command.CivelEstrategico.GetValueOrDefault()))
                {
                    if (DatabaseContext.Processos.Where(x => x.EscritorioId == escritorio.Id && x.TipoProcessoId == (int)TipoProcessoEnum.CivelEstrategico).Any())
                    {
                        string retorno = "Não é possível desmarcar: Civel Estratégico, pois existem processos associados para esta área.";
                        Logger.LogInformation(retorno);
                        return CommandResult<Escritorio>.Invalid(retorno);
                    }
                }

                if (escritorio.IndAreaCivelAdministrativo && (!command.IndAreaCivelAdministrativo))
                {
                    if (DatabaseContext.Processos.Where(x => x.EscritorioId == escritorio.Id && x.TipoProcessoId == (int)TipoProcessoEnum.CivelAdministrativo).Any())
                    {
                        string retorno = "Não é possível desmarcar: Civel Administrativo, pois existem processos associados para esta área.";
                        Logger.LogInformation(retorno);
                        return CommandResult<Escritorio>.Invalid(retorno);
                    }
                }

                if (escritorio.IndAreaCriminalAdministrativo && (!command.IndAreaCriminalAdministrativo))
                {
                    if (DatabaseContext.Processos.Where(x => x.EscritorioId == escritorio.Id && x.TipoProcessoId == (int)TipoProcessoEnum.CriminalAdministrativo).Any())
                    {
                        string retorno = "Não é possível desmarcar: Criminal Administrativo, pois existem processos associados para esta área.";
                        Logger.LogInformation(retorno);
                        return CommandResult<Escritorio>.Invalid(retorno);
                    }
                }

                if (escritorio.IndAreaCriminalJudicial && (!command.IndAreaCriminalJudicial))
                {
                    if (DatabaseContext.Processos.Where(x => x.EscritorioId == escritorio.Id && x.TipoProcessoId == (int)TipoProcessoEnum.CriminalJudicial).Any())
                    {
                        string retorno = "Não é possível desmarcar: Criminal Judicial, pois existem processos associados para esta área.";
                        Logger.LogInformation(retorno);
                        return CommandResult<Escritorio>.Invalid(retorno);
                    }
                }

                if (escritorio.IndAreaJuizado.GetValueOrDefault() && (!command.IndAreaJuizado))
                {
                    if (DatabaseContext.Processos.Where(x => x.EscritorioId == escritorio.Id && x.TipoProcessoId == (int)TipoProcessoEnum.JuizadoEspecial).Any())
                    {
                        string retorno = "Não é possível desmarcar: Juizado Especial, pois existem processos associados para esta área.";
                        Logger.LogInformation(retorno);
                        return CommandResult<Escritorio>.Invalid(retorno);
                    }
                }

                if (escritorio.IndAreaPEX && (!command.IndAreaPEX))
                {
                    if (DatabaseContext.Processos.Where(x => x.EscritorioId == escritorio.Id && x.TipoProcessoId == (int)TipoProcessoEnum.Pex).Any())
                    {
                        string retorno = "Não é possível desmarcar: PEX, pois existem processos associados para esta área.";
                        Logger.LogInformation(retorno);
                        return CommandResult<Escritorio>.Invalid(retorno);
                    }
                }

                if (escritorio.IndAreaPEX && (!command.IndAreaPEX))
                {
                    if (DatabaseContext.Protocolos.Where(x => x.ProfissionalId == escritorio.Id && x.TipoProcessoId == (int)TipoProcessoEnum.Pex).Any())
                    {
                        string retorno = "Não é possível desmarcar: PEX, pois existem protocolos associados para esta área.";
                        Logger.LogInformation(retorno);
                        return CommandResult<Escritorio>.Invalid(retorno);
                    }
                }

                if (escritorio.IndAreaProcon && (!command.IndAreaProcon))
                {
                    if (DatabaseContext.Processos.Where(x => x.EscritorioId == escritorio.Id && x.TipoProcessoId == (int)TipoProcessoEnum.Procon).Any())
                    {
                        string retorno = "Não é possível desmarcar: Procon, pois existem processos associados para esta área.";
                        Logger.LogInformation(retorno);
                        return CommandResult<Escritorio>.Invalid(retorno);
                    }
                }

                if (escritorio.IndAreaRegulatoria.GetValueOrDefault() && (!command.IndAreaRegulatoria))
                {
                    if (DatabaseContext.Processos.Where(x => x.EscritorioId == escritorio.Id && x.TipoProcessoId == (int)TipoProcessoEnum.Administrativo).Any())
                    {
                        string retorno = "Não é possível desmarcar: Administrativo, pois existem processos associados para esta área.";
                        Logger.LogInformation(retorno);
                        return CommandResult<Escritorio>.Invalid(retorno);
                    }
                }

                if (escritorio.IndAreaTrabalhista.GetValueOrDefault() && (!command.IndAreaTrabalhista))
                {
                    if (DatabaseContext.Processos.Where(x => x.EscritorioId == escritorio.Id && (x.TipoProcessoId == (int)TipoProcessoEnum.Trabalhista || x.TipoProcessoId == (int)TipoProcessoEnum.TrabalhistaAdministrativo)).Any())
                    {
                        string retorno = "Não é possível desmarcar: Trabalhista, pois existem processos associados para esta área.";
                        Logger.LogInformation(retorno);
                        return CommandResult<Escritorio>.Invalid(retorno);
                    }
                }

                if (escritorio.IndAreaTributaria.GetValueOrDefault() && (!command.IndAreaTributaria))
                {
                    if (DatabaseContext.Processos.Where(x => x.EscritorioId == escritorio.Id && (x.TipoProcessoId == (int)TipoProcessoEnum.TributarioJudicial || x.TipoProcessoId == (int)TipoProcessoEnum.TributarioAdministrativo)).Any())
                    {
                        string retorno = "Não é possível desmarcar: Tributário, pois existem processos associados para esta área.";
                        Logger.LogInformation(retorno);
                        return CommandResult<Escritorio>.Invalid(retorno);
                    }
                }

                Logger.LogInformation(Infra.Extensions.Logs.AtualizandoEntidade(entityName, $"{command.Id}, {command.Nome}"));

                escritorio.Atualizar(command.Id,
                                     command.Nome.ToUpper(),
                                     (command.Ativo != null ? command.Ativo.Value: true),
                                     command.Endereco.ToUpper(),
                                     command.CivelEstrategico,
                                     command.TipoPessoaValor,
                                     command.CPF,
                                     command.CEP,
                                     command.Cidade.ToUpper(),
                                     command.Email,
                                     command.EstadoId,
                                     command.Bairro,
                                     command.IndAdvogado,
                                     command.IndAreaCivel,
                                     command.IndAreaJuizado,
                                     command.IndAreaRegulatoria,
                                     command.IndAreaTrabalhista,
                                     command.IndAreaTributaria,
                                     command.IndAreaCivelAdministrativo,
                                     command.IndAreaCriminalAdministrativo,
                                     command.IndAreaCriminalJudicial,
                                     command.IndAreaPEX,
                                     command.IndAreaProcon,
                                     command.AlertaEm,
                                     command.CodProfissionalSAP,
                                     command.Site,
                                     command.EhContadorPex.GetValueOrDefault() ,
                                     command.CNPJ,
                                     command.Telefone,
                                     command.TelefoneDDD,
                                     command.Celular,
                                     command.CelularDDD,
                                     command.Fax,
                                     command.FaxDDD,
                                     command.selecionadosJec,
                                     command.selecionadosCivelConsumidor,
                                     command.EnviarAppPreposto) ;

                if (escritorio.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida($"{command.Id}, {command.Nome}"));
                    return CommandResult<Escritorio>.Invalid(escritorio.Notifications.ToNotificationsString());
                }

                var ListaJec = (from a in DatabaseContext.EscritoriosEstados
                                where (a.Profissional.Id == command.Id) && (a.TipoProcesso.Id == (int)TipoProcessoEnum.JuizadoEspecial)
                                select a).AsNoTracking();

                foreach (var item in ListaJec)
                {
                    var remover = escritorio.selecionadosJec.FirstOrDefault(x => x.Id == item.Estado.Id && !x.Selecionado);

                    if (remover != null)
                    {
                        var removerEstado = DatabaseContext.EscritoriosEstados.FirstOrDefault(x => x.Estado.Id == remover.Id && x.Profissional.Id == command.Id && x.TipoProcesso.Id == (int)TipoProcessoEnum.JuizadoEspecial);

                        if (removerEstado != null)
                        {
                            DatabaseContext.Remove(removerEstado);
                        }
                    }
                }

                var listaGravar = escritorio.selecionadosJec.Where(x => x.Selecionado);
                foreach (var item in listaGravar)
                {
                    if (!ListaJec.Where(x => x.Estado.Id == item.Id).Any())
                    {
                        var novo = EscritorioEstado.Criar(command.Id, item.Id, (int)TipoProcessoEnum.JuizadoEspecial);
                        DatabaseContext.Add(novo);
                    }
                }

                var ListaCivelConsumidor = (from a in DatabaseContext.EscritoriosEstados
                                            where (a.Profissional.Id == command.Id) && (a.TipoProcesso.Id == (int)TipoProcessoEnum.CivelConsumidor)
                                            select a).AsNoTracking();

                foreach (var item in ListaCivelConsumidor)
                {
                    var remover = escritorio.selecionadosCivelConsumidor.FirstOrDefault(x => x.Id == item.Estado.Id && !x.Selecionado);

                    if (remover != null)
                    {
                        var removerEstado = DatabaseContext.EscritoriosEstados.FirstOrDefault(x => x.Estado.Id == remover.Id && x.Profissional.Id == command.Id && x.TipoProcesso.Id == (int)TipoProcessoEnum.CivelConsumidor);

                        if (removerEstado != null)
                        {
                            DatabaseContext.Remove(removerEstado);
                        }
                    }
                }

                if (escritorio.selecionadosCivelConsumidor != null)
                {
                    var listaCivelGravar = escritorio.selecionadosCivelConsumidor.Where(x => x.Selecionado);
                    foreach (var item in listaCivelGravar)
                    {
                        if (!ListaCivelConsumidor.Where(x => x.Estado.Id == item.Id).Any())
                        {
                            var novo = EscritorioEstado.Criar(command.Id, item.Id, (int)TipoProcessoEnum.CivelConsumidor);
                            DatabaseContext.Add(novo);
                        }
                    }
                }

                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade($"{command.Id}, {command.Nome}"));
                DatabaseContext.SaveChanges();

                Logger.LogInformation(Infra.Extensions.Logs.OperacaoFinalizada(commandName));
                return CommandResult<Escritorio>.Valid(escritorio);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, Infra.Extensions.Logs.OperacaoComErro(commandName));
                return CommandResult<Escritorio>.Invalid(ex.Message);
            }
        }

        public CommandResult Remover(int id)
        {
            string entityName = "Escritorio";
            string commandName = $"Removendo {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try
            {
                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ESCRITORIO))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ESCRITORIO, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, $"{id}"));
                Escritorio escritorio = DatabaseContext.Escritorios.FirstOrDefault(x => x.Id == id);

                if (escritorio is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, $"{id}"));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, $"{id}"));
                }

                string message = "Não é possível excluir esse escritório, pois ele está relacionado com";

                if (DatabaseContext.Processos.Where(x => x.EscritorioId == escritorio.Id || x.EscritorioAcompanhanteId == escritorio.Id).Any())
                {
                    string retorno = message + " Processos";
                    Logger.LogInformation(retorno);
                    return CommandResult.Invalid(retorno);
                }

                if (DatabaseContext.EscritoriosDosUsuarios.Where(x => x.EscritorioId == escritorio.Id).Any())
                {
                    string retorno = message + " ACA_USUARIO_ESCRITORIO";
                    Logger.LogInformation(retorno);
                    return CommandResult.Invalid(retorno);
                }

                if (DatabaseContext.PagamentosProcesso.Where(x => x.Profissional.Id == escritorio.Id).Any())
                {
                    string retorno = message + " PAGAMENTO_PROCESSO";
                    Logger.LogInformation(retorno);
                    return CommandResult.Invalid(retorno);
                }

                if (DatabaseContext.DespesasDosProfissionais.Where(x => x.ProfissionalId == escritorio.Id).Any())
                {
                    string retorno = message + " DESPESA_PROFISSIONAL";
                    Logger.LogInformation(retorno);
                    return CommandResult.Invalid(retorno);
                }

                if (DatabaseContext.Fornecedores.Where(x => x.EscritorioId == escritorio.Id).Any())
                {
                    string retorno = message + " FORNECEDOR";
                    Logger.LogInformation(retorno);
                    return CommandResult.Invalid(retorno);
                }

                if (DatabaseContext.AdvogadosDosAutores.Where(x => x.ProfissionalId == escritorio.Id).Any())
                {
                    string retorno = message + " ADVOGADO_AUTOR_PROCESSO";
                    Logger.LogInformation(retorno);
                    return CommandResult.Invalid(retorno);
                }

                if (DatabaseContext.AudienciasDosProcessos.Where(x => x.EscritorioId == escritorio.Id).Any())
                {
                    string retorno = message + " AUDIENCIA_PROCESSO";
                    Logger.LogInformation(retorno);
                    return CommandResult.Invalid(retorno);
                }

                if (DatabaseContext.Protocolos.Where(x => x.ProfissionalId == escritorio.Id).Any())
                {
                    string retorno = message + " PROTOCOLOS";
                    Logger.LogInformation(retorno);
                    return CommandResult.Invalid(retorno);
                }

                if (DatabaseContext.AdvogadosDosEscritorios.Where(x => x.EscritorioId == escritorio.Id).Any())
                {
                    string retorno = "Não é possível excluir esse escritório, pois existem advogados associados a ele.";
                    Logger.LogInformation(retorno);
                    return CommandResult.Invalid(retorno);
                }

                var estadoAtuacao = DatabaseContext.EscritoriosEstados.Where(x => x.Profissional.Id == escritorio.Id);
                foreach (var item in estadoAtuacao)
                {
                    DatabaseContext.Remove(item);
                }

                Logger.LogInformation(Infra.Extensions.Logs.RemovendoEntidade(entityName, $"{id}"));
                DatabaseContext.Remove(escritorio);

                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade($"{id}"));
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