using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Domain.ContingenciaPex.DTO;
using Perlink.Oi.Juridico.Domain.ContingenciaPex.Entity;
using Perlink.Oi.Juridico.Domain.ContingenciaPex.Interface.Repository;
using Shared.Data;
using Shared.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Perlink.Oi.Juridico.Data.ContingenciaPex.Repository
{
    public class FechamentoPexMediaRepository : BaseCrudRepository<FechamentoPexMedia, long>, IFechamentoPexMediaRepository
    {
        private readonly JuridicoContext _context;
        private readonly IAuthenticatedUser _user;
        public FechamentoPexMediaRepository(JuridicoContext context, IAuthenticatedUser user) : base(context, user)
        {
            _context = context;
            _user = user;
        }
        public override Task<IDictionary<string, string>> RecuperarDropDown()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<FechamentoContingenciaPexMediaDTO>> ListarFechamentos(string dataInicio, string dataFim, int quantidade, int pagina)
        {
            var sql = @"SELECT COD_SOLIC_FECHAMENTO_CONT AS ID,
                           DAT_FECHAMENTO AS DATAFECHAMENTO,
                           NOME_USUARIO AS NOMEUSUARIO,
                           NRO_MESES_MEDIA_HISTORICA AS NUMEROMESES,
                           PER_HAIRCUT AS PERCENTUALHAIRCUT,
                           VAL_MULT_DESVIO_PADRAO AS MULTDESVIOPADRAO,
                           DECODE(IND_APLICAR_HAIRCUT_PROC_GAR, 'S', 'SIM', 'NÃO') AS INDAPLICARHAIRCUT,
                           LISTAGG(NOME, ', ') WITHIN GROUP(ORDER BY NOME) AS EMPRESAS,
                           max(DAT_GERACAO) as DATAEXECUCAO
                      FROM (SELECT FPM.COD_SOLIC_FECHAMENTO_CONT,
                                   FPM.DAT_FECHAMENTO,
                                   USU.NOME_USUARIO,
                                   FPM.NRO_MESES_MEDIA_HISTORICA,
                                   FPM.PER_HAIRCUT,
                                   FPM.VAL_MULT_DESVIO_PADRAO,
                                   FPM.IND_APLICAR_HAIRCUT_PROC_GAR,
                                   EMP.NOME,
                                   FPM.DAT_GERACAO
                              FROM JUR.FECHAMENTO_PEX_MEDIA     FPM,
                                   JUR.EMPRESAS_CENTRALIZADORAS EMP,
                                   JUR.ACA_USUARIO              USU
                             WHERE EMP.CODIGO = FPM.COD_EMPRESA_CENTRALIZADORA
                               AND FPM.COD_USUARIO = USU.COD_USUARIO
                               AND FPM.DAT_FECHAMENTO BETWEEN
                                   TO_DATE({0}, 'DD/MM/YYYY') AND
                                   TO_DATE({1}, 'DD/MM/YYYY')
                             ORDER BY FPM.DAT_FECHAMENTO, FPM.DAT_GERACAO DESC)
                     GROUP BY COD_SOLIC_FECHAMENTO_CONT,
                              DAT_FECHAMENTO,
                              NOME_USUARIO,
                              NRO_MESES_MEDIA_HISTORICA,
                              PER_HAIRCUT,
                              VAL_MULT_DESVIO_PADRAO,
                              DECODE(IND_APLICAR_HAIRCUT_PROC_GAR, 'S', 'SIM', 'NÃO')
                    ORDER BY  DAT_FECHAMENTO desc,
                              max(DAT_GERACAO) desc";

            var resultado = await _context.Query<FechamentoContingenciaPexMediaDTO>().FromSql(sql, dataInicio, dataFim).ToListAsync();
            return resultado.Skip((pagina - 1) * quantidade).Take(quantidade);
        }

        public async Task<int> TotalFechamentos(string dataInicio, string dataFim)
        {
            var sql = @"SELECT COD_SOLIC_FECHAMENTO_CONT AS ID,
                           DAT_FECHAMENTO AS DATAFECHAMENTO,
                           NOME_USUARIO AS NOMEUSUARIO,
                           NRO_MESES_MEDIA_HISTORICA AS NUMEROMESES,
                           PER_HAIRCUT AS PERCENTUALHAIRCUT,
                           VAL_MULT_DESVIO_PADRAO AS MULTDESVIOPADRAO,
                           DECODE(IND_APLICAR_HAIRCUT_PROC_GAR, 'S', 'SIM', 'NÃO') AS INDAPLICARHAIRCUT,
                           LISTAGG(NOME, ', ') WITHIN GROUP(ORDER BY NOME) AS EMPRESAS,
                           max(DAT_GERACAO) as DATAEXECUCAO
                      FROM (SELECT FPM.COD_SOLIC_FECHAMENTO_CONT,
                                   FPM.DAT_FECHAMENTO,
                                   USU.NOME_USUARIO,
                                   FPM.NRO_MESES_MEDIA_HISTORICA,
                                   FPM.PER_HAIRCUT,
                                   FPM.VAL_MULT_DESVIO_PADRAO,
                                   FPM.IND_APLICAR_HAIRCUT_PROC_GAR,
                                   EMP.NOME,
                                   FPM.DAT_GERACAO
                              FROM JUR.FECHAMENTO_PEX_MEDIA     FPM,
                                   JUR.EMPRESAS_CENTRALIZADORAS EMP,
                                   JUR.ACA_USUARIO              USU
                             WHERE EMP.CODIGO = FPM.COD_EMPRESA_CENTRALIZADORA
                               AND FPM.COD_USUARIO = USU.COD_USUARIO
                               AND FPM.DAT_FECHAMENTO BETWEEN
                                   TO_DATE({0}, 'DD/MM/YYYY') AND
                                   TO_DATE({1}, 'DD/MM/YYYY')
                             ORDER BY FPM.DAT_FECHAMENTO, FPM.DAT_GERACAO DESC)
                     GROUP BY COD_SOLIC_FECHAMENTO_CONT,
                              DAT_FECHAMENTO,
                              NOME_USUARIO,
                              NRO_MESES_MEDIA_HISTORICA,
                              PER_HAIRCUT,
                              VAL_MULT_DESVIO_PADRAO,
                              DECODE(IND_APLICAR_HAIRCUT_PROC_GAR, 'S', 'SIM', 'NÃO')
                    ORDER BY  DAT_FECHAMENTO desc,
                              max(DAT_GERACAO) desc";

            var resultado = await _context.Query<FechamentoContingenciaPexMediaDTO>().FromSql(sql, dataInicio, dataFim).ToListAsync();
            return resultado.Count();
        }
    }
}
