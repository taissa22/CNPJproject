export class EventoDisponivel {
  private constructor(    
    id: number,
    label: string,
    selecionado: boolean,    
) {
  this.id = id;
  this.label = label;
  this.selecionado = selecionado;
 
}
  readonly id : number;
  readonly label: string;
  readonly selecionado: boolean;
 

  static fromObj(obj: any): EventoDisponivel {
    return ({
      id : obj.id,
      label : obj.label,
      selecionado : obj.selecionado      
    });
  }



}
