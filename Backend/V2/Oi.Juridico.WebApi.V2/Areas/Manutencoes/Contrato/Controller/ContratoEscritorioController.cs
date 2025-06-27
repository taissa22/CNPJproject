using Oi.Juridico.AddOn.Extensions.IEnumerable;
using Oi.Juridico.Contextos.V2.ManutencaoContratoEscritorioContext.Data;
using Oi.Juridico.Contextos.V2.ManutencaoContratoEscritorioContext.Entities;
using Oi.Juridico.WebApi.V2.Areas.DistribuicaoProcessoEscritorio.DTOs;
using Oi.Juridico.WebApi.V2.Areas.Manutencoes.Contrato.CsvHelperMap;
using Oi.Juridico.WebApi.V2.Areas.Manutencoes.Contrato.DTOs;
using Perlink.Oi.Juridico.Infra.ValueObjects;
using Shared.Tools;
using System.Globalization;
using System.IO.Compression;
using System.IO;
using ObterEscritorioResponse = Oi.Juridico.WebApi.V2.Areas.Manutencoes.Contrato.DTOs.ObterEscritorioResponse;
using SixLabors.ImageSharp.ColorSpaces;

namespace Oi.Juridico.WebApi.V2.Areas.Manutencoes.Contrato.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContratoEscritorioController : ControllerBase
    {
        private readonly ManutencaoContratoEscritorioDbContext _db;
        private readonly ILogger<ContratoEscritorioController> _logger;

        public ContratoEscritorioController(ManutencaoContratoEscritorioDbContext db, ILogger<ContratoEscritorioController> logger)
        {
            _db = db;
            _logger = logger;
        }

        #region COMBOS

        [HttpGet("obter/tipo-contrato")]
        public async Task<IActionResult> ObterTipoContratoAsync(CancellationToken ct)
        {
            try
            {
                var queryResult = await _db.TipoContratoEscritorio.AsNoTracking().Select(x => new
                {
                    Codigo = x.CodTipoContratoEscritorio,
                    Descricao = x.DscTipoContrato
                }).ToListAsync(ct);
                return Ok(queryResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("obter/estados")]
        public async Task<IActionResult> ObterEstadosAsync(CancellationToken ct)
        {
            try
            {
                var queryResult = await _db.Estado.AsNoTracking().Select(x => new
                {
                    Codigo = x.CodEstado,
                    Descricao = x.NomEstado
                }).OrderBy(x => x.Codigo).ToListAsync(ct);
                return Ok(queryResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("obter/escritorio")]
        public async Task<IActionResult> ObterEscritorioAsync(CancellationToken ct)
        {
            try
            {
                var queryResult = await _db.Profissional
                .AsNoTracking()
                .Where(x => x.IndEscritorio == "S" && x.IndAtivo == "S")
                .OrderBy(x => x.NomProfissional)
                .Select(x => new
                {
                    Codigo = x.CodProfissional,
                    Descricao = x.NomProfissional
                })
                .ToListAsync(ct);

                return Ok(queryResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("obter/atuacao")]
        public async Task<IActionResult> ObterAtuacaoAsync(CancellationToken ct)
        {
            try
            {
                var queryResult = await _db.ContratoAtuacao.AsNoTracking().Select(x => new
                {
                    Codigo = x.CodContratoAtuacao,
                    Descricao = x.DscContratoAtuacao
                }).OrderBy(x => x.Codigo).ToListAsync(ct);
                return Ok(queryResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("obter/diretoria")]
        public async Task<IActionResult> ObterDiretoriaAsync(CancellationToken ct)
        {
            try
            {
                var queryResult = await _db.ContratoDiretoria.AsNoTracking().Select(x => new
                {
                    Codigo = x.CodContratoDiretoria,
                    Descricao = x.DscContratoDiretoria,
                    Indicador = x.IndDiretoriaPadraoContrato
                }).OrderBy(x => x.Codigo).ToListAsync(ct);
                return Ok(queryResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region METHODS

        [HttpGet("obter/lista-contratos")]
        public async Task<IActionResult> ObterListaContratosAsync(CancellationToken ct, [FromQuery] BuscaContratoEscritorioRequest contratoDto, [FromQuery] string? ordem, [FromQuery] bool asc = true, [FromQuery] int page = 0, [FromQuery] int size = 8)
        {
            try
            {
                var query = QueryContrato(contratoDto, ordem, asc);
                var lista = await query.Skip((page - 1) * size).Take(size).ToArrayAsync(ct);
                var total = await query.CountAsync(ct);

                return Ok(new { lista, total });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("obter-contrato")]
        public async Task<IActionResult> ObterContratoAsync(CancellationToken ct, [FromQuery] int codContrato)
        {
            try
            {
                var query = QueryObterContrato(codContrato);
                var contrato = await query.ToArrayAsync(ct);
                return Ok(contrato);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("salvar-contrato")]
        public async Task<IActionResult> SalvarContratoAsync(CancellationToken ct, [FromBody] ContratoEscritorioRequest dto)
        {
            try
            {

                if (_db.ContratoEscritorio.Any(x => x.NumContratoJecVc == dto.NumContratoJecVc && dto.NumContratoJecVc != null))
                    return BadRequest($"O contrato não poderá ser cadastrado. Já existe um contrato cadastrado com esse número de contrato JEC/CC.");

                if (_db.ContratoEscritorio.Any(x => x.NumContratoProcon == dto.NumContratoProcon && dto.NumContratoProcon != null))
                    return BadRequest($"O contrato não poderá ser cadastrado. Já existe um contrato cadastrado com esse número de contrato PROCON.");

                var contrato = new ContratoEscritorio();
                var contratoEscritorioEstado = new ContratoEscritorioEstado();

                contrato.CodTipoContratoEscritorio = dto.CodTipoContratoEscritorio;
                contrato.IndAtivo = dto.IndAtivo;
                contrato.IndConsideraCalculoVep = dto.IndConsideraCalculoVep;
                contrato.NumContratoJecVc = dto.NumContratoJecVc;
                contrato.NumContratoProcon = dto.NumContratoProcon;
                contrato.NomContrato = dto.NomContrato;
                contrato.NumSgpagJecVc = dto.NumSgpagJecVc;
                contrato.NumSgpagProcon = dto.NumSgpagProcon;
                contrato.Cnpj = dto.Cnpj;
                contrato.ValVep = dto.ValVep;
                contrato.ValUnitarioJecCc = dto.ValUnitarioJecCc;
                contrato.ValUnitarioProcon = dto.ValUnitarioProcon;
                contrato.DatInicioVigencia = dto.DatInicioVigencia;
                contrato.DatFimVigencia = dto.DatFimVigencia;
                contrato.ValUnitAudCapital = dto.ValUnitAudCapital;
                contrato.ValUnitAudInterior = dto.ValUnitAudInterior;
                contrato.IndPermanenciaLegado = dto.IndPermanenciaLegado;
                contrato.NumMesesPermanencia = dto.NumMesesPermanencia;
                contrato.ValDescontoPermanencia = dto.ValDescontoPermanencia;
                contrato.CodContratoAtuacao = dto.CodContratoAtuacao;
                contrato.CodContratoDiretoria = dto.CodContratoDiretoria;

                await _db.ContratoEscritorio.AddAsync(contrato);
                await _db.SaveChangesAsync(User.Identity!.Name!, true, ct);

                foreach (var codEscritorio in dto.Escritorios)
                {
                    foreach (var estado in dto.Estados)
                    {
                        contratoEscritorioEstado.CodContratoEscritorio = contrato.CodContratoEscritorio;
                        contratoEscritorioEstado.CodProfissional = codEscritorio;
                        contratoEscritorioEstado.CodEstado = estado;

                        await _db.ContratoEscritorioEstado.AddAsync(contratoEscritorioEstado);
                        await _db.SaveChangesAsync(User.Identity!.Name!, true, ct);
                    }
                }

                return Ok("Contrato registrado com sucesso!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest("Não foi possível salvar o contrato. Verifique os dados novamente.");
            }
        }

        [HttpPut("editar-contrato")]
        public async Task<IActionResult> AtualizarContratoAsync(CancellationToken ct, [FromQuery] int id, [FromBody] ContratoEscritorioRequest dto)
        {
            try
            {
                var contrato = await _db.ContratoEscritorio.FirstOrDefaultAsync(x => x.CodContratoEscritorio == id);
                if (contrato == null)
                    return BadRequest("Contrato não encontrado.");

                var NumContratoInicialJecVc = contrato.NumContratoJecVc;
                var NumContratoInicialProcon = contrato.NumContratoProcon;

                if (dto.NumContratoJecVc != NumContratoInicialJecVc)
                    if (_db.ContratoEscritorio.Any(x => x.NumContratoJecVc == dto.NumContratoJecVc && dto.NumContratoJecVc != null))
                        return BadRequest($"O contrato não poderá ser cadastrado. Já existe um contrato cadastrado com esse número de contrato JEC/CC.");

                if (dto.NumContratoProcon != NumContratoInicialProcon)
                    if (_db.ContratoEscritorio.Any(x => x.NumContratoProcon == dto.NumContratoProcon && dto.NumContratoProcon != null))
                        return BadRequest($"O contrato não poderá ser cadastrado. Já existe um contrato cadastrado com esse número de contrato PROCON.");

                var contratoEscritorioEstado = new ContratoEscritorioEstado();

                contrato.CodTipoContratoEscritorio = dto.CodTipoContratoEscritorio;
                contrato.IndAtivo = dto.IndAtivo;
                contrato.IndConsideraCalculoVep = dto.IndConsideraCalculoVep;
                contrato.NumContratoJecVc = dto.NumContratoJecVc;
                contrato.NumContratoProcon = dto.NumContratoProcon;
                contrato.NomContrato = dto.NomContrato;
                contrato.NumSgpagJecVc = dto.NumSgpagJecVc;
                contrato.NumSgpagProcon = dto.NumSgpagProcon;
                contrato.Cnpj = dto.Cnpj;
                contrato.ValVep = dto.ValVep;
                contrato.ValUnitarioJecCc = dto.ValUnitarioJecCc;
                contrato.ValUnitarioProcon = dto.ValUnitarioProcon;
                contrato.DatInicioVigencia = dto.DatInicioVigencia;
                contrato.DatFimVigencia = dto.DatFimVigencia;
                contrato.ValUnitAudCapital = dto.ValUnitAudCapital;
                contrato.ValUnitAudInterior = dto.ValUnitAudInterior;
                contrato.IndPermanenciaLegado = dto.IndPermanenciaLegado;
                contrato.NumMesesPermanencia = dto.NumMesesPermanencia;
                contrato.ValDescontoPermanencia = dto.ValDescontoPermanencia;
                contrato.CodContratoAtuacao = dto.CodContratoAtuacao;
                contrato.CodContratoDiretoria = dto.CodContratoDiretoria;

                await _db.SaveChangesAsync(User.Identity!.Name!, true, ct);

                // exclui os escritórios com estados removidos da tela
                var escritoriosEstados = await _db.ContratoEscritorioEstado.Where(x => x.CodContratoEscritorio == id).ToListAsync(ct);
                var removerEscritoriosEstado = escritoriosEstados.Where(x => !dto.Estados.Contains(x.CodEstado)).ToList();

                foreach (var r in removerEscritoriosEstado)
                {
                    var escritorio = await _db.ContratoEscritorioEstado.FirstAsync(x => x.CodContratoEscritorio == id && x.CodEstado == r.CodEstado && x.CodProfissional == r.CodProfissional);
                    _db.ContratoEscritorioEstado.Remove(escritorio);
                    await _db.SaveChangesAsync(User.Identity!.Name!, true, ct);
                }

                // exclui os escritórios removidos da tela
                var escritorios = await _db.ContratoEscritorioEstado.Where(x => x.CodContratoEscritorio == id).ToListAsync();
                var removerEscritorios = escritorios.Where(x => !dto.Escritorios.Contains(x.CodProfissional)).ToList();

                foreach (var r in removerEscritorios)
                {
                    var escritorio = await _db.ContratoEscritorioEstado.FirstAsync(x => x.CodContratoEscritorio == id && x.CodEstado == r.CodEstado && x.CodProfissional == r.CodProfissional);
                    _db.ContratoEscritorioEstado.Remove(escritorio);
                    await _db.SaveChangesAsync(User.Identity!.Name!, true, ct);
                }

                // adiciona os novos escritorios
                foreach (var codEscritorio in dto.Escritorios)
                {
                    foreach (var estado in dto.Estados)
                    {
                        var escritorioRegistrado = escritorios.Any(x => x.CodContratoEscritorio == id && x.CodProfissional == codEscritorio && x.CodEstado == estado);

                        if (!escritorioRegistrado)
                        {
                            contratoEscritorioEstado.CodContratoEscritorio = id;
                            contratoEscritorioEstado.CodProfissional = codEscritorio;
                            contratoEscritorioEstado.CodEstado = estado;

                            await _db.ContratoEscritorioEstado.AddAsync(contratoEscritorioEstado);
                            await _db.SaveChangesAsync(User.Identity!.Name!, true, ct);
                        }
                    }

                }

                await _db.SaveChangesAsync(User.Identity!.Name!, true, ct);

                return Ok("Contrato alterado com sucesso!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest("Não foi possível salvar a alteração do contrato. Verifique os dados novamente.");
            }
        }

        [HttpDelete("excluir-contrato")]
        public async Task<IActionResult> ExcluirContratoAsync(CancellationToken ct, [FromQuery] int id)
        {
            try
            {
                var contrato = _db.ContratoEscritorio.FirstOrDefault(x => x.CodContratoEscritorio == id);
                if (contrato == null)
                    return BadRequest("Contrato não encontrado.");

                var contratoEscritorio = _db.ContratoEscritorioEstado.Where(x => x.CodContratoEscritorio == id).ToList();

                _db.ContratoEscritorioEstado.RemoveRange(contratoEscritorio);
                _db.ContratoEscritorio.Remove(contrato);

                await _db.SaveChangesAsync(User.Identity!.Name!, true, ct);
                return Ok("Contrato excluído com sucesso!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.ToString());
            }
        }


        #endregion

        #region DOWNLOAD METHODS

        [HttpGet("download-lista-contrato")]
        public async Task<FileContentResult> DownloadListaContratoAsync(CancellationToken ct, [FromQuery] BuscaContratoEscritorioRequest contratoDto, [FromQuery] string? ordem, [FromQuery] bool asc = true)
        {
            var queryResult = await QueryContrato(contratoDto, ordem, asc).ToArrayAsync(ct);

            var nomeArquivoListaContratos = $"Lista_Contratos_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

            var csv = queryResult.ToCsvByteArray(typeof(ListaContratoEscritorioResponseMap), false);

            var bytes = Encoding.UTF8.GetPreamble().Concat(csv).ToArray();

            return File(bytes, "application/octet-stream", nomeArquivoListaContratos);
        }

        [HttpGet("download-log-contrato")]
        public async Task<FileContentResult> DownloadLogContratoAsync(CancellationToken ct)
        {
            var query = await QueryLogContrato(ct);

            var queryEstado = await QueryLogContratoEscritorio(ct);

            var nomeArquivoListaContratos = $"Log_Contrato_Escritorio_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            var nomeArquivoListaContratosEstado = $"Log_Contrato_Escritorio_Estado_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

            var csvContrato = query.ToCsvByteArray(typeof(LogContratoEscritorioResponseMap), false);
            var csvContratoEstado = queryEstado.ToCsvByteArray(typeof(LogContratoEscritorioEstadoResponseMap), false);
          
            using (var compressedFileStream = new MemoryStream())
            {
                using (var zipArchive = new ZipArchive(compressedFileStream, ZipArchiveMode.Update, false, Encoding.UTF8))
                {
                    var bytesContrato = Encoding.UTF8.GetPreamble().Concat(csvContrato).ToArray();
                    var bytesContratoEstado = Encoding.UTF8.GetPreamble().Concat(csvContratoEstado).ToArray();                  

                    //Criar uma entrada para cada anexo a ser "Zipado"
                    var zipEntryContratro = zipArchive.CreateEntry(nomeArquivoListaContratos);

                    //Pegar o stream do anexo
                    using (var originalFileStream = new MemoryStream(bytesContrato))
                    {
                        using (var zipEntryStream = zipEntryContratro.Open())
                        {
                            //Copia o anexo na memória para a entrada ZIP criada
                            originalFileStream.CopyTo(zipEntryStream);
                        }
                    }

                    //Criar uma entrada para cada anexo a ser "Zipado"
                    var zipEntryContratroEstado = zipArchive.CreateEntry(nomeArquivoListaContratosEstado);

                    //Pegar o stream do anexo
                    using (var originalFileStream = new MemoryStream(bytesContratoEstado))
                    {
                        using (var zipEntryStream = zipEntryContratroEstado.Open())
                        {
                            //Copia o anexo na memória para a entrada ZIP criada
                            originalFileStream.CopyTo(zipEntryStream);
                        }
                    }

                }

                return File(compressedFileStream.ToArray(), "application/zip", $"Log_Contratos_{DateTime.Now:yyyyMMdd_HHmmss}.zip");
            }
        }

        #endregion

        #region PRIVATE METHODS

        private IQueryable<ContratoEscritorioResponse> QueryContrato(BuscaContratoEscritorioRequest contratoDto, string? ordem, bool asc)
        {
            var queryBase = from ce in _db.ContratoEscritorio
                            join cee in _db.ContratoEscritorioEstado
                            on ce.CodContratoEscritorio equals cee.CodContratoEscritorio
                            where (ce.NomContrato.Trim().ToUpper().Contains(contratoDto.NomContrato.Trim().ToUpper())) &&
                                  (contratoDto.TipoContrato == 0 || ce.CodTipoContratoEscritorio == contratoDto.TipoContrato)
                            select new
                            {
                                contratoEscritorio = ce,
                                contratoEscritorioEstado = cee
                            };

            var queryGroup = queryBase.GroupBy(x => x.contratoEscritorio.CodContratoEscritorio)
                       .Select(grupo => new
                       {
                           CodContratoEscritorio = grupo.Key,
                           EscritoriosLista = grupo.Select(x => x.contratoEscritorioEstado.CodProfissionalNavigation.NomProfissional).OrderBy(x => x).ToList(),
                           UfLista = grupo.Select(x => x.contratoEscritorioEstado.CodEstado).OrderBy(x => x).ToList(),
                           TipoContratoEscritorio = grupo.Select(x => x.contratoEscritorio.CodTipoContratoEscritorioNavigation.DscTipoContrato).FirstOrDefault(),
                           DatInicioVigencia = grupo.Select(x => x.contratoEscritorio.DatInicioVigencia).FirstOrDefault(),
                           DatFimVigencia = grupo.Select(x => x.contratoEscritorio.DatFimVigencia).FirstOrDefault(),
                           Cnpj = grupo.Select(x => x.contratoEscritorio.Cnpj).FirstOrDefault(),
                           NumContratoJecVc = grupo.Select(x => x.contratoEscritorio.NumContratoJecVc).FirstOrDefault(),
                           NumContratoProcon = grupo.Select(x => x.contratoEscritorio.NumContratoProcon).FirstOrDefault(),
                           NomContrato = grupo.Select(x => x.contratoEscritorio.NomContrato).FirstOrDefault(),
                           ValUnitarioJecCc = grupo.Select(x => x.contratoEscritorio.ValUnitarioJecCc).FirstOrDefault(),
                           ValUnitarioProcon = grupo.Select(x => x.contratoEscritorio.ValUnitarioProcon).FirstOrDefault(),
                           ValUnitAudCapital = grupo.Select(x => x.contratoEscritorio.ValUnitAudCapital).FirstOrDefault(),
                           ValUnitAudInterior = grupo.Select(x => x.contratoEscritorio.ValUnitAudInterior).FirstOrDefault(),
                           ValVep = grupo.Select(x => x.contratoEscritorio.ValVep).FirstOrDefault(),
                           NumSgpagJecVc = grupo.Select(x => x.contratoEscritorio.NumSgpagJecVc).FirstOrDefault(),
                           NumSgpagProcon = grupo.Select(x => x.contratoEscritorio.NumSgpagProcon).FirstOrDefault(),
                           NumMesesPermanencia = grupo.Select(x => x.contratoEscritorio.NumMesesPermanencia).FirstOrDefault(),
                           ValDescontoPermanencia = grupo.Select(x => x.contratoEscritorio.ValDescontoPermanencia).FirstOrDefault(),
                           IndPermanenciaLegado = grupo.Select(x => x.contratoEscritorio.IndPermanenciaLegado == "S" ? "Sim" : "Não").FirstOrDefault(),
                           IndAtivo = grupo.Select(x => x.contratoEscritorio.IndAtivo == "S" ? "Ativo" : "Inativo").FirstOrDefault(),
                           IndConsideraCalculoVep = grupo.Select(x => x.contratoEscritorio.IndConsideraCalculoVep == "S" ? "Sim" : "Não").FirstOrDefault(),
                           ContratoAtuacao = grupo.Select(x => x.contratoEscritorio.CodContratoAtuacaoNavigation.DscContratoAtuacao).FirstOrDefault(),
                           ContratoDiretoria = grupo.Select(x => x.contratoEscritorio.CodContratoDiretoriaNavigation.DscContratoDiretoria).FirstOrDefault()
                       });

            var queryResult = queryGroup.Select(x => new ContratoEscritorioResponse
            {
                CodContratoEscritorio = x.CodContratoEscritorio,
                Escritorios = string.Join(" | ", x.EscritoriosLista.Distinct()),
                UF = string.Join(" | ", x.UfLista.Distinct()),
                TipoContratoEscritorio = x.TipoContratoEscritorio,
                DatInicioVigencia = x.DatInicioVigencia,
                DatFimVigencia = x.DatFimVigencia,
                Cnpj = x.Cnpj,
                NumContratoJecVc = x.NumContratoJecVc,
                NumContratoProcon = x.NumContratoProcon,
                NomContrato = x.NomContrato,
                ValUnitarioJecCc = x.ValUnitarioJecCc,
                ValUnitarioProcon = x.ValUnitarioProcon,
                ValUnitAudCapital = x.ValUnitAudCapital,
                ValUnitAudInterior = x.ValUnitAudInterior,
                ValVep = x.ValVep,
                NumSgpagJecVc = x.NumSgpagJecVc,
                NumSgpagProcon = x.NumSgpagProcon,
                NumMesesPermanencia = x.NumMesesPermanencia,
                ValDescontoPermanencia = x.ValDescontoPermanencia,
                IndPermanenciaLegado = x.IndPermanenciaLegado,
                IndAtivo = x.IndAtivo,
                IndConsideraCalculoVep = x.IndConsideraCalculoVep,
                ContratoAtuacao = x.ContratoAtuacao,
                ContratoDiretoria = x.ContratoDiretoria
            });

            switch (ordem)
            {
                case "Tipo Contrato":
                    queryResult = asc ? queryResult.OrderBy(x => x.TipoContratoEscritorio) : queryResult.OrderByDescending(x => x.TipoContratoEscritorio);
                    break;
                case "Nome":
                    queryResult = asc ? queryResult.OrderBy(x => x.NomContrato) : queryResult.OrderByDescending(x => x.NomContrato);
                    break;
                case "Nome Contrato":
                    queryResult = asc ? queryResult.OrderBy(x => x.NomContrato) : queryResult.OrderByDescending(x => x.NomContrato);
                    break;
                case "Numero Contrato":
                    queryResult = asc ? queryResult.OrderBy(x => x.NumContratoJecVc) : queryResult.OrderByDescending(x => x.NumContratoJecVc);
                    break;
                case "Status":
                    queryResult = asc ? queryResult.OrderBy(x => x.IndAtivo) : queryResult.OrderByDescending(x => x.IndAtivo);
                    break;
                default:
                    queryResult = queryResult.OrderBy(x => x.NomContrato);
                    break;
            }

            return queryResult;
        }

        private IQueryable<ObterEscritorioResponse> QueryObterContrato(int codContrato)
        {
            var queryBase = from ce in _db.ContratoEscritorio
                            join cee in _db.ContratoEscritorioEstado
                            on ce.CodContratoEscritorio equals cee.CodContratoEscritorio
                            where ce.CodContratoEscritorio == codContrato
                            select new
                            {
                                contratoEscritorio = ce,
                                contratoEscritorioEstado = cee
                            };

            var queryResult = queryBase.GroupBy(x => x.contratoEscritorio.CodContratoEscritorio)
                       .Select(grupo => new ObterEscritorioResponse
                       {
                           CodContratoEscritorio = grupo.Key,

                           Escritorios = grupo.Select(x => x.contratoEscritorioEstado.CodProfissional).OrderBy(x => x).ToList(),
                           UF = grupo.Select(x => x.contratoEscritorioEstado.CodEstado).OrderBy(x => x).ToList(),

                           CodTipoContratoEscritorio = grupo.Select(x => x.contratoEscritorio.CodTipoContratoEscritorio).FirstOrDefault(),
                           DatInicioVigencia = grupo.Select(x => x.contratoEscritorio.DatInicioVigencia).FirstOrDefault(),
                           DatFimVigencia = grupo.Select(x => x.contratoEscritorio.DatFimVigencia).FirstOrDefault(),
                           Cnpj = grupo.Select(x => x.contratoEscritorio.Cnpj).FirstOrDefault(),
                           NumContratoJecVc = grupo.Select(x => x.contratoEscritorio.NumContratoJecVc).FirstOrDefault(),
                           NumContratoProcon = grupo.Select(x => x.contratoEscritorio.NumContratoProcon).FirstOrDefault(),
                           NomContrato = grupo.Select(x => x.contratoEscritorio.NomContrato).FirstOrDefault(),
                           ValUnitarioJecCc = grupo.Select(x => x.contratoEscritorio.ValUnitarioJecCc).FirstOrDefault(),
                           ValUnitarioProcon = grupo.Select(x => x.contratoEscritorio.ValUnitarioProcon).FirstOrDefault(),
                           ValUnitAudCapital = grupo.Select(x => x.contratoEscritorio.ValUnitAudCapital).FirstOrDefault(),
                           ValUnitAudInterior = grupo.Select(x => x.contratoEscritorio.ValUnitAudInterior).FirstOrDefault(),
                           ValVep = grupo.Select(x => x.contratoEscritorio.ValVep).FirstOrDefault(),
                           NumSgpagJecVc = grupo.Select(x => x.contratoEscritorio.NumSgpagJecVc).FirstOrDefault(),
                           NumSgpagProcon = grupo.Select(x => x.contratoEscritorio.NumSgpagProcon).FirstOrDefault(),
                           NumMesesPermanencia = grupo.Select(x => x.contratoEscritorio.NumMesesPermanencia).FirstOrDefault(),
                           ValDescontoPermanencia = grupo.Select(x => x.contratoEscritorio.ValDescontoPermanencia).FirstOrDefault(),
                           IndPermanenciaLegado = grupo.Select(x => x.contratoEscritorio.IndPermanenciaLegado == "S").FirstOrDefault(),
                           IndAtivo = grupo.Select(x => x.contratoEscritorio.IndAtivo == "S").FirstOrDefault(),
                           IndConsideraCalculoVep = grupo.Select(x => x.contratoEscritorio.IndConsideraCalculoVep == "S").FirstOrDefault(),
                           CodContratoAtuacao = grupo.Select(x => x.contratoEscritorio.CodContratoAtuacaoNavigation.CodContratoAtuacao).FirstOrDefault(),
                           CodContratoDiretoria = grupo.Select(x => x.contratoEscritorio.CodContratoDiretoriaNavigation.CodContratoDiretoria).FirstOrDefault()
                       });

            return queryResult;
        }

        private async Task<IEnumerable<LogContratoEscritorioResponse>> QueryLogContrato(CancellationToken ct)
        {
            var query = (from lce in _db.LogContratoEscritorio
                         select new LogContratoEscritorioResponse
                         {
                             CodContratoEscritorio = lce.CodContratoEscritorio,
                             OperacaoContrato = lce.Operacao,
                             DatLogContrato = lce.DatLog.HasValue ? new DateTime(lce.DatLog.Value.Year, lce.DatLog.Value.Month, lce.DatLog.Value.Day, lce.DatLog.Value.Hour, lce.DatLog.Value.Minute, lce.DatLog.Value.Second).DataHoraFormatada() : null,
                             CodUsuario = lce.CodUsuario,
                             CodTipoContratoEscritorioA = lce.CodTipoContratoEscritorioA,
                             CodTipoContratoEscritorioD = lce.CodTipoContratoEscritorioD,
                             DatInicioVigenciaA = lce.DatInicioVigenciaA.HasValue ? new DateTime(lce.DatInicioVigenciaA.Value.Year, lce.DatInicioVigenciaA.Value.Month, lce.DatInicioVigenciaA.Value.Day, lce.DatInicioVigenciaA.Value.Hour, lce.DatInicioVigenciaA.Value.Minute, 0).DataHoraFormatada() : null,
                             DatInicioVigenciaD = lce.DatInicioVigenciaD.HasValue ? new DateTime(lce.DatInicioVigenciaD.Value.Year, lce.DatInicioVigenciaD.Value.Month, lce.DatInicioVigenciaD.Value.Day, lce.DatInicioVigenciaD.Value.Hour, lce.DatInicioVigenciaD.Value.Minute, 0).DataHoraFormatada() : null,
                             DatFimVigenciaA = lce.DatFimVigenciaA.HasValue ? new DateTime(lce.DatFimVigenciaA.Value.Year, lce.DatFimVigenciaA.Value.Month, lce.DatFimVigenciaA.Value.Day, lce.DatFimVigenciaA.Value.Hour, lce.DatFimVigenciaA.Value.Minute, 0).DataHoraFormatada() : null,
                             DatFimVigenciaD = lce.DatFimVigenciaD.HasValue ? new DateTime(lce.DatFimVigenciaD.Value.Year, lce.DatFimVigenciaD.Value.Month, lce.DatFimVigenciaD.Value.Day, lce.DatFimVigenciaD.Value.Hour, lce.DatFimVigenciaD.Value.Minute, 0).DataHoraFormatada() : null,
                             CnpjA = lce.CnpjA,
                             CnpjD = lce.CnpjD,
                             NumContratoJecVcA = lce.NumContratoJecVcA,
                             NumContratoJecVcD = lce.NumContratoJecVcD,
                             NumContratoProconA = lce.NumContratoProconA,
                             NumContratoProconD = lce.NumContratoProconD,
                             NomContratoA = lce.NomContratoA,
                             NomContratoD = lce.NomContratoD,
                             ValUnitarioJecCcA = lce.ValUnitarioJecCcA,
                             ValUnitarioJecCcD = lce.ValUnitarioJecCcD,
                             ValUnitarioProconA = lce.ValUnitarioProconA,
                             ValUnitarioProconD = lce.ValUnitarioProconD,
                             ValUnitAudCapitalA = lce.ValUnitAudCapitalA,
                             ValUnitAudCapitalD = lce.ValUnitAudCapitalD,
                             ValUnitAudInteriorA = lce.ValUnitAudInteriorA,
                             ValUnitAudInteriorD = lce.ValUnitAudInteriorD,
                             ValVepA = lce.ValVepA,
                             ValVepD = lce.ValVepD,
                             NumSgpagJecVcA = lce.NumSgpagJecVcA,
                             NumSgpagJecVcD = lce.NumSgpagJecVcD,
                             NumSgpagProconA = lce.NumSgpagProconA,
                             NumSgpagProconD = lce.NumSgpagProconD,
                             IndPermanenciaLegadoA = lce.IndPermanenciaLegadoA,
                             IndPermanenciaLegadoD = lce.IndPermanenciaLegadoD,
                             NumMesesPermanenciaA = lce.NumMesesPermanenciaA,
                             NumMesesPermanenciaD = lce.NumMesesPermanenciaD,
                             ValDescontoPermanenciaA = lce.ValDescontoPermanenciaA,
                             ValDescontoPermanenciaD = lce.ValDescontoPermanenciaD,
                             IndAtivoA = lce.IndAtivoA,
                             IndAtivoD = lce.IndAtivoD,
                             IndConsideraCalculoVepA = lce.IndConsideraCalculoVepA,
                             IndConsideraCalculoVepD = lce.IndConsideraCalculoVepD,
                             CodContratoAtuacaoA = lce.CodContratoAtuacaoA,
                             CodContratoAtuacaoD = lce.CodContratoAtuacaoD,
                             CodContratoDiretoriaA = lce.CodContratoDiretoriaA,
                             CodContratoDiretoriaD = lce.CodContratoDiretoriaD

                         }).Distinct();

            return (await query.ToListAsync(ct)).OrderByDescending(x => x.DatLogContrato);
        }

        private async Task<IEnumerable<LogContratoEscritorioEstadoResponse>> QueryLogContratoEscritorio(CancellationToken ct)
        {
            var query = (from lcee in _db.LogContratoEscritorioEstado
                         select new LogContratoEscritorioEstadoResponse
                         {         
                             CodContratoEscritorio = lcee.CodContratoEscritorio,
                             Operacao = lcee.Operacao,
                             DatLog = lcee.DatLog.HasValue ? new DateTime(lcee.DatLog.Value.Year, lcee.DatLog.Value.Month, lcee.DatLog.Value.Day, lcee.DatLog.Value.Hour, lcee.DatLog.Value.Minute, 0).DataHoraFormatada() : null, // verificar se n vai quebrar
                             CodUsuario = lcee.CodUsuario,
                             CodProfissionalA = lcee.CodProfissionalA,
                             CodProfissionalD = lcee.CodProfissionalD,
                             CodEstadoA = lcee.CodEstadoA,
                             CodEstadoD = lcee.CodEstadoD
                         }).Distinct();

            return (await query.ToListAsync(ct)).OrderByDescending(x => x.DatLog);
        }
        #endregion

    }
}
