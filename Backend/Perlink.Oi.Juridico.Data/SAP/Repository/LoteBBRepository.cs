using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using Perlink.Oi.Juridico.Data.Compartilhado.Repository;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Repository;
using Shared.Data;
using Shared.Domain.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.SAP.Repository
{
    public class LoteBBRepository : BaseCrudRepository<Lote, long>, ILoteBBRepository
    {
        private readonly JuridicoContext dbContext;
        private readonly LoteLancamentoRepository loteLancamentoRepository;
        private readonly LancamentoProcessoRepository lancamentoProcessoRepository;

        public LoteBBRepository(JuridicoContext dbContext, IAuthenticatedUser user) : base(dbContext, user)
        {
            this.dbContext = dbContext;
            loteLancamentoRepository = new LoteLancamentoRepository(dbContext, user);
            lancamentoProcessoRepository = new LancamentoProcessoRepository(dbContext, user);
        }

        public override async Task<IDictionary<string, string>> RecuperarDropDown()
        {
            return await dbContext.Set<Lote>()
                    .AsNoTracking()
                    .OrderBy(x => x.DataCriacao)
                    .ToDictionaryAsync(x => x.Id.ToString(),
                                       x => x.NumeroLoteBB.ToString());
        }

        public bool MontarHeader(ArquivoBBDTO arquivoBBDTO)
        {
            string sql = @"
                    SELECT '000'                                                                   ||
			             LTRIM(TO_CHAR(NVL(ECC.COD_CONVENIO_BB, 0), '0000'))                     ||
			             LTRIM(TO_CHAR(MOD(NVL(L.NRO_LOTE_BB,0), 1000), '000'))                  ||
			             LTRIM(TO_CHAR(L.DAT_CRIACAO_LOTE, 'DD.MM.YYYY'))                        ||
                   'DJO100'                                                                ||
                   ' '                                                                     ||
                   LTRIM(TO_CHAR(NVL(ECC.COD_BANCO_DEBITO_BB,0), '000000000'))             ||
                   LTRIM(TO_CHAR(NVL(ECC.COD_AGENCIA_DEBITO_BB,0), '0000'))                ||
                   LPAD(ECC.NUM_CONTA_DEBITO_BB, 11, '0' )                           ||
                   LPAD(ECC.CNPJ_EMPRESA_CONVENIO_BB, 14, '0')                     ||
                   LTRIM(TO_CHAR(NVL(ECC.COD_MCI,0), '000000000'))                         ||
                   LPAD(' ',326,' ') HEADER,
                   NVL(ECC.NUM_AGENCIA_DEPOSITARIA, 0) NUMAGENCIA

              FROM   JUR.EMPRESAS_CENTRALIZADORAS EC,
                   JUR.LOTE                      L,
                   JUR.PARTE                     P,
                   JUR.EMPR_CENTRALIZADORA_CONVENIO ECC
              WHERE  EC.CODIGO           = P.EMPCE_COD_EMP_CENTRALIZADORA
              AND    L.COD_PARTE_EMPRESA = P.COD_PARTE
              AND    L.NRO_LOTE_BB       = :NUM_LOTE_BB
              AND    EC.CODIGO           = ECC.COD_EMPRESA_CENTRALIZADORA
              AND    ECC.COD_ESTADO      = :COD_ESTADO";

            var parametros = new List<OracleParameter>()
            {
                new OracleParameter("NUM_LOTE_BB", arquivoBBDTO.NumeroLoteBB),
                new OracleParameter("COD_ESTADO", arquivoBBDTO.CodigoEstado)
            };

            var resultado = dbContext.ExecutarQuery(sql, parametros.ToArray()).Result;
            if (((Newtonsoft.Json.Linq.JContainer)(JsonConvert.DeserializeObject<dynamic>(resultado))).Count > 0)
            {
                var row = JsonConvert.DeserializeObject<dynamic>(resultado)[0];
                arquivoBBDTO.ConteudoArquivo.AppendLine(row.HEADER.Value);
                arquivoBBDTO.NumeroAgenciaDepositaria = row.NUMAGENCIA.Value.ToString();
                return true;
            }
            return false;
        }

        public bool MontarDetalheArquivo(ArquivoBBDTO arquivoBBDTO)
        {
            string sql = $@"
                SELECT '111'||
                        LTRIM(TO_CHAR(NVL(T.CODIGO, 0), '000000000'))||
                        LTRIM(TO_CHAR(NVL(BC.CODIGO, 0), '000000000'))||
                        LTRIM(TO_CHAR(NVL(O.CODIGO, 0), '000000000'))||
                        LTRIM(TO_CHAR(NVL(N.CODIGO, 0), '0000'))||
                        '0196'||
                        LTRIM(TO_CHAR(NVL(M.CODIGO, 0), '0000'))||
                        LTRIM(RPAD(PR.NOM_PARTE, 30, ' '))||
                        DECODE(PR.COD_TIPO_PARTE, 'F', '1', '2')||
                        LTRIM(TO_CHAR(NVL(DECODE(PR.COD_TIPO_PARTE, 'F', PR.CPF_PARTE, PR.CGC_PARTE),'0'), '00000000000000'))||
                        LTRIM(RPAD(PA.NOM_PARTE,30,' '))||
                        DECODE(PA.COD_TIPO_PARTE, 'F', '1', '2')||
                        LTRIM(TO_CHAR(NVL(DECODE(PA.COD_TIPO_PARTE, 'F', PA.CPF_PARTE, PA.CGC_PARTE),'0'), '00000000000000'))||
                        '1'||
                        LTRIM(RPAD(P.NRO_PROCESSO_CARTORIO, 25, ' '))||
                        DECODE(LP.DAT_GUIA_JUDICIAL, NULL, LTRIM(TO_CHAR(LP.DAT_LANCAMENTO, 'DD.MM.YYYY')), LTRIM(TO_CHAR(LP.DAT_GUIA_JUDICIAL, 'DD.MM.YYYY')))||
                        LTRIM(RPAD(LTRIM(DECODE(LENGTH(TO_CHAR(LP.NRO_GUIA)), 6, TO_CHAR(LP.NRO_GUIA, '0000000'), 7, TO_CHAR(LP.NRO_GUIA, '0000000'), 8, TO_CHAR(LP.NRO_GUIA, '00000000'), 9, TO_CHAR(LP.NRO_GUIA, '000000000'), 10, TO_CHAR(LP.NRO_GUIA, '0000000000'), TO_CHAR(LP.NRO_GUIA))), 15, ' '))||
                        LTRIM(TO_CHAR(P.COD_PROCESSO, '000000000'))||
                        LTRIM(TO_CHAR(LP.COD_LANCAMENTO, '0000'))||
                        LTRIM(TO_CHAR(LP.VAL_LANCAMENTO*100, '00000000000000000'))||
                        LTRIM(TO_CHAR(:NUM_AG_DEPOSITARIA, '0000'))||
                        LTRIM(RPAD(TRIM(TO_CHAR(L.COD_LOTE,'000000'))||TRIM(TO_CHAR(L.NRO_LOTE_BB,'000000'))||TRIM(to_char(P.COD_PROCESSO,'00000000'))||TRIM(To_Char(LP.COD_LANCAMENTO,'0000')), 26, ' '))||
                        LPAD(' ', 69, ' ')||
                        LTRIM(TO_CHAR(NVL(LP.NUM_CONTA_JUDICIAL,0), '0000000000000'))||
                        LPAD(' ', 75, ' ') as LINHA
                FROM   JUR.BB_ORGAOS            O,
                        JUR.ACAO                AC,
                        JUR.LANCAMENTO_PROCESSO LP,
                        JUR.PARTE               PR,
                        JUR.PARTE               PA,
                        JUR.PROCESSO             P,
                        JUR.LOTE                 L,
                        JUR.COMARCA             CO,
                        JUR.BB_COMARCAS         BC,
                        JUR.BB_TRIBUNAIS         T,
                        JUR.BB_MODALIDADES       M,
                        JUR.LOTE_LANCAMENTO     LL,
                        JUR.BB_NATUREZAS_ACOES   N,
                        JUR.VARA                 V
                WHERE  P.COD_PROCESSO               = LP.COD_PROCESSO
                AND    AC.COD_ACAO                  = P.COD_ACAO
                AND    P.COD_COMARCA                = CO.COD_COMARCA
                AND    BC.ID                        = O.BBCOM_ID_BB_COMARCA
                AND    CO.BBCOM_ID_BB_COMARCA       = BC.ID
                AND    O.BBTRI_ID_BB_TRIBUNAL       = T.ID
                AND    LP.BBMOD_ID_BB_MODALIDADE    = M.ID
                AND    L.COD_LOTE                   = LL.COD_LOTE
                AND    LL.COD_PROCESSO              = LP.COD_PROCESSO
                AND    LL.COD_LANCAMENTO            = LP.COD_LANCAMENTO
                AND    P.COD_PARTE_EMPRESA          = PR.COD_PARTE
                AND    PA.COD_PARTE                 = LP.COD_PARTE
                AND    AC.BBNAT_ID_BB_NAT_ACAO      = N.ID
                AND    V.COD_COMARCA                = P.COD_COMARCA
                AND    V.COD_VARA                   = P.COD_VARA
                AND    V.COD_TIPO_VARA              = P.COD_TIPO_VARA
                AND    O.ID                         = V.BBORG_ID_BB_ORGAO
                AND    L.NRO_LOTE_BB                = : NUM_LOTE_BB
                ";

            var parametros = new List<OracleParameter>()
            {
                new OracleParameter("NUM_AG_DEPOSITARIA", arquivoBBDTO.NumeroAgenciaDepositaria),
                new OracleParameter("NUM_LOTE_BB", arquivoBBDTO.NumeroLoteBB)
            };

            var resultado = dbContext.ExecutarQuery(sql, parametros.ToArray()).Result;
            if (((Newtonsoft.Json.Linq.JContainer)(JsonConvert.DeserializeObject<dynamic>(resultado))).Count > 0)
            {
                foreach (var row in JsonConvert.DeserializeObject<dynamic>(resultado))
                {
                    arquivoBBDTO.ConteudoArquivo.AppendLine(row.LINHA.Value);
                }
                return true;
            }
            return false;
        }

        public bool MontarTrailerArquivo(ArquivoBBDTO arquivoBBDTO)
        {
            string sql = $@"
                    SELECT '999'                                                                   ||
			                LPAD(' ',13,' ')                                                       ||
			                LTRIM(TO_CHAR(COUNT(1)+2, '000000000'))                                ||
			                LTRIM(TO_CHAR(NVL(SUM(LP.VAL_LANCAMENTO)*100, 0), '00000000000000000'))||
			                LPAD(' ',358,' ') TRAILER
	                FROM   JUR.LANCAMENTO_PROCESSO LP,
			                JUR.LOTE                 L,
			                JUR.LOTE_LANCAMENTO     LL
	                WHERE  L.COD_LOTE                = LL.COD_LOTE
	                    AND    LL.COD_PROCESSO           = LP.COD_PROCESSO
	                    AND    LL.COD_LANCAMENTO         = LP.COD_LANCAMENTO
	                    AND    L.NRO_LOTE_BB             = :NUM_LOTE_BB
                ";

            var parametros = new List<OracleParameter>() { new OracleParameter("NUM_LOTE_BB", arquivoBBDTO.NumeroLoteBB) };
            var resultado = dbContext.ExecutarQuery(sql, parametros.ToArray()).Result;
            if (((Newtonsoft.Json.Linq.JContainer)(JsonConvert.DeserializeObject<dynamic>(resultado))).Count > 0)
            {
                var row = JsonConvert.DeserializeObject<dynamic>(resultado)[0];
                arquivoBBDTO.ConteudoArquivo.AppendLine(row.TRAILER.Value);
                return true;
            }
            return false;
        }

        public bool ExisteParametroConvenio(long numeroLoteBB, string codigoEstado)
        {
            var retorno = dbContext.Set<Lote>()
                             .Count(l => l.Parte.EmpresaCentralizadora.Id == l.Parte.CodigoEmpresaCentralizadora &&
                                       l.Parte.Id == l.CodigoParte &&
                                       l.NumeroLoteBB == numeroLoteBB &&
                                       l.Parte.EmpresaCentralizadora.EmpresasCentralizadorasConvenio.Any(P => P.CodigoEmpresaCentralizadora == l.Parte.EmpresaCentralizadora.Id) &&
                                       l.Parte.EmpresaCentralizadora.EmpresasCentralizadorasConvenio.Any(p => p.CodigoEstado == codigoEstado));

            return retorno > 0 ? true : false;
        }

        public string[] ValidaLancamentosLoteBB(long numeroLoteBB)
        {
            //var retorno = dbContext.Set<LoteLancamento>()
            //                       .Count(ll => ll.Id == ll.Lote.Id && ll.Lote.NumeroLoteBB == numeroLoteBB);

            //return retorno == 0;

            var sql = @"
                SELECT 
                LL.COD_PROCESSO AS PROCESSO,
                LL.COD_LANCAMENTO AS LANCAMENTO,
                DECODE(AUTOR.COD_PARTE, NULL, 'Falta Nome do Autor na guia','') AS MENSAGEM_AUTOR,
                DECODE(BBM.ID , NULL, 'Falta configuração na Modalidade BB','') AS MENSAGEM_MODALIDADE,
                DECODE(BBN.ID , NULL, 'Falta configuração na Ação BB','') AS MENSAGEM_NATUREZA,
                DECODE(BBC.ID , NULL, 'Falta configuração na Comarca BB','') AS MENSAGEM_COMARCA,
                DECODE(BBO.ID , NULL, 'Falta configuração na Vara BB','') AS MENSAGEM_ORGAO,
                DECODE(BBT.ID , NULL, 'Falta configuração no Orgão BB','') AS MENSAGEM_TRIBUNAL
                FROM JUR.LOTE L,
                     JUR.LOTE_LANCAMENTO LL,
                     JUR.PROCESSO P,
                     JUR.ACAO ACAO,
                     JUR.COMARCA CO,
                     JUR.PARTE EMPR,
                     JUR.VARA V,
                     JUR.TIPO_VARA TV,
                     JUR.LANCAMENTO_PROCESSO LP,
                     JUR.PARTE AUTOR,
                     JUR.BB_COMARCAS BBC,
                     JUR.BB_TRIBUNAIS  BBT,
                     JUR.BB_MODALIDADES BBM,
                     JUR.BB_ORGAOS BBO,
                     JUR.BB_NATUREZAS_ACOES BBN,
                     jur.status_pagamento  sp,
                     jur.categoria_pagamento cp 
                     WHERE L.nro_LOTE_bb = :NUM_LOTE_BB
                     AND LL.COD_LOTE = L.COD_LOTE
                     AND LP.COD_PROCESSO = LL.COD_PROCESSO
                     AND LP.COD_LANCAMENTO = LL.COD_LANCAMENTO
                     AND P.COD_PROCESSO = LL.COD_PROCESSO
                     AND ACAO.COD_ACAO = P.COD_ACAO
                     AND CO.COD_COMARCA = P.COD_COMARCA
                     AND EMPR.COD_PARTE = P.COD_PARTE_EMPRESA
                     AND V.COD_COMARCA = P.COD_COMARCA
                     AND V.COD_VARA = P.COD_VARA
                     AND V.COD_TIPO_VARA = P.COD_TIPO_VARA
                     AND TV.COD_TIPO_VARA = P.COD_TIPO_VARA
                     AND AUTOR.COD_PARTE(+) = LP.COD_PARTE
                     AND BBM.ID(+) = LP.BBMOD_ID_BB_MODALIDADE
                     AND BBN.ID(+) = ACAO.BBNAT_ID_BB_NAT_ACAO
                     AND BBC.ID(+) = CO.BBCOM_ID_BB_COMARCA
                     AND BBO.ID(+) = V.BBORG_ID_BB_ORGAO
                     AND BBT.ID(+) = BBO.BBTRI_ID_BB_TRIBUNAL
                     and lp.cod_cat_pagamento = cp.cod_cat_pagamento
                     and lp.cod_status_pagamento = sp.cod_status_pagamento
                     AND (AUTOR.COD_PARTE Is Null OR BBM.ID Is Null OR BBN.ID Is Null OR
                     BBC.ID Is Null OR BBO.ID Is Null OR BBT.ID Is Null)
            ";

            var parametros = new List<OracleParameter>()
            {
                new OracleParameter("NUM_LOTE_BB", numeroLoteBB),
            };

            var resultado = dbContext.ExecutarQuery(sql, parametros.ToArray()).Result;
            if (((Newtonsoft.Json.Linq.JContainer)(JsonConvert.DeserializeObject<dynamic>(resultado))).Count > 0)
            {
                List<string> listaMensagens = new List<string>();

                for (int i = 0; i < ((Newtonsoft.Json.Linq.JContainer)(JsonConvert.DeserializeObject<dynamic>(resultado))).Count; i++)
                {
                    string linhaMensagem = $"Proceso: {JsonConvert.DeserializeObject<dynamic>(resultado)[i].PROCESSO.Value} - Lançamento: {JsonConvert.DeserializeObject<dynamic>(resultado)[i].LANCAMENTO.Value}";


                    if (JsonConvert.DeserializeObject<dynamic>(resultado)[0].MENSAGEM_AUTOR.Value != null)
                    {
                        linhaMensagem += $" - {JsonConvert.DeserializeObject<dynamic>(resultado)[i].MENSAGEM_AUTOR.Value}";                     
                    }

                    if (JsonConvert.DeserializeObject<dynamic>(resultado)[0].MENSAGEM_MODALIDADE.Value != null)
                    {
                        linhaMensagem += linhaMensagem.Length > 0 ? $" - {JsonConvert.DeserializeObject<dynamic>(resultado)[i].MENSAGEM_MODALIDADE.Value}" : JsonConvert.DeserializeObject<dynamic>(resultado)[i].MENSAGEM_MODALIDADE.Value;
                    }

                    if (JsonConvert.DeserializeObject<dynamic>(resultado)[0].MENSAGEM_NATUREZA.Value != null)
                    {
                        linhaMensagem += linhaMensagem.Length > 0 ? $" - {JsonConvert.DeserializeObject<dynamic>(resultado)[i].MENSAGEM_NATUREZA.Value}" : JsonConvert.DeserializeObject<dynamic>(resultado)[i].MENSAGEM_NATUREZA.Value;
                    }

                    if (JsonConvert.DeserializeObject<dynamic>(resultado)[0].MENSAGEM_COMARCA.Value != null)
                    {
                        linhaMensagem += linhaMensagem.Length > 0 ? $" - {JsonConvert.DeserializeObject<dynamic>(resultado)[i].MENSAGEM_COMARCA.Value}" : JsonConvert.DeserializeObject<dynamic>(resultado)[i].MENSAGEM_COMARCA.Value;
                    }

                    if (JsonConvert.DeserializeObject<dynamic>(resultado)[0].MENSAGEM_ORGAO.Value != null)
                    {
                        linhaMensagem += linhaMensagem.Length > 0 ? $" - {JsonConvert.DeserializeObject<dynamic>(resultado)[i].MENSAGEM_ORGAO.Value}" : JsonConvert.DeserializeObject<dynamic>(resultado)[i].MENSAGEM_ORGAO.Value;
                    }

                    if (JsonConvert.DeserializeObject<dynamic>(resultado)[0].MENSAGEM_TRIBUNAL.Value != null)
                    {
                        linhaMensagem += linhaMensagem.Length > 0 ? $" - {JsonConvert.DeserializeObject<dynamic>(resultado)[i].MENSAGEM_TRIBUNAL.Value}" : JsonConvert.DeserializeObject<dynamic>(resultado)[i].MENSAGEM_TRIBUNAL.Value;
                    }
                    
                    listaMensagens.Add(linhaMensagem);                                      
                }

                return listaMensagens.ToArray();
            }

            return null;
        }

        public async Task<List<string>> RecuperarEstados(long CodigoLote)
        {
            var retorno = await dbContext.Set<LoteLancamento>()
                             .Where(lp => lp.Id == CodigoLote &&
                                    lp.CodigoProcesso == lp.LancamentoProcesso.Id &&
                                    lp.CodigoLancamento == lp.LancamentoProcesso.CodigoLancamento &&
                                    lp.LancamentoProcesso.Processo.Id == lp.LancamentoProcesso.Id &&
                                    lp.LancamentoProcesso.Processo.CodigoComarca == lp.LancamentoProcesso.Processo.Comarca.Id)
                             .Select(co => co.LancamentoProcesso.Processo.Comarca.CodigoEstado).ToListAsync();

            return retorno;
        }
        public async Task<IList<LancamentoProcesso>> ObterLancamentosPorLoteComAssociacao(long codigoLote)
        {
            var retorno = await dbContext.Set<LoteLancamento>()
                        .AsNoTracking()
                        .Include(ll => ll.LancamentoProcesso)
                        .Where(ll => ll.CodigoLancamento == ll.LancamentoProcesso.CodigoLancamento
                                    && ll.CodigoProcesso == ll.LancamentoProcesso.Id
                                    && ll.Lote.Id == codigoLote
                        )
                        .Select(ll => ll.LancamentoProcesso)
                        .ToListAsync();

            return retorno;
        }
    }
}