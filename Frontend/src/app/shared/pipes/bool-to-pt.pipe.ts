import { Pipe, PipeTransform } from '@angular/core';
import { isBoolean } from 'util';

@Pipe({
  name: 'boolToPT'
})
export class BoolToPTPipe implements PipeTransform {

  transform(value: any): any {
    return isBoolean(value) ? (value ? 'Sim' : 'Não') : value;
  }

}
