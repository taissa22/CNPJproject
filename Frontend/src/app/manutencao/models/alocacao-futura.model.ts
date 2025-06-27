import { EstadoEnum } from ".";

export class AlocacaoFutura {
  private constructor(
    tipo : string,
    data : Date,
   // uf : string,
    comarca : string,
    varaId : number,
    varaNome : string,
    numeroProcesso : string,
    tipoProcesso : string,
    codProcessoInterno : number 

) {
    this.tipo = tipo,
    this.data = data,
    //this.uf = uf;
    this.comarca = comarca;
    this.varaId = varaId;
    this.varaNome = varaNome;
    this.numeroProcesso = numeroProcesso;
    this.tipoProcesso = tipoProcesso;
    this.codProcessoInterno = codProcessoInterno;


}
  readonly tipo : string;
  readonly data : Date;
 // readonly uf : string;
  readonly comarca : string;
  readonly varaId : number;
  readonly varaNome : string;
  readonly numeroProcesso : string;
  readonly tipoProcesso : string;
  readonly codProcessoInterno : number ;


  static fromObj(obj: any): AlocacaoFutura {
    return ({
      tipo : obj.tipo,
      data : obj.data,
//      uf : obj.uf,
      comarca : obj.comarca,
      varaId : obj.varaId,
      varaNome : obj.varaNome,
      numeroProcesso : obj.numeroProcesso,
      tipoProcesso : obj.tipoProcesso,
      codProcessoInterno : obj.codProcessoInterno,
    });
  }



}
