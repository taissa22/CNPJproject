using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Perlink.Oi.Juridico.Domain.SAP;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO.LoteCriacao;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Data;
using Shared.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2010.Drawing;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity;
using Perlink.Oi.Juridico.Domain.Logs.DTO;
using Shared.Domain.Impl.Entity;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Repository
{
    public class LancamentoProcessoRepository : BaseCrudRepository<LancamentoProcesso, long>, ILancamentoProcessoRepository
    {
        private readonly JuridicoContext dbContext;

        public LancamentoProcessoRepository(JuridicoContext dbContext, IAuthenticatedUser user) : base(dbContext, user)
        {
            this.dbContext = dbContext;
        }

        public override async Task<IDictionary<string, string>> RecuperarDropDown()
        {
            return await dbContext.Set<LancamentoProcesso>()
                    .AsNoTracking()
                    .OrderBy(x => x.Id)
                    .ToDictionaryAsync(x => x.Id.ToString(),
                                       x => x.NumeroGuia.ToString());
        }

        public async Task<ICollection<LancamentoProcesso>> RecuperarPorParteNumeroGuia(long NumeroGuia)
        {
            var LancamentoProcesso = await context.Set<LancamentoProcesso>().Where(filtro => filtro.NumeroGuia.ToString()
                                                                            .Contains(NumeroGuia.ToString()))
                                                  .ToListAsync();
            return LancamentoProcesso;
        }

        public async Task<long> RecuperarLancamentoProcesso(long NumeroGuia, long CodigoTipoProcesso)
        {
            if (CodigoTipoProcesso != 0)
            {
                var Resultado = await context.Set<LancamentoProcesso>()
                                        .Where(x => x.Processo.CodigoTipoProcesso == CodigoTipoProcesso
                                                &&
                                               x.NumeroGuia == NumeroGuia)
                                        .Select(p => p.NumeroGuia)
                                        .FirstOrDefaultAsync();
                if (Resultado.HasValue)
                    return Resultado.Value;
            }
            else
            {
                var Resultado = await context.Set<LancamentoProcesso>()
                                        .Where(x => x.NumeroGuia == NumeroGuia)
                                        .Select(p => p.NumeroGuia)
                                        .FirstOrDefaultAsync();
                if (Resultado.HasValue)
                    return Resultado.Value;
            }

            return 0;
        }

        public async Task<bool> ExisteLancamentoProcessoComFormaPagamento(long codigoFormaPagamento)
        {
            var resultado = await dbContext.LancamentoProcessos.AnyAsync(lp => lp.CodigoFormaPagamento.Equals(codigoFormaPagamento));

            return resultado;
        }

        public async Task<long> ObterTotalLancamentosDoLote(List<long> lista)
        {
            var totalLancamentos = 0;
            var step = lista.Count() > 1000 ? 1000 : 0;
            var pular = 0;
            while (step < lista.Count() + 1000)
            {
                var subArr = lista.Skip(pular).Take(1000);
                var count = await context.Set<LoteLancamento>()
                            .CountAsync(p => subArr.Contains(p.Id));

                totalLancamentos += count;
                step += 1000;
                pular += 1000;
            }
            return totalLancamentos;
        }

        public async Task<ICollection<LoteLancamentoDTO>> ObterLancamentoDoLote(long codigoLote)
        {
            var resultado = await context.Set<LoteLancamento>()
                            .Where(p => p.Id == codigoLote)
                            .Select(LoteLancamento => new LoteLancamentoDTO()
                            {
                                Id = codigoLote,
                                NumeroProcesso = LoteLancamento.LancamentoProcesso.Processo.NumeroProcessoCartorio,
                                Uf = LoteLancamento.LancamentoProcesso.Processo.Comarca.CodigoEstado,
                                NomeComarca = LoteLancamento.LancamentoProcesso.Processo.Comarca.Nome,
                                CodigoVara = LoteLancamento.LancamentoProcesso.Processo.Vara.CodigoVara.ToString(),
                                NomeTipoVara = LoteLancamento.LancamentoProcesso.Processo.TipoVara.NomeTipoVara,
                                DataEnvioEscritorio = LoteLancamento.LancamentoProcesso.DataEnvioEscritorio.HasValue ? LoteLancamento.LancamentoProcesso.DataEnvioEscritorio.Value.ToString("dd/MM/yyy") : "",
                                Escritorio = LoteLancamento.LancamentoProcesso.Processo.Profissional.NomeProfissional,
                                TipoLancamento = LoteLancamento.LancamentoProcesso.TipoLancamento.Descricao,
                                CategoriaPagamento = LoteLancamento.LancamentoProcesso.CategoriaPagamento.DescricaoCategoriaPagamento,
                                StatusPagamento = LoteLancamento.LancamentoProcesso.StatusPagamento.Descricao,
                                DataLancamento = LoteLancamento.LancamentoProcesso.DataLancamento.ToString("dd/MM/yyy HH:mm:ss"),
                                NumeroGuia = LoteLancamento.LancamentoProcesso.NumeroGuia.HasValue ? LoteLancamento.LancamentoProcesso.NumeroGuia.Value.ToString("G") : "",
                                NumeroPedidoSAP = LoteLancamento.Lote.NumeroPedidoSAP.HasValue ? LoteLancamento.Lote.NumeroPedidoSAP.Value.ToString("G") : "",
                                DataRecebimentoFiscal = LoteLancamento.LancamentoProcesso.DataRecebimentoFiscal.HasValue ? LoteLancamento.LancamentoProcesso.DataRecebimentoFiscal.Value.ToString("dd/MM/yyy") : "",
                                DataPagamentoPedido = LoteLancamento.LancamentoProcesso.DataPagamentoPedido.HasValue ? LoteLancamento.LancamentoProcesso.DataPagamentoPedido.Value.ToString("dd/MM/yyy") : "",
                                ValorLiquido = LoteLancamento.LancamentoProcesso.ValorLancamento.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR")),
                                TextoSAP = LoteLancamento.LancamentoProcesso.ComentarioSap,
                                Comentario = LoteLancamento.LancamentoProcesso.Comentario,
                                Autor = LoteLancamento.LancamentoProcesso.ParteProcesso.Parte.Nome,
                                NumeroContaJudicial = LoteLancamento.LancamentoProcesso.NumeroContaJudicial.HasValue ? LoteLancamento.LancamentoProcesso.NumeroContaJudicial.Value.ToString("G") : "",
                                NumeroParcelaJudicial = LoteLancamento.LancamentoProcesso.NumeroParcelaContaJudicial.HasValue ? LoteLancamento.LancamentoProcesso.NumeroParcelaContaJudicial.Value.ToString("G") : "",
                                AutenticacaoEletronica = LoteLancamento.LancamentoProcesso.CodigoAutenticacaoEletronica,
                                StatusParcelaBancoDoBrasil = LoteLancamento.LancamentoProcesso.BancoDoBrasilStatusParcela.Descricao,
                                DataEfetivacaoParcelaBancoDoBrasil = LoteLancamento.LancamentoProcesso.DataEfetivacaoParcelaBancoDoBrasil.HasValue ? LoteLancamento.LancamentoProcesso.DataEfetivacaoParcelaBancoDoBrasil.Value.ToString("dd/MM/yyy") : "",
                                LancamentoEstornado = LoteLancamento.LancamentoProcesso.IndicadorExluido,
                                CodigoProcesso = LoteLancamento.CodigoProcesso,
                                CodigoLancamento = LoteLancamento.CodigoLancamento
                            }).ToListAsync();

            return resultado;
        }

        public void CancelarLoteLancamento(long codigoLote, long codigoStatusPagamento)
        {
            if (!(codigoStatusPagamento == (int)StatusPagamentoEnum.PedidoSAPCriadoAguardando))
            {
                try
                {
                    int novoStatusPagamento;

                    switch (codigoStatusPagamento)
                    {
                        case (int)StatusPagamentoEnum.LoteGeradoAguardandoEnvioSAP:
                            novoStatusPagamento = 1;
                            break;

                        case (int)StatusPagamentoEnum.PedidoSAPRecebidoFiscal:
                            novoStatusPagamento = 10;
                            break;

                        default: novoStatusPagamento = 0; break;
                    }

                    var lancamentosProcesso = context.Set<LoteLancamento>()
                                                     .Include(ll => ll.LancamentoProcesso)
                                                     .Where(p => p.Id == codigoLote)
                                                     .ToList();

                    lancamentosProcesso.ForEach(ll =>
                    {
                        ll.LancamentoProcesso.CodigoStatusPagamento = novoStatusPagamento;
                        base.Atualizar(ll.LancamentoProcesso).Wait();
                    });
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<string> RecuperarDataEnvioEscritorio(long NumeroProcesso)
        {
            var DataEnvioEscritorio = await context.Set<LancamentoProcesso>()
                                   .Where(LancamentoProcesso => LancamentoProcesso.Id == NumeroProcesso)
                                   .Select(s => s.DataEnvioEscritorio == null ? "" : s.DataEnvioEscritorio.Value.ToString("dd/MM/yyyy HH:mm:ss"))
                                   .AsNoTracking()
                                   .FirstOrDefaultAsync();

            return DataEnvioEscritorio;
        }

        public async Task<ICollection<LoteLancamentoExportacaoDTO>> ExportarLancamentoDoLote(long codigoLote)
        {
            var resultado = await context.Set<LoteLancamento>()
                            .Where(p => p.Id == codigoLote)
                            .Select(LoteLancamento => new LoteLancamentoExportacaoDTO()
                            {
                                Id = codigoLote,
                                NumeroProcesso = "'" + LoteLancamento.LancamentoProcesso.Processo.NumeroProcessoCartorio,
                                Uf = LoteLancamento.LancamentoProcesso.Processo.Comarca.CodigoEstado,
                                NomeComarca = LoteLancamento.LancamentoProcesso.Processo.Comarca.Nome,
                                CodigoVara = LoteLancamento.LancamentoProcesso.Processo.Vara.CodigoVara.ToString(),
                                NomeTipoVara = LoteLancamento.LancamentoProcesso.Processo.TipoVara.NomeTipoVara,
                                DataEnvioEscritorio = LoteLancamento.LancamentoProcesso.DataEnvioEscritorio ?? null,
                                Escritorio = LoteLancamento.LancamentoProcesso.Processo.Profissional.NomeProfissional,
                                TipoLancamento = LoteLancamento.LancamentoProcesso.TipoLancamento.Descricao,
                                CategoriaPagamento = LoteLancamento.LancamentoProcesso.CategoriaPagamento.DescricaoCategoriaPagamento,
                                StatusPagamento = LoteLancamento.LancamentoProcesso.StatusPagamento.Descricao,
                                DataLancamento = LoteLancamento.LancamentoProcesso.DataLancamento.ToString("dd/MM/yyyy HH:mm:ss"),
                                NumeroGuia = LoteLancamento.LancamentoProcesso.NumeroGuia.HasValue ? LoteLancamento.LancamentoProcesso.NumeroGuia.Value.ToString("G") : "",
                                NumeroPedidoSAP = LoteLancamento.Lote.NumeroPedidoSAP.HasValue ? LoteLancamento.Lote.NumeroPedidoSAP.Value.ToString("G") : "",
                                DataRecebimentoFiscal = LoteLancamento.LancamentoProcesso.DataRecebimentoFiscal.HasValue ? LoteLancamento.LancamentoProcesso.DataRecebimentoFiscal.Value.ToString("dd/MM/yyyy") : "",
                                DataPagamentoPedido = LoteLancamento.LancamentoProcesso.DataPagamentoPedido.HasValue ? LoteLancamento.LancamentoProcesso.DataPagamentoPedido.Value.ToString("dd/MM/yyyy") : "",
                                ValorLiquido = LoteLancamento.LancamentoProcesso.ValorLancamento.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR")),
                                TextoSAP = LoteLancamento.LancamentoProcesso.ComentarioSap,
                                Comentario = LoteLancamento.LancamentoProcesso.Comentario,
                                Autor = LoteLancamento.LancamentoProcesso.ParteProcesso.Parte.Nome,
                                NumeroContaJudicial = LoteLancamento.LancamentoProcesso.NumeroContaJudicial.HasValue ? LoteLancamento.LancamentoProcesso.NumeroContaJudicial.Value.ToString("G") : "",
                                NumeroParcelaJudicial = LoteLancamento.LancamentoProcesso.NumeroParcelaContaJudicial.HasValue ? LoteLancamento.LancamentoProcesso.NumeroParcelaContaJudicial.Value.ToString("G") : "",
                                AutenticacaoEletronica = LoteLancamento.LancamentoProcesso.CodigoAutenticacaoEletronica,
                                StatusParcelaBancoDoBrasil = LoteLancamento.LancamentoProcesso.BancoDoBrasilStatusParcela.Descricao,
                                DataEfetivacaoParcelaBancoDoBrasil = LoteLancamento.LancamentoProcesso.DataEfetivacaoParcelaBancoDoBrasil.HasValue ? LoteLancamento.LancamentoProcesso.DataEfetivacaoParcelaBancoDoBrasil.Value.ToString("dd/MM/yyy") : "",
                                CodigoProcesso = LoteLancamento.CodigoProcesso.ToString(),
                                CodigoLancamento = LoteLancamento.CodigoLancamento.ToString(),
                                CodigoComarca = LoteLancamento.LancamentoProcesso.Processo.CodigoComarca.ToString(),
                                CodigoTipoVara = LoteLancamento.LancamentoProcesso.Processo.CodigoTipoVara.ToString(),
                                CodigoTipoLancamento = LoteLancamento.LancamentoProcesso.CodigoTipoLancamento.ToString(),
                                CodigoCategoriaPagamento = LoteLancamento.LancamentoProcesso.CodigoCatPagamento.ToString(),
                                QuantidadeLancamento = LoteLancamento.LancamentoProcesso.QuantidadeLancamento.ToString(),
                                DataCriacaoPedido = LoteLancamento.Lote.DataCriacaoPedido.HasValue ? LoteLancamento.Lote.DataCriacaoPedido.Value.ToString("dd/MM/yyyy") : "",
                                DataRecebimentoFisico = LoteLancamento.Lote.DataRecebimentoFisico.HasValue ? LoteLancamento.Lote.DataRecebimentoFisico.Value.ToString("dd/MM/yyyy") : "",
                                CodigoUsuarioRecebedor = LoteLancamento.LancamentoProcesso.CodigoUsuarioRecebedor.ToString(),
                                IndicaExcluido = LoteLancamento.LancamentoProcesso.IndicadorExluido.ToString(),
                                CodigoProfissional = LoteLancamento.LancamentoProcesso.Processo.CodigoProfissonal.ToString(),
                                CodigoStatusPagamento = LoteLancamento.LancamentoProcesso.CodigoStatusPagamento.ToString()
                            }).ToListAsync();

            return resultado;
        }

        public async Task<ICollection<LoteCriacaoResultadoEmpresaCentralizadoraDTO>> ObterLotesAgrupadoPorEmpresaCentralizadora(LoteCriacaoFiltroEmpresaCentralizadoraDTO filtros)
        {
            string sql;
            switch (filtros.CodigoTipoProcesso)
            {
                //Consumidor
                case 1:
                    sql = @"select tbl.empce_cod_emp_centralizadora as CodigoEmpresaCentralizadora,
                           tbl.nome as DescricaoEmpresaCentralizadora,
                           count(*) as TotalLote
                          from (SELECT jur.processo.cod_tipo_processo,
                                       jur.parte.empce_cod_emp_centralizadora,
                                       ec.nome,
                                       jur.processo.cod_parte_empresa,
                                       decode(forn.DATA, null, '', c.cod_estado) cod_estado,
                                       jur.parte.nom_parte,
                                       jur.lancamento_processo.cod_forma_pagamento,
                                       jur.forma_pagamento.dsc_forma_pagamento,
                                       jur.lancamento_processo.cod_centro_custo,
                                       jur.centro_custo.cod_centro_custo_sap,
                                       jur.lancamento_processo.cod_fornecedor,
                                       jur.fornecedor.nom_fornecedor,
                                       count(*) as qtd_lancamentos
                                  FROM jur.centro_custo,
                                       jur.forma_pagamento,
                                       jur.fornecedor,
                                       jur.lancamento_processo,
                                       jur.parte,
                                       jur.processo,
                                       jur.empresas_sap es,
                                       jur.comarca c,
                                       jur.empresas_centralizadoras ec,
                                       THE (SELECT CAST(JUR.F_OBTEM_DADOS((select dsc_conteudo_parametro
                                                                            from jur.parametro_juridico
                                                                           where cod_parametro =
                                                                                 'SAP_FORNECEDOR_BB'),
                                                                          '|') AS
                                                        JUR.MYTABLETYPE)
                                              FROM DUAL) FORN
                                 WHERE (jur.lancamento_processo.cod_fornecedor =
                                       jur.fornecedor.cod_fornecedor)
                                   and (jur.lancamento_processo.cod_forma_pagamento =
                                       jur.forma_pagamento.cod_forma_pagamento)
                                   and (jur.lancamento_processo.cod_centro_custo =
                                       jur.centro_custo.cod_centro_custo)
                                   and (jur.processo.cod_parte_empresa = jur.parte.cod_parte)
                                   and (jur.processo.cod_processo =
                                       jur.lancamento_processo.cod_processo)
                                   and (jur.lancamento_processo.cod_status_pagamento in
                                       (1, 5, 10))
                                   and (jur.processo.cod_tipo_processo = {0})
                                   and ( trunc(jur.lancamento_processo.dat_lancamento) >= decode({1}, null, to_date('01/01/2000', 'dd/mm/yyyy'), {1}))
                                   and( trunc(jur.lancamento_processo.dat_lancamento) <= decode({2}, null, trunc(sysdate + 1), {2}) )
			                       and( jur.lancamento_processo.val_lancamento >= decode({3}, null, 0, {3}) )
			                       and( jur.lancamento_processo.val_lancamento <= decode({4}, null, 999999999999999, {4}) )
                                   and (jur.lancamento_processo.ind_excluido = 'N')
                                   and (jur.parte.esap_cod_empresa_sap = es.codigo(+))
                                   and (jur.processo.cod_comarca = c.cod_comarca)
                                   and (jur.lancamento_processo.cod_fornecedor = forn.DATA(+))
                                   and (jur.parte.empce_cod_emp_centralizadora = ec.codigo)
                                 GROUP BY jur.processo.cod_tipo_processo,
                                          jur.parte.empce_cod_emp_centralizadora,
                                          ec.nome,
                                          jur.processo.cod_parte_empresa,
                                          decode(forn.DATA, null, '', c.cod_estado),
                                          jur.parte.nom_parte,
                                          jur.lancamento_processo.cod_forma_pagamento,
                                          jur.forma_pagamento.dsc_forma_pagamento,
                                          jur.lancamento_processo.cod_centro_custo,
                                          jur.centro_custo.cod_centro_custo_sap,
                                          jur.lancamento_processo.cod_fornecedor,
                                          jur.fornecedor.nom_fornecedor) tbl
                     group by tbl.empce_cod_emp_centralizadora, tbl.nome
                     order by tbl.nome";

                    return await this.context.Query<LoteCriacaoResultadoEmpresaCentralizadoraDTO>().FromSql(sql, filtros.CodigoTipoProcesso, filtros.DataCriacaoLancamentoInicio, filtros.DataCriacaoLancamentoFim, filtros.ValorLancamentoInicio, filtros.ValorLancamentoFim).ToListAsync();

                //PEX
                case 18:
                    sql = @"select tb1.empce_cod_emp_centralizadora as CodigoEmpresaCentralizadora,
                           tb1.nome as DescricaoEmpresaCentralizadora,
                           count(*) as TotalLote
                          from (SELECT jur.processo.cod_tipo_processo,
                                       jur.parte.empce_cod_emp_centralizadora,
                                       ec.nome,
                                       jur.processo.cod_parte_empresa,
                                       decode(forn.DATA, null, '', c.cod_estado) cod_estado,
                                       jur.parte.nom_parte,
                                       jur.lancamento_processo.cod_forma_pagamento,
                                       jur.forma_pagamento.dsc_forma_pagamento,
                                       jur.lancamento_processo.cod_centro_custo,
                                       jur.centro_custo.cod_centro_custo_sap cod_centro_custo_sap,
                                       jur.lancamento_processo.cod_fornecedor,
                                       jur.fornecedor.nom_fornecedor,
                                       count(*) as qtd_lancamentos,
                                       c.cod_estado as estadoComarca
                                  FROM jur.centro_custo,
                                       jur.forma_pagamento,
                                       jur.fornecedor,
                                       jur.lancamento_processo,
                                       jur.parte,
                                       jur.processo,
                                       jur.empresas_sap es,
                                       jur.comarca c,
                                       jur.empresas_centralizadoras ec,
                                       THE (SELECT CAST(JUR.F_OBTEM_DADOS((select dsc_conteudo_parametro
                                                                            from jur.parametro_juridico
                                                                           where cod_parametro =
                                                                                 'SAP_FORNECEDOR_BB'),
                                                                          '|') AS
                                                        JUR.MYTABLETYPE)
                                              FROM DUAL) FORN
                                 WHERE (jur.lancamento_processo.cod_fornecedor =
                                       jur.fornecedor.cod_fornecedor)
                                   and (jur.lancamento_processo.cod_forma_pagamento =
                                       jur.forma_pagamento.cod_forma_pagamento)
                                   and (jur.lancamento_processo.cod_centro_custo =
                                       jur.centro_custo.cod_centro_custo)
                                   and (jur.processo.cod_parte_empresa = jur.parte.cod_parte)
                                   and (jur.processo.cod_processo =
                                       jur.lancamento_processo.cod_processo)
                                   and (jur.lancamento_processo.cod_status_pagamento in
                                       (1, 5, 10))
                                   and (jur.processo.cod_tipo_processo = {0})
                                  and ( trunc(jur.lancamento_processo.dat_lancamento) >= decode({1}, null, to_date('01/01/2000', 'dd/mm/yyyy'), {1}))
                                   and( trunc(jur.lancamento_processo.dat_lancamento) <= decode({2}, null, trunc(sysdate + 1), {2}) )
			                       and( jur.lancamento_processo.val_lancamento >= decode({3}, null, 0, {3}) )
			                       and( jur.lancamento_processo.val_lancamento <= decode({4}, null, 999999999999999, {4}) )
                                   and (jur.lancamento_processo.ind_excluido = 'N')
                                   and (jur.parte.esap_cod_empresa_sap = es.codigo(+))
                                   and (jur.processo.cod_comarca = c.cod_comarca)
                                   and (jur.lancamento_processo.cod_fornecedor = forn.DATA(+))
                                   and (jur.parte.empce_cod_emp_centralizadora = ec.codigo)
                                 GROUP BY jur.processo.cod_tipo_processo,
                                          jur.parte.empce_cod_emp_centralizadora,
                                          ec.nome,
                                          jur.processo.cod_parte_empresa,
                                          decode(forn.DATA, null, '', c.cod_estado),
                                          jur.parte.nom_parte,
                                          jur.lancamento_processo.cod_forma_pagamento,
                                          jur.forma_pagamento.dsc_forma_pagamento,
                                          jur.lancamento_processo.cod_centro_custo,
                                          jur.centro_custo.cod_centro_custo_sap,
                                          jur.lancamento_processo.cod_fornecedor,
                                          jur.fornecedor.nom_fornecedor,
                                          c.cod_estado) tb1
                     group by tb1.empce_cod_emp_centralizadora, tb1.nome
                     order by tb1.nome";

                    return await this.context.Query<LoteCriacaoResultadoEmpresaCentralizadoraDTO>().FromSql(sql, filtros.CodigoTipoProcesso, filtros.DataCriacaoLancamentoInicio, filtros.DataCriacaoLancamentoFim, filtros.ValorLancamentoInicio, filtros.ValorLancamentoFim).ToListAsync();

                //Juizado
                case 7:
                    sql = @"select tb1.empce_cod_emp_centralizadora as CodigoEmpresaCentralizadora,
                           tb1.nome as DescricaoEmpresaCentralizadora,
                           count(*) as TotalLote
                          from (SELECT jur.processo.cod_tipo_processo,
                                       jur.parte.empce_cod_emp_centralizadora,
                                       ec.nome,
                                       jur.processo.cod_parte_empresa,
                                       decode(forn.DATA, null, '', c.cod_estado) cod_estado,
                                       jur.parte.nom_parte,
                                       jur.lancamento_processo.cod_forma_pagamento,
                                       jur.forma_pagamento.dsc_forma_pagamento,
                                       jur.lancamento_processo.cod_centro_custo,
                                       jur.centro_custo.cod_centro_custo_sap cod_centro_custo_sap,
                                       jur.lancamento_processo.cod_fornecedor,
                                       jur.fornecedor.nom_fornecedor,
                                       jur.grupos_lotes_juizados.codigo cod_grupo_lote_juizado,
                                       jur.grupos_lotes_juizados.descricao dsc_grupo_lote_juizado,
                                       count(*) as qtd_lancamentos,
                                       c.cod_estado as estadoComarca
                                  FROM jur.centro_custo,
                                       jur.forma_pagamento,
                                       jur.fornecedor,
                                       jur.lancamento_processo,
                                       jur.parte,
                                       jur.processo,
                                       jur.profissional,
                                       jur.grupos_lotes_juizados,
                                       jur.empresas_sap es,
                                       jur.comarca c,
                                       jur.empresas_centralizadoras ec,
                                       THE (SELECT CAST(JUR.F_OBTEM_DADOS((select dsc_conteudo_parametro
                                                                            from jur.parametro_juridico
                                                                           where cod_parametro =
                                                                                 'SAP_FORNECEDOR_BB'),
                                                                          '|') AS
                                                        JUR.MYTABLETYPE)
                                              FROM DUAL) FORN
                                 WHERE (jur.lancamento_processo.cod_fornecedor =
                                       jur.fornecedor.cod_fornecedor)
                                   and (jur.lancamento_processo.cod_forma_pagamento =
                                       jur.forma_pagamento.cod_forma_pagamento)
                                   and (jur.lancamento_processo.cod_centro_custo =
                                       jur.centro_custo.cod_centro_custo)
                                   and (jur.processo.cod_parte_empresa = jur.parte.cod_parte)
                                   and (jur.processo.cod_processo =
                                       jur.lancamento_processo.cod_processo)

                                   and ( jur.processo.cod_profissional =
                                        jur.profissional.cod_profissional (+) )
                                   and( jur.grupos_lotes_juizados.codigo =
                                        jur.profissional.glj_cod_grupo_lote_juizado)

                                   and (jur.lancamento_processo.cod_status_pagamento in
                                       (1, 5, 10))
                                   and (jur.processo.cod_tipo_processo = {0})
                                   and ( trunc(jur.lancamento_processo.dat_lancamento) >= decode({1}, null, to_date('01/01/2000', 'dd/mm/yyyy'), {1}))
                                   and( trunc(jur.lancamento_processo.dat_lancamento) <= decode({2}, null, trunc(sysdate + 1), {2}) )
			                       and( jur.lancamento_processo.val_lancamento >= decode({3}, null, 0, {3}) )
			                       and( jur.lancamento_processo.val_lancamento <= decode({4}, null, 999999999999999, {4}) )
                                  and  (jur.processo.ind_processo_ativo <> 'E' )
                                   and (jur.lancamento_processo.ind_excluido = 'N')
                                   and (jur.parte.esap_cod_empresa_sap = es.codigo(+))
                                   and (jur.processo.cod_comarca = c.cod_comarca)
                                   and (jur.lancamento_processo.cod_fornecedor = forn.DATA(+))
                                   and (jur.parte.empce_cod_emp_centralizadora = ec.codigo)
                                 GROUP BY jur.processo.cod_tipo_processo,
                                          jur.parte.empce_cod_emp_centralizadora,
                                          ec.nome,
                                          jur.processo.cod_parte_empresa,
                                          decode(forn.DATA, null, '', c.cod_estado),
                                          jur.parte.nom_parte,
                                          jur.lancamento_processo.cod_forma_pagamento,
                                          jur.forma_pagamento.dsc_forma_pagamento,
                                          jur.lancamento_processo.cod_centro_custo,
                                          jur.centro_custo.cod_centro_custo_sap,
                                          jur.lancamento_processo.cod_fornecedor,
                                          jur.fornecedor.nom_fornecedor,
                                          jur.grupos_lotes_juizados.codigo,
                                         jur.grupos_lotes_juizados.descricao,
                                          c.cod_estado) tb1
                     group by tb1.empce_cod_emp_centralizadora, tb1.nome
                     order by tb1.nome
                ";

                    return await this.context.Query<LoteCriacaoResultadoEmpresaCentralizadoraDTO>().FromSql(sql, filtros.CodigoTipoProcesso, filtros.DataCriacaoLancamentoInicio, filtros.DataCriacaoLancamentoFim, filtros.ValorLancamentoInicio, filtros.ValorLancamentoFim).ToListAsync();

                //civel estrategico e Trabalhista
                default:
                    sql = @"select tb1.empce_cod_emp_centralizadora as CodigoEmpresaCentralizadora,
                           tb1.nome as DescricaoEmpresaCentralizadora,
                           count(*) as TotalLote
                          from (SELECT jur.processo.cod_tipo_processo,
                                        jur.parte.empce_cod_emp_centralizadora,
                                        ec.nome,
                                       jur.processo.cod_parte_empresa,
                                       decode(forn.DATA, null, '', c.cod_estado) cod_estado,
                                       jur.parte.nom_parte,
                                       jur.lancamento_processo.cod_forma_pagamento,
                                       jur.forma_pagamento.dsc_forma_pagamento,
                                       jur.lancamento_processo.cod_centro_custo,
                                       jur.centro_custo.cod_centro_custo_sap cod_centro_custo_sap,
                                       jur.lancamento_processo.cod_fornecedor,
                                       jur.fornecedor.nom_fornecedor,
                                       count(*) as qtd_lancamentos,
                                       c.cod_estado as estadoComarca
                                  FROM jur.centro_custo,
                                       jur.forma_pagamento,
                                       jur.fornecedor,
                                       jur.lancamento_processo,
                                       jur.parte,
                                       jur.processo,
                                       jur.empresas_sap es,
                                       jur.comarca c,
                                       jur.empresas_centralizadoras ec,
                                       THE (SELECT CAST(JUR.F_OBTEM_DADOS((select dsc_conteudo_parametro
                                                                            from jur.parametro_juridico
                                                                           where cod_parametro =
                                                                                 'SAP_FORNECEDOR_BB'),
                                                                          '|') AS
                                                        JUR.MYTABLETYPE)
                                              FROM DUAL) FORN
                                 WHERE (jur.lancamento_processo.cod_fornecedor =
                                       jur.fornecedor.cod_fornecedor)
                                   and (jur.lancamento_processo.cod_forma_pagamento =
                                       jur.forma_pagamento.cod_forma_pagamento)
                                   and (jur.lancamento_processo.cod_centro_custo =
                                       jur.centro_custo.cod_centro_custo)
                                   and (jur.processo.cod_parte_empresa = jur.parte.cod_parte)
                                   and (jur.processo.cod_processo =
                                       jur.lancamento_processo.cod_processo)
                                   and (jur.lancamento_processo.cod_status_pagamento in
                                       (1, 5, 10))
                                   and (jur.processo.cod_tipo_processo = {0})
                                   and ( trunc(jur.lancamento_processo.dat_lancamento) >= decode({1}, null, to_date('01/01/2000', 'dd/mm/yyyy'), {1}))
                                   and( trunc(jur.lancamento_processo.dat_lancamento) <= decode({2}, null, trunc(sysdate + 1), {2}) )
			                       and( jur.lancamento_processo.val_lancamento >= decode({3}, null, 0, {3}) )
			                       and( jur.lancamento_processo.val_lancamento <= decode({4}, null, 999999999999999, {4}) )
                                   and (jur.lancamento_processo.ind_excluido = 'N')
                                   and (jur.parte.esap_cod_empresa_sap = es.codigo(+))
                                   and (jur.processo.cod_comarca = c.cod_comarca)
                                   and (jur.lancamento_processo.cod_fornecedor = forn.DATA(+))
                                   and (jur.parte.empce_cod_emp_centralizadora = ec.codigo)
                                 GROUP BY jur.processo.cod_tipo_processo,
                                          jur.parte.empce_cod_emp_centralizadora,
                                          ec.nome,
                                          jur.processo.cod_parte_empresa,
                                          decode(forn.DATA, null, '', c.cod_estado),
                                          jur.parte.nom_parte,
                                          jur.lancamento_processo.cod_forma_pagamento,
                                          jur.forma_pagamento.dsc_forma_pagamento,
                                          jur.lancamento_processo.cod_centro_custo,
                                          jur.centro_custo.cod_centro_custo_sap,
                                          jur.lancamento_processo.cod_fornecedor,
                                          jur.fornecedor.nom_fornecedor,
                                          c.cod_estado) tb1
                     group by tb1.empce_cod_emp_centralizadora, tb1.nome
                     order by tb1.nome";

                    return await this.context.Query<LoteCriacaoResultadoEmpresaCentralizadoraDTO>().FromSql(sql, filtros.CodigoTipoProcesso, filtros.DataCriacaoLancamentoInicio, filtros.DataCriacaoLancamentoFim, filtros.ValorLancamentoInicio, filtros.ValorLancamentoFim).ToListAsync();
            }
        }

        public async Task<ICollection<LoteCriacaoResultadoEmpresaGrupoDTO>> ObterLotesAgrupadoPorEmpresadoGrupo(LoteCriacaoFiltroEmpresaGrupoDTO filtros)
        {
           
            string sql;
            switch (filtros.CodigoTipoProcesso)
            {
                //Consumidor
                case 1:
                    sql = @"SELECT l.CentroSAP,
                       l.CodigoEmpresaGrupo,
                       l.DescricaoCentroCusto,
                       l.CodigoCentroCusto,
                       l.DescricaoEmpresaGrupo,
                       l.DescricaoFormaPagamento,
                       l.CodigoFormaPagamento,
                       l.DescricaoFornecedor,
                       l.CodigoFornecedor,
                       l.TotalLancamneto,
                       l.Uf,
                       l.FornecedorSAP,
                       l.IndicaBordero
                  FROM (select tbl.cod_parte_empresa   as CodigoEmpresaGrupo,
                               tbl.nom_parte           as DescricaoEmpresaGrupo,
                               tbl.estadoComarca       as Uf,
                               tbl.nom_fornecedor      as DescricaoFornecedor,
                               tbl.cod_fornecedor      as CodigoFornecedor,
                               tbl.dsc_forma_pagamento as DescricaoFormaPagamento,
                               tbl.cod_forma_pagamento as CodigoFormaPagamento,
                               cc.dsc_centro_custo     as DescricaoCentroCusto,
                               cc.cod_centro_custo     as CodigoCentroCusto,
                               p.COD_CENTRO_SAP        as CentroSAP,
                               tbl.qtd_lancamentos     as TotalLancamneto,
                               f.cod_fornecedor_sap    as FornecedorSAP,
                               fp.ind_bordero          as IndicaBordero,
                               rownum                  r2
                          from (SELECT jur.processo.cod_tipo_processo,
                                       jur.processo.cod_parte_empresa,
                                       decode(forn.DATA, null, '', c.cod_estado) cod_estado,
                                       jur.parte.nom_parte,
                                       jur.lancamento_processo.cod_forma_pagamento,
                                       jur.forma_pagamento.dsc_forma_pagamento,
                                       jur.lancamento_processo.cod_centro_custo,
                                       jur.centro_custo.cod_centro_custo_sap,
                                       jur.lancamento_processo.cod_fornecedor,
                                       jur.fornecedor.nom_fornecedor,
                                       count(*) as qtd_lancamentos,
                                       c.cod_estado as estadoComarca
                                  FROM jur.centro_custo,
                                       jur.forma_pagamento,
                                       jur.fornecedor,
                                       jur.lancamento_processo,
                                       jur.parte,
                                       jur.processo,
                                       jur.empresas_sap es,
                                       jur.comarca c,
                                       jur.empresas_centralizadoras ec,
                                       THE (SELECT CAST(JUR.F_OBTEM_DADOS((select dsc_conteudo_parametro
                                                                            from jur.parametro_juridico
                                                                           where cod_parametro =
                                                                                 'SAP_FORNECEDOR_BB'),
                                                                          '|') AS
                                                        JUR.MYTABLETYPE)
                                              FROM DUAL) FORN
                                 WHERE (jur.lancamento_processo.cod_fornecedor =
                                       jur.fornecedor.cod_fornecedor)
                                   and (jur.lancamento_processo.cod_forma_pagamento =
                                       jur.forma_pagamento.cod_forma_pagamento)
                                   and (jur.lancamento_processo.cod_centro_custo =
                                       jur.centro_custo.cod_centro_custo)
                                   and (jur.processo.cod_parte_empresa = jur.parte.cod_parte)
                                   and (jur.processo.cod_processo =
                                       jur.lancamento_processo.cod_processo)
                                   and (jur.lancamento_processo.cod_status_pagamento in
                                       (1, 5, 10))
                                   and (jur.processo.cod_tipo_processo = {0})
                                   and ( trunc(jur.lancamento_processo.dat_lancamento) >= decode({1}, null, to_date('01/01/2000', 'dd/mm/yyyy'), {1}))
                                   and( trunc(jur.lancamento_processo.dat_lancamento) <= decode({2}, null, trunc(sysdate + 1), {2}) )
			                       and( jur.lancamento_processo.val_lancamento >= decode({3}, null, 0, {3}) )
			                       and( jur.lancamento_processo.val_lancamento <= decode({4}, null, 999999999999999, {4}) )
                                   and (jur.lancamento_processo.ind_excluido = 'N')
                                   and (jur.parte.esap_cod_empresa_sap = es.codigo(+))
                                   and (jur.processo.cod_comarca = c.cod_comarca)
                                   and (jur.lancamento_processo.cod_fornecedor = forn.DATA(+))
                                   and (jur.parte.empce_cod_emp_centralizadora = ec.codigo)
                                   and (ec.codigo = {5})
                                 GROUP BY jur.processo.cod_tipo_processo,
                                          jur.processo.cod_parte_empresa,
                                          decode(forn.DATA, null, '', c.cod_estado),
                                          jur.parte.nom_parte,
                                          jur.lancamento_processo.cod_forma_pagamento,
                                          jur.forma_pagamento.dsc_forma_pagamento,
                                          jur.lancamento_processo.cod_centro_custo,
                                          jur.centro_custo.cod_centro_custo_sap,
                                          jur.lancamento_processo.cod_fornecedor,
                                          jur.fornecedor.nom_fornecedor,
                                          c.cod_estado) tbl,
                               jur.centro_custo cc,
                               jur.fornecedor f,
                               jur.parte p,
                               jur.forma_pagamento fp
                         where cc.cod_centro_custo = tbl.cod_centro_custo
                           and tbl.cod_fornecedor = f.cod_fornecedor
                           and tbl.cod_parte_empresa = p.COD_PARTE
                           and tbl.cod_forma_pagamento = fp.cod_forma_pagamento) l
                 
                   
                 order by l.DescricaoEmpresaGrupo";
                    return await this.context.Query<LoteCriacaoResultadoEmpresaGrupoDTO>().FromSql(sql, filtros.CodigoTipoProcesso, filtros.DataCriacaoLancamentoInicio, filtros.DataCriacaoLancamentoFim, filtros.ValorLancamentoInicio, filtros.ValorLancamentoFim, filtros.CodigoEmpresaCentralizadora).ToListAsync();
                //PEX
                case 18:
                    sql = @"SELECT l.CentroSAP,
                       l.CodigoEmpresaGrupo,
                       l.DescricaoCentroCusto,
                       l.CodigoCentroCusto,
                       l.DescricaoEmpresaGrupo,
                       l.DescricaoFormaPagamento,
                       l.CodigoFormaPagamento,
                       l.DescricaoFornecedor,
                       l.CodigoFornecedor,
                       l.TotalLancamneto,
                       l.Uf,
                       l.FornecedorSAP,
                       l.IndicaBordero
                  FROM (select tbl.cod_parte_empresa   as CodigoEmpresaGrupo,
                               tbl.nom_parte           as DescricaoEmpresaGrupo,
                               tbl.estadoComarca       as Uf,
                               tbl.nom_fornecedor      as DescricaoFornecedor,
                               tbl.cod_fornecedor      as CodigoFornecedor,
                               tbl.dsc_forma_pagamento as DescricaoFormaPagamento,
                               tbl.cod_forma_pagamento as CodigoFormaPagamento,
                               cc.dsc_centro_custo     as DescricaoCentroCusto,
                               cc.cod_centro_custo     as CodigoCentroCusto,
                               p.COD_CENTRO_SAP        as CentroSAP,
                               tbl.qtd_lancamentos     as TotalLancamneto,
                               f.cod_fornecedor_sap    as FornecedorSAP,
                               fp.ind_bordero          as IndicaBordero,
                               rownum                  r2
                          from (SELECT jur.processo.cod_tipo_processo,
                                       jur.processo.cod_parte_empresa,
                                       decode(forn.DATA, null, '', c.cod_estado) cod_estado,
                                       jur.parte.nom_parte,
                                       jur.lancamento_processo.cod_forma_pagamento,
                                       jur.forma_pagamento.dsc_forma_pagamento,
                                       jur.lancamento_processo.cod_centro_custo,
                                       jur.centro_custo.cod_centro_custo_sap cod_centro_custo_sap,
                                       jur.lancamento_processo.cod_fornecedor,
                                       jur.fornecedor.nom_fornecedor,
                                       count(*) as qtd_lancamentos,
                                       c.cod_estado as estadoComarca
                                  FROM jur.centro_custo,
                                       jur.forma_pagamento,
                                       jur.fornecedor,
                                       jur.lancamento_processo,
                                       jur.parte,
                                       jur.processo,
                                       jur.empresas_sap es,
                                       jur.comarca c,
                                       jur.empresas_centralizadoras ec,
                                       THE (SELECT CAST(JUR.F_OBTEM_DADOS((select dsc_conteudo_parametro
                                                                            from jur.parametro_juridico
                                                                           where cod_parametro =
                                                                                 'SAP_FORNECEDOR_BB'),
                                                                          '|') AS
                                                        JUR.MYTABLETYPE)
                                              FROM DUAL) FORN
                                 WHERE (jur.lancamento_processo.cod_fornecedor =
                                       jur.fornecedor.cod_fornecedor)
                                   and (jur.lancamento_processo.cod_forma_pagamento =
                                       jur.forma_pagamento.cod_forma_pagamento)
                                   and (jur.lancamento_processo.cod_centro_custo =
                                       jur.centro_custo.cod_centro_custo)
                                   and (jur.processo.cod_parte_empresa = jur.parte.cod_parte)
                                   and (jur.processo.cod_processo =
                                       jur.lancamento_processo.cod_processo)
                                   and (jur.lancamento_processo.cod_status_pagamento in
                                       (1, 5, 10))
                                   and (jur.processo.cod_tipo_processo = {0})

                                  and ( trunc(jur.lancamento_processo.dat_lancamento) >= decode({1}, null, to_date('01/01/2000', 'dd/mm/yyyy'), {1}))
                                   and( trunc(jur.lancamento_processo.dat_lancamento) <= decode({2}, null, trunc(sysdate + 1), {2}) )
			                       and( jur.lancamento_processo.val_lancamento >= decode({3}, null, 0, {3}) )
			                       and( jur.lancamento_processo.val_lancamento <= decode({4}, null, 999999999999999, {4}) )

                                   and (jur.lancamento_processo.ind_excluido = 'N')
                                   and (jur.parte.esap_cod_empresa_sap = es.codigo(+))
                                   and (jur.processo.cod_comarca = c.cod_comarca)
                                   and (jur.lancamento_processo.cod_fornecedor = forn.DATA(+))
                                   and (jur.parte.empce_cod_emp_centralizadora = ec.codigo)
                                   and (ec.codigo = {5})
                                 GROUP BY jur.processo.cod_tipo_processo,
                                          jur.processo.cod_parte_empresa,
                                          decode(forn.DATA, null, '', c.cod_estado),
                                          jur.parte.nom_parte,
                                          jur.lancamento_processo.cod_forma_pagamento,
                                          jur.forma_pagamento.dsc_forma_pagamento,
                                          jur.lancamento_processo.cod_centro_custo,
                                          jur.centro_custo.cod_centro_custo_sap,
                                          jur.lancamento_processo.cod_fornecedor,
                                          jur.fornecedor.nom_fornecedor,
                                          c.cod_estado) tbl,
                               jur.centro_custo cc,
                               jur.fornecedor f,
                               jur.parte p,
                               jur.forma_pagamento fp
                         where cc.cod_centro_custo = tbl.cod_centro_custo
                           and tbl.cod_fornecedor = f.cod_fornecedor
                           and tbl.cod_parte_empresa = p.COD_PARTE
                           and tbl.cod_forma_pagamento = fp.cod_forma_pagamento) l
                 
                   
                    order by l.DescricaoEmpresaGrupo";

                    return await this.context.Query<LoteCriacaoResultadoEmpresaGrupoDTO>().FromSql(sql, filtros.CodigoTipoProcesso, filtros.DataCriacaoLancamentoInicio, filtros.DataCriacaoLancamentoFim, filtros.ValorLancamentoInicio, filtros.ValorLancamentoFim, filtros.CodigoEmpresaCentralizadora).ToListAsync();

                //Juizado
                case 7:
                    sql = @"SELECT l.CentroSAP,
                       l.CodigoEmpresaGrupo,
                       l.DescricaoCentroCusto,
                       l.CodigoCentroCusto,
                       l.DescricaoEmpresaGrupo,
                       l.DescricaoFormaPagamento,
                       l.CodigoFormaPagamento,
                       l.DescricaoFornecedor,
                       l.CodigoFornecedor,
                       l.TotalLancamneto,
                       l.Uf,
                       l.FornecedorSAP,
                       l.IndicaBordero
                  FROM (select tbl.cod_parte_empresa   as CodigoEmpresaGrupo,
                               tbl.nom_parte           as DescricaoEmpresaGrupo,
                               tbl.estadoComarca       as Uf,
                               tbl.nom_fornecedor      as DescricaoFornecedor,
                               tbl.cod_fornecedor      as CodigoFornecedor,
                               tbl.dsc_forma_pagamento as DescricaoFormaPagamento,
                               tbl.cod_forma_pagamento as CodigoFormaPagamento,
                               cc.dsc_centro_custo     as DescricaoCentroCusto,
                               cc.cod_centro_custo     as CodigoCentroCusto,
                               p.COD_CENTRO_SAP        as CentroSAP,
                               tbl.qtd_lancamentos     as TotalLancamneto,
                               f.cod_fornecedor_sap    as FornecedorSAP,
                               fp.ind_bordero          as IndicaBordero,
                               rownum                  r2
                          from (SELECT jur.processo.cod_tipo_processo,
                                       jur.processo.cod_parte_empresa,
                                       decode(forn.DATA, null, '', c.cod_estado) cod_estado,
                                       jur.parte.nom_parte,
                                       jur.lancamento_processo.cod_forma_pagamento,
                                       jur.forma_pagamento.dsc_forma_pagamento,
                                       jur.lancamento_processo.cod_centro_custo,
                                       jur.centro_custo.cod_centro_custo_sap cod_centro_custo_sap,
                                       jur.lancamento_processo.cod_fornecedor,
                                       jur.fornecedor.nom_fornecedor,
                                       jur.grupos_lotes_juizados.codigo cod_grupo_lote_juizado,
                                       jur.grupos_lotes_juizados.descricao dsc_grupo_lote_juizado,
                                       count(*) as qtd_lancamentos,
                                       c.cod_estado as estadoComarca
                                  FROM jur.centro_custo,
                                       jur.forma_pagamento,
                                       jur.fornecedor,
                                       jur.lancamento_processo,
                                       jur.parte,
                                       jur.processo,
                                       jur.profissional,
                                       jur.grupos_lotes_juizados,
                                       jur.empresas_sap es,
                                       jur.comarca c,
                                       jur.empresas_centralizadoras ec,
                                       THE (SELECT CAST(JUR.F_OBTEM_DADOS((select dsc_conteudo_parametro
                                                                            from jur.parametro_juridico
                                                                           where cod_parametro =
                                                                                 'SAP_FORNECEDOR_BB'),
                                                                          '|') AS
                                                        JUR.MYTABLETYPE)
                                              FROM DUAL) FORN
                                 WHERE (jur.lancamento_processo.cod_fornecedor =
                                       jur.fornecedor.cod_fornecedor)
                                   and (jur.lancamento_processo.cod_forma_pagamento =
                                       jur.forma_pagamento.cod_forma_pagamento)
                                   and (jur.lancamento_processo.cod_centro_custo =
                                       jur.centro_custo.cod_centro_custo)
                                   and (jur.processo.cod_parte_empresa = jur.parte.cod_parte)
                                   and (jur.processo.cod_processo =
                                       jur.lancamento_processo.cod_processo)

                                   and ( jur.processo.cod_profissional =
                                        jur.profissional.cod_profissional (+) )
                                   and( jur.grupos_lotes_juizados.codigo =
                                        jur.profissional.glj_cod_grupo_lote_juizado)

                                   and (jur.lancamento_processo.cod_status_pagamento in
                                       (1, 5, 10))
                                   and (jur.processo.cod_tipo_processo = {0})
                                    and ( trunc(jur.lancamento_processo.dat_lancamento) >= decode({1}, null, to_date('01/01/2000', 'dd/mm/yyyy'), {1}))
                                   and( trunc(jur.lancamento_processo.dat_lancamento) <= decode({2}, null, trunc(sysdate + 1), {2}) )
			                       and( jur.lancamento_processo.val_lancamento >= decode({3}, null, 0, {3}) )
			                       and( jur.lancamento_processo.val_lancamento <= decode({4}, null, 999999999999999, {4}) )
                                  and  (jur.processo.ind_processo_ativo <> 'E' )
                                   and (jur.lancamento_processo.ind_excluido = 'N')
                                   and (jur.parte.esap_cod_empresa_sap = es.codigo(+))
                                   and (jur.processo.cod_comarca = c.cod_comarca)
                                   and (jur.lancamento_processo.cod_fornecedor = forn.DATA(+))
                                   and (jur.parte.empce_cod_emp_centralizadora = ec.codigo)
                                   and (ec.codigo = {5})
                                 GROUP BY jur.processo.cod_tipo_processo,
                                          jur.processo.cod_parte_empresa,
                                          decode(forn.DATA, null, '', c.cod_estado),
                                          jur.parte.nom_parte,
                                          jur.lancamento_processo.cod_forma_pagamento,
                                          jur.forma_pagamento.dsc_forma_pagamento,
                                          jur.lancamento_processo.cod_centro_custo,
                                          jur.centro_custo.cod_centro_custo_sap,
                                          jur.lancamento_processo.cod_fornecedor,
                                          jur.fornecedor.nom_fornecedor,
                                          jur.grupos_lotes_juizados.codigo,
                                         jur.grupos_lotes_juizados.descricao,
                                          c.cod_estado) tbl,
                               jur.centro_custo cc,
                               jur.fornecedor f,
                               jur.parte p,
                               jur.forma_pagamento fp
                         where cc.cod_centro_custo = tbl.cod_centro_custo
                           and tbl.cod_fornecedor = f.cod_fornecedor
                           and tbl.cod_parte_empresa = p.COD_PARTE
                           and tbl.cod_forma_pagamento = fp.cod_forma_pagamento) l
                 
                   
                 order by l.DescricaoEmpresaGrupo ";
                    return await this.context.Query<LoteCriacaoResultadoEmpresaGrupoDTO>().FromSql(sql, filtros.CodigoTipoProcesso, filtros.DataCriacaoLancamentoInicio, filtros.DataCriacaoLancamentoFim, filtros.ValorLancamentoInicio, filtros.ValorLancamentoFim, filtros.CodigoEmpresaCentralizadora).ToListAsync();

                //civel estrategico e Trabalhista
                default:
                    sql = @"SELECT l.CentroSAP,
                       l.CodigoEmpresaGrupo,
                       l.DescricaoCentroCusto,
                       l.CodigoCentroCusto,
                       l.DescricaoEmpresaGrupo,
                       l.DescricaoFormaPagamento,
                       l.CodigoFormaPagamento,
                       l.DescricaoFornecedor,
                       l.CodigoFornecedor,
                       l.TotalLancamneto,
                       l.Uf,
                       l.FornecedorSAP,
                       l.IndicaBordero
                  FROM (select tbl.cod_parte_empresa   as CodigoEmpresaGrupo,
                               tbl.nom_parte           as DescricaoEmpresaGrupo,
                               tbl.estadoComarca       as Uf,
                               tbl.nom_fornecedor      as DescricaoFornecedor,
                               tbl.cod_fornecedor      as CodigoFornecedor,
                               tbl.dsc_forma_pagamento as DescricaoFormaPagamento,
                               tbl.cod_forma_pagamento as CodigoFormaPagamento,
                               cc.dsc_centro_custo     as DescricaoCentroCusto,
                               cc.cod_centro_custo     as CodigoCentroCusto,
                               p.COD_CENTRO_SAP        as CentroSAP,
                               tbl.qtd_lancamentos     as TotalLancamneto,
                               f.cod_fornecedor_sap    as FornecedorSAP,
                               fp.ind_bordero          as IndicaBordero,
                               rownum                  r2
                          from (SELECT jur.processo.cod_tipo_processo,
                                       jur.processo.cod_parte_empresa,
                                       decode(forn.DATA, null, '', c.cod_estado) cod_estado,
                                       jur.parte.nom_parte,
                                       jur.lancamento_processo.cod_forma_pagamento,
                                       jur.forma_pagamento.dsc_forma_pagamento,
                                       jur.lancamento_processo.cod_centro_custo,
                                       jur.centro_custo.cod_centro_custo_sap cod_centro_custo_sap,
                                       jur.lancamento_processo.cod_fornecedor,
                                       jur.fornecedor.nom_fornecedor,
                                       count(*) as qtd_lancamentos,
                                       c.cod_estado as estadoComarca
                                  FROM jur.centro_custo,
                                       jur.forma_pagamento,
                                       jur.fornecedor,
                                       jur.lancamento_processo,
                                       jur.parte,
                                       jur.processo,
                                       jur.empresas_sap es,
                                       jur.comarca c,
                                       jur.empresas_centralizadoras ec,
                                       THE (SELECT CAST(JUR.F_OBTEM_DADOS((select dsc_conteudo_parametro
                                                                            from jur.parametro_juridico
                                                                           where cod_parametro =
                                                                                 'SAP_FORNECEDOR_BB'),
                                                                          '|') AS
                                                        JUR.MYTABLETYPE)
                                              FROM DUAL) FORN
                                 WHERE (jur.lancamento_processo.cod_fornecedor =
                                       jur.fornecedor.cod_fornecedor)
                                   and (jur.lancamento_processo.cod_forma_pagamento =
                                       jur.forma_pagamento.cod_forma_pagamento)
                                   and (jur.lancamento_processo.cod_centro_custo =
                                       jur.centro_custo.cod_centro_custo)
                                   and (jur.processo.cod_parte_empresa = jur.parte.cod_parte)
                                   and (jur.processo.cod_processo =
                                       jur.lancamento_processo.cod_processo)
                                   and (jur.lancamento_processo.cod_status_pagamento in
                                       (1, 5, 10))
                                   and (jur.processo.cod_tipo_processo = {0})
                                   and ( trunc(jur.lancamento_processo.dat_lancamento) >= decode({1}, null, to_date('01/01/2000', 'dd/mm/yyyy'), {1}))
                                   and( trunc(jur.lancamento_processo.dat_lancamento) <= decode({2}, null, trunc(sysdate + 1), {2}) )
			                       and( jur.lancamento_processo.val_lancamento >= decode({3}, null, 0, {3}) )
			                       and( jur.lancamento_processo.val_lancamento <= decode({4}, null, 999999999999999, {4}) )
                                   and (jur.lancamento_processo.ind_excluido = 'N')
                                   and (jur.parte.esap_cod_empresa_sap = es.codigo(+))
                                   and (jur.processo.cod_comarca = c.cod_comarca)
                                   and (jur.lancamento_processo.cod_fornecedor = forn.DATA(+))
                                   and (jur.parte.empce_cod_emp_centralizadora = ec.codigo)
                                   and (ec.codigo = {5})
                                 GROUP BY jur.processo.cod_tipo_processo,
                                          jur.processo.cod_parte_empresa,
                                          decode(forn.DATA, null, '', c.cod_estado),
                                          jur.parte.nom_parte,
                                          jur.lancamento_processo.cod_forma_pagamento,
                                          jur.forma_pagamento.dsc_forma_pagamento,
                                          jur.lancamento_processo.cod_centro_custo,
                                          jur.centro_custo.cod_centro_custo_sap,
                                          jur.lancamento_processo.cod_fornecedor,
                                          jur.fornecedor.nom_fornecedor,
                                          c.cod_estado) tbl,
                               jur.centro_custo cc,
                               jur.fornecedor f,
                               jur.parte p,
                               jur.forma_pagamento fp
                         where cc.cod_centro_custo = tbl.cod_centro_custo
                           and tbl.cod_fornecedor = f.cod_fornecedor
                           and tbl.cod_parte_empresa = p.COD_PARTE
                           and tbl.cod_forma_pagamento = fp.cod_forma_pagamento) l
                 
                   
                  order by l.DescricaoEmpresaGrupo";

                    return await this.context.Query<LoteCriacaoResultadoEmpresaGrupoDTO>().FromSql(sql, filtros.CodigoTipoProcesso, filtros.DataCriacaoLancamentoInicio, filtros.DataCriacaoLancamentoFim, filtros.ValorLancamentoInicio, filtros.ValorLancamentoFim, filtros.CodigoEmpresaCentralizadora).ToListAsync();
            }
        }

        public async Task<int> ObterTotalLotesAgrupadoPorEmpresadoGrupo(LoteCriacaoFiltroEmpresaGrupoDTO filtros)
        {
            string sql;
            switch (filtros.CodigoTipoProcesso)
            {
                //Consumidor
                case 1:
                    sql = @"SELECT l.CentroSAP,
                       l.CodigoEmpresaGrupo,
                       l.DescricaoCentroCusto,
                       l.CodigoCentroCusto,
                       l.DescricaoEmpresaGrupo,
                       l.DescricaoFormaPagamento,
                       l.CodigoFormaPagamento,
                       l.DescricaoFornecedor,
                       l.CodigoFornecedor,
                       l.TotalLancamneto,
                       l.Uf,
                       l.FornecedorSAP,
                       l.IndicaBordero
                  FROM (select tbl.cod_parte_empresa   as CodigoEmpresaGrupo,
                               tbl.nom_parte           as DescricaoEmpresaGrupo,
                               tbl.estadoComarca       as Uf,
                               tbl.nom_fornecedor      as DescricaoFornecedor,
                               tbl.cod_fornecedor      as CodigoFornecedor,
                               tbl.dsc_forma_pagamento as DescricaoFormaPagamento,
                               tbl.cod_forma_pagamento as CodigoFormaPagamento,
                               cc.dsc_centro_custo     as DescricaoCentroCusto,
                               cc.cod_centro_custo     as CodigoCentroCusto,
                               p.COD_CENTRO_SAP        as CentroSAP,
                               tbl.qtd_lancamentos     as TotalLancamneto,
                               f.cod_fornecedor_sap    as FornecedorSAP,
                               fp.ind_bordero          as IndicaBordero,
                               rownum                  r2
                          from (SELECT jur.processo.cod_tipo_processo,
                                       jur.processo.cod_parte_empresa,
                                       decode(forn.DATA, null, '', c.cod_estado) cod_estado,
                                       jur.parte.nom_parte,
                                       jur.lancamento_processo.cod_forma_pagamento,
                                       jur.forma_pagamento.dsc_forma_pagamento,
                                       jur.lancamento_processo.cod_centro_custo,
                                       jur.centro_custo.cod_centro_custo_sap,
                                       jur.lancamento_processo.cod_fornecedor,
                                       jur.fornecedor.nom_fornecedor,
                                       count(*) as qtd_lancamentos,
                                       c.cod_estado as estadoComarca
                                  FROM jur.centro_custo,
                                       jur.forma_pagamento,
                                       jur.fornecedor,
                                       jur.lancamento_processo,
                                       jur.parte,
                                       jur.processo,
                                       jur.empresas_sap es,
                                       jur.comarca c,
                                       jur.empresas_centralizadoras ec,
                                       THE (SELECT CAST(JUR.F_OBTEM_DADOS((select dsc_conteudo_parametro
                                                                            from jur.parametro_juridico
                                                                           where cod_parametro =
                                                                                 'SAP_FORNECEDOR_BB'),
                                                                          '|') AS
                                                        JUR.MYTABLETYPE)
                                              FROM DUAL) FORN
                                 WHERE (jur.lancamento_processo.cod_fornecedor =
                                       jur.fornecedor.cod_fornecedor)
                                   and (jur.lancamento_processo.cod_forma_pagamento =
                                       jur.forma_pagamento.cod_forma_pagamento)
                                   and (jur.lancamento_processo.cod_centro_custo =
                                       jur.centro_custo.cod_centro_custo)
                                   and (jur.processo.cod_parte_empresa = jur.parte.cod_parte)
                                   and (jur.processo.cod_processo =
                                       jur.lancamento_processo.cod_processo)
                                   and (jur.lancamento_processo.cod_status_pagamento in
                                       (1, 5, 10))
                                   and (jur.processo.cod_tipo_processo = {0})
                                   and ( trunc(jur.lancamento_processo.dat_lancamento) >= decode({1}, null, to_date('01/01/2000', 'dd/mm/yyyy'), {1}))
                                   and( trunc(jur.lancamento_processo.dat_lancamento) <= decode({2}, null, trunc(sysdate + 1), {2}) )
			                       and( jur.lancamento_processo.val_lancamento >= decode({3}, null, 0, {3}) )
			                       and( jur.lancamento_processo.val_lancamento <= decode({4}, null, 999999999999999, {4}) )
                                   and (jur.lancamento_processo.ind_excluido = 'N')
                                   and (jur.parte.esap_cod_empresa_sap = es.codigo(+))
                                   and (jur.processo.cod_comarca = c.cod_comarca)
                                   and (jur.lancamento_processo.cod_fornecedor = forn.DATA(+))
                                   and (jur.parte.empce_cod_emp_centralizadora = ec.codigo)
                                   and (ec.codigo = {5})
                                 GROUP BY jur.processo.cod_tipo_processo,
                                          jur.processo.cod_parte_empresa,
                                          decode(forn.DATA, null, '', c.cod_estado),
                                          jur.parte.nom_parte,
                                          jur.lancamento_processo.cod_forma_pagamento,
                                          jur.forma_pagamento.dsc_forma_pagamento,
                                          jur.lancamento_processo.cod_centro_custo,
                                          jur.centro_custo.cod_centro_custo_sap,
                                          jur.lancamento_processo.cod_fornecedor,
                                          jur.fornecedor.nom_fornecedor,
                                          c.cod_estado) tbl,
                               jur.centro_custo cc,
                               jur.fornecedor f,
                               jur.parte p,
                               jur.forma_pagamento fp
                         where cc.cod_centro_custo = tbl.cod_centro_custo
                           and tbl.cod_fornecedor = f.cod_fornecedor
                           and tbl.cod_parte_empresa = p.COD_PARTE
                           and tbl.cod_forma_pagamento = fp.cod_forma_pagamento) l";

                    return await this.context.Query<LoteCriacaoResultadoEmpresaGrupoDTO>().FromSql(sql, filtros.CodigoTipoProcesso, filtros.DataCriacaoLancamentoInicio, filtros.DataCriacaoLancamentoFim, filtros.ValorLancamentoInicio, filtros.ValorLancamentoFim, filtros.CodigoEmpresaCentralizadora).CountAsync();

                //PEX
                case 18:
                    sql = @"SELECT l.CentroSAP,
                       l.CodigoEmpresaGrupo,
                       l.DescricaoCentroCusto,
                       l.CodigoCentroCusto,
                       l.DescricaoEmpresaGrupo,
                       l.DescricaoFormaPagamento,
                       l.CodigoFormaPagamento,
                       l.DescricaoFornecedor,
                       l.CodigoFornecedor,
                       l.TotalLancamneto,
                       l.Uf,
                       l.FornecedorSAP,
                       l.IndicaBordero
                  FROM (select tbl.cod_parte_empresa   as CodigoEmpresaGrupo,
                               tbl.nom_parte           as DescricaoEmpresaGrupo,
                               tbl.estadoComarca       as Uf,
                               tbl.nom_fornecedor      as DescricaoFornecedor,
                               tbl.cod_fornecedor      as CodigoFornecedor,
                               tbl.dsc_forma_pagamento as DescricaoFormaPagamento,
                               tbl.cod_forma_pagamento as CodigoFormaPagamento,
                               cc.dsc_centro_custo     as DescricaoCentroCusto,
                               cc.cod_centro_custo     as CodigoCentroCusto,
                               p.COD_CENTRO_SAP        as CentroSAP,
                               tbl.qtd_lancamentos     as TotalLancamneto,
                               f.cod_fornecedor_sap    as FornecedorSAP,
                               fp.ind_bordero          as IndicaBordero,
                               rownum                  r2
                          from (SELECT jur.processo.cod_tipo_processo,
                                       jur.processo.cod_parte_empresa,
                                       decode(forn.DATA, null, '', c.cod_estado) cod_estado,
                                       jur.parte.nom_parte,
                                       jur.lancamento_processo.cod_forma_pagamento,
                                       jur.forma_pagamento.dsc_forma_pagamento,
                                       jur.lancamento_processo.cod_centro_custo,
                                       jur.centro_custo.cod_centro_custo_sap cod_centro_custo_sap,
                                       jur.lancamento_processo.cod_fornecedor,
                                       jur.fornecedor.nom_fornecedor,
                                       count(*) as qtd_lancamentos,
                                       c.cod_estado as estadoComarca
                                  FROM jur.centro_custo,
                                       jur.forma_pagamento,
                                       jur.fornecedor,
                                       jur.lancamento_processo,
                                       jur.parte,
                                       jur.processo,
                                       jur.empresas_sap es,
                                       jur.comarca c,
                                       jur.empresas_centralizadoras ec,
                                       THE (SELECT CAST(JUR.F_OBTEM_DADOS((select dsc_conteudo_parametro
                                                                            from jur.parametro_juridico
                                                                           where cod_parametro =
                                                                                 'SAP_FORNECEDOR_BB'),
                                                                          '|') AS
                                                        JUR.MYTABLETYPE)
                                              FROM DUAL) FORN
                                 WHERE (jur.lancamento_processo.cod_fornecedor =
                                       jur.fornecedor.cod_fornecedor)
                                   and (jur.lancamento_processo.cod_forma_pagamento =
                                       jur.forma_pagamento.cod_forma_pagamento)
                                   and (jur.lancamento_processo.cod_centro_custo =
                                       jur.centro_custo.cod_centro_custo)
                                   and (jur.processo.cod_parte_empresa = jur.parte.cod_parte)
                                   and (jur.processo.cod_processo =
                                       jur.lancamento_processo.cod_processo)
                                   and (jur.lancamento_processo.cod_status_pagamento in
                                       (1, 5, 10))
                                   and (jur.processo.cod_tipo_processo = {0})

                                   and ( trunc(jur.lancamento_processo.dat_lancamento) >= decode({1}, null, to_date('01/01/2000', 'dd/mm/yyyy'), {1}))
                                   and( trunc(jur.lancamento_processo.dat_lancamento) <= decode({2}, null, trunc(sysdate + 1), {2}) )
			                       and( jur.lancamento_processo.val_lancamento >= decode({3}, null, 0, {3}) )
			                       and( jur.lancamento_processo.val_lancamento <= decode({4}, null, 999999999999999, {4}) )

                                   and (jur.lancamento_processo.ind_excluido = 'N')
                                   and (jur.parte.esap_cod_empresa_sap = es.codigo(+))
                                   and (jur.processo.cod_comarca = c.cod_comarca)
                                   and (jur.lancamento_processo.cod_fornecedor = forn.DATA(+))
                                   and (jur.parte.empce_cod_emp_centralizadora = ec.codigo)
                                   and (ec.codigo = {5})
                                 GROUP BY jur.processo.cod_tipo_processo,
                                          jur.processo.cod_parte_empresa,
                                          decode(forn.DATA, null, '', c.cod_estado),
                                          jur.parte.nom_parte,
                                          jur.lancamento_processo.cod_forma_pagamento,
                                          jur.forma_pagamento.dsc_forma_pagamento,
                                          jur.lancamento_processo.cod_centro_custo,
                                          jur.centro_custo.cod_centro_custo_sap,
                                          jur.lancamento_processo.cod_fornecedor,
                                          jur.fornecedor.nom_fornecedor,
                                          c.cod_estado) tbl,
                               jur.centro_custo cc,
                               jur.fornecedor f,
                               jur.parte p,
                               jur.forma_pagamento fp
                         where cc.cod_centro_custo = tbl.cod_centro_custo
                           and tbl.cod_fornecedor = f.cod_fornecedor
                           and tbl.cod_parte_empresa = p.COD_PARTE
                           and tbl.cod_forma_pagamento = fp.cod_forma_pagamento) l";

                    return await this.context.Query<LoteCriacaoResultadoEmpresaGrupoDTO>().FromSql(sql, filtros.CodigoTipoProcesso, filtros.DataCriacaoLancamentoInicio, filtros.DataCriacaoLancamentoFim, filtros.ValorLancamentoInicio, filtros.ValorLancamentoFim, filtros.CodigoEmpresaCentralizadora).CountAsync();

                //Juizado
                case 7:
                    sql = @"SELECT l.CentroSAP,
                       l.CodigoEmpresaGrupo,
                       l.DescricaoCentroCusto,
                       l.CodigoCentroCusto,
                       l.DescricaoEmpresaGrupo,
                       l.DescricaoFormaPagamento,
                       l.CodigoFormaPagamento,
                       l.DescricaoFornecedor,
                       l.CodigoFornecedor,
                       l.TotalLancamneto,
                       l.Uf,
                       l.FornecedorSAP,
                       l.IndicaBordero
                  FROM (select tbl.cod_parte_empresa   as CodigoEmpresaGrupo,
                               tbl.nom_parte           as DescricaoEmpresaGrupo,
                               tbl.estadoComarca       as Uf,
                               tbl.nom_fornecedor      as DescricaoFornecedor,
                               tbl.cod_fornecedor      as CodigoFornecedor,
                               tbl.dsc_forma_pagamento as DescricaoFormaPagamento,
                               tbl.cod_forma_pagamento as CodigoFormaPagamento,
                               cc.dsc_centro_custo     as DescricaoCentroCusto,
                               cc.cod_centro_custo     as CodigoCentroCusto,
                               p.COD_CENTRO_SAP        as CentroSAP,
                               tbl.qtd_lancamentos     as TotalLancamneto,
                               f.cod_fornecedor_sap    as FornecedorSAP,
                               fp.ind_bordero          as IndicaBordero,
                               rownum                  r2
                          from (SELECT jur.processo.cod_tipo_processo,
                                       jur.processo.cod_parte_empresa,
                                       decode(forn.DATA, null, '', c.cod_estado) cod_estado,
                                       jur.parte.nom_parte,
                                       jur.lancamento_processo.cod_forma_pagamento,
                                       jur.forma_pagamento.dsc_forma_pagamento,
                                       jur.lancamento_processo.cod_centro_custo,
                                       jur.centro_custo.cod_centro_custo_sap cod_centro_custo_sap,
                                       jur.lancamento_processo.cod_fornecedor,
                                       jur.fornecedor.nom_fornecedor,
                                       jur.grupos_lotes_juizados.codigo cod_grupo_lote_juizado,
                                       jur.grupos_lotes_juizados.descricao dsc_grupo_lote_juizado,
                                       count(*) as qtd_lancamentos,
                                       c.cod_estado as estadoComarca
                                  FROM jur.centro_custo,
                                       jur.forma_pagamento,
                                       jur.fornecedor,
                                       jur.lancamento_processo,
                                       jur.parte,
                                       jur.processo,
                                       jur.profissional,
                                       jur.grupos_lotes_juizados,
                                       jur.empresas_sap es,
                                       jur.comarca c,
                                       jur.empresas_centralizadoras ec,
                                       THE (SELECT CAST(JUR.F_OBTEM_DADOS((select dsc_conteudo_parametro
                                                                            from jur.parametro_juridico
                                                                           where cod_parametro =
                                                                                 'SAP_FORNECEDOR_BB'),
                                                                          '|') AS
                                                        JUR.MYTABLETYPE)
                                              FROM DUAL) FORN
                                 WHERE (jur.lancamento_processo.cod_fornecedor =
                                       jur.fornecedor.cod_fornecedor)
                                   and (jur.lancamento_processo.cod_forma_pagamento =
                                       jur.forma_pagamento.cod_forma_pagamento)
                                   and (jur.lancamento_processo.cod_centro_custo =
                                       jur.centro_custo.cod_centro_custo)
                                   and (jur.processo.cod_parte_empresa = jur.parte.cod_parte)
                                   and (jur.processo.cod_processo =
                                       jur.lancamento_processo.cod_processo)

                                   and ( jur.processo.cod_profissional =
                                        jur.profissional.cod_profissional (+) )
                                   and( jur.grupos_lotes_juizados.codigo =
                                        jur.profissional.glj_cod_grupo_lote_juizado)

                                   and (jur.lancamento_processo.cod_status_pagamento in
                                       (1, 5, 10))
                                   and (jur.processo.cod_tipo_processo = {0})
                                 and ( trunc(jur.lancamento_processo.dat_lancamento) >= decode({1}, null, to_date('01/01/2000', 'dd/mm/yyyy'), {1}))
                                   and( trunc(jur.lancamento_processo.dat_lancamento) <= decode({2}, null, trunc(sysdate + 1), {2}) )
			                       and( jur.lancamento_processo.val_lancamento >= decode({3}, null, 0, {3}) )
			                       and( jur.lancamento_processo.val_lancamento <= decode({4}, null, 999999999999999, {4}) )
                                  and  (jur.processo.ind_processo_ativo <> 'E' )
                                   and (jur.lancamento_processo.ind_excluido = 'N')
                                   and (jur.parte.esap_cod_empresa_sap = es.codigo(+))
                                   and (jur.processo.cod_comarca = c.cod_comarca)
                                   and (jur.lancamento_processo.cod_fornecedor = forn.DATA(+))
                                   and (jur.parte.empce_cod_emp_centralizadora = ec.codigo)
                                   and (ec.codigo = {5})
                                 GROUP BY jur.processo.cod_tipo_processo,
                                          jur.processo.cod_parte_empresa,
                                          decode(forn.DATA, null, '', c.cod_estado),
                                          jur.parte.nom_parte,
                                          jur.lancamento_processo.cod_forma_pagamento,
                                          jur.forma_pagamento.dsc_forma_pagamento,
                                          jur.lancamento_processo.cod_centro_custo,
                                          jur.centro_custo.cod_centro_custo_sap,
                                          jur.lancamento_processo.cod_fornecedor,
                                          jur.fornecedor.nom_fornecedor,
                                          jur.grupos_lotes_juizados.codigo,
                                         jur.grupos_lotes_juizados.descricao,
                                          c.cod_estado) tbl,
                               jur.centro_custo cc,
                               jur.fornecedor f,
                               jur.parte p,
                               jur.forma_pagamento fp
                         where cc.cod_centro_custo = tbl.cod_centro_custo
                           and tbl.cod_fornecedor = f.cod_fornecedor
                           and tbl.cod_parte_empresa = p.COD_PARTE
                           and tbl.cod_forma_pagamento = fp.cod_forma_pagamento) l
                ";
                    return await this.context.Query<LoteCriacaoResultadoEmpresaGrupoDTO>().FromSql(sql, filtros.CodigoTipoProcesso, filtros.DataCriacaoLancamentoInicio, filtros.DataCriacaoLancamentoFim, filtros.ValorLancamentoInicio, filtros.ValorLancamentoFim, filtros.CodigoEmpresaCentralizadora).CountAsync();

                //civel estrategico e Trabalhista
                default:
                    sql = @"SELECT l.CentroSAP,
                       l.CodigoEmpresaGrupo,
                       l.DescricaoCentroCusto,
                       l.CodigoCentroCusto,
                       l.DescricaoEmpresaGrupo,
                       l.DescricaoFormaPagamento,
                       l.CodigoFormaPagamento,
                       l.DescricaoFornecedor,
                       l.CodigoFornecedor,
                       l.TotalLancamneto,
                       l.Uf,
                       l.FornecedorSAP,
                       l.IndicaBordero
                  FROM (select tbl.cod_parte_empresa   as CodigoEmpresaGrupo,
                               tbl.nom_parte           as DescricaoEmpresaGrupo,
                               tbl.estadoComarca       as Uf,
                               tbl.nom_fornecedor      as DescricaoFornecedor,
                               tbl.cod_fornecedor      as CodigoFornecedor,
                               tbl.dsc_forma_pagamento as DescricaoFormaPagamento,
                               tbl.cod_forma_pagamento as CodigoFormaPagamento,
                               cc.dsc_centro_custo     as DescricaoCentroCusto,
                               cc.cod_centro_custo     as CodigoCentroCusto,
                               p.COD_CENTRO_SAP        as CentroSAP,
                               tbl.qtd_lancamentos     as TotalLancamneto,
                               f.cod_fornecedor_sap    as FornecedorSAP,
                               fp.ind_bordero          as IndicaBordero,
                               rownum                  r2
                          from (SELECT jur.processo.cod_tipo_processo,
                                       jur.processo.cod_parte_empresa,
                                       decode(forn.DATA, null, '', c.cod_estado) cod_estado,
                                       jur.parte.nom_parte,
                                       jur.lancamento_processo.cod_forma_pagamento,
                                       jur.forma_pagamento.dsc_forma_pagamento,
                                       jur.lancamento_processo.cod_centro_custo,
                                       jur.centro_custo.cod_centro_custo_sap cod_centro_custo_sap,
                                       jur.lancamento_processo.cod_fornecedor,
                                       jur.fornecedor.nom_fornecedor,
                                       count(*) as qtd_lancamentos,
                                       c.cod_estado as estadoComarca
                                  FROM jur.centro_custo,
                                       jur.forma_pagamento,
                                       jur.fornecedor,
                                       jur.lancamento_processo,
                                       jur.parte,
                                       jur.processo,
                                       jur.empresas_sap es,
                                       jur.comarca c,
                                       jur.empresas_centralizadoras ec,
                                       THE (SELECT CAST(JUR.F_OBTEM_DADOS((select dsc_conteudo_parametro
                                                                            from jur.parametro_juridico
                                                                           where cod_parametro =
                                                                                 'SAP_FORNECEDOR_BB'),
                                                                          '|') AS
                                                        JUR.MYTABLETYPE)
                                              FROM DUAL) FORN
                                 WHERE (jur.lancamento_processo.cod_fornecedor =
                                       jur.fornecedor.cod_fornecedor)
                                   and (jur.lancamento_processo.cod_forma_pagamento =
                                       jur.forma_pagamento.cod_forma_pagamento)
                                   and (jur.lancamento_processo.cod_centro_custo =
                                       jur.centro_custo.cod_centro_custo)
                                   and (jur.processo.cod_parte_empresa = jur.parte.cod_parte)
                                   and (jur.processo.cod_processo =
                                       jur.lancamento_processo.cod_processo)
                                   and (jur.lancamento_processo.cod_status_pagamento in
                                       (1, 5, 10))
                                   and (jur.processo.cod_tipo_processo = {0})
                                   and ( trunc(jur.lancamento_processo.dat_lancamento) >= decode({1}, null, to_date('01/01/2000', 'dd/mm/yyyy'), {1}))
                                   and( trunc(jur.lancamento_processo.dat_lancamento) <= decode({2}, null, trunc(sysdate + 1), {2}) )
			                       and( jur.lancamento_processo.val_lancamento >= decode({3}, null, 0, {3}) )
			                       and( jur.lancamento_processo.val_lancamento <= decode({4}, null, 999999999999999, {4}) )
                                   and (jur.lancamento_processo.ind_excluido = 'N')
                                   and (jur.parte.esap_cod_empresa_sap = es.codigo(+))
                                   and (jur.processo.cod_comarca = c.cod_comarca)
                                   and (jur.lancamento_processo.cod_fornecedor = forn.DATA(+))
                                   and (jur.parte.empce_cod_emp_centralizadora = ec.codigo)
                                   and (ec.codigo = {5})
                                 GROUP BY jur.processo.cod_tipo_processo,
                                          jur.processo.cod_parte_empresa,
                                          decode(forn.DATA, null, '', c.cod_estado),
                                          jur.parte.nom_parte,
                                          jur.lancamento_processo.cod_forma_pagamento,
                                          jur.forma_pagamento.dsc_forma_pagamento,
                                          jur.lancamento_processo.cod_centro_custo,
                                          jur.centro_custo.cod_centro_custo_sap,
                                          jur.lancamento_processo.cod_fornecedor,
                                          jur.fornecedor.nom_fornecedor,
                                          c.cod_estado) tbl,
                               jur.centro_custo cc,
                               jur.fornecedor f,
                               jur.parte p,
                               jur.forma_pagamento fp
                         where cc.cod_centro_custo = tbl.cod_centro_custo
                           and tbl.cod_fornecedor = f.cod_fornecedor
                           and tbl.cod_parte_empresa = p.COD_PARTE
                           and tbl.cod_forma_pagamento = fp.cod_forma_pagamento) l
                ";

                    return await this.context.Query<LoteCriacaoResultadoEmpresaGrupoDTO>().FromSql(sql, filtros.CodigoTipoProcesso, filtros.DataCriacaoLancamentoInicio, filtros.DataCriacaoLancamentoFim, filtros.ValorLancamentoInicio, filtros.ValorLancamentoFim, filtros.CodigoEmpresaCentralizadora).CountAsync();
            }
        }

        public async Task<ICollection<LoteCriacaoResultadoLancamentoDTO>> ObterLancamentosLoteCriacao(LoteCriacaoFiltroLancamentoDTO filtros, ILogger logger)
        {

            var sql =string.Format(@"SELECT jur.lancamento_processo.cod_processo         as CodigoProcesso,
                               jur.lancamento_processo.cod_lancamento       as CodigoLancamento,
                               jur.processo.nro_processo_cartorio           as NumeroProcesso,
                               jur.comarca.cod_estado                       as Uf,
                               jur.processo.cod_comarca                     as CodigoComarca,
                               jur.comarca.nom_comarca                      as NomeComarca,
                               jur.processo.cod_vara                        as CodigoVara,
                               jur.processo.cod_tipo_vara                   as CodigoTipoVara,
                               jur.tipo_vara.nom_tipo_vara                  as NomeTipoVara,
                               jur.lancamento_processo.cod_tipo_lancamento  as CodigoTipoLancamento,
                               jur.tipo_lancamento.dsc_tipo_lancamento      as DescricaoTipoLancamento,
                               jur.lancamento_processo.cod_cat_pagamento    as CodigoCategoriaPagamento,
                               jur.categoria_pagamento.dsc_cat_pagamento    as DescricaoCategoriaPagamento,
                               jur.lancamento_processo.dat_lancamento       as DataCriacaoLancamento,
                               jur.lancamento_processo.qtd_lancamento       as QuantidadeLancamento,
                               jur.lancamento_processo.val_lancamento       as ValorLiquido,
                               jur.lancamento_processo.comentario_sap       as TextoSAP,
                               jur.parte.nom_parte                          as NomeParte,
                               jur.profissional.nom_profissional            as DescricaoEscritorio,
                               jur.lancamento_processo.nro_guia             as NumeroGuia,
                               jur.lancamento_processo.cod_status_pagamento as CodigoStatusPagamento,
                               jur.lancamento_processo.cod_parte            as CodigoParte
                  FROM jur.lancamento_processo,
                       jur.processo,
                       jur.comarca,
                       jur.tipo_vara,
                       jur.categoria_pagamento,
                       jur.tipo_lancamento,
                       jur.parte,
                       jur.profissional
                 WHERE jur.processo.cod_processo = jur.lancamento_processo.cod_processo
                   and jur.comarca.cod_comarca = jur.processo.cod_comarca
                   and jur.categoria_pagamento.cod_cat_pagamento =
                       jur.lancamento_processo.cod_cat_pagamento
                   and jur.tipo_lancamento.cod_tipo_lancamento =
                       jur.lancamento_processo.cod_tipo_lancamento
                   and jur.processo.cod_tipo_vara = jur.tipo_vara.cod_tipo_vara
                   and jur.lancamento_processo.cod_centro_custo = {0}
                   AND jur.lancamento_processo.cod_fornecedor = {1}
                   AND jur.lancamento_processo.cod_forma_pagamento ={2}
                   AND jur.lancamento_processo.cod_status_pagamento in (1,5,10)
                   AND jur.processo.cod_parte_empresa = {3}
                   AND jur.processo.cod_tipo_processo = {4}
                   AND jur.lancamento_processo.ind_excluido = 'N'                   
                   and jur.profissional.cod_profissional = jur.processo.cod_profissional
                   and jur.lancamento_processo.cod_parte = jur.parte.cod_parte(+)
                   and jur.comarca.cod_estado = '{5}'
                   {6}
                   {7}
                   {8}
                   {9}
                ORDER BY jur.profissional.nom_profissional, jur.parte.nom_parte   ", filtros.CodigoCentroCusto, filtros.CodigoFornecedor,
                filtros.CodigoFormaPagamento, filtros.CodigoEmpresaGrupo, filtros.CodigoTipoProcesso, filtros.Uf,
                filtros.DataInicialLancamento.HasValue ? "and jur.lancamento_processo.dat_lancamento >= to_date('" + filtros.DataInicialLancamento.Value.ToString("dd/MM/yyyy") + " 00:00:00" + "', 'dd/mm/yyyy HH24:MI:SS')" : null,
                filtros.DataFinalLancamento.HasValue ? "and jur.lancamento_processo.dat_lancamento <= to_date('" + filtros.DataFinalLancamento.Value.ToString("dd/MM/yyyy") + " 23:59:59" + "', 'dd/mm/yyyy HH24:MI:SS')" : null,
                filtros.ValorInicialLancamento.HasValue ? "and jur.lancamento_processo.val_lancamento >= " + filtros.ValorInicialLancamento.Value : null,
                filtros.ValorFinalLancamento.HasValue ? "and jur.lancamento_processo.val_lancamento <= " + filtros.ValorFinalLancamento.Value : null);

            Stopwatch s = new Stopwatch();            
            s.Start();
            logger.LogInformation("Inicio da consulta no banco");            
            var result = await this.context.Query<LoteCriacaoResultadoLancamentoDTO>().FromSql(sql).ToListAsync();
            s.Stop();            
           logger.LogInformation("tempo gasto: " + s.Elapsed.TotalSeconds + " em segundos");
            logger.LogInformation("Fim da consulta no banco");
            return result;
        }

        public async Task<LancamentoProcesso> ObterLancamentoProcesso(long CodigoProcesso, long CodigoLancamento)
        {
            var resultado = context.Set<LancamentoProcesso>()
                    .AsNoTracking()
                    .Where(l => l.Id == CodigoProcesso && l.CodigoLancamento == CodigoLancamento);
            var ret = await resultado.FirstOrDefaultAsync();
            return ret;
        }
        
        public async Task<bool> ExisteLancamentoProcessoComFornecedor(long codigoFornecedor)
        {
            var resultado = await context.Set<LancamentoProcesso>()
                .AnyAsync(l => l.CodigoFornecedor == codigoFornecedor);

            return resultado;
        }

        public async Task AlterarDataEnvioEscritorio(LancamentoProcesso lancamentoProcesso)
        {
            await base.Atualizar(lancamentoProcesso);
        }

        public async Task<long?> RecuperarNumeroContaJudicial(long numeroContaJudicial)
        {
            var retorno = await context.Set<LancamentoProcesso>().Where(processo => processo.NumeroContaJudicial.Equals(numeroContaJudicial))

                                   .AsNoTracking()
                                    .Select(p => p.NumeroContaJudicial)
                                    .FirstOrDefaultAsync();
            return retorno;
        }

        public async Task<CargaCompromissoParcela> AtualizarParcelaCompromisso(long codProcesso, long codLancamento)
        {
            try
            {
                var retorno = context.Set<CargaCompromissoParcela>().FirstOrDefault(x => x.CodProcesso == codProcesso && x.CodLancamento == codLancamento);
                retorno.Status = 10;  //Estorno
                retorno.ComentarioEstorno = "Pagamento estornado por " + user.Login + " em " + DateTime.UtcNow;
                context.Update(retorno);
                return retorno;
            }
            catch (Exception)
            {
                return null;                
            }   
        }

        public async Task<ICollection<LancamentoProcesso>> ConsultaLancamentoEstorno(long codigoProcesso, long codigoTipoProcesso)
        {
            long[] statusPagamentoCivelConsumidor = new long[] { StatusPagamentoEnum.PedidoSAPPago.GetHashCode(), StatusPagamentoEnum.PedidoSAPPagoManualmente.GetHashCode(), StatusPagamentoEnum.Estorno.GetHashCode(), StatusPagamentoEnum.PedidoSAPPagoLancamentoRetidoRJ.GetHashCode(), StatusPagamentoEnum.PedidoSAPRetido.GetHashCode() };
            long[] statusPagamento = new long[] { StatusPagamentoEnum.PedidoSAPPago.GetHashCode(), StatusPagamentoEnum.PedidoSAPPagoManualmente.GetHashCode(), StatusPagamentoEnum.Estorno.GetHashCode() };
            long[] tipoLancamentoCivelConsumidor = new long[] { TipoLancamentoEnum.Garantias.GetHashCode(), TipoLancamentoEnum.DespesasJudiciais.GetHashCode() };
            long[] tipoLancamento = new long[] { TipoLancamentoEnum.Garantias.GetHashCode(), TipoLancamentoEnum.DespesasJudiciais.GetHashCode(), TipoLancamentoEnum.Pagamentos.GetHashCode() };
            IQueryable<LancamentoProcesso> query = context.Set<LancamentoProcesso>()
                .Include(l => l.StatusPagamento)
                .Include(l => l.CategoriaPagamento)
                .Include(l => l.FormaPagamento)
                .Include(l => l.Fornecedor)
                .Include(l => l.CentroCusto)
                .Include(l => l.TipoLancamento)
                .Where(lp => !lp.IndicadorExluido && lp.Id == codigoProcesso);

            if (codigoTipoProcesso == TipoProcessoEnum.CivelConsumidor.GetHashCode())
            {
                query = query.Where(lp => (lp.CodigoTipoLancamento == 3 && statusPagamentoCivelConsumidor.Contains(lp.CodigoStatusPagamento))
                                           || (tipoLancamentoCivelConsumidor.Contains(lp.CodigoTipoLancamento) && statusPagamento.Contains(lp.CodigoStatusPagamento)));
            }
            else
            {
                query = query.Where(lp => tipoLancamento.Contains(lp.CodigoTipoLancamento) && statusPagamento.Contains(lp.CodigoStatusPagamento));
            }

            return await query.ToListAsync();
        }

        public async Task<bool> ExisteCategoriaPagAssociadoLancamento(long codigoCategoriaPagamento)
        {
            var result = await dbContext.LancamentoProcessos
                            .AnyAsync(lp => lp.CodigoCatPagamento == codigoCategoriaPagamento);
            return result;
        }

        public async Task<bool> ExisteCentroCustoAssociado(long codigoCentroCusto)
        {
            var result = await dbContext.LancamentoProcessos
                            .AnyAsync(lp => lp.CodigoCentroCusto == codigoCentroCusto);
            return result;
        }

        public async Task<bool> ExisteBBModalidadeAssociadoLancamento(long id)
        {
            var result = await dbContext.LancamentoProcessos
                            .AnyAsync(lp => lp.IdBBModalidade == id);
            return result;
        }

        public async Task<bool> ExisteLamentoProcessoAssociadoComStatusParcela(long CodigoBBStatusParcela)
        {
            var result = await dbContext.LancamentoProcessos
                            .AnyAsync(lp => lp.IdBbStatusParcela == CodigoBBStatusParcela);

            return result;
        }

        //To DO - Refatorar o metodo  para utilizar o entity
        //public async Task<ICollection<LoteCriacaoResultadoEmpresaCentralizadoraDTO>> ObterLotesAgrupadoPorEmpresaCentralizadora(LoteCriacaoFiltroDTO filtros) {
        //    IQueryable<LancamentoProcesso> query = IQuerybleLancamentoProcessoCriarLote(filtros);
        //    var resultado = await query
        //        .Select(lancamnetoProcesso => new LoteCriacaoResultadoEmpresaCentralizadoraDTO {
        //            CodigoEmpresaCentralizadora = lancamnetoProcesso.ParteProcesso.Parte.CodigoEmpresaCentralizadora.Value,
        //            DescricaoEmpresaCentralizadora = lancamnetoProcesso.ParteProcesso.Parte.Nome
        //        })
        //        .ToListAsync();
        //    return resultado;
        //}

        //private IQueryable<LancamentoProcesso> IQuerybleLancamentoProcessoCriarLote(LoteCriacaoFiltroDTO filtros) {
        //    long[] codigoStatusPagamento = new long[] { 1, 5, 10 };
        //    //To do - criar relacionamento entre processo e parte
        //    IQueryable<LancamentoProcesso> query = context.Set<LancamentoProcesso>()
        //                                           .Include(q => q.Fornecedor)
        //                                           .Include(q => q.FormaPagamento)
        //                                           .Include(q => q.CentroCusto)
        //                                           .Include(q => q.Processo)
        //                                           .ThenInclude(q => q.Comarca);

        //    query = query.Where(q => q.Processo.CodigoTipoProcesso == filtros.CodigoTipoProcesso &&
        //                             codigoStatusPagamento.Contains(q.CodigoStatusPagamento) &&
        //                             !q.IndicadorExluido);

        //    return query;
        //}
    }
}