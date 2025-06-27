export class EsF2501Response {

    private constructor(
        idF2501: number,
        codProcesso: number,
        codParte: number,
        statusFormulario: number,
        logCodUsuario: string,
        logDataOperacao: Date,
        evtcontprocId: string,
        ideeventoIndretif: number,
        ideempregadorTpinsc: number,
        ideempregadorNrinsc: string,
        ideprocNrproctrab: string,
        ideprocPerapurpgto: string,
        ideprocObs: string,
        parentIdF2501: number,
        idetrabCpftrab: string,
        ideeventoNrrecibo: string,
        exclusaoNrrecibo: string,
        infoIrComplemDtLaudo: Date,
        okSemRecibo: string
    ) {
        this.idF2501 = idF2501;
        this.codProcesso = codProcesso;
        this.codParte = codParte;
        this.statusFormulario = statusFormulario;
        this.logCodUsuario = logCodUsuario;
        this.logDataOperacao = logDataOperacao;
        this.evtcontprocId = evtcontprocId;
        this.ideeventoIndretif = ideeventoIndretif;
        this.ideempregadorTpinsc = ideempregadorTpinsc;
        this.ideempregadorNrinsc = ideempregadorNrinsc;
        this.ideprocNrproctrab = ideprocNrproctrab;
        this.ideprocPerapurpgto = ideprocPerapurpgto;
        this.ideprocObs = ideprocObs;
        this.parentIdF2501 = parentIdF2501;
        this.idetrabCpftrab = idetrabCpftrab;
        this.ideeventoNrrecibo = ideeventoNrrecibo;
        this.exclusaoNrrecibo = exclusaoNrrecibo;
        this.infoIrComplemDtLaudo = infoIrComplemDtLaudo;
        this.okSemRecibo = okSemRecibo
        
    }

    readonly idF2501: number;
    readonly codProcesso: number;
    readonly codParte: number;
    readonly statusFormulario: number;
    readonly logCodUsuario: string;
    readonly logDataOperacao: Date;
    readonly evtcontprocId: string;
    readonly ideeventoIndretif: number;
    readonly ideempregadorTpinsc: number;
    readonly ideempregadorNrinsc: string;
    readonly ideprocNrproctrab: string;
    readonly ideprocPerapurpgto: string;
    readonly ideprocObs: string;
    readonly parentIdF2501: number;
    readonly idetrabCpftrab: string;
    readonly ideeventoNrrecibo: string;
    readonly exclusaoNrrecibo: string;
    readonly infoIrComplemDtLaudo: Date;
    readonly okSemRecibo: string;

    static fromObj(obj: any): EsF2501Response {
        return new EsF2501Response(
            obj.idF2501,
            obj.codProcesso,
            obj.codParte,
            obj.statusFormulario,
            obj.logCodUsuario,
            obj.logDataOperacao,
            obj.evtcontprocId,
            obj.ideeventoIndretif,
            obj.ideempregadorTpinsc,
            obj.ideempregadorNrinsc,
            obj.ideprocNrproctrab,
            obj.ideprocPerapurpgto,
            obj.ideprocObs,
            obj.parentIdF2501,
            obj.idetrabCpftrab,
            obj.ideeventoNrrecibo,
            obj.exclusaoNrrecibo,
            obj.infoircomplemDtlaudo,
            obj.okSemRecibo
        );
    }
}
