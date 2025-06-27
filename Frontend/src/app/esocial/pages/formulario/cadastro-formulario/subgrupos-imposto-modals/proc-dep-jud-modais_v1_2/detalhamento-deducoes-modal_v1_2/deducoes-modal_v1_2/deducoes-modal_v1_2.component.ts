import { AfterViewInit, Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ErrorLib } from '@esocial/libs/error-lib';
import { PensaoDepJud } from '@esocial/models/subgrupos/v1_2/pensaoDepJud';
import { ESocialListaFormularioService } from '@esocial/services/aplicacao/e-social-lista-formulario.service';
import { DeducoesDepJudModalService } from '@esocial/services/formulario/subgrupos/imposto-subgrupos-services/deducoes-dep-jud-modal.service';
import { StaticInjector } from '@esocial/static-injector';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DialogService } from '@shared/services/dialog.service';

@Component({
  selector: 'app-deducoes-modal_v1_2',
  templateUrl: './deducoes-modal_v1_2.component.html',
  styleUrls: ['./deducoes-modal_v1_2.component.scss']
})
export class DeducoesModal_v1_2Component implements AfterViewInit {

  constructor(
    private modal: NgbActiveModal,
    private service: DeducoesDepJudModalService,
    private serviceList: ESocialListaFormularioService,
    private dialog: DialogService,
  ) { }

  ngAfterViewInit(): void {
    this.obterListaIndApuracaoValores();
  }

  temPermissaoBlocoCeDeE: boolean = false;
  titulo: string;
  codF2501: number;
  codInfoValores: number;
  codigoDedSusp: number;
  pensaoDepJud: PensaoDepJud;
  codigoReceitaList = [];
  listaTipoDeducao: Array<any>

  //#region MÉTODOS
  async obterPensaoDepJud() {
    const response = await this.service.obterDeducao(this.codInfoValores, this.codigoDedSusp);
    if (response){
          this.pensaoDepJud = response;
          this.iniciarForm();
    }
  }

  async obterListaIndApuracaoValores() {
    await this.serviceList.obterListaTipoDeducoesValoresAsync().then(x => {
      this.listaTipoDeducao = x      
    });
    this.iniciarForm();
  }

  async salvar() {
    let valueSubmit = this.formGroup.value

    try {
      if (this.titulo == "Alteração") {
        await this.service.atualizar(this.codF2501, this.codInfoValores, this.codigoDedSusp, valueSubmit);
      } 
      if (this.titulo == "Inclusão") {
        await this.service.incluir(this.codF2501, this.codInfoValores, valueSubmit);
      }

      await this.dialog.alert('Operação realizada', `${this.titulo} realizada com sucesso`);
      this.modal.close(true);

    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      await this.dialog.err(`Desculpe, não foi possivel realizar a ${this.titulo}`,mensagem)
    }
  }

  //#endregion


  static exibeModalAlterar(codF2501: number, codInfoValores: number, codigoDedSusp: number, temPermissaoBlocoCeDeE: boolean): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      DeducoesModal_v1_2Component,
      { windowClass: 'modal-dep-processos', centered: true, size: 'lg', backdrop: 'static' }
    );
    modalRef.componentInstance.codF2501 = codF2501;
    modalRef.componentInstance.codInfoValores = codInfoValores;
    modalRef.componentInstance.codigoDedSusp = codigoDedSusp;
    modalRef.componentInstance.temPermissaoBlocoCeDeE = temPermissaoBlocoCeDeE;
    modalRef.componentInstance.titulo = temPermissaoBlocoCeDeE ? 'Alteração' : 'Consulta';
    modalRef.componentInstance.obterPensaoDepJud();
    return modalRef.result;
  }

  static exibeModalIncluir(codF2501: number, codInfoValores: number): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      DeducoesModal_v1_2Component,
      { windowClass: 'modal-dep-processos', centered: true, size: 'lg', backdrop: 'static' }
    );
    modalRef.componentInstance.codF2501 = codF2501;
    modalRef.componentInstance.codInfoValores = codInfoValores;
    modalRef.componentInstance.temPermissaoBlocoCeDeE = true;
    modalRef.componentInstance.titulo = 'Inclusão';
    modalRef.componentInstance.iniciarForm();
    return modalRef.result;
  }

  close(): void {
    this.modal.close(false);
  }

  //#region FORMCONTROL

  dedsuspIndtpdeducaoFormControl: FormControl = new FormControl(null, Validators.required);
  dedsuspVlrdedsuspFormControl: FormControl = new FormControl(null, [Validators.min(0.01)]);

  formGroup: FormGroup = new FormGroup({
    dedsuspIndtpdeducao: this.dedsuspIndtpdeducaoFormControl,
    dedsuspVlrdedsusp: this.dedsuspVlrdedsuspFormControl
  });

  iniciarForm() {
    if(this.codigoDedSusp > 0){
      this.dedsuspIndtpdeducaoFormControl.setValue(this.pensaoDepJud.dedsuspIndtpdeducao);
      this.dedsuspVlrdedsuspFormControl.setValue(this.pensaoDepJud.dedsuspVlrdedsusp);
      if (!this.temPermissaoBlocoCeDeE) {
        this.dedsuspIndtpdeducaoFormControl.disable();
        this.dedsuspVlrdedsuspFormControl.disable();
      }
    }else{
      this.dedsuspIndtpdeducaoFormControl.setValue(null);
      this.dedsuspVlrdedsuspFormControl.setValue(null);
    }
  }

  desabilitaTooltip(control: FormControl):boolean{
    return control.valid || control.untouched || control.disabled
  }
  //#endregion
}