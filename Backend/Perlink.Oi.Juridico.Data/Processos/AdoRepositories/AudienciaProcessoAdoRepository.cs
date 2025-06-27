using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using Perlink.Oi.Juridico.Data.Compartilhado.AdoBaseRepository;
using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;
using Perlink.Oi.Juridico.Domain.Processos.DTO;
using Perlink.Oi.Juridico.Domain.Processos.Interface.AdoRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Perlink.Oi.Juridico.Data.Processos.AdoRepositories
{
    public class AudienciaProcessoAdoRepository : AdoBaseRepository, IAudienciaProcessoAdoRepository
    {
        public IEnumerable<AudienciaProcessoResultadoDTO> ObterTodosPorFiltro(List<Filter> lstFilters, int pageNumber, int pageSize, List<SortOrder> orders, bool IsExportMethod = false)
        {
            var list = new List<AudienciaProcessoResultadoDTO>();
            var paramList = new List<OracleParameter>();
            OracleDataReader dataReader = null;

            try
            {
                var sql = new StringBuilder();

                if (!IsExportMethod) {
                    sql.Append(@"SELECT cod_processo_key, seq_audiencia_key, estado, comarca, tipo_vara, cod_vara, date_audiencia, horario_audiencia, 
                                   tipo_audiencia, preposto, escritorio_audiencia, advogado_escritorio, preposto_acompanhante, escritorio_acompanhante, 
                                   advogado_acompanhante, tipo_processo, estrategico, numero_processo, classificacao_hierarquica, empresa_grupo, endereco,
                                   escritorio_processo, ddd_advogado_escritorio, nro_advogado_escritorio, ddd_advogado_acompanhante, nro_advogado_acompanhante, 
                                   migrado  FROM
                             (
                                SELECT a.cod_processo_key, a.seq_audiencia_key, a.estado, a.comarca, a.tipo_vara, a.cod_vara, a.date_audiencia, a.horario_audiencia, 
                                       a.tipo_audiencia, a.preposto, a.escritorio_audiencia, a.advogado_escritorio, a.preposto_acompanhante, a.escritorio_acompanhante, 
                                       a.advogado_acompanhante, a.tipo_processo, a.estrategico, a.numero_processo, a.classificacao_hierarquica, a.empresa_grupo, a.endereco,
                                       a.escritorio_processo, a.ddd_advogado_escritorio, a.nro_advogado_escritorio, a.ddd_advogado_acompanhante, a.nro_advogado_acompanhante, 
                                       a.migrado, rownum r__ FROM
                                ( ");
                }

                sql.Append(@"SELECT jur.audiencia_processo.cod_processo as cod_processo_key,
                                           jur.audiencia_processo.seq_audiencia as seq_audiencia_key,
                                           jur.comarca.cod_estado as estado,
                                           jur.comarca.nom_comarca as comarca,
                                           jur.tipo_vara.nom_tipo_vara as tipo_vara,
                                           jur.vara.cod_vara as cod_vara,      
                                           jur.audiencia_processo.dat_audiencia as date_audiencia,
                                           jur.audiencia_processo.hor_audiencia as horario_audiencia,
                                           jur.tipo_audiencia.dsc_tipo_audiencia as tipo_audiencia,
                                           Decode(jur.audiencia_processo.cod_preposto,
                                                  null,
                                                  '',
                                                  nvl(jur.preposto.nom_preposto, '') ||
                                                  decode(nvl(jur.preposto.ind_preposto_ativo, 'S'),
                                                         'N',
                                                         ' [Inativo]',
                                                         decode(nvl(jur.preposto.ind_preposto_trabalhista, 'S'),
                                                                'S',
                                                                '',
                                                                ' [Inativo]'))) as preposto,        
                                           UPPER(Decode(jur.audiencia_processo.adves_cod_profissional,
                                                  null,
                                                  '',
                                                  nvl(jur.escritorio_audiencia.nom_profissional, '') ||
                                                  decode(nvl(jur.escritorio_audiencia.ind_ativo, 'S'),
                                                         'N',
                                                         ' [Inativo]',
                                                         decode(nvl(jur.escritorio_audiencia.ind_area_trabalhista,
                                                                    'S'),
                                                                'S',
                                                                '',
                                                                ' [Inativo]')))) as escritorio_audiencia,
                                           jur.advogado_escritorio.nom_advogado as advogado_escritorio,                                           
                                           Decode(jur.audiencia_processo.cod_preposto_acomp,
                                                  null,
                                                  '',
                                                  nvl(preposto_acompanhante.nom_preposto, '') ||
                                                  decode(nvl(preposto_acompanhante.ind_preposto_ativo, 'S'),
                                                         'N',
                                                         ' [Inativo]',
                                                         decode(nvl(preposto_acompanhante.ind_preposto_trabalhista,
                                                                    'S'),
                                                                'S',
                                                                '',
                                                                ' [Inativo]'))) as preposto_acompanhante,
                                           UPPER(Decode(jur.audiencia_processo.adves_cod_profissional_acomp,
                                                  null,
                                                  '',
                                                  nvl(escritorio_acompanhante.nom_profissional, '') ||
                                                  decode(nvl(escritorio_acompanhante.ind_ativo, 'S'),
                                                         'N',
                                                         ' [Inativo]',
                                                         decode(nvl(escritorio_acompanhante.ind_area_trabalhista,
                                                                    'S'),
                                                                'S',
                                                                '',
                                                                ' [Inativo]')))) as escritorio_acompanhante,
                                           advogado_acompanhante.nom_advogado as advogado_acompanhante,                                           
                                           jur.tipo_processo.dsc_tipo_processo as tipo_processo,
                                           Decode(jur.processo.cod_tipo_processo,
                                                  9,
                                                  'S',
                                                  jur.processo.ind_prioritaria) as estrategico,
                                           jur.processo.nro_processo_cartorio as numero_processo,       
                                           decode(jur.processo.cod_classificacao_processo,
                                                  'P',
                                                  'PRIMÁRIO',
                                                  decode(jur.processo.cod_classificacao_processo,
                                                         'S',
                                                         'SECUNDÁRIO',
                                                         'ÚNICO')) as classificacao_hierarquica,
                                           jur.parte.nom_parte as empresa_grupo,
                                           jur.vara.end_vara as endereco,
                                           jur.profissional.nom_profissional as escritorio_processo,
                                           jur.advogado_escritorio.nro_ddd_celular as ddd_advogado_escritorio,
                                           jur.advogado_escritorio.nro_celular as nro_advogado_escritorio,  
                                           jur.advogado_acompanhante.nro_ddd_celular as ddd_advogado_acompanhante,
                                           jur.advogado_acompanhante.nro_celular as nro_advogado_acompanhante,  
                                           decode(nvl( jur.migracoes_tipos_processos.proc_cod_processo_de,0), 0, 'N', 'S') as migrado                                          
                                      FROM jur.comarca,
                                           jur.processo,
                                           jur.tipo_vara,
                                           jur.vara,
                                           jur.acao,
                                           jur.audiencia_processo,
                                           jur.advogado_escritorio,
                                           jur.profissional,
                                           jur.profissional escritorio_audiencia,
                                           jur.tipo_audiencia,
                                           jur.preposto,
                                           jur.migracoes_tipos_processos,
                                           jur.preposto preposto_acompanhante,
                                           jur.profissional escritorio_acompanhante,
                                           jur.advogado_escritorio advogado_acompanhante,
                                           jur.parte,
                                           jur.tipo_processo
                                     WHERE (jur.processo.cod_comarca = jur.comarca.cod_comarca)
                                       and (jur.processo.cod_tipo_vara = jur.tipo_vara.cod_tipo_vara)
                                       and (jur.vara.cod_tipo_vara = jur.tipo_vara.cod_tipo_vara)
                                       and (jur.vara.cod_comarca = jur.processo.cod_comarca)
                                       and (jur.processo.cod_tipo_processo = jur.tipo_processo.cod_tipo_processo)
                                       and (jur.vara.cod_vara = jur.processo.cod_vara)
                                       and (jur.vara.cod_tipo_vara = jur.processo.cod_tipo_vara)
                                       and (jur.vara.cod_comarca = jur.comarca.cod_comarca)
                                       and (jur.processo.cod_acao = jur.acao.cod_acao)
                                       and (jur.processo.cod_processo = jur.audiencia_processo.cod_processo)
                                       and (jur.processo.cod_profissional = jur.profissional.cod_profissional)
                                       and (jur.processo.cod_parte_empresa = jur.parte.cod_parte)
                                       and (jur.tipo_audiencia.cod_tipo_aud(+) = jur.audiencia_processo.cod_tipo_aud)
                                       and (jur.preposto.cod_preposto(+) = jur.audiencia_processo.cod_preposto)
                                       and (jur.migracoes_tipos_processos.proc_cod_processo_para(+) = jur.processo.cod_processo)
                                       and (jur.advogado_escritorio.cod_advogado(+) = jur.audiencia_processo.adves_cod_advogado)
                                       and (jur.advogado_escritorio.cod_profissional(+) = jur.audiencia_processo.adves_cod_profissional)
                                       and (jur.audiencia_processo.adves_cod_profissional = jur.escritorio_audiencia.cod_profissional(+))
                                       and (jur.audiencia_processo.cod_preposto_acomp = preposto_acompanhante.cod_preposto(+))
                                       and (jur.audiencia_processo.adves_cod_profissional_acomp = escritorio_acompanhante.cod_profissional(+))
                                       and (jur.audiencia_processo.adves_cod_profissional_acomp = advogado_acompanhante.cod_profissional(+))
                                       and (jur.audiencia_processo.adves_cod_advogado_acomp = advogado_acompanhante.cod_advogado(+))
                                       and (jur.processo.cod_tipo_processo = 2) ");

                #region "Dynamic Filters"

                var count = 0;
                var classificacaoHierarquicaDictionary = new Dictionary<int, char>();
                classificacaoHierarquicaDictionary.Add(1, 'U');
                classificacaoHierarquicaDictionary.Add(2, 'P');
                classificacaoHierarquicaDictionary.Add(3, 'S');

                foreach (Filter objFilter in lstFilters.Where(x => !string.IsNullOrEmpty(x.FieldName) && !string.IsNullOrEmpty(x.Value)))
                {
                    switch (objFilter.FieldName.ToLower())
                    {
                        case "estrategico":

                            #region estrategico

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.EqualsTo:
                                    if (Convert.ToInt32(objFilter.Value) < 3 && Convert.ToInt32(objFilter.Value) > 0 )
                                    {
                                        var filtro = objFilter.Value == "1" ? 'S' : 'N';
                                        sql.Append($"and (jur.processo.ind_prioritaria = '{filtro}') ");
                                    }
                                    break;
                                
                                default:
                                    break;
                            }

                            break;

                        #endregion

                        case "periodopendenciacalculo":

                            #region periodoPendenciaCalculo

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.Between:
                                    List<string> lstDate = objFilter.Value.Split(';').ToList();

                                    sql.Append($"and (Exists (select 1 from jur.parte_pedido_processo where jur.parte_pedido_processo.cod_processo = jur.processo.cod_processo and (jur.parte_pedido_processo.data_revisao_valor between TO_DATE('{lstDate[0].Trim()} 00:00:00', 'YYYY-MM-DD HH24:MI:SS') and TO_DATE('{lstDate[1].Trim()} 23:59:59', 'YYYY-MM-DD HH24:MI:SS')))) ");

                                    break;
                                
                                default:
                                    break;
                            }

                            break;

                            #endregion

                        case "dataaudiencia":

                            #region dataAudiencia

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.Between:
                                    List<string> lstDate = objFilter.Value.Split(';').ToList();

                                    sql.Append($"and (jur.audiencia_processo.dat_audiencia between TO_DATE('{lstDate[0].Trim()} 00:00:00', 'YYYY-MM-DD HH24:MI:SS') and TO_DATE('{lstDate[1].Trim()} 23:59:59', 'YYYY-MM-DD HH24:MI:SS')) ");

                                    break;

                                default:
                                    break;
                            }

                            break;

                            #endregion

                        case "classificacaohierarquica":

                            #region classificacaoHierarquica

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.EqualsTo:
                                    if (Convert.ToInt32(objFilter.Value) < 4 && Convert.ToInt32(objFilter.Value) > 0)
                                    {
                                        sql.Append($"and (jur.processo.cod_classificacao_processo = '{classificacaoHierarquicaDictionary.GetValueOrDefault(Convert.ToInt32(objFilter.Value))}') ");
                                    }                                   
                                    break;
                                
                                default:
                                    break;
                            }

                            break;

                        #endregion

                        case "advogadoaudiencia":

                            #region advogadoAudiencia

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.In:
                                    List<string> lstValuesIn = objFilter.Value.Split(';').ToList();
                                    List<string> lstValues2In = objFilter.Value2.Split(';').ToList();
                                    sql.Append($"and (((nvl(jur.advogado_escritorio.cod_profissional, 0), nvl(jur.advogado_escritorio.cod_advogado, 0))  in (");

                                    for (var i = 0; i < lstValuesIn.Count(); i++)
                                    {
                                        if (i == 0)
                                        {
                                            sql.Append($"(:advacompcp{i} , :advacompae{i})");
                                        }
                                        else if (i % 1000 == 0 && i != 0)
                                        {
                                            sql.Append($")) or (nvl(jur.advogado_escritorio.cod_profissional, 0), nvl(jur.advogado_escritorio.cod_advogado, 0)) in (");
                                            sql.Append($"(:advacompcp{i} , :advacompae{i})");
                                        }
                                        else
                                        {
                                            sql.Append($", (:advacompcp{i} , :advacompae{i})");
                                        }

                                        paramList.Add(new OracleParameter($"advaudcp{i}", lstValues2In[i]));
                                        paramList.Add(new OracleParameter($"advaudae{i}", lstValuesIn[i]));
                                    }

                                    sql.Append($"))) ");                                
                                        
                                    break;
                                
                                default:
                                    break;
                            }

                            break;

                        #endregion

                        case "advogadoacompanhante":

                            #region advogadoAcompanhante

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.In:
                                    List<string> lstValuesIn = objFilter.Value.Split(';').ToList();
                                    List<string> lstValues2In = objFilter.Value2.Split(';').ToList();
                                    sql.Append($"and (((nvl(advogado_acompanhante.cod_profissional, 0), nvl(advogado_acompanhante.cod_advogado, 0))  in (");

                                    for (var i = 0; i < lstValuesIn.Count(); i++)
                                    {
                                        if (i == 0)
                                        {
                                            sql.Append($"(:advacompcp{i} , :advacompac{i})");
                                        }
                                        else if (i % 1000 == 0 && i != 0)
                                        {
                                            sql.Append($")) or (nvl(advogado_acompanhante.cod_profissional, 0), nvl(advogado_acompanhante.cod_advogado, 0)) in (");
                                            sql.Append($"(:advacompcp{i} , :advacompac{i})");
                                        }
                                        else
                                        {
                                            sql.Append($", (:advacompcp{i} , :advacompac{i})");
                                        }

                                        paramList.Add(new OracleParameter($"advacompcp{i}", lstValues2In[i]));
                                        paramList.Add(new OracleParameter($"advacompac{i}", lstValuesIn[i]));
                                    }

                                    sql.Append($"))) ");
                                   
                                    break;
                                default:
                                    break;
                            }

                            break;

                        #endregion

                        case "codcomarca":

                            #region codComarca

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.In:
                                    List<string> lstValuesIn = objFilter.Value.Split(';').ToList();
                                    sql.Append($"and ((nvl(jur.processo.cod_comarca, 0) in (");

                                    for (var i = 0; i < lstValuesIn.Count(); i++)
                                    {
                                        if (i == 0)
                                        {
                                            sql.Append($":codcom{i}");
                                        }
                                        else if (i % 1000 == 0 && i != 0)
                                        {
                                            sql.Append($")) or (nvl(jur.processo.cod_comarca, 0) in (");
                                            sql.Append($":codcom{i}");
                                        }
                                        else
                                        {
                                            sql.Append($", :codcom{i}");
                                        }

                                        paramList.Add(new OracleParameter($"codcom{i}", lstValuesIn[i]));
                                    }

                                    sql.Append($"))) ");
                                    break;
                                default:
                                    break;
                            }

                            break;

                        #endregion

                        case "empresagrupo":

                            #region empresaGrupo

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.In:
                                    List<string> lstValuesIn = objFilter.Value.Split(';').ToList();
                                    sql.Append($"and ((nvl(jur.processo.cod_parte_empresa, 0) in (");

                                    for (var i = 0; i < lstValuesIn.Count(); i++)
                                    {
                                        if (i == 0)
                                        {
                                            sql.Append($":empgr{i}");
                                        }
                                        else if (i % 1000 == 0 && i != 0)
                                        {
                                            sql.Append($")) or (nvl(jur.processo.cod_parte_empresa, 0) in (");
                                            sql.Append($":empgr{i}");
                                        }
                                        else
                                        {
                                            sql.Append($", :empgr{i}");
                                        }

                                        paramList.Add(new OracleParameter($"empgr{i}", lstValuesIn[i]));
                                    }

                                    sql.Append($"))) ");
                                    break;
                                default:
                                    break;
                            }

                            break;

                        #endregion

                        case "escritorioaudiencia":

                            #region escritorioAudiencia

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.In_Escritorio_Audiencia:
                                    List<string> lstValuesIn = objFilter.Value.Split(';').ToList();
                                    sql.Append($"and ((nvl(jur.audiencia_processo.adves_cod_profissional, 0) in (");

                                    for (var i = 0; i < lstValuesIn.Count(); i++)
                                    {
                                        if (i == 0)
                                        {
                                            sql.Append($":escaud{i}");
                                        }
                                        else if (i % 1000 == 0 && i != 0)
                                        {
                                            sql.Append($")) or (nvl(jur.audiencia_processo.adves_cod_profissional, 0) in (");
                                            sql.Append($":escaud{i}");
                                        }
                                        else
                                        {
                                            sql.Append($", :escaud{i}");
                                        }

                                        paramList.Add(new OracleParameter($"escaud{i}", lstValuesIn[i]));
                                    }

                                    sql.Append($"))) ");
                                    break;
                                case FilterOperatorEnum.In_Escritorio_Processo:
                                    List<string> lstValuesIn2 = objFilter.Value.Split(';').ToList();
                                    sql.Append($"and ((nvl(jur.processo.cod_profissional, 0) in (");

                                    for (var i = 0; i < lstValuesIn2.Count(); i++)
                                    {
                                        if (i == 0)
                                        {
                                            sql.Append($":escprc{i}");
                                        }
                                        else if (i % 1000 == 0 && i != 0)
                                        {
                                            sql.Append($")) or (nvl(jur.processo.cod_profissional, 0) in (");
                                            sql.Append($":escprc{i}");
                                        }
                                        else
                                        {
                                            sql.Append($", :escprc{i}");
                                        }

                                        paramList.Add(new OracleParameter($"escprc{i}", lstValuesIn2[i]));
                                    }

                                    sql.Append($"))) ");
                                    break;
                                default:
                                    break;
                            }

                            break;

                        #endregion

                        case "escritorioacompanhante":

                            #region escritorioAcompanhante

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.In:
                                    List<string> lstValuesIn = objFilter.Value.Split(';').ToList();
                                    sql.Append($"and ((nvl(jur.audiencia_processo.adves_cod_profissional_acomp, 0) in (");

                                    for (var i = 0; i < lstValuesIn.Count(); i++)
                                    {
                                        if (i == 0)
                                        {
                                            sql.Append($":escacomp{i}");
                                        }
                                        else if (i % 1000 == 0 && i != 0)
                                        {
                                            sql.Append($")) or (nvl(jur.audiencia_processo.adves_cod_profissional_acomp, 0) in (");
                                            sql.Append($":escacomp{i}");
                                        }
                                        else
                                        {
                                            sql.Append($", :escacomp{i}");
                                        }

                                        paramList.Add(new OracleParameter($"escacomp{i}", lstValuesIn[i]));
                                    }

                                    sql.Append($"))) ");
                                    break;
                                default:
                                    break;
                            }

                            break;

                        #endregion

                        case "siglaestado":

                            #region siglaEstado

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.In:
                                    List<string> lstValuesIn = objFilter.Value.Split(';').ToList();
                                    sql.Append($"and ((UPPER(jur.comarca.cod_estado) in (");

                                    for (var i = 0; i < lstValuesIn.Count(); i++)
                                    {
                                        if (i == 0)
                                        {
                                            sql.Append($":sigest{i}");
                                        }
                                        else if (i % 1000 == 0 && i != 0)
                                        {
                                            sql.Append($")) or (UPPER(jur.comarca.cod_estado) in (");
                                            sql.Append($":sigest{i}");
                                        }
                                        else
                                        {
                                            sql.Append($", :sigest{i}");
                                        }

                                        paramList.Add(new OracleParameter($"sigest{i}", lstValuesIn[i]));
                                    }

                                    sql.Append($"))) ");
                                    break;
                                default:
                                    break;
                            }

                            break;

                        #endregion

                        case "preposto":

                            #region preposto

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.In:
                                    List<string> lstValuesIn = objFilter.Value.Split(';').ToList();
                                    sql.Append($"and ((nvl(jur.audiencia_processo.cod_preposto, 0) in (");

                                    for (var i = 0; i < lstValuesIn.Count(); i++)
                                    {
                                        if (i == 0)
                                        {
                                            sql.Append($":prep{i}");
                                        }
                                        else if (i % 1000 == 0 && i != 0)
                                        {
                                            sql.Append($")) or (nvl(jur.audiencia_processo.cod_preposto, 0) in (");
                                            sql.Append($":prep{i}");
                                        }
                                        else
                                        {
                                            sql.Append($", :prep{i}");
                                        }

                                        paramList.Add(new OracleParameter($"prep{i}", lstValuesIn[i]));
                                    }

                                    sql.Append($"))) ");
                                    break;
                                default:
                                    break;
                            }

                            break;

                        #endregion

                        case "prepostoacompanhante":

                            #region prepostoAcompanhante

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.In:
                                    List<string> lstValuesIn = objFilter.Value.Split(';').ToList();
                                    sql.Append($"and ((nvl(jur.audiencia_processo.cod_preposto_acomp, 0) in (");

                                    for (var i = 0; i < lstValuesIn.Count(); i++)
                                    {
                                        if (i == 0)
                                        {
                                            sql.Append($":prepacomp{i}");
                                        }
                                        else if (i % 1000 == 0 && i != 0)
                                        {
                                            sql.Append($")) or (nvl(jur.audiencia_processo.cod_preposto_acomp, 0) in (");
                                            sql.Append($":prepacomp{i}");
                                        }
                                        else
                                        {
                                            sql.Append($", :prepacomp{i}");
                                        }

                                        paramList.Add(new OracleParameter($"prepacomp{i}", lstValuesIn[i]));
                                    }

                                    sql.Append($"))) ");
                                    break;
                                default:
                                    break;
                            }

                            break;

                        #endregion

                        case "codprocesso":

                            #region codProcesso

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.In:
                                    List<string> lstValuesIn = objFilter.Value.Split(';').ToList();
                                    sql.Append($"and ((nvl(jur.processo.cod_processo, 0) in (");

                                    for (var i = 0; i < lstValuesIn.Count(); i++)
                                    {
                                        if (i == 0)
                                        {
                                            sql.Append($":proc{i}");
                                        }
                                        else if (i % 1000 == 0 && i != 0)
                                        {
                                            sql.Append($")) or (nvl(jur.processo.cod_processo, 0) in (");
                                            sql.Append($":proc{i}");
                                        }
                                        else
                                        {
                                            sql.Append($", :proc{i}");
                                        }

                                        paramList.Add(new OracleParameter($"proc{i}", lstValuesIn[i]));
                                    }

                                    sql.Append($"))) ");
                                    break;
                                default:
                                    break;
                            }

                            break;

                        #endregion

                        case "vara":

                            #region vara

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.EqualsTo:
                                    List<string> lstValuesIn = objFilter.Value.Split(';').ToList(); // ["1,2,3", "1,2,3", "1,2,3"]
                                    sql.Append($"and (");

                                    for (var i = 0; i < lstValuesIn.Count(); i++)
                                    {
                                        var lstTemp = lstValuesIn[i].Split(',').ToList(); // ["1","2","3"] - Sempre na ordem (comarca, vara, tipovara)

                                        if (i == 0)
                                        {
                                            for (var j = 0; j < lstTemp.Count(); j++)
                                            {
                                                if (j == 0)
                                                {
                                                    sql.Append($"(jur.processo.cod_comarca = :vara{i}{j} ");
                                                }
                                                else if (j == 1)
                                                {
                                                    sql.Append($"and jur.processo.cod_vara = :vara{i}{j} ");
                                                }
                                                else if (j == 2)
                                                {
                                                    sql.Append($"and jur.processo.cod_tipo_vara = :vara{i}{j}) ");
                                                }

                                                paramList.Add(new OracleParameter($"vara{i}{j}", lstTemp[j]));
                                            }
                                        }
                                        else
                                        {
                                            for (var j = 0; j < lstTemp.Count(); j++)
                                            {
                                                if (j == 0)
                                                {
                                                    sql.Append($"or (jur.processo.cod_comarca = :vara{i}{j} ");
                                                }
                                                else if (j == 1)
                                                {
                                                    sql.Append($"and jur.processo.cod_vara = :vara{i}{j} ");
                                                }
                                                else if (j == 2)
                                                {
                                                    sql.Append($"and jur.processo.cod_tipo_vara = :vara{i}{j}) ");
                                                }

                                                paramList.Add(new OracleParameter($"vara{i}{j}", lstTemp[j]));
                                            }
                                        }

                                    }

                                    sql.Append($") ");
                                    break;
                                default:
                                    break;
                            }

                            break;

                        #endregion

                        case "classificacaoclosing":

                            #region classificacaoClosing

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.EqualsTo:
                                    if (Convert.ToInt32(objFilter.Value) < 4 && Convert.ToInt32(objFilter.Value) > 0)
                                    {
                                        sql.Append($"and (jur.processo.cod_classificacao_closing = '{Convert.ToInt32(objFilter.Value)}') ");
                                    }
                                    break;

                                default:
                                    break;
                            }

                            break;

                        #endregion

                        default: break;
                    }

                    count++;
                }

                #endregion

                sql.Append("ORDER BY" + GetOrderBy(orders));

                if (!IsExportMethod)
                {
                    sql.Append(@") a
                                 WHERE rownum < ((:pageNumber * :pageSize) + 1 )
                                 ) WHERE r__ >= (((:pageNumber-1) * :pageSize) + 1)");

                    paramList.Add(new OracleParameter("pageNumber", pageNumber));
                    paramList.Add(new OracleParameter("pageSize", pageSize));
                }

                AbrirConexao();
                dataReader = ExecuteReader(sql.ToString(), paramList.ToArray());                        

                while (dataReader.Read())
                {
                    var obj = new AudienciaProcessoResultadoDTO
                    {
                        CodProcesso = dataReader.GetInt64(0),
                        SeqAudiencia = dataReader.GetInt64(1),
                        SiglaEstado = dataReader.IsDBNull(2) ? string.Empty : dataReader.GetString(2).Trim(),
                        Comarca = dataReader.IsDBNull(3) ? string.Empty : dataReader.GetString(3).Trim(),
                        TipoVara = dataReader.IsDBNull(4) ? string.Empty : dataReader.GetString(4).Trim(),
                        CodVara = dataReader.GetInt32(5),
                        DataAudiencia = dataReader.IsDBNull(6) ? string.Empty : dataReader.GetDateTime(6).ToShortDateString(),
                        HorarioAudiencia = dataReader.IsDBNull(7) ? string.Empty : dataReader.GetDateTime(7).ToShortTimeString(),
                        TipoAudiencia = dataReader.IsDBNull(8) ? string.Empty : dataReader.GetString(8).Trim(),
                        Preposto = dataReader.IsDBNull(9) ? string.Empty : dataReader.GetString(9).Trim(),
                        EscritorioAudiencia = dataReader.IsDBNull(10) ? string.Empty : dataReader.GetString(10).Trim(),
                        AdvogadoAudiencia = dataReader.IsDBNull(11) ? string.Empty : dataReader.GetString(11).Trim(),
                        PrepostoAcompanhante = dataReader.IsDBNull(12) ? string.Empty : dataReader.GetString(12).Trim(),
                        EscritorioAcompanhante = dataReader.IsDBNull(13) ? string.Empty : dataReader.GetString(13).Trim(),
                        AdvogadoAcompanhante = dataReader.IsDBNull(14) ? string.Empty : dataReader.GetString(14).Trim(),
                        TipoProcesso = dataReader.IsDBNull(15) ? string.Empty : dataReader.GetString(15).Trim(),
                        Estrategico = dataReader.IsDBNull(16) ? string.Empty : dataReader.GetString(16).Trim(),
                        NumeroProcesso = dataReader.IsDBNull(17) ? string.Empty : dataReader.GetString(17).Trim(),
                        ClassificacaoHierarquica = dataReader.IsDBNull(18) ? string.Empty : dataReader.GetString(18).Trim(),
                        EmpresaGrupo = dataReader.IsDBNull(19) ? string.Empty : dataReader.GetString(19).Trim(),
                        Endereco = dataReader.IsDBNull(20) ? string.Empty : dataReader.GetString(20).Trim(),
                        EscritorioProcesso = dataReader.IsDBNull(21) ? string.Empty : dataReader.GetString(21).Trim(),
                        DddAdvogadoEscritorio = dataReader.IsDBNull(22) ? string.Empty : dataReader.GetString(22).Trim(),
                        NroAdvogadoEscritorio = dataReader.IsDBNull(23) ? string.Empty : dataReader.GetString(23).Trim(),
                        DddAdvogadoAcompanhante = dataReader.IsDBNull(24) ? string.Empty : dataReader.GetString(24).Trim(),
                        NroAdvogadoAcompanhante = dataReader.IsDBNull(25) ? string.Empty : dataReader.GetString(25).Trim(),
                        Migrado = dataReader.IsDBNull(26) ? string.Empty : dataReader.GetString(26).Trim()
                    };
 
                    list.Add(obj);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                }

                FecharConexao();
            }

            return list;
        }

        public IEnumerable<AudienciaProcessoCompletoResultadoDTO> ObterTodosCompletoPorFiltro(List<Filter> lstFilters, int pageNumber, int pageSize, List<SortOrder> orders, bool IsExportMethod = false)
        {
            var list = new List<AudienciaProcessoCompletoResultadoDTO>();
            var paramList = new List<OracleParameter>();
            OracleDataReader dataReader = null;

            try
            {
                var sql = new StringBuilder();

                if (!IsExportMethod)
                {
                    sql.Append(@"SELECT cod_processo_key, seq_audiencia_key, estado, comarca, tipo_vara, cod_vara, date_audiencia, horario_audiencia, 
                                   tipo_audiencia, preposto, escritorio_audiencia, advogado_escritorio, preposto_acompanhante, escritorio_acompanhante, 
                                   advogado_acompanhante, tipo_processo, estrategico, numero_processo, classificacao_hierarquica, empresa_grupo, endereco,
                                   escritorio_processo, ddd_advogado_escritorio, nro_advogado_escritorio, ddd_advogado_acompanhante, nro_advogado_acompanhante, 
                                   migrado, reclamadas, reclamante, pedido FROM
                             (
                                SELECT a.cod_processo_key, a.seq_audiencia_key, a.estado, a.comarca, a.tipo_vara, a.cod_vara, a.date_audiencia, a.horario_audiencia, 
                                       a.tipo_audiencia, a.preposto, a.escritorio_audiencia, a.advogado_escritorio, a.preposto_acompanhante, a.escritorio_acompanhante, 
                                       a.advogado_acompanhante, a.tipo_processo, a.estrategico, a.numero_processo, a.classificacao_hierarquica, a.empresa_grupo, a.endereco,
                                       a.escritorio_processo, a.ddd_advogado_escritorio, a.nro_advogado_escritorio, a.ddd_advogado_acompanhante, a.nro_advogado_acompanhante, 
                                       a.migrado, a.reclamadas, a.reclamante, a.pedido, rownum r__ FROM
                                ( ");
                }

                sql.Append(@"SELECT jur.audiencia_processo.cod_processo as cod_processo_key,
                                           jur.audiencia_processo.seq_audiencia as seq_audiencia_key,
                                           jur.comarca.cod_estado as estado,
                                           jur.comarca.nom_comarca as comarca,
                                           jur.tipo_vara.nom_tipo_vara as tipo_vara,
                                           jur.vara.cod_vara as cod_vara,      
                                           jur.audiencia_processo.dat_audiencia as date_audiencia,
                                           jur.audiencia_processo.hor_audiencia as horario_audiencia,
                                           jur.tipo_audiencia.dsc_tipo_audiencia as tipo_audiencia,
                                           Decode(jur.audiencia_processo.cod_preposto,
                                                  null,
                                                  '',
                                                  nvl(jur.preposto.nom_preposto, '') ||
                                                  decode(nvl(jur.preposto.ind_preposto_ativo, 'S'),
                                                         'N',
                                                         ' [Inativo]',
                                                         decode(nvl(jur.preposto.ind_preposto_trabalhista, 'S'),
                                                                'S',
                                                                '',
                                                                ' [Inativo]'))) as preposto,        
                                           UPPER(Decode(jur.audiencia_processo.adves_cod_profissional,
                                                  null,
                                                  '',
                                                  nvl(jur.escritorio_audiencia.nom_profissional, '') ||
                                                  decode(nvl(jur.escritorio_audiencia.ind_ativo, 'S'),
                                                         'N',
                                                         ' [Inativo]',
                                                         decode(nvl(jur.escritorio_audiencia.ind_area_trabalhista,
                                                                    'S'),
                                                                'S',
                                                                '',
                                                                ' [Inativo]')))) as escritorio_audiencia,
                                           jur.advogado_escritorio.nom_advogado as advogado_escritorio,                                           
                                           Decode(jur.audiencia_processo.cod_preposto_acomp,
                                                  null,
                                                  '',
                                                  nvl(preposto_acompanhante.nom_preposto, '') ||
                                                  decode(nvl(preposto_acompanhante.ind_preposto_ativo, 'S'),
                                                         'N',
                                                         ' [Inativo]',
                                                         decode(nvl(preposto_acompanhante.ind_preposto_trabalhista,
                                                                    'S'),
                                                                'S',
                                                                '',
                                                                ' [Inativo]'))) as preposto_acompanhante,
                                           UPPER(Decode(jur.audiencia_processo.adves_cod_profissional_acomp,
                                                  null,
                                                  '',
                                                  nvl(escritorio_acompanhante.nom_profissional, '') ||
                                                  decode(nvl(escritorio_acompanhante.ind_ativo, 'S'),
                                                         'N',
                                                         ' [Inativo]',
                                                         decode(nvl(escritorio_acompanhante.ind_area_trabalhista,
                                                                    'S'),
                                                                'S',
                                                                '',
                                                                ' [Inativo]')))) as escritorio_acompanhante,
                                           advogado_acompanhante.nom_advogado as advogado_acompanhante,                                           
                                           jur.tipo_processo.dsc_tipo_processo as tipo_processo,
                                           Decode(jur.processo.cod_tipo_processo,
                                                  9,
                                                  'S',
                                                  jur.processo.ind_prioritaria) as estrategico,
                                           jur.processo.nro_processo_cartorio as numero_processo,       
                                           decode(jur.processo.cod_classificacao_processo,
                                                  'P',
                                                  'PRIMÁRIO',
                                                  decode(jur.processo.cod_classificacao_processo,
                                                         'S',
                                                         'SECUNDÁRIO',
                                                         'ÚNICO')) as classificacao_hierarquica,
                                           jur.parte.nom_parte as empresa_grupo,
                                           jur.vara.end_vara as endereco,
                                           jur.profissional.nom_profissional as escritorio_processo,
                                           jur.advogado_escritorio.nro_ddd_celular as ddd_advogado_escritorio,
                                           jur.advogado_escritorio.nro_celular as nro_advogado_escritorio,  
                                           jur.advogado_acompanhante.nro_ddd_celular as ddd_advogado_acompanhante,
                                           jur.advogado_acompanhante.nro_celular as nro_advogado_acompanhante,  
                                           decode(nvl( jur.migracoes_tipos_processos.proc_cod_processo_de,0), 0, 'N', 'S') as migrado,
                                           (SELECT listagg(p.nom_parte, ' , ') within group (order by p.nom_parte)
                                           FROM jur.parte_processo pp,
                                                jur.parte p
                                           WHERE pp.COD_PROCESSO = jur.PROCESSO.COD_PROCESSO
                                           AND P.COD_PARTE = pp.COD_PARTE
                                           AND PP.COD_TIPO_PARTICIPACAO = 4)  AS reclamadas,
                                           pt.nom_parte AS reclamante,    
                                           jur.pedido.dsc_pedido AS pedido 
                                      FROM jur.comarca,
                                           jur.processo,
                                           jur.tipo_vara,
                                           jur.vara,
                                           jur.acao,
                                           jur.audiencia_processo,
                                           jur.advogado_escritorio,
                                           jur.profissional,
                                           jur.profissional escritorio_audiencia,
                                           jur.tipo_audiencia,
                                           jur.preposto,
                                           jur.migracoes_tipos_processos,
                                           jur.preposto preposto_acompanhante,
                                           jur.profissional escritorio_acompanhante,
                                           jur.advogado_escritorio advogado_acompanhante,
                                           jur.parte,
                                           jur.tipo_processo,
                                           jur.pedido_processo,
                                           jur.pedido,
                                           jur.parte_pedido_processo,
                                           jur.parte pt
                                     WHERE (jur.processo.cod_comarca = jur.comarca.cod_comarca)
                                       and (jur.processo.cod_tipo_vara = jur.tipo_vara.cod_tipo_vara)
                                       and (jur.vara.cod_tipo_vara = jur.tipo_vara.cod_tipo_vara)
                                       and (jur.vara.cod_comarca = jur.processo.cod_comarca)
                                       and (jur.processo.cod_tipo_processo = jur.tipo_processo.cod_tipo_processo)
                                       and (jur.vara.cod_vara = jur.processo.cod_vara)
                                       and (jur.vara.cod_tipo_vara = jur.processo.cod_tipo_vara)
                                       and (jur.vara.cod_comarca = jur.comarca.cod_comarca)
                                       and (jur.processo.cod_acao = jur.acao.cod_acao)
                                       and (jur.processo.cod_processo = jur.audiencia_processo.cod_processo)
                                       and (jur.processo.cod_profissional = jur.profissional.cod_profissional)
                                       and (jur.processo.cod_parte_empresa = jur.parte.cod_parte)
                                       and (jur.tipo_audiencia.cod_tipo_aud(+) = jur.audiencia_processo.cod_tipo_aud)
                                       and (jur.preposto.cod_preposto(+) = jur.audiencia_processo.cod_preposto)
                                       and (jur.migracoes_tipos_processos.proc_cod_processo_para(+) = jur.processo.cod_processo)
                                       and (jur.advogado_escritorio.cod_advogado(+) = jur.audiencia_processo.adves_cod_advogado)
                                       and (jur.advogado_escritorio.cod_profissional(+) = jur.audiencia_processo.adves_cod_profissional)
                                       and (jur.audiencia_processo.adves_cod_profissional = jur.escritorio_audiencia.cod_profissional(+))
                                       and (jur.audiencia_processo.cod_preposto_acomp = preposto_acompanhante.cod_preposto(+))
                                       and (jur.audiencia_processo.adves_cod_profissional_acomp = escritorio_acompanhante.cod_profissional(+))
                                       and (jur.audiencia_processo.adves_cod_profissional_acomp = advogado_acompanhante.cod_profissional(+))
                                       and (jur.audiencia_processo.adves_cod_advogado_acomp = advogado_acompanhante.cod_advogado(+))
                                       and (jur.processo.cod_tipo_processo = 2) 
                                       and (jur.processo.cod_processo = jur.pedido_processo.COD_PROCESSO)
                                       AND (jur.PEDIDO_PROCESSO.COD_PEDIDO = jur.PEDIDO.COD_PEDIDO)
                                       AND (jur.PARTE_PEDIDO_PROCESSO.cod_processo = jur.processo.cod_processo)
                                       AND (jur.PARTE_PEDIDO_PROCESSO.COD_PEDIDO = jur.PEDIDO.COD_PEDIDO)
                                       AND (pt.COD_PARTE = jur.PARTE_PEDIDO_PROCESSO.COD_PARTE)");

                #region "Dynamic Filters"

                var count = 0;
                var classificacaoHierarquicaDictionary = new Dictionary<int, char>();
                classificacaoHierarquicaDictionary.Add(1, 'U');
                classificacaoHierarquicaDictionary.Add(2, 'P');
                classificacaoHierarquicaDictionary.Add(3, 'S');

                foreach (Filter objFilter in lstFilters.Where(x => !string.IsNullOrEmpty(x.FieldName) && !string.IsNullOrEmpty(x.Value)))
                {
                    switch (objFilter.FieldName.ToLower())
                    {
                        case "estrategico":

                            #region estrategico

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.EqualsTo:
                                    if (Convert.ToInt32(objFilter.Value) < 3 && Convert.ToInt32(objFilter.Value) > 0)
                                    {
                                        var filtro = objFilter.Value == "1" ? 'S' : 'N';
                                        sql.Append($"and (jur.processo.ind_prioritaria = '{filtro}') ");
                                    }
                                    break;

                                default:
                                    break;
                            }

                            break;

                        #endregion

                        case "periodopendenciacalculo":

                            #region periodoPendenciaCalculo

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.Between:
                                    List<string> lstDate = objFilter.Value.Split(';').ToList();

                                    sql.Append($"and (Exists (select 1 from jur.parte_pedido_processo where jur.parte_pedido_processo.cod_processo = jur.processo.cod_processo and (jur.parte_pedido_processo.data_revisao_valor between TO_DATE('{lstDate[0].Trim()} 00:00:00', 'YYYY-MM-DD HH24:MI:SS') and TO_DATE('{lstDate[1].Trim()} 23:59:59', 'YYYY-MM-DD HH24:MI:SS')))) ");

                                    break;

                                default:
                                    break;
                            }

                            break;

                        #endregion

                        case "dataaudiencia":

                            #region dataAudiencia

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.Between:
                                    List<string> lstDate = objFilter.Value.Split(';').ToList();

                                    sql.Append($"and (jur.audiencia_processo.dat_audiencia between TO_DATE('{lstDate[0].Trim()} 00:00:00', 'YYYY-MM-DD HH24:MI:SS') and TO_DATE('{lstDate[1].Trim()} 23:59:59', 'YYYY-MM-DD HH24:MI:SS')) ");

                                    break;

                                default:
                                    break;
                            }

                            break;

                        #endregion

                        case "classificacaohierarquica":

                            #region classificacaoHierarquica

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.EqualsTo:
                                    if (Convert.ToInt32(objFilter.Value) < 4 && Convert.ToInt32(objFilter.Value) > 0)
                                    {
                                        sql.Append($"and (jur.processo.cod_classificacao_processo = '{classificacaoHierarquicaDictionary.GetValueOrDefault(Convert.ToInt32(objFilter.Value))}') ");
                                    }
                                    break;

                                default:
                                    break;
                            }

                            break;

                        #endregion

                        case "advogadoaudiencia":

                            #region advogadoAudiencia

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.In:
                                    List<string> lstValuesIn = objFilter.Value.Split(';').ToList();
                                    List<string> lstValues2In = objFilter.Value2.Split(';').ToList();
                                    sql.Append($"and (((nvl(jur.advogado_escritorio.cod_profissional, 0), nvl(jur.advogado_escritorio.cod_advogado, 0))  in (");

                                    for (var i = 0; i < lstValuesIn.Count(); i++)
                                    {
                                        if (i == 0)
                                        {
                                            sql.Append($"(:advacompcp{i} , :advacompae{i})");
                                        }
                                        else if (i % 1000 == 0 && i != 0)
                                        {
                                            sql.Append($")) or (nvl(jur.advogado_escritorio.cod_profissional, 0), nvl(jur.advogado_escritorio.cod_advogado, 0)) in (");
                                            sql.Append($"(:advacompcp{i} , :advacompae{i})");
                                        }
                                        else
                                        {
                                            sql.Append($", (:advacompcp{i} , :advacompae{i})");
                                        }

                                        paramList.Add(new OracleParameter($"advaudcp{i}", lstValues2In[i]));
                                        paramList.Add(new OracleParameter($"advaudae{i}", lstValuesIn[i]));
                                    }

                                    sql.Append($"))) ");

                                    break;

                                default:
                                    break;
                            }

                            break;

                        #endregion

                        case "advogadoacompanhante":

                            #region advogadoAcompanhante

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.In:
                                    List<string> lstValuesIn = objFilter.Value.Split(';').ToList();
                                    List<string> lstValues2In = objFilter.Value2.Split(';').ToList();
                                    sql.Append($"and (((nvl(advogado_acompanhante.cod_profissional, 0), nvl(advogado_acompanhante.cod_advogado, 0))  in (");

                                    for (var i = 0; i < lstValuesIn.Count(); i++)
                                    {
                                        if (i == 0)
                                        {
                                            sql.Append($"(:advacompcp{i} , :advacompac{i})");
                                        }
                                        else if (i % 1000 == 0 && i != 0)
                                        {
                                            sql.Append($")) or (nvl(advogado_acompanhante.cod_profissional, 0), nvl(advogado_acompanhante.cod_advogado, 0)) in (");
                                            sql.Append($"(:advacompcp{i} , :advacompac{i})");
                                        }
                                        else
                                        {
                                            sql.Append($", (:advacompcp{i} , :advacompac{i})");
                                        }

                                        paramList.Add(new OracleParameter($"advacompcp{i}", lstValues2In[i]));
                                        paramList.Add(new OracleParameter($"advacompac{i}", lstValuesIn[i]));
                                    }

                                    sql.Append($"))) ");

                                    break;
                                default:
                                    break;
                            }

                            break;

                        #endregion

                        case "codcomarca":

                            #region codComarca

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.In:
                                    List<string> lstValuesIn = objFilter.Value.Split(';').ToList();
                                    sql.Append($"and ((nvl(jur.processo.cod_comarca, 0) in (");

                                    for (var i = 0; i < lstValuesIn.Count(); i++)
                                    {
                                        if (i == 0)
                                        {
                                            sql.Append($":codcom{i}");
                                        }
                                        else if (i % 1000 == 0 && i != 0)
                                        {
                                            sql.Append($")) or (nvl(jur.processo.cod_comarca, 0) in (");
                                            sql.Append($":codcom{i}");
                                        }
                                        else
                                        {
                                            sql.Append($", :codcom{i}");
                                        }

                                        paramList.Add(new OracleParameter($"codcom{i}", lstValuesIn[i]));
                                    }

                                    sql.Append($"))) ");
                                    break;
                                default:
                                    break;
                            }

                            break;

                        #endregion

                        case "empresagrupo":

                            #region empresaGrupo

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.In:
                                    List<string> lstValuesIn = objFilter.Value.Split(';').ToList();
                                    sql.Append($"and ((nvl(jur.processo.cod_parte_empresa, 0) in (");

                                    for (var i = 0; i < lstValuesIn.Count(); i++)
                                    {
                                        if (i == 0)
                                        {
                                            sql.Append($":empgr{i}");
                                        }
                                        else if (i % 1000 == 0 && i != 0)
                                        {
                                            sql.Append($")) or (nvl(jur.processo.cod_parte_empresa, 0) in (");
                                            sql.Append($":empgr{i}");
                                        }
                                        else
                                        {
                                            sql.Append($", :empgr{i}");
                                        }

                                        paramList.Add(new OracleParameter($"empgr{i}", lstValuesIn[i]));
                                    }

                                    sql.Append($"))) ");
                                    break;
                                default:
                                    break;
                            }

                            break;

                        #endregion

                        case "escritorioaudiencia":

                            #region escritorioAudiencia

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.In_Escritorio_Audiencia:
                                    List<string> lstValuesIn = objFilter.Value.Split(';').ToList();
                                    sql.Append($"and ((nvl(jur.audiencia_processo.adves_cod_profissional, 0) in (");

                                    for (var i = 0; i < lstValuesIn.Count(); i++)
                                    {
                                        if (i == 0)
                                        {
                                            sql.Append($":escaud{i}");
                                        }
                                        else if (i % 1000 == 0 && i != 0)
                                        {
                                            sql.Append($")) or (nvl(jur.audiencia_processo.adves_cod_profissional, 0) in (");
                                            sql.Append($":escaud{i}");
                                        }
                                        else
                                        {
                                            sql.Append($", :escaud{i}");
                                        }

                                        paramList.Add(new OracleParameter($"escaud{i}", lstValuesIn[i]));
                                    }

                                    sql.Append($"))) ");
                                    break;
                                case FilterOperatorEnum.In_Escritorio_Processo:
                                    List<string> lstValuesIn2 = objFilter.Value.Split(';').ToList();
                                    sql.Append($"and ((nvl(jur.processo.cod_profissional, 0) in (");

                                    for (var i = 0; i < lstValuesIn2.Count(); i++)
                                    {
                                        if (i == 0)
                                        {
                                            sql.Append($":escprc{i}");
                                        }
                                        else if (i % 1000 == 0 && i != 0)
                                        {
                                            sql.Append($")) or (nvl(jur.processo.cod_profissional, 0) in (");
                                            sql.Append($":escprc{i}");
                                        }
                                        else
                                        {
                                            sql.Append($", :escprc{i}");
                                        }

                                        paramList.Add(new OracleParameter($"escprc{i}", lstValuesIn2[i]));
                                    }

                                    sql.Append($"))) ");
                                    break;
                                default:
                                    break;
                            }

                            break;

                        #endregion

                        case "escritorioacompanhante":

                            #region escritorioAcompanhante

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.In:
                                    List<string> lstValuesIn = objFilter.Value.Split(';').ToList();
                                    sql.Append($"and ((nvl(jur.audiencia_processo.adves_cod_profissional_acomp, 0) in (");

                                    for (var i = 0; i < lstValuesIn.Count(); i++)
                                    {
                                        if (i == 0)
                                        {
                                            sql.Append($":escacomp{i}");
                                        }
                                        else if (i % 1000 == 0 && i != 0)
                                        {
                                            sql.Append($")) or (nvl(jur.audiencia_processo.adves_cod_profissional_acomp, 0) in (");
                                            sql.Append($":escacomp{i}");
                                        }
                                        else
                                        {
                                            sql.Append($", :escacomp{i}");
                                        }

                                        paramList.Add(new OracleParameter($"escacomp{i}", lstValuesIn[i]));
                                    }

                                    sql.Append($"))) ");
                                    break;
                                default:
                                    break;
                            }

                            break;

                        #endregion

                        case "siglaestado":

                            #region siglaEstado

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.In:
                                    List<string> lstValuesIn = objFilter.Value.Split(';').ToList();
                                    sql.Append($"and ((UPPER(jur.comarca.cod_estado) in (");

                                    for (var i = 0; i < lstValuesIn.Count(); i++)
                                    {
                                        if (i == 0)
                                        {
                                            sql.Append($":sigest{i}");
                                        }
                                        else if (i % 1000 == 0 && i != 0)
                                        {
                                            sql.Append($")) or (UPPER(jur.comarca.cod_estado) in (");
                                            sql.Append($":sigest{i}");
                                        }
                                        else
                                        {
                                            sql.Append($", :sigest{i}");
                                        }

                                        paramList.Add(new OracleParameter($"sigest{i}", lstValuesIn[i]));
                                    }

                                    sql.Append($"))) ");
                                    break;
                                default:
                                    break;
                            }

                            break;

                        #endregion

                        case "preposto":

                            #region preposto

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.In:
                                    List<string> lstValuesIn = objFilter.Value.Split(';').ToList();
                                    sql.Append($"and ((nvl(jur.audiencia_processo.cod_preposto, 0) in (");

                                    for (var i = 0; i < lstValuesIn.Count(); i++)
                                    {
                                        if (i == 0)
                                        {
                                            sql.Append($":prep{i}");
                                        }
                                        else if (i % 1000 == 0 && i != 0)
                                        {
                                            sql.Append($")) or (nvl(jur.audiencia_processo.cod_preposto, 0) in (");
                                            sql.Append($":prep{i}");
                                        }
                                        else
                                        {
                                            sql.Append($", :prep{i}");
                                        }

                                        paramList.Add(new OracleParameter($"prep{i}", lstValuesIn[i]));
                                    }

                                    sql.Append($"))) ");
                                    break;
                                default:
                                    break;
                            }

                            break;

                        #endregion

                        case "prepostoacompanhante":

                            #region prepostoAcompanhante

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.In:
                                    List<string> lstValuesIn = objFilter.Value.Split(';').ToList();
                                    sql.Append($"and ((nvl(jur.audiencia_processo.cod_preposto_acomp, 0) in (");

                                    for (var i = 0; i < lstValuesIn.Count(); i++)
                                    {
                                        if (i == 0)
                                        {
                                            sql.Append($":prepacomp{i}");
                                        }
                                        else if (i % 1000 == 0 && i != 0)
                                        {
                                            sql.Append($")) or (nvl(jur.audiencia_processo.cod_preposto_acomp, 0) in (");
                                            sql.Append($":prepacomp{i}");
                                        }
                                        else
                                        {
                                            sql.Append($", :prepacomp{i}");
                                        }

                                        paramList.Add(new OracleParameter($"prepacomp{i}", lstValuesIn[i]));
                                    }

                                    sql.Append($"))) ");
                                    break;
                                default:
                                    break;
                            }

                            break;

                        #endregion

                        case "codprocesso":

                            #region codProcesso

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.In:
                                    List<string> lstValuesIn = objFilter.Value.Split(';').ToList();
                                    sql.Append($"and ((nvl(jur.processo.cod_processo, 0) in (");

                                    for (var i = 0; i < lstValuesIn.Count(); i++)
                                    {
                                        if (i == 0)
                                        {
                                            sql.Append($":proc{i}");
                                        }
                                        else if (i % 1000 == 0 && i != 0)
                                        {
                                            sql.Append($")) or (nvl(jur.processo.cod_processo, 0) in (");
                                            sql.Append($":proc{i}");
                                        }
                                        else
                                        {
                                            sql.Append($", :proc{i}");
                                        }

                                        paramList.Add(new OracleParameter($"proc{i}", lstValuesIn[i]));
                                    }

                                    sql.Append($"))) ");
                                    break;
                                default:
                                    break;
                            }

                            break;

                        #endregion

                        case "vara":

                            #region vara

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.EqualsTo:
                                    List<string> lstValuesIn = objFilter.Value.Split(';').ToList(); // ["1,2,3", "1,2,3", "1,2,3"]
                                    sql.Append($"and (");

                                    for (var i = 0; i < lstValuesIn.Count(); i++)
                                    {
                                        var lstTemp = lstValuesIn[i].Split(',').ToList(); // ["1","2","3"] - Sempre na ordem (comarca, vara, tipovara)

                                        if (i == 0)
                                        {
                                            for (var j = 0; j < lstTemp.Count(); j++)
                                            {
                                                if (j == 0)
                                                {
                                                    sql.Append($"(jur.processo.cod_comarca = :vara{i}{j} ");
                                                }
                                                else if (j == 1)
                                                {
                                                    sql.Append($"and jur.processo.cod_vara = :vara{i}{j} ");
                                                }
                                                else if (j == 2)
                                                {
                                                    sql.Append($"and jur.processo.cod_tipo_vara = :vara{i}{j}) ");
                                                }

                                                paramList.Add(new OracleParameter($"vara{i}{j}", lstTemp[j]));
                                            }
                                        }
                                        else
                                        {
                                            for (var j = 0; j < lstTemp.Count(); j++)
                                            {
                                                if (j == 0)
                                                {
                                                    sql.Append($"or (jur.processo.cod_comarca = :vara{i}{j} ");
                                                }
                                                else if (j == 1)
                                                {
                                                    sql.Append($"and jur.processo.cod_vara = :vara{i}{j} ");
                                                }
                                                else if (j == 2)
                                                {
                                                    sql.Append($"and jur.processo.cod_tipo_vara = :vara{i}{j}) ");
                                                }

                                                paramList.Add(new OracleParameter($"vara{i}{j}", lstTemp[j]));
                                            }
                                        }

                                    }

                                    sql.Append($") ");
                                    break;
                                default:
                                    break;
                            }

                            break;

                        #endregion

                        case "classificacaoclosing":

                            #region classificacaoClosing

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.EqualsTo:
                                    if (Convert.ToInt32(objFilter.Value) < 4 && Convert.ToInt32(objFilter.Value) > 0)
                                    {
                                        sql.Append($"and (jur.processo.cod_classificacao_closing = '{Convert.ToInt32(objFilter.Value)}') ");
                                    }
                                    break;

                                default:
                                    break;
                            }

                            break;

                        #endregion

                        default: break;
                    }

                    count++;
                }

                #endregion

                //orders.Add(new SortOrder{Direction = "ASC", Property = "Reclamante"});
                //orders.Add(new SortOrder { Direction = "ASC", Property = "Pedido" });

                sql.Append("ORDER BY" + GetOrderBy(orders));
                sql.Append(", pt.NOM_PARTE ASC, dsc_pedido asc");

                if (!IsExportMethod)
                {
                    sql.Append(@") a
                                 WHERE rownum < ((:pageNumber * :pageSize) + 1 )
                                 ) WHERE r__ >= (((:pageNumber-1) * :pageSize) + 1)");

                    paramList.Add(new OracleParameter("pageNumber", pageNumber));
                    paramList.Add(new OracleParameter("pageSize", pageSize));
                }

                AbrirConexao();
                dataReader = ExecuteReader(sql.ToString(), paramList.ToArray());

                while (dataReader.Read())
                {
                    var obj = new AudienciaProcessoCompletoResultadoDTO
                    {
                        CodProcesso = dataReader.GetInt64(0),
                        SeqAudiencia = dataReader.GetInt64(1),
                        SiglaEstado = dataReader.IsDBNull(2) ? string.Empty : dataReader.GetString(2).Trim(),
                        Comarca = dataReader.IsDBNull(3) ? string.Empty : dataReader.GetString(3).Trim(),
                        TipoVara = dataReader.IsDBNull(4) ? string.Empty : dataReader.GetString(4).Trim(),
                        CodVara = dataReader.GetInt32(5),
                        DataAudiencia = dataReader.IsDBNull(6) ? string.Empty : dataReader.GetDateTime(6).ToShortDateString(),
                        HorarioAudiencia = dataReader.IsDBNull(7) ? string.Empty : dataReader.GetDateTime(7).ToShortTimeString(),
                        TipoAudiencia = dataReader.IsDBNull(8) ? string.Empty : dataReader.GetString(8).Trim(),
                        Preposto = dataReader.IsDBNull(9) ? string.Empty : dataReader.GetString(9).Trim(),
                        EscritorioAudiencia = dataReader.IsDBNull(10) ? string.Empty : dataReader.GetString(10).Trim(),
                        AdvogadoAudiencia = dataReader.IsDBNull(11) ? string.Empty : dataReader.GetString(11).Trim(),
                        PrepostoAcompanhante = dataReader.IsDBNull(12) ? string.Empty : dataReader.GetString(12).Trim(),
                        EscritorioAcompanhante = dataReader.IsDBNull(13) ? string.Empty : dataReader.GetString(13).Trim(),
                        AdvogadoAcompanhante = dataReader.IsDBNull(14) ? string.Empty : dataReader.GetString(14).Trim(),
                        TipoProcesso = dataReader.IsDBNull(15) ? string.Empty : dataReader.GetString(15).Trim(),
                        Estrategico = dataReader.IsDBNull(16) ? string.Empty : dataReader.GetString(16).Trim(),
                        NumeroProcesso = dataReader.IsDBNull(17) ? string.Empty : dataReader.GetString(17).Trim(),
                        ClassificacaoHierarquica = dataReader.IsDBNull(18) ? string.Empty : dataReader.GetString(18).Trim(),
                        EmpresaGrupo = dataReader.IsDBNull(19) ? string.Empty : dataReader.GetString(19).Trim(),
                        Endereco = dataReader.IsDBNull(20) ? string.Empty : dataReader.GetString(20).Trim(),
                        EscritorioProcesso = dataReader.IsDBNull(21) ? string.Empty : dataReader.GetString(21).Trim(),
                        DddAdvogadoEscritorio = dataReader.IsDBNull(22) ? string.Empty : dataReader.GetString(22).Trim(),
                        NroAdvogadoEscritorio = dataReader.IsDBNull(23) ? string.Empty : dataReader.GetString(23).Trim(),
                        DddAdvogadoAcompanhante = dataReader.IsDBNull(24) ? string.Empty : dataReader.GetString(24).Trim(),
                        NroAdvogadoAcompanhante = dataReader.IsDBNull(25) ? string.Empty : dataReader.GetString(25).Trim(),
                        Migrado = dataReader.IsDBNull(26) ? string.Empty : dataReader.GetString(26).Trim(),
                        Reclamadas = dataReader.IsDBNull(27) ? string.Empty : dataReader.GetString(27).Trim(),
                        Reclamante = dataReader.IsDBNull(28) ? string.Empty : dataReader.GetString(28).Trim(),
                        Pedido = dataReader.IsDBNull(29) ? string.Empty : dataReader.GetString(29).Trim()
                    };

                    list.Add(obj);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                }

                FecharConexao();
            }

            return list;
        }


        public string ObterQueryRelatorio(List<Filter> lstFilters, int pageNumber, int pageSize, List<SortOrder> orders, bool IsExportMethod = false)
        {
            var list = new List<AudienciaProcessoResultadoDTO>();
            var paramList = new List<OracleParameter>();
            OracleDataReader dataReader = null;

            try
            {
                var sql = new StringBuilder();

                if (!IsExportMethod)
                {
                    sql.Append(@"SELECT cod_processo_key, seq_audiencia_key, estado, comarca, tipo_vara, cod_vara, date_audiencia, horario_audiencia, 
                                   tipo_audiencia, preposto, escritorio_audiencia, advogado_escritorio, preposto_acompanhante, escritorio_acompanhante, 
                                   advogado_acompanhante, tipo_processo, estrategico, numero_processo, classificacao_hierarquica, empresa_grupo, endereco,
                                   escritorio_processo FROM
                             (
                                SELECT a.cod_processo_key, a.seq_audiencia_key, a.estado, a.comarca, a.tipo_vara, a.cod_vara, a.date_audiencia, a.horario_audiencia, 
                                       a.tipo_audiencia, a.preposto, a.escritorio_audiencia, a.advogado_escritorio, a.preposto_acompanhante, a.escritorio_acompanhante, 
                                       a.advogado_acompanhante, a.tipo_processo, a.estrategico, a.numero_processo, a.classificacao_hierarquica, a.empresa_grupo, a.endereco,
                                       a.escritorio_processo, rownum r__ FROM
                                ( ");
                }

                sql.Append(@"SELECT jur.audiencia_processo.cod_processo as cod_processo_key,
                                           jur.audiencia_processo.seq_audiencia as seq_audiencia_key,
                                           jur.comarca.cod_estado as estado,
                                           jur.comarca.nom_comarca as comarca,
                                           jur.tipo_vara.nom_tipo_vara as tipo_vara,
                                           jur.vara.cod_vara as cod_vara,      
                                           jur.audiencia_processo.dat_audiencia as date_audiencia,
                                           jur.audiencia_processo.hor_audiencia as horario_audiencia,
                                           jur.tipo_audiencia.dsc_tipo_audiencia as tipo_audiencia,
                                           Decode(jur.audiencia_processo.cod_preposto,
                                                  null,
                                                  '',
                                                  nvl(jur.preposto.cod_estado, '') || ' - ' ||
                                                  nvl(jur.preposto.nom_preposto, '') ||
                                                  decode(nvl(jur.preposto.ind_preposto_ativo, 'S'),
                                                         'N',
                                                         ' [Inativo]',
                                                         decode(nvl(jur.preposto.ind_preposto_trabalhista, 'S'),
                                                                'S',
                                                                '',
                                                                ' [Inativo]'))) as preposto,        
                                           UPPER(Decode(jur.audiencia_processo.adves_cod_profissional,
                                                  null,
                                                  '',
                                                  nvl(jur.escritorio_audiencia.nom_profissional, '') ||
                                                  decode(nvl(jur.escritorio_audiencia.ind_ativo, 'S'),
                                                         'N',
                                                         ' [Inativo]',
                                                         decode(nvl(jur.escritorio_audiencia.ind_area_trabalhista,
                                                                    'S'),
                                                                'S',
                                                                '',
                                                                ' [Inativo]')))) as escritorio_audiencia,
                                           jur.advogado_escritorio.nom_advogado as advogado_escritorio,
                                           Decode(jur.audiencia_processo.cod_preposto_acomp,
                                                  null,
                                                  '',
                                                  nvl(preposto_acompanhante.cod_estado, '') || ' - ' ||
                                                  nvl(preposto_acompanhante.nom_preposto, '') ||
                                                  decode(nvl(preposto_acompanhante.ind_preposto_ativo, 'S'),
                                                         'N',
                                                         ' [Inativo]',
                                                         decode(nvl(preposto_acompanhante.ind_preposto_trabalhista,
                                                                    'S'),
                                                                'S',
                                                                '',
                                                                ' [Inativo]'))) as preposto_acompanhante,
                                           UPPER(Decode(jur.audiencia_processo.adves_cod_profissional_acomp,
                                                  null,
                                                  '',
                                                  nvl(escritorio_acompanhante.nom_profissional, '') ||
                                                  decode(nvl(escritorio_acompanhante.ind_ativo, 'S'),
                                                         'N',
                                                         ' [Inativo]',
                                                         decode(nvl(escritorio_acompanhante.ind_area_trabalhista,
                                                                    'S'),
                                                                'S',
                                                                '',
                                                                ' [Inativo]')))) as escritorio_acompanhante,
                                           advogado_acompanhante.nom_advogado as advogado_acompanhante,
                                           jur.tipo_processo.dsc_tipo_processo as tipo_processo,
                                           Decode(jur.processo.cod_tipo_processo,
                                                  9,
                                                  'S',
                                                  jur.processo.ind_prioritaria) as estrategico,
                                           jur.processo.nro_processo_cartorio as numero_processo,       
                                           decode(jur.processo.cod_classificacao_processo,
                                                  'P',
                                                  'PRIMÁRIO',
                                                  decode(jur.processo.cod_classificacao_processo,
                                                         'S',
                                                         'SECUNDÁRIO',
                                                         'ÚNICO')) as classificacao_hierarquica,
                                           jur.parte.nom_parte as empresa_grupo,
                                           jur.vara.end_vara as endereco,
                                           jur.profissional.nom_profissional as escritorio_processo
                                      FROM jur.comarca,
                                           jur.processo,
                                           jur.tipo_vara,
                                           jur.vara,
                                           jur.acao,
                                           jur.audiencia_processo,
                                           jur.advogado_escritorio,
                                           jur.profissional,
                                           jur.profissional escritorio_audiencia,
                                           jur.tipo_audiencia,
                                           jur.preposto,
                                           jur.migracoes_tipos_processos,
                                           jur.preposto preposto_acompanhante,
                                           jur.profissional escritorio_acompanhante,
                                           jur.advogado_escritorio advogado_acompanhante,
                                           jur.parte,
                                           jur.tipo_processo
                                     WHERE (jur.processo.cod_comarca = jur.comarca.cod_comarca)
                                       and (jur.processo.cod_tipo_vara = jur.tipo_vara.cod_tipo_vara)
                                       and (jur.vara.cod_tipo_vara = jur.tipo_vara.cod_tipo_vara)
                                       and (jur.vara.cod_comarca = jur.processo.cod_comarca)
                                       and (jur.processo.cod_tipo_processo = jur.tipo_processo.cod_tipo_processo)
                                       and (jur.vara.cod_vara = jur.processo.cod_vara)
                                       and (jur.vara.cod_tipo_vara = jur.processo.cod_tipo_vara)
                                       and (jur.vara.cod_comarca = jur.comarca.cod_comarca)
                                       and (jur.processo.cod_acao = jur.acao.cod_acao)
                                       and (jur.processo.cod_processo = jur.audiencia_processo.cod_processo)
                                       and (jur.processo.cod_profissional = jur.profissional.cod_profissional)
                                       and (jur.processo.cod_parte_empresa = jur.parte.cod_parte)
                                       and (jur.tipo_audiencia.cod_tipo_aud(+) = jur.audiencia_processo.cod_tipo_aud)
                                       and (jur.preposto.cod_preposto(+) = jur.audiencia_processo.cod_preposto)
                                       and (jur.migracoes_tipos_processos.proc_cod_processo_para(+) = jur.processo.cod_processo)
                                       and (jur.advogado_escritorio.cod_advogado(+) = jur.audiencia_processo.adves_cod_advogado)
                                       and (jur.advogado_escritorio.cod_profissional(+) = jur.audiencia_processo.adves_cod_profissional)
                                       and (jur.audiencia_processo.adves_cod_profissional = jur.escritorio_audiencia.cod_profissional(+))
                                       and (jur.audiencia_processo.cod_preposto_acomp = preposto_acompanhante.cod_preposto(+))
                                       and (jur.audiencia_processo.adves_cod_profissional_acomp = escritorio_acompanhante.cod_profissional(+))
                                       and (jur.audiencia_processo.adves_cod_profissional_acomp = advogado_acompanhante.cod_profissional(+))
                                       and (jur.audiencia_processo.adves_cod_advogado_acomp = advogado_acompanhante.cod_advogado(+))
                                       and (jur.processo.cod_tipo_processo = 2) ");

                #region "Dynamic Filters"

                var count = 0;
                var classificacaoHierarquicaDictionary = new Dictionary<int, char>();
                classificacaoHierarquicaDictionary.Add(1, 'U');
                classificacaoHierarquicaDictionary.Add(2, 'P');
                classificacaoHierarquicaDictionary.Add(3, 'S');

                foreach (Filter objFilter in lstFilters.Where(x => !string.IsNullOrEmpty(x.FieldName) && !string.IsNullOrEmpty(x.Value)))
                {
                    switch (objFilter.FieldName.ToLower())
                    {
                        case "estrategico":

                            #region estrategico

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.EqualsTo:
                                    if (Convert.ToInt32(objFilter.Value) < 3 && Convert.ToInt32(objFilter.Value) > 0)
                                    {
                                        var filtro = objFilter.Value == "1" ? 'S' : 'N';
                                        sql.Append($"and (jur.processo.ind_prioritaria = '{filtro}') ");
                                    }
                                    break;

                                default:
                                    break;
                            }

                            break;

                        #endregion

                        case "periodopendenciacalculo":

                            #region periodoPendenciaCalculo

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.Between:
                                    List<string> lstDate = objFilter.Value.Split(';').ToList();

                                    sql.Append($"and (Exists (select 1 from jur.parte_pedido_processo where jur.parte_pedido_processo.cod_processo = jur.processo.cod_processo and (jur.parte_pedido_processo.data_revisao_valor between TO_DATE('{lstDate[0].Trim()} 00:00:00', 'YYYY-MM-DD HH24:MI:SS') and TO_DATE('{lstDate[1].Trim()} 23:59:59', 'YYYY-MM-DD HH24:MI:SS')))) ");

                                    break;

                                default:
                                    break;
                            }

                            break;

                        #endregion

                        case "dataaudiencia":

                            #region dataAudiencia

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.Between:
                                    List<string> lstDate = objFilter.Value.Split(';').ToList();

                                    sql.Append($"and (jur.audiencia_processo.dat_audiencia between TO_DATE('{lstDate[0].Trim()} 00:00:00', 'YYYY-MM-DD HH24:MI:SS') and TO_DATE('{lstDate[1].Trim()} 23:59:59', 'YYYY-MM-DD HH24:MI:SS')) ");

                                    break;

                                default:
                                    break;
                            }

                            break;

                        #endregion

                        case "classificacaohierarquica":

                            #region classificacaoHierarquica

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.EqualsTo:
                                    if (Convert.ToInt32(objFilter.Value) < 4 && Convert.ToInt32(objFilter.Value) > 0)
                                    {
                                        sql.Append($"and (jur.processo.cod_classificacao_processo = '{classificacaoHierarquicaDictionary.GetValueOrDefault(Convert.ToInt32(objFilter.Value))}') ");
                                    }
                                    break;

                                default:
                                    break;
                            }

                            break;

                        #endregion

                        case "advogadoaudiencia":

                            #region advogadoAudiencia

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.In:
                                    List<string> lstValuesIn = objFilter.Value.Split(';').ToList();
                                    List<string> lstValues2In = objFilter.Value2.Split(';').ToList();
                                    sql.Append($"and (((nvl(jur.advogado_escritorio.cod_profissional, 0), nvl(jur.advogado_escritorio.cod_advogado, 0))  in (");

                                    for (var i = 0; i < lstValuesIn.Count(); i++)
                                    {
                                        if (i == 0)
                                        {
                                            sql.Append($"(:advacompcp{i} , :advacompae{i})");
                                        }
                                        else if (i % 1000 == 0 && i != 0)
                                        {
                                            sql.Append($")) or (nvl(jur.advogado_escritorio.cod_profissional, 0), nvl(jur.advogado_escritorio.cod_advogado, 0)) in (");
                                            sql.Append($"(:advacompcp{i} , :advacompae{i})");
                                        }
                                        else
                                        {
                                            sql.Append($", (:advacompcp{i} , :advacompae{i})");
                                        }

                                        paramList.Add(new OracleParameter($"advaudcp{i}", lstValues2In[i]));
                                        paramList.Add(new OracleParameter($"advaudae{i}", lstValuesIn[i]));
                                    }

                                    sql.Append($"))) ");

                                    break;

                                default:
                                    break;
                            }

                            break;

                        #endregion

                        case "advogadoacompanhante":

                            #region advogadoAcompanhante

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.In:
                                    List<string> lstValuesIn = objFilter.Value.Split(';').ToList();
                                    List<string> lstValues2In = objFilter.Value2.Split(';').ToList();
                                    sql.Append($"and (((nvl(advogado_acompanhante.cod_profissional, 0), nvl(advogado_acompanhante.cod_advogado, 0))  in (");

                                    for (var i = 0; i < lstValuesIn.Count(); i++)
                                    {
                                        if (i == 0)
                                        {
                                            sql.Append($"(:advacompcp{i} , :advacompac{i})");
                                        }
                                        else if (i % 1000 == 0 && i != 0)
                                        {
                                            sql.Append($")) or (nvl(advogado_acompanhante.cod_profissional, 0), nvl(advogado_acompanhante.cod_advogado, 0)) in (");
                                            sql.Append($"(:advacompcp{i} , :advacompac{i})");
                                        }
                                        else
                                        {
                                            sql.Append($", (:advacompcp{i} , :advacompac{i})");
                                        }

                                        paramList.Add(new OracleParameter($"advacompcp{i}", lstValues2In[i]));
                                        paramList.Add(new OracleParameter($"advacompac{i}", lstValuesIn[i]));
                                    }

                                    sql.Append($"))) ");

                                    break;
                                default:
                                    break;
                            }

                            break;

                        #endregion

                        case "codcomarca":

                            #region codComarca

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.In:
                                    List<string> lstValuesIn = objFilter.Value.Split(';').ToList();
                                    sql.Append($"and ((nvl(jur.processo.cod_comarca, 0) in (");

                                    for (var i = 0; i < lstValuesIn.Count(); i++)
                                    {
                                        if (i == 0)
                                        {
                                            sql.Append($":codcom{i}");
                                        }
                                        else if (i % 1000 == 0 && i != 0)
                                        {
                                            sql.Append($")) or (nvl(jur.processo.cod_comarca, 0) in (");
                                            sql.Append($":codcom{i}");
                                        }
                                        else
                                        {
                                            sql.Append($", :codcom{i}");
                                        }

                                        paramList.Add(new OracleParameter($"codcom{i}", lstValuesIn[i]));
                                    }

                                    sql.Append($"))) ");
                                    break;
                                default:
                                    break;
                            }

                            break;

                        #endregion

                        case "empresagrupo":

                            #region empresaGrupo

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.In:
                                    List<string> lstValuesIn = objFilter.Value.Split(';').ToList();
                                    sql.Append($"and ((nvl(jur.processo.cod_parte_empresa, 0) in (");

                                    for (var i = 0; i < lstValuesIn.Count(); i++)
                                    {
                                        if (i == 0)
                                        {
                                            sql.Append($":empgr{i}");
                                        }
                                        else if (i % 1000 == 0 && i != 0)
                                        {
                                            sql.Append($")) or (nvl(jur.processo.cod_parte_empresa, 0) in (");
                                            sql.Append($":empgr{i}");
                                        }
                                        else
                                        {
                                            sql.Append($", :empgr{i}");
                                        }

                                        paramList.Add(new OracleParameter($"empgr{i}", lstValuesIn[i]));
                                    }

                                    sql.Append($"))) ");
                                    break;
                                default:
                                    break;
                            }

                            break;

                        #endregion

                        case "escritorioaudiencia":

                            #region escritorioAudiencia

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.In_Escritorio_Audiencia:
                                    List<string> lstValuesIn = objFilter.Value.Split(';').ToList();
                                    sql.Append($"and ((nvl(jur.audiencia_processo.adves_cod_profissional, 0) in (");

                                    for (var i = 0; i < lstValuesIn.Count(); i++)
                                    {
                                        if (i == 0)
                                        {
                                            sql.Append($":escaud{i}");
                                        }
                                        else if (i % 1000 == 0 && i != 0)
                                        {
                                            sql.Append($")) or (nvl(jur.audiencia_processo.adves_cod_profissional, 0) in (");
                                            sql.Append($":escaud{i}");
                                        }
                                        else
                                        {
                                            sql.Append($", :escaud{i}");
                                        }

                                        paramList.Add(new OracleParameter($"escaud{i}", lstValuesIn[i]));
                                    }

                                    sql.Append($"))) ");
                                    break;
                                case FilterOperatorEnum.In_Escritorio_Processo:
                                    List<string> lstValuesIn2 = objFilter.Value.Split(';').ToList();
                                    sql.Append($"and ((nvl(jur.processo.cod_profissional, 0) in (");

                                    for (var i = 0; i < lstValuesIn2.Count(); i++)
                                    {
                                        if (i == 0)
                                        {
                                            sql.Append($":escprc{i}");
                                        }
                                        else if (i % 1000 == 0 && i != 0)
                                        {
                                            sql.Append($")) or (nvl(jur.processo.cod_profissional, 0) in (");
                                            sql.Append($":escprc{i}");
                                        }
                                        else
                                        {
                                            sql.Append($", :escprc{i}");
                                        }

                                        paramList.Add(new OracleParameter($"escprc{i}", lstValuesIn2[i]));
                                    }

                                    sql.Append($"))) ");
                                    break;
                                default:
                                    break;
                            }

                            break;

                        #endregion

                        case "escritorioacompanhante":

                            #region escritorioAcompanhante

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.In:
                                    List<string> lstValuesIn = objFilter.Value.Split(';').ToList();
                                    sql.Append($"and ((nvl(jur.audiencia_processo.adves_cod_profissional_acomp, 0) in (");

                                    for (var i = 0; i < lstValuesIn.Count(); i++)
                                    {
                                        if (i == 0)
                                        {
                                            sql.Append($":escacomp{i}");
                                        }
                                        else if (i % 1000 == 0 && i != 0)
                                        {
                                            sql.Append($")) or (nvl(jur.audiencia_processo.adves_cod_profissional_acomp, 0) in (");
                                            sql.Append($":escacomp{i}");
                                        }
                                        else
                                        {
                                            sql.Append($", :escacomp{i}");
                                        }

                                        paramList.Add(new OracleParameter($"escacomp{i}", lstValuesIn[i]));
                                    }

                                    sql.Append($"))) ");
                                    break;
                                default:
                                    break;
                            }

                            break;

                        #endregion

                        case "siglaestado":

                            #region siglaEstado

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.In:
                                    List<string> lstValuesIn = objFilter.Value.Split(';').ToList();
                                    sql.Append($"and ((UPPER(jur.comarca.cod_estado) in (");

                                    for (var i = 0; i < lstValuesIn.Count(); i++)
                                    {
                                        if (i == 0)
                                        {
                                            sql.Append($":sigest{i}");
                                        }
                                        else if (i % 1000 == 0 && i != 0)
                                        {
                                            sql.Append($")) or (UPPER(jur.comarca.cod_estado) in (");
                                            sql.Append($":sigest{i}");
                                        }
                                        else
                                        {
                                            sql.Append($", :sigest{i}");
                                        }

                                        paramList.Add(new OracleParameter($"sigest{i}", lstValuesIn[i]));
                                    }

                                    sql.Append($"))) ");
                                    break;
                                default:
                                    break;
                            }

                            break;

                        #endregion

                        case "preposto":

                            #region preposto

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.In:
                                    List<string> lstValuesIn = objFilter.Value.Split(';').ToList();
                                    sql.Append($"and ((nvl(jur.audiencia_processo.cod_preposto, 0) in (");

                                    for (var i = 0; i < lstValuesIn.Count(); i++)
                                    {
                                        if (i == 0)
                                        {
                                            sql.Append($":prep{i}");
                                        }
                                        else if (i % 1000 == 0 && i != 0)
                                        {
                                            sql.Append($")) or (nvl(jur.audiencia_processo.cod_preposto, 0) in (");
                                            sql.Append($":prep{i}");
                                        }
                                        else
                                        {
                                            sql.Append($", :prep{i}");
                                        }

                                        paramList.Add(new OracleParameter($"prep{i}", lstValuesIn[i]));
                                    }

                                    sql.Append($"))) ");
                                    break;
                                default:
                                    break;
                            }

                            break;

                        #endregion

                        case "prepostoacompanhante":

                            #region prepostoAcompanhante

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.In:
                                    List<string> lstValuesIn = objFilter.Value.Split(';').ToList();
                                    sql.Append($"and ((nvl(jur.audiencia_processo.cod_preposto_acomp, 0) in (");

                                    for (var i = 0; i < lstValuesIn.Count(); i++)
                                    {
                                        if (i == 0)
                                        {
                                            sql.Append($":prepacomp{i}");
                                        }
                                        else if (i % 1000 == 0 && i != 0)
                                        {
                                            sql.Append($")) or (nvl(jur.audiencia_processo.cod_preposto_acomp, 0) in (");
                                            sql.Append($":prepacomp{i}");
                                        }
                                        else
                                        {
                                            sql.Append($", :prepacomp{i}");
                                        }

                                        paramList.Add(new OracleParameter($"prepacomp{i}", lstValuesIn[i]));
                                    }

                                    sql.Append($"))) ");
                                    break;
                                default:
                                    break;
                            }

                            break;

                        #endregion

                        case "codprocesso":

                            #region codProcesso

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.In:
                                    List<string> lstValuesIn = objFilter.Value.Split(';').ToList();
                                    sql.Append($"and ((nvl(jur.processo.cod_processo, 0) in (");

                                    for (var i = 0; i < lstValuesIn.Count(); i++)
                                    {
                                        if (i == 0)
                                        {
                                            sql.Append($":proc{i}");
                                        }
                                        else if (i % 1000 == 0 && i != 0)
                                        {
                                            sql.Append($")) or (nvl(jur.processo.cod_processo, 0) in (");
                                            sql.Append($":proc{i}");
                                        }
                                        else
                                        {
                                            sql.Append($", :proc{i}");
                                        }

                                        paramList.Add(new OracleParameter($"proc{i}", lstValuesIn[i]));
                                    }

                                    sql.Append($"))) ");
                                    break;
                                default:
                                    break;
                            }

                            break;

                        #endregion

                        case "vara":

                            #region vara

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.EqualsTo:
                                    List<string> lstValuesIn = objFilter.Value.Split(';').ToList(); // ["1,2,3", "1,2,3", "1,2,3"]
                                    sql.Append($"and (");

                                    for (var i = 0; i < lstValuesIn.Count(); i++)
                                    {
                                        var lstTemp = lstValuesIn[i].Split(',').ToList(); // ["1","2","3"] - Sempre na ordem (comarca, vara, tipovara)

                                        if (i == 0)
                                        {
                                            for (var j = 0; j < lstTemp.Count(); j++)
                                            {
                                                if (j == 0)
                                                {
                                                    sql.Append($"(jur.processo.cod_comarca = :vara{i}{j} ");
                                                }
                                                else if (j == 1)
                                                {
                                                    sql.Append($"and jur.processo.cod_vara = :vara{i}{j} ");
                                                }
                                                else if (j == 2)
                                                {
                                                    sql.Append($"and jur.processo.cod_tipo_vara = :vara{i}{j}) ");
                                                }

                                                paramList.Add(new OracleParameter($"vara{i}{j}", lstTemp[j]));
                                            }
                                        }
                                        else
                                        {
                                            for (var j = 0; j < lstTemp.Count(); j++)
                                            {
                                                if (j == 0)
                                                {
                                                    sql.Append($"or (jur.processo.cod_comarca = :vara{i}{j} ");
                                                }
                                                else if (j == 1)
                                                {
                                                    sql.Append($"and jur.processo.cod_vara = :vara{i}{j} ");
                                                }
                                                else if (j == 2)
                                                {
                                                    sql.Append($"and jur.processo.cod_tipo_vara = :vara{i}{j}) ");
                                                }

                                                paramList.Add(new OracleParameter($"vara{i}{j}", lstTemp[j]));
                                            }
                                        }

                                    }

                                    sql.Append($") ");
                                    break;
                                default:
                                    break;
                            }

                            break;

                        #endregion

                        default: break;
                    }

                    count++;
                }

                #endregion

                sql.Append("ORDER BY" + GetOrderBy(orders));

                if (!IsExportMethod)
                {
                    sql.Append(@") a
                                 WHERE rownum < ((:pageNumber * :pageSize) + 1 )
                                 ) WHERE r__ >= (((:pageNumber-1) * :pageSize) + 1)");

                    paramList.Add(new OracleParameter("pageNumber", pageNumber));
                    paramList.Add(new OracleParameter("pageSize", pageSize));
                }

                return sql.ToString();

            }
            catch (Exception ex)
            {
                throw ex;
            }          
        }


        public int GetTotalCount(List<Filter> lstFilters)
        {
            OracleDataReader dataReader = null;

            try
            {
                var sql = new StringBuilder();
                var paramList = new List<OracleParameter>();

                sql.Append(@"SELECT Count(*)
                                      FROM jur.comarca,
                                           jur.processo,
                                           jur.tipo_vara,
                                           jur.vara,
                                           jur.acao,
                                           jur.audiencia_processo,
                                           jur.advogado_escritorio,
                                           jur.profissional,
                                           jur.profissional escritorio_audiencia,
                                           jur.tipo_audiencia,
                                           jur.preposto,
                                           jur.migracoes_tipos_processos,
                                           jur.preposto preposto_acompanhante,
                                           jur.profissional escritorio_acompanhante,
                                           jur.advogado_escritorio advogado_acompanhante,
                                           jur.parte,
                                           jur.tipo_processo
                                     WHERE (jur.processo.cod_comarca = jur.comarca.cod_comarca)
                                       and (jur.processo.cod_tipo_vara = jur.tipo_vara.cod_tipo_vara)
                                       and (jur.vara.cod_tipo_vara = jur.tipo_vara.cod_tipo_vara)
                                       and (jur.vara.cod_comarca = jur.processo.cod_comarca)
                                       and (jur.processo.cod_tipo_processo = jur.tipo_processo.cod_tipo_processo)
                                       and (jur.vara.cod_vara = jur.processo.cod_vara)
                                       and (jur.vara.cod_tipo_vara = jur.processo.cod_tipo_vara)
                                       and (jur.vara.cod_comarca = jur.comarca.cod_comarca)
                                       and (jur.processo.cod_acao = jur.acao.cod_acao)
                                       and (jur.processo.cod_processo = jur.audiencia_processo.cod_processo)
                                       and (jur.processo.cod_profissional = jur.profissional.cod_profissional)
                                       and (jur.processo.cod_parte_empresa = jur.parte.cod_parte)
                                       and (jur.tipo_audiencia.cod_tipo_aud(+) = jur.audiencia_processo.cod_tipo_aud)
                                       and (jur.preposto.cod_preposto(+) = jur.audiencia_processo.cod_preposto)
                                       and (jur.migracoes_tipos_processos.proc_cod_processo_para(+) = jur.processo.cod_processo)
                                       and (jur.advogado_escritorio.cod_advogado(+) = jur.audiencia_processo.adves_cod_advogado)
                                       and (jur.advogado_escritorio.cod_profissional(+) = jur.audiencia_processo.adves_cod_profissional)
                                       and (jur.audiencia_processo.adves_cod_profissional = jur.escritorio_audiencia.cod_profissional(+))
                                       and (jur.audiencia_processo.cod_preposto_acomp = preposto_acompanhante.cod_preposto(+))
                                       and (jur.audiencia_processo.adves_cod_profissional_acomp = escritorio_acompanhante.cod_profissional(+))
                                       and (jur.audiencia_processo.adves_cod_profissional_acomp = advogado_acompanhante.cod_profissional(+))
                                       and (jur.audiencia_processo.adves_cod_advogado_acomp = advogado_acompanhante.cod_advogado(+))
                                       and (jur.processo.cod_tipo_processo = 2) ");

                #region "Dynamic Filters"

                var count = 0;
                var classificacaoHierarquicaDictionary = new Dictionary<int, char>();
                classificacaoHierarquicaDictionary.Add(1, 'U');
                classificacaoHierarquicaDictionary.Add(2, 'P');
                classificacaoHierarquicaDictionary.Add(3, 'S');

                foreach (Filter objFilter in lstFilters.Where(x => !string.IsNullOrEmpty(x.FieldName) && !string.IsNullOrEmpty(x.Value)))
                {
                    switch (objFilter.FieldName.ToLower())
                    {
                        case "estrategico":

                            #region estrategico

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.EqualsTo:
                                    if (Convert.ToInt32(objFilter.Value) < 3 && Convert.ToInt32(objFilter.Value) > 0)
                                    {
                                        var filtro = objFilter.Value == "1" ? 'S' : 'N';
                                        sql.Append($"and (jur.processo.ind_prioritaria = '{filtro}') ");
                                    }
                                    break;


                                default:
                                    break;
                            }

                            break;

                        #endregion

                        case "periodopendenciacalculo":

                            #region periodoPendenciaCalculo

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.Between:
                                    List<string> lstDate = objFilter.Value.Split(';').ToList();

                                    sql.Append($"and (Exists (select 1 from jur.parte_pedido_processo where jur.parte_pedido_processo.cod_processo = jur.processo.cod_processo and (jur.parte_pedido_processo.data_revisao_valor between TO_DATE('{lstDate[0].Trim()} 00:00:00', 'YYYY-MM-DD HH24:MI:SS') and TO_DATE('{lstDate[1].Trim()} 23:59:59', 'YYYY-MM-DD HH24:MI:SS')))) ");

                                    break;

                                default:
                                    break;
                            }

                            break;

                        #endregion

                        case "dataaudiencia":

                            #region dataAudiencia

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.Between:
                                    List<string> lstDate = objFilter.Value.Split(';').ToList();

                                    sql.Append($"and (jur.audiencia_processo.dat_audiencia between TO_DATE('{lstDate[0].Trim()} 00:00:00', 'YYYY-MM-DD HH24:MI:SS') and TO_DATE('{lstDate[1].Trim()} 23:59:59', 'YYYY-MM-DD HH24:MI:SS')) ");

                                    break;

                                default:
                                    break;
                            }

                            break;

                        #endregion

                        case "classificacaohierarquica":

                            #region classificacaoHierarquica

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.EqualsTo:
                                    if (Convert.ToInt32(objFilter.Value) < 4 && Convert.ToInt32(objFilter.Value) > 0)
                                    {
                                        sql.Append($"and (jur.processo.cod_classificacao_processo = '{classificacaoHierarquicaDictionary.GetValueOrDefault(Convert.ToInt32(objFilter.Value))}') ");
                                    }
                                    break;

                                default:
                                    break;
                            }

                            break;

                        #endregion

                        case "advogadoaudiencia":

                            #region advogadoAudiencia

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.In:
                                    List<string> lstValuesIn = objFilter.Value.Split(';').ToList();
                                    List<string> lstValues2In = objFilter.Value2.Split(';').ToList();
                                    sql.Append($"and (((nvl(jur.advogado_escritorio.cod_profissional, 0), nvl(jur.advogado_escritorio.cod_advogado, 0))  in (");

                                    for (var i = 0; i < lstValuesIn.Count(); i++)
                                    {
                                        if (i == 0)
                                        {
                                            sql.Append($"(:advacompcp{i} , :advacompae{i})");
                                        }
                                        else if (i % 1000 == 0 && i != 0)
                                        {
                                            sql.Append($")) or (nvl(jur.advogado_escritorio.cod_profissional, 0), nvl(jur.advogado_escritorio.cod_advogado, 0)) in (");
                                            sql.Append($"(:advacompcp{i} , :advacompae{i})");
                                        }
                                        else
                                        {
                                            sql.Append($", (:advacompcp{i} , :advacompae{i})");
                                        }

                                        paramList.Add(new OracleParameter($"advaudcp{i}", lstValues2In[i]));
                                        paramList.Add(new OracleParameter($"advaudae{i}", lstValuesIn[i]));
                                    }

                                    sql.Append($"))) ");
                                    break;

                                default:
                                    break;
                            }

                            break;

                        #endregion

                        case "advogadoacompanhante":

                            #region advogadoAcompanhante

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.In:
                                    List<string> lstValuesIn = objFilter.Value.Split(';').ToList();
                                    List<string> lstValues2In = objFilter.Value2.Split(';').ToList();
                                    sql.Append($"and (((nvl(advogado_acompanhante.cod_profissional, 0), nvl(advogado_acompanhante.cod_advogado, 0))  in (");

                                    for (var i = 0; i < lstValuesIn.Count(); i++)
                                    {
                                        if (i == 0)
                                        {
                                            sql.Append($"(:advacompcp{i} , :advacompac{i})");
                                        }
                                        else if (i % 1000 == 0 && i != 0)
                                        {
                                            sql.Append($")) or (nvl(advogado_acompanhante.cod_profissional, 0), nvl(advogado_acompanhante.cod_advogado, 0)) in (");
                                            sql.Append($"(:advacompcp{i} , :advacompac{i})");
                                        }
                                        else
                                        {
                                            sql.Append($", (:advacompcp{i} , :advacompac{i})");
                                        }

                                        paramList.Add(new OracleParameter($"advacompcp{i}", lstValues2In[i]));
                                        paramList.Add(new OracleParameter($"advacompac{i}", lstValuesIn[i]));
                                    }

                                    sql.Append($"))) ");
                                    break;
                                default:
                                    break;
                            }

                            break;

                        #endregion

                        case "codcomarca":

                            #region codComarca

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.In:
                                    List<string> lstValuesIn = objFilter.Value.Split(';').ToList();
                                    sql.Append($"and ((nvl(jur.processo.cod_comarca, 0) in (");

                                    for (var i = 0; i < lstValuesIn.Count(); i++)
                                    {
                                        if (i == 0)
                                        {
                                            sql.Append($":codcom{i}");
                                        }
                                        else if (i % 1000 == 0 && i != 0)
                                        {
                                            sql.Append($")) or (nvl(jur.processo.cod_comarca, 0) in (");
                                            sql.Append($":codcom{i}");
                                        }
                                        else
                                        {
                                            sql.Append($", :codcom{i}");
                                        }

                                        paramList.Add(new OracleParameter($"codcom{i}", lstValuesIn[i]));
                                    }

                                    sql.Append($"))) ");
                                    break;
                                default:
                                    break;
                            }

                            break;

                        #endregion

                        case "empresagrupo":

                            #region empresaGrupo

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.In:
                                    List<string> lstValuesIn = objFilter.Value.Split(';').ToList();
                                    sql.Append($"and ((nvl(jur.processo.cod_parte_empresa, 0) in (");

                                    for (var i = 0; i < lstValuesIn.Count(); i++)
                                    {
                                        if (i == 0)
                                        {
                                            sql.Append($":empgr{i}");
                                        }
                                        else if (i % 1000 == 0 && i != 0)
                                        {
                                            sql.Append($")) or (nvl(jur.processo.cod_parte_empresa, 0) in (");
                                            sql.Append($":empgr{i}");
                                        }
                                        else
                                        {
                                            sql.Append($", :empgr{i}");
                                        }

                                        paramList.Add(new OracleParameter($"empgr{i}", lstValuesIn[i]));
                                    }

                                    sql.Append($"))) ");
                                    break;
                                default:
                                    break;
                            }

                            break;

                        #endregion

                        case "escritorioaudiencia":

                            #region escritorioAudiencia

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.In_Escritorio_Audiencia:
                                    List<string> lstValuesIn = objFilter.Value.Split(';').ToList();
                                    sql.Append($"and ((nvl(jur.audiencia_processo.adves_cod_profissional, 0) in (");

                                    for (var i = 0; i < lstValuesIn.Count(); i++)
                                    {
                                        if (i == 0)
                                        {
                                            sql.Append($":escaud{i}");
                                        }
                                        else if (i % 1000 == 0 && i != 0)
                                        {
                                            sql.Append($")) or (nvl(jur.audiencia_processo.adves_cod_profissional, 0) in (");
                                            sql.Append($":escaud{i}");
                                        }
                                        else
                                        {
                                            sql.Append($", :escaud{i}");
                                        }

                                        paramList.Add(new OracleParameter($"escaud{i}", lstValuesIn[i]));
                                    }

                                    sql.Append($"))) ");
                                    break;
                                case FilterOperatorEnum.In_Escritorio_Processo:
                                    List<string> lstValuesIn2 = objFilter.Value.Split(';').ToList();
                                    sql.Append($"and ((nvl(jur.processo.cod_profissional, 0) in (");

                                    for (var i = 0; i < lstValuesIn2.Count(); i++)
                                    {
                                        if (i == 0)
                                        {
                                            sql.Append($":escprc{i}");
                                        }
                                        else if (i % 1000 == 0 && i != 0)
                                        {
                                            sql.Append($")) or (nvl(jur.processo.cod_profissional, 0) in (");
                                            sql.Append($":escprc{i}");
                                        }
                                        else
                                        {
                                            sql.Append($", :escprc{i}");
                                        }

                                        paramList.Add(new OracleParameter($"escprc{i}", lstValuesIn2[i]));
                                    }

                                    sql.Append($"))) ");
                                    break;
                                default:
                                    break;
                            }

                            break;

                        #endregion

                        case "escritorioacompanhante":

                            #region escritorioAcompanhante

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.In:
                                    List<string> lstValuesIn = objFilter.Value.Split(';').ToList();
                                    sql.Append($"and ((nvl(jur.audiencia_processo.adves_cod_profissional_acomp, 0) in (");

                                    for (var i = 0; i < lstValuesIn.Count(); i++)
                                    {
                                        if (i == 0)
                                        {
                                            sql.Append($":escacomp{i}");
                                        }
                                        else if (i % 1000 == 0 && i != 0)
                                        {
                                            sql.Append($")) or (nvl(jur.audiencia_processo.adves_cod_profissional_acomp, 0) in (");
                                            sql.Append($":escacomp{i}");
                                        }
                                        else
                                        {
                                            sql.Append($", :escacomp{i}");
                                        }

                                        paramList.Add(new OracleParameter($"escacomp{i}", lstValuesIn[i]));
                                    }

                                    sql.Append($"))) ");
                                    break;
                                default:
                                    break;
                            }

                            break;

                        #endregion

                        case "siglaestado":

                            #region siglaEstado

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.In:
                                    List<string> lstValuesIn = objFilter.Value.Split(';').ToList();
                                    sql.Append($"and ((UPPER(jur.comarca.cod_estado) in (");

                                    for (var i = 0; i < lstValuesIn.Count(); i++)
                                    {
                                        if (i == 0)
                                        {
                                            sql.Append($":sigest{i}");
                                        }
                                        else if (i % 1000 == 0 && i != 0)
                                        {
                                            sql.Append($")) or (UPPER(jur.comarca.cod_estado) in (");
                                            sql.Append($":sigest{i}");
                                        }
                                        else
                                        {
                                            sql.Append($", :sigest{i}");
                                        }

                                        paramList.Add(new OracleParameter($"sigest{i}", lstValuesIn[i]));
                                    }

                                    sql.Append($"))) ");
                                    break;
                                default:
                                    break;
                            }

                            break;

                        #endregion

                        case "preposto":

                            #region preposto

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.In:
                                    List<string> lstValuesIn = objFilter.Value.Split(';').ToList();
                                    sql.Append($"and ((nvl(jur.audiencia_processo.cod_preposto, 0) in (");

                                    for (var i = 0; i < lstValuesIn.Count(); i++)
                                    {
                                        if (i == 0)
                                        {
                                            sql.Append($":prep{i}");
                                        }
                                        else if (i % 1000 == 0 && i != 0)
                                        {
                                            sql.Append($")) or (nvl(jur.audiencia_processo.cod_preposto, 0) in (");
                                            sql.Append($":prep{i}");
                                        }
                                        else
                                        {
                                            sql.Append($", :prep{i}");
                                        }

                                        paramList.Add(new OracleParameter($"prep{i}", lstValuesIn[i]));
                                    }

                                    sql.Append($"))) ");
                                    break;
                                default:
                                    break;
                            }

                            break;

                        #endregion

                        case "prepostoacompanhante":

                            #region prepostoAcompanhante

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.In:
                                    List<string> lstValuesIn = objFilter.Value.Split(';').ToList();
                                    sql.Append($"and ((nvl(jur.audiencia_processo.cod_preposto_acomp, 0) in (");

                                    for (var i = 0; i < lstValuesIn.Count(); i++)
                                    {
                                        if (i == 0)
                                        {
                                            sql.Append($":prepacomp{i}");
                                        }
                                        else if (i % 1000 == 0 && i != 0)
                                        {
                                            sql.Append($")) or (nvl(jur.audiencia_processo.cod_preposto_acomp, 0) in (");
                                            sql.Append($":prepacomp{i}");
                                        }
                                        else
                                        {
                                            sql.Append($", :prepacomp{i}");
                                        }

                                        paramList.Add(new OracleParameter($"prepacomp{i}", lstValuesIn[i]));
                                    }

                                    sql.Append($"))) ");
                                    break;
                                default:
                                    break;
                            }

                            break;

                        #endregion

                        case "codprocesso":

                            #region codProcesso

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.In:
                                    List<string> lstValuesIn = objFilter.Value.Split(';').ToList();
                                    sql.Append($"and ((nvl(jur.processo.cod_processo, 0) in (");

                                    for (var i = 0; i < lstValuesIn.Count(); i++)
                                    {
                                        if (i == 0)
                                        {
                                            sql.Append($":proc{i}");
                                        }
                                        else if (i % 1000 == 0 && i != 0)
                                        {
                                            sql.Append($")) or (nvl(jur.processo.cod_processo, 0) in (");
                                            sql.Append($":proc{i}");
                                        }
                                        else
                                        {
                                            sql.Append($", :proc{i}");
                                        }

                                        paramList.Add(new OracleParameter($"proc{i}", lstValuesIn[i]));
                                    }

                                    sql.Append($"))) ");
                                    break;
                                default:
                                    break;
                            }

                            break;

                        #endregion

                        case "vara":

                            #region vara

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.EqualsTo:
                                    List<string> lstValuesIn = objFilter.Value.Split(';').ToList(); // ["1,2,3", "1,2,3", "1,2,3"]
                                    sql.Append($"and (");

                                    for (var i = 0; i < lstValuesIn.Count(); i++)
                                    {
                                        var lstTemp = lstValuesIn[i].Split(',').ToList(); // ["1","2","3"] - Sempre na ordem (comarca, vara, tipovara)

                                        if (i == 0)
                                        {                                            
                                            for (var j = 0; j < lstTemp.Count(); j++)
                                            {
                                                if (j == 0)
                                                {
                                                    sql.Append($"(jur.processo.cod_comarca = :vara{i}{j} ");
                                                }
                                                else if (j == 1) 
                                                {
                                                    sql.Append($"and jur.processo.cod_vara = :vara{i}{j} ");
                                                }
                                                else if (j == 2)
                                                {
                                                    sql.Append($"and jur.processo.cod_tipo_vara = :vara{i}{j}) ");
                                                }

                                                paramList.Add(new OracleParameter($"vara{i}{j}", lstTemp[j]));
                                            }
                                        }
                                        else
                                        {
                                            for (var j = 0; j < lstTemp.Count(); j++)
                                            {
                                                if (j == 0)
                                                {
                                                    sql.Append($"or (jur.processo.cod_comarca = :vara{i}{j} ");
                                                }
                                                else if (j == 1)
                                                {
                                                    sql.Append($"and jur.processo.cod_vara = :vara{i}{j} ");
                                                }
                                                else if (j == 2)
                                                {
                                                    sql.Append($"and jur.processo.cod_tipo_vara = :vara{i}{j}) ");
                                                }

                                                paramList.Add(new OracleParameter($"vara{i}{j}", lstTemp[j]));
                                            }
                                        }
                                        
                                    }

                                    sql.Append($") ");
                                    break;
                                default:
                                    break;
                            }

                            break;

                        #endregion

                        case "classificacaoclosing":

                            #region classificacaoClosing

                            switch (objFilter.FilterOperator)
                            {
                                case FilterOperatorEnum.EqualsTo:
                                    if (Convert.ToInt32(objFilter.Value) < 4 && Convert.ToInt32(objFilter.Value) > 0)
                                    {
                                        sql.Append($"and (jur.processo.cod_classificacao_closing = '{Convert.ToInt32(objFilter.Value)}') ");
                                    }
                                    break;

                                default:
                                    break;
                            }

                            break;

                        #endregion

                        default: break;
                    }

                    count++;
                }

                #endregion

                AbrirConexao();
                dataReader = ExecuteReader(sql.ToString(), paramList.ToArray());

                if (dataReader.Read())
                {
                    return dataReader.IsDBNull(0) ? 0 : dataReader.GetInt32(0);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                }

                FecharConexao();
            }

            return 0;
        }

        private string GetOrderBy(IList<SortOrder> orders)
        {
            var orderBy = string.Empty;

            foreach(var order in orders)
            {
                switch (order.Property.ToLower())
                {
                    case "siglaestado":
                        if (order.Direction.ToLower().Equals("asc"))
                            orderBy += @" estado asc,";
                        else
                            orderBy += @" estado desc,";

                        break;
                    case "comarca":
                        if (order.Direction.ToLower().Equals("asc"))
                            orderBy += @" comarca asc,";
                        else
                            orderBy += @" comarca desc,";

                        break;
                    case "codvara":
                        if (order.Direction.ToLower().Equals("asc"))
                            orderBy += @" cod_vara asc,";
                        else
                            orderBy += @" cod_vara desc,";

                        break;
                    case "tipovara":
                        if (order.Direction.ToLower().Equals("asc"))
                            orderBy += @" tipo_vara asc,";
                        else
                            orderBy += @" tipo_vara desc,";

                        break;
                    case "dataaudiencia":
                        if (order.Direction.ToLower().Equals("asc"))
                            orderBy += @" date_audiencia asc,";
                        else
                            orderBy += @" date_audiencia desc,";

                        break;
                    case "horarioaudiencia":
                        if (order.Direction.ToLower().Equals("asc"))
                            orderBy += @" horario_audiencia asc,";
                        else
                            orderBy += @" horario_audiencia desc,";

                        break;
                    case "tipoaudiencia":
                        if (order.Direction.ToLower().Equals("asc"))
                            orderBy += @" tipo_audiencia asc,";
                        else
                            orderBy += @" tipo_audiencia desc,";

                        break;
                    case "preposto":
                        if (order.Direction.ToLower().Equals("asc"))
                            orderBy += @" preposto asc,";
                        else
                            orderBy += @" preposto desc,";

                        break;
                    case "escritorioaudiencia":
                        if (order.Direction.ToLower().Equals("asc"))
                            orderBy += @" escritorio_audiencia asc,";
                        else
                            orderBy += @" escritorio_audiencia desc,";

                        break;
                    case "advogadoaudiencia":
                        if (order.Direction.ToLower().Equals("asc"))
                            orderBy += @" advogado_escritorio asc,";
                        else
                            orderBy += @" advogado_escritorio desc,";

                        break;
                    case "prepostoacompanhante":
                        if (order.Direction.ToLower().Equals("asc"))
                            orderBy += @" preposto_acompanhante asc,";
                        else
                            orderBy += @" preposto_acompanhante desc,";

                        break;
                    case "escritorioacompanhante":
                        if (order.Direction.ToLower().Equals("asc"))
                            orderBy += @" escritorio_acompanhante asc,";
                        else
                            orderBy += @" escritorio_acompanhante desc,";

                        break;
                    case "advogadoacompanhante":
                        if (order.Direction.ToLower().Equals("asc"))
                            orderBy += @" advogado_acompanhante asc,";
                        else
                            orderBy += @" advogado_acompanhante desc,";

                        break;
                    case "tipoprocesso":
                        if (order.Direction.ToLower().Equals("asc"))
                            orderBy += @" tipo_processo asc,";
                        else
                            orderBy += @" tipo_processo desc,";

                        break;
                    case "estrategico":
                        if (order.Direction.ToLower().Equals("asc"))
                            orderBy += @" estrategico asc,";
                        else
                            orderBy += @" estrategico desc,";

                        break;
                    case "numeroprocesso":
                        if (order.Direction.ToLower().Equals("asc"))
                            orderBy += @" numero_processo asc,";
                        else
                            orderBy += @" numero_processo desc,";

                        break;
                    case "classificacaohierarquica":
                        if (order.Direction.ToLower().Equals("asc"))
                            orderBy += @" classificacao_hierarquica asc,";
                        else
                            orderBy += @" classificacao_hierarquica desc,";

                        break;
                    case "empresagrupo":
                        if (order.Direction.ToLower().Equals("asc"))
                            orderBy += @" empresa_grupo asc,";
                        else
                            orderBy += @" empresa_grupo desc,";

                        break;
                    case "endereco":
                        if (order.Direction.ToLower().Equals("asc"))
                            orderBy += @" endereco asc,";
                        else
                            orderBy += @" endereco desc,";

                        break;
                    case "escritorioprocesso":
                        if (order.Direction.ToLower().Equals("asc"))
                            orderBy += @" escritorio_processo asc,";
                        else
                            orderBy += @" escritorio_processo desc,";

                        break;
                    default:
                        break;
                }
            }

            return orderBy.Remove(orderBy.Length - 1, 1);
        }
    }
}
