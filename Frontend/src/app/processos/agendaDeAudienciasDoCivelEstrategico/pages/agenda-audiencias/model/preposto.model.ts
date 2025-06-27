import { Estado } from 'src/app/core/models/estado.model';
export class Preposto {
  ativo: boolean;
  civelEstrategico: boolean;
  estado: Estado;
  id: number;
  nome: string;

  constructor(id: number, nome: string, estadoId: string, estadoNome: string, civelEstrategico: boolean, ativo: boolean) {
    this.id = id;
    this.nome = nome;
    this.estado = new Estado(estadoId, estadoNome);
    this.civelEstrategico = civelEstrategico;
    this.ativo = ativo;
  }

  tratarNome() {
    let nome = `${ this.estado.id } - ${ this.nome }`;
    if (!this.ativo) nome += ' [INATIVO]';
    return nome;
  }
}
