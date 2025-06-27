using Oi.Juridico.Contextos.V2.ParametroJuridicoContext.Data;
using Oi.Juridico.Contextos.V2.PautaJuizadoComposicaoContext.Data;
using Oi.Juridico.Contextos.V2.PautaJuizadoComposicaoContext.Entities;
using Oi.Juridico.WebApi.V2.Areas.PautaJuizado.DTOs;
using Oi.Juridico.WebApi.V2.Areas.PautaJuizado.Models;
using Oi.Juridico.WebApi.V2.Services;

namespace Oi.Juridico.WebApi.V2.Areas.PautaJuizado.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PautaJuizadoComposicao : ControllerBase
    {
        private PautaJuizadoComposicaoContext _pautaJuizadoComposicaoContext;
        private ParametroJuridicoContext _parametroJuridicoContext;
        private EmailSender _emailSender;

        public PautaJuizadoComposicao(PautaJuizadoComposicaoContext pautaJuizadoComposicaoContext, ParametroJuridicoContext parametroJuridicoContext, EmailSender emailSender)
        {
            _pautaJuizadoComposicaoContext = pautaJuizadoComposicaoContext;
            _parametroJuridicoContext = parametroJuridicoContext;
            _emailSender = emailSender;
        }

        [HttpPost("ListarPautaComposicao")]
        public IActionResult ListarPautaComposicaoAsync([FromBody] ListarPautaComposicao model, CancellationToken ct)
        {
            List<ListarPautaComposicaoResponse> lstPauta = new List<ListarPautaComposicaoResponse>();

            using (var command = _pautaJuizadoComposicaoContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = model.PreparaListaPautaParaSql(model, User.Identity!.Name!);

                _pautaJuizadoComposicaoContext.Database.OpenConnection();

                using (var reader = command.ExecuteReader())
                    lstPauta = model.DadosListaPauta(reader, model);
            }

            _pautaJuizadoComposicaoContext.Database.CloseConnection();

            return Ok(new
            {
                data = lstPauta.Skip(model.NumeroPagina - 1).Take(model.QuantidadePorPagina),
                total = lstPauta.Count()
            });
        }

        [HttpPost("ListarPautaComposicaoAudiencia")]
        public IActionResult ListarPautaComposicaoAudienciaAsync([FromBody] ListarPautaComposicaoAudiencia model, CancellationToken ct)
        {
            List<ListarPautaComposicaoAudienciaResponse> lstAudiencia = new List<ListarPautaComposicaoAudienciaResponse>();

            using (var command = _pautaJuizadoComposicaoContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = model.PreparaListaAudienciaParaSql(model, User.Identity!.Name!);

                _pautaJuizadoComposicaoContext.Database.OpenConnection();

                using (var reader = command.ExecuteReader())
                    lstAudiencia = model.DadosListaAudiencia(reader, model);
            }

            lstAudiencia = ObterPedidosPorProcesso(lstAudiencia);

            _pautaJuizadoComposicaoContext.Database.CloseConnection();

            return Ok(new
            {
                data = lstAudiencia,
                total = lstAudiencia.Count()
            });
        }



        [HttpPost("AlterarAlocacaoTodos")]
        public IActionResult AlterarTipoAlocacaoPaginaAsync([FromBody] ListarPautaComposicao model, CancellationToken ct)
        {
            var lista = ObterListaPautaComposicaoBase(model, ct);
            _pautaJuizadoComposicaoContext.Database.BeginTransaction();

            try
            {
                ListarPautaComposicaoAudiencia filtro = new ListarPautaComposicaoAudiencia();
                List<ListarPautaComposicaoAudienciaResponse> lstAudiencia;
                string[] listaParte;

                foreach (ListarPautaComposicaoResponse itemPauta in lista)
                {
                    filtro.CodVara = itemPauta.CodVara;
                    filtro.CodTipoVara = itemPauta.CodTipoVara;
                    filtro.CodComarca = itemPauta.CodComarca;
                    filtro.CodStatusAudiencia = model.CodStatusAudiencia;
                    filtro.CodTipoVara = itemPauta.CodTipoVara;
                    filtro.CodVara = itemPauta.CodVara;
                    filtro.DataAudiencia = itemPauta.Data;
                    filtro.SituacaoProcesso = model.SituacaoProcesso;


                    lstAudiencia = new List<ListarPautaComposicaoAudienciaResponse>();

                    using (var command = _pautaJuizadoComposicaoContext.Database.GetDbConnection().CreateCommand())
                    {
                        command.CommandText = filtro.PreparaListaAudienciaParaSql(filtro, User.Identity!.Name!);

                        _pautaJuizadoComposicaoContext.Database.OpenConnection();

                        using (var reader = command.ExecuteReader())
                            lstAudiencia = filtro.DadosListaAudiencia(reader, filtro);
                    }



                    //var sql = "";

                    foreach (ListarPautaComposicaoAudienciaResponse item in lstAudiencia)
                    {
                        //  sql = "";
                        //  sql = "UPDATE JUR.AUDIENCIA_PROCESSO  SET COD_ALOCACAO_PREPOSTO = " + model.AlocacaoTipo.ToString() + ",IND_TERCEIRIZADO = " + (model.AlocacaoTipo.ToString() == "3" ? "'S'" : "'N'") + " WHERE ";
                        //  sql += "COD_PROCESSO = " + item.CodProcesso + " AND SEQ_AUDIENCIA = " + item.SeqAudiencia;
                        //  _pautaJuizadoComposicaoContext.Database.ExecuteSqlRaw(sql);

                        var audiencia = _pautaJuizadoComposicaoContext.AudienciaProcesso.FirstOrDefault(x => x.CodProcesso == item.CodProcesso && x.SeqAudiencia == item.SeqAudiencia);

                        if (audiencia!.CodAlocacaoPreposto != model.AlocacaoTipo.ToString())
                        {
                            var htmlText = "";

                            var query = (from p in _pautaJuizadoComposicaoContext.Processo
                                         join prof in _pautaJuizadoComposicaoContext.Profissional
                                         on p.CodProfissional equals prof.CodProfissional
                                         where p.CodProcesso == item.CodProcesso
                                         select new
                                         {
                                             Nome = prof.NomProfissional,
                                             Email = prof.DscEmail
                                         }).FirstOrDefault();

                            if (query != null)
                            {
                                if (audiencia!.CodAlocacaoPreposto != "3" && model.AlocacaoTipo == 3)
                                {
                                    htmlText = $"<p>Prezados, informamos que a audiência do processo <b>{item.CodProcesso}</b> será terceirizada. Solicitamos indicação de preposto pelo escritório.</p>";
                                }
                                else if (audiencia!.CodAlocacaoPreposto == "3" && model.AlocacaoTipo == 2)
                                {
                                    htmlText = $"<p>Prezados, informamos a audiência do processo <b>{item.CodProcesso}</b> será conduzida por um preposto próprio da Oi. O preposto designado para essa audiência será informado no registro da mesma, na aba “Audiências” do SISJUR.</p>";
                                }

                                _emailSender.SendEmailAsync(query!.Email, query!.Nome, "Audiencia Terceirizada", htmlText);
                            }

                            audiencia!.CodAlocacaoPreposto = model.AlocacaoTipo.ToString();
                            audiencia!.IndTerceirizado = model.AlocacaoTipo == 3 ? "S" : "N";
                            audiencia!.DatTerceirizado = DateTime.Now;
                            //  await _pautaJuizadoComposicaoContext.SaveChangesAsync(User.Identity!.Name!, true);
                        }



                        if (model.AlocacaoTipo != 2)
                        {
                            var prepostoLista = _pautaJuizadoComposicaoContext.AlocacaoPreposto.Where(x => x.CodComarca == itemPauta.CodComarca &&
                                                                                                       x.CodVara == itemPauta.CodVara &&
                                                                                                       x.CodTipoVara == itemPauta.CodTipoVara && x.DatAlocacao == DateTime.Parse(itemPauta.Data) &&
                                                                                                       x.CodParteEmpresa == Convert.ToInt64(item.CodParte)).ToList();
                            foreach (var preposto in prepostoLista)
                            {
                                _pautaJuizadoComposicaoContext.AlocacaoPreposto.Remove(preposto);
                            }

                        }

                    }


                }


                _pautaJuizadoComposicaoContext.Database.CommitTransaction();
                _pautaJuizadoComposicaoContext.SaveChanges(User.Identity!.Name!, true);
                _pautaJuizadoComposicaoContext.Database.CloseConnection();
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }


        }



        private List<ListarPautaComposicaoResponse> ObterListaPautaComposicaoBase(ListarPautaComposicao model, CancellationToken ct)
        {
            List<ListarPautaComposicaoResponse> lstPauta = new List<ListarPautaComposicaoResponse>();

            using (var command = _pautaJuizadoComposicaoContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = model.PreparaListaPautaParaSql(model, User.Identity!.Name!);

                _pautaJuizadoComposicaoContext.Database.OpenConnection();

                using (var reader = command.ExecuteReader())
                    lstPauta = model.DadosListaPauta(reader, model);
            }

            _pautaJuizadoComposicaoContext.Database.CloseConnection();

            return lstPauta;
        }





        [HttpPost("ListarPrepostosNaoAlocados")]
        public IActionResult PrepostosNaoAlocadosAsync([FromBody] ListarPautaComposicaoPreposto model, CancellationToken ct)
        {
            List<ListarPrepostoNaoAlocadoResponse> lstPreposto = new List<ListarPrepostoNaoAlocadoResponse>();

            using (var command = _pautaJuizadoComposicaoContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = model.PreparaListaPrepostoNaoAlocadoParaSql(model);

                _pautaJuizadoComposicaoContext.Database.OpenConnection();

                using (var reader = command.ExecuteReader())
                    lstPreposto = model.DadosListaNaoAlocadoPreposto(reader, model);
            }

            _pautaJuizadoComposicaoContext.Database.CloseConnection();

            return Ok(new
            {
                data = lstPreposto,
                total = lstPreposto.Count()
            });
        }

        [HttpPost("ListarPrepostosAlocados")]
        public IActionResult PrepostosAlocadosAsync([FromBody] ListarPautaComposicaoPreposto model, CancellationToken ct)
        {
            List<ListarPrepostoAlocadoResponse> lstPreposto = new List<ListarPrepostoAlocadoResponse>();

            using (var command = _pautaJuizadoComposicaoContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = model.PreparaListaPrepostoAlocadoParaSql(model);

                _pautaJuizadoComposicaoContext.Database.OpenConnection();

                using (var reader = command.ExecuteReader())
                    lstPreposto = model.DadosListaAlocadoPreposto(reader, model);
            }

            _pautaJuizadoComposicaoContext.Database.CloseConnection();

            return Ok(new
            {
                data = lstPreposto,
                total = lstPreposto.Count()
            });
        }

        [HttpPut("SalvarAudiencia")]
        public async Task<IActionResult> SalvarAudienciaAsync([FromBody] SalvarAudiencia[] model, CancellationToken ct)
        {
            try
            {
                foreach (SalvarAudiencia processo in model)
                {
                    var audiencia = await _pautaJuizadoComposicaoContext.AudienciaProcesso.FirstOrDefaultAsync(x => x.CodProcesso == processo.CodProcesso && x.SeqAudiencia == processo.SeqAudiencia);

                    if (audiencia!.CodAlocacaoPreposto != processo.AlocacaoTipo.ToString())
                    {
                        var htmlText = "";

                        var query = await (from p in _pautaJuizadoComposicaoContext.Processo
                                           join prof in _pautaJuizadoComposicaoContext.Profissional
                                           on p.CodProfissional equals prof.CodProfissional
                                           where p.CodProcesso == processo.CodProcesso
                                           select new
                                           {
                                               Nome = prof.NomProfissional,
                                               Email = prof.DscEmail
                                           }).FirstOrDefaultAsync(ct);

                        if (query != null)
                        {
                            if (audiencia!.IndTerceirizado == "N" && processo.Terceirizado == "S")
                            {
                                htmlText = $"<p>Prezados, informamos que a audiência do processo <b>{processo.CodProcesso}</b> será terceirizada. Solicitamos indicação de preposto pelo escritório.</p>";
                            }
                            else if (audiencia!.IndTerceirizado == "S" && processo.Terceirizado == "N")
                            {
                                htmlText = $"<p>Prezados, informamos a audiência do processo <b>{processo.CodProcesso}</b> será conduzida por um preposto próprio da Oi. O preposto designado para essa audiência será informado no registro da mesma, na aba “Audiências” do SISJUR.</p>";
                            }

                            await _emailSender.SendEmailAsync(query!.Email, query!.Nome, "Audiencia Terceirizada", htmlText);
                        }

                        audiencia!.CodAlocacaoPreposto = processo.AlocacaoTipo.ToString();
                        audiencia!.IndTerceirizado = processo.AlocacaoTipo == 3 ? "S" : "N";
                        audiencia!.DatTerceirizado = DateTime.Now;

                        await _pautaJuizadoComposicaoContext.SaveChangesAsync(User.Identity!.Name!, true);
                    }
                }

                return Ok();
            }
            catch (Exception ex)
            {
                var mensagem = ex.InnerException != null ? ex.InnerException.Message : ex.Message;

                return BadRequest(mensagem);
            }
        }

        [HttpPost("SalvarPautaComposicao")]
        public async Task<IActionResult> SalvarAsync([FromBody] SalvarPauta model, CancellationToken ct)
        {
            try
            {
                foreach (string codParte in model.PreparaEmpresasGrupo(model.CodParteEmpresa))
                {
                    var prepostoLista = await _pautaJuizadoComposicaoContext.AlocacaoPreposto.Where(x => x.CodComarca == model.CodComarca &&
                                                                                                         x.CodVara == model.CodVara &&
                                                                                                         x.CodTipoVara == model.CodTipoVara && x.DatAlocacao == DateTime.Parse(model.DataAudiencia) &&
                                                                                                         x.CodParteEmpresa == Convert.ToInt64(codParte)).ToListAsync(ct);
                    foreach (var preposto in prepostoLista)
                    {
                        _pautaJuizadoComposicaoContext.AlocacaoPreposto.Remove(preposto);
                    }
                }

                await _pautaJuizadoComposicaoContext.SaveChangesAsync(User.Identity!.Name!, true);

                AdicionaPrepostos(model);



                return Ok();
            }
            catch (Exception ex)
            {
                var mensagem = ex.InnerException != null ? ex.InnerException.Message : ex.Message;

                return BadRequest(mensagem);
            }
        }

        private void AdicionaPrepostos(SalvarPauta model)
        {
            foreach (string codParte in model.PreparaEmpresasGrupo(model.CodParteEmpresa))
            {
                var prepostoJaAdicionado = _pautaJuizadoComposicaoContext.AlocacaoPreposto.Where(x => x.CodComarca == model.CodComarca &&
                                                                                                        x.CodVara == model.CodVara &&
                                                                                                        x.CodTipoVara == model.CodTipoVara && x.DatAlocacao == DateTime.Parse(model.DataAudiencia) &&
                                                                                                        x.CodParteEmpresa == Convert.ToInt64(codParte)).ToList();
                if (prepostoJaAdicionado.Count == 0)
                {
                    foreach (var prep in model.CodPreposto)
                    {
                        AlocacaoPreposto alocacaoPreposto = new AlocacaoPreposto
                        {
                            CodParteEmpresa = Convert.ToInt32(codParte),
                            CodComarca = model.CodComarca,
                            CodVara = model.CodVara,
                            CodTipoVara = model.CodTipoVara,
                            CodPreposto = prep,
                            DatAlocacao = Convert.ToDateTime(model.DataAudiencia),
                            IndPrincipal = model.FormataIndicaPrepostoPrincipal(prep, model.CodPrepostoPrincipal)
                        };

                        _pautaJuizadoComposicaoContext.AlocacaoPreposto.Add(alocacaoPreposto);

                    }
                    _pautaJuizadoComposicaoContext.SaveChanges(User.Identity!.Name!, true);
                }
            }
        }

        private List<ListarPautaComposicaoAudienciaResponse> ObterPedidosPorProcesso(List<ListarPautaComposicaoAudienciaResponse> lstAudiencia)
        {
            foreach (var audiencia in lstAudiencia)
            {
                var queryPedidosProcesso = from pedido in _pautaJuizadoComposicaoContext.Pedido.AsNoTracking()
                                           join pedidoProcesso in _pautaJuizadoComposicaoContext.PedidoProcesso.AsNoTracking()
                                           on pedido.CodPedido equals pedidoProcesso.CodPedido
                                           where pedidoProcesso.CodProcesso == audiencia.CodProcesso
                                           orderby pedido.DscPedido
                                           select new Pedido
                                           {
                                               DscPedido = pedido.DscPedido
                                           };

                var listaDePedidos = queryPedidosProcesso.Select(x => x.DscPedido);

                foreach (var pedido in listaDePedidos)
                {
                    audiencia.Pedido += pedido + "@@@";
                }

                if (audiencia.Pedido[audiencia.Pedido.Length - 1] == '@')
                    audiencia.Pedido = audiencia.Pedido.Remove(audiencia.Pedido.Length - 3);
            }

            return lstAudiencia;
        }














       











    }
}
