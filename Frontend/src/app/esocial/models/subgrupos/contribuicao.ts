export class Contribuicao {
    constructor(
        idEsF2501Infocrcontrib: number,
        idEsF2501Calctrib: number,
        logDataOperacao: Date,
        logCodUsuario: string,
        infocrcontribTpcr: number,
        infocrcontribVrcr: number,
        descricaoTpcr: string
    ) {
        this.idEsF2501Infocrcontrib = idEsF2501Infocrcontrib;
        this.idEsF2501Calctrib = idEsF2501Calctrib;
        this.logDataOperacao = logDataOperacao;
        this.logCodUsuario = logCodUsuario;
        this.infocrcontribTpcr = infocrcontribTpcr;
        this.infocrcontribVrcr = infocrcontribVrcr;
        this.descricaoTpcr = descricaoTpcr;
    }

    readonly idEsF2501Infocrcontrib: number;
    readonly idEsF2501Calctrib: number;
    readonly logDataOperacao: Date;
    readonly logCodUsuario: string;
    readonly infocrcontribTpcr: number;
    readonly infocrcontribVrcr: number;
    readonly descricaoTpcr: string;

    static fromObj(item: any) {
        return new Contribuicao(
            item.idEsF2501Infocrcontrib,
            item.idEsF2501Calctrib,
            item.logDataOperacao,
            item.logCodUsuario,
            item.infocrcontribTpcr,
            item.infocrcontribVrcr,
            item.descricaoTpcr
        );
    }

}
