export class Dependente {
  constructor(idEsF2500Dependente : number,
              idF2500 : number,
              dependenteCpfdep : string,
              dependenteTpdep : string,
              dependenteDescdep : string,
              logCodUsuario : string,
              logDataOperacao : Date,descricaoTipoDependente : string){

    this.idEsF2500Dependente = idEsF2500Dependente,
    this.idF2500 = idF2500,
    this.dependenteCpfdep = dependenteCpfdep,
    this.dependenteTpdep = dependenteTpdep,
    this.dependenteDescdep = dependenteDescdep,
    this.logCodUsuario = logCodUsuario,
    this.logDataOperacao= logDataOperacao
    this.descricaoTipoDependente = descricaoTipoDependente;
  }

  readonly idEsF2500Dependente : number;
  readonly idF2500 : number;
  readonly dependenteCpfdep : string;
  readonly dependenteTpdep : string;
  readonly dependenteDescdep : string;
  readonly logCodUsuario : string;
  readonly logDataOperacao : Date;
  readonly descricaoTipoDependente : string


  static fromObj(item: any){
    return new Dependente(item.idEsF2500Dependente ,item.idF2500 , item.dependenteCpfdep , item.dependenteTpdep ,
                          item.dependenteDescdep ,item.logCodUsuario , item.logDataOperacao, item.descricaoTipoDependente );
  }

}
