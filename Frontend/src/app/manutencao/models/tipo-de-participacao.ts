export class TipoDeParticipacao {
  private constructor(codigo: number, descricao: string) {
    this.codigo = codigo;
    this.descricao = descricao;
  }

  readonly codigo: number;
  readonly descricao: string;

  static fromObj(obj: TipoDeParticipacao): TipoDeParticipacao {
    return new TipoDeParticipacao(obj.codigo, obj.descricao);
  }
}
