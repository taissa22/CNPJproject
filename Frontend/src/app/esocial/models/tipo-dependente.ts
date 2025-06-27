export class TipoDependente {
  constructor(id: number, descricao: string) {
    (this.id = id), (this.descricao = descricao);
  }

  readonly id: number;
  readonly descricao: string;

  static fromObj(item: any) {
    return new TipoDependente(item.id, item.descricao);
  }
}
