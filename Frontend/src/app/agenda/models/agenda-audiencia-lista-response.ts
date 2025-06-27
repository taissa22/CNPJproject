export class AgendaAudienciaListaResponse {
    codProcesso: number;
    dateAudiencia: Date;
    horarioAudiencia: Date;
    tipoAudiencia: string;
    numeroProcesso: string;
    classificacaoHierarquica: string;
    descricaoClassificHierarquica: string;
    processoAtivo: string;
    escritorioProcesso: string;
    estado: string;
    comarca: string;
    codVara: number;
    codTipoVara: number;
    tipoVara: string;
    usuarioUltAteracao: string;
    dataUltAtualizacao: Date;
    classificacaoProcesso: string;
    descrcaoClassificacaoProcesso: string;
    estrategico: string;
    seqAudiencia: number;
    reclamantes: string;
    reclamadas: Array<Reclamadas>;
    descLocalidade: string;
    descModalidade: string;
}

class Reclamadas{
    id: number;
    descricao: string;
    codPreposto?: number
}