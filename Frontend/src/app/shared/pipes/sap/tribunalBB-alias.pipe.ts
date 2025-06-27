import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'tribunalBBAlias'
})
export class TribunalBBAliasPipe implements PipeTransform {

  transform(key: string): string {
    return {
      id: 'Código',
      codigoBB: 'Tribunal BB',
      indicadorInstancia: 'Instância Designada',
      descricao: 'Descrição do Tribunal',

    }[key];
  }
}

