import { AfterViewInit, Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ErrorLib } from '@esocial/libs/error-lib';
import { ValorIrrf } from '@esocial/models/subgrupos/v1_2/valorIrrf';
import { ESocialListaFormularioService } from '@esocial/services/aplicacao/e-social-lista-formulario.service';
import { ValoresDepJudModalService } from '@esocial/services/formulario/subgrupos/imposto-subgrupos-services/valores-dep-jud-modal.service';
import { StaticInjector } from '@esocial/static-injector';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DialogService } from '@shared/services/dialog.service';

@Component({
  selector: 'app-valores-modal_v1_2',
  templateUrl: './valores-modal_v1_2.component.html',
  styleUrls: ['./valores-modal_v1_2.component.scss']
})
export class ValoresModal_v1_2Component implements AfterViewInit {

  constructor(
    private modal: NgbActiveModal,
    private service: ValoresDepJudModalService,
    private serviceList: ESocialListaFormularioService,
    private dialog: DialogService,
  ) { }

  ngAfterViewInit(): void {
    this.obterListaIndApuracaoValores();
  }

  temPermissaoBlocoCeDeE: boolean = false;
  titulo: string;
  codF2501: number;
  codigoInfoValores : number;
  codigoInfoProcRet : number;
  valor: ValorIrrf;

  codigoReceitaList = [];
  regex12_2 = /^([0-9]{1,10}\,[0-9]{2})/g;
  listaIndApuracao: Array<any>


  //#region MÉTODOS
  async obterValor() {
    const response = await this.service.obterValor(this.codigoInfoProcRet, this.codigoInfoValores);
    if (response){
          this.valor = response;
          this.iniciarForm();
    }
  }

  async obterListaIndApuracaoValores() {
    await this.serviceList.obterListaIndApuracaoValoresAsync().then(x => {
      this.listaIndApuracao = x      
    });
    this.iniciarForm();
  }

  async salvar() {
    let valueSubmit = this.formGroup.value

    try {
      if (this.titulo == "Alteração") {
        await this.service.atualizar(this.codF2501, this.codigoInfoProcRet, this.codigoInfoValores, valueSubmit);
      } 
      if (this.titulo == "Inclusão") {
        await this.service.incluir(this.codF2501, this.codigoInfoProcRet, valueSubmit);
      }

      await this.dialog.alert('Operação realizada', `${this.titulo} realizada com sucesso`);
      this.modal.close(true);

    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      await this.dialog.err(`Desculpe, não foi possivel realizar a ${this.titulo}`,mensagem)
    }
  }

  //#endregion


  static exibeModalAlterar(codF2501: number, codigoInfoValores: number, codigoInfoProcRet: number, temPermissaoBlocoCeDeE: boolean): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      ValoresModal_v1_2Component,
      { windowClass: 'modal-dep-processos', centered: true, size: 'lg', backdrop: 'static' }
    );
    modalRef.componentInstance.codF2501 = codF2501;
    modalRef.componentInstance.codigoInfoValores = codigoInfoValores;
    modalRef.componentInstance.codigoInfoProcRet = codigoInfoProcRet;
    modalRef.componentInstance.temPermissaoBlocoCeDeE = temPermissaoBlocoCeDeE;
    modalRef.componentInstance.titulo = temPermissaoBlocoCeDeE ? 'Alteração' : 'Consulta';
    modalRef.componentInstance.obterValor();
    return modalRef.result;
  }
  
  static exibeModalIncluir(codF2501: number,  codigoInfoProcRet: number): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      ValoresModal_v1_2Component,
      { windowClass: 'modal-dep-processos', centered: true, size: 'lg', backdrop: 'static' }
      );
      modalRef.componentInstance.codF2501 = codF2501;
      modalRef.componentInstance.codigoInfoProcRet = codigoInfoProcRet;
    modalRef.componentInstance.temPermissaoBlocoCeDeE = true;
    modalRef.componentInstance.titulo = 'Inclusão';
    modalRef.componentInstance.iniciarForm();
    return modalRef.result;
  }

  close(): void {
    this.modal.close(false);
  }

  //#region FORMCONTROL

  infovaloresIndapuracaoFormControl: FormControl = new FormControl(null, Validators.required);
  infovaloresVlrnretidoFormControl: FormControl = new FormControl(null, [Validators.min(0.01)]);
  infovaloresVlrdepjudFormControl: FormControl = new FormControl(null, [Validators.min(0.01)]);
  infovaloresVlrcmpanocalFormControl: FormControl = new FormControl(null, [Validators.min(0.01)]);
  infovaloresVlrcmpanoantFormControl: FormControl = new FormControl(null, [Validators.min(0.01)]);
  infovaloresVlrrendsuspFormControl: FormControl = new FormControl(null, [Validators.min(0.01)]);

  formGroup: FormGroup = new FormGroup({
    infovaloresIndapuracao: this.infovaloresIndapuracaoFormControl,
    infovaloresVlrnretido: this.infovaloresVlrnretidoFormControl,
    infovaloresVlrdepjud: this.infovaloresVlrdepjudFormControl,
    infovaloresVlrcmpanocal: this.infovaloresVlrcmpanocalFormControl,
    infovaloresVlrcmpanoant: this.infovaloresVlrcmpanoantFormControl,
    infovaloresVlrrendsusp: this.infovaloresVlrrendsuspFormControl
  });

  iniciarForm() {
    if(this.codigoInfoValores > 0){
      this.infovaloresIndapuracaoFormControl.setValue(this.valor.infovaloresIndapuracao);
      this.infovaloresVlrnretidoFormControl.setValue(this.valor.infovaloresVlrnretido);
      this.infovaloresVlrdepjudFormControl.setValue(this.valor.infovaloresVlrdepjud);
      this.infovaloresVlrcmpanocalFormControl.setValue(this.valor.infovaloresVlrcmpanocal);
      this.infovaloresVlrcmpanoantFormControl.setValue(this.valor.infovaloresVlrcmpanoant);
      this.infovaloresVlrrendsuspFormControl.setValue(this.valor.infovaloresVlrrendsusp);

      if (!this.temPermissaoBlocoCeDeE) {
        this.infovaloresIndapuracaoFormControl.disable();
        this.infovaloresVlrnretidoFormControl.disable();
        this.infovaloresVlrdepjudFormControl.disable();
        this.infovaloresVlrcmpanocalFormControl.disable();
        this.infovaloresVlrcmpanoantFormControl.disable();
        this.infovaloresVlrrendsuspFormControl.disable();
      }

    }else{
        this.infovaloresIndapuracaoFormControl.setValue(null);
        this.infovaloresVlrnretidoFormControl.setValue(null);
        this.infovaloresVlrdepjudFormControl.setValue(null);
        this.infovaloresVlrcmpanocalFormControl.setValue(null);
        this.infovaloresVlrcmpanoantFormControl.setValue(null);
        this.infovaloresVlrrendsuspFormControl.setValue(null);
    }
  }

  desabilitaTooltip(control: FormControl):boolean{
    return control.valid || control.untouched || control.disabled
  }

  //#endregion
}