using Oi.Juridico.Contextos.V2.ESocialContext.Data;
using Oi.Juridico.Contextos.V2.ESocialContext.Entities;
using Oi.Juridico.Shared.V2.Enums;
using Oi.Juridico.Shared.V2.Helpers;
using System.IO;
using System.Xml.Serialization;
using Oi.Juridico.AddOn.Extensions.IEnumerable;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Http;
using Oi.Juridico.WebApi.V2.Services;
using Perlink.Oi.Juridico.Infra.Constants;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1.RequestDTOs;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1.DTOs;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1.Services;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1.DTOs.CsvHelperMap;
using Perlink.Oi.Juridico.Application.Security;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1.Controllers
{
    [Route("api/esocial/v1/[controller]")]
    [ApiController]
    public class ESocialAcompanhamentoController : ControllerBase
    {
        private ESocialDbContext _eSocialDbContext;
        private readonly ControleDeAcessoService _controleDeAcessoService;

        public ESocialAcompanhamentoController(ESocialDbContext eSocialDbContext, ControleDeAcessoService controleDeAcessoService)
        {
            _eSocialDbContext = eSocialDbContext;
            _controleDeAcessoService = controleDeAcessoService;
        }

        #region Consultas

        [HttpPost("obter")]
        public async Task<ActionResult<RetornoPaginadoDTO<VEsAcompanhamentoDTO>>> Obter([FromBody] EsF2500AcompanhamentoRequestDTO requestDTO,
                                                                                                         [FromServices] ESocialDownloadRetornoService service,
                                                                                                         CancellationToken ct,
                                                                                                         [FromQuery] int? pagina = 1,
                                                                                                         [FromQuery] int? quantidade = 8
                                                                                                         )
        {
            try
            {
                var (dadosInvalido, listaErros) = requestDTO.Validar();

                if (dadosInvalido)
                {
                    return BadRequest(listaErros);
                }

                var resultado = await Acompanhamento(requestDTO, ct, pagina, quantidade, service);

                return Ok(resultado);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("status-formulario")]
        public async Task<ActionResult<IEnumerable<RetornoListaDTO>>> ObterListaStatusFormularioAsync()
        {

            var listaEnum = EnumExtension.ToList<EsocialStatusFormulario>()
                .Where(x => x.ToInt() != 0)
                .Select(x => new RetornoListaDTO()
                {
                    Id = x.ToInt(),
                    Descricao = x.Descricao()
                }
                );

            return Ok(listaEnum);
        }

        [HttpPost("empresas-formulario")]
        public async Task<ActionResult<IEnumerable<RetornoListaDTO>>> ObterListaEmpresaFormularioAsync()
        {
            try
            {
                var loginUsuario = User.Identity!.Name;

                var usuarioEmpresaDoGrupo = _eSocialDbContext.AcaUsuarioEmpresaGrupo.AsNoTracking().Where(u => u.CodUsuario == loginUsuario).Select(s => s.CodParte);

                if (usuarioEmpresaDoGrupo.Any())
                {
                    var listaEnum = await (from e in _eSocialDbContext.EsEmpresaAgrupadora.AsNoTracking()
                                           join p in _eSocialDbContext.Parte on e.IdEsEmpresaAgrupadora equals p.IdEsEmpresaAgrupadora
                                           where usuarioEmpresaDoGrupo.Contains(p.CodParte)
                                           select new RetornoListaDTO()
                                           {
                                               Id = e.IdEsEmpresaAgrupadora,
                                               Descricao = e.NomEmpresaAgrupadora
                                           }).Distinct().ToListAsync();
                    return Ok(listaEnum);
                }
                else
                {
                    var listaEnum = await (from e in _eSocialDbContext.EsEmpresaAgrupadora.AsNoTracking()
                                           select new RetornoListaDTO()
                                           {
                                               Id = e.IdEsEmpresaAgrupadora,
                                               Descricao = e.NomEmpresaAgrupadora
                                           }).ToListAsync();
                    return Ok(listaEnum);
                }
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }

        }

        [HttpPost("uf")]
        public async Task<ActionResult<IEnumerable<RetornoListaUFDTO>>> ObterUFAsync()
        {
            try
            {
                var _listaEstado = await _eSocialDbContext.Estado
                .Select(e => new RetornoListaUFDTO()
                {
                    Id = e.CodEstado,
                    Descricao = e.CodEstado //string.Format("{0} - {1}", e.CodEstado, e.NomEstado)
                }).OrderBy(e => e.Id).ToListAsync();

                return _listaEstado;

            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }

        }

        [HttpPost("escritorio")]
        public async Task<ActionResult<IEnumerable<RetornoListaEscritorioContador>>> ObterEscritorioAsync()
        {
            try
            {
                var loginUsuario = User.Identity!.Name;
                var usuario = await _eSocialDbContext.AcaUsuario.AsNoTracking()
                .Where(x => x.CodUsuario == loginUsuario)
                .Select(x => new
                {
                    EhEscritorio = x.AcaUsuarioEscritorio.Any(p => p.CodProfissionalNavigation.IndEscritorio == "S"),
                    CodProfissional = x.AcaUsuarioEscritorio.Where(u => u.CodUsuario == loginUsuario).Select(s => s.CodProfissional)
                }).FirstOrDefaultAsync();

                if (usuario!.EhEscritorio && usuario.CodProfissional.Any())
                {
                    var listaEscritorio = await (from prof in _eSocialDbContext.Profissional.AsNoTracking()
                                                     //join p in _eSocialDbContext.Processo on prof.CodProfissional equals p.CodProfissional
                                                 where prof.IndEscritorio == "S" && prof.IndAreaTrabalhista == "S" && usuario.CodProfissional.Contains(prof.CodProfissional)
                                                 select new RetornoListaEscritorioContador()
                                                 {
                                                     Id = prof.NomProfissional,
                                                     Descricao = prof.NomProfissional,
                                                     EhEscriCont = usuario.EhEscritorio,
                                                     CodProf = prof.CodProfissional,
                                                 }).Distinct().OrderBy(o => o.Descricao).ToListAsync();
                    return listaEscritorio;
                }
                else
                {
                    var listaEscritorio = await (from prof in _eSocialDbContext.Profissional.AsNoTracking()
                                                     //join p in _eSocialDbContext.Processo on prof.CodProfissional equals p.CodProfissional
                                                 where prof.IndEscritorio == "S" && prof.IndAreaTrabalhista == "S"
                                                 select new RetornoListaEscritorioContador()
                                                 {
                                                     Id = prof.NomProfissional,
                                                     Descricao = prof.NomProfissional,
                                                     EhEscriCont = usuario.EhEscritorio,
                                                     CodProf = prof.CodProfissional,
                                                 }).Distinct().OrderBy(o => o.Descricao).ToListAsync();
                    return listaEscritorio;
                }

            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }

        }

        [HttpPost("contador")]
        public async Task<ActionResult<IEnumerable<RetornoListaEscritorioContador>>> ObterContadorAsync()
        {
            try
            {

                var loginUsuario = User.Identity!.Name;
                var usuario = await _eSocialDbContext.AcaUsuario.AsNoTracking()
                .Where(x => x.CodUsuario == loginUsuario)
                .Select(x => new
                {
                    EhContador = x.AcaUsuarioEscritorio.Any(p => p.CodProfissionalNavigation.IndContador == "S"),
                    CodProfissional = x.AcaUsuarioEscritorio.Where(u => u.CodUsuario == loginUsuario).Select(s => s.CodProfissional)
                }).FirstOrDefaultAsync();

                if (usuario!.EhContador && usuario.CodProfissional.Any())
                {
                    var listaContador = await (from prof in _eSocialDbContext.Profissional.AsNoTracking()
                                                   //join p in _eSocialDbContext.Processo on prof.CodProfissional equals p.CodContador
                                               where prof.IndContador == "S" && usuario.CodProfissional.Contains(prof.CodProfissional)
                                                 select new RetornoListaEscritorioContador()
                                                 {
                                                     Id = prof.NomProfissional,
                                                     Descricao = prof.NomProfissional,
                                                     EhEscriCont = usuario.EhContador,
                                                     CodProf = prof.CodProfissional
                                                 }).Distinct().OrderBy(o => o.Descricao).ToListAsync();
                    return listaContador;
                }
                else
                {
                    var listaContador = await (from prof in _eSocialDbContext.Profissional.AsNoTracking()
                                                   //join p in _eSocialDbContext.Processo on prof.CodProfissional equals p.CodContador
                                               where prof.IndContador == "S"
                                               select new RetornoListaEscritorioContador()
                                                 {
                                                     Id = prof.NomProfissional,
                                                     Descricao = prof.NomProfissional,
                                                     EhEscriCont = usuario.EhContador,
                                                     CodProf = prof.CodProfissional
                                                 }).Distinct().OrderBy(o => o.Descricao).ToListAsync();
                    return listaContador;
                }

            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }

        }

        #endregion

        #region Exportações
        [HttpPost("exportar-lista")]
        public async Task<FileResult> ExportarCsvLista([FromBody] EsF2500AcompanhamentoRequestDTO requestDTO,
                                                                                                         CancellationToken ct,
                                                                                                         [FromServices] ESocialDownloadRetornoService service,
                                                                                                         [FromQuery] int? pagina = 1,
                                                                                                         [FromQuery] int? quantidade = 8
                                                                                                         )
        {

            var resultado = Acompanhamento(requestDTO, ct, pagina, quantidade, service, false);
            var arquivo = GerarCSV(resultado);
            return arquivo;

        }

        [HttpGet("exportar-xml")]
        public async Task<ActionResult> ExportarXml([FromQuery] int codigoFormulario, [FromQuery] bool f2500, CancellationToken ct)
        {
            try
            {
                var dadosArquivo = await _eSocialDbContext.VEsAcompanhamento.AsNoTracking()
                        .Where(x => x.IdFormulario == codigoFormulario && x.TipoFormulario == (f2500 ? "F_2500" : "F_2501")
                        && x.StatusFormulario == EsocialStatusFormulario.EnviadoESocial.ToByte())
                    .Select(x => new { Id = x.IdEsEmpresaAgrupadora, NomeArquivo = x.NomeArquivoEnviado!, x.TipoFormulario }).FirstOrDefaultAsync(ct);

                if (dadosArquivo == null)
                {
                    throw new Exception($"Arquivo não encontrado para o formulario: {codigoFormulario} ");
                }

                var caminhoArquivo = await _eSocialDbContext.EsEmpresaAgrupadora.AsNoTracking()
                                .Where(x => x.IdEsEmpresaAgrupadora == dadosArquivo!.Id)
                              .Select(x => x.DirXmlEnvioSisjur).FirstOrDefaultAsync();
                var memory = await Download($"{caminhoArquivo}\\{dadosArquivo!.NomeArquivo}");

                if (memory == null)
                    return BadRequest("Arquivo não encontrado");
                return File(memory, GetContentType(dadosArquivo!.NomeArquivo), dadosArquivo!.NomeArquivo);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }


        }

        [HttpPost("upload/retorno")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> UploadArquivoRetorno([FromForm] IFormFile arquivoRetorno, CancellationToken ct)
        {
            if (!_controleDeAcessoService.TemPermissao(Permissoes.ACESSAR_ACOMPANHAMENTO_ENVIO_ESOCIAL))
            {
                return BadRequest("O usuário não tem permissão para acessar a tela de Acompanhamento de Envio do eSocial");
            }

            if (arquivoRetorno is null) return BadRequest("Nenhum arquivo para anexar.");

            try
            {
                using (var reader = new StreamReader(arquivoRetorno.OpenReadStream()))
                {
                    var conteudo = reader.ReadToEnd();
                    var linhas = conteudo.Split('\n');

                    if (arquivoRetorno.FileName.ToUpper().Contains(".TXT"))
                    {
                        var result = ChangeUploadTXTtoXML.ConvertToXml(arquivoRetorno.FileName, "E:\\juridico\\outputXML\\", linhas.ToList(), _eSocialDbContext);
                        if (result == false)
                        {
                            return BadRequest("Processo não encontrou informações do ENVIO ou não encontrou dados do CPF informado.");
                        }
                    }
                    else if (arquivoRetorno.FileName.ToUpper().Contains(".XML"))
                    {
                        var result = ChangeUploadTXTtoXML.RecebeXML(linhas.ToList(), _eSocialDbContext);
                        if (result == false)
                        {
                            return BadRequest("Processo não encontrou informações do ENVIO ou não encontrou dados do CPF informado.");
                        }
                    }
                    else
                    {
                        return BadRequest("Processo não encontrou informações do ENVIO ou não encontrou dados suficientes.");
                    }
                }

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpGet("exportar-retorno")]
        public async Task<ActionResult> ExportarAcompanhamentoRetorno([FromQuery] int codigoFormulario, [FromQuery] bool f2500, [FromServices] ESocialDownloadRetornoService service, CancellationToken ct)
        {
            try
            {
                var responseService = await service.ExportarRetorno(_eSocialDbContext, codigoFormulario, f2500, ct);

                string inicioNomeArquivo = responseService.StatusFormulario == EsocialStatusFormulario.PendenteAcaoFPW.ToByte() ? "Retorno_FPW" : "Retorno_eSocial";

                return File(responseService.Dados!, "text/csv", $"{inicioNomeArquivo}_{responseService.InfoprocessoNrproctrab}_{responseService.CpfParte?.Replace(".", "").Replace("-", "")}_{responseService.TipoFormulario}_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.csv");


            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("exportar-historico")]
        public async Task<ActionResult> ExportarAcompanhamentoHistoricoRetorno([FromQuery] int codigoFormulario, [FromQuery] bool f2500, [FromServices] ESocialDownloadRetornoService service, CancellationToken ct)
        {
            try
            {
                var responseService = await service.ExportarHistoricoRetorno(_eSocialDbContext, codigoFormulario, f2500, ct);

                return File(responseService.Dados!, "text/csv", $"Hist_Retorno_eSocial_{responseService.InfoprocessoNrproctrab}_{responseService.CpfParte!.Replace(".", "").Replace("-", "")}_{responseService!.TipoFormulario}_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.csv");

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("exportar-criticas")]
        public async Task<ActionResult> ExportarCriticasRetorno([FromBody] EsF2500AcompanhamentoRequestDTO requestDTO, [FromServices] ESocialDownloadRetornoService service,
                                                                                                         [FromQuery] int? pagina = 1,
                                                                                                         [FromQuery] int? quantidade = 8
                                                                                                         , CancellationToken ct = default(CancellationToken))
        {
            try
            {
                var (dadosInvalido, listaErros) = requestDTO.Validar();

                if (dadosInvalido)
                {
                    return BadRequest(listaErros);
                }
                //requestDTO.StatusExecucao = new List<byte?>() { 5, 11 };

                var resultado = Acompanhamento(requestDTO, ct, pagina, quantidade, service, false).Result.Lista.Select(y => y.IdFormulario).ToList();
                var bytes = await service.ExportarCriticasRetorno(_eSocialDbContext, resultado, requestDTO, ct);
                return File(bytes, "text/csv", $"Retorno_eSocial_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.csv");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        #endregion

        #region Métodos privados
        private FileResult GerarXml2500(EsF2500DTO formularioDTO)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(EsF2500DTO));
            var stringwriter = new StringWriter();
            serializer.Serialize(stringwriter, formularioDTO);

            string xml = stringwriter.ToString(); //string presented xml
            var fileStreamResult = File(Encoding.UTF8.GetBytes(xml), "text/xml", "XmlF2500.xml");

            return fileStreamResult;

        }

        private FileResult GerarXml2501(EsF2501DTO formularioDTO)
        {


            XmlSerializer serializer = new XmlSerializer(typeof(EsF2501DTO));
            var stringwriter = new StringWriter();
            serializer.Serialize(stringwriter, formularioDTO);

            string xml = stringwriter.ToString(); //string presented xml
            var fileStreamResult = File(Encoding.UTF8.GetBytes(xml), "text/xml", "XmlF2501.xml");

            return fileStreamResult;

        }

        private static EsF2501DTO PreencheFormulario2501DTO(ref EsF2501? formulario2501)
        {
            return new EsF2501DTO()
            {
                IdF2501 = formulario2501!.IdF2501,
                CodParte = formulario2501!.CodParte,
                CodProcesso = formulario2501!.CodProcesso,
                StatusFormulario = formulario2501!.StatusFormulario,
                LogCodUsuario = formulario2501!.LogCodUsuario,
                LogDataOperacao = formulario2501!.LogDataOperacao,
                IdeempregadorNrinsc = formulario2501!.IdeempregadorNrinsc,
                IdeempregadorTpinsc = formulario2501!.IdeempregadorTpinsc,
                IdeeventoIndretif = formulario2501!.IdeeventoIndretif,
                IdeprocNrproctrab = formulario2501!.IdeprocNrproctrab,
                IdeprocObs = formulario2501!.IdeprocObs,
                IdeprocPerapurpgto = formulario2501!.IdeprocPerapurpgto,
                ParentIdF2501 = formulario2501!.ParentIdF2501,
                IdetrabCpftrab = formulario2501!.IdetrabCpftrab
            };
        }

        private FileResult GerarCSV(Task<RetornoPaginadoDTO<VEsAcompanhamentoDTO>> resultado)
        {
            var lista = resultado.Result.Lista.ToArray();

            var csv = lista.ToCsvByteArray(typeof(ExportaAcompanhamentoResponseMap), sanitizeForInjection: false);
            var bytes = Encoding.UTF8.GetPreamble().Concat(csv).ToArray();

            return File(bytes, "text/csv", "acompanhamento.csv");
        }

        private async Task<RetornoPaginadoDTO<VEsAcompanhamentoDTO>> Acompanhamento(EsF2500AcompanhamentoRequestDTO requestDTO, CancellationToken ct, int? pagina, int? quantidade, ESocialDownloadRetornoService service, bool paginacao = true)
        {
            var resultado = new RetornoPaginadoDTO<VEsAcompanhamentoDTO>();

            using var scope = await _eSocialDbContext.Database.BeginTransactionAsync(ct);
            _eSocialDbContext.PesquisarPorCaseInsensitive();

            var queryAcompanhamento = _eSocialDbContext.VEsAcompanhamento.AsNoTracking()
                                                .Where(requestDTO.BuildFilter())
                                                .OrderByDescending(x => x.LogDataOperacao)
                                                .Select(a => new VEsAcompanhamentoDTO
                                                {
                                                    CgcParteEmpresa = a.CgcParteEmpresa,
                                                    CodComarca = a.CodComarca,
                                                    CodParte = a.CodParte,
                                                    CodParteEmpresa = a.CodParteEmpresa,
                                                    CodProcesso = a.CodProcesso,
                                                    CodTipoVara = a.CodTipoVara,
                                                    CodVara = a.CodVara,
                                                    CpfParte = a.CpfParte,
                                                    IdEsEmpresaAgrupadora = a!.IdEsEmpresaAgrupadora!,
                                                    IdFormulario = a.IdFormulario,
                                                    IndProcessoAtivo = a.IndProcessoAtivo,
                                                    IndProprioTerceiro = a.IndProprioTerceiro,
                                                    InfoprocessoNrproctrab = a.InfoprocessoNrproctrab,
                                                    LogCodUsuario = a.LogCodUsuario,
                                                    LogDataOperacao = a.LogDataOperacao,
                                                    NomComarca = a.NomComarca,
                                                    NomParte = a.NomParte,
                                                    NomParteEmpresa = a.NomParteEmpresa,
                                                    NomTipoVara = a.NomTipoVara,
                                                    StatusFormulario = a.StatusFormulario,
                                                    TipoFormulario = a.TipoFormulario,
                                                    CodEstado = a.CodEstado,
                                                    InfoprocjudDtsent = a.InfoprocjudDtsent,
                                                    TipoFormularioTipo = a.IdeeventoIndretif,
                                                    FinalizadoContador = a.FinalizadoContador,
                                                    FinalizadoEscritorio = a.FinalizadoEscritorio,
                                                    NrRecibo = a.IdeeventoNrrecibo,
                                                    NomEscritorio = a.NomEscritorio,
                                                    NomContador = a.NomContador,
                                                    NomeUsuario = a.NomeUsuario,
                                                    PeriodoApuracao = !string.IsNullOrEmpty(a.IdeprocPerapurpgto) ? a.IdeprocPerapurpgto.Substring(4, 6) + "/" + a.IdeprocPerapurpgto.Substring(0, 4) : string.Empty,
                                                    ExclusaoNrrecibo = a.ExclusaoNrrecibo,
                                                    NomeArquivoEnviado = a.NomeArquivoEnviado,
                                                    NomeArquivoRetornado = a.NomeArquivoRetornado,
                                                    VersaoEsocial = a.VersaoEsocial,
                                                    ehDataFutura = isDataFutura(a.IdeprocPerapurpgto)
                                                });

            var listaAcompanhamento = new List<VEsAcompanhamentoDTO>();

            if (paginacao)
            {
                var total = await queryAcompanhamento.CountAsync(ct);
                var skip = PaginationHelper.PagesToSkip(quantidade!.Value, total, pagina!.Value);

                listaAcompanhamento = total != 0 ? await queryAcompanhamento.Skip(skip).Take(quantidade!.Value).ToListAsync(ct) : new List<VEsAcompanhamentoDTO> { };

                resultado = new RetornoPaginadoDTO<VEsAcompanhamentoDTO>
                {
                    Total = total,
                    Lista = listaAcompanhamento,
                    ComCritica = queryAcompanhamento!.Where(x => x.StatusFormulario == EsocialStatusFormulario.RetornoESocialNaoOk.ToByte() || x.StatusFormulario == EsocialStatusFormulario.PendenteAcaoFPW.ToByte()).Select(y => y.IdFormulario).Any()
                };
            }
            else
            {
                listaAcompanhamento = await queryAcompanhamento.ToListAsync(ct);
                resultado = new RetornoPaginadoDTO<VEsAcompanhamentoDTO>
                {
                    Total = 0,
                    Lista = listaAcompanhamento != null ? listaAcompanhamento : new List<VEsAcompanhamentoDTO> { },
                    ComCritica = listaAcompanhamento!.Where(x => x.StatusFormulario == EsocialStatusFormulario.RetornoESocialNaoOk.ToByte() || x.StatusFormulario == EsocialStatusFormulario.PendenteAcaoFPW.ToByte()).Select(y => y.IdFormulario).Any()
                };
            }


            var listaRetornos = await _eSocialDbContext.EsInconsistenciasF2500F2501.AsNoTracking()
                                        .Where(x => listaAcompanhamento!.Select(l => l.IdFormulario).Contains(x.IdF2500) || listaAcompanhamento!.Select(l => l.IdFormulario).Contains(x.IdF2501)).ToListAsync(ct);


            foreach (var formulario in listaAcompanhamento!)
            {
                formulario.ExibirHistorico = listaRetornos.Where(x => ((formulario.TipoFormulario == "F_2500" && x.IdF2500 == formulario.IdFormulario) || (formulario.TipoFormulario == "F_2501" && x.IdF2501 == formulario.IdFormulario))).Select(x => x.NomeArquivoEnviado).Distinct().Count() > 1;
                formulario.ExibirRetorno = listaRetornos.Any(x => ((formulario.NomeArquivoEnviado != null && x.NomeArquivoEnviado.Contains(formulario.NomeArquivoEnviado)) || (formulario.NomeArquivoRetornado != null && x.NomeArquivoEnviado.Contains(formulario.NomeArquivoRetornado) && formulario.StatusFormulario == EsocialStatusFormulario.Exclusao3500NaoOk.ToShort())) && ((formulario.TipoFormulario == "F_2500" && x.IdF2500 == formulario.IdFormulario) || (formulario.TipoFormulario == "F_2501" && x.IdF2501 == formulario.IdFormulario)));
            }

            return resultado;
        }

        private static bool isDataFutura(string periodoApuracao)
        {
            if (string.IsNullOrEmpty(periodoApuracao) || periodoApuracao.Length != 6)
                return false;

            int ano = int.Parse(periodoApuracao.Substring(0, 4));
            int mes = int.Parse(periodoApuracao.Substring(4, 2));

            DateTime now = DateTime.Now;
            int anoAtual = now.Year;
            int mesAtual = now.Month;

            return ano > anoAtual || (ano == anoAtual && mes > mesAtual);
        }

        private async Task<Stream?> Download(string url)
        {
            try
            {
                var filePath = url;
                if (!System.IO.File.Exists(filePath))
                {
                    return null;
                }
                var memory = new MemoryStream();
                using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                return memory;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private string GetContentType(string file)
        {
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(file, out string contentType))
                contentType = "application/octet-stream";
            return contentType;
        }
        #endregion


    }
}