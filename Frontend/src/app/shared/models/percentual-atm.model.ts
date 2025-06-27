export class PercentualAtm {
  id: number;
  uf: string;
  percentual: number;

  static fromJson(o: PercentualAtm): PercentualAtm {
    const percentual = new PercentualAtm();
    percentual.id = o.id;
    percentual.uf = o.uf;
    percentual.percentual = o.percentual;
    return percentual;
  }
}
