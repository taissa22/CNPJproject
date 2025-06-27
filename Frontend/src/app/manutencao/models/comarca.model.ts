import { ComarcaBB } from "./comarca-bb.model";
import { Estado } from "./estado.model";
import { Vara } from "./vara.model";

export class Comarca{
  private constructor(
    id: number,            
    nome: string,      
    estadoId:string,    
    escritorioCivelId:number,
    escritorioTrabalhistaId:number,
    profissionalCivelEstrategicoId:number,
    comarcaBBId:number,
    comarcaBB: ComarcaBB,
    varas : Vara[]
  ) {
    this.id = id;       
    this.nome = nome;      
    this.escritorioCivelId = escritorioCivelId;
    this.escritorioTrabalhistaId = escritorioTrabalhistaId;
    this.profissionalCivelEstrategicoId = profissionalCivelEstrategicoId;
    this.comarcaBBId = comarcaBBId;
    this.comarcaBB = ComarcaBB.fromObj(comarcaBB);
    this.varas = varas;
  }

  readonly id : number = 0;
  readonly nome : string = "";
  readonly estado :Estado = null;
  readonly escritorioCivelId :number = 0;
  readonly escritorioTrabalhistaId :number = 0;
  readonly profissionalCivelEstrategicoId:number = 0;   
  readonly comarcaBBId : number = 0;
  readonly comarcaBB : ComarcaBB = null;
  readonly varas : Vara[] = [];

  static fromObj(obj: any): Comarca {
    if(!obj) return null;

    let varas:Vara[] = [];
    if(obj.varas && obj.varas.length) obj.varas.map(v => { varas.push(Vara.fromObj(v)); });

    return ({
     id: obj.id,       
     nome: obj.nome,      
     estado: Estado.fromObj(obj.estado),
     escritorioCivelId: obj.escritorioCivelId,
     escritorioTrabalhistaId: obj.escritorioTrabalhistaId,
     profissionalCivelEstrategicoId: obj.profissionalCivelEstrategicoId,
     comarcaBBId: obj.comarcaBBId,
     comarcaBB : obj.comarcaBB ? ComarcaBB.fromObj(obj.comarcaBB) : null,
     varas : varas
    });
  }
}
