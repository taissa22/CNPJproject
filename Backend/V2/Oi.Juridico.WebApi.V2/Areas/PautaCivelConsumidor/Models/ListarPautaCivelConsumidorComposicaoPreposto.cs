
using Oi.Juridico.WebApi.V2.Areas.PautaJuizado.DTOs;
using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;
using System.Data.Common;
using System.Data.SqlClient;

namespace Oi.Juridico.WebApi.V2.Areas.PautaJuizado.Models
{
    public class ListarPautaCivelConsumidorComposicaoPreposto
    {
        public bool PrepUFJuizado { get; set; }

        public bool PrepNaoAlocadoJuizado { get; set; }

        public string DataAudiencia { get; set; }

        public string CodParteEmpresa { get; set; }

        public string CodEstado { get; set; }

        public short CodComarca { get; set; }

        public short CodVara { get; set; }

        public short CodTipoVara { get; set; }

        public ListarPautaCivelConsumidorComposicaoPreposto()
        {

        }

        #region PREPOSTOS NÃO ALOCADOS

        public string PreparaListaPrepostoNaoAlocadoParaSql(ListarPautaCivelConsumidorComposicaoPreposto model)
        {
            var sql = @"SELECT distinct pre2.COD_PREPOSTO, pre2.NOM_PREPOSTO, pre2.MATRICULA
                        FROM jur.PREPOSTO pre2
                        WHERE not exists (SELECT distinct pre.NOM_PREPOSTO
                        FROM jur.ALOCACAO_PREPOSTO apr, jur.PREPOSTO pre ";

            sql += PreparaNaoAlocadoPorJuizadoSql(model);

            if (model.PrepNaoAlocadoJuizado)
                sql = PreparaNaoAlocadoSql(model);

            //if (model.PrepUFJuizado)
            //    sql += " AND pre2.COD_ESTADO = '" + model.CodEstado + "' AND pre2.IND_PREPOSTO_ATIVO = 'S' ";

            sql += @" AND pre2.IND_PREPOSTO_CIVEL = 'S'
                      ORDER BY pre2.NOM_PREPOSTO";

            return sql;
        }

        public string PreparaNaoAlocadoPorJuizadoSql(ListarPautaCivelConsumidorComposicaoPreposto model)
        {
            var sql = String.Format(@"WHERE apr.COD_COMARCA = {0}
                                     AND apr.COD_TIPO_VARA = {1}
                                     AND apr.COD_VARA = {2}
                                     AND apr.COD_PREPOSTO = pre.COD_PREPOSTO
                                     AND pre2.COD_PREPOSTO = pre.COD_PREPOSTO
                                     AND apr.DAT_ALOCACAO = '{3}'", model.CodComarca,
                                                                         model.CodTipoVara,
                                                                         model.CodVara,
                                                                         model.DataAudiencia);

            if (model.CodParteEmpresa != "")
            {
                sql += "AND apr.COD_PARTE_EMPRESA IN(" + PreparaEmpresaParaSqlClauseIn(model.CodParteEmpresa) + ")";
            }

            return sql;
        }

        public string PreparaNaoAlocadoSql(ListarPautaCivelConsumidorComposicaoPreposto model)
        {
            var sql = String.Format(@"SELECT distinct pre2.COD_PREPOSTO, pre2.NOM_PREPOSTO, pre2.MATRICULA
                                   FROM jur.PREPOSTO pre2
                                   WHERE NOT EXISTS 
                                   (SELECT 1 FROM jur.ALOCACAO_PREPOSTO apr
                                    WHERE apr.COD_PREPOSTO = pre2.COD_PREPOSTO
                                      AND apr.DAT_ALOCACAO = '{0}'", model.DataAudiencia);

            if (model.CodParteEmpresa != "")
            {
                sql += "AND apr.COD_PARTE_EMPRESA IN(" + PreparaEmpresaParaSqlClauseIn(model.CodParteEmpresa) + "))";
            }
            else
            {
                sql += ")";
            }

            return sql;
        }

        public List<ListarPrepostoNaoAlocadoResponse> DadosListaNaoAlocadoPreposto(DbDataReader reader, ListarPautaCivelConsumidorComposicaoPreposto model)
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

        public string PreparaListaPrepostoAlocadoParaSql(ListarPautaCivelConsumidorComposicaoPreposto model)
        {
            var sql = @"SELECT distinct pre.COD_PREPOSTO, pre.NOM_PREPOSTO, pre.MATRICULA, apr.IND_PRINCIPAL
                        FROM jur.ALOCACAO_PREPOSTO APR, jur.PREPOSTO pre ";

            sql += PreparaAlocadoPorJuizadoSql(model);


            sql += " ORDER BY NOM_PREPOSTO";

            return sql;
        }

        public string PreparaAlocadoPorJuizadoSql(ListarPautaCivelConsumidorComposicaoPreposto model)
        {
            var sql = String.Format(@"WHERE apr.COD_COMARCA = {0}
                                     AND apr.COD_TIPO_VARA = {1}
                                     AND apr.COD_VARA = {2}
                                     AND apr.COD_PREPOSTO = pre.COD_PREPOSTO
                                     AND apr.DAT_ALOCACAO = '{3}'", model.CodComarca,
                                                                                           model.CodTipoVara,
                                                                                           model.CodVara,
                                                                                           model.DataAudiencia);
            if (model.CodParteEmpresa != "")
            {
                sql += "AND apr.COD_PARTE_EMPRESA IN(" + PreparaEmpresaParaSqlClauseIn(model.CodParteEmpresa) + ")";
            }
            return sql;
        }

        public List<ListarPrepostoAlocadoResponse> DadosListaAlocadoPreposto(DbDataReader reader, ListarPautaCivelConsumidorComposicaoPreposto model)
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
