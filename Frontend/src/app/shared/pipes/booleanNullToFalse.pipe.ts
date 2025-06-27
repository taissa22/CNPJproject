import { Pipe, PipeTransform } from '@angular/core';
import { isBoolean } from 'util';

@Pipe({
  name: 'booleanNullToFalse'
})
export class BooleanNullToFalsePipe implements PipeTransform {

  transform(value: any): any {
    return isBoolean(value) ? value ? false : true : null;
  }

}
