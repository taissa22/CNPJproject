export class Abono {
  constructor(
    idEsF2500Abono: number,
    idEsF2500Infocontrato: number,
    logCodUsuario: string,
    logDataOperacao: Date,
    abonoAnobase: string
  ) {
    (this.idEsF2500Abono = idEsF2500Abono),
      (this.idEsF2500Infocontrato = idEsF2500Infocontrato),
      (this.logCodUsuario = logCodUsuario),
      (this.logDataOperacao = logDataOperacao),
      (this.abonoAnobase = abonoAnobase);      
  }

  readonly idEsF2500Abono: number;
  readonly idEsF2500Infocontrato: number;
  readonly logCodUsuario: string;
  readonly logDataOperacao: Date;
  readonly abonoAnobase: string;
 


  static fromObj(item: any) {
    return new Abono(
      item.idEsF2500Abono,
      item.idEsF2500Infocontrato,
      item.logCodUsuario,
      item.logDataOperacao,
      item.abonoAnobase
    );
  }
}
