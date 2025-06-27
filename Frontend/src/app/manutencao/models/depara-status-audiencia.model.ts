export class DeparaStatusAudiencia {
  private constructor(
    id: number,
    idStatusApp: number,
    idSubStatusApp: number,
    idStatusSisjur: number,
    descricaoStatusApp: string,
    descricaoSubStatusApp: string,
    descricaoStatusSisjur: string,
    criacaoAutomaticaNovaAudiencia: string,
    descricaoTipoProcesso: string,
) {
  this.id = id;
  this.idStatusApp = idStatusApp;
  this.idSubStatusApp  = idSubStatusApp;
  this.idStatusSisjur  =  idStatusSisjur;
  this.descricaoStatusApp  = descricaoStatusApp;
  this.descricaoSubStatusApp = descricaoSubStatusApp;
  this.descricaoStatusSisjur = descricaoStatusSisjur;
  this.criacaoAutomaticaNovaAudiencia = criacaoAutomaticaNovaAudiencia;
  this.descricaoTipoProcesso = descricaoTipoProcesso;
}
  readonly id: number;
  readonly idStatusApp: number;
  readonly idSubStatusApp: number;
  readonly idStatusSisjur: number;
  readonly descricaoStatusApp: string;
  readonly descricaoSubStatusApp: string;
  readonly descricaoStatusSisjur: string;
  readonly criacaoAutomaticaNovaAudiencia: string;
  readonly descricaoTipoProcesso: string;

  static fromObj(obj: any): DeparaStatusAudiencia {
    return ({
      id: obj.id,
      idStatusApp: obj.idStatusApp,
      idSubStatusApp: obj.idSubStatusApp,
      idStatusSisjur: obj.idStatusSisjur,
      descricaoStatusApp: obj.descricaoStatusApp,
      descricaoSubStatusApp: obj.descricaoSubStatusApp,
      descricaoStatusSisjur: obj.descricaoStatusSisjur,
      criacaoAutomaticaNovaAudiencia: obj.criacaoAutomaticaNovaAudiencia,
      descricaoTipoProcesso : obj.descricaoTipoProcesso
    });
  }



}
