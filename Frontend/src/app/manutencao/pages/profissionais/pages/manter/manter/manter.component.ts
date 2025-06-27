import { Component, OnInit, Input, QueryList } from '@angular/core';
import { FormControl, FormGroup, AbstractControl, Validators } from '@angular/forms';
import { NgbModalRef, NgbModal, NgbActiveModal, NgbTab, NgbTabset } from '@ng-bootstrap/ng-bootstrap';

import { HttpErrorResult } from '@core/http/http-error-result';
import { DialogService } from '@shared/services/dialog.service';

import { Profissional } from '@manutencao/models';
import { ProfissionaisService } from '@manutencao/services';

import { Estados } from '@core/models/estados.model';
import { Estado } from '@core/models/estado.model';
import { StaticInjector } from '@manutencao/static-injector';

@Component({
  selector: 'app-manter',
  templateUrl: './manter.component.html',
  styleUrls: ['./manter.component.scss']
})
export class ManterProfissionalComponent implements OnInit {
  titulo: string = 'Inclusão de Profissional';
  entidade: Profissional;

  tipoPessoaFormControl: FormControl = new FormControl('F', [Validators.required]);
  cnpjFormControl: FormControl = new FormControl('');
  cpfFormControl: FormControl = new FormControl('', [Validators.required]);
  nomeFormControl: FormControl = new FormControl('', [Validators.required, Validators.maxLength(400)]);
  emailFormControl: FormControl = new FormControl('', [Validators.maxLength(60)]);
  contadorFormControl: FormControl = new FormControl(false);
  contadorPexFormControl: FormControl = new FormControl(false);
  telefoneFormControl: FormControl = new FormControl(null);
  celularFormControl: FormControl = new FormControl(null);
  faxFormControl: FormControl = new FormControl(null);
  advogadoAutorFormControl: FormControl = new FormControl(false);
  registroOABFormControl: FormControl = new FormControl('', [Validators.maxLength(7)]);
  estadoFormControl: FormControl = new FormControl('');
  estadoOABFormControl: FormControl = new FormControl('');
  enderecoFormControl: FormControl = new FormControl('', [Validators.maxLength(60)]);
  enderecosAdicionaisFormControl: FormControl = new FormControl('', [Validators.maxLength(4000)]);
  telefonesAdicionaisFormControl: FormControl = new FormControl('', [Validators.maxLength(4000)]);
  cidadeFormControl: FormControl = new FormControl('', [Validators.maxLength(30)]);
  bairroFormControl: FormControl = new FormControl('', [Validators.maxLength(30)]);
  cepFormControl: FormControl = new FormControl(null);

  formulario: FormGroup = new FormGroup({
    tipoPessoa: this.tipoPessoaFormControl,
    cnpj: this.cnpjFormControl,
    cpf: this.cpfFormControl,
    nome: this.nomeFormControl,
    email: this.emailFormControl,
    contador: this.contadorFormControl,
    contadorPex: this.contadorPexFormControl,
    telefone: this.telefoneFormControl,
    celular: this.celularFormControl,
    fax: this.faxFormControl,
    advogadoAutor: this.advogadoAutorFormControl,
    registroOAB: this.registroOABFormControl,
    estado: this.estadoFormControl,
    estadoOAB: this.estadoOABFormControl,
    endereco: this.enderecoFormControl,
    enderecosAdicionais: this.enderecosAdicionaisFormControl,
    telefonesAdicionais: this.telefonesAdicionaisFormControl,
    cidade: this.cidadeFormControl,
    bairro: this.bairroFormControl,
    cep: this.cepFormControl
  });

  estados: Array<Estado> = [];

  mascaraCelular = ['(', /[1-9]/, /\d/, ')', /\d/, /\d/, /\d/, /\d/, /\d/, '-', /\d/, /\d/, /\d/, /\d/];
  mascaraTelefone = ['(', /[1-9]/, /\d/, ')', /\d/, /\d/, /\d/, /\d/, '-', /\d/, /\d/, /\d/, /\d/];
  mascaraFax = ['(', /[1-9]/, /\d/, ')', /\d/, /\d/, /\d/, /\d/, '-', /\d/, /\d/, /\d/, /\d/];
  mascaraCep = [/[0-9]/, /[0-9]/, /[0-9]/, /[0-9]/, /[0-9]/, '-', /[0-9]/, /[0-9]/, /[0-9]/];
  mascaraCpf = [/[0-9]/, /[0-9]/, /[0-9]/, '.', /[0-9]/, /[0-9]/, /[0-9]/, '.', /[0-9]/, /[0-9]/, /[0-9]/, '-', /[0-9]/, /[0-9]/];
  // tslint:disable-next-line: max-line-length
  mascaraCnpj = [/[0-9]/, /[0-9]/, '.', /[0-9]/, /[0-9]/, /[0-9]/, '.', /[0-9]/, /[0-9]/, /[0-9]/, '/', /[0-9]/, /[0-9]/, /[0-9]/, /[0-9]/, '-', /[0-9]/, /[0-9]/];

