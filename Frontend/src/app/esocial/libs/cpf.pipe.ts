import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'cpf'
})
export class CpfPipe implements PipeTransform {
  transform(value: any): string {
    if (!value) {
      return '';
    }

    // Remove tudo que não é dígito do valor informado
    const cpf = value.toString().replace(/[^\d]/g, '');

    // Formata o CPF com pontos e traço
    return `${cpf.slice(0, 3)}.${cpf.slice(3, 6)}.${cpf.slice(6, 9)}-${cpf.slice(9)}`;
  }
}
