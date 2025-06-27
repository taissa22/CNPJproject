import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'percentualAtmAlias'
})
export class PercentualAtmAliasPipe implements PipeTransform {
  transform(key: string): string {
    return {
      uf: 'UF',
      percentual: '% ATM'
    }[key];
  }
}
