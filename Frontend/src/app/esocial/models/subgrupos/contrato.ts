export class Contrato {
  constructor(
    idF2500: number,
    logDataOperacao: Date,
    logCodUsuario: string,
    infocontrTpcontr: number,
    infocontrIndcontr: string,
    infocontrDtadmorig: Date,
    infocontrIndreint: string,
    infocontrIndcateg: string,
    infocontrIndnatativ: string,
    infocontrIndmotdeslig: string,
    infocontrIndunic: string,
    infocontrMatricula: string,
    infocontrCodcateg: number,
    infocontrDtinicio: Date,
    infocomplCodcbo: string,
    infocomplNatatividade: number,
    infotermDtterm: Date,
    infotermMtvdesligtsv: string,
    idEsF2500Infocontrato: number,
    descricaoTipoContrato : string,
    indProprioTerceiro : string,
    descricaoCategoria: string,
    infovlrIndreperc: number,
  ) {
    (this.idF2500 = idF2500),
      (this.logDataOperacao = logDataOperacao),
      (this.logCodUsuario = logCodUsuario),
      (this.infocontrTpcontr = infocontrTpcontr),
      (this.infocontrIndcontr = infocontrIndcontr),
      (this.infocontrDtadmorig = infocontrDtadmorig),
      (this.infocontrIndreint = infocontrIndreint),
      (this.infocontrIndcateg = infocontrIndcateg),
      (this.infocontrIndnatativ = infocontrIndnatativ),
      (this.infocontrIndmotdeslig = infocontrIndmotdeslig),
      (this.infocontrIndunic = infocontrIndunic),
      (this.infocontrMatricula = infocontrMatricula),
      (this.infocontrCodcateg = infocontrCodcateg),
      (this.infocontrDtinicio = infocontrDtinicio),
      (this.idEsF2500Infocontrato = idEsF2500Infocontrato),
      (this.descricaoTipoContrato = descricaoTipoContrato);
      (this.infocomplCodcbo = infocomplCodcbo);
      (this.infocomplNatatividade = infocomplNatatividade);
      (this.infotermDtterm = infotermDtterm);
      (this.infotermMtvdesligtsv = infotermMtvdesligtsv);
      (this.indProprioTerceiro = indProprioTerceiro);
      (this.descricaoCategoria = descricaoCategoria);
      (this.infovlrIndreperc = infovlrIndreperc)
  }

  readonly idF2500: number;
  readonly logDataOperacao: Date;
  readonly logCodUsuario: string;
  readonly infocontrTpcontr: number;
  readonly infocontrIndcontr: string;
  readonly infocontrDtadmorig: Date;
  readonly infocontrIndreint: string;
  readonly infocontrIndcateg: string;
  readonly infocontrIndnatativ: string;
  readonly infocontrIndmotdeslig: string;
  readonly infocontrIndunic: string;
  readonly infocontrMatricula: string;
  readonly infocontrCodcateg: number;
  readonly infocontrDtinicio: Date;
  readonly idEsF2500Infocontrato: number;
  readonly descricaoTipoContrato : string;
  readonly infocomplCodcbo: string;
  readonly infocomplNatatividade: number;
  readonly infotermDtterm: Date;
  readonly infotermMtvdesligtsv: string;
  readonly indProprioTerceiro: string;
  readonly descricaoCategoria: string;
  readonly infovlrIndreperc: number;


  static fromObj(item: any) {
    return new Contrato(
      item.idF2500,
      item.logDataOperacao,
      item.logCodUsuario,
      item.infocontrTpcontr,
      item.infocontrIndcontr,
      item.infocontrDtadmorig,
      item.infocontrIndreint,
      item.infocontrIndcateg,
      item.infocontrIndnatativ,
      item.infocontrIndmotdeslig,
      item.infocontrIndunic,
      item.infocontrMatricula,
      item.infocontrCodcateg,
      item.infocontrDtinicio,
      item.infocomplCodcbo,
      item.infocomplNatatividade,
      item.infotermDtterm,
      item.infotermMtvdesligtsv,
      item.idEsF2500Infocontrato,
      item.descricaoTipoContrato,
      item.indProprioTerceiro, 
      item.descricaoCategoria,
      item.infovlrIndreperc
    );
  }
}
