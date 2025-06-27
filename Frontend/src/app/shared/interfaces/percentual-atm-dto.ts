export class PercentualAtmDTO {
  estadoId: string;
  percentual: number;
  dataVigencia: Date;
  nomeEstado: string;

  static fromJson(o: PercentualAtmDTO): PercentualAtmDTO {
    const percentual = new PercentualAtmDTO();
    percentual.estadoId = o.estadoId;
    percentual.nomeEstado = o.nomeEstado;
    percentual.percentual = o.percentual;
    percentual.dataVigencia = o.dataVigencia;
    return percentual;
  }
}
