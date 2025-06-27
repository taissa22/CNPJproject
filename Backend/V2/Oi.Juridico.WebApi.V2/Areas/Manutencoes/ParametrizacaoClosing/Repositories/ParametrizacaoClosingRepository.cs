using Microsoft.Win32;
using Oi.Juridico.Contextos.V2.ParametrizacaoClosingContext.Data;
using Oi.Juridico.Contextos.V2.ParametroJuridicoContext.Data;
using Oi.Juridico.WebApi.V2.Areas.Manutencoes.ParametrizacaoClosing.Dtos;
using Oracle.ManagedDataAccess.Client;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity;
using System.Data;
using System.Drawing.Text;

namespace Oi.Juridico.WebApi.V2.Areas.Manutencoes.ParametrizacaoClosing.Repositories
{
    public class ParametrizacaoClosingRepository
    {
        private readonly ParametrizacaoClosingContext _parametrizacaoClosingContext;

        public ParametrizacaoClosingRepository(ParametrizacaoClosingContext parametrizacaoClosingContext)
        {
            _parametrizacaoClosingContext = parametrizacaoClosingContext;
        }

        public async Task <bool> ValidarRegraClassificacaoClosing(AtualizarRequest request, string tipoTecnologia)
        {
            OracleParameter[] @params = {
                                            // Valor de retorno
                                            new OracleParameter("returnVal", OracleDbType.Int64) { Direction = ParameterDirection.ReturnValue },
 
                                            // P_COD_CLASSIFICACAO_CLOSING
                                            new OracleParameter("P_COD_CLASSIFICACAO_CLOSING", OracleDbType.Int64) {
                                                Value = request.ClassificaoClosing
                                            },
                                            // P_COD_CLASSF_CLOSING_CLIENTCO
                                            new OracleParameter("P_COD_CLASSF_CLOSING_CLIENTCO", OracleDbType.Int64) {
                                                Value = request.ClassificaoClosingClientCO
                                            },
                                            // P_LST_TIPO_TEC (passando como DBNull)
                                            new OracleParameter("P_LST_TIPO_TEC", OracleDbType.Varchar2) { Value = tipoTecnologia},
                                            // P_COD_PROCESSO
                                            new OracleParameter("P_COD_PROCESSO", OracleDbType.Varchar2) {
                                                Value = null
                                            }
                                        };

            await _parametrizacaoClosingContext.Database.ExecuteSqlRawAsync(
                "BEGIN :returnVal := JUR.FN_REGRA_CC_JEC_PERC_RESP(:P_COD_CLASSIFICACAO_CLOSING, :P_COD_CLASSF_CLOSING_CLIENTCO, :P_LST_TIPO_TEC, :P_COD_PROCESSO); END;",
                @params
            );

            int result = Convert.ToInt32(((Oracle.ManagedDataAccess.Types.OracleDecimal)(@params[0].Value)).Value);

            return result == 1;

        }

    }
}
