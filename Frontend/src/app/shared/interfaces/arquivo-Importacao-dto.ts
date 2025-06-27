export interface ArquivoImportacaodto {
  numeroLoteBB: number;
  status: string;
  tipoProcesso: string;
  dataRemessa: Date,
  dataProcessamentoRemessa: string,
  quantidadeRegistrosProcessados: number,
  quantidadeRegistrosArquivo: number,
  valorTotalRemessa: number,
  valorTotalGuiaProcessada: number
}
