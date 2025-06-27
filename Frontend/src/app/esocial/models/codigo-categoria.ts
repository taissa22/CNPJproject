export class CodigoCategoria {
  constructor(id: number, descricao: string) {
    (this.id = id), (this.descricao = descricao);
  }

  readonly id: number;
  readonly descricao: string;

  static fromObj(item: any) {
    return new CodigoCategoria(item.id, item.descricao);
  }
}
