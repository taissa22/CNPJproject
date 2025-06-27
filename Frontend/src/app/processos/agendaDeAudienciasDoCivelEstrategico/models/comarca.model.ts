import { Estado } from './estado.model';
export class Comarca {
  constructor() {
    this.id = -1;
    this.nome = '';
    this.estado = null;
  }
  id: number;
  nome: string;
  estado: Estado;
}
