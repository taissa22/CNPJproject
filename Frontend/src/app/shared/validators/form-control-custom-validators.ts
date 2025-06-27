import { Injectable } from '@angular/core';
import { AbstractControl, Validators } from '@angular/forms';
import { dateMaxValid } from '@manutencao/pages/partes/pages/manter/manter.component';
import { debug } from 'console';
import { CabecalhoLancamentosHonorarioCivelEstrategico } from 'src/app/processos/models/cabecalho-tabela-log-civel-estrategico/lancamentos-honorario-estrategico';

@Injectable({
  providedIn: 'root'
})
export class FormControlCustomValidators {
  telefoneValido(tamanhoMinimo: number) {
    return (control: AbstractControl): Validators => {
      let telefone = control.value;

      if (telefone === null || telefone === '') {
        return null;
      }

      control.setErrors(null);

      telefone = telefone.replace(/[^0-9]+/g, '');

      if (telefone.length < tamanhoMinimo) {
        return { telefoneInvalido: true };
      }

      return null;
    };
  }

  cpfValido() {
    return (control: AbstractControl): Validators => {
      const re = /\D/g;
      let cpf = control.value;
      if (cpf) {
        cpf = cpf.replace(re, '');
        let numbers;
        let digits;
        let sum;
        let i;
        let result;
        let equalDigits;
        equalDigits = 1;
        if (cpf.length < 11) {
          return { cpfNotValid: true };
        }

        for (i = 0; i < cpf.length - 1; i++) {
          if (cpf.charAt(i) !== cpf.charAt(i + 1)) {
            equalDigits = 0;
            break;
          }
        }

        if (!equalDigits) {
          numbers = cpf.substring(0, 9);
          digits = cpf.substring(9);
          sum = 0;
          for (i = 10; i > 1; i--) {
            sum += numbers.charAt(10 - i) * i;
          }

          result = sum % 11 < 2 ? 0 : 11 - (sum % 11);

          if (result !== Number(digits.charAt(0))) {
            return { cpfNotValid: true };
          }
          numbers = cpf.substring(0, 10);
          sum = 0;

          for (i = 11; i > 1; i--) {
            sum += numbers.charAt(11 - i) * i;
          }
          result = sum % 11 < 2 ? 0 : 11 - (sum % 11);

          if (result !== Number(digits.charAt(1))) {
            return { cpfNotValid: true };
          }
          return null;
        } else {
          return { cpfNotValid: true };
        }
      }
      return null;
    };
  }

  cpfValidoSisjur() {
    return (control: AbstractControl): Validators => {
      const re = /\D/g;
      let cpf = control.value;
      if (cpf) {
        cpf = cpf.replace(re, '');
        let numbers;
        let digits;
        let sum;
        let i;
        let result;
        let equalDigits;
        equalDigits = 1;
        if (cpf.length < 11) {
          return { cpfNotValid: true };
        }

        if (cpf !== '11111111111' && cpf !== '99999999999') {
          for (i = 0; i < cpf.length - 1; i++) {
            if (cpf.charAt(i) !== cpf.charAt(i + 1)) {
              equalDigits = 0;
              break;
            }
          }

          if (!equalDigits) {
            numbers = cpf.substring(0, 9);
            digits = cpf.substring(9);
            sum = 0;
            for (i = 10; i > 1; i--) {
              sum += numbers.charAt(10 - i) * i;
            }

            result = sum % 11 < 2 ? 0 : 11 - (sum % 11);

            if (result !== Number(digits.charAt(0))) {
              return { cpfNotValid: true };
            }
            numbers = cpf.substring(0, 10);
            sum = 0;

            for (i = 11; i > 1; i--) {
              sum += numbers.charAt(11 - i) * i;
            }
            result = sum % 11 < 2 ? 0 : 11 - (sum % 11);

            if (result !== Number(digits.charAt(1))) {
              return { cpfNotValid: true };
            }
            return null;
          } else {
            return { cpfNotValid: true };
          }
        }
      }
      return null;
    };
  }

