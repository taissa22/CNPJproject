export class Periodo {
  constructor(
    idF2500: number,
    ideperiodoPerref: string,
    logCodUsuario: string,
    logDataOperacao: Date,
    basecalculoVrbccpmensal: number,
    basecalculoVrbccp13: number,
    infoagnocivoGrauexp: number,
    basemudcategCodcateg: number,
    basemudcategVrbccprev: number,
    idEsF2500Ideperiodo: number,
    infoFGTSvrBcFGTSProcTrab: number,
    infoFGTSvrBcFGTSSefip: number,
    infoFGTSvrBcFGTSDecAnt: number
  ) {
    (this.idF2500 = idF2500),
      (this.ideperiodoPerref = ideperiodoPerref != null ? ideperiodoPerref.split('-').reverse().join('/') : null),
      (this.logCodUsuario = logCodUsuario),
      (this.logDataOperacao = logDataOperacao),
      (this.basecalculoVrbccpmensal = basecalculoVrbccpmensal),
      (this.basecalculoVrbccp13 = basecalculoVrbccp13),
      (this.infoagnocivoGrauexp = infoagnocivoGrauexp),
      (this.basemudcategCodcateg = basemudcategCodcateg),
      (this.basemudcategVrbccprev = basemudcategVrbccprev),
      (this.idEsF2500Ideperiodo = idEsF2500Ideperiodo),
      this.infoFGTSvrBcFGTSProcTrab = infoFGTSvrBcFGTSProcTrab,
      this.infoFGTSvrBcFGTSSefip = infoFGTSvrBcFGTSSefip
      this.infoFGTSvrBcFGTSDecAnt = infoFGTSvrBcFGTSDecAnt;
      this.basecalculoVrbccpmensalFormatado = basecalculoVrbccpmensal != null ? basecalculoVrbccpmensal.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL', currencyDisplay: 'symbol', minimumFractionDigits: 2 }) : '';
      this.basecalculoVrbccp13Formatado = basecalculoVrbccp13 != null ? basecalculoVrbccp13.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL', currencyDisplay: 'symbol', minimumFractionDigits: 2 }) : '';
      this.basemudcategVrbccprevFormatada = basemudcategVrbccprev != null ? basemudcategVrbccprev.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL', currencyDisplay: 'symbol', minimumFractionDigits: 2 }) : '';

  }

  readonly idF2500: number;
  readonly ideperiodoPerref: string;
  readonly logCodUsuario: string;
  readonly logDataOperacao: Date;
  readonly basecalculoVrbccpmensal: number;
  readonly basecalculoVrbccpmensalFormatado : string;
  readonly basecalculoVrbccp13: number;
  readonly basecalculoVrbccp13Formatado: string;
  readonly infoagnocivoGrauexp: number;
  readonly basemudcategCodcateg: number;
  readonly basemudcategVrbccprev: number;
  readonly basemudcategVrbccprevFormatada: string;
  readonly idEsF2500Ideperiodo: number;
  readonly infoFGTSvrBcFGTSProcTrab: number;
  readonly infoFGTSvrBcFGTSSefip: number;
  readonly infoFGTSvrBcFGTSDecAnt: number;

  static fromObj(item: any) {
    
    return new Periodo(
      item.idF2500,
      item.ideperiodoPerref,
      item.logCodUsuario,
      item.logDataOperacao,
      item.basecalculoVrbccpmensal,
      item.basecalculoVrbccp13,
      item.infoagnocivoGrauexp,
      item.basemudcategCodcateg,
      item.basemudcategVrbccprev,
      item.idEsF2500Ideperiodo,
      item.infofgtsVrbcfgtsproctrab,
      item.infofgtsVrbcfgtssefip,
      item.infofgtsVrbcfgtsdecant
    );
  }
}