  constructor(
    public activeModal: NgbActiveModal, private service: ProfissionaisService,
    private dialog: DialogService) { }

  async ngOnInit(): Promise<void> {
    try {
      this.estados = Estados.obterUfs();
      if (this.entidade) {

        const telefone = this.entidade.telefone ? this.entidade.telefoneDDD + this.entidade.telefone : null;
        const celular = this.entidade.celular ? this.entidade.celularDDD + this.entidade.celular : null;
        const fax = this.entidade.fax ? this.entidade.faxDDD + this.entidade.fax : null;
        const cep = this.entidade.cep ? this.entidade.cep : null;

        this.tipoPessoaFormControl.setValue(this.entidade.tipoPessoa.valor ? this.entidade.tipoPessoa.valor : 'F');
        this.cnpjFormControl.setValue(this.entidade.cnpj);
        this.cpfFormControl.setValue(this.entidade.cpf);
        this.nomeFormControl.setValue(this.entidade.nome);
        this.emailFormControl.setValue(this.entidade.email);
        this.contadorFormControl.setValue(this.entidade.ehContador);
        this.contadorPexFormControl.setValue(this.entidade.ehContadorPex);
        this.telefoneFormControl.setValue(telefone);
        this.celularFormControl.setValue(celular);
        this.faxFormControl.setValue(fax);
        this.advogadoAutorFormControl.setValue(this.entidade.ehAdvogado);
        this.registroOABFormControl.setValue(this.entidade.registroOAB);
        this.estadoFormControl.setValue(this.entidade.estado ? this.entidade.estado.id : '');
        this.estadoOABFormControl.setValue(this.entidade.estadoOAB ? this.entidade.estadoOAB.id : '');
        this.enderecoFormControl.setValue(this.entidade.endereco);
        this.enderecosAdicionaisFormControl.setValue(this.entidade.enderecosAdicionais);
        this.telefonesAdicionaisFormControl.setValue(this.entidade.telefonesAdicionais);
        this.cidadeFormControl.setValue(this.entidade.cidade);
        this.bairroFormControl.setValue(this.entidade.bairro);
        this.cepFormControl.setValue(cep);
      }

      this.aoSelecionarTipoPessoa();
      this.aoMarcarAdvogadoDoAutor();

    } catch (error) {
      console.error(error);
      // await this.dialog.showErr('Não foi possível carregar as informações', (error as HttpErrorResult).messages.join('\n'));
      this.activeModal.dismiss();
    }
  }

  //#region MODAL

  // tslint:disable-next-line: member-ordering
  private static exibeModal(): NgbModalRef {
    const modalService: NgbModal = StaticInjector.Instance.get(NgbModal);
    const modalRef = modalService.open(ManterProfissionalComponent, { centered: true, size: 'lg', backdrop: 'static' });
    return modalRef;
  }

  // tslint:disable-next-line: member-ordering
  public static async exibeModalDeCriar(): Promise<void> {
    const modalRef = this.exibeModal();
    modalRef.componentInstance.titulo = 'Inclusão de Profissional';
    await modalRef.result;
  }

  // tslint:disable-next-line: member-ordering
  public static async exibeModalDeAlterar(entidade: Profissional): Promise<void> {
    const modalRef = this.exibeModal();
    modalRef.componentInstance.titulo = 'Edição de Profissional';
    modalRef.componentInstance.entidade = entidade;
    await modalRef.result;
  }

  //#endregion MODAL

