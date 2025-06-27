import { AfterViewInit, Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { StaticInjector } from '@manutencao/static-injector';
import { DialogService } from '@shared/services/dialog.service';
import { HttpErrorResult } from '@core/http/http-error-result';
import { MotivoProvavelZero } from '@manutencao/models/motivo-provavel-zero';
import { MotivoProvavelZeroService } from '@manutencao/services/motivo-provavel-zero.service';

@Component({
  selector: 'app-motivo-provavel-zero-modal',
  templateUrl: './motivo-provavel-zero-modal.component.html',
  styleUrls: ['./motivo-provavel-zero-modal.component.scss'],

})
export class MotivoProvavelZeroModalComponent implements AfterViewInit {
  motivo: MotivoProvavelZero; 
  titulo = ""; 

  constructor(
    private modal: NgbActiveModal,
    private service: MotivoProvavelZeroService,
    private dialogService: DialogService
  ) { }

  descricaoFormControl: FormControl = new FormControl('', [ Validators.required, Validators.maxLength(100)  ]);
 
  formGroup: FormGroup = new FormGroup({
    descricao: this.descricaoFormControl
  });

  async ngAfterViewInit(): Promise<void> {

    if (this.motivo) {      
      this.descricaoFormControl.setValue(this.motivo.descricao);
    }
  }

  close(): void {
    this.modal.close(false);
  }

  async save(): Promise<void> {
    const operacao = this.motivo ? 'Alteração' : 'Inclusão';
    try {
      if (!this.descricaoFormControl.value.replace(/\s/g, '').length) {
        await this.dialogService.err(`${operacao} não realizada`, `O campo descrição não pode contar apenas espaços.`);
        return;
      }

      if (this.motivo) {
        
        await this.service.atualizar(this.motivo.id, this.descricaoFormControl.value);
        await this.dialogService.alert(`${operacao} realizada com sucesso`); 
      } else {
        await this.service.incluir(this.descricaoFormControl.value);          
        await this.dialogService.alert(`${operacao} realizada com sucesso`);
      }
      
      this.modal.close(true);
    } catch (error) {
      await this.dialogService.err(`${operacao} não realizada`, (error as HttpErrorResult).messages.join('\n'));
    }
  }

  //#region MODAL

  // tslint:disable-next-line: member-ordering
  static exibeModalDeIncluir(): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(MotivoProvavelZeroModalComponent, {windowClass: 'motivo-modal', centered: true, size: 'sm', backdrop: 'static' });
    modalRef.componentInstance.titulo = 'Inclusão do Motivo Provável Zero';
    return modalRef.result;
  }

  // tslint:disable-next-line: member-ordering
  static exibeModalDeAlterar(motivo: MotivoProvavelZero): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(MotivoProvavelZeroModalComponent, {windowClass: 'motivo-modal', centered: true, size: 'sm', backdrop: 'static' });
    modalRef.componentInstance.titulo = 'Alteração do Motivo Provável Zero';
    modalRef.componentInstance.motivo = motivo;
    return modalRef.result;
  }

  //#endregion MODAL
}
