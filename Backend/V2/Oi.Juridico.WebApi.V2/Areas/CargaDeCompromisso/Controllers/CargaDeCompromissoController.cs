using Microsoft.AspNetCore.Authorization;
using Oi.Juridico.Contextos.V2.CargaDeCompromissoContext.Data;
using Oi.Juridico.Contextos.V2.CargaDeCompromissoContext.Entities;
using Oi.Juridico.Contextos.V2.ControleDeAcessoContext.Data;
using Oi.Juridico.Contextos.V2.ParametroJuridicoContext.Data;
using Oi.Juridico.WebApi.V2.Areas.AgendamentoCargaCompromissos.Services;
using Oi.Juridico.WebApi.V2.Areas.CargaDeCompromisso.DTOs;
using Org.BouncyCastle.Bcpg.OpenPgp;
using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;
using System.IO;



namespace Oi.Juridico.WebApi.V2.Areas.Contingencia.Movimentacao.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CargaDeCompromissoController : ControllerBase
    {
        private CargaDeCompromissoDbContext _context;
        private readonly ParametroJuridicoContext _parametroJuridico;
        private readonly ControleDeAcessoContext _db;
        //private readonly ILogger _logger;

        public CargaDeCompromissoController(ParametroJuridicoContext parametroJuridico, ControleDeAcessoContext db, CargaDeCompromissoDbContext context)
        {
            _parametroJuridico = parametroJuridico;
            _context = context;
            _db = db;
            //_logger = logger;
        }

        [HttpGet("Obter")]
        public async Task<IActionResult> ListarAsync(CancellationToken ct,                                                     
                                                     [FromQuery] int? condicaoProcesso =1 ,     //1 = Contem  2 = Igual
                                                     [FromQuery] int? codigoProcesso = null, //texto da pesquia
                                                     [FromQuery] int? statusParcela = 0, 
                                                     [FromQuery] DateTime? vencimentoDe = null,
                                                     [FromQuery] DateTime? vencimentoAte = null,
                                                     [FromQuery] string? nome = null,       //campo busca nome
                                                     [FromQuery] string? documento = null,  //campo busca de documento 
                                                     [FromQuery] int? carga = null,
                                                     [FromQuery] int? compromisso = null,
                                                     [FromQuery] int? tipoPesquisa = 1 ,   // Tipo pesquisa Nome Ou Documento informado no campo 1 - Credor | 2 - Autor | 3 - Beneficiario
                                                     [FromQuery] int size = 50,
                                                     [FromQuery] int? tipoProcesso = 0,
                                                     [FromQuery] int? campoBuscaProcesso = 1,
                                                     [FromQuery] int? classeCredito = null)
        {

            var (retorno, total, mensagem) = await  Obter(ct,condicaoProcesso, codigoProcesso, statusParcela, vencimentoDe, vencimentoAte, nome,
                                                   documento, carga, compromisso, tipoPesquisa, size, tipoProcesso, campoBuscaProcesso,classeCredito);

            if (retorno == null && mensagem != string.Empty)
            {
                return BadRequest(mensagem);
            }

            return Ok(new
            {
                Total = total,
                Items = retorno
            });
        }

        private async Task<(List<CargaDeCompromissoDTO> , int qtd, string mensagemErro )> Obter(CancellationToken ct,
                                                                          int? condicaoProcesso = 1,     //1 = Contem  2 = Igual
                                                                          int? codigoProcesso = null, //texto da pesquia
                                                                          int? statusParcela = 0,
                                                                          DateTime? vencimentoDe = null,
                                                                          DateTime? vencimentoAte = null,
                                                                          string? nome = null,       //campo busca nome
                                                                          string? documento = null,  //campo busca de documento 
                                                                          int? carga = null,
                                                                          int? compromisso = null,
                                                                          int? tipoPesquisa = 1,   // Tipo pesquisa Nome Ou Documento informado no campo  1 - Autor | 2 - Beneficiario |  3 - Credor 
                                                                          int size = 50,
                                                                          int? tipoProcesso = 0,
                                                                          int? campoBuscaProcesso = 1,
                                                                          int? classeCredito = null)
        {
            try
            {
                //   var query = _context.CargaCompromisso.Include(x => x.CargaCompromissoParcela.OrderBy(x => x.NroParcela)).AsQueryable();

                var query = _context.CargaCompromisso.Include(x => x.CargaCompromissoParcela
                                                     .OrderBy(x => x.NroParcela))
                                                     .ThenInclude(parcela => parcela.Cod)
                                                     .AsQueryable();


                if (tipoProcesso > 0)
                {
                    query = query.Where(c => c.CodTipoProcesso == tipoProcesso);
                }

                if (codigoProcesso != null)
                {
                    query = condicaoProcesso == 1 ? query.Where(c => c.CodProcesso == codigoProcesso) : query.Where(c => c.CodProcesso == codigoProcesso);
                }

                if (documento != null)
                {
                    var doc = Regex.Replace(documento, "[^0-9]", "");
                    if (tipoPesquisa == 1)
                    {
                        query = query.Where(c => c.DocAutor.Contains(doc));
                    }
                    if (tipoPesquisa == 2)
                    {
                        query = query.Where(c => c.DocCredor.Contains(doc));
                    }
                    else
                    {
                        query = query.Where(c => c.NomeBeneficiario.Contains(doc));

                    }
                }
                if (nome != null)
                {
                    if (tipoPesquisa == 1)
                    {
                        query = query.Where(c => c.DocAutor.Contains(nome));
                    }
                    else
                    {
                        if (tipoPesquisa == 2)
                        {
                            query = query.Where(c => c.BorderoBeneficiario.Contains(nome));
                        }
                        else
                        {
                            query = query.Where(c => c.NomeCredor.Contains(nome));
                        }
                    }
                }

                if (classeCredito != null && classeCredito > 0)
                {
                    var classeCreditoDescricao = _context.ClasseCredito.FirstOrDefault(x => x.CodClasseCredito == classeCredito).DscClasseCredito;
                    query = query.Where(x => x.ClasseCredito == classeCreditoDescricao);
                }


                if (carga > 0)
                {
                    query = query.Where(c => c.CodAgendCargaComp == carga);
                }

                if (compromisso > 0)
                {
                    query = query.Where(c => c.Id == compromisso);
                }


                //            var items = await query.OrderByDescending(x => x.Id).ToListAsync(ct);

                if (vencimentoDe.HasValue && vencimentoAte.HasValue)
                {
                    query = query.Where(x => x.CargaCompromissoParcela.Any(y => y.Vencimento >= vencimentoDe && y.Vencimento <= vencimentoAte));
                }

                if (statusParcela != null && statusParcela > 0)
                {                    
                    var listaId = _context.CargaCompromissoParcela.Where(x => x.Status == statusParcela).Select(x => x.IdCompromisso).ToList();
                    if (listaId != null)
                    {
                        query = query.Where(x => listaId.Contains(x.Id));
                    }
                }

                var total = await query.CountAsync(ct);

                if (size == 50 && total > 50)
                {
                    return (null, total,string.Empty);
                }

                size = size == -1 ? 50 : size;


                var retorno = query.OrderByDescending(x => x.Id).Take(size).Select(x => new CargaDeCompromissoDTO
                {
                    Id = x.Id,
                    CodAgendCargaComp = x.CodAgendCargaComp,
                    CodProcesso = x.CodProcesso,
                    CodTipoProcesso = x.CodTipoProcesso,
                    CodCatPagamento = x.CodCatPagamento,
                    DocAutor = x.DocAutor,
                    QtdParcelas = x.QtdParcelas,
                    MotivoExclusao = x.MotivoExclusao,
                    NomeBeneficiario = x.NomeBeneficiario,
                    DataPrimeiraParcela = x.DataPrimeiraParcela,
                    NroGuia = x.NroGuia,
                    CodBancoArrecadador = x.CodBancoArrecadador,
                    CodFornecedor = x.CodFornecedor,
                    CodFormaPgto = x.CodFormaPgto,
                    CodCentroCusto = x.CodCentroCusto,
                    ComentarioLancamento = x.ComentarioLancamento,
                    ComentarioSap = x.ComentarioSap,
                    BorderoBeneficiario = x.BorderoBeneficiario,
                    BorderoDoc = x.BorderoDoc,
                    BorderoBanco = x.BorderoBanco,
                    BorderoBancoDv = x.BorderoBancoDv,
                    BorderoAgencia = x.BorderoAgencia,
                    BorderoAgenciaDv = x.BorderoAgenciaDv,
                    BorderoCc = x.BorderoCc,
                    BorderoCcDv = x.BorderoCcDv,
                    BorderoValor = x.BorderoValor,
                    BorderoCidade = x.BorderoCidade,
                    BorderoHistorico = x.BorderoHistorico,
                    ValorTotal = x.ValorTotal,
                    CodigoCredor = x.CodigoCredor,
                    NomeCredor = x.NomeCredor,
                    ClasseCredito = x.ClasseCredito,
                    DocCredor = x.DocCredor,
                    StatusProcesso = _context.Processo.FirstOrDefault(y => y.CodProcesso == x.CodProcesso).IndProcessoAtivo != "S" ? "INATIVO" : "ATIVO",
                    cargaCompromissoParcela = x.CargaCompromissoParcela.Select(p => new CargaDeCompromissoParcelaDTO
                    {
                        Id = p.Id,
                        IdCompromisso = p.IdCompromisso,
                        SeqLancamento = p.SeqLancamento,
                        NroParcela = p.NroParcela,
                        Valor = p.Valor,
                        Vencimento = p.Vencimento,
                        Status = p.Status,
                        MotivoExclusao = p.MotivoExclusao,
                        Deletado = p.Deletado,
                        DeletadoDatahora = p.DeletadoDatahora,
                        DeletadoLogin = p.DeletadoLogin,
                        CodLancamento = p.CodLancamento,
                        CodProcesso = p.CodProcesso,
                        MotivoCancelamento = p.MotivoCancelamento,
                        ComentarioCancelamento = p.ComentarioCancelamento,
                        ComentarioEstorno = p.ComentarioEstorno,
                        UsrSolicCancelamento = p.UsrSolicCancelamento,
                        DataSolicCancelamento = p.DataSolicCancelamento,
                        NumeroPedidoSAP = p.Cod != null ? p.Cod.NroPedidoSap : null
                    }).ToList(),

                }).ToList();



                return (retorno, retorno.Count(),string.Empty);
            }
            catch (Exception e)
            {
                return (null, 0, e.StackTrace); 
            }


        }

        [HttpGet("ObterAgendamentosExportacao")]
        public async Task<IActionResult> ListarAgendamentosExportacao(CancellationToken ct, 
                                                     [FromQuery] string? pesquisa,
                                                     [FromQuery] int? tipoPesquisa,
                                                     [FromQuery] int? campoPesquisa,
                                                     [FromQuery] DateTime? solicitacaoDe,
                                                     [FromQuery] DateTime? solicitacaoAte)
        {

            var retorno = _context.AgendExpCargaComp.AsQueryable();

            if (solicitacaoDe.HasValue && solicitacaoAte.HasValue)
            {
                var ate = new DateTime(solicitacaoAte.GetValueOrDefault().Year, solicitacaoAte.GetValueOrDefault().Month, solicitacaoAte.GetValueOrDefault().Day, 23, 59, 59);
                retorno = retorno.Where(x => x.DatSolicitacao >= solicitacaoDe && x.DatSolicitacao <= ate);
            }

            if (!String.IsNullOrEmpty(pesquisa))
            {
                if (campoPesquisa == 1)
                {
                    retorno = tipoPesquisa == 1 ? retorno.Where(x => x.UsrSolicitacao.ToUpper() == pesquisa.ToUpper()) : retorno.Where(x => x.UsrSolicitacao.ToUpper().Contains(pesquisa.ToUpper()));
                }
                else
                {
                    retorno = tipoPesquisa == 1 ? retorno.Where(x => x.UsrSolicitacao.ToUpper() == pesquisa.ToUpper()) : retorno.Where(x => x.UsrSolicitacao.ToUpper().Contains(pesquisa.ToUpper()));
                }
            }
            var total = retorno.Count();
            return Ok(retorno.OrderByDescending(x => x.DatSolicitacao).Take(15).ToList());
        }

        [HttpGet("Novo")]
        public async Task<IActionResult> NovoAgendamento(CancellationToken ct,
                                                           [FromQuery] int? tipoProcesso,
                                                           [FromQuery] int? condicaoProcesso,
                                                           [FromQuery] string? codigoProcesso,
                                                           [FromQuery] int? statusParcela,
                                                           [FromQuery] DateTime? vencimentoDe,
                                                           [FromQuery] DateTime? vencimentoAte,
                                                           [FromQuery] string? nome,
                                                           [FromQuery] string? documento,
                                                           [FromQuery] int? carga,
                                                           [FromQuery] int? compromisso,
                                                           [FromQuery] int? tipoPesquisa,
                                                           [FromQuery] int? classeCredito)
        {
            try
            {
                AgendExpCargaComp novo = new();

                novo.TipoProcesso = tipoProcesso;
                novo.DatSolicitacao = DateTime.Now;
                novo.Status = 1;//Agendado
                novo.StatusParcela = statusParcela;
                novo.TpPesqProcesso = condicaoProcesso;
                novo.TpPesqPartCred = tipoPesquisa;
                novo.CargaId = carga;
                novo.AgendamentoId = compromisso;
                novo.UsrSolicitacao = User.Identity!.Name;
                novo.VencimentoIni = vencimentoDe;
                novo.VencimentoFim = vencimentoAte;
                novo.NroProcesso = codigoProcesso;
                novo.Nome = nome;
                novo.Documento = documento;
                novo.ClasseCredito = classeCredito;

                _context.Add(novo);
                _context.SaveChanges();

                return Ok(novo);
            }
            catch (Exception e)
            {
                BadRequest(e);                
            }
            return BadRequest();
        }

        [HttpGet("obter-classecredito")]
        public async Task<IActionResult> ObterClasseCredito([FromServices] AgendamentoCargaCompromissoService service, CancellationToken ct)
        {
            try
            {
                var lista = await _context.ClasseCredito.ToListAsync();

                if (lista == null)
                {
                    return BadRequest("Falha no carregamento.");
                }

                var retorno = lista.Select(x => new RetornoComboDTO
                {
                    Id = x.CodClasseCredito,
                    Descricao = x.DscClasseCredito
                });

                return Ok(retorno);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("download/{codAgendamento}")]
        [AllowAnonymous]
        public async Task<IActionResult> ObterArquivoAsync(CancellationToken ct, int codAgendamento)
        {
            try
            {
                var agendamento = _context.AgendExpCargaComp.FirstOrDefault(x => x.Id == codAgendamento);
                var caminhoNas = await _parametroJuridico.TratarCaminhoDinamicoArrayAsync("DIR_NAS_EXPOR_COMPROM_RJ");
                
                foreach (var arquivo in caminhoNas)
                {
                    var caminhoArquivo = Path.Combine(arquivo, agendamento.NomArqGerado);
                    if (System.IO.File.Exists(caminhoArquivo))
                    {
                        var dados = await System.IO.File.ReadAllBytesAsync(caminhoArquivo);
                        return File(dados, "application/zip", agendamento.NomArqGerado);
                    }
                }

                return BadRequest("Arquivo não encontrado.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
