import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { ResultadoNegociacaoService } from '@manutencao/services/resultado-negociacao.service';
import { StaticInjector } from '@manutencao/static-injector';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DialogService } from '@shared/services/dialog.service';

@Component({
  selector: 'app-resultado-negociacao-modal',
  templateUrl: './resultado-negociacao-modal.component.html',
  styleUrls: ['./resultado-negociacao-modal.component.scss']
})
export class ResultadoNegociacaoModalComponent implements OnInit {

  constructor(
    private modal: NgbActiveModal,
    private dialogService: DialogService,
    private service: ResultadoNegociacaoService,
    private formBuilder: FormBuilder,
  ) { }

  ngOnInit() {
  }

  operacao = '';
  codResultadoNegociacao = 0;
  dscResultadoNegociacaoFormControl : FormControl = new FormControl('',[Validators.required]);
  indNegociacaoFormControl : FormControl = new FormControl(false);
  indPosSentencaFormControl : FormControl = new FormControl(false);
  codTipoResultadoFormControl : FormControl = new FormControl('',[Validators.required]);
  indAtivoFormControl : FormControl = new FormControl(true);

  formGroup: FormGroup = this.formBuilder.group({
    codResultadoNegociacao: this.codResultadoNegociacao,
    dscResultadoNegociacao: this.dscResultadoNegociacaoFormControl,
    indNegociacao: this.indNegociacaoFormControl,
    indPosSentenca: this.indPosSentencaFormControl,
    codTipoResultado: this.codTipoResultadoFormControl,
    indAtivo: this.indAtivoFormControl
  });

  public static exibeModalDeIncluir(): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(ResultadoNegociacaoModalComponent, { windowClass: 'resultado-negociacao-modal', centered: true, size: 'lg', backdrop: 'static' });
      modalRef.componentInstance.operacao = "Inclusão";
      return modalRef.result;
  }

  public static exibeModalDeAlterar(codResultadoNegociacao: number): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(ResultadoNegociacaoModalComponent, { windowClass: 'resultado-negociacao-modal', centered: true, size: 'lg', backdrop: 'static' });
      modalRef.componentInstance.operacao = "Alteração";
      modalRef.componentInstance.codResultadoNegociacao = codResultadoNegociacao;
      modalRef.componentInstance.obterResultadoNegociacaoAsync();
    return modalRef.result;
  }

  desabilitaTooltip(formControl: FormControl) {
    return formControl.untouched || formControl.valid;
  }

  close(): void {
    this.modal.close(false);
  }

  async save(){
    this.formGroup.get('codResultadoNegociacao').setValue(this.codResultadoNegociacao);
    console.log(this.formGroup.value)
    
    if(!this.dscResultadoNegociacaoFormControl.value){
      await this.dialogService.err(`${this.operacao} Resultado de Negociação`, 'Campo descrição deve ser preenchido.' );
      return this.formGroup.markAllAsTouched();
    }

    if(!this.indNegociacaoFormControl.value && !this.indPosSentencaFormControl.value){
      return this.dialogService.err(`${this.operacao} Resultado de Negociação`, 'Deve ser selecionado ao menos 1 tipo de negociação.' );
    }
    
    if(!this.codTipoResultadoFormControl.value){
      return this.dialogService.err(`${this.operacao} Resultado de Negociação`, 'Deve ser selecionado um tipo resultado.' );
    }
    
    if(this.codResultadoNegociacao == 0){
      try{
        await this.service.salvarResultadoNegociacaoAsync(this.formGroup.value);      
        await this.dialogService.alert('Cadastrar Resultado de Negociação.', "Resultado de Negociação incluído com sucesso.");
        this.modal.close(true);
      } catch (error) {      
        return this.dialogService.err('Cadastrar Resultado de Negociação', error );
      }
    }else{
      try{
        await this.service.editarResultadoNegociacaoAsync(this.formGroup.value);      
        await this.dialogService.alert('Editar Resultado de Negociação.', "Resultado de Negociação alterado com sucesso.");
        this.modal.close(true);
      } catch (error) {      
        return this.dialogService.err('Editar Resultado de Negociação', error );
      }
    }
  }

  async obterResultadoNegociacaoAsync(){
    try {
      const resultadoNegociacaoRetorno = await this.service.obterResultadoNegociacaoAsync(this.codResultadoNegociacao);
      this.dscResultadoNegociacaoFormControl.setValue(resultadoNegociacaoRetorno[0].dscResultadoNegociacao);
      this.indNegociacaoFormControl.setValue(resultadoNegociacaoRetorno[0].indNegociacao == 'S');
      this.indPosSentencaFormControl.setValue(resultadoNegociacaoRetorno[0].indPosSentenca == 'S');
      this.codTipoResultadoFormControl.setValue(resultadoNegociacaoRetorno[0].codTipoResultado);
      this.indAtivoFormControl.setValue(resultadoNegociacaoRetorno[0].indAtivo == 'S');
      console.log(resultadoNegociacaoRetorno[0])
    } catch (error) {
      await this.dialogService.err(
        "Erro ao buscar.",
        "Não foi possível realizar a busca da Resultado de Negociação."
      );
    }
  }

  validaCheck(opt: number) {
    if(opt == 1)
      this.indNegociacaoFormControl.setValue(!this.indNegociacaoFormControl.value)
    if(opt == 2)
      this.indPosSentencaFormControl.setValue(!this.indPosSentencaFormControl.value)

    if(!this.indNegociacaoFormControl.value && !this.indPosSentencaFormControl.value){
      this.indNegociacaoFormControl.setValidators(this.customRequiredValidator());
      this.indPosSentencaFormControl.setValidators(this.customRequiredValidator());      
    }else{
      this.indNegociacaoFormControl.clearValidators();
      this.indPosSentencaFormControl.clearValidators();
    }
    this.indNegociacaoFormControl.updateValueAndValidity();
    this.indPosSentencaFormControl.updateValueAndValidity();

  }

  customRequiredValidator(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
      const value = control.value;
      if (!value || value === 'false' || value === 0) {
        return { 'required': true };
      }
      return null;
    };
  }

}
