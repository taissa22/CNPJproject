export class Objeto {
  private constructor(
    id: number,
    descricao: string,
    tipoProcesso: {id: number; nome: string; nomeEnum: string},
    ehTributarioAdministrativo: boolean,
    ehTributarioJudicial: boolean,
    ativoTributarioAdministrativo: boolean,
    ativoTributarioJudicial: boolean,
    ehTrabalhistaAdministrativo: boolean,
    grupoPedidoId : number,
    grupoPedidoDescricao : string

) {
  this.id = id;  
  this.descricao = descricao;
  this.tipoProcesso = tipoProcesso;
  this.ehTributarioAdministrativo  =  ehTributarioAdministrativo;
  this.ehTributarioJudicial  = ehTributarioJudicial;
  this.ativoTributarioAdminstrativo  =  ativoTributarioAdministrativo;
  this.ativoTributarioJudicial  = ativoTributarioJudicial;
  this.ehTrabalhistaAdministrativo = ehTrabalhistaAdministrativo;
  this.grupoPedidoId = grupoPedidoId ? grupoPedidoId : 0;
  this.grupoPedidoDescricao = grupoPedidoDescricao;
}
  readonly id: number;
  readonly descricao: string;
  readonly tipoProcesso: {id: number; nome: string; nomeEnum: string};
  readonly ehTributarioAdministrativo: boolean;
  readonly ehTributarioJudicial: boolean;
  readonly ativoTributarioAdminstrativo: boolean;
  readonly ativoTributarioJudicial: boolean;
  readonly ehTrabalhistaAdministrativo: boolean;
  readonly grupoPedidoId : number;
  readonly grupoPedidoDescricao : string;

  static fromObj(obj: any): Objeto {
    return ({
      id: obj.id,
      descricao: obj.descricao,
      tipoProcesso: obj.tipoProcesso,
      ehTributarioAdministrativo: obj.ehTributarioAdministrativo,
      ehTributarioJudicial: obj.ehTributarioJudicial,
      ativoTributarioAdminstrativo: obj.ativoTributarioAdministrativo,
      ativoTributarioJudicial: obj.ativoTributarioJudicial,  
      ehTrabalhistaAdministrativo: obj.ehTrabalhistaAdministrativo,
      grupoPedidoId: obj.grupoPedidoId,
      grupoPedidoDescricao: obj.grupoPedidoDescricao       
    });
  }



}
