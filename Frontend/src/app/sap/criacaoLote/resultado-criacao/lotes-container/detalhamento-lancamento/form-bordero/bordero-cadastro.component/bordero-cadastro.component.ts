import { FormBorderoService } from '../form-bordero.service';
import { Component, OnInit } from '@angular/core';
import {
  FormGroup,
  Validators,
  FormBuilder,

} from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap';
import { HelperAngular } from '@shared/helpers/helper-angular';
import { LoteCriacaoBorderoDto } from '@shared/interfaces/lote-criacao-bordero-dto';
import { Subscription } from 'rxjs';
import { CriacaoService } from 'src/app/sap/criacaoLote/criacao.service';
import { LoteService } from 'src/app/core/services/sap/lote.service';
import { take } from 'rxjs/operators';
import { AbstractControl } from '@angular/forms';

@Component({
  selector: 'app-bordero-cadastro.component',
  templateUrl: './bordero-cadastro.component.html',
  styleUrls: ['./bordero-cadastro.component.scss']
})
export class BorderoCadastroComponent implements OnInit {
  public isCPF = true;
  titulo = '';
  bordero: LoteCriacaoBorderoDto;
  modoSalvar = 'Cadastrar';

  registerForm: FormGroup;
  bodyDeletarBordero = '';

  dataAtual: string;

  // tslint:disable-next-line: variable-name
  _filtroLista = '';
  subscriptions: Subscription[] = [];

  constructor(private fb: FormBuilder,
              public bsModalRef: BsModalRef,
              private criacaoService: CriacaoService,
              private helpAngular: HelperAngular,
              private service: FormBorderoService,
              private loteService: LoteService,) { }

  ngOnInit() {
    this.titulo =
      this.modoSalvar === 'Editar' ? 'Editar Borderô' : 'Incluir Borderô';
    this.validation();
    this.getBorderos();
    this.registerForm.get('cpfBeneficiario').setValidators([Validators.required, Validators.minLength(11), this.cpfValido()]);
    this.setCPFCNPJValidators();

    this.criacaoService.borderoSelecionado.subscribe(i => {
      if (i) {
        if (i.cpfBeneficiario) {
          this.isCPF = true;
          this.registerForm.get('cnpjBeneficiario').setValue('');
        } else {
          this.isCPF = false;
          this.registerForm.get('cpfBeneficiario').setValue('');
        }
        this.modoSalvar = 'Editar';
        this.editarBordero(i);
      }
     }
    );


  }

  setCPFCNPJValidators() {
    const cpfControl = this.registerForm.get('cpfBeneficiario');
    const cnpjControl = this.registerForm.get('cnpjBeneficiario');
    this.registerForm.get('cpfcnpj').valueChanges.subscribe(tipoCadastro => {
      if (tipoCadastro === 'cpf') {
        cnpjControl.setValidators(null);
        cpfControl.setValidators([Validators.required, Validators.minLength(11), this.cpfValido()]);
      }

      if (tipoCadastro === 'cnpj') {
        cpfControl.setValidators(null);
        cnpjControl.setValidators([Validators.required, Validators.minLength(14), this.cnpjValido()]);
      }

      cpfControl.updateValueAndValidity();
      cnpjControl.updateValueAndValidity();
    });
  }

  valorCPFCNPJ(val: string) {
    if (val === 'cpf') {
      this.isCPF = true;
      this.registerForm.get('cnpjBeneficiario').setValue('');
    } else {
      this.isCPF = false;
      this.registerForm.get('cpfBeneficiario').setValue('');

    }
  }

  validation() {
    this.registerForm = this.fb.group({
      nomeBeneficiario: ['', [Validators.required, Validators.maxLength(30)]],
      cnpjBeneficiario: [null],
      cpfBeneficiario: [null],
      cpfcnpj: ['cpf'],
      numeroBancoBeneficiario: [
        '',
        [Validators.required, Validators.maxLength(3), this.somenteNumeros()]
      ],
      digitoBancoBeneficiario: [
        '',
        [Validators.required, Validators.maxLength(1), this.somenteNumeros()]
      ],
      numeroAgenciaBeneficiario: [
        '',
        [Validators.required, Validators.maxLength(11), this.somenteNumeros()]
      ],
      digitoAgenciaBeneficiario: [
        '',
        [Validators.required, Validators.maxLength(1), this.somenteNumeros()]
      ],
      numeroContaCorrenteBeneficiario: [
        '',
        [Validators.required, Validators.maxLength(18), this.somenteNumeros()]
      ],
      digitoContaCorrenteBeneficiario: [
        '',
        [Validators.required, Validators.maxLength(1), this.somenteNumeros()]
      ],
      valor: ['', [Validators.required, Validators.min(0.01)]],
      cidadeBeneficiario: ['', [Validators.required, Validators.maxLength(50)]],
      comentario: ['']
    });
  }

