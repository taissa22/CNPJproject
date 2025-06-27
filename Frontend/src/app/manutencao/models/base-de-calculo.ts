export class BaseDeCalculo {
  private constructor(
    codigo: number,
    descricao: string,
    ehCalculoInicial: boolean
  ) {
    this.codigo = codigo;
    this.descricao = descricao;
    this.ehCalculoInicial = ehCalculoInicial;
  }

  readonly codigo: number;
  readonly descricao: string;
  readonly ehCalculoInicial: boolean;

  static fromObj(obj: BaseDeCalculoBack): BaseDeCalculo {
    return new BaseDeCalculo(obj.codigo, obj.descricao, obj.indBaseInicial);
  }
}

export class BaseDeCalculoBack {
  codigo: number;
  descricao: string;
  indBaseInicial: boolean;
}