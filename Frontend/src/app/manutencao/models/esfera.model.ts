export class Esfera {
    private constructor(id: number, nome: string, corrigePrincipal: boolean, corrigeMultas: boolean, corrigeJuros:  boolean) {
      this.id = id;
      this.nome = nome;
      this.corrigePrincipal = corrigePrincipal;
      this.corrigeMultas = corrigeMultas;
      this.corrigeJuros = corrigeJuros;      
    }
  
    readonly id: number;
    readonly nome: string;
    readonly corrigePrincipal: boolean;
    readonly corrigeMultas: boolean;
    readonly corrigeJuros: boolean;
    
  
    static fromObj(obj: any): Esfera {
      return new Esfera(obj.id, obj.nome, obj.corrigePrincipal, obj.corrigeMultas, obj.corrigeJuros);
    }
}


  
