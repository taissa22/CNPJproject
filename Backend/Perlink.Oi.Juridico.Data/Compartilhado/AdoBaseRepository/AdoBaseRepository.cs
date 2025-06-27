using System;
using Oracle.ManagedDataAccess.Client;
using Perlink.Oi.Juridico.Domain.Compartilhado;
using System.Data;

namespace Perlink.Oi.Juridico.Data.Compartilhado.AdoBaseRepository
{
    public class AdoBaseRepository
    {
        private OracleConnection conexao;

        public void AbrirConexao()
        {
            if (conexao == null)
            {
                string connStr = Runtime.ConnectionString;
                conexao = new OracleConnection(connStr);
            }

            if (conexao.State == ConnectionState.Closed)
            {
                conexao.Open();
            }
        }

        public OracleDataReader ExecuteReader(string sql, params OracleParameter[] parametros)
        {
            OracleCommand comando = new OracleCommand(sql, conexao);

            if (parametros != null)
            {
                comando.Parameters.AddRange(parametros);
            }

            return comando.ExecuteReader();
        }

        public Int32 ExecuteNonQuery(string sql, params OracleParameter[] parametros)
        {
            OracleCommand comando = new OracleCommand(sql, conexao);

            if (parametros != null)
            {
                comando.Parameters.AddRange(parametros);
            }

            return comando.ExecuteNonQuery();
        }

        public object ExecuteScalar(string sql, params OracleParameter[] parametros)
        {
            OracleCommand comando = new OracleCommand(sql, conexao);

            if (parametros != null)
            {
                comando.Parameters.AddRange(parametros);
            }

            return comando.ExecuteScalar();
        }

        public void FecharConexao()
        {
            if (conexao != null)
            {
                conexao.Close();
            }
        }
    }
}
