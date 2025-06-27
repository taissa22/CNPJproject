// angular
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

// 3rd party
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';

// core & shared
import { StaticInjector } from '@manutencao/static-injector';
import { DialogService } from '@shared/services/dialog.service';

// local
import { TipoDeParticipacao } from '@manutencao/models/tipo-de-participacao';
import { TipoDeParticipacaoService } from '@manutencao/services/tipo-de-participacao.service';
import { TipoDeParticipacaoServiceMock } from '@manutencao/services/tipo-de-participacao.service.mock';
import { HttpErrorResult } from '@core/http/http-error-result';

@Component({
  selector: 'app-tipo-de-participacao-modal',
  templateUrl: './tipo-de-participacao-modal.component.html',
  styleUrls: ['./tipo-de-participacao-modal.component.scss'],
})
export class TipoDeParticipacaoModalComponent implements OnInit {
  tipoDeParticipacao: TipoDeParticipacao;
  titulo: string;


  constructor(
    private modal: NgbActiveModal,
    private service: TipoDeParticipacaoService,
    private dialogService: DialogService
  ) {}

  descricaoFormControl: FormControl = new FormControl('', [
    Validators.required,
    Validators.maxLength(20)
  ]);

  formGroup: FormGroup = new FormGroup({
    descricao: this.descricaoFormControl
  });

  ngOnInit(): void {
    if (this.tipoDeParticipacao) {
      this.descricaoFormControl.setValue(this.tipoDeParticipacao.descricao);
    }
  }

  close(): void {
    this.modal.close(false);
  }

  async save(): Promise<void> {
    let operacao = this.tipoDeParticipacao ? 'Alteração' : 'Inclusão';

    if (!this.descricaoFormControl.value.replace(/\s/g, '').length) {
      await this.dialogService.err(`${operacao} não realizada`, `O campo descrição não pode contar apenas espaços.`);
      return;
    }
    
    try {
      if (this.tipoDeParticipacao) {
        await this.service.alterar(
          this.tipoDeParticipacao.codigo,
          this.descricaoFormControl.value
        );
      } else {
        await this.service.incluir(this.descricaoFormControl.value);
      }    

      await this.dialogService.alert(`${operacao} realizada com sucesso`);
      this.modal.close(true);
    } catch (error) {
      console.log(error);
      if ((error as HttpErrorResult).messages.join().includes('essa descrição')){
       await this.dialogService.info(`Desculpe, não é possível fazer a ${operacao}`, (error as HttpErrorResult).messages.join('\n'));
       return;
      }
      await this.dialogService.err(`${operacao} não realizada`, (error as HttpErrorResult).messages.join('\n'));
    }
  }

  // tslint:disable-next-line: member-ordering
  static exibeModalDeIncluir(): Promise<boolean> {
    // prettier-ignore
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(TipoDeParticipacaoModalComponent, { centered: true, size: 'sm', backdrop: 'static' });
    modalRef.componentInstance.titulo = "Incluir Tipo de Participação";
    return modalRef.result;
  }

  // tslint:disable-next-line: member-ordering
  static exibeModalDeAlterar(
    tipoDeParticipacao: TipoDeParticipacao
  ): Promise<boolean> {
    // prettier-ignore
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(TipoDeParticipacaoModalComponent, { centered: true, size: 'sm', backdrop: 'static' });
    modalRef.componentInstance.titulo = "Alterar Tipo de Participação";
    modalRef.componentInstance.tipoDeParticipacao = tipoDeParticipacao;
    return modalRef.result;
  }
}
