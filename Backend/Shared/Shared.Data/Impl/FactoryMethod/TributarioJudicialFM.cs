using Shared.Data.Interface.FactoryMethod;
using System.Text;

namespace Shared.Data.Impl.FactoryMethod
{
    public class TributarioJudicialFM : IQueryFM
    {
        public string RetornaFrom()
        {
            return @"
                    from jur.processo,   
                    jur.comarca,   
                    jur.parte,   
                    jur.tipo_vara
                        ";
        }

        public string RetornaGroupBy()
        {
            return @"
                ";
        }

        public string RetornaJoin()
        {
            return @"
                        WHERE 
                            ( jur.comarca.cod_comarca (+) = jur.processo.cod_comarca ) and  
                            ( jur.parte.cod_parte = jur.processo.cod_parte_empresa ) and  
                            ( jur.processo.cod_tipo_vara = jur.tipo_vara.cod_tipo_vara(+)) and
                            ( jur.processo.cod_tipo_processo = 5)
                ";
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
                            (SELECT  nvl(sum(nvl(round(lop.valor_pagamento,2),0)),0) val_pagamento
			                        FROM  jur.lancamento_processo pg,
					                          jur.lancamento_objeto_processo lop,   
				                        jur.categoria_pagamento cp
			                        WHERE pg.cod_processo                        = lop.cod_processo                              and
					                          pg.cod_lancamento                      = lop.cod_lancamento                            and
				                        pg.cod_cat_pagamento             = cp.cod_cat_pagamento                  and
				                        pg.cod_processo                = jur.processo.cod_processo               and 
				                        cp.ind_baixa_garantia             = 'S'                              and
				                        pg.cod_tipo_lancamento            = 3                              and   
				                        pg.cod_status_pagamento <> '10'                                      and    pg.ind_excluido = 'N'
                            ) ValorTotalPagoGarantia,  		                    
                                (   SELECT  
                                        nvl(sum( (round(nvl(pg1.val_lancamento,0), 2) + round(NVL(pg1.valor_correcao,0), 2) + round(nvl(pg1.valor_juros,0), 2) ) * round(nvl(cg.valor_multiplicador,0), 2)),0) val_lancamentos
			                        FROM  jur.lancamento_processo pg1,   
				                        jur.categoria_pagamento cp1,
					                          jur.tipos_saldos_garantias ts,
					                          jur.classes_garantias cg
			                        WHERE pg1.cod_cat_pagamento             = cp1.cod_cat_pagamento                  and
				                        pg1.cod_processo                = jur.processo.cod_processo               and
					                          cp1.clgar_cod_classe_garantia          = cg.codigo                                     and
					                          cg.tsgar_cod_tipo_saldo_garantia       = ts.codigo                                     and
					                          ts.ind_baixa_pagamento              = 'S'                              and
				                        pg1.cod_tipo_lancamento            = 1
				                        and pg1.cod_status_pagamento <> '10'  and    pg1.ind_excluido = 'N'  
                            ) SaldoDepositoBloqueio   
            ";
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
                        (geral.ValorTotalPagoGarantia - geral.SaldoDepositoBloqueio) as SaldoGarantia     
                    from (
            ");
            query.AppendLine(subQuery.ToString());
            query.AppendLine(") geral");
            return query;
        }
        public string RetornaOrderBy()
        {
            return @" order by 
                   jur.processo.nro_processo_cartorio asc";
        }
    }
}
