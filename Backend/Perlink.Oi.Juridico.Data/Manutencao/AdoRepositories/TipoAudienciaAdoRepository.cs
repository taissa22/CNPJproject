using Oracle.ManagedDataAccess.Client;
using Perlink.Oi.Juridico.Data.Compartilhado.AdoBaseRepository;
using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.Manutencao.DTO;
using Perlink.Oi.Juridico.Domain.Manutencao.Interfaces.AdoRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Data.Manutencao.AdoRepositories
{
    public class TipoAudienciaAdoRepository : AdoBaseRepository, ITipoAudienciaAdoRepository
    {
        public int GetTotalCount(long? codTipoProcesso, string descricao)
        {
            OracleDataReader dataReader = null;

            try
            {
                var sql = new StringBuilder();
                var paramList = new List<OracleParameter>();

                sql.Append("SELECT Count(*) FROM JUR.TIPO_AUDIENCIA ta ");

                #region "Filters"

                if (codTipoProcesso.HasValue)
                {
                    sql.Append($@"where ta.ind_civel = (case when {codTipoProcesso.Value} = 1 then 'S' else 'N' end)
                                       and ta.ind_civel_estrategico = (case when {codTipoProcesso.Value} = 9 then 'S' else 'N' end)
                                       and ta.ind_trabalhista = (case when {codTipoProcesso.Value} = 2 then 'S' else 'N' end)
                                       and ta.ind_trabalhista_adm = (case when {codTipoProcesso.Value} = 6 then 'S' else 'N' end)
                                       and ta.ind_tributaria_adm = (case when {codTipoProcesso.Value} = 4 then 'S' else 'N' end)
                                       and ta.ind_tributaria_jud = (case when {codTipoProcesso.Value} = 5 then 'S' else 'N' end)
                                       and ta.ind_juizado = (case when {codTipoProcesso.Value} = 7 then 'S' else 'N' end)
                                       and ta.ind_administrativo = (case when {codTipoProcesso.Value} = 3 then 'S' else 'N' end)
                                       and ta.ind_civel_adm = (case when {codTipoProcesso.Value} = 12 then 'S' else 'N' end)
                                       and ta.ind_criminal_judicial = (case when {codTipoProcesso.Value} = 15 then 'S' else 'N' end)
                                       and ta.ind_criminal_adm = (case when {codTipoProcesso.Value} = 14 then 'S' else 'N' end)
                                       and ta.ind_procon = (case when {codTipoProcesso.Value} = 17 then 'S' else 'N' end)
                                       and ta.ind_pex = (case when {codTipoProcesso.Value} = 18 then 'S' else 'N' end) ");

                }

                if (!string.IsNullOrEmpty(descricao) && codTipoProcesso.HasValue)
                {
                    sql.Append($"and ta.dsc_tipo_audiencia like '%{descricao}%' ");
                }
                else if (!string.IsNullOrEmpty(descricao) && !codTipoProcesso.HasValue)
                {
                    sql.Append($"where ta.dsc_tipo_audiencia like '%{descricao}%' ");
                }

                #endregion

                AbrirConexao();
                dataReader = ExecuteReader(sql.ToString(), paramList.ToArray());

                while (dataReader.Read())
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

        public IEnumerable<FiltroTipoAudienciaResultadoDTO> ObterTodosPorFiltro(long? codTipoProcesso, string descricao, int pageNumber, int pageSize, List<SortOrder> orders, bool IsExportMethod)
        {
            var list = new List<FiltroTipoAudienciaResultadoDTO>();
            var paramList = new List<OracleParameter>();
            OracleDataReader dataReader = null;

            try
            {
                var sql = new StringBuilder();

                if (!IsExportMethod)
                {
                    sql.Append(@"SELECT cod_tipo_audiencia, descricao, sigla, ativo, tipoProcesso FROM (
                                    SELECT a.cod_tipo_audiencia, a.descricao, a.sigla, a.ativo,
                                           a.tipoProcesso, rownum r__ FROM ( ");                
                }

                sql.Append(@"SELECT ta.cod_tipo_aud as cod_tipo_audiencia,
                                    ta.dsc_tipo_audiencia as descricao,
                                    ta.sgl_tipo_audiencia as sigla,
                                    ta.ind_ativo as ativo,
                                    (CASE
                                        WHEN ta.ind_civel = 'S' THEN 'Cível Consumidor'
                                        WHEN ta.ind_civel_estrategico = 'S' THEN 'Cível Estratégico'
                                        WHEN ta.ind_trabalhista = 'S' THEN 'Trabalhista'
                                        WHEN ta.ind_trabalhista_adm = 'S' THEN 'Trabalhista Administrativo'
                                        WHEN ta.ind_tributaria_adm = 'S' THEN 'Tributário Administrativo'
                                        WHEN ta.ind_tributaria_jud = 'S' THEN 'Tributário Judicial'
                                        WHEN ta.ind_juizado = 'S' THEN 'Juizado Especial'
                                        WHEN ta.ind_administrativo = 'S' THEN 'Administrativo'
                                        WHEN ta.ind_civel_adm = 'S' THEN 'Cível Administrativo'
                                        WHEN ta.ind_criminal_judicial = 'S' THEN 'Criminal Judicial'
                                        WHEN ta.ind_criminal_adm = 'S' THEN 'Criminal Administrativo'
                                        WHEN ta.ind_procon = 'S' THEN 'Procon'
                                        WHEN ta.ind_pex = 'S' THEN 'Pex'
                                        ELSE 'Desconhecido'
                                        END) as tipoProcesso
                                FROM JUR.TIPO_AUDIENCIA ta ");

                #region "Filters"

                if (codTipoProcesso.HasValue) 
                {
                    sql.Append($@"where ta.ind_civel = (case when {codTipoProcesso.Value} = 1 then 'S' else 'N' end)
                                       and ta.ind_civel_estrategico = (case when {codTipoProcesso.Value} = 9 then 'S' else 'N' end)
                                       and ta.ind_trabalhista = (case when {codTipoProcesso.Value} = 2 then 'S' else 'N' end)
                                       and ta.ind_trabalhista_adm = (case when {codTipoProcesso.Value} = 6 then 'S' else 'N' end)
                                       and ta.ind_tributaria_adm = (case when {codTipoProcesso.Value} = 4 then 'S' else 'N' end)
                                       and ta.ind_tributaria_jud = (case when {codTipoProcesso.Value} = 5 then 'S' else 'N' end)
                                       and ta.ind_juizado = (case when {codTipoProcesso.Value} = 7 then 'S' else 'N' end)
                                       and ta.ind_administrativo = (case when {codTipoProcesso.Value} = 3 then 'S' else 'N' end)
                                       and ta.ind_civel_adm = (case when {codTipoProcesso.Value} = 12 then 'S' else 'N' end)
                                       and ta.ind_criminal_judicial = (case when {codTipoProcesso.Value} = 15 then 'S' else 'N' end)
                                       and ta.ind_criminal_adm = (case when {codTipoProcesso.Value} = 14 then 'S' else 'N' end)
                                       and ta.ind_procon = (case when {codTipoProcesso.Value} = 17 then 'S' else 'N' end)
                                       and ta.ind_pex = (case when {codTipoProcesso.Value} = 18 then 'S' else 'N' end) ");
                    
                } 

                if (!string.IsNullOrEmpty(descricao) && codTipoProcesso.HasValue)
                {
                    sql.Append($"and LOWER(ta.dsc_tipo_audiencia) like '%{descricao.ToLower()}%' ");
                }
                else if (!string.IsNullOrEmpty(descricao) && !codTipoProcesso.HasValue)
                {
                    sql.Append($"where LOWER(ta.dsc_tipo_audiencia) like '%{descricao.ToLower()}%' ");
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
                    var obj = new FiltroTipoAudienciaResultadoDTO();
                    obj.CodTipoAudiencia = dataReader.GetInt64(0);
                    obj.Descricao = dataReader.IsDBNull(1) ? string.Empty : dataReader.GetString(1).Trim();
                    obj.Sigla = dataReader.IsDBNull(2) ? string.Empty : dataReader.GetString(2).Trim();
                    obj.EstaAtivo = dataReader.IsDBNull(3) ? false : dataReader.GetString(3) == "S" ? true : false;
                    obj.TipoProcesso = dataReader.IsDBNull(4) ? string.Empty : dataReader.GetString(4).Trim();

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

        private string GetOrderBy(IList<SortOrder> orders)
        {
            var orderBy = string.Empty;

            foreach (var order in orders)
            {
                switch (order.Property.ToLower())
                {
                    case "codtipoaudiencia":
                        if (order.Direction.ToLower().Equals("asc"))
                            orderBy += @" cod_tipo_audiencia asc,";
                        else
                            orderBy += @" cod_tipo_audiencia desc,";

                        break;
                    case "sigla":
                        if (order.Direction.ToLower().Equals("asc"))
                            orderBy += @" sigla asc,";
                        else
                            orderBy += @" sigla desc,";

                        break;
                    case "descricao":
                        if (order.Direction.ToLower().Equals("asc"))
                            orderBy += @" descricao asc,";
                        else
                            orderBy += @" descricao desc,";

                        break;
                    case "tipodeprocesso":
                        if (order.Direction.ToLower().Equals("asc"))
                            orderBy += @" tipoProcesso asc,";
                        else
                            orderBy += @" tipoProcesso desc,";

                        break;
                    case "ativo":
                        if (order.Direction.ToLower().Equals("asc"))
                            orderBy += @" ativo asc,";
                        else
                            orderBy += @" ativo desc,";

                        break;
                    default:
                        break;
                }
            }

            return orderBy.Remove(orderBy.Length - 1, 1);
        }
    }
}
