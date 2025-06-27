export class DeparaStatusNegociacao {
  private constructor(
    id: number,
    idStatusApp: number,
    idSubStatusApp: number,
    idStatusSisjur: number,
    descricaoStatusApp: string,
    descricaoSubStatusApp: string,
    descricaoStatusSisjur: string,
    //criaNegociacoes: string,
    descricaoTipoProcesso: string,
) {
  this.id = id;
  this.idStatusApp = idStatusApp;
  this.idSubStatusApp  = idSubStatusApp;
  this.idStatusSisjur  =  idStatusSisjur;
  this.descricaoStatusApp  = descricaoStatusApp;
  this.descricaoSubStatusApp = descricaoSubStatusApp;
  this.descricaoStatusSisjur = descricaoStatusSisjur;
  //this.criaNegociacoes = criaNegociacoes;
  this.descricaoTipoProcesso = descricaoTipoProcesso;
}
  readonly id: number;
  readonly idStatusApp: number;
  readonly idSubStatusApp: number;
  readonly idStatusSisjur: number;
  readonly descricaoStatusApp: string;
  readonly descricaoSubStatusApp: string;
  readonly descricaoStatusSisjur: string;
  //readonly criaNegociacoes: string;
  readonly descricaoTipoProcesso: string;

  static fromObj(obj: any): DeparaStatusNegociacao {
    return ({
      id: obj.id,
      idStatusApp: obj.idStatusApp,
      idSubStatusApp: obj.idSubStatusApp,
      idStatusSisjur: obj.idStatusSisjur,
      descricaoStatusApp: obj.descricaoStatusApp,
      descricaoSubStatusApp: obj.descricaoSubStatusApp,
      descricaoStatusSisjur: obj.descricaoStatusSisjur,
      //criaNegociacoes: obj.criaNegociacoes,
      descricaoTipoProcesso : obj.descricaoTipoProcesso
    });
  }



}
