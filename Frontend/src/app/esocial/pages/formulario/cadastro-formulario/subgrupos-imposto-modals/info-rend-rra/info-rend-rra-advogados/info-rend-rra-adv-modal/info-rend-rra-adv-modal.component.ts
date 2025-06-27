import { AfterViewInit, Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ErrorLib } from '@esocial/libs/error-lib';
import { ideAdvogado } from '@esocial/models/subgrupos/v1_2/ideAdvogado';
import { ESocialListaFormularioService } from '@esocial/services/aplicacao/e-social-lista-formulario.service';
import { InfoRendRraAdvogadosService } from '@esocial/services/formulario/subgrupos/imposto-subgrupos-services/info-rend-rra-advogados.service';
import { StaticInjector } from '@esocial/static-injector';
import { EsocialFormcontrolCustomValidators } from '@esocial/validators/esocial-formcontrol-custom-validators';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DialogService } from '@shared/services/dialog.service';
import { FormControlCustomValidators } from '@shared/validators/form-control-custom-validators';

@Component({
  selector: 'app-info-rend-rra-adv-modal',
  templateUrl: './info-rend-rra-adv-modal.component.html',
  styleUrls: ['./info-rend-rra-adv-modal.component.scss']
})
export class InfoRendRraAdvModalComponent implements AfterViewInit {

