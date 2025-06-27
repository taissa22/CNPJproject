export class Cotacao {
  private constructor(
    dataCotacao: Date,
    valorCotacao: string,
    valorCotacaoAcumulado: string,
    indice: {id: number, descricao: string, codigoValorIndice: string, acumulado:boolean},

  ) {
    this.dataCotacao = dataCotacao;
    this.valor = valorCotacao;
    this.valorAcumulado = valorCotacaoAcumulado;
    this.indice = indice;
  }

  readonly dataCotacao: Date;
  readonly valor: string;
  readonly valorAcumulado: string;
  readonly indice: {id: number, descricao: string, codigoValorIndice: string, acumulado: boolean};

  static fromObj(obj: Cotacao): Cotacao {
    return new Cotacao(obj.dataCotacao, obj.valor, obj.valorAcumulado, obj.indice);
  }
}
