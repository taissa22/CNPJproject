export class PenAlim {
  constructor(
    idEsF2501Infocrirrf: number,
    idEsF2501Penalim: number,
    logCodUsuario: string,
    logDataOperacao: Date,
    penalimTprend: number,
    penalimCpfdep: string,
    penalimVlrpensao: number,
    descricaoTipoRend: string
  ) {
    this.idEsF2501Infocrirrf = idEsF2501Infocrirrf,
      this.idEsF2501Penalim = idEsF2501Penalim,
      this.logCodUsuario = logCodUsuario,
      this.logDataOperacao = logDataOperacao,
      this.penalimTprend = penalimTprend,
      this.penalimCpfdep = penalimCpfdep,
      this.penalimVlrpensao = penalimVlrpensao,
      this.descricaoTipoRend = descricaoTipoRend
  }

  readonly idEsF2501Infocrirrf: number;
  readonly idEsF2501Penalim: number;
  readonly logCodUsuario: string;
  readonly logDataOperacao: Date;
  readonly penalimTprend: number;
  readonly penalimCpfdep: string;
  readonly penalimVlrpensao: number;
  readonly descricaoTipoRend: string;

  static fromObj(item: any) {
    return new PenAlim(
      item.idEsF2501Infocrirrf,
      item.idEsF2501Penalim,
      item.logCodUsuario,
      item.logDataOperacao,
      item.penalimTprend,
      item.penalimCpfdep,
      item.penalimVlrpensao,
      item.descricaoTipoRend

    );
  }
}
