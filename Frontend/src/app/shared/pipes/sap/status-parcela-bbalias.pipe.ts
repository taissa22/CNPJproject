import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'StatusParcelaBBAlias'
})
export class StatusParcelaBBAliasPipe implements PipeTransform {

  transform(key: string): string {
    return {
      id: 'Código',
      codigoBB: 'Status Parcela BB',
      descricao: 'Descrição',


    }[key];
  }

}
