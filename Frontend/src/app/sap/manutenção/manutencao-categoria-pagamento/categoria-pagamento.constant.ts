import { TipoProcessoEnum } from '@shared/enums/tipo-processoEnum.enum';
import { TipoLancamentoCategoriaPagamento } from '@shared/enums/tipo-lancamento-categoria-pagamento.enum';
import { IndicadoresTipoProcesso } from '../interface/ICategoriaPagamento';




// Esse é um arquivo de constantes, qualquer mudança será automaticamente
//refletida em onde é usado!


/**
 *Essa função retorna o número de tipo de processo pelo indicador marcado como true
 *
 * @export
 * @param {IndicadoresTipoProcesso} indicador - objeto de indicador
 * @returns
 */
export function mapearIndicadoresTipoProcessoParaNumero(indicador: IndicadoresTipoProcesso){

  if(indicador.indicadorAdministrativo) return TipoProcessoEnum.administrativo;
  if(indicador.indicadorCivelConsumidor)  return TipoProcessoEnum.civelConsumidor;
  if(indicador.indicadorCivelEstrategico)  return TipoProcessoEnum.civelEstrategico;
  if(indicador.indicadorJuizado)  return TipoProcessoEnum.juizadoEspecial;
  if(indicador.indicadorPex)  return TipoProcessoEnum.PEX;
  if(indicador.indicadorProcon)  return TipoProcessoEnum.procon;
  if(indicador.indicadorTrabalhista)  return TipoProcessoEnum.trabalhista;
  if(indicador.ind_TributarioAdministrativo)  return TipoProcessoEnum.tributarioAdministrativo;
  if(indicador.indicadorTributarioJudicial)  return TipoProcessoEnum.tributarioJudicial

}



 /**
   * Nome para ser adicionado na exportação da categoria de pagamento
   */
