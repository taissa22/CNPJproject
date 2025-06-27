using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;
using Perlink.Oi.Juridico.Domain.SAP.DTO.SaldoGarantia;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Perlink.Oi.Juridico.Domain.SAP.Enum;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Repository;
using Shared.Data;
using Shared.Domain.Interface;
using Shared.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.SAP.Repository
{
    public class CriterioSaldoGarantiaRepository : BaseCrudRepository<CriterioSaldoGarantia, long>, ICriterioSaldoGarantiaRepository
    {
        private readonly JuridicoContext dbContext;

        public CriterioSaldoGarantiaRepository(JuridicoContext dbContext, IAuthenticatedUser user) : base(dbContext, user)
        {
            this.dbContext = dbContext;
        }



        public override Task<IDictionary<string, string>> RecuperarDropDown()
        {
            throw new System.NotImplementedException();
        }

        public async Task CriarCriteriosAsync(AgendamentoSaldoGarantia agendamento, SaldoGarantiaAgendamentoDTO filtroDTO)
        {
            List<CriterioSaldoGarantia> listaCriterios = new List<CriterioSaldoGarantia>();

            // Critérios que serão adicionados no banco (Segue parâmetros do filtroDTO)
            // OBS: Tipo processo é obrigatório em toda requisição.
            listaCriterios.Add(new CriterioSaldoGarantia
            {
                Criterio = @"EXISTS (
                        SELECT * FROM jur.lancamento_processo
                        WHERE
                            (jur.lancamento_processo.cod_tipo_lancamento = 1)   and
                            (jur.lancamento_processo.cod_processo = jur.processo.cod_processo) and
                            (jur.processo.cod_tipo_processo = :TP_PROCESSO)
                    )",
                Parametros = ":TP_PROCESSO",
                ValoresParametros = filtroDTO.TipoProcesso.ToString(),
                NomeCriterio = "TipoProcesso",
                NomeCriterioFormatado = "Tipo Processo"
            });

            //Só grava o critério de status do processo caso haja valor selecionado no filtro (não considera 'ambos')
            if (filtroDTO.StatusDoProcesso.HasValor_S_N() == "S")
            {
                listaCriterios.Add(new CriterioSaldoGarantia
                {
                    Criterio = "(jur.processo.ind_processo_ativo = :ST_PROCESSO)",
                    Parametros = ":ST_PROCESSO",
                    ValoresParametros = $"{filtroDTO.StatusDoProcesso.Retorna_S_N()}",
                    NomeCriterio = "StatusDoProcesso",
                    NomeCriterioFormatado = "Status do Processo"
                });
            }
            if (filtroDTO.DataFinalizacaoContabilInicio.HasValue && filtroDTO.DataFinalizacaoContabilFim.HasValue)
                listaCriterios.Add(new CriterioSaldoGarantia
                {
                    Criterio = "dat_finalizacao_contabil between :DAT_CONTAB_INICIO and :DAT_CONTAB_FIM",
                    Parametros = ":DAT_CONTAB_INICIO;:DAT_CONTAB_FIM",
                    ValoresParametros = $"{filtroDTO.DataFinalizacaoContabilInicio.DataFormatada()};{filtroDTO.DataFinalizacaoContabilFim.DataFormatada()}",
                    NomeCriterio = "DataFinalizacaoContabil",
                    NomeCriterioFormatado = "Data Finalização Contábil"
                });
            if (filtroDTO.ValorDepositoInicio.GetValueOrDefault() >= 0 && filtroDTO.ValorDepositoFim.GetValueOrDefault() > 0)
            {
                listaCriterios.Add(new CriterioSaldoGarantia
                {
                    Criterio = @"
                                ((cg.tsgar_cod_tipo_saldo_garantia = 1 and cg.valor_multiplicador = 1 and valor_principal between :VAL_DEPOSITO_INICIO and :VAL_DEPOSITO_FIM) 
		                            or (cg.tsgar_cod_tipo_saldo_garantia <> 1))",
                    Parametros = ":VAL_DEPOSITO_INICIO;:VAL_DEPOSITO_FIM",
                    ValoresParametros = $"{filtroDTO.ValorDepositoInicio.GetValueOrDefault()};{filtroDTO.ValorDepositoFim.GetValueOrDefault()}",
                    NomeCriterio = "ValorDeposito",
                    NomeCriterioFormatado = "Valor Depósito"
                });
            }
            if (filtroDTO.ValorBloqueioInicio.GetValueOrDefault() >= 0 && filtroDTO.ValorBloqueioFim.GetValueOrDefault() > 0)
            {
                listaCriterios.Add(new CriterioSaldoGarantia
                {
                    Criterio = @"
	                            ((cg.tsgar_cod_tipo_saldo_garantia = 2 and cg.valor_multiplicador = 1 and    valor_principal between :VAL_BLOQUEIO_INICIO and :VAL_BLOQUEIO_FIM) 
		                            or (cg.tsgar_cod_tipo_saldo_garantia <> 2))
                        ",
                    Parametros = ":VAL_BLOQUEIO_INICIO;:VAL_BLOQUEIO_FIM",
                    ValoresParametros = $"{filtroDTO.ValorBloqueioInicio.GetValueOrDefault()};{filtroDTO.ValorBloqueioFim.GetValueOrDefault()}",
                    NomeCriterio = "ValorBloqueio",
                    NomeCriterioFormatado = "Valor Bloqueio"
                });
            }
            if (filtroDTO.UmBloqueio && filtroDTO.TipoGarantia?.Count() > 0)
            {
                listaCriterios.Add(new CriterioSaldoGarantia
                {
                    Criterio = $@"
                        exists ( 
			                select lp.cod_processo, count(1) 
			                from   jur.lancamento_processo lp2, jur.categoria_pagamento cp2,  jur.classes_garantias cg2 
			                where  lp2.cod_processo = lp.cod_processo  and    
					                lp2.cod_cat_pagamento = cp2.cod_cat_pagamento  and    
					                cp2.clgar_cod_classe_garantia = cg2.codigo  and    
					                cg2.tsgar_cod_tipo_saldo_garantia = 2 					                
			                group by jur.processo.cod_processo  
			                having count(1) = :UM_BLOQUEIO
			            )",
                    Parametros = ":UM_BLOQUEIO",
                    ValoresParametros = "1",
                    NomeCriterio = "UmBloqueio",
                    NomeCriterioFormatado = "Um bloqueio"
                });
            }
            if (!string.IsNullOrEmpty(filtroDTO.NumeroConta))
            {

                listaCriterios.Add(new CriterioSaldoGarantia
                {
                    Criterio = @"
                            upper(lp.num_conta) = :NUMERO_CONTA",
                    Parametros = ":NUMERO_CONTA",
                    ValoresParametros = filtroDTO.NumeroConta,
                    NomeCriterio = "NumeroConta",
                    NomeCriterioFormatado = "Numero Conta"
                });
            }

            if (filtroDTO.TipoGarantia?.Count() > 0)
            {
                listaCriterios.Add(new CriterioSaldoGarantia
                {
                    Criterio = "cg.tsgar_cod_tipo_saldo_garantia",
                    Parametros = ":TIPOS_SALDO_GARANTIAS",
                    ValoresParametros = String.Join(", ", filtroDTO.TipoGarantia),
                    NomeCriterio = "ListaTipoGarantia",
                    NomeCriterioFormatado = "Tipo Garantia"
                });
            }

            if (!string.IsNullOrEmpty(filtroDTO.NumeroAgencia))
            {

                listaCriterios.Add(new CriterioSaldoGarantia
                {
                    Criterio = @"
                        upper(lp.num_agencia) = :NUMERO_AGENCIA",
                    Parametros = ":NUMERO_AGENCIA",
                    ValoresParametros = filtroDTO.NumeroAgencia,
                    NomeCriterio = "NumeroAgencia",
                    NomeCriterioFormatado = "Número Agência"
                });
            }

            if (filtroDTO.ConsiderarMigrados.HasValue && filtroDTO.ConsiderarMigrados.Value.HasValor_S_N() == "S")
            {
                listaCriterios.Add(new CriterioSaldoGarantia
                {
                    Criterio = $@"
                        ( jur.processo.ind_migrado = :IND_MIGRADOS )",
                    Parametros = ":IND_MIGRADOS",
                    ValoresParametros = filtroDTO.ConsiderarMigrados.Value.Retorna_S_N(),
                    NomeCriterio = "ConsiderarMigrados",
                    NomeCriterioFormatado = "Considerar Migrados"
                });
            }

            if (filtroDTO.IdsBanco != null && filtroDTO.IdsBanco.Count() > 0)
            {
                listaCriterios.Add(new CriterioSaldoGarantia
                {
                    Criterio = "lp.bco_cod_banco",
                    Parametros = ":BANCOS",
                    ValoresParametros = string.Join(",", filtroDTO.IdsBanco),
                    NomeCriterio = "ListaBancos",
                    NomeCriterioFormatado = "Lista Bancos"
                });
            }
            if (filtroDTO.IdsEmpresaGrupo != null && filtroDTO.IdsEmpresaGrupo.Count() > 0)
            {
                listaCriterios.Add(new CriterioSaldoGarantia
                {
                    Criterio = "nvl(jur.processo.cod_parte_empresa, 0)",
                    Parametros = ":EMPRESAS",
                    ValoresParametros = string.Join(",", filtroDTO.IdsEmpresaGrupo),
                    NomeCriterio = "ListaEmpresas",
                    NomeCriterioFormatado = "Lista Empresas"
                });
            }
            if (filtroDTO.IdsEstado != null && filtroDTO.IdsEstado.Count() > 0)
            {
                listaCriterios.Add(new CriterioSaldoGarantia
                {
                    Criterio = filtroDTO.TipoProcesso != TipoProcessoEnum.TributarioAdministrativo.GetHashCode() ? "nvl(jur.comarca.cod_estado, '*')" : "nvl( jur.processo.cod_estado, '*' )",
                    Parametros = ":ESTADO",
                    ValoresParametros = string.Join(",", filtroDTO.IdsEstado),
                    NomeCriterio = "ListaEstados",
                    NomeCriterioFormatado = "Lista Estados"
                });
            }
            if (filtroDTO.IdsProcesso != null && filtroDTO.IdsProcesso.Count() > 0)
            {
                listaCriterios.Add(new CriterioSaldoGarantia
                {
                    Criterio = "jur.processo.cod_processo",
                    Parametros = ":IDS_PROCESSOS",
                    ValoresParametros = string.Join(",", filtroDTO.IdsProcesso),
                    NomeCriterio = "ListaProcessos",
                    NomeCriterioFormatado = "Lista Processos"
                });
            }
            if (filtroDTO.RiscoPerda?.Count() > 0 &&
                        agendamento.CodigoTipoProcesso != TipoProcessoEnum.CivelEstrategico.GetHashCode() &&
                        agendamento.CodigoTipoProcesso != TipoProcessoEnum.TributarioAdministrativo.GetHashCode() &&
                        agendamento.CodigoTipoProcesso != TipoProcessoEnum.TributarioJudicial.GetHashCode()
                )
            {
                List<string> riscosPerdas = new List<string>();
                foreach (var rp in filtroDTO.RiscoPerda)
                {
                    if (rp.Value == (long)RiscoPerdaEnum.PR) riscosPerdas.Add(RiscoPerdaEnum.PR.Descricao());
                    if (rp.Value == (long)RiscoPerdaEnum.PO) riscosPerdas.Add(RiscoPerdaEnum.PO.Descricao());
                    if (rp.Value == (long)RiscoPerdaEnum.RE) riscosPerdas.Add(RiscoPerdaEnum.RE.Descricao());
                }
                var strCriterio = "";
                if (agendamento.CodigoTipoProcesso == TipoProcessoEnum.Trabalhista.GetHashCode())
                    strCriterio = $@"
                        (  
		                    exists ( 
			                    select 1 
			                    from   jur.parte_pedido_processo ppp1 
			                    where  ppp1.cod_processo = lp.cod_processo  
				                    and   ppp1.cod_risco_perda in ({riscosPerdas.ListToItensInString<string>()})
		                    )
	                    )                            
                    ";
                else
                    strCriterio = $@"
                        jur.processo.cod_risco_perda in ({riscosPerdas.ListToItensInString<string>()})
                    ";
                listaCriterios.Add(new CriterioSaldoGarantia
                {
                    Criterio = strCriterio,
                    Parametros = "*",
                    ValoresParametros = string.Join(",", riscosPerdas),
                    NomeCriterio = "ListaRiscosPerdas",
                    NomeCriterioFormatado = "Risco Perda"
                });
            }

            // Inserção dos critérios
            foreach (CriterioSaldoGarantia criterio in listaCriterios)
            {
                criterio.CodigoAgendamento = agendamento.Id;
                await Inserir(criterio);
            }
        }
        public async Task<ICollection<CriterioSaldoGarantia>> RecuperarPorAgendamento(long codigoAgendamento)
        {
            return await dbContext.CriterioSaldoGarantias
                        .AsNoTracking()
                        .Where(cs => cs.CodigoAgendamento == codigoAgendamento)
                        .ToListAsync();
        } 
    }
}

