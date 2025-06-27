export class ClassificacaoHierarquica {

    constructor( classificacaoId: string, nome: string) {
      this.id = classificacaoId;
      this.nome = nome;
      this.descricao = `${ classificacaoId } - ${ nome }`;
    }
  
    id: string;
    nome: string;
    descricao: string;
  }
  