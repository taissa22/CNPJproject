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
    public class TipoParticipacaoAdoRepository : AdoBaseRepository, ITipoParticipacaoAdoRepository
    {
        public int GetTotalCount(string descricao)
        {
            OracleDataReader dataReader = null;

            try
            {
                var sql = new StringBuilder();
                var paramList = new List<OracleParameter>();

                sql.Append("SELECT Count(*) FROM JUR.tipo_participacao t ");

                #region "Filters"
                if (!string.IsNullOrEmpty(descricao))
                {
                    sql.Append($"where LOWER(t.dsc_tipo_participacao) like '%{descricao.ToLower()}%' ");
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

        public IEnumerable<FiltroTipoParticipacaoResultadoDTO> ObterTodosPorFiltro(string descricao, int pageNumber, int pageSize, List<SortOrder> orders, bool IsExportMethod)
        {
            var list = new List<FiltroTipoParticipacaoResultadoDTO>();
            var paramList = new List<OracleParameter>();
            OracleDataReader dataReader = null;

            try
            {
                var sql = new StringBuilder();

                if (!IsExportMethod)
                {
                    sql.Append(@"SELECT cod_tipo_participacao, descricao FROM (
                                    SELECT a.cod_tipo_participacao, a.descricao, rownum r__ FROM ( ");
                }

                sql.Append(@"select t.cod_tipo_participacao as cod_tipo_participacao, 
                                    t.dsc_tipo_participacao as descricao
                             from jur.tipo_participacao t ");

                #region "Filters"
                if (!string.IsNullOrEmpty(descricao))
                {
                    sql.Append($"where LOWER(t.dsc_tipo_participacao) like '%{descricao.ToLower()}%' ");
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
                    var obj = new FiltroTipoParticipacaoResultadoDTO();
                    obj.CodTipoParticipacao = dataReader.GetInt64(0);
                    obj.Descricao = dataReader.IsDBNull(1) ? string.Empty : dataReader.GetString(1).Trim();

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
                    case "codtipoparticipacao":
                        if (order.Direction.ToLower().Equals("asc"))
                            orderBy += @" cod_tipo_participacao asc,";
                        else
                            orderBy += @" cod_tipo_participacao desc,";

                        break;
                    case "descricao":
                        if (order.Direction.ToLower().Equals("asc"))
                            orderBy += @" descricao asc,";
                        else
                            orderBy += @" descricao desc,";

                        break;
                    default:
                        break;
                }
            }

            return orderBy.Remove(orderBy.Length - 1, 1);
        }
    }
}
