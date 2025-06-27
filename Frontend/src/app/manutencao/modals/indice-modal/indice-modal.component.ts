import { IndiceService } from './../../services/indice.service';
import { Indice } from '@manutencao/models/indice';
// angular
import { AfterViewInit, Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

// 3rd party
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';

// core & shared
import { StaticInjector } from '@manutencao/static-injector';
import { DialogService } from '@shared/services/dialog.service';

// local
import { HttpErrorResult } from '@core/http/http-error-result';

@Component({
  selector: 'app-indice-modal',
  templateUrl: './indice-modal.component.html',
  styleUrls: ['./indice-modal.component.scss'],

})
export class IndiceModalComponent implements AfterViewInit {
  indice: Indice;
  titulo: string
  indiceColuna: string = '';
  acumulado: boolean = false;

  constructor(
    private modal: NgbActiveModal,
    private service: IndiceService,
    private dialogService: DialogService
  ) { }

  descricaoFormControl: FormControl = new FormControl('', [
    Validators.required,
    Validators.maxLength(20)
  ]);
  tipoValorFormControl: FormControl = new FormControl('F', [
    Validators.required
  ]);

  mensalFormControl: FormControl = new FormControl(true, [
    Validators.required
  ]);

  acumuladoFormControl: FormControl = new FormControl(false);
  calculoAutmaticoFormControl: FormControl = new FormControl(false);

  formGroup: FormGroup = new FormGroup({
    descricao: this.descricaoFormControl,
    tipoValor: this.tipoValorFormControl,
    mensal: this.mensalFormControl,
    acumulado: this.acumuladoFormControl,
    acumuladoAutomatico: this.calculoAutmaticoFormControl,
  });

  async ngAfterViewInit(): Promise<void> {

    if (this.indice) {
      const utilizadoEmCotacao = await this.service.utilizadoEmCotacao(this.indice.id);

      this.descricaoFormControl.setValue(this.indice.descricao);
      this.tipoValorFormControl.setValue(this.indice.codigoValorIndice)
      this.mensalFormControl.setValue(this.indice.codigoTipoIndice == 'M' ? true : false);
      this.acumuladoFormControl.setValue(this.indice.acumulado);
      this.calculoAutmaticoFormControl.setValue(this.indice.acumuladoAutomatico);

      if (utilizadoEmCotacao)
        this.tipoValorFormControl.disable();

      this.acumulado = this.indice.acumulado;
      this.indiceColuna = this.indice.codigoValorIndice;
    }
  }

  close(): void {
    this.modal.close(false);
  }

  async save(): Promise<void> {
    const operacao = this.indice ? 'Alteração' : 'Inclusão';
    try {
      if (!this.descricaoFormControl.value.replace(/\s/g, '').length) {
        await this.dialogService.err(`${operacao} não realizada`, `O campo descrição não pode contar apenas espaços.`);
        return;
      }

      if (this.indice) {

        await this.service.alterar(
          this.indice.id,
          this.descricaoFormControl.value,
          this.mensalFormControl.value,
          this.tipoValorFormControl.value,
          this.acumuladoFormControl.value,
          this.calculoAutmaticoFormControl.value);
      } else {
        await this.service.incluir(
          this.descricaoFormControl.value,
          this.mensalFormControl.value,
          this.tipoValorFormControl.value,
          this.acumuladoFormControl.value,
          this.calculoAutmaticoFormControl.value);
      }

      await this.dialogService.alert(`${operacao} realizada com sucesso`);
      this.modal.close(true);
    } catch (error) {
      await this.dialogService.err(`${operacao} não realizada`, (error as HttpErrorResult).messages.join('\n'));
    }
  }

  onOptionsSelected(value: string) {
    this.indiceColuna = value;
  }

  eventCheck(event) {
    this.acumulado = event.target.checked;
  }

  //#region MODAL

  // tslint:disable-next-line: member-ordering
  static exibeModalDeIncluir(): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(IndiceModalComponent, { centered: true, size: 'sm', backdrop: 'static' });
    modalRef.componentInstance.titulo = 'Incluir Índice';
    return modalRef.result;
  }

  // tslint:disable-next-line: member-ordering
  static exibeModalDeAlterar(indice: Indice): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(IndiceModalComponent, { centered: true, size: 'sm', backdrop: 'static' });
    modalRef.componentInstance.titulo = 'Alterar Índice';
    modalRef.componentInstance.indice = indice;
    return modalRef.result;
  }

  //#endregion MODAL
}
