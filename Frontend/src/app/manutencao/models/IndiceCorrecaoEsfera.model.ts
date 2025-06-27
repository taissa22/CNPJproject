import { Indice } from "./indice-model";

export class IndiceCorrecaoEsfera {
    private constructor(esferaid: number, datavigencia: Date, indiceid: number, indice : Indice) {
      this.esferaId = esferaid;
      this.dataVigencia = datavigencia;
      this.indiceId = indiceid;  
      this.indice = indice;
    }
  
    readonly esferaId: number;
    readonly dataVigencia: Date;
    readonly indiceId: number;
    readonly indice : Indice;
    
  
    static fromObj(obj: any): IndiceCorrecaoEsfera {
      return new IndiceCorrecaoEsfera(obj.esferaId, obj.dataVigencia, obj.indiceId, Indice.fromObj(obj.indice));
    }
}


  
