
using Oi.Juridico.WebApi.V2.Areas.PautaJuizado.DTOs;
using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;
using System.Data.Common;
using System.Data.SqlClient;

namespace Oi.Juridico.WebApi.V2.Areas.PautaJuizado.Models
{
    public class ListarPautaProconComposicaoPreposto
    {
        public bool PrepUFJuizado { get; set; }

        public bool PrepNaoAlocadoJuizado { get; set; }

        public string DataAudiencia { get; set; }

        public string CodParteEmpresa { get; set; }

        public string CodEstado { get; set; }

        public short CodComarca { get; set; }

        public short CodVara { get; set; }

        public short CodTipoVara { get; set; }

        public int CodGrupoProcon { get; set; }

        public string PorProcon { get; set; }

        public ListarPautaProconComposicaoPreposto()
        {

        }

        #region PREPOSTOS NÃO ALOCADOS

        public string PreparaListaPrepostoNaoAlocadoParaSql(ListarPautaProconComposicaoPreposto model)
        {
            var sql = @"SELECT distinct pre2.COD_PREPOSTO, pre2.NOM_PREPOSTO, pre2.MATRICULA
                        FROM jur.PREPOSTO pre2
                        WHERE not exists (SELECT distinct pre.NOM_PREPOSTO
                        FROM jur.ALOCACAO_PREPOSTO apr, jur.PREPOSTO pre ";

            if (model.PorProcon == "S")
                sql += PreparaNaoAlocadoPorJuizadoSql(model);
            else
                sql += PreparaNaoAlocadoPorGrupoSql(model);

            if (model.PrepNaoAlocadoJuizado)
                sql = PreparaNaoAlocadoSql(model);

            //if (model.PrepUFJuizado)
            //    sql += " AND pre2.COD_ESTADO = '" + model.CodEstado + "' AND pre2.IND_PREPOSTO_ATIVO = 'S' ";

            sql += @" AND pre2.IND_PROCON = 'S'
                      ORDER BY pre2.NOM_PREPOSTO";

            return sql;
        }

        public string PreparaNaoAlocadoPorJuizadoSql(ListarPautaProconComposicaoPreposto model)
        {
            return String.Format(@"WHERE apr.COD_COMARCA = {0}
                                     AND apr.COD_TIPO_VARA = {1}
                                     AND apr.COD_VARA = {2}
                                     AND apr.COD_PREPOSTO = pre.COD_PREPOSTO
                                     AND pre2.COD_PREPOSTO = pre.COD_PREPOSTO
                                     AND apr.DAT_ALOCACAO = TO_DATE('{3}','DD/MM/YYYY')
                                     AND apr.COD_PARTE_EMPRESA IN ({4} ) )", model.CodComarca,
                                                                             model.CodTipoVara,
                                                                             model.CodVara,
                                                                             model.DataAudiencia,
                                                                             PreparaEmpresaParaSqlClauseIn(model.CodParteEmpresa));
        }

        public string PreparaNaoAlocadoSql(ListarPautaProconComposicaoPreposto model)
        {
            return String.Format(@"SELECT distinct pre2.COD_PREPOSTO, pre2.NOM_PREPOSTO, pre2.MATRICULA
                                   FROM jur.PREPOSTO pre2
                                   WHERE NOT EXISTS 
                                   (SELECT 1 FROM jur.ALOCACAO_PREPOSTO apr
                                    WHERE apr.COD_PREPOSTO = pre2.COD_PREPOSTO
                                      AND apr.COD_PARTE_EMPRESA IN ({1})
                                      AND apr.DAT_ALOCACAO = TO_DATE('{0}','DD/MM/YYYY') )", model.DataAudiencia,
                                                                       PreparaEmpresaParaSqlClauseIn(model.CodParteEmpresa));
        }

        public List<ListarPrepostoNaoAlocadoResponse> DadosListaNaoAlocadoPreposto(DbDataReader reader, ListarPautaProconComposicaoPreposto model)
        {
            List<ListarPrepostoNaoAlocadoResponse> lstPreposto = new List<ListarPrepostoNaoAlocadoResponse>();

            while (reader.Read())
            {
                ListarPrepostoNaoAlocadoResponse preposto = new ListarPrepostoNaoAlocadoResponse();

                preposto.Id = Convert.ToInt32(reader["COD_PREPOSTO"]);
                preposto.Preposto = reader["NOM_PREPOSTO"].ToString().ToUpper() + " (Matr.: " + (reader["MATRICULA"].ToString() == null ? "" : reader["MATRICULA"].ToString()) + ")";

                lstPreposto.Add(preposto);
            }

            return lstPreposto;
        }

        #endregion

        #region PREPOSTOS ALOCADOS

