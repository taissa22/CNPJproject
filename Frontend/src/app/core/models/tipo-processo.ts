export class TipoProcesso {
  descricao: string;
  id: number;

  
  static fromObj(obj: TipoProcesso): TipoProcesso { return ({id: obj.id,descricao: obj.descricao}); }
}
