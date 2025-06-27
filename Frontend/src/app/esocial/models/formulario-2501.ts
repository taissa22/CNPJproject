import { StatusFormularioEsocial } from '../enum/status-formulario';

export class Formulario2501 {
  private constructor(
    codProcesso: number,
    idF2501: number,
    codParte: number,
    historico: Array<string>,
    statusFormulario: number,
    finalizadoEscritorio: boolean,
    finalizadoContador: boolean,
    logDataOperacao: Date,
    nomeUsuario: string,
    logCodUsuario: string,
    logDataOperacaoFormatada: string,
    indRetificado: boolean,
    nroRecibo: string,
    periodoApuracao: string,
    exibirRetorno2501: boolean,
    versaoEsocial: string
  ) {
    this.codProcesso = codProcesso;
    this.idF2501 = idF2501;
    this.codParte = codParte;
    this.historico = historico;
    this.finalizadoEscritorio = finalizadoEscritorio;
    this.finalizadoContador = finalizadoContador;
    this.statusFormulario = statusFormulario;
    this.indRetificado = indRetificado;
    this.nroRecibo = nroRecibo;
    this.periodoApuracao = periodoApuracao;
    this.logDataOperacao = logDataOperacao;
    this.logDataOperacaoFormatada = logDataOperacaoFormatada;
    this.logCodUsuario = logCodUsuario;
    this.nomeUsuario = nomeUsuario;
    this.exibirRetorno2501 = exibirRetorno2501;
    this.versaoEsocial = versaoEsocial;
  }

  readonly codProcesso: number;
  readonly idF2501: number;
  readonly codParte: number;
  readonly historico: Array<string>;
  readonly statusFormulario: StatusFormularioEsocial;
  readonly indRetificado: boolean;
  readonly nroRecibo: string;
  readonly periodoApuracao: string;
  readonly finalizadoEscritorio: boolean;
  readonly finalizadoContador: boolean;
  readonly logDataOperacao: Date;
  readonly logDataOperacaoFormatada: string;
  readonly nomeUsuario: string;
  readonly logCodUsuario: string;
  readonly exibirRetorno2501: boolean;
  readonly versaoEsocial: string;

  EhDataFutura(): boolean {
    const data = this.periodoApuracao;

    if (!data) {
      return false;
    }
    const inputYear = parseInt(data.substring(0, 4), 10);
    const inputMonth = parseInt(data.substring(4, 6), 10);

    const now = new Date();
    const currentYear = now.getFullYear();
    const currentMonth = now.getMonth() + 1;

    if (inputYear > currentYear) {
      return true;
    } else if (inputYear === currentYear && inputMonth > currentMonth) {
      return true;
    }
    return false;
  }

  static fromObj(obj: any): Formulario2501 {
    return obj
      ? new Formulario2501(
          obj.codProcesso,
          obj.idF2501,
          obj.codParte,
          obj.historico,
          obj.statusFormulario,
          obj.finalizadoEscritorio,
          obj.finalizadoContador,
          obj.logDataOperacao,
          obj.nomeUsuario,
          obj.logCodUsuario,
          new Date(obj.logDataOperacao).toLocaleString().replace(',', ''),
          obj.indRetificado,
          obj.nroRecibo,
          obj.periodoApuracao,
          obj.exibirRetorno2501,
          obj.versaoEsocial
        )
      : null;
  }
}