        public string PreparaListaPrepostoAlocadoParaSql(ListarPautaProconComposicaoPreposto model)
        {
            var sql = @"SELECT distinct pre.COD_PREPOSTO, pre.NOM_PREPOSTO, pre.MATRICULA, apr.IND_PRINCIPAL
                        FROM jur.ALOCACAO_PREPOSTO APR, jur.PREPOSTO pre ";

            if (model.PorProcon == "S")
                sql += PreparaAlocadoPorJuizadoSql(model);
            else
                sql += PreparaAlocadoPorGrupoSql(model);


            sql += " ORDER BY NOM_PREPOSTO";

            return sql;
        }

        public string PreparaAlocadoPorJuizadoSql(ListarPautaProconComposicaoPreposto model)
        {
            return String.Format(@"WHERE apr.COD_COMARCA = {0}
                                     AND apr.COD_TIPO_VARA = {1}
                                     AND apr.COD_VARA = {2}
                                     AND apr.COD_PREPOSTO = pre.COD_PREPOSTO
                                     AND apr.DAT_ALOCACAO = TO_DATE('{3}','DD/MM/YYYY')
                                     AND apr.COD_PARTE_EMPRESA IN ({4}) ", model.CodComarca,
                                                                               model.CodTipoVara,
                                                                               model.CodVara,
                                                                               model.DataAudiencia,
                                                                               PreparaEmpresaParaSqlClauseIn(model.CodParteEmpresa));
        }

        public List<ListarPrepostoAlocadoResponse> DadosListaAlocadoPreposto(DbDataReader reader, ListarPautaProconComposicaoPreposto model)
        {
            List<ListarPrepostoAlocadoResponse> lstPreposto = new List<ListarPrepostoAlocadoResponse>();

            while (reader.Read())
            {
                ListarPrepostoAlocadoResponse preposto = new ListarPrepostoAlocadoResponse();

                preposto.Id = Convert.ToInt32(reader["COD_PREPOSTO"]);
                preposto.Preposto = reader["NOM_PREPOSTO"].ToString().ToUpper() + " (Matr.: " + (reader["MATRICULA"].ToString() == null ? "" : reader["MATRICULA"].ToString()) + ")";
                preposto.Principal = reader["IND_PRINCIPAL"].ToString() == "S";

                var c = reader["IND_PRINCIPAL"].ToString();

                lstPreposto.Add(preposto);
            }

            return lstPreposto;
        }

        public string PreparaNaoAlocadoPorGrupoSql(ListarPautaProconComposicaoPreposto model)
        {
            return String.Format(@", jur.GRUPO_JUIZADO_VARA gjv
                                   WHERE apr.COD_COMARCA=gjv.COD_COMARCA
                                     AND apr.COD_TIPO_VARA = gjv.COD_TIPO_VARA 
                                     AND apr.COD_VARA = gjv.COD_VARA 
                                     AND gjv.COD_GRUPO_JUIZADO = {0}
                                     AND apr.COD_PREPOSTO = pre.COD_PREPOSTO    
                                     AND pre2.COD_PREPOSTO = pre.COD_PREPOSTO   
                                     AND apr.DAT_ALOCACAO = TO_DATE('{1}','DD/MM/YYYY') )  
                                     AND apr.COD_PARTE_EMPRESA IN ({2}) ", model.CodGrupoProcon,
                                                                           model.DataAudiencia,
                                                                           PreparaEmpresaParaSqlClauseIn(model.CodParteEmpresa));
        }

        public string PreparaAlocadoPorGrupoSql(ListarPautaProconComposicaoPreposto model)
        {
            return String.Format(@", jur.GRUPO_JUIZADO_VARA gjv
                                   WHERE apr.COD_COMARCA = gjv.COD_COMARCA
                                     AND apr.COD_TIPO_VARA = gjv.COD_TIPO_VARA 
                                     AND apr.COD_VARA = gjv.COD_VARA 
                                     AND gjv.COD_GRUPO_JUIZADO = {0}
                                     AND apr.COD_PREPOSTO = pre.COD_PREPOSTO    
                                     AND apr.DAT_ALOCACAO = TO_DATE('{1}','DD/MM/YYYY')
                                     AND apr.COD_PARTE_EMPRESA IN ({2}) ", model.CodGrupoProcon,
                                                                             model.DataAudiencia,
                                                                             PreparaEmpresaParaSqlClauseIn(model.CodParteEmpresa));
        }

        #endregion

        public string PreparaEmpresaParaSqlClauseIn(string codParteEmpresa)
        {
            var empresas = codParteEmpresa.Split(",");

            string strSql = String.Empty;

            for (int i = 0; i < empresas.Length; i++)
            {
                strSql = strSql + empresas[i] + ",";
            }

            return strSql.Substring(0, strSql.Length - 1);
        }
    }
}
