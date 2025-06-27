export class AgendaAudienciaBuscaRequest {
    dataAudienciaDe: string;
    dataAudienciaAte: string;
    classificHierarUnico: boolean;
    classificHierarPrimario: boolean;
    classificHierarSecundario: boolean;
    classificProcessoPrimario: boolean;
    classificProcessoTerceiro: boolean;
    estado: Array<string>;
    tipoAudiencia: Array<number>;
    processoEstrategico: number;
    estadoSelecionado : string;
}