using DocumentFormat.OpenXml.EMMA;
using Microsoft.AspNetCore.Authorization;
using Oi.Juridico.Contextos.V2.ManutencaoEscritorioContext.Data;
using Oi.Juridico.Contextos.V2.ManutencaoEscritorioContext.Entities;
using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;
using Perlink.Oi.Juridico.Infra.External;
using Perlink.Oi.Juridico.Infra.Constants;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Oi.Juridico.Contextos.V2.ControleDeAcessoContext.Data;
using Oi.Juridico.WebApi.V2.Helpers;
using Oi.Juridico.Contextos.V2.DistribuicaoProcessoEscritorioContext.Data;
using Perlink.Oi.Juridico.Application.Manutencao.Repositories;
using Perlink.Oi.Juridico.Infra.Dto;
using Perlink.Oi.Juridico.Infra;
using Oi.Juridico.WebApi.V2.Areas.Manutencoes.Solicitante.CsvHelperMap;
using Oi.Juridico.AddOn.Extensions.IEnumerable;
using Oi.Juridico.WebApi.V2.Extensions;
using DocumentFormat.OpenXml.Wordprocessing;
using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.ValueObjects;
using System.Runtime.ConstrainedExecution;
using System.Security.Policy;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Oi.Juridico.WebApi.V2.Areas.Manutencoes.Escritorio
{
    [Authorize]
    [ApiController]
    [Route("manutencao/escritorio")]
    public class EscritorioController : ApiControllerBase
    {
        private readonly ManutencaoEscritorioContext _db;
        private readonly DistribuicaoProcessoEscritorioContext _distribuicao;
        private readonly ControleDeAcessoContext _controleAcesso;
        private readonly ILogger<EscritorioController> _logger;

        public EscritorioController(ManutencaoEscritorioContext db, ILogger<EscritorioController> logger, ControleDeAcessoContext controleAcesso, DistribuicaoProcessoEscritorioContext distribuicao)
        {
            _db = db;
            _logger = logger;
            _controleAcesso = controleAcesso;
            _distribuicao = distribuicao;
        }

        [HttpGet]
        public async Task<IActionResult> ObterPaginado(CancellationToken ct, string? pesquisa, string? estado, int areaAtuacao = 0, int pagina = 1, int quantidade = 8, string coluna = "nome", string direcao = "asc")
        {
            using var scope = await _db.Database.BeginTransactionAsync(ct);

            _db.PesquisarPorCaseInsensitive();

            string logName = "Escritório";
            _logger.LogInformation(LogHelpers.IniciandoOperacao($"Obter {logName}"));

            if (await _controleAcesso.TemPermissaoAsync(User.Identity!.Name, Permissoes.ACESSAR_ESCRITORIO) == false)
            {
                _logger.LogInformation(LogHelpers.PermissaoNegada(Permissoes.ACESSAR_ESCRITORIO, User.Identity!.Name!));
                return Result(CommandResult<PaginatedQueryResult<ObterPaginadoResponse>>.Forbidden());
            }

            _logger.LogInformation(LogHelpers.Obtendo(logName));
            var listaBase = ObterBase(estado, areaAtuacao, sort: EnumHelpers.ParseOrDefault(coluna, EscritorioSort.Id), ascending: direcao == "asc", pesquisa);

            _logger.LogInformation(LogHelpers.Obtendo("Total de registros"));
            var total = await listaBase.CountAsync(ct);

            _logger.LogInformation(LogHelpers.Obtendo("Quantidade de registros para saltar"));
            var skip = Pagination.PagesToSkip(quantidade, total, pagina);

            var resultado = new PaginatedQueryResult<ObterPaginadoResponse>()
            {
                Total = total,
                Data = await listaBase.Skip(skip).Take(quantidade).ToArrayAsync(ct)
            };

            _logger.LogInformation(LogHelpers.Retornando(logName));
            return Result(CommandResult<PaginatedQueryResult<ObterPaginadoResponse>>.Valid(resultado));
        }

        [HttpPost]
        public async Task<IActionResult> CriarEscritorioAsync(CancellationToken ct, [FromBody] CriarEscritorioRequest dados)
        {
            string entityName = "Escritorio";
            string commandName = $"Criar {entityName}";
            _logger.LogInformation(LogHelpers.IniciandoOperacao(commandName));

            try
            {
                dados.Validate();
                if (dados.Invalid)
                {
                    _logger.LogInformation(LogHelpers.ComandoInvalido(commandName));
                    return Result(CommandResult<CriarEscritorioRequest>.Invalid(dados.Notifications.ToNotificationsString()));
                }

                if (await _controleAcesso.TemPermissaoAsync(User.Identity!.Name, Permissoes.ACESSAR_ESCRITORIO) == false)
                {
                    _logger.LogInformation(LogHelpers.PermissaoNegada(Permissoes.ACESSAR_ESCRITORIO, User.Identity!.Name!));
                    return Result(CommandResult<PaginatedQueryResult<ObterPaginadoResponse>>.Forbidden());
                }

                // verifica se o escritório já está cadastrado
                {
                    _logger.LogInformation(LogHelpers.ValidandoDuplicidades());

                    var TaCadastrado = await EsctitorioEstaCadastradoAsync(dados, false, ct);

                    if (TaCadastrado)
                    {
                        _logger.LogInformation(LogHelpers.EntidadeInvalida("Escritorio"));
                        return Result(CommandResult<CriarEscritorioRequest>.Invalid($"Já existe um Escritório cadastrado com esse {(dados.TipoPessoaValor == "J" ? "CNPJ" : "CPF")}"));
                    }
                }

                if (dados.HasNotifications)
                {
                    _logger.LogInformation(LogHelpers.EntidadeInvalida(entityName));
                    return Result(CommandResult<CriarEscritorioRequest>.Invalid(dados.Notifications.ToNotificationsString()));
                }

                _logger.LogInformation(LogHelpers.CriandoEntidade(entityName));
                var escritorio = new Profissional();
                escritorio.CodProfissional = (int)_db.GetNextSequence("PROF_SEQ_01");
                escritorio.NomProfissional = dados.Nome!.ToUpper();
                escritorio.IndEscritorio = "S";
                escritorio.CodTipoPessoa = dados.TipoPessoaValor;
                escritorio.CpfProfissional = dados.CPF;
                escritorio.CodCep = dados.CEP;
                escritorio.DscCidade = dados.Cidade;
                escritorio.EndProfissional = dados.Endereco;
                escritorio.Telefone = dados.Telefone;
                escritorio.DddTelefone = dados.TelefoneDDD;
                escritorio.Celular = dados.Celular;
                escritorio.DddCelular = dados.CelularDDD;
                escritorio.Fax = dados.Fax;
                escritorio.DddFax = dados.FaxDDD;
                escritorio.DscEmail = dados.Email;
                escritorio.CodEstado = dados.EstadoId;
                escritorio.DscBairro = dados.Bairro;
                escritorio.IndAdvogado = dados.IndAdvogado ? "S" : "N";
                escritorio.AlertaEm = dados.AlertaEm;
                escritorio.CodProfissionalSap = dados.CodProfissionalSAP;
                escritorio.DscSite = dados.Site;
                escritorio.CgcProfissional = dados.CNPJ;

                escritorio.IndAtivo = dados.Ativo.GetValueOrDefault() ? "S" : "N";
                escritorio.IndAreaCivel = dados.IndAreaCivel ? "S" : "N";
                escritorio.IndAreaJuizado = dados.IndAreaJuizado ? "S" : "N";
                escritorio.IndAreaRegulatoria = dados.IndAreaRegulatoria ? "S" : "N";
                escritorio.IndAreaTrabalhista = dados.IndAreaTrabalhista ? "S" : "N";
                escritorio.IndAreaTributaria = dados.IndAreaTributaria ? "S" : "N";
                escritorio.IndCivelAdm = dados.IndAreaCivelAdministrativo ? "S" : "N";
                escritorio.IndCriminalAdm = dados.IndAreaCriminalAdministrativo ? "S" : "N";
                escritorio.IndCriminalJudicial = dados.IndAreaCriminalJudicial ? "S" : "N";
                escritorio.IndCivelEstrategico = dados.CivelEstrategico.GetValueOrDefault() ? "S" : "N";
                escritorio.IndPex = dados.IndAreaPEX ? "S" : "N";
                escritorio.IndContadorPex = dados.EhContadorPex.GetValueOrDefault() ? "S" : "N";
                escritorio.IndProcon = dados.IndAreaProcon ? "S" : "N";
                escritorio.EnviarAppPreposto = dados.EnviarAppPreposto? "S": "N";

                if (escritorio.IndAreaJuizado == "S")
                {
                    var listaGravarJec = dados.selecionadosJec.Where(x => x.Selecionado).Select(x => new EscritorioEstado { CodEstado = x.Id, CodTipoProcesso = 7 }).ToArray();
                    escritorio.EscritorioEstado.AddAll(listaGravarJec);
                }

                if (escritorio.IndAreaCivel == "S")
                {
                    var listaCivelGravar = dados.selecionadosCivelConsumidor.Where(x => x.Selecionado).Select(x => new EscritorioEstado { CodEstado = x.Id, CodTipoProcesso = 1 }).ToArray();
                    escritorio.EscritorioEstado.AddAll(listaCivelGravar);
                }

                _db.Profissional.Add(escritorio);
                _logger.LogInformation(LogHelpers.SalvandoEntidade(entityName));

                await _db.SaveChangesAsync(User.Identity!.Name, true, ct);

                // seta o id gerado na inclusão
                dados.Id = escritorio.CodProfissional;

                _logger.LogInformation(LogHelpers.OperacaoFinalizada(commandName));

                return Result(CommandResult<CriarEscritorioRequest>.Valid(dados));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, LogHelpers.OperacaoComErro(commandName));
                return Result(CommandResult<CriarEscritorioRequest>.Invalid(ex.Message));
            }
        }

        private async Task<bool> EsctitorioEstaCadastradoAsync(CriarEscritorioRequest dados, bool ehAlteracao, CancellationToken ct)
        {
            return dados.TipoPessoaValor == "J"
                ? await _db.Profissional.AnyAsync(x => x.CgcProfissional == dados.CNPJ && (ehAlteracao == false || x.CodProfissional != dados.Id), ct)
                : await _db.Profissional.AnyAsync(x => x.CpfProfissional == dados.CPF && (ehAlteracao == false || x.CodProfissional != dados.Id), ct);
        }

        [HttpPut]
        public async Task<IActionResult> AtualizarEscritorioAsync(CancellationToken ct, [FromBody] AtualizarEscritorioRequest dados)
        {
            string entityName = "Escritorio";
            string commandName = $"Atualizar {entityName}";
            _logger.LogInformation(LogHelpers.IniciandoOperacao(commandName));

            try
            {
                dados.Validate();
                if (dados.Invalid)
                {
                    _logger.LogInformation(LogHelpers.ComandoInvalido(commandName));
                    return Result(CommandResult<AtualizarEscritorioRequest>.Invalid(dados.Notifications.ToNotificationsString()));
                }

                if (await _controleAcesso.TemPermissaoAsync(User.Identity!.Name, Permissoes.ACESSAR_ESCRITORIO) == false)
                {
                    _logger.LogInformation(LogHelpers.PermissaoNegada(Permissoes.ACESSAR_ESCRITORIO, User.Identity!.Name!));
                    return Result(CommandResult<PaginatedQueryResult<ObterPaginadoResponse>>.Forbidden());
                }

                _logger.LogInformation(LogHelpers.ObtendoEntidade(entityName, $"{dados.Id}, {dados.Nome}"));
                var escritorio = _db.Profissional.Include(x => x.EscritorioEstado).FirstOrDefault(x => x.CodProfissional == dados.Id);

                if (escritorio is null)
                {
                    _logger.LogInformation(LogHelpers.EntidadeNaoEncontrada(entityName, $"{dados.Id}, {dados.Nome}"));
                    return Result(CommandResult<AtualizarEscritorioRequest>.Invalid(LogHelpers.EntidadeNaoEncontrada(entityName, $"{dados.Id}, {dados.Nome}")));
                }

                var TaCadastrado = await EsctitorioEstaCadastradoAsync(dados, true, ct);

                if (TaCadastrado)
                {
                    _logger.LogInformation(LogHelpers.EntidadeInvalida("Escritorio"));
                    return Result(CommandResult<CriarEscritorioRequest>.Invalid($"Já existe um Escritório cadastrado com esse {(dados.TipoPessoaValor == "J" ? "CNPJ" : "CPF")}"));
                }

                // verifica se é possível desmarcar
                {
                    string? retorno = null;
                    var codTipoProcessos = await _db.Processo.Where(x => x.CodProfissional == escritorio.CodProfissional).Select(x => x.CodTipoProcesso).Distinct().ToArrayAsync(ct);

                    if (escritorio.IndAtivo == "S" && !dados.Ativo.GetValueOrDefault() && await _db.ParamDistribEscritorio.AnyAsync(x => x.CodProfissional == escritorio.CodProfissional, ct))
                    {
                        retorno = "Não é possível inativar o escritório pois ele está sendo utilizado em uma parametrização de distribuição.";
                    }
                    else if (escritorio.IndAreaCivel == "S" && !dados.IndAreaCivel && (codTipoProcessos.Contains((byte)TipoProcessoEnum.CivelConsumidor) || await _db.ParamDistribEscritorio.AnyAsync(x => x.CodParamDistribuicaoNavigation.CodTipoProcesso == (byte)TipoProcessoEnum.CivelConsumidor && x.CodProfissional == dados.Id, ct)))
                    {
                        retorno = "Não é possível desmarcar: Civel Consumidor, pois existem processos associados para esta área.";
                    }
                    else if (escritorio.IndCivelEstrategico == "S" && !dados.CivelEstrategico.GetValueOrDefault() && codTipoProcessos.Contains((byte)TipoProcessoEnum.CivelEstrategico))
                    {
                        retorno = "Não é possível desmarcar: Civel Estratégico, pois existem processos associados para esta área.";
                    }
                    else if (escritorio.IndCivelAdm == "S" && !dados.IndAreaCivelAdministrativo && codTipoProcessos.Contains((byte)TipoProcessoEnum.CivelAdministrativo))
                    {
                        retorno = "Não é possível desmarcar: Civel Administrativo, pois existem processos associados para esta área.";
                    }
                    else if (escritorio.IndCriminalAdm == "S" && !dados.IndAreaCriminalAdministrativo && codTipoProcessos.Contains((byte)TipoProcessoEnum.CriminalAdministrativo))
                    {
                        retorno = "Não é possível desmarcar: Criminal Administrativo, pois existem processos associados para esta área.";
                    }
                    else if (escritorio.IndCriminalJudicial == "S" && !dados.IndAreaCriminalJudicial && codTipoProcessos.Contains((byte)TipoProcessoEnum.CriminalJudicial))
                    {
                        retorno = "Não é possível desmarcar: Criminal Judicial, pois existem processos associados para esta área.";
                    }
                    else if (escritorio.IndAreaJuizado == "S" && !dados.IndAreaJuizado && (codTipoProcessos.Contains((byte)TipoProcessoEnum.JuizadoEspecial) || await _db.ParamDistribEscritorio.AnyAsync(x => x.CodParamDistribuicaoNavigation.CodTipoProcesso == (byte)TipoProcessoEnum.JuizadoEspecial && x.CodProfissional == dados.Id, ct)))
                    {
                        retorno = "Não é possível desmarcar: Juizado Especial, pois existem processos associados para esta área.";
                    }
                    else if (escritorio.IndPex == "S" && (!dados.IndAreaPEX))
                    {
                        if (codTipoProcessos.Contains((byte)TipoProcessoEnum.Pex))
                        {
                            retorno = "Não é possível desmarcar: PEX, pois existem processos associados para esta área.";
                        }
                        else if (await _db.Protocolos.AnyAsync(x => x.CodProfissional == escritorio.CodProfissional && x.CodTipoProcesso == (int)TipoProcessoEnum.Pex, ct))
                        {
                            retorno = "Não é possível desmarcar: PEX, pois existem protocolos associados para esta área.";
                        }
                    }
                    else if (escritorio.IndProcon == "S" && !dados.IndAreaProcon && codTipoProcessos.Contains((byte)TipoProcessoEnum.Procon))
                    {
                        retorno = "Não é possível desmarcar: Procon, pois existem processos associados para esta área.";
                    }
                    else if (escritorio.IndAreaRegulatoria == "S" && !dados.IndAreaRegulatoria && codTipoProcessos.Contains((byte)TipoProcessoEnum.Administrativo))
                    {
                        retorno = "Não é possível desmarcar: Administrativo, pois existem processos associados para esta área.";
                    }
                    else if (escritorio.IndAreaTrabalhista == "S" && !dados.IndAreaTrabalhista && (codTipoProcessos.Contains((byte)TipoProcessoEnum.Trabalhista) || codTipoProcessos.Contains((byte)TipoProcessoEnum.TrabalhistaAdministrativo)))
                    {
                        retorno = "Não é possível desmarcar: Trabalhista, pois existem processos associados para esta área.";
                    }
                    else if (escritorio.IndAreaTributaria == "S" && !dados.IndAreaTributaria && (codTipoProcessos.Contains((byte)TipoProcessoEnum.TributarioJudicial) || codTipoProcessos.Contains((byte)TipoProcessoEnum.TributarioAdministrativo)))
                    {
                        retorno = "Não é possível desmarcar: Tributário, pois existem processos associados para esta área.";
                    }

                    if (!string.IsNullOrEmpty(retorno))
                    {
                        _logger.LogInformation(retorno);
                        return Result(CommandResult<AtualizarEscritorioRequest>.Invalid(retorno));
                    }
                }

                _logger.LogInformation(LogHelpers.AtualizandoEntidade(entityName, $"{dados.Id}, {dados.Nome}"));

                escritorio.NomProfissional = dados.Nome!.ToUpper();
                escritorio.IndAtivo = dados.Ativo.GetValueOrDefault() ? "S" : "N";
                escritorio.EndProfissional = dados.Endereco!.ToUpper();
                escritorio.DscBairro = dados.Bairro;
                escritorio.DscCidade = dados.Cidade!.ToUpper();
                escritorio.CodEstado = dados.EstadoId;
                escritorio.CodCep = dados.CEP;
                escritorio.DscEmail = dados.Email;
                escritorio.DscSite = dados.Site;
                escritorio.CodTipoPessoa = dados.TipoPessoaValor;
                escritorio.CpfProfissional = dados.CPF;
                escritorio.CgcProfissional = dados.CNPJ;
                escritorio.AlertaEm = dados.AlertaEm;
                escritorio.CodProfissionalSap = dados.CodProfissionalSAP;
                escritorio.Telefone = dados.Telefone;
                escritorio.DddTelefone = dados.TelefoneDDD;
                escritorio.Celular = dados.Celular;
                escritorio.DddCelular = dados.CelularDDD;
                escritorio.Fax = dados.Fax;
                escritorio.DddFax = dados.FaxDDD;
                escritorio.IndAdvogado = dados.IndAdvogado ? "S" : "N";
                escritorio.IndCivelEstrategico = dados.CivelEstrategico.GetValueOrDefault() ? "S" : "N";
                escritorio.IndAreaCivel = dados.IndAreaCivel ? "S" : "N";
                escritorio.IndAreaJuizado = dados.IndAreaJuizado ? "S" : "N";
                escritorio.IndAreaRegulatoria = dados.IndAreaRegulatoria ? "S" : "N";
                escritorio.IndAreaTrabalhista = dados.IndAreaTrabalhista ? "S" : "N";
                escritorio.IndAreaTributaria = dados.IndAreaTributaria ? "S" : "N";
                escritorio.IndCivelAdm = dados.IndAreaCivelAdministrativo ? "S" : "N";
                escritorio.IndCriminalJudicial = dados.IndAreaCriminalJudicial ? "S" : "N";
                escritorio.IndCriminalAdm = dados.IndAreaCriminalAdministrativo ? "S" : "N";
                escritorio.IndPex = dados.IndAreaPEX ? "S" : "N";
                escritorio.IndProcon = dados.IndAreaProcon ? "S" : "N";
                escritorio.IndContadorPex = dados.EhContadorPex.GetValueOrDefault() ? "S" : "N";
                escritorio.EnviarAppPreposto = dados.EnviarAppPreposto ? "S" : "N";

                // trata os estados do JEC
                {
                    var selecionadosJec = dados.selecionadosJec.Where(x => x.Selecionado == true).Select(x => x.Id).ToArray();
                    var naoSelecionadosJec = dados.selecionadosJec.Where(x => x.Selecionado == false).Select(x => x.Id).ToArray();

                    var remover = escritorio.EscritorioEstado.Where(x => x.CodTipoProcesso == 7).Select(x => x.CodEstado).Intersect(naoSelecionadosJec).ToArray();
                    escritorio.EscritorioEstado.RemoveAll(x => x.CodTipoProcesso == 7 && remover.Contains(x.CodEstado));

                    var incluir = selecionadosJec.Except(escritorio.EscritorioEstado.Where(x => x.CodTipoProcesso == 7).Select(x => x.CodEstado))
                        .Select(x => new EscritorioEstado { CodTipoProcesso = 7, CodProfissional = escritorio.CodProfissional, CodEstado = x }).ToArray();
                    await _db.EscritorioEstado.AddRangeAsync(incluir, ct);
                }

                // trata os estados do Cível Consumidor
                {
                    var selecionadosCivelConsumidor = dados.selecionadosCivelConsumidor.Where(x => x.Selecionado == true).Select(x => x.Id).ToArray();
                    var naoSelecionadosCivelConsumidor = dados.selecionadosCivelConsumidor.Where(x => x.Selecionado == false).Select(x => x.Id).ToArray();

                    var remover = escritorio.EscritorioEstado.Where(x => x.CodTipoProcesso == 1).Select(x => x.CodEstado).Intersect(naoSelecionadosCivelConsumidor).ToArray();
                    escritorio.EscritorioEstado.RemoveAll(x => x.CodTipoProcesso == 1 && remover.Contains(x.CodEstado));

                    var incluir = selecionadosCivelConsumidor.Except(escritorio.EscritorioEstado.Where(x => x.CodTipoProcesso == 1).Select(x => x.CodEstado))
                        .Select(x => new EscritorioEstado { CodTipoProcesso = 1, CodProfissional = escritorio.CodProfissional, CodEstado = x }).ToArray();
                    await _db.EscritorioEstado.AddRangeAsync(incluir, ct);
                }

                _logger.LogInformation(LogHelpers.SalvandoEntidade($"{dados.Id}, {dados.Nome}"));
                await _db.SaveChangesAsync(User.Identity!.Name, true, ct);

                _logger.LogInformation(LogHelpers.OperacaoFinalizada(commandName));
                return Result(CommandResult<AtualizarEscritorioRequest>.Valid(dados));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, LogHelpers.OperacaoComErro(commandName));
                return Result(CommandResult<AtualizarEscritorioRequest>.Invalid(ex.Message));
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> RemoverEscritorioAsync(CancellationToken ct, int id)
        {
            string entityName = "Escritorio";
            string commandName = $"Removendo {entityName}";
            _logger.LogInformation(LogHelpers.IniciandoOperacao(commandName));

            try
            {
                if (await _controleAcesso.TemPermissaoAsync(User.Identity!.Name, Permissoes.ACESSAR_ESCRITORIO) == false)
                {
                    _logger.LogInformation(LogHelpers.PermissaoNegada(Permissoes.ACESSAR_ESCRITORIO, User.Identity!.Name!));
                    return Result(CommandResult<PaginatedQueryResult<ObterPaginadoResponse>>.Forbidden());
                }

                // verifica se o escritório existe
                if (await _db.Profissional.AnyAsync(x => x.CodProfissional == id, ct) == false)
                {
                    _logger.LogInformation(LogHelpers.EntidadeNaoEncontrada(entityName, $"{id}"));
                    return Result(CommandResult.Invalid(LogHelpers.EntidadeNaoEncontrada(entityName, $"{id}")));
                }

                // verifica se o escritóro está sendo utilizado em alguma tabela
                {
                    string message = "Não é possível excluir esse escritório, pois ele está relacionado com";
                    string retorno = "";

                    if (await _db.ParamDistribEscritorio.AnyAsync(x => x.CodProfissional == id, ct))
                    {
                        retorno = "O escritório não poderá ser excluído, pois  o escritório está parametrizado em uma chave de distribuição automática de processos.<br/><br/>Menu > Workflow > Cadastro > Parametrizar Distribuição de Processos aos Escritórios";
                    }
                    else if (await _db.Processo.AnyAsync(x => x.CodProfissional == id || x.CodEscritorioAcompanhante == id, ct))
                    {
                        retorno = message + " Processos";
                    }
                    else if (await _db.AcaUsuarioEscritorio.AnyAsync(x => x.CodProfissional == id, ct))
                    {
                        retorno = message + " ACA_USUARIO_ESCRITORIO";
                    }
                    else if (await _db.PagamentoProcesso.AnyAsync(x => x.CodProfissional == id, ct))
                    {
                        retorno = message + " PAGAMENTO_PROCESSO";
                    }
                    else if (await _db.DespesaProfissional.AnyAsync(x => x.CodProfissional == id, ct))
                    {
                        retorno = message + " DESPESA_PROFISSIONAL";
                    }
                    else if (await _db.Fornecedor.AnyAsync(x => x.CodProfissional == id, ct))
                    {
                        retorno = message + " FORNECEDOR";
                    }
                    else if (await _db.AdvogadoAutorProcesso.AnyAsync(x => x.CodProfissional == id, ct))
                    {
                        retorno = message + " ADVOGADO_AUTOR_PROCESSO";
                    }
                    else if (await _db.AudienciaProcesso.AnyAsync(x => x.AdvesCodProfissional == id, ct))
                    {
                        retorno = message + " AUDIENCIA_PROCESSO";
                    }
                    else if (await _db.Protocolos.AnyAsync(x => x.CodProfissional == id, ct))
                    {
                        retorno = message + " PROTOCOLOS";
                    }
                    else if (await _db.AdvogadoEscritorio.AnyAsync(x => x.CodProfissional == id, ct))
                    {
                        retorno = message + " ADVOGADO_ESCRITORIO";
                    }

                    // se a variável de retorno não estiver vazia, é porque o escritório está sendo utilizado em alguma tabela
                    if (string.IsNullOrEmpty(retorno) == false)
                    {
                        _logger.LogInformation(retorno);
                        return Result(CommandResult.Invalid(retorno));
                    }
                }

                // exclui os estados de atuacao do escritório
                _logger.LogInformation(LogHelpers.RemovendoEntidade(entityName, $"{id}"));

                using var scope = await _db.Database.BeginTransactionAsync(ct);

                _db.ExecutarProcedureDeLog(User.Identity!.Name, true);

                await _db.EscritorioEstado.Where(x => x.CodProfissional == id).ExecuteDeleteAsync(ct);
                await _db.Profissional.Where(x => x.CodProfissional == id).ExecuteDeleteAsync(ct);

                await scope.CommitAsync(ct);

                _logger.LogInformation(LogHelpers.OperacaoFinalizada(commandName));
                return Result(CommandResult.Valid());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, LogHelpers.OperacaoComErro(commandName));
                return Result(CommandResult.Invalid(ex.Message));
            }
        }

        [HttpGet("exportar")]
        public async Task<IActionResult> ExportarEscritorioAsync(CancellationToken ct, string? estado, int areaAtuacao, string coluna = "nome", string direcao = "asc", string? pesquisa = null)
        {
            if (await _controleAcesso.TemPermissaoAsync(User.Identity!.Name, Permissoes.ACESSAR_ESCRITORIO) == false)
            {
                _logger.LogInformation(LogHelpers.PermissaoNegada(Permissoes.ACESSAR_ESCRITORIO, User.Identity!.Name!));
                return Result(CommandResult<PaginatedQueryResult<ObterPaginadoResponse>>.Forbidden());
            }

            string logName = "Escritório";
            _logger.LogInformation(LogHelpers.IniciandoOperacao($"Obter {logName}"));

            _logger.LogInformation(LogHelpers.Obtendo(logName));
            var resultado = ObterBase(estado, areaAtuacao, sort: EnumHelpers.ParseOrDefault(coluna, EscritorioSort.Id), ascending: direcao == "asc", pesquisa).ToArray();

            var csv = resultado.ToCsvByteArray(typeof(ObterPaginadoResponseMap), false);

            var bytes = Encoding.UTF8.GetPreamble().Concat(csv).ToArray();

            string nomeArquivo = $"Escritorios_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

            return File(bytes, "application/octet-stream", nomeArquivo);
        }

        [HttpGet("exportar-atuacao")]
        public async Task<IActionResult> ExportarAtuacaoEscritorioAsync(CancellationToken ct, string? estado, int areaAtuacao, string coluna = "nome", string direcao = "asc", string? pesquisa = null)
        {
            var area = AreaAtuacao(areaAtuacao);

            var query = _db.EscritorioEstado
                .AsNoTracking()
                .WhereIfNotNull(x => x.CodProfissionalNavigation.NomProfissional.Contains(pesquisa!), pesquisa)
                .WhereIfNotNull(x => x.CodProfissionalNavigation.CodEstado == estado, estado)
                .Where(a => a.CodProfissionalNavigation.IndEscritorio == "S" &&
                            (areaAtuacao <= 0 ||
                                  (area.juizado == "S" && a.CodProfissionalNavigation.IndAreaJuizado == area.juizado) ||
                                  (area.civel == "S" && a.CodProfissionalNavigation.IndAreaCivel == area.civel) ||
                                  (area.regulatorio == "S" && a.CodProfissionalNavigation.IndAreaRegulatoria == area.regulatorio) ||
                                  (area.tributario == "S" && a.CodProfissionalNavigation.IndAreaTributaria == area.tributario) ||
                                  (area.trabalhista == "S" && a.CodProfissionalNavigation.IndAreaTrabalhista == area.trabalhista) ||
                                  (area.procon == "S" && a.CodProfissionalNavigation.IndProcon == area.procon) ||
                                  (area.pex == "S" && a.CodProfissionalNavigation.IndPex == area.pex) ||
                                  (area.criminalAdm == "S" && a.CodProfissionalNavigation.IndCriminalAdm == area.criminalAdm) ||
                                  (area.criminalJudicial == "S" && a.CodProfissionalNavigation.IndCriminalJudicial == area.criminalJudicial) ||
                                  (area.civelEstrategico == "S" && a.CodProfissionalNavigation.IndCivelEstrategico == area.civelEstrategico) ||
                                  (area.civelAdm == "S" && a.CodProfissionalNavigation.IndCivelAdm == area.civelAdm)
                            ))
                .Select(x => new ExportarAtuacaoEscritorioResponse
                {
                    Nome = x.CodProfissionalNavigation.NomProfissional,
                    TipoPessoaValor = x.CodProfissionalNavigation.CodTipoPessoa,
                    CPF = x.CodProfissionalNavigation.CpfProfissional ?? x.CodProfissionalNavigation.CgcProfissional,
                    CNPJ = x.CodProfissionalNavigation.CgcProfissional,
                    Ativo = x.CodProfissionalNavigation.IndAtivo == "S" ? true : false,
                    CodTipoProcesso = x.CodTipoProcesso,
                    CodEstadoCivelConsumidor = x.CodTipoProcesso == 1 ? x.CodEstado : "",
                    CodEstadoJuizado = x.CodTipoProcesso == 7 ? x.CodEstado : "",
                    CodEstado = x.CodEstado,
                });

            var sort = EnumHelpers.ParseOrDefault(coluna, EscritorioSort.Id);
            var ascending = direcao == "asc";

            switch (sort)
            {
                case EscritorioSort.Id:
                    query = query.SortBy(a => a.TipoPessoaValor, ascending).ThenBy(x => x.CodEstado);
                    break;

                case EscritorioSort.Nome:
                    query = query.SortBy(a => a.Nome, ascending).ThenBy(x => x.CodEstado);
                    break;

                case EscritorioSort.CPFCPNJ:
                    query = query.SortBy(a => a.CPF, ascending).ThenBy(x => x.CodEstado);
                    break;

                case EscritorioSort.Ativo:
                    query = query.SortBy(a => a.Ativo, ascending).ThenBy(x => x.CodEstado);
                    break;

                case EscritorioSort.TipoPessoa:
                    query = query.SortBy(a => a.TipoPessoaValor, ascending).ThenBy(x => x.CodEstado);
                    break;
            }

            var resultado = await query.ToArrayAsync(ct);

            string nomeArquivo = $"Estados_de_Atuacao_do_Escritorio_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

            var csv = resultado.ToCsvByteArray(typeof(ExportarAtuacaoEscritorioResponseMap), false);

            var bytes = Encoding.UTF8.GetPreamble().Concat(csv).ToArray();

            return File(bytes, "application/octet-stream", nomeArquivo);
        }

        [HttpGet("exportar-advogado")]
        public async Task<IActionResult> ExportarAdvogadoEscritorioAsync(CancellationToken ct, string? estado, int areaAtuacao, string coluna = "nome", string direcao = "asc", string pesquisa = null)
        {
            var area = AreaAtuacao(areaAtuacao);

            var query = from ee in _db.AdvogadoEscritorio
                        where ee.CodProfissionalNavigation.IndEscritorio == "S" && (areaAtuacao <= 0 ||
                                 (area.juizado == "S" && ee.CodProfissionalNavigation.IndAreaJuizado == area.juizado) ||
                                 (area.civel == "S" && ee.CodProfissionalNavigation.IndAreaCivel == area.civel) ||
                                 (area.regulatorio == "S" && ee.CodProfissionalNavigation.IndAreaRegulatoria == area.regulatorio) ||
                                 (area.tributario == "S" && ee.CodProfissionalNavigation.IndAreaTributaria == area.tributario) ||
                                 (area.trabalhista == "S" && ee.CodProfissionalNavigation.IndAreaTrabalhista == area.trabalhista) ||
                                 (area.procon == "S" && ee.CodProfissionalNavigation.IndProcon == area.procon) ||
                                 (area.pex == "S" && ee.CodProfissionalNavigation.IndPex == area.pex) ||
                                 (area.criminalAdm == "S" && ee.CodProfissionalNavigation.IndCriminalAdm == area.criminalAdm) ||
                                 (area.criminalJudicial == "S" && ee.CodProfissionalNavigation.IndCriminalJudicial == area.criminalJudicial) ||
                                 (area.civelAdm == "S" && ee.CodProfissionalNavigation.IndCivelAdm == area.civelAdm) ||
                                 (area.civelEstrategico == "S" && ee.CodProfissionalNavigation.IndCivelEstrategico == area.civelEstrategico)
                                 )
                        select new ExportarAdvogadoEscritorioResponse
                        {
                            NomeEscritorio = ee.CodProfissionalNavigation.NomProfissional,
                            CPF = ee.CodProfissionalNavigation.CpfProfissional ?? ee.CodProfissionalNavigation.CgcProfissional,
                            CNPJ = ee.CodProfissionalNavigation.CgcProfissional,
                            EstadoId = ee.CodEstado,
                            NumeroOAB = ee.NroOabAdvogado,
                            NomeAdvogado = ee.NomAdvogado,
                            CelularDDD = ee.NroDddCelular,
                            Celular = ee.NroCelular,
                            EhContato = ee.IndContatoEscritorio,
                            TipoPessoaValor = ee.CodProfissionalNavigation.CodTipoPessoa,
                            Ativo = ee.CodProfissionalNavigation.IndAtivo == "S" ? true : false,
                        };

            var sort = EnumHelpers.ParseOrDefault(coluna, EscritorioSort.Id);
            var ascending = direcao == "asc";

            switch (sort)
            {
                case EscritorioSort.Id:
                    query = query.SortBy(a => a.TipoPessoaValor, ascending).ThenBy(x => x.NomeAdvogado);
                    break;

                case EscritorioSort.Nome:
                    query = query.SortBy(a => a.NomeEscritorio, ascending).ThenBy(x => x.NomeAdvogado);
                    break;

                case EscritorioSort.CPFCPNJ:
                    query = query.SortBy(a => a.CPF, ascending).ThenBy(x => x.NomeAdvogado);
                    break;

                case EscritorioSort.Ativo:
                    query = query.SortBy(a => a.Ativo, ascending).ThenBy(x => x.NomeAdvogado);
                    break;

                case EscritorioSort.TipoPessoa:
                    query = query.SortBy(a => a.TipoPessoaValor, ascending).ThenBy(x => x.NomeAdvogado);
                    break;
            }

            query = query.WhereIfNotNull(x => x.NomeEscritorio.Contains(pesquisa!), pesquisa);
            query = query.WhereIfNotNull(x => x.EstadoId == estado, estado);

            var resultado = await query.ToArrayAsync(ct);

            string nomeArquivo = $"Advogados_do_Escritorio_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            var csv = resultado.ToCsvByteArray(typeof(ExportarAdvogadoEscritorioResponseMap), false);

            var bytes = Encoding.UTF8.GetPreamble().Concat(csv).ToArray();

            return File(bytes, "application/octet-stream", nomeArquivo);
        }

        private static (string civel, string juizado, string regulatorio, string trabalhista, string tributario, string civelAdm, string civelEstrategico,
            string criminalAdm, string criminalJudicial, string pex, string procon) AreaAtuacao(int areaAtuacao)
        {
            var civel = areaAtuacao == (int)TipoProcessoEnum.CivelConsumidor ? "S" : "N";
            var juizado = areaAtuacao == (int)TipoProcessoEnum.JuizadoEspecial ? "S" : "N";
            var regulatorio = areaAtuacao == (int)TipoProcessoEnum.Administrativo ? "S" : "N";
            var trabalhista = areaAtuacao == (int)TipoProcessoEnum.Trabalhista ? "S" : "N";
            var tributario = areaAtuacao == (int)TipoProcessoEnum.TributarioAdministrativo ? "S" : "N";
            var civelAdm = areaAtuacao == (int)TipoProcessoEnum.CivelAdministrativo ? "S" : "N";
            var civelEstrategico = areaAtuacao == (int)TipoProcessoEnum.CivelEstrategico ? "S" : "N";
            var criminalAdm = areaAtuacao == (int)TipoProcessoEnum.CriminalAdministrativo ? "S" : "N";
            var criminalJudicial = areaAtuacao == (int)TipoProcessoEnum.CriminalJudicial ? "S" : "N";
            var pex = areaAtuacao == (int)TipoProcessoEnum.Pex ? "S" : "N";
            var procon = areaAtuacao == (int)TipoProcessoEnum.Procon ? "S" : "N";

            return (civel, juizado, regulatorio, trabalhista, tributario, civelAdm, civelEstrategico, criminalAdm, criminalJudicial, pex, procon);
        }

        [HttpGet("escritorio-estado")]
        public async Task<IActionResult> ObterEscritoriosEstadosAsync(CancellationToken ct, int escritorioId, int tipoProcessoId)
        {
            string logName = "Escritório Estado";
            _logger.LogInformation(LogHelpers.IniciandoOperacao($"Obter {logName}"));

            if (await _controleAcesso.TemPermissaoAsync(User.Identity!.Name, Permissoes.ACESSAR_ESCRITORIO) == false)
            {
                _logger.LogInformation(LogHelpers.PermissaoNegada(Permissoes.ACESSAR_ESCRITORIO, User.Identity!.Name!));
                return Result(CommandResult<IReadOnlyCollection<ObterEscritoriosEstadosResponse>>.Forbidden());
            }

            _logger.LogInformation(LogHelpers.Obtendo(logName));

            var query = from a in _db.EscritorioEstado.AsNoTracking()
                        where (tipoProcessoId <= 0 || a.CodTipoProcesso == tipoProcessoId) && (a.CodProfissional == escritorioId)
                        select new ObterEscritoriosEstadosResponse(a.CodEstado, true, a.CodTipoProcesso);

            var resultado = await query.ToArrayAsync(ct);

            _logger.LogInformation(LogHelpers.Retornando(logName));
            return Result(CommandResult<IReadOnlyCollection<ObterEscritoriosEstadosResponse>>.Valid(resultado));
        }

        [HttpPost("validar-escritorio")]
        public async Task<IActionResult> ValidaEscritorioParametrizado(CancellationToken ct, int id)
        {
            try
            {
                var escritorio = await _distribuicao.ParamDistribEscritorio.AsNoTracking()
                                                    .Where(x => x.CodProfissional == id)
                                                    .Select(s => s.CodParamDistribuicaoNavigation.CodTipoProcesso)
                                                    .Distinct()
                                                    .ToListAsync(ct);

                return Ok(new
                {
                    parametrizado = escritorio.Count > 0,
                    naturezas = escritorio
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private IQueryable<ObterPaginadoResponse> ObterBase(string? estado, int areaAtuacao, EscritorioSort sort, bool ascending, string? pesquisa)
        {
            string logName = "Escritório";
            _logger.LogInformation(LogHelpers.IniciandoOperacao($"Ordenar {logName}"));

            var area = AreaAtuacao(areaAtuacao);

            var query = (from a in _db.Profissional
                         where a.IndEscritorio == "S" && (areaAtuacao <= 0 ||
                                  (area.juizado == "S" && a.IndAreaJuizado == area.juizado) ||
                                  (area.civel == "S" && a.IndAreaCivel == area.civel) ||
                                  (area.regulatorio == "S" && a.IndAreaRegulatoria == area.regulatorio) ||
                                  (area.tributario == "S" && a.IndAreaTributaria == area.tributario) ||
                                  (area.trabalhista == "S" && a.IndAreaTrabalhista == area.trabalhista) ||
                                  (area.procon == "S" && a.IndProcon == area.procon) ||
                                  (area.pex == "S" && a.IndPex == area.pex) ||
                                  (area.criminalAdm == "S" && a.IndCriminalAdm == area.criminalAdm) ||
                                  (area.criminalJudicial == "S" && a.IndCriminalJudicial == area.criminalJudicial) ||
                                  (area.civelEstrategico == "S" && a.IndCivelEstrategico == area.civelEstrategico) ||
                                  (area.civelAdm == "S" && a.IndCivelAdm == area.civelAdm)
                               )
                         select new ObterPaginadoResponse
                         {
                             Id = a.CodProfissional,
                             Nome = a.NomProfissional,
                             TipoPessoaValor = a.CodTipoPessoa,
                             CPF = a.CpfProfissional ?? a.CgcProfissional,
                             CNPJ = a.CgcProfissional,
                             Ativo = a.IndAtivo == "S",
                             EstadoId = a.CodEstado,
                             IndAreaRegulatoria = a.IndAreaRegulatoria == "S",
                             IndAreaCivelAdministrativo = a.IndCivelAdm == "S",
                             IndAreaCivel = a.IndAreaCivel == "S",
                             CivelEstrategico = a.IndCivelEstrategico == "S",
                             IndAreaCriminalAdministrativo = a.IndCriminalAdm == "S",
                             IndAreaCriminalJudicial = a.IndCriminalJudicial == "S",
                             IndAreaJuizado = a.IndAreaJuizado == "S",
                             IndAreaPEX = a.IndPex == "S",
                             IndAreaProcon = a.IndProcon == "S",
                             IndAreaTrabalhista = a.IndAreaTrabalhista == "S",
                             IndAreaTributaria = a.IndAreaTributaria == "S",
                             Endereco = a.EndProfissional,
                             Cidade = a.DscCidade,
                             Bairro = a.DscBairro,
                             Cep = a.CodCep,
                             Telefone = a.Telefone,
                             TelefoneDDD = a.DddTelefone,
                             Email = a.DscEmail,
                             Site = a.DscSite,
                             Fax = a.Fax,
                             FaxDDD = a.DddFax,
                             Celular = a.Celular,
                             CelularDDD = a.DddCelular,
                             AlertaEm = a.AlertaEm,
                             CodProfissionalSAP = a.CodProfissionalSap,
                             GljCodGrupoLoteJuizado = a.GljCodGrupoLoteJuizado,
                             enviarAppPreposto=a.EnviarAppPreposto=="S"
                         }).AsNoTracking();

            switch (sort)
            {
                case EscritorioSort.Id:
                    query = query.SortBy(a => a.TipoPessoaValor, ascending);
                    break;

                case EscritorioSort.Nome:
                    query = query.SortBy(a => a.Nome, ascending);
                    break;

                case EscritorioSort.CPFCPNJ:
                    query = query.SortBy(a => a.CPF, ascending);
                    break;

                case EscritorioSort.Ativo:
                    query = query.SortBy(a => a.Ativo, ascending);
                    break;

                case EscritorioSort.TipoPessoa:
                    query = query.SortBy(a => a.TipoPessoaValor, ascending);
                    break;
            }

            query = query.WhereIfNotNull(x => x.Nome.Contains(pesquisa!), pesquisa);
            query = query.WhereIfNotNull(x => x.EstadoId == estado, estado);

            return query;
        }

    }
}