  async confirmar(tabset: NgbTabset): Promise<void> {
    this.formulario.markAllAsTouched();

    if (this.formulario.invalid) {
      tabset.select(tabset.tabs.first.id);
      return;
    }

    try {
      const homonimoValido = await this.validarHomonimo();
      if (!homonimoValido) {
        return;
      }

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

  private async validarHomonimo(): Promise<boolean> {

    const pessoaFisica: boolean = this.tipoPessoaFormControl.value === 'F';

    if (!pessoaFisica) {
      return true;
    }

    const existeHomonimo: boolean = await this.service.existe({nome: this.nomeFormControl.value, id: this.entidade ? this.entidade.id : null});

    if (!existeHomonimo) {
      return true;
    }

    // tslint:disable-next-line: max-line-length
    const confirmarHomonimo: boolean = await this.dialog.showConfirm('Confirmar Homônimo', 'Profissional com o mesmo nome encontrado, confirmar operação?');

    if (confirmarHomonimo) {
      return true;
    }

    return false;
  }

  private async criar(): Promise<any> {
    try {
      const pessoaJuridica: boolean = this.tipoPessoaFormControl.value === 'J';
      const documento: string = pessoaJuridica ?
        this.cnpjFormControl.value :
        this.cpfFormControl.value;
      const result = await this.service.criar({
        nome: this.nomeFormControl.value,
        pessoaJuridica: pessoaJuridica,
        documento: documento.replace(/[^0-9]+/g, ''),
        email: this.emailFormControl.value,
        estado: this.estadoFormControl.value,
        endereco: this.enderecoFormControl.value,
        cep: this.cepFormControl.value ? this.cepFormControl.value.toString().replace(/[^0-9]+/g, '') : null,
        cidade: this.cidadeFormControl.value,
        bairro: this.bairroFormControl.value,
        enderecosAdicionais: this.enderecosAdicionaisFormControl.value,
        telefone: this.telefoneFormControl.value ? this.telefoneFormControl.value.toString().replace(/[^0-9]+/g, '') : '',
        fax: this.faxFormControl.value ? this.faxFormControl.value.toString().replace(/[^0-9]+/g, '') : '',
        celular: this.celularFormControl.value ? this.celularFormControl.value.toString().replace(/[^0-9]+/g, '') : '',
        telefonesAdicionais: this.telefonesAdicionaisFormControl.value,
        advogado: this.advogadoAutorFormControl.value,
        numeroOab: this.registroOABFormControl.value,
        estadoOab: this.estadoOABFormControl.value,
        contador: this.contadorFormControl.value,
        contadorPex: this.contadorPexFormControl.value
      });

      console.log(result);

      await this.dialog.showAlert('Cadastro realizado com sucesso', 'O Profissional foi registrado no sistema.');
    } catch (error) {
      await this.dialog.showErr('Cadastro não realizado', (error as HttpErrorResult).messages.join('\n'));
      throw error;
    }
  }

  private async atualizar(): Promise<any> {
    try {
      const pessoaJuridica: boolean = this.tipoPessoaFormControl.value === 'J';
      const documento: string = pessoaJuridica ?
        this.cnpjFormControl.value :
        this.cpfFormControl.value;
      await this.service.atualizar({
        id: this.entidade.id,
        nome: this.nomeFormControl.value,
        pessoaJuridica: pessoaJuridica,
        documento: documento.replace(/[^0-9]+/g, ''),
        email: this.emailFormControl.value,
        estado: this.estadoFormControl.value,
        endereco: this.enderecoFormControl.value,
        cep: this.cepFormControl.value ? this.cepFormControl.value.toString().replace(/[^0-9]+/g, '') : 0,
        cidade: this.cidadeFormControl.value,
        bairro: this.bairroFormControl.value,
        enderecosAdicionais: this.enderecosAdicionaisFormControl.value,
        telefone: this.telefoneFormControl.value ? this.telefoneFormControl.value.toString().replace(/[^0-9]+/g, '') : '',
        fax: this.faxFormControl.value ? this.faxFormControl.value.toString().replace(/[^0-9]+/g, '') : '',
        celular: this.celularFormControl.value ? this.celularFormControl.value.toString().replace(/[^0-9]+/g, '') : '',
        telefonesAdicionais: this.telefonesAdicionaisFormControl.value,
        advogado: this.advogadoAutorFormControl.value,
        numeroOab: this.registroOABFormControl.value,
        estadoOab: this.estadoOABFormControl.value,
        contador: this.contadorFormControl.value,
        contadorPex: this.contadorPexFormControl.value
      });
      await this.dialog.showAlert('Cadastro atualizado com sucesso', 'O Profissional foi atualizado no sistema.');
    } catch (error) {
      await this.dialog.showErr('Operação não realizada', (error as HttpErrorResult).messages.join('\n'));
      throw error;
    }
  }

  desabilitaTooltip(formControl: FormControl) {
    return formControl.untouched || formControl.valid;
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

  aoSelecionarTipoPessoa(): void {
    const pessoaEhFisica = this.tipoPessoaFormControl.value === 'F';
    if (pessoaEhFisica) {
      this.cnpjFormControl.setValidators([]);
      this.cpfFormControl.setValidators([Validators.required, this.cpfValido()]);
    }
    if (!pessoaEhFisica) {
      this.cpfFormControl.setValidators([]);
      this.cnpjFormControl.setValidators([Validators.required, this.cnpjValido()]);
    }
    this.cpfFormControl.updateValueAndValidity();
    this.cnpjFormControl.updateValueAndValidity();
  }

  aoMarcarAdvogadoDoAutor() {
    if (this.advogadoAutorFormControl.value === true) {
      this.registroOABFormControl.setValidators([Validators.required]);
      this.estadoOABFormControl.setValidators([Validators.required]);
    } else {
      this.registroOABFormControl.setValidators([]);
      this.estadoOABFormControl.setValidators([]);
    }
    this.registroOABFormControl.updateValueAndValidity();
    this.estadoOABFormControl.updateValueAndValidity();
  }
}