export function nomeExportacao(tipoSelecionado, tipoLancamento) {
  switch (tipoSelecionado) {

    case TipoProcessoEnum.civelConsumidor: {
      if (tipoLancamento
        === TipoLancamentoCategoriaPagamento.despesasJudiciais) {
        return 'Cat_Pag_Lanc_Des_Jud_CivCon';
        break;
      } else if (tipoLancamento
        === TipoLancamentoCategoriaPagamento.garantias) {
        return 'Cat_Pag_Lanc_Gar_CivCon';
        break;
      }
      if (tipoLancamento
        === TipoLancamentoCategoriaPagamento.honorários) {
        return 'Cat_Pag_Lanc_Hon_CivCon';
        break;
      }
      if (tipoLancamento ===
        TipoLancamentoCategoriaPagamento.pagamentos) {
        return 'Cat_Pag_Lanc_Pag_CivCon';
        break;
      }
    }
    case TipoProcessoEnum.civelEstrategico: {
      if (tipoLancamento ===
        TipoLancamentoCategoriaPagamento.despesasJudiciais) {
        return 'Cat_Pag_Lanc_Des_Jud_CivEst';
        break;
      }
      if (tipoLancamento ===
        TipoLancamentoCategoriaPagamento.garantias) {
        return 'Cat_Pag_Lanc_Gar_CivEst';
        break;
      }
      if (tipoLancamento ===
        TipoLancamentoCategoriaPagamento.honorários) {
        return 'Cat_Pag_Lanc_Hon_CivEst';
        break;
      }
      if (tipoLancamento ===
        TipoLancamentoCategoriaPagamento.pagamentos) {
        return 'Cat_Pag_Lanc_Pag_CivEst';
        break;
      }
    }
    case TipoProcessoEnum.juizadoEspecial: {
      if (tipoLancamento ===
        TipoLancamentoCategoriaPagamento.despesasJudiciais) {
        return 'Cat_Pag_Lanc_Des_Jud_Juiz';
        break;
      }
      if (tipoLancamento ===
        TipoLancamentoCategoriaPagamento.garantias) {
        return 'Cat_Pag_Lanc_Gar_Juiz';
        break;
      }
      if (tipoLancamento ===
        TipoLancamentoCategoriaPagamento.pagamentos) {
        return 'Cat_Pag_Lanc_Pag_Juiz';
        break;
      }
    }
    case TipoProcessoEnum.trabalhista: {
      if (tipoLancamento ===
        TipoLancamentoCategoriaPagamento.despesasJudiciais) {
        return 'Cat_Pag_Lanc_Des_Jud_Trab';
        break;
      }
      if (tipoLancamento ===
        TipoLancamentoCategoriaPagamento.garantias) {
        return 'Cat_Pag_Lanc_Gar_Trab';
        break;
      }
      if (tipoLancamento ===
        TipoLancamentoCategoriaPagamento.honorários) {
        return 'Cat_Pag_Lanc_Hon_Trab';
        break;
      }
      if (tipoLancamento ===
        TipoLancamentoCategoriaPagamento.pagamentos) {
        return 'Cat_Pag_Lanc_Pag_Trab';
        break;
      }
    }
    case TipoProcessoEnum.tributarioAdministrativo: {
      if (tipoLancamento ===
        TipoLancamentoCategoriaPagamento.despesasJudiciais) {
        return 'Cat_Pag_Lanc_Des_Jud_TriAdm';
        break;
      }
      if (tipoLancamento ===
        TipoLancamentoCategoriaPagamento.garantias) {
        return 'Cat_Pag_Lanc_Gar_TriAdm';
        break;
      }
      if (tipoLancamento ===
        TipoLancamentoCategoriaPagamento.honorários) {
        return 'Cat_Pag_Lanc_Hon_TriAdmn';
        break;
      }
      if (tipoLancamento ===
        TipoLancamentoCategoriaPagamento.pagamentos) {
        return 'Cat_Pag_Lanc_Pag_TriAdm';
        break;
      }
    }
    case TipoProcessoEnum.tributarioJudicial: {
      if (tipoLancamento ===
        TipoLancamentoCategoriaPagamento.despesasJudiciais) {
        return 'Cat_Pag_Lanc_Des_Jud_TriJud';
        break;
      }
      if (tipoLancamento ===
        TipoLancamentoCategoriaPagamento.garantias) {
        return 'Cat_Pag_Lanc_Gar_TriJud';
        break;
      }
      if (tipoLancamento ===
        TipoLancamentoCategoriaPagamento.honorários) {
        return 'Cat_Pag_Lanc_Hon_TriJud';
        break;
      }
      if (tipoLancamento ===
        TipoLancamentoCategoriaPagamento.pagamentos) {
        return 'Cat_Pag_Lanc_Pag_TriJud';
        break;
      }
    }
    case TipoProcessoEnum.administrativo: {
      if (tipoLancamento ===
        TipoLancamentoCategoriaPagamento.pagamentos) {
        return 'Cat_Pag_Lanc_Pag_Adm';
        break;
      }
      if (tipoLancamento ===
        TipoLancamentoCategoriaPagamento.recuperacaoPagamento) {
        return 'Cat_Pag_Lanc_Rec_Pag_Adm';
        break;
      }
    }
    case TipoProcessoEnum.procon: {
      if (tipoLancamento ===
        TipoLancamentoCategoriaPagamento.despesasJudiciais) {
        return 'Cat_Pag_Lanc_Des_Jud_Proc';
        break;
      }
      if (tipoLancamento ===
        TipoLancamentoCategoriaPagamento.pagamentos) {
        return 'Cat_Pag_Lanc_Pag_Proc';
        break;
      }
    }
    case TipoProcessoEnum.PEX: {
      if (tipoLancamento ===
        TipoLancamentoCategoriaPagamento.despesasJudiciais) {
        return 'Cat_Pag_Lanc_Des_Jud_Pex';
        break;
      }
      if (tipoLancamento ===
        TipoLancamentoCategoriaPagamento.garantias) {
        return 'Cat_Pag_Lanc_Gar_Pex';
        break;
      }
      if (tipoLancamento ===
        TipoLancamentoCategoriaPagamento.honorários) {
        return 'Cat_Pag_Lanc_Hon_Pex';
        break;
      }
      if (tipoLancamento ===
        TipoLancamentoCategoriaPagamento.pagamentos) {
        return 'Cat_Pag_Lanc_Pag_Pex';
        break;
      }
    }
  }
};

 /**
   * verifica qual o tipo de lançamento será mostrado por tipo de processo
   */