  cnpjValido() {
    return (control: AbstractControl): Validators => {
      const re = /\D/g;
      let cnpj = control.value;
      if (cnpj) {
        cnpj = cnpj.replace(re, '');
        if (cnpj.length !== 14) {
          return { cnpjNotValid: true };
        }

        // Elimina CNPJs invalidos conhecidos
        if (
          cnpj === '00000000000000' ||
          cnpj === '11111111111111' ||
          cnpj === '22222222222222' ||
          cnpj === '33333333333333' ||
          cnpj === '44444444444444' ||
          cnpj === '55555555555555' ||
          cnpj === '66666666666666' ||
          cnpj === '77777777777777' ||
          cnpj === '88888888888888' ||
          cnpj === '99999999999999'
        ) {
          return { cnpjNotValid: true };
        }

        // Valida DVs
        let tamanho;
        let numeros;
        let digitos;
        let soma;
        let pos;
        let i;
        let resultado;

        tamanho = cnpj.length - 2;
        numeros = cnpj.substring(0, tamanho);
        digitos = cnpj.substring(tamanho);
        soma = 0;
        pos = tamanho - 7;

        for (i = tamanho; i >= 1; i--) {
          soma += numeros.charAt(tamanho - i) * pos--;
          if (pos < 2) {
            pos = 9;
          }
        }
        resultado = soma % 11 < 2 ? 0 : 11 - (soma % 11);
        // tslint:disable-next-line: triple-equals
        if (resultado != digitos.charAt(0)) {
          return { cnpjNotValid: true };
        }

        tamanho = tamanho + 1;
        numeros = cnpj.substring(0, tamanho);
        soma = 0;
        pos = tamanho - 7;
        for (i = tamanho; i >= 1; i--) {
          soma += numeros.charAt(tamanho - i) * pos--;
          if (pos < 2) {
            pos = 9;
          }
        }
        resultado = soma % 11 < 2 ? 0 : 11 - (soma % 11);
        // tslint:disable-next-line: triple-equals
        if (resultado != digitos.charAt(1)) {
          return { cnpjNotValid: true };
        }
        return null;
      }
      return null;
    };
  }

  cnpjValidoSisjur() {
    return (control: AbstractControl): Validators => {
      const re = /\D/g;
      let cnpj = control.value;
      if (cnpj) {
        cnpj = cnpj.replace(re, '');
        if (cnpj.length !== 14) {
          return { cnpjNotValid: true };
        }

        if (cnpj === '11111111111111' || cnpj === '99999999999999') {
          return null;
        }

        // Elimina CNPJs invalidos conhecidos
        if (
          cnpj === '00000000000000' ||
          cnpj === '11111111111111' ||
          cnpj === '22222222222222' ||
          cnpj === '33333333333333' ||
          cnpj === '44444444444444' ||
          cnpj === '55555555555555' ||
          cnpj === '66666666666666' ||
          cnpj === '77777777777777' ||
          cnpj === '88888888888888' ||
          cnpj === '99999999999999'
        ) {
          return { cnpjNotValid: true };
        }

        // Valida DVs
        let tamanho;
        let numeros;
        let digitos;
        let soma;
        let pos;
        let i;
        let resultado;

        tamanho = cnpj.length - 2;
        numeros = cnpj.substring(0, tamanho);
        digitos = cnpj.substring(tamanho);
        soma = 0;
        pos = tamanho - 7;

        for (i = tamanho; i >= 1; i--) {
          soma += numeros.charAt(tamanho - i) * pos--;
          if (pos < 2) {
            pos = 9;
          }
        }
        resultado = soma % 11 < 2 ? 0 : 11 - (soma % 11);
        // tslint:disable-next-line: triple-equals
        if (resultado != digitos.charAt(0)) {
          return { cnpjNotValid: true };
        }

        tamanho = tamanho + 1;
        numeros = cnpj.substring(0, tamanho);
        soma = 0;
        pos = tamanho - 7;
        for (i = tamanho; i >= 1; i--) {
          soma += numeros.charAt(tamanho - i) * pos--;
          if (pos < 2) {
            pos = 9;
          }
        }
        resultado = soma % 11 < 2 ? 0 : 11 - (soma % 11);
        // tslint:disable-next-line: triple-equals
        if (resultado != digitos.charAt(1)) {
          return { cnpjNotValid: true };
        }
        return null;
      }
      return null;
    };
  }

  cpfDuplicado(cpf: string) {
    return (control: AbstractControl): Validators => {
      const re = /\D/g;

      let input = control.value;

      if (!cpf || !input) return null;

      cpf = cpf.replace(re, '');

      input = input.replace(re, '');

      if (cpf == input) {
        {
          return { cpfDuplicado: true };
        }
      }

      return null;
    };
  }

  periodoDeApuracao(data: string) {
    return (control: AbstractControl): Validators => {
      if (!data) {
        return null;
      }
      const inputYear = parseInt(data.substring(0, 4), 10);
      const inputMonth = parseInt(data.substring(4, 6), 10);

      const now = new Date();
      const currentYear = now.getFullYear();
      const currentMonth = now.getMonth() + 1;

      if (inputYear > currentYear) {
        return { periodoInvalido: true };
      } else if (inputYear === currentYear && inputMonth > currentMonth) {
        return { periodoInvalido: true };
      }

      return null;
    };
  }

  periodInvalid() {
    return (control: AbstractControl): Validators => {
      let period = control.value;
      if (!period) return null;

      let periodInvalidMax = new Date(
        new Date().getFullYear().toString() +
          '-' +
          (new Date().getMonth() + 1).toString() +
          '-02'
      );

      if (periodInvalidMax < period) {
        return { periodInvalid: true };
      }

      return null;
    };
  }
}
