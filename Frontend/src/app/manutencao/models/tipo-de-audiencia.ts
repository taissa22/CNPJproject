export class TipoDeAudiencia{
  private constructor(
    codigoTipoAudiencia: number,
    sigla: string,
    descricao: string,
    tipoDeProcesso: {id: number, nome: string},
    ativo: boolean,
    linkVirtual: boolean,
    idMigracao?: number,
    ativoDePara?: boolean,
    descricaoMigracao?: string,
  ) {
    this.codigoTipoAudiencia = codigoTipoAudiencia;
    this.sigla = sigla;
    this.descricao = descricao;
    this.tipoDeProcesso = tipoDeProcesso;
    this.ativo = ativo;
    this.linkVirtual = linkVirtual;
    this. idMigracao = idMigracao;
    this.ativoDePara = ativoDePara;
    this.descricaoMigracao = descricaoMigracao;
  }

  readonly codigoTipoAudiencia: number;
  readonly sigla: string;
  readonly descricao: string;
  readonly tipoDeProcesso: {id: number, nome: string};
  readonly ativo: boolean;
  readonly linkVirtual: boolean;
  readonly idMigracao: number;
  readonly ativoDePara: boolean;
  readonly descricaoMigracao: string;

  static fromObj(obj: any): TipoDeAudiencia {
    return ({
      codigoTipoAudiencia: obj.codigoTipoAudiencia,
      sigla: obj.sigla,
      descricao: obj.descricao,
      tipoDeProcesso: obj.tipoProcesso,
      ativo: obj.ativo,
      linkVirtual: obj.linkVirtual,
      idMigracao: obj.idMigracao,
      ativoDePara: obj.ativoDePara,
      descricaoMigracao: obj.descricaoMigracao
    });
  }
}


export class TipoDeAudienciaBack {
  readonly codigoTipoAudiencia: number;
  readonly sigla: string;
  readonly descricao: string;
  readonly tipoProcesso: {id: number, nome: string};
  readonly ativo: boolean;
  readonly linkVirtual: boolean;
  readonly idMigracao: number;
  readonly ativoDePara: boolean;
  readonly descricaoMigracao: string;
}
