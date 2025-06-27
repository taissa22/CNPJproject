// angular
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

// 3rd party
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';

// core & shared
import { StaticInjector } from '@manutencao/static-injector';
import { DialogService } from '@shared/services/dialog.service';

// local
import { BaseDeCalculo } from '@manutencao/models/base-de-calculo';
import { BaseDeCalculoService } from '@manutencao/services/base-de-calculo.service';
import { BaseDeCalculoServiceMock } from '@manutencao/services/base-de-calculo.service.mock';
import { HttpErrorResult } from '@core/http/http-error-result';

@Component({
  selector: 'app-base-de-calculo-modal',
  templateUrl: './base-de-calculo-modal.component.html',
  styleUrls: ['./base-de-calculo-modal.component.scss'],

})
export class BaseDeCalculoModalComponent implements OnInit {
  baseDeCalculo: BaseDeCalculo;
  titulo: string;
  baseInicial: boolean = false;

  constructor(
    private modal: NgbActiveModal,
    private service: BaseDeCalculoService,
    private dialogService: DialogService
  ) {}

  descricaoFormControl: FormControl = new FormControl('', [
    Validators.required,
    Validators.maxLength(50)
  ]);
  ehCalculoInicialFormControl: FormControl = new FormControl(false, [
    Validators.required
  ]);

  formGroup: FormGroup = new FormGroup({
    descricao: this.descricaoFormControl,
    ehCalculoInicial: this.ehCalculoInicialFormControl
  });

  ngOnInit(): void {
    if (this.baseDeCalculo) {
      this.baseInicial = this.baseDeCalculo.ehCalculoInicial;
      this.descricaoFormControl.setValue(this.baseDeCalculo.descricao);
      this.ehCalculoInicialFormControl.setValue(this.baseDeCalculo.ehCalculoInicial);
    }
  }

  close(): void {
    this.modal.close(false);
  }

  async save(): Promise<void> {
    const operacao = this.baseDeCalculo ? 'Alteração' : 'Inclusão';

    try {
      if (!this.descricaoFormControl.value.replace(/\s/g, '').length) {
        await this.dialogService.err(`${operacao} não realizada`, `O campo descrição não pode contar apenas espaços.`);
        return;
      }

      if (this.baseDeCalculo) {
        await  this.service.alterar(
          this.baseDeCalculo.codigo,
          this.descricaoFormControl.value,
          this.ehCalculoInicialFormControl.value
        );
      } else {
        await  this.service.incluir(this.descricaoFormControl.value);
      }
      await this.dialogService.alert(`${operacao} realizada com sucesso`);
      this.modal.close(true);
    } catch (error) {
      await this.dialogService.err(`${operacao} não realizada`, (error as HttpErrorResult).messages.join('\n'));
    }
  }

  //#region MODAL

  // tslint:disable-next-line: member-ordering
  static exibeModalDeIncluir(): Promise<boolean> {
    // prettier-ignore
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(BaseDeCalculoModalComponent, { centered: true, size: 'sm', backdrop: 'static' });
    modalRef.componentInstance.titulo = 'Incluir Base de Cálculo';
    return modalRef.result;
  }

  // tslint:disable-next-line: member-ordering
  static exibeModalDeAlterar(baseDeCalculo: BaseDeCalculo): Promise<boolean> {
    // prettier-ignore
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(BaseDeCalculoModalComponent, { centered: true, size: 'sm', backdrop: 'static' });
    modalRef.componentInstance.baseDeCalculo = baseDeCalculo;
    modalRef.componentInstance.titulo = 'Alterar Base de Cálculo';
    return modalRef.result;
  }

  desmarcouBaseInicial(): boolean {
    if (this.baseInicial) {
      return this.ehCalculoInicialFormControl.value === false;
    }
    return false;
  }

  marcouBaseInicial(): boolean {
    if (!this.baseInicial) {
      return this.ehCalculoInicialFormControl.value === true;
    }
    return false;
  }

  //#endregion MODAL
}
