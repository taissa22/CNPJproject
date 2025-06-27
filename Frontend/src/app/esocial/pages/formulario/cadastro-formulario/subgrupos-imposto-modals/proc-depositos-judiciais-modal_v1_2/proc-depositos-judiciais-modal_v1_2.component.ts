import { AfterViewInit, Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { ErrorLib } from '@esocial/libs/error-lib';
import { ESocialListaFormularioService } from '@esocial/services/aplicacao/e-social-lista-formulario.service';
import { StaticInjector } from '@esocial/static-injector';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DialogService } from '@shared/services/dialog.service';

@Component({
  selector: 'app-proc-depositos-judiciais-modal_v1_2',
  templateUrl: './proc-depositos-judiciais-modal_v1_2.component.html',
  styleUrls: ['./proc-depositos-judiciais-modal_v1_2.component.scss']
})
export class ProcDepositosJudiciaisModal_v1_2Component implements AfterViewInit {

  constructor(
    private modal: NgbActiveModal,
    // private service: ImpostoService,
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
  valorIrrf13: number;
  // imposto: Imposto;
  codigoReceitaList = [];
  regex12_2 = /^([0-9]{1,10}\,[0-9]{2})/g;
  descCount = 0;


  //#region MÉTODOS
  async obterImposto() {
    // const response = await this.service.obterImposto(this.codF2501, this.codIrrf);
    // if (response){
    //       this.imposto = response;
    //       this.iniciarForm();
    // }
  }

  async obterCodigoReceita() {
    const response = await this.serviceList.obterCodigoReceitaIRRFAsync();
    if (response)
    {
      this.codigoReceitaList = response.map((cr) => {
        return(
          {
            id: cr.id,
            descricao: cr.descricao,
            descricaoConcatenada: `${cr.id} - ${cr.descricao}`
          }
        );
      });
      this.iniciarForm();
    }
  }

  async salvar() {
    let operacao = this.titulo == 'Alterar' ? 'Alteração' : 'Inclusão';
    let valueSubmit = this.formGroup.value
    valueSubmit.calctribPerref = Number(valueSubmit.calctribPerref);
    valueSubmit.calctribVrbccpmensal = Number(valueSubmit.calctribVrbccpmensal);

    try {
      if (this.titulo == "Alterar") {
        // await this.service.atualizar(this.codF2501, this.codIrrf, valueSubmit);
      } else {
        // await this.service.incluir(this.codF2501, valueSubmit);
      }

      await this.dialog.alert('Operação realizada', `${operacao} realizada com sucesso`);
      this.modal.close(true);

    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      await this.dialog.err(`Desculpe, não foi possivel realizar a ${operacao}`,mensagem)
    }
  }

  //#endregion


  static exibeModalAlterar(codF2501: number, codIrrf: number, codReceita: string, valorIrrf: number, valorIrrf13: number, temPermissaoBlocoCeDeE: boolean): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      ProcDepositosJudiciaisModal_v1_2Component,
      { windowClass: 'modal-judicial', centered: true, size: 'lg', backdrop: 'static' }
    );
    modalRef.componentInstance.codF2501 = codF2501;
    modalRef.componentInstance.codIrrf = codIrrf;
    modalRef.componentInstance.codReceita = codReceita;
    modalRef.componentInstance.valorIrrf = valorIrrf;
    modalRef.componentInstance.valorIrrf13 = valorIrrf13;
    modalRef.componentInstance.temPermissaoBlocoCeDeE = temPermissaoBlocoCeDeE;
    modalRef.componentInstance.obterImposto();
    return modalRef.result;
  }

  static exibeModalIncluir(codF2501: number, codIrrf: number): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      ProcDepositosJudiciaisModal_v1_2Component,
      { windowClass: 'modal-judicial', centered: true, size: 'lg', backdrop: 'static' }
    );
    modalRef.componentInstance.codF2501 = codF2501;
    modalRef.componentInstance.codIrrf = codIrrf;
    modalRef.componentInstance.temPermissaoBlocoCeDeE = true;
    modalRef.componentInstance.titulo = 'Incluir';
    modalRef.componentInstance.iniciarForm();
    return modalRef.result;
  }

  close(): void {
    this.modal.close(false);
  }

  //#region FORMCONTROL

  infocrcontribTpcrFormControl: FormControl = new FormControl(null);
  infocrcontribVrcrFormControl: FormControl = new FormControl(null);
  descricaoFormControl: FormControl = new FormControl(null);

  formGroup: FormGroup = new FormGroup({
    infocrcontribTpcr: this.infocrcontribTpcrFormControl,
    infocrcontribVrcr: this.infocrcontribVrcrFormControl,
    descricao: this.descricaoFormControl
  });

  iniciarForm() {
    if(this.codIrrf > 0){
      // this.infocrcontribTpcrFormControl.setValue(this.imposto.infocrcontribTpcr);
      // this.infocrcontribVrcrFormControl.setValue(this.imposto.infocrcontribVrcr);
      if (!this.temPermissaoBlocoCeDeE) {
        this.infocrcontribTpcrFormControl.disable();
        this.infocrcontribVrcrFormControl.disable();
        this.descricaoFormControl.disable();
      }
    }else{
      this.infocrcontribTpcrFormControl.setValue(null);
      this.infocrcontribVrcrFormControl.setValue(null);
      this.descricaoFormControl.setValue(null);
    }
  }

  desabilitaTooltip(control: FormControl):boolean{
    return control.valid || control.untouched || control.disabled
  }

  countDesc(){
    this.descCount = this.descricaoFormControl.value.length;
  }
  //#endregion
}
