import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'modalidadeprodutobbalias'
})
export class ModalidadeProdutoBbAliasPipe implements PipeTransform {

  transform(key: string): string {
    return {
      id: 'Código',
      codigoBB: 'Modalidade BB',
      descricao: 'Descrição',
    }[key];
  }

}
