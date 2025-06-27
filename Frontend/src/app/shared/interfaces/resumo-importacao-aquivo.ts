export interface ResumoImportacaoAquivo {
    tipoProcesso?: string;
    numeroLoteBB?: number;
    dataRemessa?: string;
    dataProcessamentoRemessa?: string;
    status?: string;
    quantidadeRegistrosArquivo?: number;
    valorTotalRemessa?: number;
    quantidadeRegistrosProcessados?: number;
    valorTotalGuiaProcessada?: number;
}
