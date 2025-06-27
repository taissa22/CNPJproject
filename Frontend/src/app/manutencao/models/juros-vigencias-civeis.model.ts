export class JurosVigenciasCiveis {
  private constructor(
    codigo: number,
    tipoDeProcesso: {id: number, nome: string},
    dataVigencia: Date,
    valorJuros: number,
  ) {
    this.codigo = codigo;
    this.tipoDeProcesso = tipoDeProcesso,
    this.dataVigencia = dataVigencia;
    this.valorJuros = valorJuros;
  }

  readonly codigo: number;
  readonly tipoDeProcesso: {id: number, nome: string}
  readonly dataVigencia: Date;
  readonly valorJuros: number;

  static fromObj(obj: JurosVigenciasCiveisBack): JurosVigenciasCiveis {
    return ({
      codigo: obj.CodTipoProcesso,
      tipoDeProcesso: obj.tipoProcesso,
      dataVigencia: obj.dataVigencia,
      valorJuros: obj.valorJuros,
    });
  }
}

export class JurosVigenciasCiveisBack {
  readonly CodTipoProcesso: number;
  readonly tipoProcesso: {id: number, nome: string}
  readonly dataVigencia: Date;
  readonly valorJuros: number;
}
  