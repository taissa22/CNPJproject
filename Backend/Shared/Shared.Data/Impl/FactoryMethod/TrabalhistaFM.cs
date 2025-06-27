using Shared.Data.Interface.FactoryMethod;
using System.Text;

namespace Shared.Data.Impl.FactoryMethod
{
    public class TrabalhistaFM : IQueryFM
    {
        public string RetornaFrom()
        {
            return @"from
                            jur.processo,
                            jur.comarca,
                            jur.parte,
                            jur.tipo_vara,
                            jur.lancamento_processo lp,
                            jur.categoria_pagamento cp,
                            jur.classes_garantias cg,
                            jur.banco b
                        ";
        }

        public string RetornaGroupBy()
        {
            return @"
                        group by 
                            jur.processo.cod_processo, 
                            jur.processo.nro_processo_cartorio, 
                            jur.comarca.cod_estado, 
                            jur.comarca.nom_comarca, 
                            jur.processo.cod_vara, 
                            jur.tipo_vara.nom_tipo_vara, 
                            jur.parte.nom_parte, 
                            b.nom_banco, 
                            decode(cg.tsgar_cod_tipo_saldo_garantia, 1, 'DEPÓSITO', 2, 'BLOQUEIO', 'OUTROS'), 
                            jur.processo.cod_profissional, 
                            jur.processo.dat_finalizacao_contabil, 
                            decode(jur.processo.ind_processo_ativo, 'S', 'True', 'False'), 
                            decode( jur.processo.ind_prioritaria, 'S', 'True', 'False' )
                    ";
        }

        public string RetornaJoin()
        {
            return @"
                        where  jur.comarca.cod_comarca            = jur.processo.cod_comarca
                            and    jur.parte.cod_parte            = jur.processo.cod_parte_empresa
                            and    jur.processo.cod_tipo_vara     = jur.tipo_vara.cod_tipo_vara
                            and    jur.processo.cod_processo      = lp.cod_processo
                            and    lp.cod_cat_pagamento           = cp.cod_cat_pagamento
                            and    cp.clgar_cod_classe_garantia   = cg.codigo
                            and    lp.bco_cod_banco           (+) = b.cod_banco
                            and    jur.processo.cod_tipo_processo = 2
                            and    lp.cod_status_pagamento <> '10'
                            and    lp.ind_excluido = 'N'                ";
        }

        public string RetornaSelect()
        {
            return @"
                        select distinct jur.processo.cod_processo IdProcesso,
                            jur.processo.nro_processo_cartorio NumeroProcesso,
                            jur.comarca.cod_estado CodigoEstado,
                            jur.comarca.nom_comarca DescricaoComarca,
                            jur.processo.cod_vara CodigoVara,
                            jur.tipo_vara.nom_tipo_vara DescricaoTipoVara,
                            jur.parte.nom_parte DescricaoEmpresaGrupo,
                            decode(jur.processo.ind_processo_ativo, 'S', 'True', 'False') as Ativo,
                            decode( jur.processo.ind_prioritaria, 'S', 'True', 'False' ) Estrategico,
                            b.nom_banco DescricaoBanco,
                            decode(cg.tsgar_cod_tipo_saldo_garantia, 1, 'DEPÓSITO', 2, 'BLOQUEIO', 'OUTROS') DescricaoTipoGarantia,
                            jur.processo.cod_profissional CodigoProfissional,
                            nvl(jur.processo.dat_finalizacao_contabil, '') DataFinalizacaoContabil,
                            sum(round(decode(cg.valor_multiplicador, 1, valor_principal, 0), 2)) ValorPrincipal,
                            sum(round(decode(cg.valor_multiplicador, 1, valor_correcao, 0), 2)) ValorCorrecaoPrincipal,
                            sum(round(decode(cg.valor_multiplicador, 1, valor_ajuste_correcao, 0), 2)) ValorAjusteCorrecao,
                            sum(round(decode(cg.valor_multiplicador, 1, valor_juros, 0), 2)) ValorJurosPrincipal,
                            sum(round(decode(cg.valor_multiplicador, 1, valor_ajuste_juros, 0), 2)) ValorAjusteJuros,
                            sum(round(decode(cg.tplc_cod_tipo_lancamento, 3, (-1) * valor_principal, 0), 2)) ValorPagamentoPrincipal,
                            sum(round(decode(cg.tplc_cod_tipo_lancamento, 3, (-1) * valor_correcao, 0), 2)) ValorPagamentoCorrecao,
                            sum(round(decode(cg.tplc_cod_tipo_lancamento, 3, (-1) * valor_juros, 0), 2)) ValorPagamentosJuros,
                            sum(round(decode(decode(cg.valor_multiplicador, -1, 1,0) + decode(cg.tplc_cod_tipo_lancamento, 3, 0,1), 2, (-1) * valor_principal, 0), 2)) ValorLevantadoPrincipal,
                            sum(round(decode(decode(cg.valor_multiplicador, -1, 1,0) + decode(cg.tplc_cod_tipo_lancamento, 3, 0,1), 2, (-1) * valor_correcao, 0), 2)) ValorLevantadoCorrecao,
                            sum(round(decode(decode(cg.valor_multiplicador, -1, 1,0) + decode(cg.tplc_cod_tipo_lancamento, 3, 0,1), 2, (-1) * valor_juros, 0), 2)) ValorLevantadoJuros";
        }

        public string RetornaWhere()
        {
            //não implementado por enquanto - add parametros de AgendamentoSaldoGarantia e Lista de OracleParameter
            throw new System.NotImplementedException();
        }
        public StringBuilder CalcularSaldoQuery(StringBuilder subQuery)
        {
            StringBuilder query = new StringBuilder();
            query.AppendLine(@"
                    select geral.*,
                        (geral.ValorPrincipal + geral.ValorPagamentoPrincipal + geral.ValorLevantadoPrincipal) as ValorSaldoPrincipal,
                        (geral.ValorCorrecaoPrincipal + geral.ValorPagamentoCorrecao + geral.ValorLevantadoCorrecao) as ValorSaldoCorrecao,
                        (geral.ValorJurosPrincipal + geral.ValorPagamentosJuros + geral.ValorLevantadoJuros) as ValorSaldoJuros     
                    from (
            ");
            query.AppendLine(subQuery.ToString());
            query.AppendLine(") geral");
            return query;
        }
        public string RetornaOrderBy()
        {
            return @" order by 
                   jur.processo.cod_processo asc";
        }
    }
}
