using Oi.Juridico.Contextos.V2.ESocialContext.Entities;
using Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.DTOs;
using Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.RequestDTOs;
using Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.Services;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1.DTOs;
using Oi.Juridico.WebApi.V2.Attributes;
using Perlink.Oi.Juridico.Infra.Constants;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Cadastro.Controllers
{

    [Route("api/esocial/formulario/ESocialF2501")]
    [ApiController]
    public class ESocialF2501InfoDepController : ControllerBase
    {
        private const int QuantidadePorPagina = 10;

        #region Consulta

        [HttpGet("consulta/infodep/{codigoFormulario}/{codigoInfodep}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult<EsF2501InfoDepDTO>> RetornaInfoDepF2501Async([FromRoute] int codigoFormulario,
                                                                                    [FromRoute] long codigoInfodep,
                                                                                    [FromServices] ESocialF2501InfoDepService service,
                                                                                    CancellationToken ct)
        {
            try
            {
                var infoDep = await service.RetornaInfoDepPorIdAsync(codigoInfodep, ct);
                if (infoDep is not null)
                {
                    EsF2501InfoDepDTO infoDepDTO = service.PreencheInfoDepDTO(ref infoDep);

                    return Ok(infoDepDTO);
                }

                return BadRequest($"Formulário não encontrado para o id: {codigoFormulario} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao recuperar dados do dependente.", erro = e.Message });
            }
        }

        #endregion Consulta

        #region Lista paginado

        [HttpGet("lista/infodep/{codigoFormulario}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult<RetornoPaginadoDTO<EsF2501InfoDepDTO>>> ListaInfoDepF2501Async([FromRoute] int codigoFormulario,
                                                                                                      [FromQuery] int pagina,
                                                                                                      [FromQuery] string coluna,
                                                                                                      [FromQuery] bool ascendente,
                                                                                                      [FromServices] ESocialF2501InfoDepService service,
                                                                                                      [FromServices] ESocialF2501Service service2501,
                                                                                                      CancellationToken ct)
        {
            try
            {
                var existeFormulario = await service2501.ExisteFormularioPorIdAsync(codigoFormulario, ct);
                if (existeFormulario)
                {
                    IQueryable<EsF2501InfoDepDTO> listaInfoDep = service.RecuperaListaInfoDep(codigoFormulario);

                    switch (coluna.ToLower())
                    {
                        case "cpf":
                            listaInfoDep = ascendente ? listaInfoDep.OrderBy(x => x.InfodepCpfdep) : listaInfoDep.OrderByDescending(x => x.InfodepCpfdep);
                            break;
                        case "nome":
                            listaInfoDep = ascendente ? listaInfoDep.OrderBy(x => x.InfodepNome == null ? 0 : 1) : listaInfoDep.OrderByDescending(x => x.InfodepNome == null ? 0 : 1);
                            break;
                        default:
                            listaInfoDep = ascendente ? listaInfoDep.OrderBy(x => x.IdEsF2501Infodep) : listaInfoDep.OrderByDescending(x => x.IdEsF2501Infodep);
                            break;
                    }

                    var total = await listaInfoDep.CountAsync(ct);

                    var skip = Pagination.PagesToSkip(QuantidadePorPagina, total, pagina);

                    var resultado = new RetornoPaginadoDTO<EsF2501InfoDepDTO>
                    {
                        Total = total,
                        Lista = await listaInfoDep.Skip(skip).Take(QuantidadePorPagina).ToListAsync(ct)
                    };

                    return Ok(resultado);
                }

                return BadRequest($"Formulário não encontrado para o id: {codigoFormulario} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao recuperar lista dos Dependenstes.", erro = e.Message });
            }
        }

        #endregion Lista paginado

        #region Alteração

        [HttpPut("alteracao/infodep/{codigoFormulario}/{codigoInfodep}")]
        [TemPermissao(Permissoes.ESOCIAL_BLOCO_G_2501)]
        public async Task<ActionResult> AlteraInfoDepF2501Async([FromRoute] int codigoFormulario,
                                                                [FromRoute] int codigoInfodep,
                                                                [FromBody] EsF2501InfoDepRequestDTO requestDTO,
                                                                [FromServices] ESocialF2501InfoDepService service,
                                                                [FromServices] ESocialF2501Service service2501,
                                                                [FromServices] DBContextService serviceDbContext,
                                                                CancellationToken ct)
        {
            try
            {
                var (infoDepInvalido, listaErrosDTO) = requestDTO.Validar();

                var formulario = await service2501.RetornaFormularioPorIdAsync(codigoFormulario, ct);

                if (formulario != null)
                {

                    if (!await service2501.FormularioPodeSerSalvoPorIdAsync(codigoFormulario, ct))
                    {
                        return BadRequest("O formulário deve estar com status 'Rascunho' para alterar uma informação de dependente");
                    }

                    var infoDep = await service.RetornaInfoDepEditavelPorIdAsync(codigoInfodep, ct);

                    if (infoDep is not null)
                    {
                        service.PreencheInfoDep(ref infoDep, requestDTO, User);
                    }
                    else
                    {
                        return BadRequest("Registro não encontrado. Alteração não efetuada.");
                    }

                    if (!await service2501.FormularioPodeSerSalvoPorIdAsync(codigoFormulario, ct))
                    {
                        return BadRequest("O formulário deve estar com status 'Rascunho' para alterar uma informação de dependente.");
                    }

                    var listaErros = listaErrosDTO.ToList();
                    listaErros.AddRange(service.ValidaAlteracaoInfoDep(codigoInfodep, requestDTO, formulario).ToList());

                    infoDepInvalido = listaErros.Count > 0;

                    if (infoDepInvalido)
                    {
                        return BadRequest(listaErros);
                    }

                    await serviceDbContext.SalvaAlteracoesAsync(ct);

                    return Ok("Registro alterado com sucesso.");
                }

                return BadRequest("O formulário informado não foi encontrado.");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao alterar informações de dependentes.", erro = e.Message });
            }
        }

        #endregion Alteração

        #region Inclusão

        [HttpPost("inclusao/infodep/{codigoFormulario}")]
        [TemPermissao(Permissoes.ESOCIAL_BLOCO_G_2501)]
        public async Task<ActionResult> CadastraCalcribF2501Async([FromRoute] int codigoFormulario,
                                                                  [FromBody] EsF2501InfoDepRequestDTO requestDTO,
                                                                  [FromServices] ESocialF2501InfoDepService service,
                                                                  [FromServices] ESocialF2501Service service2501,
                                                                  [FromServices] DBContextService serviceDbContext,
                                                                  CancellationToken ct)
        {
            try
            {
                var (infoDepInvalido, listaErrosDTO) = requestDTO.Validar();

                var formulario = await service2501.RetornaFormularioPorIdAsync(codigoFormulario, ct);

                if (formulario != null)
                {
                    if (!await service2501.FormularioPodeSerSalvoPorIdAsync(codigoFormulario, ct))
                    {
                        return BadRequest("O formulário deve estar com status 'Rascunho' para incluir uma informação de dependente");
                    }

                    if (await service.QuantidadeMaximaDeInfoDepExcedida(999, codigoFormulario, ct))
                    {
                        return BadRequest("O sistema só permite a inclusão de até 999 uma informação de dependente.");
                    };

                    var infoDep = new EsF2501Infodep();

                    service.PreencheInfoDep(ref infoDep, requestDTO, User, codigoFormulario);

                    service.AdicionaInfoDepAoContexto(ref infoDep);

                    if (!await service2501.FormularioPodeSerSalvoPorIdAsync(codigoFormulario, ct))
                    {
                        return BadRequest("O formulário deve estar com status 'Rascunho' para incluir uma informação de dependente");
                    }

                    var listaErros = listaErrosDTO.ToList();
                    listaErros.AddRange(service.ValidaInclusaoInfoDep(requestDTO, formulario).ToList());

                    infoDepInvalido = listaErros.Count > 0;

                    if (infoDepInvalido)
                    {
                        return BadRequest(listaErros);
                    }

                    await serviceDbContext.SalvaAlteracoesAsync(ct);

                    return Ok("Registro incluído com sucesso.");
                }

                return BadRequest("O formulário informado não foi encontrado.");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao cadastrar uma informação de dependente.", erro = e.Message });
            }
        }

        #endregion Inclusão

        #region ARQUIVOS

        //[HttpPost("upload/infodep/{codF2501}/{opcaoCarga}")]
        //[Consumes("multipart/form-data")]
        //public async Task<IActionResult> IncluirAnexoAsync([FromRoute] int codF2501, [FromRoute] int opcaoCarga, [FromForm] IFormFile arquivo, CancellationToken ct)
        //{
        //    using var scope = await _eSocialDbContext.Database.BeginTransactionAsync(ct);

        //    try
        //    {
        //        if (arquivo is null) return BadRequest("Nenhum arquivo para anexar.");

        //        double limiteArquivoEmMB = 10;

        //        if (FileHelper.ExtensaoArquivoInvalida(arquivo.FileName, ".csv"))
        //        {
        //            return BadRequest("Extensão do arquivo inválida. Deve ser um arquivo \".csv\".");
        //        }

        //        using (var reader = new StreamReader(arquivo.OpenReadStream()))
        //        {
        //            var configuration = new CsvConfiguration(CultureInfo.GetCultureInfo("pt-BR"))
        //            {
        //                Delimiter = ";",
        //                TrimOptions = TrimOptions.Trim,
        //                HasHeaderRecord = true,
        //                IgnoreBlankLines = false,
        //                MissingFieldFound = null
        //            };

        //            using (var csv = new CsvReader(reader, configuration))
        //            {
        //                csv.Context.RegisterClassMap(new ImportarPlanilhaCalcTribMap());

        //                var linhas = csv.GetRecords<EsF2501InfoDepRequestDTO>().ToList();

        //                var colunas = csv.HeaderRecord.Length;

        //                double tamanhoArquivo = reader.BaseStream.Length / 1024 / 1024;

        //                if (colunas > 3 || colunas < 3)
        //                {
        //                    return BadRequest("O arquivo não contém a quantidade de colunas esperadas.");
        //                }

        //                if (linhas.Count == 0)
        //                {
        //                    return BadRequest("Arquivo não contém dados para importação.");
        //                }

        //                if (linhas.Count > 361)
        //                {
        //                    return BadRequest("Quantidade de linhas no arquivo excedida. Quantidade máxima 360 linhas.");
        //                }

        //                if (tamanhoArquivo > limiteArquivoEmMB)
        //                {
        //                    return BadRequest($"O arquivo '{arquivo.FileName}' excede o tamanho permitido: {limiteArquivoEmMB}MB");
        //                }

        //                var formulario = await _eSocialDbContext.EsF2501.AsNoTracking().FirstOrDefaultAsync(x => x.IdF2501 == codF2501, ct);

        //                if (formulario is not null)
        //                {
        //                    if (formulario!.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte())
        //                    {
        //                        return BadRequest("O formulário deve estar com status 'Rascunho' para alterar um perído de base de cálculo.");
        //                    }

        //                    if (opcaoCarga == ESocialTipoCarga.ApagarRegistros.ToByte())
        //                    {
        //                        await ApagaListaCalcTrib(codF2501, ct);
        //                    }

        //                    int contadorLinhas = 1;
        //                    var linhasComErro = new List<int>();

        //                    foreach (var linha in linhas)
        //                    {
        //                        if (linha.CalctribPerref == new DateTime(1901, 01, 01) && linha.CalctribVrbccpmensal == null)
        //                        {
        //                            contadorLinhas++;
        //                            continue;
        //                        }

        //                        var (linhaInvalida, listaErros) = linha.Validar();

        //                        contadorLinhas++;

        //                        if (!linhaInvalida)
        //                        {

        //                            var listaErrosTemp = listaErros.ToList();

        //                            var calcTribUpdate = await _eSocialDbContext.EsF2501Infodep.FirstOrDefaultAsync(x => x.IdEsF2501 == codF2501 && x.CalctribPerref == linha.CalctribPerref.ToString("yyyy-MM"), ct);

        //                            if (calcTribUpdate != null)
        //                            {
        //                                PreencheCalctrib(ref calcTribUpdate, linha);
        //                            }
        //                            else
        //                            {
        //                                var calcTrib = new EsF2501Infodep();
        //                                PreencheCalctrib(ref calcTrib, linha, codF2501);
        //                                _eSocialDbContext.Add(calcTrib);
        //                            }
        //                        }
        //                        linhaInvalida = listaErros.Any();

        //                        if (linhaInvalida)
        //                        {
        //                            linhasComErro.Add(contadorLinhas);
        //                            continue;
        //                        }

        //                        await _eSocialDbContext.SaveChangesExternalScopeAsync(User.Identity!.Name, true, ct);

        //                    }

        //                    if (linhasComErro.Any())
        //                    {
        //                        scope.Rollback();
        //                        return BadRequest(linhasComErro);
        //                    }

        //                    if (await _eSocialDbContext.EsF2501Infodep.Where(x => x.IdEsF2501 == codF2501).CountAsync(ct) > 360)
        //                    {
        //                        scope.Rollback();
        //                        return BadRequest("O sistema só permite a inclusão de até 360 Bases de Cálculos.");
        //                    };

        //                    scope.Commit();
        //                    return Ok();
        //                }

        //                scope.Rollback();
        //                return BadRequest($"Formulário não encontrado para o id: {codF2501} ");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex.Message.Contains("The conversion cannot be performed") || ex.Message.Contains("An unexpected error occurred"))
        //        {
        //            var indiceInicial = ex.Message.IndexOf("Row: ") + 5;
        //            var indiceFinal = ex.Message.IndexOf("\r", indiceInicial);
        //            var tamanhoLinha = indiceFinal - indiceInicial;
        //            var linha = ex.Message.Substring(indiceInicial, tamanhoLinha);
        //            scope.Rollback();
        //            return BadRequest($"O arquivo importado contem dados que não podem ser lidos na linha: {linha}. Corrija e verifique se existem outras linhas com o mesmo problema.");
        //        }

        //        scope.Rollback();
        //        return BadRequest("Não foi possível incluir o anexo");
        //    }
        //}

        //[HttpGet("download/infodep/{codigoFormulario}")]
        //public async Task<ActionResult> ExportarCsvLista([FromRoute] int codigoFormulario, [FromQuery] bool ascendente, CancellationToken ct)
        //{


        //    var formulario2501 = await _eSocialDbContext.EsF2501.AsNoTracking().FirstOrDefaultAsync(x => x.IdF2501 == codigoFormulario, ct);
        //    if (formulario2501 is not null)
        //    {
        //        IQueryable<EsF2501InfoDepDTO> listaCalcTrib = RecuperaListaInfoDep(codigoFormulario);

        //        var arquivo = GerarCSV(listaCalcTrib.ToList(), ascendente);
        //        return arquivo;
        //    }

        //    return BadRequest($"Formulário não encontrado para o id: {codigoFormulario} ");
        //}

        #endregion ARQUIVOS

        #region Exclusão

        [HttpDelete("exclusao/infodep/{codigoFormulario}/{codigoInfodep}")]
        [TemPermissao(Permissoes.ESOCIAL_BLOCO_G_2501)]
        public async Task<ActionResult> ExcluiCalctribF2501Async([FromRoute] int codigoFormulario,
                                                                 [FromRoute] long codigoInfodep,
                                                                 [FromServices] ESocialF2501InfoDepService service,
                                                                 [FromServices] ESocialF2501Service service2501,
                                                                 [FromServices] DBContextService serviceDbContext,
                                                                 CancellationToken ct)
        {
            try
            {
                var existeFormulario = await service2501.ExisteFormularioPorIdAsync(codigoFormulario, ct);

                if (existeFormulario)
                {
                    if (!await service2501.FormularioPodeSerSalvoPorIdAsync(codigoFormulario, ct))
                    {
                        return BadRequest("O formulário deve estar com status 'Rascunho' para excluir uma informação de dependente.");
                    }

                    var infoDep = await service.RetornaInfoDepEditavelPorIdAsync(codigoInfodep, ct);

                    if (infoDep is not null)
                    {
                        await service.RemoveInfoDep(codigoInfodep, ct);
                    }
                    else
                    {
                        return BadRequest("Registro não encontrado. Exclusão não efetuada.");
                    }

                    if (!await service2501.FormularioPodeSerSalvoPorIdAsync(codigoFormulario, ct))
                    {
                        return BadRequest("O formulário deve estar com status 'Rascunho' para excluir uma informação de dependente");
                    }

                    await serviceDbContext.SalvaAlteracoesRegistraLogAsync(User!.Identity!.Name!, ct);

                    return Ok("Registro excluído com sucesso.");
                }

                return BadRequest("O formulário informado não foi encontrado.");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao excluir uma informação de dependente.", erro = e.Message });
            }
        }

        #endregion Exclusão
    }

}