  constructor(
    private modal: NgbActiveModal,
    private service: InfoRendRraAdvogadosService,
    private serviceList: ESocialListaFormularioService,
    private customValidators: FormControlCustomValidators,
    private eSocialValidators: EsocialFormcontrolCustomValidators,
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
  codigoIdeAdv: number;
  advogado: ideAdvogado;
  listaTipoAdv = [];
  listaTipoProcessos: Array<any>
  mascaraNrIncGenerica: (string | RegExp)[] = [
    /[0-9]/,
    /[0-9]/,
    /[0-9]/,
    /[0-9]/,
    /[0-9]/,
    /[0-9]/,
    /[0-9]/,
    /[0-9]/,
    /[0-9]/,
    /[0-9]/,
    /[0-9]/,
    /[0-9]/,
    /[0-9]/,
    /[0-9]/
  ];
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
  mascaraCnpj = [/[0-9]/, /[0-9]/, '.', /[0-9]/, /[0-9]/, /[0-9]/, '.', /[0-9]/, /[0-9]/, /[0-9]/, '/', /[0-9]/, /[0-9]/, /[0-9]/, /[0-9]/, '-', /[0-9]/, /[0-9]/];

  mascaraNrIncAdvogado: (string | RegExp)[] = this.mascaraNrIncGenerica;

  //#region MÉTODOS
  async obterAdvogado() {
    const response = await this.service.obterAdvogado(this.codIrrf, this.codigoIdeAdv);
    if (response){
          this.advogado = response;
          this.iniciarForm();
    }
  }

  async obterCodigoReceita() {
    this.listaTipoAdv = [{id:1, descricao: 'CNPJ'}, {id:2, descricao: 'CPF'}]
    this.iniciarForm();
  }

  async salvar() {
    let valueSubmit = this.formGroup.value
    this.formGroup.markAllAsTouched();
    if (this.formGroup.invalid) {
      return;
    }

    try {
      if (this.titulo == "Alteração") {
        await this.service.atualizar(this.codF2501, this.codIrrf, this.codigoIdeAdv, valueSubmit);
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

  adicionaValidators(){
    if (this.ideadvVlradvFormControl.value != null && this.ideadvVlradvFormControl.value == 0) {
      this.ideadvVlradvFormControl.setErrors({valorDespesaInvalido: true});
      this.ideadvVlradvFormControl.updateValueAndValidity;
      this.ideadvVlradvFormControl.markAsTouched;      
    }
    else {
      this.ideadvVlradvFormControl.setErrors(null);
      this.ideadvVlradvFormControl.updateValueAndValidity;
    }
  }

  //#endregion


  static exibeModalAlterar(codF2501: number, codIrrf: number, codigoIdeAdv: number, temPermissaoBlocoCeDeE: boolean): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      InfoRendRraAdvModalComponent,
      { windowClass: 'modal-rra-adv', centered: true, size: 'lg', backdrop: 'static' }
    );
    modalRef.componentInstance.codF2501 = codF2501;
    modalRef.componentInstance.codIrrf = codIrrf;
    modalRef.componentInstance.codigoIdeAdv = codigoIdeAdv;
    modalRef.componentInstance.temPermissaoBlocoCeDeE = temPermissaoBlocoCeDeE;
    modalRef.componentInstance.titulo = temPermissaoBlocoCeDeE ? 'Alteração' : 'Consulta';
    modalRef.componentInstance.obterAdvogado();
    return modalRef.result;
  }
  
  static exibeModalIncluir(codF2501: number, codIrrf: number): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      InfoRendRraAdvModalComponent,
      { windowClass: 'modal-rra-adv', centered: true, size: 'lg', backdrop: 'static' }
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
  ideadvTpinscFormControl: FormControl = new FormControl(null, Validators.required);
  ideadvNrinscFormControl: FormControl = new FormControl(null, Validators.required);
  ideadvVlradvFormControl: FormControl = new FormControl(null);

  formGroup: FormGroup = new FormGroup({
    ideadvTpinsc: this.ideadvTpinscFormControl,
    ideadvNrinsc: this.ideadvNrinscFormControl,
    ideadvVlradv: this.ideadvVlradvFormControl
  });

  iniciarForm() {
    if (!this.temPermissaoBlocoCeDeE) {
      this.ideadvTpinscFormControl.disable();
      this.ideadvNrinscFormControl.disable();
      this.ideadvVlradvFormControl.disable();
    }
    if(this.codigoIdeAdv){
      this.ideadvTpinscFormControl.setValue(this.advogado.ideadvTpinsc);
      this.ideadvNrinscFormControl.setValue(this.advogado.ideadvNrinsc);
      this.ideadvVlradvFormControl.setValue(this.advogado.ideadvVlradv);

      this.validaTpInsc();

    }else{
      this.ideadvTpinscFormControl.setValue(null);
      this.ideadvNrinscFormControl.setValue(null);
      this.ideadvVlradvFormControl.setValue(null);
    }
  }

  desabilitaTooltip(control: FormControl):boolean{
    return control.valid || control.untouched || control.disabled
  }

  removerNaoNumericosNumProcesso() {
    this.ideadvVlradvFormControl.setValue(this.ideadvVlradvFormControl.value.replace(/[^0-9]/g, ''));
  }
  
  validaTpInsc(): any {
    this.ideadvNrinscFormControl.clearValidators();
    if(!this.ideadvTpinscFormControl.value){
      this.ideadvNrinscFormControl.setValue(undefined);
      this.ideadvNrinscFormControl.setValidators([Validators.required, Validators.minLength(1)]);
      return;
    }

    switch(this.ideadvTpinscFormControl.value){
      case 1:        
        this.mascaraNrIncAdvogado = this.mascaraCnpj;
        this.ideadvNrinscFormControl.setValidators([Validators.required, this.customValidators.cnpjValido(), Validators.minLength(1)]);
        this.ideadvNrinscFormControl.updateValueAndValidity();
      break;
      case 2:
        this.mascaraNrIncAdvogado = this.mascaraCpf;
        this.ideadvNrinscFormControl.setValidators([Validators.required, this.customValidators.cpfValido(), Validators.minLength(1)]);
        this.ideadvNrinscFormControl.updateValueAndValidity();
      break;
      default:
        this.mascaraNrIncAdvogado = this.mascaraNrIncGenerica;
        this.ideadvNrinscFormControl.setValidators([Validators.required, Validators.minLength(1)]);
        this.ideadvNrinscFormControl.updateValueAndValidity();
        break;
    }
  }
  //#endregion
}