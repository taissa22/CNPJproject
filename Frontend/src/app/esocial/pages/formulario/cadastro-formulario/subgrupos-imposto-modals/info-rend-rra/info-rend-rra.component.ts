import { AfterViewInit, Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ErrorLib } from '@esocial/libs/error-lib';
import { InfoRendRra } from '@esocial/models/subgrupos/v1_2/infoRRA';
import { InfoRendRraService } from '@esocial/services/formulario/subgrupos/imposto-subgrupos-services/info-rend-rra.service';
import { StaticInjector } from '@esocial/static-injector';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DialogService } from '@shared/services/dialog.service';

@Component({
  selector: 'app-info-rend-rra',
  templateUrl: './info-rend-rra.component.html',
  styleUrls: ['./info-rend-rra.component.scss']
})
export class InfoRendRraComponent implements AfterViewInit {

  constructor(
    private modal: NgbActiveModal,
    private service: InfoRendRraService,
    // private serviceList: ESocialListaFormularioService,
    private dialog: DialogService,
  ) { }

  ngAfterViewInit(): void {
    this.iniciarForm();
  }

  temPermissaoBlocoCeDeE: boolean = false;
  titulo: string;
  codF2501: number;
  codIrrf: number;
  codReceita: string;
  valorIrrf: number;
  valorIrrf13: number;
  infoRra: InfoRendRra;
  codigoReceitaList = [];
  regex12_2 = /^([0-9]{1,10}\,[0-9]{2})/g;
  regex3_1 = /^[0-9]{1,3}(\,[0-9]{1})?/g;
  descCount = 0;


  //#region MÉTODOS
  async obterRRA() {
    const response = await this.service.obterRra(this.codF2501, this.codIrrf);
    if (response){
      this.infoRra = response;
      this.iniciarForm();
    }
  }

  async salvar() {
    let valueSubmit = this.formGroup.value
    valueSubmit.inforraQtdmesesrra = valueSubmit.inforraQtdmesesrra ? valueSubmit.inforraQtdmesesrra.replace(',', '.'): null;

    this.adicionaValidators();

    this.formGroup.markAllAsTouched();
    if (this.formGroup.invalid) {
      return;
    }
    
    try {
      await this.service.atualizar(this.codF2501, this.codIrrf, valueSubmit);

      await this.dialog.alert('Operação realizada', `Alteração realizada com sucesso`);
      this.modal.close(true);

    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      await this.dialog.err(`Desculpe, não foi possivel realizar a Alteração`,mensagem)
    }
  }

  //#endregion


