import { StaticInjector } from './../../../static-injector';
import { Component, OnInit, ViewChildren, QueryList } from '@angular/core';
import { FormControl, Validators, FormGroup, FormArray, AbstractControl } from '@angular/forms';
import { NgbModal, NgbModalRef, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectComponent } from '@ng-select/ng-select';

import { HttpErrorResult } from '@core/http';
import { DialogService } from '@shared/services/dialog.service';

import { EmpresaCentralizadora, Convenio } from '@manutencao/models';
import { EmpresaCentralizadoraService } from '@manutencao/services';
// import { InjectorInstance } from '@manutencao/manutencao.module';
import { Estado } from '@core/models/estado.model';
import { Estados } from '@core/models/estados.model';

@Component({
  selector: 'app-empresa-centralizadora-modal',
  templateUrl: './empresa-centralizadora-modal.component.html',
  styleUrls: ['./empresa-centralizadora-modal.component.scss']
})
export class EmpresaCentralizadoraModalComponent implements OnInit {
  titulo: string = 'Incluir Empresa Centralizadora';
  empresaCentralizadora: EmpresaCentralizadora;

  estados: Array<Estado> = Estados.obterUfs();

  // tslint:disable-next-line: max-line-length
  mascaraCnpj = [/[0-9]/, /[0-9]/, '.', /[0-9]/, /[0-9]/, /[0-9]/, '.', /[0-9]/, /[0-9]/, /[0-9]/, '/', /[0-9]/, /[0-9]/, /[0-9]/, /[0-9]/, '-', /[0-9]/, /[0-9]/];
  soNumeros = [/[0-9]/, /[0-9]/, /[0-9]/, /[0-9]/, /[0-9]/, /[0-9]/, /[0-9]/, /[0-9]/, /[0-9]/, /[0-9]/, /[0-9]/, /[0-9]/, /[0-9]/];

  nomeFormControl: FormControl = new FormControl('', [Validators.required, Validators.maxLength(100)]);
  conveniosFromArray: FormArray = new FormArray([]);

  formulario: FormGroup = new FormGroup({
    nome: this.nomeFormControl,
    convenios: this.conveniosFromArray
  });

  @ViewChildren(NgSelectComponent) viewChildren: QueryList<NgSelectComponent>;

  private convenioFormulario(convenio?: Convenio): FormGroup {

    const convenioFormGroup = new FormGroup({
      estado: new FormControl(null, [Validators.required, this.estadoUnico()]),
      codigo: new FormControl(null, [Validators.maxLength(4), Validators.required]),
      cnpj: new FormControl(null, [Validators.required, this.cnpjValido()]),
      bancoDebito: new FormControl(null, [Validators.maxLength(9), Validators.required]),
      agenciaDebito: new FormControl(null, [Validators.maxLength(9), Validators.required]),
      digitoAgenciaDebito: new FormControl(null, [Validators.maxLength(1), Validators.required]),
      contaDebito: new FormControl(null, [Validators.maxLength(11), Validators.required]),
      mci: new FormControl(null, [Validators.maxLength(9), Validators.required]),
      agenciaDepositaria: new FormControl(null, [Validators.maxLength(4), Validators.required]),
      digitoAgenciaDepositaria: new FormControl(null, [Validators.maxLength(1), Validators.required])
    });

    if (convenio) {
      convenioFormGroup.get('estado').setValue(convenio.estadoId);
      convenioFormGroup.get('codigo').setValue(convenio.codigo);
      convenioFormGroup.get('cnpj').setValue(convenio.cnpj);
      convenioFormGroup.get('bancoDebito').setValue(convenio.bancoDebito);
      convenioFormGroup.get('agenciaDebito').setValue(convenio.agenciaDebito);
      convenioFormGroup.get('digitoAgenciaDebito').setValue(convenio.digitoAgenciaDebito);
      convenioFormGroup.get('contaDebito').setValue(convenio.contaDebito);
      convenioFormGroup.get('mci').setValue(convenio.mci);
      convenioFormGroup.get('agenciaDepositaria').setValue(convenio.agenciaDepositaria);
      convenioFormGroup.get('digitoAgenciaDepositaria').setValue(convenio.digitoAgenciaDepositaria);
    }
    return convenioFormGroup;
  }

  constructor(
    private activeModal: NgbActiveModal, private dialog: DialogService,
    private empresaCentralizadoraService: EmpresaCentralizadoraService) { }

