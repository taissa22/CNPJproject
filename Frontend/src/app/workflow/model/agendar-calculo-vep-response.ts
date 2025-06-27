export class AgendarCalculoVepResponse {
    cod: number;
    datAgendamento: Date;
    datInicioExecucao: Date;
    datFimExecucao: Date;
    datProximaExecucao: Date;
    usrCodUsuario: string;
    periodicidadeExecucao: number;
    numMeses: number;
    indUltimoDiaDoMes: string;
    diaDoMes: number;
    status: number;
}