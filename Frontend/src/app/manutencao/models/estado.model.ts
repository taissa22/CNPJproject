export class Estado {

    constructor(id: string, nome: string, indiceId = 0 , valorDeJuros = 0, selecionado = false) {
      this.id = id;
      this.nome = nome;
      this.indiceId = indiceId;
      this.valorJuros = valorDeJuros;
      this.descricao = `${ id } - ${ nome }`;
      this.selecionado = selecionado;
    }

    id: string;
    nome: string;
    indiceId : number;
    valorJuros : number;
    descricao : string;
    selecionado : boolean;

    public static fromObj(obj :any){
       if(!obj) return null;
       return new Estado(obj.id, obj.nome, obj.indiceId , obj.valorJuros, false);
    }
  }
