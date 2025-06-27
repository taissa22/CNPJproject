
using Oi.Juridico.WebApi.V2.Areas.PautaJuizado.DTOs;
using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;
using System.Data.Common;
using System.Data.SqlClient;

namespace Oi.Juridico.WebApi.V2.Areas.PautaJuizado.Models
{
    public class ListarPautaComposicao
    {
        public int NumeroPagina { get; set; }

        public int QuantidadePorPagina { get; set; }

        public string PorJuizado { get; set; }

        public string PeriodoInicio { get; set; }

        public string PeriodoFim { get; set; }

        public string SituacaoProcesso { get; set; }

        public string? CodEstado { get; set; }

        public short? CodTipoAudiencia { get; set; }

        public short CodComarca { get; set; }

        public short CodVara { get; set; }

        public short CodTipoVara { get; set; }

        public short CodStatusAudiencia { get; set; }

        public int CodEmpresaGrupo { get; set; }

        public short CodEmpresaCentralizadora { get; set; }

        public string? AudienciaSemPreposto { get; set; }

        public short? CodGrupoJuizado { get; set; }

        public string PrepostoAlocado { get; set; }

        public int AlocacaoTipo { get; set; }

        public ListarPautaComposicao()
        {

        }

        public string PreparaListaPautaParaSql(ListarPautaComposicao model, string usuario)
        {
            var sql = String.Empty;

            if (model.PorJuizado == "S")
                sql = PreparaListaPautaSomenteJuizadoParaSql(model, usuario);
            else
                sql = PreparaListaPautaSomenteGrupoParaSql(model, usuario);


            if (model.CodTipoAudiencia > 0)
                sql = sql + " AND AP.COD_TIPO_AUD = " + model.CodTipoAudiencia;

            if (model.AudienciaSemPreposto == "S" || model.AudienciaSemPreposto == "N")
            {
                var sqlAudienciaSemPreposto = @"SELECT distinct APR.COD_PREPOSTO                                          
                                                FROM jur.ALOCACAO_PREPOSTO APR
                                                WHERE apr.COD_COMARCA = p.COD_COMARCA
                                                AND rownum <= 1
                                                AND apr.COD_PARTE_EMPRESA=p.COD_PARTE_EMPRESA
                                                AND apr.COD_TIPO_VARA = p.COD_TIPO_VARA
                                                AND apr.COD_VARA = p.COD_VARA
                                                AND apr.DAT_ALOCACAO = AP.DAT_AUDIENCIA";

                if (model.AudienciaSemPreposto == "S")
                    sql += " AND not exists (" + sqlAudienciaSemPreposto + ")";
                else
                    sql += " AND exists (" + sqlAudienciaSemPreposto + ")";
            }

            if (model.CodStatusAudiencia > 0)
                sql += " AND ap.cod_status_audiencia =  " + model.CodStatusAudiencia;

            if (model.CodEmpresaGrupo > 0)
                sql += " AND pa.COD_PARTE = " + model.CodEmpresaGrupo;

            if (model.CodEmpresaCentralizadora > 0)
                sql += " AND pa.EMPCE_COD_EMP_CENTRALIZADORA = " + model.CodEmpresaCentralizadora;

            if (model.PrepostoAlocado != "0")
            {
                sql += String.Format(@" AND exists(SELECT 1 
                                                   FROM jur.ALOCACAO_PREPOSTO APR, jur.PREPOSTO pre 
                                                   WHERE apr.COD_COMARCA = c.COD_COMARCA
                                                     AND rownum <= 1
                                                     AND apr.COD_TIPO_VARA = tv.COD_TIPO_VARA
                                                     AND apr.COD_VARA = p.COD_VARA
                                                     AND apr.COD_PREPOSTO = pre.COD_PREPOSTO
                                                     AND apr.DAT_ALOCACAO = ap.DAT_AUDIENCIA
                                                     AND apr.COD_PARTE_EMPRESA = pa.COD_PARTE
                                                     AND pre.COD_PREPOSTO IN ({0}))", model.PrepostoAlocado);
            }

            sql += " GROUP BY DAT_AUDIENCIA, p.COD_COMARCA, p.COD_TIPO_VARA, p.COD_VARA, c.COD_ESTADO, C.NOM_COMARCA, tv.NOM_TIPO_VARA" +
                   " ORDER BY DAT_AUDIENCIA, c.COD_ESTADO, c.NOM_COMARCA, p.COD_VARA, tv.NOM_TIPO_VARA";

            return sql;
        }

