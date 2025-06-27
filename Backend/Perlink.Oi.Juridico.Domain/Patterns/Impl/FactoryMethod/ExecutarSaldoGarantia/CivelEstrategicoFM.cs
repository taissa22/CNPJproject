using Perlink.Oi.Juridico.Domain.Patterns.Interface.FactoryMethod;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.Patterns.Impl.FactoryMethod.ExecutarSaldoGarantia
{
    public class CivelEstrategicoFM : IQuerySaldoGarantiaFM
    {
        public string RetornaFrom()
        {
            return @"
                    FROM jur.processo,   
                         jur.comarca,   
                         jur.parte,   
                         jur.tipo_vara
                    ";
        }

        public string RetornaGroupBy()
        {
            return @"";
        }

        public string RetornaJoin()
        {
            return @"
                   where ( jur.comarca.cod_comarca  = jur.processo.cod_comarca         )   and  
                         ( jur.parte.cod_parte      = jur.processo.cod_parte_empresa   )   and  
                         ( jur.processo.cod_tipo_vara  = jur.tipo_vara.cod_tipo_vara   )   and  
                      ( jur.processo.cod_tipo_processo  = 9                            )
                    ";
        }

        public string RetornaSelect()
        {
            return @"
                        SELECT distinct jur.processo.cod_processo IdProcesso,   
                             jur.processo.nro_processo_cartorio NumeroProcesso,   
                             jur.comarca.cod_estado CodigoEstado,
                             jur.comarca.nom_comarca DescricaoComarca,   
                             jur.processo.cod_vara CodigoVara,   
                             jur.tipo_vara.nom_tipo_vara DescricaoTipoVara,   
                             jur.parte.nom_parte DescricaoEmpresaGrupo,   
                             decode(jur.processo.ind_processo_ativo, 'S', 'Sim', 'Não') as Ativo,
                          (SELECT  nvl(sum(nvl(round(pg.val_lancamento,2),0)),0) val_pagamento 
                            FROM  jur.lancamento_processo pg,   
                                jur.categoria_pagamento cp
                            WHERE pg.cod_cat_pagamento             = cp.cod_cat_pagamento                  and
                                pg.cod_processo                = jur.processo.cod_processo               and
                                cp.ind_baixa_garantia             = 'S'                              and
                                pg.cod_tipo_lancamento            = 3
                                and pg.cod_status_pagamento <> '10'  and    pg.ind_excluido = 'N') ValorTotalPagoGarantia,
                          (SELECT  nvl(sum( (nvl(round(pg1.val_lancamento,2),0) + NVL(round(pg1.valor_correcao,2),0) + nvl(round(pg1.valor_juros,2),0) ) * 
                                                    cg.valor_multiplicador),0) val_lancamentos
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
                                and pg1.cod_status_pagamento <> '10'  and    pg1.ind_excluido = 'N') SaldoDepositoBloqueio,
                         jur.processo.cod_profissional CodigoProfissional,
                         nvl(jur.processo.dat_finalizacao_contabil, '01/01/0001') DataFinalizacaoContabil";
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
                        (geral.ValorTotalPagoGarantia - geral.SaldoDepositoBloqueio) as SaldoGrantia     
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
