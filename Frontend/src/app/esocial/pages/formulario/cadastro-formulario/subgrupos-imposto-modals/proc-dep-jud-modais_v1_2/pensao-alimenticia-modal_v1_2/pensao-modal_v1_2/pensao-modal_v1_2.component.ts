import { AfterViewInit, Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ErrorLib } from '@esocial/libs/error-lib';
import { BenefPensao } from '@esocial/models/subgrupos/v1_2/benefPensao';
import { ESocialListaFormularioService } from '@esocial/services/aplicacao/e-social-lista-formulario.service';
import { PensaoDepJudModalService } from '@esocial/services/formulario/subgrupos/imposto-subgrupos-services/pensao-dep-jud-modal.service';
import { StaticInjector } from '@esocial/static-injector';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DialogService } from '@shared/services/dialog.service';
import { FormControlCustomValidators } from '@shared/validators/form-control-custom-validators';

@Component({
  selector: 'app-pensao-modal_v1_2',
  templateUrl: './pensao-modal_v1_2.component.html',
  styleUrls: ['./pensao-modal_v1_2.component.scss']
})
export class PensaoModal_v1_2Component implements AfterViewInit {

  constructor(
    private modal: NgbActiveModal,
    private service: PensaoDepJudModalService,
    private serviceList: ESocialListaFormularioService,
    private customValidators: FormControlCustomValidators,
    private dialog: DialogService,
  ) { }

  ngAfterViewInit(): void {
    this.iniciarForm();
  }

  temPermissaoBlocoCeDeE: boolean = false;
  titulo: string;
  codF2501: number;
  codigoDedSusp: number;
  codigoBenefPen: number;
  benefPensao: BenefPensao;
  codigoReceitaList = [];
  descCount = 0;

  mascaraCpf = [
    /[0-9]/,
    /[0-9]/,
    /[0-9]/,
    '.',
    /[0-9]/,
    /[0-9]/,
    /[0-9]/,
    '.',
    /[0-9]/,
    /[0-9]/,
    /[0-9]/,
    '-',
    /[0-9]/,
    /[0-9]/
  ];


  //#region MÉTODOS
  async obterBenefPensao() {
    const response = await this.service.obterPensao(this.codigoDedSusp, this.codigoBenefPen);
    if (response){
          this.benefPensao = response;
          this.iniciarForm();
    }
  }

  async salvar() {
    this.benefpenCpfdepFormControl.setValue(this.benefpenCpfdepFormControl.value.replace(/[.-]/g, ''))
    let valueSubmit = this.formGroup.value;

    try {
      if (this.titulo == "Alteração") {
        await this.service.atualizar(this.codF2501, this.codigoDedSusp, this.codigoBenefPen, valueSubmit);
      } 
      if (this.titulo == "Inclusão") {
        await this.service.incluir(this.codF2501, this.codigoDedSusp, valueSubmit);
      }

      await this.dialog.alert('Operação realizada', `${this.titulo} realizada com sucesso`);
      this.modal.close(true);

    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      await this.dialog.err(`Desculpe, não foi possivel realizar a ${this.titulo}`,mensagem)
    }
  }

  //#endregion


  static exibeModalAlterar(codF2501: number, codigoDedSusp: number, codigoBenefPen: number, temPermissaoBlocoCeDeE: boolean): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      PensaoModal_v1_2Component,
      { windowClass: 'modal-dep-processos', centered: true, size: 'lg', backdrop: 'static' }
    );
    modalRef.componentInstance.codF2501 = codF2501;
    modalRef.componentInstance.codigoDedSusp = codigoDedSusp;
    modalRef.componentInstance.codigoBenefPen = codigoBenefPen;
    modalRef.componentInstance.temPermissaoBlocoCeDeE = temPermissaoBlocoCeDeE;
    modalRef.componentInstance.titulo = temPermissaoBlocoCeDeE ? 'Alteração' : 'Consulta';
    modalRef.componentInstance.obterBenefPensao();
    return modalRef.result;
  }

  static exibeModalIncluir(codF2501: number, codigoDedSusp: number): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      PensaoModal_v1_2Component,
      { windowClass: 'modal-dep-processos', centered: true, size: 'lg', backdrop: 'static' }
    );
    modalRef.componentInstance.codF2501 = codF2501;
    modalRef.componentInstance.codigoDedSusp = codigoDedSusp;
    modalRef.componentInstance.temPermissaoBlocoCeDeE = true;
    modalRef.componentInstance.titulo = 'Inclusão';
    modalRef.componentInstance.iniciarForm();
    return modalRef.result;
  }

  close(): void {
    this.modal.close(false);
  }

  //#region FORMCONTROL
  benefpenCpfdepFormControl: FormControl = new FormControl(null, [Validators.required, this.customValidators.cpfValido()]);
  benefpenVlrdepensuspFormControl: FormControl = new FormControl(null, [Validators.required, Validators.min(0.01)]);

  formGroup: FormGroup = new FormGroup({
    benefpenCpfdep: this.benefpenCpfdepFormControl,
    benefpenVlrdepensusp: this.benefpenVlrdepensuspFormControl,
  });

  iniciarForm() {
    if(this.codigoBenefPen > 0){
      this.benefpenCpfdepFormControl.setValue(this.benefPensao.benefpenCpfdep);
      this.benefpenVlrdepensuspFormControl.setValue(this.benefPensao.benefpenVlrdepensusp);
      if (!this.temPermissaoBlocoCeDeE) {
        this.benefpenCpfdepFormControl.disable();
        this.benefpenVlrdepensuspFormControl.disable();
      }
    }else{
      this.benefpenCpfdepFormControl.setValue(null);
      this.benefpenVlrdepensuspFormControl.setValue(null);
    }
  }

  desabilitaTooltip(control: FormControl):boolean{
    return control.valid || control.untouched || control.disabled
  }

  removerNaoNumericosCpf() {
    this.benefpenCpfdepFormControl.setValue(this.benefpenCpfdepFormControl.value.replace(/[^0-9]/g, ''));
  }
  //#endregion
}