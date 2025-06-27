import { Indice } from "./indice-model";

export class IndicesVigencias {
  public constructor(
    processoId: number,
    dataVigencia: Date,
    indice: Indice,
    descricao: String
  
  ) {
    this.processoId = processoId;
    this.dataVigencia = dataVigencia;
    this.indice = indice;
    this.descricao = this.indice.descricao
    
  }

  readonly processoId: number;
  readonly dataVigencia: Date;
  readonly indice: Indice;
  readonly descricao: String
 

  static fromObj(obj: IndicesVigencias): IndicesVigencias {
    return new IndicesVigencias(obj.processoId, obj.dataVigencia, obj.indice, obj.descricao);
  }
}