export const listaTipoProcesso = [{
  id: 2,
  descricao: 'Despesas Judiciais',
  tipoProcesso: [TipoProcessoEnum.PEX,
  TipoProcessoEnum.civelConsumidor,
  TipoProcessoEnum.civelEstrategico,
  TipoProcessoEnum.juizadoEspecial,
  TipoProcessoEnum.procon,
  TipoProcessoEnum.trabalhista,
  TipoProcessoEnum.tributarioAdministrativo,
  TipoProcessoEnum.tributarioJudicial]
}, {
  id: 1,
  descricao: 'Garantias',
  tipoProcesso: [TipoProcessoEnum.PEX,
  TipoProcessoEnum.civelConsumidor,
  TipoProcessoEnum.civelEstrategico,
  TipoProcessoEnum.juizadoEspecial,
  TipoProcessoEnum.trabalhista,
  TipoProcessoEnum.tributarioAdministrativo,
  TipoProcessoEnum.tributarioJudicial]
}, {
  id: 4,
  descricao: 'Honorários',
  tipoProcesso: [TipoProcessoEnum.PEX,
  TipoProcessoEnum.civelConsumidor,
  TipoProcessoEnum.civelEstrategico,
  TipoProcessoEnum.trabalhista,
  TipoProcessoEnum.tributarioAdministrativo,
  TipoProcessoEnum.tributarioJudicial]
}, {
  id: 3,
  descricao: 'Pagamentos',
  tipoProcesso: [TipoProcessoEnum.PEX,
  TipoProcessoEnum.civelConsumidor,
  TipoProcessoEnum.civelEstrategico,
  TipoProcessoEnum.juizadoEspecial,
  TipoProcessoEnum.procon,
  TipoProcessoEnum.trabalhista,
  TipoProcessoEnum.tributarioAdministrativo,
  TipoProcessoEnum.tributarioJudicial,
  TipoProcessoEnum.administrativo]
},
{
  id: 5,
  descricao: 'Recuperação de Pagamento',
  tipoProcesso: [TipoProcessoEnum.administrativo]
}

];

 /**
   * lista de fornecedoresPermitidos
   */
export const fornecedoresPermitidos = [
  {
    id: 1,
    descricao: 'BANCO'
  },
  {
    id: 2,
    descricao: 'ESCRITÓRIO/PROFISSIONAL'
  },
  {
    id: 3,
    descricao: 'TODOS'
  }
];

 /**
   * Valores referentes a grid de manutenção de categoria de pagamento,
   * ela diferente por tipo de processo e tipo de lançamento
   */
