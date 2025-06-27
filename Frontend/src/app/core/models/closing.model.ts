export class Closing {

    constructor( closingId: string, nome: string) {
      this.id = closingId;
      this.nome = nome;
      this.descricao = `${ closingId } - ${ nome }`;
    }
  
    id: string;
    nome: string;
    descricao: string;
  }
  