  static exibeModalAlterar(codF2501: number, codIrrf: number, codReceita: string, valorIrrf: number, valorIrrf13: number, temPermissaoBlocoCeDeE: boolean): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      InfoRendRraComponent,
      { windowClass: 'modal-rra', centered: true, size: 'lg', backdrop: 'static' }
    );
    modalRef.componentInstance.codF2501 = codF2501;
    modalRef.componentInstance.codIrrf = codIrrf;
    modalRef.componentInstance.codReceita = codReceita;
    modalRef.componentInstance.valorIrrf = valorIrrf;
    modalRef.componentInstance.valorIrrf13 = valorIrrf13;
    modalRef.componentInstance.temPermissaoBlocoCeDeE = temPermissaoBlocoCeDeE;
    modalRef.componentInstance.obterRRA();
    return modalRef.result;
  }

  static exibeModalIncluir(codF2501: number, codIrrf: number): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      InfoRendRraComponent,
      { windowClass: 'modal-rra', centered: true, size: 'lg', backdrop: 'static' }
    );
    modalRef.componentInstance.codF2501 = codF2501;
    modalRef.componentInstance.codIrrf = codIrrf;
    modalRef.componentInstance.temPermissaoBlocoCeDeE = true;
    modalRef.componentInstance.iniciarForm();
    return modalRef.result;
  }

  close(): void {
    this.modal.close(false);
  }

  //#region FORMCONTROL

  inforraDescrraFormControl: FormControl = new FormControl(null);
  inforraQtdmesesrraFormControl: FormControl = new FormControl(null);
  despprocjudVlrdespcustasFormControl: FormControl = new FormControl(null);
  despprocjudVlrdespadvogadosFormControl: FormControl = new FormControl(null);

  formGroup: FormGroup = new FormGroup({
    inforraDescrra : this.inforraDescrraFormControl,
    inforraQtdmesesrra : this.inforraQtdmesesrraFormControl,
    despprocjudVlrdespcustas : this.despprocjudVlrdespcustasFormControl,
    despprocjudVlrdespadvogados : this.despprocjudVlrdespadvogadosFormControl,
  });

  iniciarForm() {
    if(this.codIrrf > 0){
      this.inforraDescrraFormControl.setValue(this.infoRra.inforraDescrra)
      this.inforraQtdmesesrraFormControl.setValue(this.infoRra.inforraQtdmesesrra != null ? this.infoRra.inforraQtdmesesrra.toString().replace('.',',') : null)
      this.despprocjudVlrdespcustasFormControl.setValue(this.infoRra.despprocjudVlrdespcustas)
      this.despprocjudVlrdespadvogadosFormControl.setValue(this.infoRra.despprocjudVlrdespadvogados)

      this.countDesc();

      if (!this.temPermissaoBlocoCeDeE) {
          this.inforraDescrraFormControl.disable();
          this.inforraQtdmesesrraFormControl.disable();
          this.despprocjudVlrdespcustasFormControl.disable();
          this.despprocjudVlrdespadvogadosFormControl.disable();
      }

      if (this.codReceita && this.codReceita != '188951 - IRRF - RRA') {
        this.inforraDescrraFormControl.disable();
        this.inforraQtdmesesrraFormControl.disable();
      }
    }else{
      this.inforraDescrraFormControl.setValue(null);
      this.inforraQtdmesesrraFormControl.setValue(null);
      this.despprocjudVlrdespcustasFormControl.setValue(null);
      this.despprocjudVlrdespadvogadosFormControl.setValue(null);
    }
  }

  desabilitaTooltip(control: FormControl):boolean{
    return control.valid || control.untouched || control.disabled
  }

  countDesc(){
    this.descCount = this.inforraDescrraFormControl.value ? this.inforraDescrraFormControl.value.length: 0;
  }

  removerNaoNumericosMeses() {
    let value = this.inforraQtdmesesrraFormControl.value;

    value = value.replace(/[^0-9\,]/g, '');

    if (value.split('').filter((item) => (item == ',')).length > 1) {
      value = value.substring(0, value.length -1);
    }

    if (value == ',') {
      value = '';
    }

    let valueCheck = value.split(',');    

    if (valueCheck[0] != null && Number(valueCheck[0]) > 999) {
      valueCheck[0] = valueCheck[0].toString().substring(0,3);
    }

    if (valueCheck[1] != null && Number(valueCheck[1]) > 9)
    {
       valueCheck[1] = valueCheck[1].toString().substring(0,1);        
    } 

    value = valueCheck.join(','); 

    this.inforraQtdmesesrraFormControl.setValue(value);
  }

  adicionaValidators(){
    // if (this.inforraDescrraFormControl.value && (this.inforraQtdmesesrraFormControl.value == null || this.inforraQtdmesesrraFormControl.value == '')) {
    //   this.inforraQtdmesesrraFormControl.setValidators([Validators.required]);      
    // } 
    // else
    // {
    //   this.inforraQtdmesesrraFormControl.clearValidators();
    // }

    // if (this.inforraQtdmesesrraFormControl.value && (this.inforraDescrraFormControl.value == null || this.inforraDescrraFormControl.value == '')) {
    //   this.inforraDescrraFormControl.setValidators([Validators.required]);
    // } 
    // else
    // {
    //   this.inforraDescrraFormControl.clearValidators();
    // }

    if (this.despprocjudVlrdespcustasFormControl.value && (this.despprocjudVlrdespadvogadosFormControl.value == null || this.despprocjudVlrdespadvogadosFormControl.value == '')) {
      this.despprocjudVlrdespadvogadosFormControl.setValidators([Validators.required]);      
    }
    else
    {
      this.despprocjudVlrdespadvogadosFormControl.clearValidators();
    }

    if (this.despprocjudVlrdespadvogadosFormControl.value && (this.despprocjudVlrdespcustasFormControl.value == null || this.despprocjudVlrdespcustasFormControl.value == '')) {
      this.despprocjudVlrdespcustasFormControl.setValidators([Validators.required]);
    }
    else
    {
      this.despprocjudVlrdespcustasFormControl.clearValidators();
    }

    this.inforraQtdmesesrraFormControl.updateValueAndValidity();
    this.inforraQtdmesesrraFormControl.markAsTouched();
    this.inforraDescrraFormControl.updateValueAndValidity();
    this.inforraDescrraFormControl.markAsTouched();

    this.despprocjudVlrdespadvogadosFormControl.updateValueAndValidity();
    this.despprocjudVlrdespadvogadosFormControl.markAsTouched();
    this.despprocjudVlrdespcustasFormControl.updateValueAndValidity();
    this.despprocjudVlrdespcustasFormControl.markAsTouched();
  }

  validaValorMeses(){
    let erros = this.inforraQtdmesesrraFormControl.errors;
    if (this.inforraQtdmesesrraFormControl.value != null && this.inforraQtdmesesrraFormControl.value != '' && Number(this.inforraQtdmesesrraFormControl.value) <= 0) {
            
      if (erros != null) {
        erros['valorInvalido'] = true;
        this.inforraQtdmesesrraFormControl.setErrors(erros);
      } else {
        this.inforraQtdmesesrraFormControl.setErrors({valorInvalido: true});
      }
              
      this.inforraQtdmesesrraFormControl.updateValueAndValidity;
      this.inforraQtdmesesrraFormControl.markAsTouched();
    } else {
      this.inforraQtdmesesrraFormControl.setErrors(erros);
    }
    let errosRc = this.inforraQtdmesesrraFormControl.errors;
    if (!this.inforraQtdmesesrraFormControl.value && (this.codReceita && this.codReceita == '188951 - IRRF - RRA')) {
      if (errosRc != null) {
        errosRc['campoMesesObrigatorioRRA'] = true;
        this.inforraQtdmesesrraFormControl.setErrors(errosRc);
      } else {
        this.inforraQtdmesesrraFormControl.setErrors({campoMesesObrigatorioRRA: true});
      }
              
      this.inforraQtdmesesrraFormControl.updateValueAndValidity;
      this.inforraQtdmesesrraFormControl.markAsTouched();
    }else {
      this.inforraQtdmesesrraFormControl.setErrors(errosRc);
    }
    this.validaDescricaoRRA();
  }

  validaDescricaoRRA(){
    let erros = this.inforraDescrraFormControl.errors;       
    if (!this.inforraDescrraFormControl.value && (this.codReceita && this.codReceita == '188951 - IRRF - RRA')) {
      if (erros != null) {
        erros['campoObrigatorioRRA'] = true;
        this.inforraDescrraFormControl.setErrors(erros);
      } else {
        this.inforraDescrraFormControl.setErrors({campoObrigatorioRRA: true});
      }
              
      this.inforraDescrraFormControl.updateValueAndValidity;
      this.inforraDescrraFormControl.markAsTouched();
    }else{
      this.inforraDescrraFormControl.setErrors(erros);
    }
    this.validaValorMeses();
    
  }
}
