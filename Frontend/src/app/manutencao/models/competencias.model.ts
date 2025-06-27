export class Competencia {
  nome: string;
  sequencial: number;

  static fromJson(p: Competencia): Competencia {
    const competencia = new Competencia();
    competencia.nome = p.nome;
    competencia.sequencial = p.sequencial;
    return competencia;
  }
}
