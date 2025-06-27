using Shared.Data.Interface.FactoryMethod;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Data.Impl.FactoryMethod
{
    class TributarioAdministrativoFM : IQueryFM
    {
        public string RetornaFrom()
        {
            return @" 
                    FROM jur.processo,   
                         jur.parte org,
                         jur.competencia comp,
                         jur.municipio mun,   
                         jur.parte
                    ";
        }

        public string RetornaGroupBy()
        {
            return "";
        }

        public string RetornaJoin()
        {
            return @"
                    WHERE 
                        ( org.cod_parte                       = jur.processo.cod_parte_orgao           )   and  
                        ( jur.parte.cod_parte                   = jur.processo.cod_parte_empresa         )   and  
                        ( jur.processo.cod_parte_orgao                  = comp.cod_parte (+)                   )  and
                        ( jur.processo.cod_competencia              = comp.cod_competencia (+)              )   and
                        ( jur.processo.cod_estado                       = mun.cod_estado (+)                      ) and
                        ( jur.processo.cod_municipio                    = mun.cod_municipio (+)                      ) and  
                        ( jur.processo.cod_tipo_processo           = 4                            )
                    ";
        }

        public string RetornaSelect()
        {
            return @"
                        SELECT distinct 
                            jur.processo.cod_processo IdProcesso,   
                            jur.processo.nro_processo_cartorio NumeroProcesso,   
                            jur.processo.cod_estado CodigoEstado,
                            org.nom_parte NomeOrgao,   
                            comp.nom_competencia NomeCompetencia,
                            mun.nom_municipio NomeMunicipio,   
                            jur.parte.nom_parte DescricaoEmpresaGrupo,  
                            decode(jur.processo.ind_processo_ativo, 'S', 'True', 'False') Ativo, 
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
                            (   
                                SELECT  
                                    nvl(sum( (nvl(round(pg1.val_lancamento,2),0) + NVL(round(pg1.valor_correcao,2),0) + nvl(round(pg1.valor_juros,2),0) ) * 
                                                    cg.valor_multiplicador),0) val_lancamentos
                                FROM  
                                    jur.lancamento_processo pg1,   
                                    jur.categoria_pagamento cp1,
                                    jur.tipos_saldos_garantias ts,
                                    jur.classes_garantias cg
                                WHERE pg1.cod_cat_pagamento             = cp1.cod_cat_pagamento                  and
                                    pg1.cod_processo                = jur.processo.cod_processo               and
                                    cp1.clgar_cod_classe_garantia          = cg.codigo                                     and
                                    cg.tsgar_cod_tipo_saldo_garantia       = ts.codigo                                     and
                                    ts.ind_baixa_pagamento              = 'S'                              and
                                    pg1.cod_tipo_lancamento            = 1                              and
                                    pg1.cod_status_pagamento <> '10'                                      and    pg1.ind_excluido = 'N'
                            ) SaldoDepositoBloqueio
                    ";
        }

        public string RetornaWhere()
        {
            throw new NotImplementedException();
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
                   jur.processo.cod_processo asc";
        }
    }
}
