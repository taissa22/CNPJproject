import { Component, OnInit, Input } from '@angular/core';
import { HttpErrorResult } from '@core/http/http-error-result';
import { FielDepositario } from '@manutencao/models';
import { FieisDepositariosService } from '@manutencao/services';
import { FormGroup, FormControl, Validators, AbstractControl } from '@angular/forms';
import { NgbActiveModal, NgbModalRef, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DialogService } from '@shared/services/dialog.service';
import { StaticInjector } from '@manutencao/static-injector';

@Component({
  selector: 'app-manter',
  templateUrl: './manter.component.html'
})
export class ManterFielDepositarioComponent implements OnInit {

  @Input() titulo;
  @Input() entidade: FielDepositario;

  mascaraCpf = [/[0-9]/, /\d/, /\d/, '.', /\d/, /\d/, /\d/, '.', /\d/, /\d/, /\d/, '-', /\d/, /\d/];

  nomeFormControl = new FormControl('', [Validators.required, Validators.maxLength(100)]);
  cpfFormControl = new FormControl('', [Validators.required, Validators.maxLength(14), this.cpfValido()]);

  formulario = new FormGroup({
    nome: this.nomeFormControl,
    cpf: this.cpfFormControl
  });

  constructor(
    public activeModal: NgbActiveModal,
    private service: FieisDepositariosService,
    private dialog: DialogService) { }

  ngOnInit() {
    if (this.entidade) {
      this.nomeFormControl.setValue(this.entidade.nome);
      this.cpfFormControl.setValue(this.entidade.cpf);
    }
  }

  // tslint:disable-next-line: member-ordering
  private static async exibeModal(): Promise<NgbModalRef> {
    const modalService: NgbModal = StaticInjector.Instance.get(NgbModal);
    const modalRef = modalService.open(ManterFielDepositarioComponent, { centered: true, backdrop: 'static' });
    return await modalRef;
  }

  // tslint:disable-next-line: member-ordering
  public static async exibeModalDeCriar(): Promise<void> {
    const modalRef = await this.exibeModal();
    modalRef.componentInstance.titulo = 'Inclusão de Fiel Despositário';
    await modalRef.result;
  }

  // tslint:disable-next-line: member-ordering
  public static async exibeModalDeAlterar(entidade: FielDepositario): Promise<void> {
    const modalRef = await this.exibeModal();
    modalRef.componentInstance.titulo = 'Edição de Fiel Despositário';
    modalRef.componentInstance.entidade = entidade;
    await modalRef.result;
  }

  desabilitaTooltip(formControl: FormControl) {
    return formControl.untouched || formControl.valid;
  }

  async onSubmit() {
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
      console.error(error);
    }
  }

  private async criar(): Promise<void> {
    try {
      await this.service.criar({
        nome: this.nomeFormControl.value,
        cpf: this.cpfFormControl.value.replace(/[^0-9]+/g, '')
      });
      this.dialog.showAlert('Cadastro realizado com sucesso', 'O Fiel Depositário foi registrado no sistema.').then(() => this.activeModal.close(this.entidade));
    } catch (error) {
      this.dialog.showErr('Cadastro não realizado', (error as HttpErrorResult).messages.join('\n'));
      throw error;
    }
  }

  private async atualizar(): Promise<void> {
    try {
      await this.service.atualizar({
        id: this.entidade.id,
        nome: this.nomeFormControl.value,
        cpf: this.cpfFormControl.value.replace(/[^0-9]+/g, '')
      });
      this.dialog.showAlert('Alteração realizada com sucesso', 'O Fiel Depositário foi alterado no sistema.').then(() => this.activeModal.close(this.entidade));
    } catch (error) {
      this.dialog.showErr('Alteração não realizada', (error as HttpErrorResult).messages.join('\n'));
      throw error;
    }
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
}
