import { isNullOrUndefined } from 'util';
import { Component, OnInit, Input } from '@angular/core';
import { HttpErrorResult } from '@core/http/http-error-result';
import { Estabelecimento } from '@manutencao/models/estabelecimento.model';
import { EstabelecimentosService } from '../../../../services/estabelecimentos.service';
import { FormGroup, FormControl, Validators, AbstractControl } from '@angular/forms';
import { NgbActiveModal, NgbModalRef, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DialogService } from '@shared/services/dialog.service';
import { StaticInjector } from '@manutencao/static-injector';
import { Estados } from 'src/app/core/models/estados.model';
import { Estado } from './../../../../../core/models/estado.model';

@Component({
  selector: 'app-manter',
  templateUrl: './manter.component.html'
})
export class ManterEstabelecimentosComponent implements OnInit {

  @Input() titulo;
  @Input() entidade: Estabelecimento;

  estados: Array<Estado> = [];

  cnpjFormControl = new FormControl('', [Validators.required, this.cnpjValido()]);
  nomeFormControl = new FormControl('', [Validators.required, Validators.maxLength(50)]);
  enderecoFormControl = new FormControl('', [Validators.maxLength(60)]);
  bairroFormControl = new FormControl('', [Validators.maxLength(30)]);
  cidadeFormControl = new FormControl('', [Validators.maxLength(30)]);
  estadoFormControl = new FormControl(null);
  cepFormControl = new FormControl('');
  // tslint:disable-next-line: max-line-length
  telefoneFormControl = new FormControl(null, [this.telefoneValido(10)]);
  celularFormControl = new FormControl(null, [this.telefoneValido(11)]);

  formulario = new FormGroup({
    cnpj: this.cnpjFormControl,
    nome: this.nomeFormControl,
    endereco: this.enderecoFormControl,
    bairro: this.bairroFormControl,
    cidade: this.cidadeFormControl,
    estado: this.estadoFormControl,
    cep: this.cepFormControl,
    telefone: this.telefoneFormControl,
    celular: this.celularFormControl
  });

  estado: { id: string, valor: string } = null;

  mascaraTelefone = ['(', /[1-9]/, /\d/, ')', /\d/, /\d/, /\d/, /\d/, '-', /\d/, /\d/, /\d/, /\d/];
  mascaraCelular = ['(', /[1-9]/, /\d/, ')', /\d/, /\d/, /\d/, /\d/, /\d/, '-', /\d/, /\d/, /\d/, /\d/];
  mascaraCep = [/[0-9]/, /[0-9]/, /[0-9]/, /[0-9]/, /[0-9]/, '-', /[0-9]/, /[0-9]/, /[0-9]/];
  mascaraCnpj = [/[0-9]/, /[0-9]/, '.', /[0-9]/, /[0-9]/, /[0-9]/, '.', /[0-9]/, /[0-9]/, /[0-9]/, '/', /[0-9]/, /[0-9]/, /[0-9]/, /[0-9]/, '-', /[0-9]/, /[0-9]/];

  constructor(
    public activeModal: NgbActiveModal,
    private service: EstabelecimentosService,
    private dialog: DialogService) { }

  ngOnInit() {
    this.estados = Estados.obterUfs();

    if (this.entidade) {
      const estadoSelecionado = this.entidade.estado ? this.entidade.estado.id : '';
      const telefone = this.entidade.telefone ? this.entidade.telefoneDDD + this.entidade.telefone : null;
      const celular = this.entidade.celular ? this.entidade.celularDDD + this.entidade.celular : null;

      this.cnpjFormControl.setValue(this.entidade.cnpj);
      this.nomeFormControl.setValue(this.entidade.nome);
      this.enderecoFormControl.setValue(this.entidade.endereco);
      this.bairroFormControl.setValue(this.entidade.bairro);
      this.cidadeFormControl.setValue(this.entidade.cidade);
      this.estadoFormControl.setValue(estadoSelecionado);
      this.cepFormControl.setValue(this.entidade.cep);
      this.telefoneFormControl.setValue(telefone);
      this.celularFormControl.setValue(celular);
    }

  }

