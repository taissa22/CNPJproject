export class Imposto {
    constructor(
        idEsF2501Infocrirrf: number,
        logDataOperacao: Date,
        logCodUsuario: string,
        infocrcontribTpcr: number,
        infocrcontribVrcr: number,
        descricaoTpcr: string
    ) {
        this.idEsF2501Infocrirrf = idEsF2501Infocrirrf;
        this.logDataOperacao = logDataOperacao;
        this.logCodUsuario = logCodUsuario;
        this.infocrcontribTpcr = infocrcontribTpcr;
        this.infocrcontribVrcr = infocrcontribVrcr;
        this.descricaoTpcr = descricaoTpcr;
    }

    readonly idEsF2501Infocrirrf: number;
    readonly logDataOperacao: Date;
    readonly logCodUsuario: string;
    readonly infocrcontribTpcr: number;
    readonly infocrcontribVrcr: number;
    readonly descricaoTpcr: string;

    static fromObj(item: any) {
        return new Imposto(
            item.idEsF2501Infocrirrf,
            item.logDataOperacao,
            item.logCodUsuario,
            item.infocrcontribTpcr,
            item.infocrcontribVrcr,
            item.descricaoTpcr
        );
    }

}