export const headerGrid = [{
  listaGridCivelConsumidorDespesasJudiciais: ['codigo', 'descricao', 'indAtivo'
    , 'codigoMaterialSAP', 'indEnvioSap', 'indicadorNumeroGuia', 'indicadorFinalizacaoContabil'
    , 'fornecedoresPermitidos', 'descricaoEstrategico']
},
{
  listaGridCivelConsumidorGarantias: ['codigo', 'descricao', 'indAtivo'
    , 'codigoMaterialSAP', 'indEnvioSap', 'descricaoclassegarantia', 'grupoCorrecao',
    'indicadorFinalizacaoContabil', 'descricaoEstrategico']
},
{
  listaGridCivelConsumidorHonorarios: ['codigo', 'descricao', 'indAtivo',
    'indicadorFinalizacaoContabil', 'descricaoEstrategico']
},
{
  listaGridCivelConsumidorPagamentos: ['codigo', 'descricao', 'indAtivo',
    'codigoMaterialSAP', 'indicadorContingencia', 'indEnvioSap', 'descricaoclassegarantia', 'indEncerraProcessoContabil',
    'fornecedoresPermitidos', 'descricaoJustificativa', 'responsabilidadeOi', 'descricaoEstrategico']
},
{
  listaGridCivelEstrategicoDespesasJudiciais: ['codigo', 'descricao', 'indAtivo',
    'codigoMaterialSAP', 'indEnvioSap', 'descricaoConsumidor']
},
{
  listaGridCivelEstrategicoGarantias: ['codigo', 'descricao', 'indAtivo',
    'codigoMaterialSAP', 'indEnvioSap', 'descricaoclassegarantia', 'descricaoConsumidor']
},
{
  listaGridCivelEstrategicoHonorarios: ['codigo', 'descricao', 'indAtivo', 'descricaoConsumidor']
},
{
  listaGridCivelEstrategicoPagamentos: ['codigo', 'descricao', 'indAtivo',
    'codigoMaterialSAP', 'indEnvioSap', 'descricaoclassegarantia', 'descricaoConsumidor']
},
{
  listaGridJuizadoEspecialDespesasJudiciais: ['codigo', 'descricao', 'indAtivo',
    'indicadorFinalizacaoContabil', 'codigoMaterialSAP', 'indEnvioSap', 'indicadorNumeroGuia',
    'fornecedoresPermitidos']
},
{
  listaGridJuizadoEspecialGarantias: ['codigo', 'descricao', 'indAtivo', 'indicadorFinalizacaoContabil',
    'codigoMaterialSAP', 'indEnvioSap', 'descricaoclassegarantia', 'grupoCorrecao']
},
{
  listaGridJuizadoEspecialPagamentos: ['codigo', 'descricao', 'indAtivo',
    'indicadorFinalizacaoContabil', 'indicadorContingencia', 'codigoMaterialSAP', 'indEnvioSap', 'descricaoclassegarantia',
     'descricaoJustificativa', 'fornecedoresPermitidos', 'responsabilidadeOi'
  ]
},
{
  listaGridTrabalhistaDespesasJudiciais: ['codigo', 'descricao', 'indAtivo',
    'indicadorFinalizacaoContabil', 'codigoMaterialSAP', 'indEnvioSap']
},
{
  listaGridTrabalhistaGarantias: ['codigo', 'descricao', 'indAtivo',
    'indicadorFinalizacaoContabil','indicadorContingencia', 'codigoMaterialSAP', 'indEnvioSap', 'descricaoclassegarantia',
    'grupoCorrecao']
},
{
  listaGridTrabalhistaHonorarios: ['codigo', 'descricao', 'indAtivo',
    'indicadorFinalizacaoContabil']
},
{
  listaGridTrabalhistaPagamentos: ['codigo', 'descricao', 'indAtivo',
    'codigoMaterialSAP' ,'indicadorContingencia',
     'indEnvioSap', 'descricaoclassegarantia', 'indicadorHistorico', 'responsabilidadeOi'
  ]
},
{
  listaGridTributarioAdministrativoDespesasJudiciais: ['codigo', 'descricao', 'indAtivo']
},
{
  listaGridTributarioAdministrativoGarantias: ['codigo', 'descricao', 'indAtivo'
    , 'descricaoclassegarantia']
},
{
  listaGridTributarioAdministrativoHonorarios: ['codigo', 'descricao', 'indAtivo']

},
{
  listaGridTributarioAdministrativoPagamentos: ['codigo', 'descricao', 'indAtivo'
    , 'descricaoclassegarantia']
},
{
  listaGridTributarioJudicialDespesasJudiciais: ['codigo', 'descricao', 'indAtivo']

},
{
  listaGridTributarioJudicialGarantias: ['codigo', 'descricao', 'indAtivo'
    , 'descricaoclassegarantia']
},
{
  listaGridTributarioJudicialHonorarios: ['codigo', 'descricao', 'indAtivo']

},
{
  listaGridTributarioJudicialPagamentos: ['codigo', 'descricao', 'indAtivo',
    'descricaoclassegarantia']
},
{

  listaGridAdministrativoPagamentos: ['codigo', 'descricao', 'indAtivo',
    'indicadorNumeroGuia']
},
{
  listaGridAdministrativoRecuperacaodePagamento: ['codigo', 'descricao', 'indAtivo',
    'indicadorNumeroGuia']
},
{
  listaGridProconDespesasJudiciais: ['codigo', 'descricao', 'indAtivo',
    'indicadorFinalizacaoContabil', 'indicadorNumeroGuia']
},
{
  listaGridProconPagamentos: ['codigo', 'descricao', 'indAtivo',
    'indicadorFinalizacaoContabil']
},
{
  listaGridPEXDespesasJudiciais: ['codigo', 'descricao', 'indAtivo',
    'indicadorFinalizacaoContabil', 'codigoMaterialSAP', 'indEnvioSap', 'indicadorNumeroGuia',
    'indEscritorioSolicitaLan']
},
{
  listaGridPEXGarantias: ['codigo', 'descricao', 'indAtivo',
    'indicadorFinalizacaoContabil', 'descricaoclassegarantia', 'grupoCorrecao',
    'codigoMaterialSAP', 'indEnvioSap', 'indEscritorioSolicitaLan'
  ]
},
{
  listaGridPEXHonorarios: ['codigo', 'descricao', 'indAtivo',
    'indicadorFinalizacaoContabil', 'indEscritorioSolicitaLan']
},
{
  listaGridPEXPagamentos: ['codigo', 'descricao', 'indAtivo',
    'descricaoclassegarantia', 'codigoMaterialSAP', 'indEnvioSap',
    'indicadorContingencia','descricaoJustificativa','indicadorRequerDataVencimento', 'indComprovanteSolicitacao',
    'indEscritorioSolicitaLan']
}];
