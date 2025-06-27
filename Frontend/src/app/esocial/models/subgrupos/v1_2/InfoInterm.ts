export class InfoInterm {
  constructor(
    idEsF2500Infointerm: number,
    idEsF2500Ideperiodo: number,
    infointermDia: number,
    infointermHrstrab: string,
    logCodUsuario: string,
    logDataOperacao: Date
  ) {
    (this.idEsF2500Infointerm = idEsF2500Infointerm),
      (this.idEsF2500Ideperiodo = idEsF2500Ideperiodo),
      (this.infointermDia = infointermDia),
      (this.infointermHrstrab = infointermHrstrab),
      (this.logCodUsuario = logCodUsuario),
      (this.logDataOperacao = logDataOperacao)
  }

  readonly idEsF2500Infointerm: number;
  readonly idEsF2500Ideperiodo: number;
  readonly infointermDia: number;
  readonly infointermHrstrab: string;
  readonly logCodUsuario: string;
  readonly logDataOperacao : Date;
  

  static fromObj(item: any) {
    
    return new InfoInterm(
      item.idEsF2500Infointerm,
      item.idEsF2500Ideperiodo,
      item.infointermDia,
      item.infointermHrstrab,
      item.logCodUsuario,
      item.logDataOperacao,
    );
  }
}
