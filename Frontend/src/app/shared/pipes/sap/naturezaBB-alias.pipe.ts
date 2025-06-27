import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'naturezaBBAlias'
})
export class NaturezaBBAliasPipe implements PipeTransform {

  transform(key: string): string {
    return {
      id: 'Código',
      codigoBB: 'Natureza Ação BB',
      descricao: 'Descrição',

    }[key];
  }

}