  // tslint:disable-next-line: member-ordering
  private static exibeModal(): NgbModalRef {
    const modalService: NgbModal = StaticInjector.Instance.get(NgbModal);
    const modalRef = modalService.open(ManterEstabelecimentosComponent, { centered: true, backdrop: 'static' });
    return modalRef;
  }

  // tslint:disable-next-line: member-ordering
  public static async exibeModalDeCriar(): Promise<void> {
    const modalRef = this.exibeModal();
    modalRef.componentInstance.titulo = 'Inclusão de Estabelecimento';
    await modalRef.result;
  }

  // tslint:disable-next-line: member-ordering
  public static async exibeModalDeAlterar(entidade: Estabelecimento): Promise<void> {
    const modalRef = this.exibeModal();
    modalRef.componentInstance.titulo = 'Edição de Estabelecimento';
    modalRef.componentInstance.entidade = entidade;
    await modalRef.result;
  }

  desabilitaTooltip(formControl: FormControl) {
    return formControl.untouched || formControl.valid;
  }

  async onSubmit() {
    this.formulario.markAllAsTouched();
    if (this.formulario.invalid) return false;

    if (this.entidade) {
      await this.atualizar();
    } else {
      await this.criar();
    }

    this.activeModal.dismiss();
  }

  private criarComandoSalvar(): any {
    return;
  }

  private async criar(): Promise<void> {
    try {
      await this.service.criar({
        cnpj: this.cnpjFormControl.value.replace(/[^0-9]+/g, ''),
        nome: this.nomeFormControl.value,
        endereco: this.enderecoFormControl.value,
        bairro: this.bairroFormControl.value,
        cidade: this.cidadeFormControl.value,
        estado: this.estadoFormControl.value,
        cep: this.cepFormControl.value ? this.cepFormControl.value.toString().replace(/[^0-9]+/g, '') : '',
        telefone: !isNullOrUndefined(this.telefoneFormControl.value) ? this.telefoneFormControl.value.toString().replace(/[^0-9]+/g, '') : '',
        celular: !isNullOrUndefined(this.celularFormControl.value) ? this.celularFormControl.value.toString().replace(/[^0-9]+/g, '') : ''
      });
      this.dialog.showAlert('Cadastro realizado com sucesso', 'O Estabelecimento foi registrado no sistema.');
    } catch (error) {
      this.dialog.showErr('Cadastro não realizado', (error as HttpErrorResult).messages.join('\n'));
      throw error;
    }
  }

  private async atualizar(): Promise<void> {
    try {
      await this.service.atualizar({
        id: this.entidade.id,
        cnpj: this.cnpjFormControl.value.replace(/[^0-9]+/g, ''),
        nome: this.nomeFormControl.value,
        endereco: this.enderecoFormControl.value,
        bairro: this.bairroFormControl.value,
        cidade: this.cidadeFormControl.value,
        estado: this.estadoFormControl.value,
        cep: this.cepFormControl.value ? this.cepFormControl.value.toString().replace(/[^0-9]+/g, '') : '',
        telefone: !isNullOrUndefined(this.telefoneFormControl.value) ? this.telefoneFormControl.value.toString().replace(/[^0-9]+/g, '') : '',
        celular: !isNullOrUndefined(this.celularFormControl.value) ? this.celularFormControl.value.toString().replace(/[^0-9]+/g, '') : ''
      });
      this.dialog.showAlert('Alteração realizada com sucesso', 'O Estabelecimento foi alterado no sistema.').then(() => this.activeModal.close(this.entidade));
    } catch (error) {
      this.dialog.showErr('Alteração não realizada', (error as HttpErrorResult).messages.join('\n'));
      throw error;
    }
  }

  private telefoneValido(tamanhoMinimo: number) {

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

  private cnpjValido() {

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
          cnpj === '11111111111111' ||
          cnpj === '22222222222222' ||
          cnpj === '33333333333333' ||
          cnpj === '44444444444444' ||
          cnpj === '55555555555555' ||
          cnpj === '66666666666666' ||
          cnpj === '77777777777777' ||
          cnpj === '88888888888888' ||
          cnpj === '99999999999999') {
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

}
