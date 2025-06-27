import { StatusFormularioEsocial } from "../enum/status-formulario";

export class Formulario2500 {
    private constructor(codProcesso: number, idF2500: number, codParte: number,
      statusFormulario: number, logCodUsuario: string, nomeUsuario: string,
      logDataOperacao: Date, historico: Array<string>, finalizadoEscritorio:boolean, finalizadoContador:boolean,
      logDataOperacaoFormatada: string, indRetificado: boolean, nroRecibo: string, dataRetornoOK: Date, dataRetornoExclusao: Date, exibirRetorno2500: boolean,
      versaoEsocial:string) {
        this.codProcesso = codProcesso;
        this.idF2500 = idF2500;
        this.codParte = codParte;
        this.statusFormulario = statusFormulario;
        this.indRetificado = indRetificado;
        this.nroRecibo = nroRecibo;
        this.logCodUsuario = logCodUsuario;
        this.nomeUsuario = nomeUsuario;
        this.logDataOperacao = logDataOperacao;
        this.logDataOperacaoFormatada = logDataOperacaoFormatada;
        this.historico = historico;
        this.finalizadoEscritorio = finalizadoEscritorio;
        this.finalizadoContador = finalizadoContador;
        this.dataRetornoOK = dataRetornoOK;
        this.dataRetornoExclusao = dataRetornoExclusao;
        this.exibirRetorno2500 = exibirRetorno2500;
        this.versaoEsocial = versaoEsocial;

    }

    readonly codProcesso: number;
    readonly idF2500: number;
    readonly codParte: number;
    readonly statusFormulario: StatusFormularioEsocial;
    readonly indRetificado: boolean;
    readonly nroRecibo: string;
    readonly logCodUsuario: string;
    readonly nomeUsuario: string;
    readonly logDataOperacao: Date;
    readonly historico: Array<string>;
    readonly finalizadoEscritorio: boolean;
    readonly finalizadoContador: boolean;
    readonly logDataOperacaoFormatada: string;
    readonly dataRetornoOK: Date;
    readonly dataRetornoExclusao: Date;
    readonly exibirRetorno2500: boolean;
    readonly versaoEsocial: string;

    static fromObj(obj: any): Formulario2500 {
      return obj
             ? new Formulario2500(obj.codProcesso, obj.idF2500, obj.codParte,
              obj.statusFormulario, obj.logCodUsuario, obj.nomeUsuario, obj.logDataOperacao,
              obj.historico, obj.finalizadoEscritorio, obj.finalizadoContador,
              new Date(obj.logDataOperacao).toLocaleString().replace(',',''),
              obj.indRetificado, obj.nroRecibo, obj.dataRetornoOK, obj.dataRetornoExclusao, obj.exibirRetorno2500, obj.versaoEsocial)
             : null;
    }
}
