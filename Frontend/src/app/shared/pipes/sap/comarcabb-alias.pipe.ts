import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'comarcabbalias'
})
export class ComarcabbAliasPipe implements PipeTransform {

  transform(key: string): string {
    return {
      id : 'Código',
      descricao: 'Descrição',
      codigoBB: 'Comarca BB',
      codigoEstado : 'UF',
    }[key];

  }

}
