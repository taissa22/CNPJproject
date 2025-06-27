export class Indice {
  public constructor(
    id: number,
    descricao: string,
    codigoTipoIndice: string,
    codigoValorIndice: string,
    acumulado: boolean,
    acumuladoAutomatico: boolean,
  ) {
    this.id = id;
    this.descricao = descricao;
    this.codigoTipoIndice = codigoTipoIndice;
    this.codigoValorIndice = codigoValorIndice;
    this.acumulado = acumulado;
    this.acumuladoAutomatico = acumuladoAutomatico;
  }

  readonly id: number;
  readonly descricao: string;
  readonly codigoTipoIndice: string;
  readonly codigoValorIndice: string;
  readonly acumulado: boolean;
  readonly acumuladoAutomatico: boolean;

  static fromObj(obj: Indice): Indice {
    return new Indice(obj.id, obj.descricao, obj.codigoTipoIndice, obj.codigoValorIndice, obj.acumulado, obj.acumuladoAutomatico);
  }
}
