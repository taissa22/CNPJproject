export interface LoteFiltroDTO {
  dataCriacaoPedidoMaior?: string;
  dataCriacaoPedidoMenor?: string;
  dataCriacaoMaior?: string;
  dataCriacaoMenor?: string;
  tipoProcesso: number;
  statusContabil?: number;
  statusProcesso?: number;
  dataCancelamentoLoteInicio?: string;
  dataCancelamentoLoteFim?: string;
  dataErroProcessamentoInicio?: string;
  dataErroProcessamentoFim?: string;
  dataRecebimentoFiscalInicio?: string;
  dataRecebimentoFiscalFim?: string;
  dataPagamentoPedidoInicio?: string;
  dataPagamentoPedidoFim?: string;
  dataEnvioEscritorioInicio?: string;
  dataEnvioEscritorioFim?: string;
  numeroProcesso?:number[];
  valorTotalLoteInicio?: number;
  valorTotalLoteFim?: number;
  idsProcessos?: number[];
  idsEmpresasGrupo?: number[];
  idsEscritorios?: number[];
  idsFornecedores?: number[];
  idsCentroCustos?: number[];
  idsTipoLancamentos?: number[];
  idsCategoriasPagamentos?: number[];
  idsStatusPagamentos?: number[];
  idsNumerosGuia?: number[];
  idsNumerosLote?: number[];
  idsPedidosSAP?: number[];
  numeroContaJudicial?: number[];
  id?: number;
  pagina: number;
  quantidade: number;
  total?: number;

}
