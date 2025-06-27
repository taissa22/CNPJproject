// prettier-ignore
export class TiposProcesso {
  private constructor(id: number, descricao: string) {
    this.id = id;
    this.descricao = descricao;
  }

  readonly id: number;
  readonly descricao: string;
  static NAO_DEFINIDO = new TiposProcesso(null, 'Não definido');

  static CIVEL_CONSUMIDOR = new TiposProcesso(1, 'Cível Consumidor');
  static TRABALHISTA = new TiposProcesso(2, 'Trabalhista');
  static ADMINISTRATIVO = new TiposProcesso(3, 'Administrativo');
  static TRIBUTARIO_ADMINISTRATIVO = new TiposProcesso(4, 'Tributário Administrativo');
  static TRIBUTARIO_JUDICIAL = new TiposProcesso(5, 'Tributário Judicial');
  static TRABALHISTA_ADMINISTRATIVO = new TiposProcesso(6, 'Trabalhista Administrativo');
  static JEC = new TiposProcesso(7, 'Juizado Especial Cível');
  static CIVEL_ESTRATEGICO = new TiposProcesso(9, 'Cível Estratégico');
  static CIVEL_ADMINISTRATIVO = new TiposProcesso(12, 'Cível Administrativo');
  static CRIMINAL = new TiposProcesso(13,'Criminal');
  static CRIMINAL_ADMINISTRATIVO = new TiposProcesso(14, 'Criminal Administrativo');
  static CRIMINAL_JUDICIAL = new TiposProcesso(15, 'Criminal Judicial');
  static PROCON = new TiposProcesso(17, 'Procon');
  static PEX = new TiposProcesso(18, 'Pex');
  static PEX_CONSUMIDOR = new TiposProcesso(18.1, 'Pex Consumidor');
  static PEX_JUIZADO = new TiposProcesso(18.2, 'Pex Juizado');

  static porId(
    id: null | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 9 | 12 | 13 | 14 | 15 | 17 | 18.1 | 18.2
  ): TiposProcesso {
    switch (id) {
        case 1:
          return this.CIVEL_CONSUMIDOR;
        case 2:
          return this.TRABALHISTA;
        case 3:
          return this.ADMINISTRATIVO;
        case 4:
          return this.TRIBUTARIO_ADMINISTRATIVO;
        case 5:
          return this.TRIBUTARIO_JUDICIAL;
        case 6:
          return this.TRABALHISTA_ADMINISTRATIVO;
        case 7:
          return this.JEC;
        case 9:
          return this.CIVEL_ESTRATEGICO;
        case 13:
          return this.CRIMINAL;
        case 12:
          return this.CIVEL_ADMINISTRATIVO;
        case 14:
          return this.CRIMINAL_ADMINISTRATIVO;
        case 15:
          return this.CRIMINAL_JUDICIAL;
        case 17:
          return this.PROCON;
        case 18.1:
          return this.PEX_CONSUMIDOR;
        case 18.2:
          return this.PEX_JUIZADO;

      case null:
      default:
        return this.NAO_DEFINIDO;
    }
  }

  static todos(): Array<TiposProcesso> {
    return [
      // this.NAO_DEFINIDO,
      this.CIVEL_CONSUMIDOR,
      this.TRABALHISTA,
      this.ADMINISTRATIVO,
      this.TRIBUTARIO_ADMINISTRATIVO,
      this.TRIBUTARIO_JUDICIAL,
      this.TRABALHISTA_ADMINISTRATIVO,
      this.JEC,
      this.CIVEL_ESTRATEGICO,
      this.CIVEL_ADMINISTRATIVO,
      this.CRIMINAL,
      this.CRIMINAL_ADMINISTRATIVO,
      this.CRIMINAL_JUDICIAL,
      this.PROCON,
      this.PEX_CONSUMIDOR,
      this.PEX_JUIZADO
    ];
  }

  static equals(one: TiposProcesso, other: TiposProcesso): boolean {
    return one && other ? one.id === other.id : one === other;
  }
}
