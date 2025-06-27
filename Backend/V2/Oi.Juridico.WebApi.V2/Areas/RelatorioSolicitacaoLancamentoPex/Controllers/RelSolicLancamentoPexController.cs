using Oi.Juridico.AddOn.Extensions.IEnumerable;
using Oi.Juridico.Contextos.V2.ParametroJuridicoContext.Data;
using Oi.Juridico.Contextos.V2.RelatorioSolicitacaoLancamentoPexContext.Data;
using Oi.Juridico.Contextos.V2.RelatorioSolicitacaoLancamentoPexContext.Entities;
using Oi.Juridico.Shared.V2.Tools;
using Oi.Juridico.WebApi.V2.Areas.RelatorioSolicitacaoLancamentoPex.DTOs;
using Oi.Juridico.WebApi.V2.Areas.RelatorioSolicitacaoLancamentoPex.DTOs.CsvHelperMap;
using Oi.Juridico.WebApi.V2.Areas.RelatorioSolicitacaoLancamentoPex.Enums;
using Oi.Juridico.WebApi.V2.Areas.RelatorioSolicitacaoLancamentoPex.Model;
using System.IO;
using System.Threading;

namespace Oi.Juridico.WebApi.V2.Areas.RelatorioSolicitacaoLancamentoPex.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RelSolicLancamentoPex : ControllerBase
    {

        private RelatorioSolicitacaoLancamentoPexContext _relSolicitacaoLancamentoPexContext;
        private ParametroJuridicoContext _parametroJuridicoContex;

        public RelSolicLancamentoPex(RelatorioSolicitacaoLancamentoPexContext relSolicitacaoLancamentoPexContext,
                                     ParametroJuridicoContext parametroJuridicoContex)
        {
            _relSolicitacaoLancamentoPexContext = relSolicitacaoLancamentoPexContext;
            _parametroJuridicoContex = parametroJuridicoContex;
        }

        [HttpGet("listaColunasSelecionaveis")]
        public async Task<IActionResult> ListarColunasRelatorioAsync(CancellationToken ct)
        {
            var data = await _relSolicitacaoLancamentoPexContext.ColunasPexRelSolic
                .AsNoTracking()
                .OrderBy(x => x.IdColRelSolic)
                .Select(x => new ListarColunasRelatorioResponse
                {
                    IdentificacaoColuna = x.IdColRelSolic,
                    NomeColuna = x.NomColRelSolic
                })
                .ToListAsync(ct);

            return Ok(new { query = data });
        }

        [HttpGet("listaEscritoriosSelecionaveis")]
        public async Task<IActionResult> ListarEscritoriosAsync(CancellationToken ct)
        {
            var data = await _relSolicitacaoLancamentoPexContext.Profissional
                .AsNoTracking()
                .Where(x => x.IndEscritorio == "S" && x.IndPex == "S")
                .OrderBy(x => x.NomProfissional)
                .Select(x => new ListarEscritoriosResponse
                {
                    Codigo = x.CodProfissional,
                    Nome = x.NomProfissional
                })
                .ToListAsync(ct);

            return Ok(new { query = data });
        }

        [HttpGet("listaEstadosSelecionaveis")]
        public async Task<IActionResult> ListarEstadosAsync(CancellationToken ct)
        {
            var data = await _relSolicitacaoLancamentoPexContext.Estado
                .AsNoTracking()
                .OrderBy(x => x.CodEstado)
                .Select(x => new ListarEstadosResponse
                {
                    Uf = x.CodEstado,
                    NomeUf = x.NomEstado
                })
                .ToListAsync(ct);

            return Ok(new { query = data });
        }

        [HttpGet("listaTipoLancamentoSelecionaveis")]
        public IActionResult ListarTipoLancamento()
        {

            var data = Enum<TipoSolicitacaoLancamento>.GetAllValuesAsIEnumerable().
                                                 Select(d => new ListarTipoLancamentoResponse(d)).
                                                 OrderBy(x => x.Nome);

            return Ok(new { query =  data});
        }

        [HttpGet("listaTipoSolicitacaoLancamentoSelecionaveis")]
        public IActionResult ListarTipoSolicitacaoLancamento()
        {
            var listaTipoSolicLancamento = Enum<TipoSolicitacaoLancamento>.GetAllValuesAsIEnumerable().
                                                Select(d => new ListaTipoSolicLancamentoResponse(d));


            return Ok(new { listaTipoSolicLancamento });
        }

        [HttpGet("listaStatusSolicitacaoLancamentoSelecionaveis")]
        public IActionResult ListaStatusSolicitacaoLancamento()
        {
            var listaStatusSolicitacaoLancamento = Enum<StatusSolicitacaoLancamento>.GetAllValuesAsIEnumerable().
                                                Select(d => new ListarStatusSolicitacaoLancamentoReponse(d)).
                                                OrderBy(x => x.Nome);


            return Ok(new { listaStatusSolicitacaoLancamento });
        }

        [HttpGet("listaHistoricoExecucaoAgendamento/{id}")]
        public IActionResult ListarHistorico(int id)
        {
            var agendamentos = new List<AgendamentoRelatorioSolicitacoes>();

            _relSolicitacaoLancamentoPexContext.AgPexRelSolicHist
                .AsNoTracking()
                .Where(x => x.AgPexRelSolicId == (decimal)id)
                .ToList().ForEach(agendamento =>
                {
                    agendamentos.Add(new AgendamentoRelatorioSolicitacoes(agendamento));
                });

            return Ok(agendamentos);

        }

        [HttpGet("DownloadListaAgendamento")]
        public async Task<IActionResult> DownloadListaAgendAsync(CancellationToken ct)
        {
            var lista = await _relSolicitacaoLancamentoPexContext.AgPexRelSolic
                .AsNoTracking()
                .ToListAsync(ct);

            var agendamentos = lista
                .Select(x => new DownloadListaAgendamentosResponse
                {
                    Id = x.Id,
                    NomAgendamento = x.NomAgendamento,
                    DscAgendamento = x.DscAgendamento,
                    DatCriacao = x.DatCriacao,
                    UsrCodUsuario = x.UsrCodUsuario,
                    DatUltExecucao = x.DatUltExecucao,
                    DatUltAlteracao = x.DatUltAlteracao,
                    Duracao = x.DatFimExecucao.HasValue == false || x.DatInicioExecucao.HasValue == false
                    ? new TimeSpan(0)
                    : x.DatFimExecucao.Value.Subtract(x.DatInicioExecucao.Value),
                    DataProxExecucao = x.DataProxExecucao,
                    IdTipAgendamento = x.TipAgendamento,
                    TipoAgendamento = ((TipoExecucaoRelatorio)x.TipAgendamento).ToDescription()
                }).OrderBy(x => x.Id);

            var array = agendamentos.ToCsvByteArray(typeof(DownloadListaAgendamentosResponseMap));

            var content = new MemoryStream(array);
            var contentType = "APPLICATION/octet-stream";
            var fileName = "ListaAgendamento.csv";
            return File(content, contentType, fileName);
        }

        [HttpGet("DownloadArquivoSolicitacao/{arquivo}")]
        public IActionResult DownloadArquivo(string arquivo = "" )
        {
            if (arquivo == "") return BadRequest("Nome Arquivo Inválido");

            //var caminhoDownload = @"C:\juridico";

            var caminhoDownload = _parametroJuridicoContex.ParametroJuridico
                                  .AsNoTracking()
                                  .Where(x => x.CodParametro == "DIR_NAS_EXEC_REL_SOL")
                                  .Select(x => x.DscConteudoParametro)
                                  .FirstOrDefault();

            if (caminhoDownload == null ) return BadRequest("Caminho do Arquivo não existe.");

            var filePath = Path.Combine(caminhoDownload, arquivo);

            if (!System.IO.File.Exists(filePath)) return BadRequest("Arquivo não Existe");

            var arquivoBytesZip = System.IO.File.ReadAllBytes(filePath);

            return File(arquivoBytesZip, "application/zip", arquivo);

        }

        [HttpPost("SalvarAgendamentoRelSolicitacao")]
        public IActionResult Incluir([FromBody] AgendamentoRelatorioSolicitacoes model, CancellationToken ct)
        {
            try
            {
                var usuario = User.Identity!.Name!;

                //Salva Agendamento
                var agendamento = model.AdicionarAgendamento(_relSolicitacaoLancamentoPexContext.AgPexRelSolic
                                                              .AsNoTracking()
                                                              .Where(x => x.NomAgendamento.Trim().ToUpper() == model.NomeDoRelatorio.Trim().ToUpper()),
                                                               usuario);

                if (!model.ErroAoAdicionar())
                {
                    model.AplicaRegraProximaExecucaoPorTipoAgendamento(agendamento);

                    _relSolicitacaoLancamentoPexContext.AgPexRelSolic.Add(agendamento);
                    _relSolicitacaoLancamentoPexContext.SaveChanges();
                }
                else
                    return BadRequest(model.RetornaErro());

                //Salva Datas Preenchidas
                var datas = model.AdicionarDatasRel(agendamento.Id);

                if (!model.ErroAoAdicionar())
                {
                    _relSolicitacaoLancamentoPexContext.AgPexRelSolicData.Add(datas);
                }
                else
                    return BadRequest(model.RetornaErro());

                //Salva colunas selecionadas
                var listaColunas = model.AdicionarListasColunasRel(agendamento.Id);

                if (!model.ErroAoAdicionar())
                {
                    if (model.ListaColunasTemItens(listaColunas))  _relSolicitacaoLancamentoPexContext.
                                                                   AgPexRelSolicColunas.
                                                                   AddRange(listaColunas);
                }
                else
                    return BadRequest(model.RetornaErro());


                //Salva Escritorios selecioandos
                var listaEscritorios = model.AdicionarListasEscritoriosRel(agendamento.Id);

                if (!model.ErroAoAdicionar())
                {
                    if (model.ListaEscritoriosTemItens(listaEscritorios))  _relSolicitacaoLancamentoPexContext.
                                                                           AgPexRelSolicEscrit.
                                                                           AddRange(listaEscritorios);
                }
                else
                    return BadRequest(model.RetornaErro());


                //Salva Sattus Solicitacao
                var listaStatus = model.AdicionarStatusSolcitacaoRel(agendamento.Id);

                if (!model.ErroAoAdicionar())
                {
                    if (model.ListaStatusTemItens(listaStatus))   _relSolicitacaoLancamentoPexContext.
                                                                  AgPexRelSolicStatus.
                                                                  AddRange(listaStatus);

                }
                else
                    return BadRequest(model.RetornaErro());

                //Salva Tipos de Lancamentos
                var listaTipoLancamento = model.AdicionarTipoLancamentoRel(agendamento.Id);

                if (!model.ErroAoAdicionar())
                {
                    if (model.ListaTipoLancamentoTemItens(listaTipoLancamento))  _relSolicitacaoLancamentoPexContext.
                                                                                  AgPexRelSolicTpLan.
                                                                                  AddRange(listaTipoLancamento);
                }
                else
                    return BadRequest(model.RetornaErro());

                //Salva Status Solicitacao selecioandos
                var listaUf = model.AdicionarUfRel(agendamento.Id);

                if (!model.ErroAoAdicionar())
                {
                    if (model.ListaUfTemItens(listaUf))   _relSolicitacaoLancamentoPexContext.
                                                           AgPexRelSolicUf.
                                                           AddRange(listaUf);

                }
                else
                    return BadRequest(model.RetornaErro());

                _relSolicitacaoLancamentoPexContext.SaveChanges();


                return Ok("Agendamento Realizado com Sucesso.");

            }
            catch (Exception ex)
            {
                var mensagem = ex.InnerException != null ? ex.InnerException.Message : ex.Message;

                return BadRequest(mensagem);
            }
        }

        [HttpPost("AlterarAgendamentoRelSolicitacao")]
        public IActionResult Alterar([FromBody] AgendamentoRelatorioSolicitacoes model)
        {
            var usuario = User.Identity!.Name!;
            AgPexRelSolic? agend = new();

            try
            {

                if (model.VerificaValores(_relSolicitacaoLancamentoPexContext.AgPexRelSolic
                                          .AsNoTracking()
                                          .Where(x => x.NomAgendamento.Trim().ToUpper() == model.NomeDoRelatorio.Trim().ToUpper() &&
                                                 x.Id != decimal.Parse(model.IdAgendamento))))
                {
                     agend = _relSolicitacaoLancamentoPexContext.AgPexRelSolic
                             .AsNoTracking()
                             .Where(x => x.Id == int.Parse(model.IdAgendamento)).FirstOrDefault();

                    if (agend != null)
                    {
                        agend.NomAgendamento = model.NomeDoRelatorio.Trim();
                        agend.DscAgendamento = model.Descricao != null ? model.Descricao.Trim() : null ;
                        agend.TipAgendamento = short.Parse(model.TipoExecucao);
                        agend.NumDiaAgendamento = short.Parse( model.DiaSemana);
                        agend.DatAgendamentoIni = string.IsNullOrEmpty(model.DataIniAgendamento) ? null : DateTime.Parse(model.DataIniAgendamento);
                        agend.DatAgendamentoFim = string.IsNullOrEmpty(model.DataFimAgendamento) ? null : DateTime.Parse(model.DataFimAgendamento);
                        agend.IndExecutarDiaUtil = model.TrataValorCampoSomenteEmDiasUteis();
                        agend.NumMesAgendamento = string.IsNullOrEmpty(model.DiaMes) ? null : short.Parse(model.DiaMes);
                        agend.DatUltAlteracao = DateTime.Now;
                        agend.DatInicioExecucao = null;


                        agend.DatFimExecucao = null;
                        agend.UsrCodUsuario = usuario;
                        agend.StatusExecucao = 0;

                        agend = model.AplicaRegraProximaExecucaoPorTipoAgendamento(agend);
                        
                        if (!string.IsNullOrEmpty(model.DataProxExecucao)) 
                            agend.DataProxExecucao = DateTime.Parse(model.DataProxExecucao); 
                        
                        _relSolicitacaoLancamentoPexContext.AgPexRelSolic.Update(agend);
                        _relSolicitacaoLancamentoPexContext.SaveChanges();

                    }
                    else return BadRequest("Agendamento não existe");

                    
                    return Ok(new { resposta = new AgendamentoRelatorioSolicitacoes(agend) });

                }
                else
                    return BadRequest(model.RetornaErro());

            }
            catch (Exception ex)
            {
                var mensagem = ex.InnerException != null ? ex.InnerException.Message : ex.Message;

                return BadRequest(mensagem);
            }
        }

        [HttpGet("Listar")]
        public IActionResult Listar(string tipoPesquisa = "JaExecutados", 
                                    string? pesquisa = "", 
                                    string tipoOrdenacao = "DataExecucao",
                                    string? dtIni = "",
                                    string? dtFim = "",
                                    int skip = 0)
        {
            var agendamentos = new List<AgendamentoRelatorioSolicitacoes>();
            DateTime dIni = DateTime.MaxValue;
            DateTime dFim = DateTime.MaxValue;
            string pesq = pesquisa + " ";


            if (!DateTime.TryParse(dtIni, out dIni))
                if (dtIni == "")
                    dIni = DateTime.MinValue.AddDays(1);
                else dIni = DateTime.MaxValue.AddDays(-1);

            if (!DateTime.TryParse(dtFim, out dFim)) dFim = DateTime.MaxValue.AddDays(-1);


            IQueryable<AgPexRelSolic> query = tipoOrdenacao switch
            {
                "DataExecucao" => _relSolicitacaoLancamentoPexContext.AgPexRelSolic
                                  .OrderByDescending(m => m.DatFimExecucao),

                "Relatorio"    => _relSolicitacaoLancamentoPexContext.AgPexRelSolic
                                  .OrderByDescending(m => m.NomAgendamento.Trim().ToLower()),

                        _      => _relSolicitacaoLancamentoPexContext.AgPexRelSolic
                                  .OrderBy(m => m.NomAgendamento.Trim().ToLower()),
            };

            query = tipoPesquisa switch
            {
                "JaExecutados"          => query.Where(m => m.DatUltExecucao <= DateTime.Now),

                "ExecucoesFuturas"      => query.Where(m => m.DataProxExecucao.Date >= DateTime.Now.Date && m.DatFimExecucao == null),

                "MostarApenasNoPeriodo" => query.Where(m => m.DatUltExecucao >= dIni.AddDays(-1) && m.DatUltExecucao <= dFim.AddDays(1)),
            
            _ => query,

            };
            if (!pesq.Trim().Equals(string.Empty))
                query = query.Where(m => m.NomAgendamento.ToLower().Trim().Contains(pesq.Trim().ToLower()));

            //query = query.Where(m => m.NomAgendamento.ToLower().Trim().Contains(pesq.Trim().ToLower()));

            //query = query.Skip(skip).Take(10);


            query.ToList().ForEach(agendamento =>
            {
                agendamentos.Add(new AgendamentoRelatorioSolicitacoes(agendamento));
            });

            return Ok(agendamentos);
        }

    }
}