  validacaoTextos(nomeControl: string, nomeCampo: string) {
    if (
      this.registerForm.invalid &&
      this.registerForm.get(nomeControl).touched &&
      this.registerForm.get(nomeControl).hasError('required')
    ) {
      return `${nomeCampo} é obrigatório!`;
    }
    if (
      this.registerForm.invalid &&
      this.registerForm.get(nomeControl).touched &&
      this.registerForm.get(nomeControl).hasError('minlength')
    ) {
      return `${nomeCampo} deve possuir no mínimo
                  ${
                    this.registerForm.get(nomeControl).errors.minlength
                      .requiredLength
                  } caracteres!`;
    }
    if (
      this.registerForm.invalid &&
      this.registerForm.get(nomeControl).touched &&
      this.registerForm.get(nomeControl).hasError('maxlength')
    ) {
      return `${nomeCampo} deve possuir no máximo
                  ${
                    this.registerForm.get(nomeControl).errors.maxlength
                      .requiredLength
                  } caracteres!`;
    }
    if (
      this.registerForm.invalid &&
      this.registerForm.get(nomeControl).touched &&
      this.registerForm.get(nomeControl).hasError('min')
    ) {
      return `O ${nomeCampo} deve ser maior ou igual a
                  ${
                    this.registerForm.get(nomeControl).errors.min.min
                  }!`;
    }
    if (
      this.registerForm.invalid &&
      this.registerForm.get(nomeControl).touched &&
      this.registerForm.get(nomeControl).hasError('cnpjNotValid')
    ) {
      return `${nomeCampo} informado inválido!`;
    }
    if (
      this.registerForm.invalid &&
      this.registerForm.get(nomeControl).touched &&
      this.registerForm.get(nomeControl).hasError('cpfNotValid')
    ) {
      return `${nomeCampo} informado inválido!`;
    }
    if (
      this.registerForm.invalid &&
      this.registerForm.get(nomeControl).touched &&
      this.registerForm.get(nomeControl).hasError('valorInvalido')
    ) {
      return `Valor do campo ${nomeCampo} inválido!`;
    }
  }

  editarBordero(bordero: LoteCriacaoBorderoDto) {
    this.titulo = 'Editar Borderô';
    this.bordero = Object.assign({}, bordero);
    this.registerForm.patchValue(this.bordero);
  }


  salvarAlteracao() {
    if (this.registerForm.valid) {
      if (this.modoSalvar === 'Cadastrar') {
        // inclusão de borderô
        this.bordero = Object.assign({}, this.registerForm.value);
        // adicionar o novo bordero na lista para ser salvo futuramente;
        let tamanhoLista = this.criacaoService.getlistaValoresBordero().length + 1;
        if (tamanhoLista === 1) {
          this.bordero.seq_Bordero = 1;
        }
        this.criacaoService.getlistaValoresBordero()
          .map(item => {
            if (item.seq_Bordero >= tamanhoLista) {
              this.bordero.seq_Bordero = item.seq_Bordero++;
            } else {
              this.bordero.seq_Bordero = tamanhoLista++;
            }
            });
        this.bordero.codigoLote = 0;
        this.criacaoService.setlistaValoresBordero(this.bordero);
        this.bsModalRef.hide();
      } else {
        this.bordero = Object.assign(
          { id: this.bordero.seq_Bordero },
          this.registerForm.value
        );
        const listanovaBordero: any[] = this.criacaoService.getlistaValoresBordero();
        listanovaBordero.forEach((item, index) => {
          if (item === this.criacaoService.borderoSelecionado.value) {
            listanovaBordero.splice(index, 1, this.bordero);
          }
        });
        this.criacaoService.borderosSubject.next(listanovaBordero);
        this.bsModalRef.hide();

      }
      this.criacaoService.borderoSelecionado.next(null);
    }
    this.service.onBorderoChanges.next(true);
  }

  getBorderos() {
    return this.criacaoService.getlistaValoresBordero();
    // faz a chamada de lista de borderos
  }

  desabilitaTooltip(nomeControl: string): boolean {
    return this.registerForm.get(nomeControl).untouched || this.registerForm.get(nomeControl).valid;
  }

  cpfValido() {
    return (control: AbstractControl): Validators => {
      const re = /\./gi;
      let cpf = control.value;
      if (cpf) {
        cpf = cpf.replace(re, '');
        cpf = cpf.replace('-', '');
        let numbers;
        let digits;
        let sum;
        let i;
        let result;
        let equalDigits;
        equalDigits = 1;
        if (cpf.length < 11) {
          return null;
        }

        // if (cpf === '11111111111' || cpf === '99999999999') {
        //   return null;
        // }


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

  cnpjValido() {
    return (control: AbstractControl): Validators => {
      const re = /\./gi;
      let cnpj = control.value;
      if (cnpj) {
        cnpj = cnpj.replace(re, '');
        cnpj = cnpj.replace('-', '');
        cnpj = cnpj.replace('/', '');
        if (cnpj.length !== 14) {
          return null;
        }

        // if (cnpj === '11111111111111' || cnpj === '99999999999999') {
        //   return null;
        // }

        // Elimina CNPJs invalidos conhecidos
        if (cnpj === '00000000000000' ||
          cnpj === '22222222222222' ||
          cnpj === '33333333333333' ||
          cnpj === '44444444444444' ||
          cnpj === '55555555555555' ||
          cnpj === '66666666666666' ||
          cnpj === '77777777777777' ||
          cnpj === '88888888888888') {
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

  somenteNumeros() {
    return (control: AbstractControl): Validators => {
      const re = /\D/g;
      const valorBase = control.value;
      let valor = control.value;
      if (valor) {
        valor = valor.replace(re, '');

        if (valor.length !== valorBase.length) {
          return { valorInvalido: true };
        }
      }
      return null;
    };
  }
}
