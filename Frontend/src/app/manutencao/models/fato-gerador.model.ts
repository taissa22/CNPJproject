import { TiposProcesso } from "@manutencao/services/tipos-de-processos";

export class FatoGerador {
  private constructor(    
    id: number,
    nome: string,   
    ativo: boolean,
    tipoProcesso : {id : string, nome : string}
) {
  this.id = id;
  this.nome = nome;  
  this.ativo = ativo; 
  this.tipoProcesso = tipoProcesso;
}
  readonly id : number;
  readonly nome: string;  
  readonly ativo: boolean;  
  readonly tipoProcesso : {id : string, nome : string}

  static fromObj(obj: any): FatoGerador {
    return ({
      id : obj.id,
      nome : obj.nome,     
      ativo : obj.ativo,
      tipoProcesso : obj.tipoProcesso
    });
  }
}
