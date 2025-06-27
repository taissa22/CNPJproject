import { isNullOrUndefined } from 'util';
import { Permissoes } from './../../../../../permissoes/permissoes';
import { UserService } from './../../../../../core/services/user.service';
import { Component, OnInit, Input } from '@angular/core';
import { FormGroup, FormControl, Validators, AbstractControl, ValidatorFn } from '@angular/forms';
import { NgbModalRef, NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { HttpErrorResult } from '@core/http/http-error-result';
import { DialogService } from '@shared/services/dialog.service';

import { Parte, EstadoEnum } from '@manutencao/models';
import { PartesService } from '@manutencao/services';

import { StaticInjector } from '@manutencao/static-injector';
import { Estados } from '@core/models';
import { BsLocaleService } from 'ngx-bootstrap';

@Component({
  selector: 'app-manter',
  templateUrl: './manter.component.html',
  styleUrls: ['./manter.component.scss']
})
export class ManterParteComponent implements OnInit {

  titulo: string = 'Inclusão de Parte';
  entidade: Parte;
  @Input() temPermissaoAlteraCartaFianca: boolean;

  tipoPessoaFormControl: FormControl = new FormControl('F', [Validators.required]);
  cpfFormControl: FormControl = new FormControl('', [Validators.required, this.cpfValido()]);
  carteiraTrabalhoFormControl: FormControl = new FormControl('', [Validators.maxLength(8)]);
  cnpjFormControl: FormControl = new FormControl('');
  nomeFormControl: FormControl = new FormControl('', [Validators.required, Validators.maxLength(400)]);
  telefoneFormControl: FormControl = new FormControl(null);
  celularFormControl: FormControl = new FormControl(null);
  valorCartaFiancaFormControl: FormControl = new FormControl(null, [Validators.maxLength(13)]);
  dataCartaFiancaFormControl: FormControl = new FormControl(null);
  enderecoFormControl: FormControl = new FormControl(null, [Validators.maxLength(400)]);
  enderecosAdicionaisFormControl: FormControl = new FormControl('', [Validators.maxLength(4000)]);
  telefonesAdicionaisFormControl: FormControl = new FormControl(null, [Validators.maxLength(4000)]);
  cidadeFormControl: FormControl = new FormControl('', [Validators.maxLength(30)]);
  estadoFormControl: FormControl = new FormControl('');
  bairroFormControl: FormControl = new FormControl('', [Validators.maxLength(30)]);
  cepFormControl: FormControl = new FormControl('');

  formulario = new FormGroup({
    tipoPessoa: this.tipoPessoaFormControl,
    cpf: this.cpfFormControl,
    carteiraTrabalho: this.carteiraTrabalhoFormControl,
    cnpj: this.cnpjFormControl,
    nome: this.nomeFormControl,
    telefone: this.telefoneFormControl,
    celular: this.celularFormControl,
    valorCartaFianca: this.valorCartaFiancaFormControl,
    dataCartaFianca: this.dataCartaFiancaFormControl,
    endereco: this.enderecoFormControl,
    enderecosAdicionais: this.enderecosAdicionaisFormControl,
    telefonesAdicionais: this.telefonesAdicionaisFormControl,
    cidade: this.cidadeFormControl,
    estado: this.estadoFormControl,
    bairro: this.bairroFormControl,
    cep: this.cepFormControl
  });

  estados: Array<EstadoEnum> = [];

  mascaraCelular = ['(', /[1-9]/, /\d/, ')', /\d/, /\d/, /\d/, /\d/, /\d/, '-', /\d/, /\d/, /\d/, /\d/];
  mascaraTelefone = ['(', /[1-9]/, /\d/, ')', /\d/, /\d/, /\d/, /\d/, '-', /\d/, /\d/, /\d/, /\d/];
  mascaraCep = [/[0-9]/, /[0-9]/, /[0-9]/, /[0-9]/, /[0-9]/, '-', /[0-9]/, /[0-9]/, /[0-9]/];
  mascaraCarteiraTrabalho = [/[0-9]/, /[0-9]/, /[0-9]/, /[0-9]/, /[0-9]/, /[0-9]/, /[0-9]/, /[0-9]/];
  // tslint:disable-next-line: max-line-length
  mascaraCnpj = [/[0-9]/, /[0-9]/, '.', /[0-9]/, /[0-9]/, /[0-9]/, '.', /[0-9]/, /[0-9]/, /[0-9]/, '/', /[0-9]/, /[0-9]/, /[0-9]/, /[0-9]/, '-', /[0-9]/, /[0-9]/];
  mascaraCpf = [/[0-9]/, /[0-9]/, /[0-9]/, '.', /[0-9]/, /[0-9]/, /[0-9]/, '.', /[0-9]/, /[0-9]/, /[0-9]/, '-', /[0-9]/, /[0-9]/];

  constructor(
    public activeModal: NgbActiveModal,
    private service: PartesService,
    private dialog: DialogService,
    private userService: UserService,
    private configLocalizacao: BsLocaleService,
  ) {
    this.configLocalizacao.use('pt-BR');
  }

  async ngOnInit(): Promise<void> {
    try {
      const usuario = this.userService.getCurrentUser();
      this.temPermissaoAlteraCartaFianca = !isNullOrUndefined(usuario.permissoes.filter(p => {
        return p === Permissoes.ALTERAR_CARTA_FIANCA;
      })[0]);

      this.estados = Estados.obterUfs();
      if (this.entidade) {
        const telefone = this.entidade.telefone ? this.entidade.telefoneDDD + this.entidade.telefone : null;
        const celular = this.entidade.celular ? this.entidade.celularDDD + this.entidade.celular : null;
        const cep = this.entidade.cep ? this.entidade.cep : null;

        this.tipoPessoaFormControl.setValue(this.entidade.tipoParte.valor);
        this.cpfFormControl.setValue(this.entidade.cpf);
        this.carteiraTrabalhoFormControl.setValue(this.entidade.carteiraDeTrabalho);
        this.cnpjFormControl.setValue(this.entidade.cnpj);
        this.nomeFormControl.setValue(this.entidade.nome);
        this.telefoneFormControl.setValue(telefone);
        this.celularFormControl.setValue(celular);
        this.valorCartaFiancaFormControl.setValue(this.entidade.valorCartaFianca);
        this.dataCartaFiancaFormControl.setValue(this.entidade.dataCartaFianca);
        this.enderecoFormControl.setValue(this.entidade.endereco);
        this.enderecosAdicionaisFormControl.setValue(this.entidade.enderecosAdicionais);
        this.telefonesAdicionaisFormControl.setValue(this.entidade.telefonesAdicionais);
        this.cidadeFormControl.setValue(this.entidade.cidade);
        this.estadoFormControl.setValue(this.entidade.estado.id);
        this.bairroFormControl.setValue(this.entidade.bairro);
        this.cepFormControl.setValue(cep);
      } else {
        this.tipoPessoaFormControl.setValue('F');
      }

      if (this.temPermissaoAlteraCartaFianca) {
        this.valorCartaFiancaFormControl.setValidators([Validators.required]);
        this.dataCartaFiancaFormControl.setValidators([Validators.required]);
      } else {
        this.valorCartaFiancaFormControl.disable();
        this.dataCartaFiancaFormControl.disable();
      }
      this.aoSelecionarTipoPessoa();
    } catch (error) {
      console.error(error);
      //await this.dialog.showErr('Não foi possível carregar as informações', (error as HttpErrorResult).messages.join('\n'));
      this.activeModal.dismiss();
    }
  }

  //#region MODAL

  // tslint:disable-next-line: member-ordering
  private static exibeModal(): NgbModalRef {
    const modalService: NgbModal = StaticInjector.Instance.get(NgbModal);
    const modalRef = modalService.open(ManterParteComponent, { centered: true, size: 'lg', backdrop: 'static' });
    return modalRef;
  }

  // tslint:disable-next-line: member-ordering
  public static async exibeModalDeCriar(): Promise<void> {
    const modalRef = this.exibeModal();
    modalRef.componentInstance.titulo = 'Inclusão de Parte';
    await modalRef.result;
  }

  // tslint:disable-next-line: member-ordering
  public static async exibeModalDeAlterar(entidade: Parte): Promise<void> {
    const modalRef = this.exibeModal();
    modalRef.componentInstance.titulo = 'Edição de Parte';
    modalRef.componentInstance.entidade = entidade;
    await modalRef.result;
  }

  //#endregion MODAL

  async confirmar(): Promise<void> {
    this.formulario.markAllAsTouched();
    if (this.formulario.invalid) {
      return;
    }

    try {
      if (this.entidade) {
        await this.atualizar();
      } else {
        await this.criar();
      }
      this.activeModal.dismiss();
    } catch (error) {
      console.log(error);
    }
  }

  private async criar(): Promise<void> {
    try {
      const pessoaJuridica: boolean = this.tipoPessoaFormControl.value === 'J';
      const documento: string = pessoaJuridica ?
        this.cnpjFormControl.value :
        this.cpfFormControl.value;
      let valorCartaFianca: number = null;
      if (this.temPermissaoAlteraCartaFianca) {
        valorCartaFianca = this.valorCartaFiancaFormControl.value ? this.valorCartaFiancaFormControl.value : 0;
      }
      await this.service.criar({
        nome: this.nomeFormControl.value,
        documento: documento.replace(/[^0-9]+/g, ''),
        pessoaJuridica: pessoaJuridica,
        carteiraTrabalho: this.carteiraTrabalhoFormControl.value,
        telefone: this.telefoneFormControl.value ? this.telefoneFormControl.value.toString().replace(/[^0-9]+/g, '') : '',
        celular: this.celularFormControl.value ? this.celularFormControl.value.toString().replace(/[^0-9]+/g, '') : '',
        valorCartaFianca: valorCartaFianca,
        dataCartaFianca: this.dataCartaFiancaFormControl.value,
        endereco: this.enderecoFormControl.value,
        enderecosAdicionais: this.enderecosAdicionaisFormControl.value,
        telefonesAdicionais: this.telefonesAdicionaisFormControl.value,
        cidade: this.cidadeFormControl.value,
        estadoId: this.estadoFormControl.value,
        bairro: this.bairroFormControl.value,
        cep: this.cepFormControl.value ? this.cepFormControl.value.toString().replace(/[^0-9]+/g, '') : '',
        tipoParteId: this.tipoPessoaFormControl.value
      });
      await this.dialog.showAlert('Cadastro realizado com sucesso', 'A Parte foi registrado no sistema.');
    } catch (error) {
      await this.dialog.showErr('Cadastro não realizado', (error as HttpErrorResult).messages.join('\n'));
      throw error;
    }
  }

  private async atualizar(): Promise<void> {
    try {
      const pessoaJuridica: boolean = this.tipoPessoaFormControl.value === 'J';
      const documento: string = pessoaJuridica ?
        this.cnpjFormControl.value :
        this.cpfFormControl.value;
      await this.service.atualizar({
        id: this.entidade.id,
        nome: this.nomeFormControl.value,
        documento: documento.replace(/[^0-9]+/g, ''),
        pessoaJuridica: pessoaJuridica,
        carteiraTrabalho: this.carteiraTrabalhoFormControl.value,
        telefone: this.telefoneFormControl.value ? this.telefoneFormControl.value.toString().replace(/[^0-9]+/g, '') : '',
        celular: this.celularFormControl.value ? this.celularFormControl.value.toString().replace(/[^0-9]+/g, '') : '',
        valorCartaFianca: this.valorCartaFiancaFormControl.value,
        dataCartaFianca: this.dataCartaFiancaFormControl.value,
        endereco: this.enderecoFormControl.value,
        enderecosAdicionais: this.enderecosAdicionaisFormControl.value,
        telefonesAdicionais: this.telefonesAdicionaisFormControl.value,
        cidade: this.cidadeFormControl.value,
        estadoId: this.estadoFormControl.value,
        bairro: this.bairroFormControl.value,
        cep: this.cepFormControl.value ? this.cepFormControl.value.toString().replace(/[^0-9]+/g, '') : '',
        tipoParteId: this.tipoPessoaFormControl.value
      });
      await this.dialog.showAlert('Cadastro atualizado com sucesso', 'A Parte foi atualizada no sistema.');
    } catch (error) {
      await this.dialog.showErr('Operação não realizada', (error as HttpErrorResult).messages.join('\n'));
      throw error;
    }
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

        if (cnpj === '11111111111111' || cnpj === '99999999999999') {
          return null;
        }

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

  aoSelecionarTipoPessoa() {
    const pessoaEhFisica = this.tipoPessoaFormControl.value === 'F';
    if (pessoaEhFisica) {
      this.cnpjFormControl.setValidators([]);
      this.cpfFormControl.setValidators([Validators.required, this.cpfValido()]);
      this.carteiraTrabalhoFormControl.setValidators([Validators.maxLength(8)]);
    }
    if (!pessoaEhFisica) {
      this.cpfFormControl.setValidators([]);
      this.carteiraTrabalhoFormControl.setValidators([]);
      this.cnpjFormControl.setValidators([Validators.required, this.cnpjValido()]);
    }
    this.cpfFormControl.updateValueAndValidity();
    this.carteiraTrabalhoFormControl.updateValueAndValidity();
    this.cnpjFormControl.updateValueAndValidity();
  }

  desabilitaTooltip(formControl: FormControl) {
    return formControl.untouched || formControl.valid;
  }

  desabilitaTooltipParaDatepicker(formControl: FormControl) {
    return formControl.pristine ||
      (formControl.dirty && formControl.invalid && formControl.untouched) ||
      !(formControl.dirty && formControl.invalid);
  }
}

export function dateMaxValid(dataMaxima: Date): ValidatorFn {
  return (control: AbstractControl): { [key: string]: any } | null => {
    const datevalid = false;
    return datevalid ? { dateMaxValid: true } : null;
  };
}

export function dateValid(): ValidatorFn {
  return (control: AbstractControl): { [key: string]: any } | null => {
    const DATE_REGEXP = /^[0-9]{4}-(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1])$/;
    return DATE_REGEXP.test(control.value) || control.value === '' ? { dateValid: true } : null;
  };
}
