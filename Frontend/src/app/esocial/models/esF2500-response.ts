export class EsF2500Response {

    private constructor(
        idF2500: number,
        codProcesso: number,
        codParte: number,
        statusFormulario: number,
        logCodUsuario: string,
        logDataOperacao: Date,
        evtproctrabId: string,
        ideeventoIndretif: number,
        ideeventoNrrecibo: string,
        ideeventoTpamb: number,
        ideeventoProcemi: number,
        ideeventoVerproc: string,
        ideempregadorTpinsc: number,
        ideempregadorNrinsc: string,
        iderespTpinsc: number,
        iderespNrinsc: string,
        infoprocessoOrigem: number,
        infoprocessoNrproctrab: string,
        infoprocessoObsproctrab: string,
        infoprocjudDtsent: Date,
        infoprocjudUfvara: string,
        infoprocjudCodmunic: number,
        infoprocjudIdvara: number,
        infoccpDtccp: Date,
        infoccpTpccp: number,
        infoccpCnpjccp: string,
        idetrabCpftrab: string,
        idetrabNmtrab: string,
        idetrabDtnascto: Date,
        IderespDtadmrespdir: Date,
        iderespMatrespdir: string,
        infocomplCodcbo: string,
        infocomplNatatividade: number,
        infovincTpregtrab: number,
        infovincTpregprev: number,
        infovincDtadm: Date,
        infovincTmpparc: number,
        duracaoTpcontr: number,
        duracaoDtterm: Date,
        duracaoClauassec: string,
        duracaoObjdet: string,
        sucessaovincTpinsc: number,
        sucessaovincNrinsc: string,
        sucessaovincMatricant: string,
        sucessaovincDttransf: Date,
        infodesligDtdeslig: Date,
        infodesligMtvdeslig: string,
        infodesligDtprojfimapi: Date,
        infotermDtterm: Date,
        infotermMtvdesligtsv: string,
        ideestabTpinsc: number,
        ideestabNrinsc: string,
        infovlrCompini: string,
        infovlrCompfim: string,
        infovlrRepercproc: number,
        infovlrVrremun: number,
        infovlrVrapi: number,
        infovlrVr13api: number,
        infovlrVrinden: number,
        infovlrVrbaseindenfgts: number,
        infovlrPagdiretoresc: string,
        exclusaoNrrecibo: string,
        okSemRecibo: string,
    ) {
        this.idF2500 = idF2500;
        this.codProcesso = codProcesso;
        this.codParte = codParte;
        this.statusFormulario = statusFormulario;
        this.logCodUsuario = logCodUsuario;
        this.logDataOperacao = logDataOperacao;
        this.evtproctrabId = evtproctrabId;
        this.ideeventoIndretif = ideeventoIndretif;
        this.ideeventoNrrecibo = ideeventoNrrecibo;
        this.ideeventoTpamb = ideeventoTpamb;
        this.ideeventoProcemi = ideeventoProcemi;
        this.ideeventoVerproc = ideeventoVerproc;
        this.ideempregadorTpinsc = ideempregadorTpinsc;
        this.ideempregadorNrinsc = ideempregadorNrinsc;
        this.iderespTpinsc = iderespTpinsc;
        this.iderespNrinsc = iderespNrinsc;
        this.infoprocessoOrigem = infoprocessoOrigem;
        this.infoprocessoNrproctrab = infoprocessoNrproctrab;
        this.infoprocessoObsproctrab = infoprocessoObsproctrab;
        this.infoprocjudDtsent = infoprocjudDtsent;
        this.infoprocjudUfvara = infoprocjudUfvara;
        this.infoprocjudCodmunic = infoprocjudCodmunic;
        this.infoprocjudIdvara = infoprocjudIdvara;
        this.infoccpDtccp = infoccpDtccp;
        this.infoccpTpccp = infoccpTpccp;
        this.infoccpCnpjccp = infoccpCnpjccp;
        this.idetrabCpftrab = idetrabCpftrab;
        this.idetrabNmtrab = idetrabNmtrab;
        this.idetrabDtnascto = idetrabDtnascto;
        this.iderespDtadmrespdir = IderespDtadmrespdir;
        this.iderespMatrespdir = iderespMatrespdir;
        this.infocomplCodcbo = infocomplCodcbo;
        this.infocomplNatatividade = infocomplNatatividade;
        this.infovincTpregtrab = infovincTpregtrab;
        this.infovincTpregprev = infovincTpregprev;
        this.infovincDtadm = infovincDtadm;
        this.infovincTmpparc = infovincTmpparc;
        this.duracaoTpcontr = duracaoTpcontr;
        this.duracaoDtterm = duracaoDtterm;
        this.duracaoClauassec = duracaoClauassec;
        this.duracaoObjdet = duracaoObjdet;
        this.sucessaovincTpinsc = sucessaovincTpinsc;
        this.sucessaovincNrinsc = sucessaovincNrinsc;
        this.sucessaovincMatricant = sucessaovincMatricant;
        this.sucessaovincDttransf = sucessaovincDttransf;
        this.infodesligDtdeslig = infodesligDtdeslig;
        this.infodesligMtvdeslig = infodesligMtvdeslig;
        this.infodesligDtprojfimapi = infodesligDtprojfimapi;
        this.infotermDtterm = infotermDtterm;
        this.infotermMtvdesligtsv = infotermMtvdesligtsv;
        this.ideestabTpinsc = ideestabTpinsc;
        this.ideestabNrinsc = ideestabNrinsc;
        this.infovlrCompini = infovlrCompini;
        this.infovlrCompfim = infovlrCompfim;
        this.infovlrRepercproc = infovlrRepercproc;
        this.infovlrVrremun = infovlrVrremun;
        this.infovlrVrapi = infovlrVrapi;
        this.infovlrVr13api = infovlrVr13api;
        this.infovlrVrinden = infovlrVrinden;
        this.infovlrVrbaseindenfgts = infovlrVrbaseindenfgts;
        this.infovlrPagdiretoresc = infovlrPagdiretoresc;
        this.exclusaoNrrecibo = exclusaoNrrecibo;
        this.okSemRecibo = okSemRecibo;
    }

    readonly idF2500: number;
    readonly codProcesso: number;
    readonly codParte: number;
    readonly statusFormulario: number;
    readonly logCodUsuario: string;
    readonly logDataOperacao: Date;
    readonly evtproctrabId: string;
    readonly ideeventoIndretif: number;
    readonly ideeventoNrrecibo: string;
    readonly ideeventoTpamb: number;
    readonly ideeventoProcemi: number;
    readonly ideeventoVerproc: string;
    readonly ideempregadorTpinsc: number;
    readonly ideempregadorNrinsc: string;
    readonly iderespTpinsc: number;
    readonly iderespNrinsc: string;
    readonly infoprocessoOrigem: number;
    readonly infoprocessoNrproctrab: string;
    readonly infoprocessoObsproctrab: string;
    readonly infoprocjudDtsent: Date;
    readonly infoprocjudUfvara: string;
    readonly infoprocjudCodmunic: number;
    readonly infoprocjudIdvara: number;
    readonly infoccpDtccp: Date;
    readonly infoccpTpccp: number;
    readonly infoccpCnpjccp: string;
    readonly idetrabCpftrab: string;
    readonly idetrabNmtrab: string;
    readonly idetrabDtnascto: Date;
    readonly iderespDtadmrespdir: Date;
    readonly iderespMatrespdir: string;
    readonly infocomplCodcbo: string;
    readonly infocomplNatatividade: number;
    readonly infovincTpregtrab: number;
    readonly infovincTpregprev: number;
    readonly infovincDtadm: Date;
    readonly infovincTmpparc: number;
    readonly duracaoTpcontr: number;
    readonly duracaoDtterm: Date;
    readonly duracaoClauassec: string;
    readonly duracaoObjdet: string;
    readonly sucessaovincTpinsc: number;
    readonly sucessaovincNrinsc: string;
    readonly sucessaovincMatricant: string;
    readonly sucessaovincDttransf: Date;
    readonly infodesligDtdeslig: Date;
    readonly infodesligMtvdeslig: string;
    readonly infodesligDtprojfimapi: Date;
    readonly infotermDtterm: Date;
    readonly infotermMtvdesligtsv: string;
    readonly ideestabTpinsc: number;
    readonly ideestabNrinsc: string;
    readonly infovlrCompini: string;
    readonly infovlrCompfim: string;
    readonly infovlrRepercproc: number;
    readonly infovlrVrremun: number;
    readonly infovlrVrapi: number;
    readonly infovlrVr13api: number;
    readonly infovlrVrinden: number;
    readonly infovlrVrbaseindenfgts: number;
    readonly infovlrPagdiretoresc: string;
    readonly exclusaoNrrecibo: string;
    readonly okSemRecibo: string;

    static fromObj(obj: any): EsF2500Response {
        return new EsF2500Response(
            obj.idF2500,
            obj.codProcesso,
            obj.codParte,
            obj.statusFormulario,
            obj.logCodUsuario,
            obj.logDataOperacao,
            obj.evtproctrabId,
            obj.ideeventoIndretif,
            obj.ideeventoNrrecibo,
            obj.ideeventoTpamb,
            obj.ideeventoProcemi,
            obj.ideeventoVerproc,
            obj.ideempregadorTpinsc,
            obj.ideempregadorNrinsc,
            obj.iderespTpinsc,
            obj.iderespNrinsc,
            obj.infoprocessoOrigem,
            obj.infoprocessoNrproctrab,
            obj.infoprocessoObsproctrab,
            obj.infoprocjudDtsent,
            obj.infoprocjudUfvara,
            obj.infoprocjudCodmunic,
            obj.infoprocjudIdvara,
            obj.infoccpDtccp,
            obj.infoccpTpccp,
            obj.infoccpCnpjccp,
            obj.idetrabCpftrab,
            obj.idetrabNmtrab,
            obj.idetrabDtnascto,
            obj.IderespDtadmrespdir,
            obj.iderespMatrespdir,
            obj.infocomplCodcbo,
            obj.infocomplNatatividade,
            obj.infovincTpregtrab,
            obj.infovincTpregprev,
            obj.infovincDtadm,
            obj.infovincTmpparc,
            obj.duracaoTpcontr,
            obj.duracaoDtterm,
            obj.duracaoClauassec,
            obj.duracaoObjdet,
            obj.sucessaovincTpinsc,
            obj.sucessaovincNrinsc,
            obj.sucessaovincMatricant,
            obj.sucessaovincDttransf,
            obj.infodesligDtdeslig,
            obj.infodesligMtvdeslig,
            obj.infodesligDtprojfimapi,
            obj.infotermDtterm,
            obj.infotermMtvdesligtsv,
            obj.ideestabTpinsc,
            obj.ideestabNrinsc,
            obj.infovlrCompini,
            obj.infovlrCompfim,
            obj.infovlrRepercproc,
            obj.infovlrVrremun,
            obj.infovlrVrapi,
            obj.infovlrVr13api,
            obj.infovlrVrinden,
            obj.infovlrVrbaseindenfgts,
            obj.infovlrPagdiretoresc,
            obj.exclusaoNrrecibo,
            obj.okSemRecibo
        );
    }
}
