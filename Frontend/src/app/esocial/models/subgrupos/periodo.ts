export class Periodo {
  constructor(
    idF2500: number,
    ideperiodoPerref: string,
    logCodUsuario: string,
    logDataOperacao: Date,
    basecalculoVrbccpmensal: number,
    basecalculoVrbccp13: number,
    basecalculoVrbcfgts: number,
    basecalculoVrbcfgts13: number,
    infoagnocivoGrauexp: number,
    infofgtsVrbcfgtsguia: number,
    infofgtsVrbcfgts13guia: number,
    infofgtsPagdireto: string,
    basemudcategCodcateg: number,
    basemudcategVrbccprev: number,
    idEsF2500Ideperiodo: number
  ) {
    (this.idF2500 = idF2500),
      (this.ideperiodoPerref = ideperiodoPerref),
      (this.logCodUsuario = logCodUsuario),
      (this.logDataOperacao = logDataOperacao),
      (this.basecalculoVrbccpmensal = basecalculoVrbccpmensal),
      (this.basecalculoVrbccp13 = basecalculoVrbccp13),
      (this.basecalculoVrbcfgts = basecalculoVrbcfgts),
      (this.basecalculoVrbcfgts13 = basecalculoVrbcfgts13),
      (this.infoagnocivoGrauexp = infoagnocivoGrauexp),
      (this.infofgtsVrbcfgtsguia = infofgtsVrbcfgtsguia),
      (this.infofgtsVrbcfgts13guia = infofgtsVrbcfgts13guia),
      (this.infofgtsPagdireto = infofgtsPagdireto),
      (this.basemudcategCodcateg = basemudcategCodcateg),
      (this.basemudcategVrbccprev = basemudcategVrbccprev),
      (this.idEsF2500Ideperiodo = idEsF2500Ideperiodo);
  }

  readonly idF2500: number;
  readonly ideperiodoPerref: string;
  readonly logCodUsuario: string;
  readonly logDataOperacao: Date;
  readonly basecalculoVrbccpmensal: number;
  readonly basecalculoVrbccp13: number;
  readonly basecalculoVrbcfgts: number;
  readonly basecalculoVrbcfgts13: number;
  readonly infoagnocivoGrauexp: number;
  readonly infofgtsVrbcfgtsguia: number;
  readonly infofgtsVrbcfgts13guia: number;
  readonly infofgtsPagdireto: string;
  readonly basemudcategCodcateg: number;
  readonly basemudcategVrbccprev: number;
  readonly idEsF2500Ideperiodo: number;

  static fromObj(item: any) {
    
    return new Periodo(
      item.idF2500,
      item.ideperiodoPerref,
      item.logCodUsuario,
      item.logDataOperacao,
      item.basecalculoVrbccpmensal,
      item.basecalculoVrbccp13,
      item.basecalculoVrbcfgts,
      item.basecalculoVrbcfgts13,
      item.infoagnocivoGrauexp,
      item.infofgtsVrbcfgtsguia,
      item.infofgtsVrbcfgts13guia,
      item.infofgtsPagdireto,
      item.basemudcategCodcateg,
      item.basemudcategVrbccprev,
      item.idEsF2500Ideperiodo
    );
  }
}
