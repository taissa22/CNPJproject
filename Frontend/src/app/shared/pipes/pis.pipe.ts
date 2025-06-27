import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'pis'
})
export class PisPipe implements PipeTransform {

  transform(value: string, ...args: any[]): any {
    if (value) {

      value = value + "";

      value = value.replace(/(\d{5})(\d{3})/, "$1-$2");
      return value;
    }

    return null;


  }

}
