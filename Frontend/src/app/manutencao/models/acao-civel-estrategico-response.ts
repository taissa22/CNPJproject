export class AcaoCivelEstrategicoResponse {
  public constructor(
    id: number,
    descricao: string
) {
  this.id = id;
  this.descricao = descricao;
}
  readonly id: number;
  readonly descricao: string;

  static fromObj(obj: any): AcaoCivelEstrategicoResponse {
    return ({
      id: obj.id,
      descricao: obj.descricao,
    });
  }
}
