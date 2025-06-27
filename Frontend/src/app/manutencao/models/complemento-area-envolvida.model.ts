export interface IComplementoAreaEnvolvida {
  id: number;
  nome: string;
  tipoProcesso: {id: number; nome: string; nomeEnum: string};
  ativo: boolean;
}

export class ComplementoAreaEnvolvida implements IComplementoAreaEnvolvida {
// export class ComplementoAreaEnvolvida {
  private constructor(
    id: number,
    nome: string,
    tipoProcesso: {id: number; nome: string; nomeEnum: string},
    ativo: boolean
  ) {
    this.id = id;
    this.nome = nome;
    this.tipoProcesso = tipoProcesso;
    this.ativo = ativo;
  }

  readonly id: number;
  readonly nome: string;
  readonly tipoProcesso: {id: number; nome: string; nomeEnum: string};
  readonly ativo: boolean;

  static fromObj(obj: any): ComplementoAreaEnvolvida {
    return ({
      id: obj.id,
      nome: obj.nome,
      tipoProcesso: obj.tipoProcesso,
      ativo: obj.ativo
    });
  }



}
