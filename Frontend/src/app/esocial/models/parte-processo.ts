import { StatusReclamanteEsocial } from '../enum/status-reclamante';
import { Formulario2500 } from './formulario-2500';
import { Formulario2501 } from './formulario-2501';

export class ParteProcesso {
  private constructor(codProcesso: number, codParte: number, nomeParte: string, cpfParte: string, statusReclamante: number, finalizadoEscritorio: boolean, finalizadoContador: boolean, formulario2500: Formulario2500, listaFormularios2501: Array<Formulario2501>) {
    this.codProcesso = codProcesso;
    this.codParte = codParte;
    this.nomeParte = nomeParte;
    this.cpfParte = cpfParte;
    this.statusReclamante = StatusReclamanteEsocial[statusReclamante];
    this.finalizadoEscritorio = finalizadoEscritorio;
    this.finalizadoContador = finalizadoContador;
    this.formulario2500 = Formulario2500.fromObj(formulario2500);
    this.listaFormularios2501 = listaFormularios2501 ? listaFormularios2501.map(x => Formulario2501.fromObj(x)) : null;
  }

  readonly codProcesso: number;
  readonly codParte: number;
  readonly nomeParte: string;
  readonly cpfParte: string;
  readonly statusReclamante: string;
  readonly finalizadoEscritorio: boolean;
  readonly finalizadoContador: boolean;
  readonly formulario2500: Formulario2500;
  readonly listaFormularios2501: Array<Formulario2501>;

  static fromObj(obj: any): ParteProcesso {
    return new ParteProcesso(obj.codProcesso, obj.codParte, obj.nomeParte, obj.cpfParte, obj.statusReclamante, obj.finalizadoEscritorio, obj.finalizadoContador, obj.formulario2500, obj.listaFormularios2501);
  }
}



