import { Comarca } from './comarca.model';
import { TipoVara } from './tipo-vara.model';

export class Vara {
  constructor(numero: number, nome: string, comarca: Comarca, tipoVara: TipoVara) {
    this.numero = numero;
    this.nome = nome;
    this.comarca = comarca;
    this.tipoVara = tipoVara;
  }

  numero: number;
  nome: string;
  comarca: Comarca;
  tipoVara: TipoVara;

  get nomeDeExibicao(): string {
    return this.numero + 'ª ' + this.tipoVara.nome;
  }
}
