export class OrientacaoJuridicaTrabalhista {
  private constructor(
    codOrientacaoJuridica: number,
    codTipoOrientacaoJuridica : number,
    tipoOrientacaoJuridica: {id: number, descricao: string},
    nome: string,
    ativo: boolean,
    descricao: string,
    palavrasChave: string,
    ehTrabalhista : boolean
  ) {
    this.codOrientacaoJuridica = codOrientacaoJuridica;
    this.codTipoOrientacaoJuridica = codTipoOrientacaoJuridica;
    this.tipoOrientacaoJuridica = tipoOrientacaoJuridica ;
    this.nome = nome;
    this.ativo = ativo;
    this.descricao = descricao;
    this.palavrasChave = palavrasChave;
    this.ehTrabalhista = ehTrabalhista;
  }

  readonly codOrientacaoJuridica: number;
  readonly codTipoOrientacaoJuridica : number;
  readonly tipoOrientacaoJuridica: {id: number, descricao: string};
  readonly nome: string;
  readonly ativo: boolean;
  readonly descricao: string;
  readonly palavrasChave: string;
  readonly ehTrabalhista : boolean;

  static fromObj(obj: any): OrientacaoJuridicaTrabalhista {
    return ({
      codOrientacaoJuridica: obj.codOrientacaoJuridica,
      codTipoOrientacaoJuridica : obj.codTipoOrientacaoJuridica,
      tipoOrientacaoJuridica: obj.tipoOrientacaoJuridica,
      nome: obj.nome,
      ativo: obj.ativo,
      descricao: obj.descricao,
      palavrasChave: obj.palavrasChave,
      ehTrabalhista : obj.ehTrabalhista
    });
  }
}

