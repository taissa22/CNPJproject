export class RelatorioPagamentoEscritorioResponse {
    cod: number;
    datAgendamento: Date;
    datInicioExecucao: Date;
    datFimExecucao: Date;
    datProximaExecucao: Date;
    usrCodUsuario: string;
    periodicidadeExecucao: number;
    mesReferencia: number;
    diaDoMes: number;
    status: number;
}