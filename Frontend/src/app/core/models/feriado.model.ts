import { isNullOrUndefined } from 'util';
import { TipoFeriado } from './tipoFeriado.model';

export class Feriado  {
  id: number;
  data: Date;
  tipoFeriado: TipoFeriado;

  constructor(id: number, data: Date, tipoFeriado?: any) {
    this.id = id;
    this.tipoFeriado = isNullOrUndefined(tipoFeriado) ? null : new TipoFeriado(tipoFeriado.id, tipoFeriado.descricao);
    this.data = new Date(data.getFullYear(), data.getMonth(), data.getDate());
  }
}
