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
    public class BaseCalculoAdoRepository : AdoBaseRepository, IBaseCalculoAdoRepository
    {
        public int GetTotalCount(string descricao)
        {
            OracleDataReader dataReader = null;

            try
            {
                var sql = new StringBuilder();
                var paramList = new List<OracleParameter>();

                sql.Append("SELECT Count(*) FROM jur.base_calculo bc ");

                #region "Filters"

                if (!string.IsNullOrEmpty(descricao))
                {
                    sql.Append($"where bc.dsc_base_calculo like '%{descricao}%' ");
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

        public IEnumerable<FiltroBaseCalculoResultadoDTO> ObterTodosPorFiltro(string descricao, int pageNumber, int pageSize, List<SortOrder> orders, bool IsExportMethod)
        {
            var list = new List<FiltroBaseCalculoResultadoDTO>();
            var paramList = new List<OracleParameter>();
            OracleDataReader dataReader = null;

            try
            {
                var sql = new StringBuilder();

                if (!IsExportMethod)
                {
                    sql.Append(@"SELECT cod_base_calculo, descricao, eh_base_inicial FROM (
                                    SELECT a.cod_base_calculo, a.descricao, a.eh_base_inicial, rownum r__ FROM ( ");
                }

                sql.Append(@"SELECT bc.COD_BASE_CALCULO as cod_base_calculo, 
                             bc.DSC_BASE_CALCULO as descricao, 
                             bc.IND_BASE_INICIAL as eh_base_inicial 
                             FROM jur.base_calculo bc ");

                #region "Filters"
                if (!string.IsNullOrEmpty(descricao))
                {
                    sql.Append($"where LOWER(bc.DSC_BASE_CALCULO) like '%{descricao.ToLower()}%' ");
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
                    var obj = new FiltroBaseCalculoResultadoDTO();
                    obj.CodBaseCalculo = dataReader.GetInt64(0);
                    obj.Descricao = dataReader.IsDBNull(1) ? string.Empty : dataReader.GetString(1).Trim();
                    obj.EhCalculoInicial = dataReader.IsDBNull(2) ? false : dataReader.GetString(2) == "S" ? true : false;

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
            var orderBy = " eh_base_inicial desc,";

            foreach (var order in orders)
            {
                if (order.Property is null) {
                    break;
                }
                switch (order.Property.ToLower())
                {
                    case "codbasecalculo":
                        if (order.Direction.ToLower().Equals("asc"))
                            orderBy += @" cod_base_calculo asc,";
                        else
                            orderBy += @" cod_base_calculo desc,";

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
