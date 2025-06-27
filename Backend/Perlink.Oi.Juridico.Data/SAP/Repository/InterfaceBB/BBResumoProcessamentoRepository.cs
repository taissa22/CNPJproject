using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using Perlink.Oi.Juridico.Data.Compartilhado.Repository;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;
using Perlink.Oi.Juridico.Domain.SAP.DTO.InterfaceBB;
using Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Repository.InterfaceBB;
using Shared.Data;
using Shared.Domain.Interface;
using Shared.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.SAP.Repository.InterfaceBB
{
    public class BBResumoProcessamentoRepository : BaseCrudRepository<BBResumoProcessamento, long>, IBBResumoProcessamentoRepository
    {
        private readonly JuridicoContext dbContext;
        private readonly EmpresaCentralizadoraConvenioRepository EmpresaCentralizadoraConvenioRepository;

        public BBResumoProcessamentoRepository(JuridicoContext dbContext, IAuthenticatedUser user) : base(dbContext, user)
        {
            this.dbContext = dbContext;
            EmpresaCentralizadoraConvenioRepository = new EmpresaCentralizadoraConvenioRepository(dbContext, user)
;        }

        public async Task<ICollection<BBResumoProcessamentoResultadoDTO>> ConsultarArquivoRetorno(BBResumoProcessamentoFiltroDTO filtros)
        {
            IQueryable<BBResumoProcessamento> query = RetornaIQuerybleFiltro(filtros);

            var retorno = await query.Select(remessa => new BBResumoProcessamentoResultadoDTO()
            {
                DataRemessa = remessa.DataRemessa.ToString("dd/MM/yyyy"),
                DataProcessamentoRemessa = remessa.DataProcessamentoRemessa.Value.ToString("dd/MM/yyyy"),
                NumeroLoteBB = remessa.NumeroLoteBB,
                QuantidadeRegistrosArquivo = remessa.QuantidadeRegistrosArquivo,
                QuantidadeRegistrosProcessados = remessa.QuantidadeRegistrosProcessados,
                Status = remessa.BBStatusRemessa.Descricao,
                ValorTotalGuiaProcessada = remessa.ValorTotalGuiaProcessada,
                ValorTotalRemessa = remessa.ValorTotalRemessa,
                TipoProcesso = EnumExtensions.GetDescricaoFromValue<TipoProcessoEnum>(remessa.Lote.CodigoTipoProcesso)
            })
            .Paginar(filtros.Pagina, filtros.Quantidade)
            .ToListAsync();

            return retorno;
        }

        private IQueryable<BBResumoProcessamento> RetornaIQuerybleFiltro(BBResumoProcessamentoFiltroDTO filtros)
        {
            IQueryable<BBResumoProcessamento> query = context.Set<BBResumoProcessamento>();
            var listaProcessos = RetornaListaProcessos(filtros);

            if (filtros.IdsNumerosGuia?.Count() > 0)
                query = query.Where(r => r.Lote.LotesLancamento.Any(ll =>
                                        filtros.IdsNumerosGuia.Contains(ll.LancamentoProcesso.NumeroGuia.Value))
                );

            if (filtros.ValorGuiaInicio.HasValue && filtros.ValorGuiaFim.HasValue)
                query = query.Where(r => r.Lote.LotesLancamento.Any(ll =>
                                        ll.LancamentoProcesso.ValorLancamento >= filtros.ValorGuiaInicio &&
                                        ll.LancamentoProcesso.ValorLancamento <= filtros.ValorGuiaFim)
                );

            if (filtros.NumerosContasJudiciais?.Count() > 0)
                query = query.Where(r => r.Lote.LotesLancamento.Any(ll =>
                                        filtros.NumerosContasJudiciais.Contains(ll.LancamentoProcesso.NumeroContaJudicial.Value))
                );

            if (listaProcessos?.Count() > 0)
                query = query.Where(r => r.Lote.LotesLancamento.Any(ll =>
                                        listaProcessos.Contains(ll.LancamentoProcesso.Id))
                );

            //filtros gerais
            if (filtros.NumeroRemessaMenor.HasValue && filtros.NumeroRemessaMaior.HasValue)
                query = query.Where(r => r.NumeroLoteBB >= filtros.NumeroRemessaMenor &&
                                        r.NumeroLoteBB <= filtros.NumeroRemessaMaior);

            if (filtros.DataRemessaMenor.HasValue && filtros.DataRemessaMaior.HasValue)
                query = query.Where(r => r.DataRemessa >= filtros.DataRemessaMenor &&
                                        r.DataRemessa <= filtros.DataRemessaMaior);
            query = query.OrderBy(r => r.Lote.TipoProcesso.Descricao)
                .ThenBy(r => r.NumeroLoteBB);
            return query;
        }

        public async Task<ICollection<BBResumoProcessamentoGuiaDTO>> BuscarGuiasOK(long numeroLoteBB)
        {
            var sql = @"
                SELECT
                    TO_CHAR(jur.lote_lancamento.cod_processo) CodigoProcesso,
                    TO_CHAR(jur.lote_lancamento.cod_lancamento) CodigoLancamento,
                    TO_CHAR(jur.processo.nro_processo_cartorio) NumeroProcesso,
                    (jur.comarca.cod_estado || ' - ' || jur.comarca.nom_comarca) Comarca,
                    (jur.processo.cod_vara || 'ª ' || jur.tipo_vara.nom_tipo_vara) Juizado,
                    jur.tipo_lancamento.dsc_tipo_lancamento DescricaoTipoLancamento,
                    TO_CHAR(jur.lancamento_processo.dat_lancamento, 'DD/MM/YYYY') DataLancamento,
                    jur.categoria_pagamento.dsc_cat_pagamento DescricaoCategoriaPagamento,
                    jur.lancamento_processo.NRO_GUIA NumeroGuia,
                    jur.lancamento_processo.NRO_PEDIDO_SAP NumeroPedidoSAP,
                    TO_CHAR(jur.lancamento_processo.dat_recebimento_fisico, 'DD/MM/YYYY') DataRecebimentoFiscal,
                    TO_CHAR(jur.lancamento_processo.DAT_PAGAMENTO_PEDIDO, 'DD/MM/YYYY') DataPagamentoPedido,
                    jur.lancamento_processo.val_lancamento ValorLiquido,
                    TO_CHAR(jur.lancamento_processo.DAT_ENVIO_ESCRITORIO, 'DD/MM/YYYY') DataEnvioEscritorio,
                    nvl(jur.profissional.nom_profissional, 'Sem Escritório') as DescricaoEscritorio,
                    jur.status_pagamento.dsc_status_pagamento StatusPagamento,
                    TO_CHAR(jur.lancamento_processo.NUM_CONTA_JUDICIAL) NumeroContaJudicial,
                    jur.lancamento_processo.NUM_PARCELA_CONTA_JUDICIAL NumeroParcelaJudicial,
                    jur.lancamento_processo.COD_AUTENTICACAO_ELETRONICA AutenticacaoEletronica,
                    TO_CHAR(jur.lancamento_processo.DATA_EFETIVACAO_PARCELA, 'DD/MM/YYYY') DataEfetivacaoParcelaBB,
                    jur.bb_status_parcelas.DESCRICAO StatusParcelaBB,
                    nvl(eg.nom_parte, 'Sem Empresa do Grupo') as DescricaoEmpresaGrupo,
                    jur.parte.nom_parte as Autor
                FROM jur.lote_lancamento,
                   jur.lancamento_processo,
                   jur.processo,
                   jur.comarca,
                   jur.tipo_vara,
                   jur.categoria_pagamento,
                   jur.tipo_lancamento,
                   jur.profissional,
                   jur.status_pagamento,
                   jur.parte,
                   jur.bb_status_parcelas,
                   jur.parte eg,
                   jur.lote l
                WHERE (jur.lote_lancamento.cod_lote = l.cod_lote)
                    and (jur.lancamento_processo.cod_processo =
                        jur.lote_lancamento.cod_processo)
                    and (jur.lancamento_processo.cod_lancamento =
                        jur.lote_lancamento.cod_lancamento)
                    and (jur.processo.cod_processo = jur.lancamento_processo.cod_processo)
                    and (jur.comarca.cod_comarca = jur.processo.cod_comarca)
                    and (jur.categoria_pagamento.cod_cat_pagamento =
                        jur.lancamento_processo.cod_cat_pagamento)
                    and (jur.tipo_lancamento.cod_tipo_lancamento =
                        jur.lancamento_processo.cod_tipo_lancamento)
                    and (jur.processo.cod_tipo_vara = jur.tipo_vara.cod_tipo_vara)
                    and (jur.profissional.cod_profissional(+) =
                        jur.processo.cod_profissional)
                    and (jur.status_pagamento.cod_status_pagamento =
                        jur.lancamento_processo.cod_status_pagamento)
                    and (jur.lancamento_processo.cod_parte = jur.parte.cod_parte(+))
                    and (jur.lancamento_processo.bbstp_ID_BB_STATUS_PARCELA =
                        jur.bb_status_parcelas.id(+))
                    and (l.nro_lote_bb = :NUM_LOTE_BB)
                    and (eg.cod_parte(+) = jur.processo.cod_parte_empresa)
                    order by NumeroProcesso
            ";

            var parametros = new List<OracleParameter>()
                {
                    new OracleParameter("NUM_LOTE_BB", numeroLoteBB)
                };
            var resultado = await dbContext.ExecutarQuery(sql, parametros.ToArray());

            if (!string.IsNullOrEmpty(resultado))
            {
                //*** Os alias da query estão acoplados aos nomes das propriedades ***
                //Ao mudar o nome de uma propriedade do DTO deve alterar também o Alias
                return JsonConvert.DeserializeObject<List<BBResumoProcessamentoGuiaDTO>>(resultado);
            }
            return null;
        }

       
        public async Task<BBResumoProcessamentoGuiaExibidaDTO> BuscarGuiaExibicao(long codigoProcesso, long codigoLancamento) {

            var resultado = await dbContext.LancamentoProcessos
                            .Where(lp => lp.Id == codigoProcesso && lp.CodigoLancamento == codigoLancamento)
                            .Select(lp => new BBResumoProcessamentoGuiaExibidaDTO() {
                                CodigoEmpresaCentralizadora = lp.Processo.Parte.EmpresaCentralizadora.Id,
                                CodigoEstado = lp.Processo.Comarca.CodigoEstado,
                                NumeroContaJudicial = lp.NumeroContaJudicial.ToString(),
                                NumeroParcelaJudicial = lp.NumeroParcelaContaJudicial.ToString(),
                                DataRecebimentoFisico = lp.DataRecebimentoFiscal.HasValue ? lp.DataRecebimentoFiscal.Value.ToString("dd/MM/yyyy") : string.Empty,
                                DataGuia = lp.DataGuiaJudicial.HasValue ? lp.DataGuiaJudicial.Value.ToString("dd/MM/yyyy") : lp.DataLancamento.ToString("dd/MM/yyyy"),
                                NumeroGuia = lp.NumeroGuia.HasValue ? lp.NumeroGuia.Value.ToString("0000000") : string.Empty,
                                NumeroProcesso = lp.Processo.NumeroProcessoCartorio,
                                DescricaoTribunalBB = lp.Processo.Vara.BBOrgaos.BBTribunais.Descricao,
                                Comarca = lp.Processo.Comarca.BBComarca.Descricao,
                                DescricaoOrgaoBB = lp.Processo.Vara.BBOrgaos.Nome,
                                DescricaoModalidadeBB = lp.IdBBModalidade.HasValue ? lp.BBModalidade.Descricao.ToString(): string.Empty,
                                Depositante = "RÉU",
                                ValorLiquido = lp.ValorLancamento,
                                DescricaoEmpresaGrupo = lp.Processo.Parte.Nome,
                                TipoReu = "JURÍDICA",
                                CpfCnpjReu = lp.Processo.Parte.Cgc.ToString(),
                                Autor = lp.ParteProcesso.Parte.Nome,
                                TipoAutor = string.IsNullOrEmpty(lp.ParteProcesso.Parte.Cpf) ? "JURÍDICA" : "FÍSICA",
                                AutenticacaoEletronica = lp.CodigoAutenticacaoEletronica,
                                CpfCnpjAutor = string.IsNullOrEmpty(lp.ParteProcesso.Parte.Cpf) ? lp.ParteProcesso.Parte.Cgc : lp.ParteProcesso.Parte.Cpf
                            }).FirstOrDefaultAsync();

            resultado.Agencia = await EmpresaCentralizadoraConvenioRepository.BuscarAgencia(resultado.CodigoEmpresaCentralizadora, resultado.CodigoEstado);

            return resultado;
        }

     

        private List<long?> RetornaListaProcessos(BBResumoProcessamentoFiltroDTO filtros)
        {
            var lista = new List<long?>();
            if (filtros.IdsProcessosCC?.Count() > 0)
                lista.AddRange(filtros.IdsProcessosCC);
            if (filtros.IdsProcessosJEC?.Count() > 0)
                lista.AddRange(filtros.IdsProcessosJEC);
            if (filtros.IdsProcessosPEX?.Count() > 0)
                lista.AddRange(filtros.IdsProcessosPEX);
            return lista;
        }
        public override Task<IDictionary<string, string>> RecuperarDropDown()
        {
            throw new System.NotImplementedException();
        }

        public async Task<ICollection<BBResumoProcessamentoResultadoDTO>> ExportarArquivoRetorno(BBResumoProcessamentoFiltroDTO filtros)
        {
            IQueryable<BBResumoProcessamento> query = RetornaIQuerybleFiltro(filtros);

            var retorno = await query.Select(remessa => new BBResumoProcessamentoResultadoDTO()
            {
                DataRemessa = remessa.DataRemessa.ToString("dd/MM/yyyy"),
                DataProcessamentoRemessa = remessa.DataProcessamentoRemessa.Value.ToString("dd/MM/yyyy"),
                NumeroLoteBB = remessa.NumeroLoteBB,
                QuantidadeRegistrosArquivo = remessa.QuantidadeRegistrosArquivo,
                QuantidadeRegistrosProcessados = remessa.QuantidadeRegistrosProcessados,
                Status = remessa.BBStatusRemessa.Descricao,
                ValorTotalGuiaProcessada = remessa.ValorTotalGuiaProcessada,
                ValorTotalRemessa = remessa.ValorTotalRemessa,
                TipoProcesso = EnumExtensions.GetDescricaoFromValue<TipoProcessoEnum>(remessa.Lote.CodigoTipoProcesso)
            })
            .ToListAsync();

            return retorno;
        }

        public async Task<int> TotaisArquivoRetorno(BBResumoProcessamentoFiltroDTO filtros)
        {
            IQueryable<BBResumoProcessamento> query = RetornaIQuerybleFiltro(filtros);
            return await query.CountAsync();
        }

        public async Task<BBResumoProcessamentoGuiaDTO> RecuperarLancamentoProcessoDoArquivo(long codProcesso, long codLancamento, long codLote)
        {
            var resultado = context.Set<LancamentoProcesso>()
                        .AsNoTracking()
                        .Where(l => l.Id == codProcesso && l.CodigoLancamento == codLancamento);
            if (codLote > 0)
                resultado = resultado
                    .Where(lp => lp.LoteLancamentos
                            .Any(ll => ll.Lote.Id == codLote));

            var ret = await resultado.Select(lp => new BBResumoProcessamentoGuiaDTO {
                CodigoLancamento = lp.CodigoLancamento.ToString(),
                CodigoProcesso = lp.Id.ToString(),
                AutenticacaoEletronica = lp.CodigoAutenticacaoEletronica,
                Autor = lp.ParteProcesso.Parte.Nome,
                NumeroContaJudicial = lp.NumeroContaJudicial.HasValue ? lp.NumeroContaJudicial.Value.ToString() : null,
                NumeroProcesso = lp.Processo.NumeroProcessoCartorio,
                Comarca = $"{lp.Processo.Comarca.CodigoEstado} - {lp.Processo.Comarca.Nome}",
                Juizado = $"{lp.Processo.CodigoVara}ª {lp.Processo.TipoVara.NomeTipoVara}",
                DescricaoEmpresaGrupo = lp.Processo.Parte.Nome,
                DataLancamento = lp.DataLancamento.ToString("dd/MM/yyyy"),
                DescricaoTipoLancamento = lp.TipoLancamento.Descricao,
                DescricaoCategoriaPagamento = lp.CategoriaPagamento.DescricaoCategoriaPagamento,
                StatusPagamento = lp.StatusPagamento.Descricao,
                DataEnvioEscritorio = lp.DataEnvioEscritorio.HasValue ? lp.DataEnvioEscritorio.Value.ToString("dd/MM/yyyy") : "",
                DescricaoEscritorio = lp.Processo.Profissional.NomeProfissional,
                NumeroPedidoSAP = lp.NumeroPedidoSap.GetValueOrDefault().ToString(),
                NumeroGuia = lp.NumeroGuia.GetValueOrDefault().ToString(),
                DataRecebimentoFiscal = lp.DataRecebimentoFiscal.HasValue ? lp.DataRecebimentoFiscal.Value.ToString("dd/MM/yyyy") : "",
                DataPagamentoPedido = lp.DataPagamentoPedido.HasValue ? lp.DataPagamentoPedido.Value.ToString("dd/MM/yyyy") : "",
                ValorLiquido = lp.ValorLancamento,
            }).FirstOrDefaultAsync();
            return ret;
        }
    }
}

