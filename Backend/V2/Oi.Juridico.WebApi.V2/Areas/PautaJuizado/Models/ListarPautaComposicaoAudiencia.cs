
using Oi.Juridico.WebApi.V2.Areas.PautaJuizado.DTOs;
using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;
using System.Data.Common;
using System.Data.SqlClient;

namespace Oi.Juridico.WebApi.V2.Areas.PautaJuizado.Models
{
    public class ListarPautaComposicaoAudiencia
    {
        public string DataAudiencia { get; set; }

        public short CodComarca { get; set; }

        public short CodVara { get; set; }

        public short CodTipoVara { get; set; }

        public short CodStatusAudiencia { get; set; }

        public string SituacaoProcesso { get; set; }

        public string Coluna { get; set; }

        public string Direcao { get; set; }


        public ListarPautaComposicaoAudiencia()
        {

        }

        public string PreparaListaAudienciaParaSql(ListarPautaComposicaoAudiencia model, string usuario)
        {
            var sql = String.Format(@"SELECT TO_CHAR(AP.HOR_AUDIENCIA,'hh24:mi') as HOR_AUDIENCIA, TAU.SGL_TIPO_AUDIENCIA, P.NRO_PROCESSO_CARTORIO || '<br>' || PA.NOM_PARTE AS NRO_PROCESSO_CARTORIO_EMPRESA,
                                      ASS.DSC_ASSUNTO, p.VAL_CAUSA as VALOR, ap.IND_TERCEIRIZADO, p.COD_COMARCA, p.COD_TIPO_VARA, p.COD_VARA, c.COD_ESTADO, c.NOM_COMARCA, tv.NOM_TIPO_VARA, P.NRO_PROCESSO_CARTORIO,
                                      pa.COD_PARTE, pa.NOM_PARTE, p.COD_PROCESSO, ap.SEQ_AUDIENCIA,ap.COD_ALOCACAO_PREPOSTO as AlocacaoTipo
                                      FROM jur.audiencia_processo ap, jur.processo p, jur.comarca c, jur.tipo_vara tv, jur.TIPO_AUDIENCIA tau, JUR.ASSUNTO ASS, JUR.PARTE PA
                                      WHERE ap.COD_PROCESSO = p.COD_PROCESSO  
                                        AND AP.COD_TIPO_AUD = TAU.COD_TIPO_AUD
                                        AND p.COD_TIPO_VARA = tv.COD_TIPO_VARA 
                                        AND p.COD_COMARCA= c.COD_COMARCA
                                        AND p.COD_PARTE_EMPRESA = pa.COD_PARTE
                                        AND P.COD_ASSUNTO = ASS.COD_ASSUNTO 
                                        AND p.COD_TIPO_PROCESSO = {0}
                                        AND P.IND_PROCESSO_ATIVO = '{1}' 
                                        {2} 
                                        AND AP.DAT_AUDIENCIA = '{3}'
                                        AND C.COD_COMARCA = {4} 
                                        AND P.COD_VARA = {5} 
                                        AND P.COD_TIPO_VARA = {6}
                                        AND exists (SELECT 1
                                                    FROM jur.USUARIO_REGIONAL ur, jur.PARTE pa
                                                    WHERE ur.cod_tipo_processo = {0}
                                                    AND rownum <= 1
                                                    AND ur.cod_tipo_processo = p.cod_tipo_processo
                                                    AND pa.cod_parte = p.cod_parte_empresa
                                                    AND ur.cod_regional = pa.cod_regional
                                                    AND pa.cod_tipo_parte = 'E'
                                                    AND ur.cod_usuario = '{7}')
                                     ORDER BY ap.DAT_AUDIENCIA", (short)TipoProcessoEnum.JuizadoEspecial, 
                                                                  model.SituacaoProcesso, 
                                                                  model.CodStatusAudiencia > 0 ? " AND ap.cod_status_audiencia = " + model.CodStatusAudiencia.ToString() : " ",
                                                                  model.DataAudiencia,
                                                                  model.CodComarca,
                                                                  model.CodVara,
                                                                  model.CodTipoVara,
                                                                  usuario);

            return sql;
        }

        public List<ListarPautaComposicaoAudienciaResponse> DadosListaAudiencia(DbDataReader reader, ListarPautaComposicaoAudiencia model)
        {
            List<ListarPautaComposicaoAudienciaResponse> lstAudiencia = new List<ListarPautaComposicaoAudienciaResponse>();

            while (reader.Read())
            {
                ListarPautaComposicaoAudienciaResponse audiencia = new ListarPautaComposicaoAudienciaResponse();

                audiencia.Hora = reader["HOR_AUDIENCIA"].ToString();
                audiencia.Tipo = reader["SGL_TIPO_AUDIENCIA"].ToString();
                audiencia.ProcessoEmpresaGrupo = reader["NRO_PROCESSO_CARTORIO_EMPRESA"].ToString();
                audiencia.Assunto = reader["DSC_ASSUNTO"].ToString();
                audiencia.ValorCausa = reader["VALOR"].ToString();    
                audiencia.Terceirizado = reader["IND_TERCEIRIZADO"].ToString();
                audiencia.CodEstado = reader["COD_ESTADO"].ToString();
                audiencia.NomComarca = reader["NOM_COMARCA"].ToString();
                audiencia.CodVara = Convert.ToInt16(reader["COD_VARA"]);
                audiencia.NomTipoVara = reader["NOM_TIPO_VARA"].ToString(); 
                audiencia.NroProcessoCartorio = reader["NRO_PROCESSO_CARTORIO"].ToString();
                audiencia.NomParte = reader["NOM_PARTE"].ToString();
                audiencia.CodProcesso = Convert.ToInt32(reader["COD_PROCESSO"]); 
                audiencia.CodParte = Convert.ToInt32(reader["COD_PARTE"]);
                audiencia.Juizado = audiencia.CodEstado + " - " + audiencia.NomComarca + " - " + audiencia.CodVara + "º JUIZADO " + audiencia.NomTipoVara;
                audiencia.SeqAudiencia = Convert.ToInt32(reader["SEQ_AUDIENCIA"]);

                if ((reader["IND_TERCEIRIZADO"].ToString() == "S") && ((reader["AlocacaoTipo"] == null) || (reader["AlocacaoTipo"] == "")))
                {
                    audiencia.AlocacaoTipo = 3;
                }
                else
                {
                    if ((reader["AlocacaoTipo"] != null) && (reader["AlocacaoTipo"].ToString() != ""))
                    {

                        audiencia.AlocacaoTipo = Convert.ToInt32(reader["AlocacaoTipo"]);
                    }
                }
                lstAudiencia.Add(audiencia);
            }

            return lstAudiencia;
        }

    }
}