        public string PreparaListaPautaSomenteJuizadoParaSql(ListarPautaComposicao model, string usuario)
        {
            var sql = String.Format(@"SELECT TO_CHAR(ap.DAT_AUDIENCIA, 'dd/mm/yyyy') as DAT_AUDIENCIA, p.COD_COMARCA, p.COD_TIPO_VARA, p.COD_VARA, c.COD_ESTADO, C.NOM_COMARCA, tv.NOM_TIPO_VARA
                         FROM jur.audiencia_processo ap, jur.processo p, jur.comarca c, jur.tipo_vara tv, jur.TIPO_AUDIENCIA tau, JUR.ASSUNTO ASS, JUR.PARTE PA
                         WHERE p.COD_COMARCA = c.COD_COMARCA
                            AND p.COD_TIPO_VARA = tv.COD_TIPO_VARA
                            AND ap.COD_PROCESSO = p.COD_PROCESSO
                            AND p.COD_PARTE_EMPRESA = pa.COD_PARTE
                            AND p.COD_TIPO_PROCESSO = {4}
                            AND AP.COD_TIPO_AUD = TAU.COD_TIPO_AUD
                            AND P.COD_ASSUNTO = ASS.COD_ASSUNTO
                            AND exists(SELECT 1
                                       FROM jur.USUARIO_REGIONAL ur, jur.PARTE pa
                                       WHERE ur.cod_tipo_processo = {4}
                                           AND rownum <= 1
                                           AND ur.cod_tipo_processo = p.cod_tipo_processo
                                           AND pa.cod_parte = p.cod_parte_empresa
                                           AND ur.cod_regional = pa.cod_regional
                                           AND pa.cod_tipo_parte = 'E'
                                           AND ur.cod_usuario = '{3}')
                            AND AP.DAT_AUDIENCIA >= '{0}'
                            AND AP.DAT_AUDIENCIA <= '{1}'
                            AND P.IND_PROCESSO_ATIVO = '{2}'", model.PeriodoInicio, model.PeriodoFim, model.SituacaoProcesso, usuario, (short)TipoProcessoEnum.JuizadoEspecial);

            if (!String.IsNullOrEmpty(model.CodEstado))
                sql = sql + " AND c.COD_ESTADO = '" + model.CodEstado + "' ";

            if (model.CodComarca > 0)
                sql = sql + " AND c.COD_COMARCA = '" + model.CodComarca + "' ";

            if (model.CodVara > 0)
                sql = sql + " AND p.COD_VARA = " + model.CodVara + " AND p.COD_TIPO_VARA = " + model.CodTipoVara;

            return sql;
        }

        public string PreparaListaPautaSomenteGrupoParaSql(ListarPautaComposicao model, string usuario)
        {
            var sql = String.Format(@"SELECT TO_CHAR(ap.DAT_AUDIENCIA, 'dd/mm/yyyy') as DAT_AUDIENCIA, p.COD_COMARCA, p.COD_TIPO_VARA, p.COD_VARA, c.COD_ESTADO, C.NOM_COMARCA, tv.NOM_TIPO_VARA
                                      FROM jur.audiencia_processo ap, jur.processo p, jur.comarca c, jur.tipo_vara tv, jur.TIPO_AUDIENCIA tau, JUR.ASSUNTO ASS, JUR.PARTE PA, jur.GRUPO_JUIZADO_VARA gjv, jur.GRUPO_JUIZADO grp
                                      WHERE p.COD_COMARCA = c.COD_COMARCA
                                      AND p.COD_COMARCA = gjv.COD_COMARCA     
                                      AND p.COD_TIPO_VARA = gjv.COD_TIPO_VARA 
                                      AND p.COD_VARA = gjv.COD_VARA  
                                      AND p.COD_PARTE_EMPRESA = pa.COD_PARTE
                                      AND grp.COD_GRUPO_JUIZADO = gjv.COD_GRUPO_JUIZADO
                                      AND p.COD_TIPO_VARA = tv.COD_TIPO_VARA  
                                      AND ap.COD_PROCESSO = p.COD_PROCESSO    
                                      AND p.COD_TIPO_PROCESSO = {4}   
                                      AND AP.COD_TIPO_AUD = TAU.COD_TIPO_AUD 
                                      AND P.COD_ASSUNTO = ASS.COD_ASSUNTO
                                      AND exists(SELECT 1
                                                   FROM jur.USUARIO_REGIONAL ur, jur.PARTE pa
                                                   WHERE ur.cod_tipo_processo = {4}
                                                       AND rownum <= 1
                                                       AND ur.cod_tipo_processo = p.cod_tipo_processo
                                                       AND pa.cod_parte = p.cod_parte_empresa
                                                       AND ur.cod_regional = pa.cod_regional
                                                       AND pa.cod_tipo_parte = 'E'
                                                       AND ur.cod_usuario = '{3}')
                                      AND AP.DAT_AUDIENCIA >= '{0}'
                                      AND AP.DAT_AUDIENCIA <= '{1}'
                                      AND P.IND_PROCESSO_ATIVO = '{2}'", model.PeriodoInicio, 
                                                                         model.PeriodoFim, 
                                                                         model.SituacaoProcesso, 
                                                                         usuario, 
                                                                         (short)TipoProcessoEnum.JuizadoEspecial);

            if (model.CodGrupoJuizado > 0)
                sql += " AND gjv.COD_GRUPO_JUIZADO = " + model.CodGrupoJuizado;

            return sql;
        }

        public List<ListarPautaComposicaoResponse> DadosListaPauta(DbDataReader reader, ListarPautaComposicao model)
        {
            List<ListarPautaComposicaoResponse> lstPauta = new List<ListarPautaComposicaoResponse>();

            while (reader.Read())
            {
                ListarPautaComposicaoResponse pauta = new ListarPautaComposicaoResponse();

                pauta.Data = reader["DAT_AUDIENCIA"].ToString();
                pauta.CodComarca = Convert.ToInt16(reader["COD_COMARCA"]);
                pauta.CodVara = Convert.ToInt16(reader["COD_VARA"]);
                pauta.CodTipoVara = Convert.ToInt16(reader["COD_TIPO_VARA"]);
                pauta.CodEstado = reader["COD_ESTADO"].ToString();

                if (model.PorJuizado == "S")
                    pauta.Juizado = pauta.CodEstado + " - " + reader["NOM_COMARCA"].ToString() + " - " + pauta.CodVara + "º JUIZADO " + reader["NOM_TIPO_VARA"].ToString();
                else
                    pauta.Juizado = reader["NOM_COMARCA"].ToString();

                lstPauta.Add(pauta);
            }

            return lstPauta;
        }
    }
}
