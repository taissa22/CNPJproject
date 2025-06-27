import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'cnpj'
})
export class CnpjPipe implements PipeTransform {
  transform(value: any): string {
    if (!value) {
      return '';
    }

    // Remove tudo que não é dígito do valor informado
    const cnpj = value.toString().replace(/[^\d]/g, '');

    // Formata o CNPJ com pontos e traço
    return `${cnpj.slice(0, 2)}.${cnpj.slice(2, 5)}.${cnpj.slice(5, 8)}/${cnpj.slice(8,12)}-${cnpj.slice(12,14)}`;
  }
}