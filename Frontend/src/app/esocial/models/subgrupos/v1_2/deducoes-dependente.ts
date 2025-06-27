export class DeduçoesDependente {
  constructor(idEsF2501Deddepen : number,
              idEsF2501Infocrirrf : number,
              deddepenTprend : number,
              deddepenCpfdep : string,
              deddepenVlrdeducao : number,
              logCodUsuario : string,
              logDataOperacao : Date,
              DescricaoTipoRend: string){

    this.idEsF2501Deddepen = idEsF2501Deddepen,
    this.idEsF2501Infocrirrf = idEsF2501Infocrirrf,
    this.deddepenTprend = deddepenTprend,
    this.deddepenCpfdep = deddepenCpfdep,
    this.deddepenVlrdeducao = deddepenVlrdeducao,
    this.logCodUsuario = logCodUsuario,
    this.logDataOperacao= logDataOperacao,
    this.descricaoTipoRend = DescricaoTipoRend

  }

  readonly idEsF2501Deddepen : number;
  readonly idEsF2501Infocrirrf : number;
  readonly deddepenTprend : number;
  readonly deddepenCpfdep : string;
  readonly deddepenVlrdeducao : number;
  readonly logCodUsuario : string;
  readonly logDataOperacao : Date;
  readonly descricaoTipoRend: string;


  static fromObj(item: any){
    return new DeduçoesDependente(item.idEsF2501Deddepen ,item.idEsF2501Infocrirrf , item.deddepenTprend , item.deddepenCpfdep ,
                          item.deddepenVlrdeducao ,item.logCodUsuario , item.logDataOperacao, item.descricaoTipoRend );
  }

}
