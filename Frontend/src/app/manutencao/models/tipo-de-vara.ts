export class TipoDeVara {
  private constructor(codigo: number, nome: string, indCivel: boolean, indCivelEstrategico: boolean, 
                      indTrabalhista: boolean, indTributaria: boolean, indJuizado: boolean, 
                      indCriminalJudicial: boolean, indProcon: boolean) {
    this.codigo = codigo;
    this.nome = nome;
    this.indCivel = indCivel;
    this.indCivelEstrategico = indCivelEstrategico;
    this.indTrabalhista = indTrabalhista;
    this.indTributaria = indTributaria;
    this.indJuizado = indJuizado;
    this.indCriminalJudicial = indCriminalJudicial;
    this.indProcon = indProcon;
  }


  readonly codigo: number;
  readonly nome: string;
  readonly indCivel: boolean;
  readonly indCivelEstrategico: boolean;
  readonly indTrabalhista: boolean;
  readonly indTributaria: boolean;
  readonly indJuizado: boolean;
  readonly indCriminalJudicial: boolean;
  readonly indProcon: boolean;

  static fromObj(obj: TipoDeVaraBack): TipoDeVara {
    if(!obj) return null;
    return new TipoDeVara(obj.id, obj.nome, obj.eh_CivelConsumidor, obj.eh_CivelEstrategico, obj.eh_Trabalhista, obj.eh_Tributaria, obj.eh_Juizado, obj.eh_CriminalJudicial, obj.eh_Procon);
  }
}

export class TipoDeVaraBack {
  readonly id: number;
  readonly nome: string;
  readonly eh_CivelConsumidor: boolean;
  readonly eh_CivelEstrategico: boolean;
  readonly eh_Trabalhista: boolean;
  readonly eh_Tributaria: boolean;
  readonly eh_Juizado: boolean;
  readonly eh_CriminalJudicial: boolean;
  readonly eh_Procon: boolean;
}
