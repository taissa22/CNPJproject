import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'toLocalDateIfDate'
})
export class ToLocalDateIfDatePipe implements PipeTransform {

  transform(value: any): any {
    const isValidDate = (d: any) => d instanceof Date && !isNaN(+d);
    const regExp = /(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2}):(\d{2})/;
    let optionsLocale = { year: 'numeric', month: 'numeric', day: 'numeric' };
    let aux;
    if (typeof value === 'string' && isNaN(+value) && regExp.test(value)) {
      try {
         aux = new Date(value);
         value = isValidDate(aux) ? aux : value; 
      } catch(e) {}
    }
    return (value instanceof Date)? value.toLocaleString('pt-BR', optionsLocale) : value;
  }

}
