export class DecisaoEvento {
  private constructor(    
    id: number,
    eventoId: number,
    descricao: string,
    riscoPerda: string,
    reverCalculo : boolean,
    perdaPotencial: string,
    ativo: boolean,
    decisaoDefault: boolean,
    
) {
  this.id = id;
  this.eventoId = eventoId;
  this.descricao = descricao;
  this.riscoPerda = riscoPerda;
  this.reverCalculo = reverCalculo;
  this.perdaPotencial = perdaPotencial;
  this.decisaoDefault = decisaoDefault; 
  this.ativo = ativo;
}
  readonly id : number;
  readonly eventoId : number;
  readonly descricao: string;
  readonly riscoPerda: string;
  readonly reverCalculo: boolean;
  readonly perdaPotencial: string;
  readonly decisaoDefault: boolean;
  readonly ativo: boolean;

  static fromObj(obj: any): DecisaoEvento {
    return ({
      id : obj.id,
      eventoId : obj.eventoId,
      descricao : obj.descricao,
      riscoPerda : obj.riscoPerda,
      reverCalculo : obj.reverCalculo,
      perdaPotencial : obj.perdaPotencial,
      decisaoDefault : obj.decisaoDefault,
      ativo : obj.ativo      
    });
  }



}
