export class EsF2500HeaderResponse {
    private constructor(
        codProcesso: number,
        nroProcessoCartorio: number,
        nomeComarca: string,
        nomeVara: string,
        ufVara: string,
        indAtivo: string,
        nomeEmpresaGrupo: string,
        indProprioTerceiro: string,
        codParte: number,
        nomeParte: string,
        cpfParte: number,
        statusFormulario: number,
        logCodUsuario: string,
        nomeUsuario: string,
        logDataOperacao: Date,
        logDataOperacaoFormatada: string,
        ideeventoNrrecibo: string,
        okSemRecibo: string
    ) {
        this.codProcesso = codProcesso;
        this.nroProcessoCartorio = nroProcessoCartorio;
        this.nomeComarca = nomeComarca;
        this.nomeVara = nomeVara;
        this.ufVara = ufVara;
        this.indAtivo = indAtivo;
        this.nomeEmpresaGrupo = nomeEmpresaGrupo;
        this.indProprioTerceiro = indProprioTerceiro;
        this.codParte = codParte;
        this.nomeParte = nomeParte;
        this.cpfParte = cpfParte;
        this.statusFormulario = statusFormulario;
        this.logCodUsuario = logCodUsuario;
        this.nomeUsuario = nomeUsuario;
        this.logDataOperacao = logDataOperacao;
        this.logDataOperacaoFormatada = logDataOperacaoFormatada;
        this.ideeventoNrrecibo = ideeventoNrrecibo;
        this.okSemRecibo = okSemRecibo;
    }

    readonly codProcesso: number;
    readonly nroProcessoCartorio: number;
    readonly nomeComarca: string;
    readonly nomeVara: string;
    readonly ufVara: string;
    readonly indAtivo: string;
    readonly nomeEmpresaGrupo: string;
    readonly indProprioTerceiro: string;
    readonly codParte: number;
    readonly nomeParte: string;
    readonly cpfParte: number;
    readonly statusFormulario: number;
    readonly logCodUsuario: string;
    readonly nomeUsuario: string;
    readonly logDataOperacao: Date;
    readonly logDataOperacaoFormatada: string;
    readonly ideeventoNrrecibo: string;
    readonly okSemRecibo: string;

    static fromObj(obj: any): EsF2500HeaderResponse {
        return new EsF2500HeaderResponse(
            obj.codProcesso,
            obj.nroProcessoCartorio,
            obj.nomeComarca,
            obj.nomeVara,
            obj.ufVara,
            obj.indAtivo,
            obj.nomeEmpresaGrupo,
            obj.indProprioTerceiro,
            obj.codParte,
            obj.nomeParte,
            obj.cpfParte,
            obj.statusFormulario,
            obj.logCodUsuario,
            obj.nomeUsuario,
            obj.logDataOperacao,
            new Date(obj.logDataOperacao).toLocaleString().replace(',', ''),            
            obj.ideeventoNrrecibo,
            obj.okSemRecibo
        );
    }
}
