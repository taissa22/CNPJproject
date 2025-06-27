export class Unicidade {
  constructor(idF2500 : number,
              logDataOperacao : Date,
              logCodUsuario:  string,
              uniccontrMatunic :string,
              uniccontrCodcateg : number,
              uniccontrDtinicio :Date,
              idEsF2500Uniccontr : number,
              uniccontrDesccateg: string){

    this.idF2500 = idF2500,
    this.logDataOperacao = logDataOperacao,
    this.logCodUsuario = logCodUsuario,
    this.uniccontrMatunic = uniccontrMatunic,
    this.uniccontrCodcateg = uniccontrCodcateg,
    this.uniccontrDtinicio = uniccontrDtinicio,
    this.idEsF2500Uniccontr = idEsF2500Uniccontr,
    this.uniccontrDesccateg = uniccontrDesccateg
  }

  readonly idF2500 : number;
  readonly logDataOperacao : Date;
  readonly logCodUsuario : string;
  readonly uniccontrMatunic : string;
  readonly uniccontrCodcateg : number;
  readonly uniccontrDtinicio : Date;
  readonly idEsF2500Uniccontr : number;
  readonly uniccontrDesccateg : string;


  static  fromObj(item: any){
    return new Unicidade(item.idF2500,item.logDataOperacao,item.logCodUsuari,item.uniccontrMatunic,item.uniccontrCodcateg,item.uniccontrDtinicio,item.idEsF2500Uniccontr,item.uniccontrDesccateg);
  }

}
