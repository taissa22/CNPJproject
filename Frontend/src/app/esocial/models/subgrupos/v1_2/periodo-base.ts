export class PeriodoBase {
    constructor(
        idEsF2501Calctrib: number,
        idEsF2501: number,
        logCodUsuario: string,
        logDataOperacao: Date,
        calctribPerref: string,
        calctribVrbccpmensal: number,
        calctribVrbccp13: number,
    ) {

        this.idEsF2501Calctrib = idEsF2501Calctrib,
            this.idEsF2501 = idEsF2501,
            this.logCodUsuario = logCodUsuario,
            this.logDataOperacao = logDataOperacao,
            this.calctribPerref = calctribPerref,
            this.calctribVrbccpmensal = calctribVrbccpmensal,
            this.calctribVrbccp13 = calctribVrbccp13
    }


    readonly idEsF2501Calctrib: number;
    readonly idEsF2501: number;
    readonly logCodUsuario: string;
    readonly logDataOperacao: Date;
    readonly calctribPerref: string;
    readonly calctribVrbccpmensal: number;
    readonly calctribVrbccp13: number;
    readonly calctribVrrendirrf: number;
    readonly calctribVrrendirrf13: number;

    static fromObj(item: any) {
        return new PeriodoBase(
            item.idEsF2501Calctrib,
            item.idEsF2501,
            item.logCodUsuario,
            item.logDataOperacao,
            item.calctribPerref,
            item.calctribVrbccpmensal,
            item.calctribVrbccp13,
        );
    }

}
