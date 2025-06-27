import { AfterViewInit, Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ErrorLib } from '@esocial/libs/error-lib';
import { ProcessoIRRF } from '@esocial/models/subgrupos/v1_2/processoIrrf';
import { ESocialListaFormularioService } from '@esocial/services/aplicacao/e-social-lista-formulario.service';
import { ProcessosDepJudModalService } from '@esocial/services/formulario/subgrupos/imposto-subgrupos-services/processos-dep-jud-modal.service';
import { StaticInjector } from '@esocial/static-injector';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DialogService } from '@shared/services/dialog.service';

@Component({
  selector: 'app-processos-modal_v1_2',
  templateUrl: './processos-modal_v1_2.component.html',
  styleUrls: ['./processos-modal_v1_2.component.scss']
})
export class ProcessosModal_v1_2Component implements AfterViewInit {

  constructor(
    private modal: NgbActiveModal,
    private service: ProcessosDepJudModalService,
    private serviceList: ESocialListaFormularioService,
    private dialog: DialogService,
  ) { }

  ngAfterViewInit(): void {
    this.obterCodigoReceita();
  }

  temPermissaoBlocoCeDeE: boolean = false;
  titulo: string;
  codF2501: number;
  codIrrf: number;
  codReceita: string;
  valorIrrf: number;
  idEsF2501Infoprocret: number;
  processo: ProcessoIRRF;
  codigoReceitaList = [];
  listaTipoProcessos: Array<any>

  //#region MÉTODOS
  async obterProcessoIRRF() {
    const response = await this.service.obterProcessoIRRF(this.codIrrf, this.idEsF2501Infoprocret);
    if (response){
          this.processo = response;
          this.iniciarForm();
    }
  }

  async obterCodigoReceita() {
    await this.serviceList.obterListaTipoProcessoIRRFAsync().then(x => {
      this.listaTipoProcessos = x      
    });
    this.iniciarForm();
  }

  async salvar() {
    let valueSubmit = this.formGroup.value
    try {
      if (this.titulo == "Alteração") {
        await this.service.atualizar(this.codF2501, this.codIrrf, this.idEsF2501Infoprocret, valueSubmit);
      } 
      if (this.titulo == "Inclusão") {
        await this.service.incluir(this.codF2501, this.codIrrf, valueSubmit);
      }

      await this.dialog.alert('Operação realizada', `${this.titulo} realizada com sucesso`);
      this.modal.close(true);

    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      await this.dialog.err(`Desculpe, não foi possivel realizar a ${this.titulo}`,mensagem)
    }
  }

  //#endregion


  static exibeModalAlterar(codF2501: number, codIrrf: number, idEsF2501Infoprocret: number, temPermissaoBlocoCeDeE: boolean): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      ProcessosModal_v1_2Component,
      { windowClass: 'modal-dep-processos', centered: true, size: 'lg', backdrop: 'static' }
    );
    modalRef.componentInstance.codF2501 = codF2501;
    modalRef.componentInstance.codIrrf = codIrrf;
    modalRef.componentInstance.idEsF2501Infoprocret = idEsF2501Infoprocret;
    modalRef.componentInstance.temPermissaoBlocoCeDeE = temPermissaoBlocoCeDeE;
    modalRef.componentInstance.titulo = temPermissaoBlocoCeDeE ? 'Alteração' : 'Consulta';
    modalRef.componentInstance.obterProcessoIRRF();
    return modalRef.result;
  }
  
  static exibeModalIncluir(codF2501: number, codIrrf: number): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      ProcessosModal_v1_2Component,
      { windowClass: 'modal-dep-processos', centered: true, size: 'lg', backdrop: 'static' }
      );
    modalRef.componentInstance.codF2501 = codF2501;
    modalRef.componentInstance.codIrrf = codIrrf;
    modalRef.componentInstance.temPermissaoBlocoCeDeE = true;
    modalRef.componentInstance.titulo = 'Inclusão';
    modalRef.componentInstance.iniciarForm();
    return modalRef.result;
  }

  close(): void {
    this.modal.close(false);
  }

  //#region FORMCONTROL

  infoprocretTpprocretFormControl: FormControl = new FormControl(null, Validators.required);
  infoprocretNrprocretFormControl: FormControl = new FormControl(null, Validators.required);
  infoprocretCodsuspFormControl: FormControl = new FormControl(null);

  formGroup: FormGroup = new FormGroup({
    infoprocretTpprocret: this.infoprocretTpprocretFormControl,
    infoprocretNrprocret: this.infoprocretNrprocretFormControl,
    infoprocretCodsusp: this.infoprocretCodsuspFormControl
  });

  iniciarForm() {
    if (!this.temPermissaoBlocoCeDeE) {
      this.infoprocretTpprocretFormControl.disable();
      this.infoprocretNrprocretFormControl.disable();
      this.infoprocretCodsuspFormControl.disable();
    }
    if(this.idEsF2501Infoprocret){
      this.infoprocretTpprocretFormControl.setValue(this.processo.infoprocretTpprocret);
      this.infoprocretNrprocretFormControl.setValue(this.processo.infoprocretNrprocret);
      this.infoprocretCodsuspFormControl.setValue(this.processo.infoprocretCodsusp);

    }else{
      this.infoprocretTpprocretFormControl.setValue(null);
      this.infoprocretNrprocretFormControl.setValue(null);
      this.infoprocretCodsuspFormControl.setValue(null);
    }
  }

  desabilitaTooltip(control: FormControl):boolean{
    return control.valid || control.untouched || control.disabled
  }

  removerNaoNumericosCodProcesso() {
    this.infoprocretCodsuspFormControl.setValue(this.infoprocretCodsuspFormControl.value.replace(/[^0-9]/g, ''));
  }
  //#endregion
}