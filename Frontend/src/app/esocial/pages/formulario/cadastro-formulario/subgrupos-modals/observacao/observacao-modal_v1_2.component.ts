import { AfterViewInit, Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ErrorLib } from '@esocial/libs/error-lib';
import { Observacao } from '@esocial/models/subgrupos/v1_2/observacao';
import { ObservacaoService } from '@esocial/services/formulario/subgrupos/observacao.service';
import { StaticInjector } from '@esocial/static-injector';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DialogService } from '@shared/services/dialog.service';

@Component({
  selector: 'app-observacao-modal-v1-2',
  templateUrl: './observacao-modal_v1_2.component.html',
  styleUrls: ['./observacao-modal_v1_2.component.scss']
})
export class ObservacaoModal_v1_2_Component implements AfterViewInit {
  observacao: Observacao = null;
  idF2500 : number;
  totalCaracterObs: number = 0;
  contratoId : number;
  temPermissaoEsocialBlocoABCDEFHI: boolean = false;

  idFormularioFormControl: FormControl = new FormControl(null);
  logDataOperacaoFormControl: FormControl = new FormControl(null);
  logCodUsuarioFormControl: FormControl = new FormControl(null);
  observacaoFormControl: FormControl = new FormControl(null, [Validators.required]);
  idFormControl: FormControl = new FormControl(null);

  formGroup: FormGroup = new FormGroup({
    idF2500: this.idFormularioFormControl,
    logDataOperacao: this.logDataOperacaoFormControl,
    logCodUsuario: this.logCodUsuarioFormControl,
    observacoesObservacao: this.observacaoFormControl,
    idEsF2500Observacoes: this.idFormControl
  });

  constructor(
    private service: ObservacaoService,
    private dialogService: DialogService,
    private modal: NgbActiveModal
  ) {}

  ngAfterViewInit() {
    this.iniciarForm();
  }

  iniciarForm() {
    if (this.observacao) {
      this.idFormularioFormControl.setValue(this.observacao.idF2500);
      this.logDataOperacaoFormControl.setValue(this.observacao.logDataOperacao);
      this.logCodUsuarioFormControl.setValue(this.observacao.logCodUsuario);
      this.observacaoFormControl.setValue(this.observacao.observacoesObservacao);
      this.idFormControl.setValue(this.observacao.idEsF2500Observacoes);

      if (!this.temPermissaoEsocialBlocoABCDEFHI) {
        this.idFormularioFormControl.disable();
        this.logDataOperacaoFormControl.disable();
        this.logCodUsuarioFormControl.disable();
        this.observacaoFormControl.disable();
        this.idFormControl.disable();
      }
    }
  }


  async salvar() {
    let operacao = this.observacao ? 'Alteração' : 'Inclusão';
    try {
      let obj = this.formGroup.value;
      obj.idF2500 = this.idF2500;

      if (this.observacao) {
        await this.service.atualizar(obj, this.contratoId);
      } else {
        await this.service.incluir(obj, this.contratoId);
      }

      await this.dialogService.alert(`${operacao} realizada com sucesso`);
      this.modal.close(true);
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      await this.dialogService.err(
        `Desculpe, não foi possível realizar a ${operacao}`,
        mensagem
      );
    }
  }



  static exibeModalAlterar(observacao: Observacao, contratoId : number, formularioId : number, temPermissaoEsocialBlocoABCDEFHI: boolean): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      ObservacaoModal_v1_2_Component,
      {
        windowClass: 'modal-observacao',
        centered: true,
        size: 'lg',
        backdrop: 'static'
      }
    );
    modalRef.componentInstance.observacao = observacao;
    modalRef.componentInstance.contratoId = contratoId;
    modalRef.componentInstance.idF2500 = formularioId;
    modalRef.componentInstance.temPermissaoEsocialBlocoABCDEFHI = temPermissaoEsocialBlocoABCDEFHI;

    return modalRef.result;
  }

  static exibeModalIncluir(f2500: number, contratoId : number): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      ObservacaoModal_v1_2_Component,
      {
        windowClass: 'modal-observacao',
        centered: true,
        size: 'lg',
        backdrop: 'static'
      }
    );
    modalRef.componentInstance.idF2500 = f2500;
    modalRef.componentInstance.contratoId = contratoId;
    modalRef.componentInstance.temPermissaoEsocialBlocoABCDEFHI = true;
    return modalRef.result;
  }

  calculaTamanho(item) {
      this.totalCaracterObs = item.length;
  }

  close(): void {
    this.modal.close(false);
  }

  desabilitaTooltip(control: FormControl):boolean{
    return control.valid || control.untouched || control.disabled
  }
}