  async ngOnInit(): Promise<void> {
    if (this.empresaCentralizadora) {
      this.nomeFormControl.setValue(this.empresaCentralizadora.nome);
      this.empresaCentralizadora.convenios
        .forEach(c => this.conveniosFromArray.push(this.convenioFormulario(c)));
    }
  }

  closeDropdowns(): void {
    this.viewChildren.forEach(c => c.close());
  }

  criarConvenio(): void {
    this.conveniosFromArray.push(this.convenioFormulario());
  }

  removerConvenio(index: number): void {
    this.conveniosFromArray.removeAt(index);
  }

  cancelar(): void {
    this.activeModal.dismiss();
  }

  async confirmar(): Promise<void> {
    this.formulario.markAllAsTouched();
    if (this.formulario.invalid) {
      return;
    }

    try {
      if (this.empresaCentralizadora) {
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
      await this.empresaCentralizadoraService.criar({
        nome: this.nomeFormControl.value,
        convenios: this.conveniosFromFormArray()
      });
      await this.dialog.showAlert('Cadastro realizado com sucesso', 'A Empresa do Grupo foi registrada no sistema.');
    } catch (error) {
      await this.dialog.showErr('Operação não realizada', (error as HttpErrorResult).messages.join('\n'));
      throw error;
    }
  }

  private async atualizar(): Promise<void> {
    try {
      await this.empresaCentralizadoraService.atualizar({
        codigo: this.empresaCentralizadora.codigo,
        nome: this.nomeFormControl.value,
        convenios: this.conveniosFromFormArray()
      });
      await this.dialog.showAlert('Cadastro atualizado com sucesso', 'A Empresa do Grupo foi atualizada no sistema.');
    } catch (error) {
      await this.dialog.showErr('Operação não realizada', (error as HttpErrorResult).messages.join('\n'));
      throw error;
    }
  }

  private conveniosFromFormArray(): Array<{
    estadoId: string,
    codigo: number,
    cnpj: string,
    bancoDebito: number,
    agenciaDebito: number,
    digitoAgenciaDebito: string,
    contaDebito: string,
    mci: number,
    agenciaDepositaria: number,
    digitoAgenciaDepositaria: string
  }> {
    return this.conveniosFromArray.controls.map((c: FormGroup) => {
      return {
        estadoId: c.controls['estado'].value,
        codigo: c.controls['codigo'].value,
        cnpj: c.controls['cnpj'].value.toString().replace(/[^0-9]+/g, ''),
        bancoDebito: c.controls['bancoDebito'].value,
        agenciaDebito: c.controls['agenciaDebito'].value,
        digitoAgenciaDebito: c.controls['digitoAgenciaDebito'].value,
        contaDebito: c.controls['contaDebito'].value,
        mci: c.controls['mci'].value,
        agenciaDepositaria: c.controls['agenciaDepositaria'].value,
        digitoAgenciaDepositaria: c.controls['digitoAgenciaDepositaria'].value,
      };
    });
  }

  desabilitaTooltip(formControl: FormControl): boolean {
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

  estadoUnico() {
    return (control: AbstractControl): Validators => {
      const estadoControls = this.conveniosFromArray.controls
        .map((c: FormGroup) => c.controls['estado'])
        .filter(c => c.value === control.value);

      if (estadoControls.length > 1) {
        return { estadoNaoUnico: true };
      }

      return null;
    };
  }

  validaEstadoUnico(): void {
    const estadoControls = this.conveniosFromArray.controls
      .map((c: FormGroup) => c.controls['estado']);

    estadoControls.forEach(c => {
      c.markAllAsTouched();
      c.updateValueAndValidity();
    });
  }

  //#region MODAL

  // tslint:disable-next-line: member-ordering
  private static exibeModal(): NgbModalRef {
    const modalService: NgbModal = StaticInjector.Instance.get(NgbModal);
    const modalRef = modalService.open(EmpresaCentralizadoraModalComponent, { centered: true, size: 'lg', backdrop: 'static' });
    return modalRef;
  }
  // tslint:disable-next-line: member-ordering
  public static async exibeModalDeCriar(): Promise<void> {
    const modalRef = this.exibeModal();
    modalRef.componentInstance.titulo = 'Inclusão de Empresa Centralizadora';
    await modalRef.result;
  }
  // tslint:disable-next-line: member-ordering
  public static async exibeModalDeAlterar(empresaCentralizadora: EmpresaCentralizadora): Promise<void> {
    const modalRef = this.exibeModal();
    modalRef.componentInstance.titulo = 'Alterar Empresa Centralizadora';
    modalRef.componentInstance.empresaCentralizadora = empresaCentralizadora;
    await modalRef.result;
  }

  //#endregion MODAL
}
