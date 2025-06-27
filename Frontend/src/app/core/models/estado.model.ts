export class Estado {

  constructor(id: string, nome: string) {
    this.id = id;
    this.nome = nome;
    this.descricao = `${ id } - ${ nome }`;
  }

  id: string;
  nome: string;
  descricao: string;
}
