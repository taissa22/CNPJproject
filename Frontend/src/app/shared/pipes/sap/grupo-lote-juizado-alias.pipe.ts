import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'grupoLoteJuizadoAlias'
})
export class GrupoLoteJuizadoAliasPipe implements PipeTransform {

  transform(key: string): string {
    return {
      descricao: 'Grupo de Lote de Juizado',

    }[key];
  }

}
