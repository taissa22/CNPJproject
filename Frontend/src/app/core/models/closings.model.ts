import { Closing } from './closing.model';
export abstract class Closings {
  static readonly PRE = new Closing('1','PRÉ');
  static readonly POS = new Closing('2','PÓS');
  static readonly HIBRIDO = new Closing('3','HÍBRIDO');
  static readonly NA = new Closing('4','N/A');
  static readonly ADEFINIR = new Closing('0','A DEFINIR');

  static obterClosings() {
    return [
      this.PRE,
      this.POS,
      this.HIBRIDO,
      this.NA,
      this.ADEFINIR
    ];
  }
}
