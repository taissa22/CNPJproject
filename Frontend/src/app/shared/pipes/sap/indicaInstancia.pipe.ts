import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'indicaInstancia'
})
export class IndicaInstanciaPipe implements PipeTransform {

  transform(value: any): any {
    return (value) ? (value == 'E' ? 'ESTADUAL' : value == 'F' ? 'FEDERAL' :
      value == 'T' ? 'TRT' : value) : value;
  }

}
