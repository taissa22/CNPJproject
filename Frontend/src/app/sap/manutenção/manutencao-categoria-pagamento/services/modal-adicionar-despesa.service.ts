import { Injectable } from '@angular/core';
import { FormGroup, Validators, FormControl, FormBuilder } from '@angular/forms';
import { BehaviorSubject } from 'rxjs';
import { TipoProcessoEnum } from '@shared/enums/tipo-processoEnum.enum';

@Injectable({
  providedIn: 'root'
})
export class ModalAdicionarDespesaService {

  constructor() { }

  txtForncedorObrigatorio = new BehaviorSubject<string>('');
  temValorVazio = new BehaviorSubject<boolean>(true);


  /**
   * Faz toda a validação do formulário da tela de modal-adicionar-despesa.
   *
   * @param registerForm: o formulário
   * @param tipoProcesso: tipo processo atual para validação
   * @param isEnviaSapTrue: a validação da tela é toda feita a partir do checklist
   * indicaEnvioSAP que verifica se é para enviar ao sap, esse campo
   * é um booleando dizendo true ou false
   */
  verificarForm(registerForm: FormGroup, tipoProcesso, isEnviaSapTrue) {
    const cdMaterial = registerForm.get('codigoMaterial');
    const fornecedorPermitido = registerForm.get('fornecedoresPermitidos');

    switch (tipoProcesso) {
      case TipoProcessoEnum.civelConsumidor: {
        if (isEnviaSapTrue) {

          cdMaterial.enable();
          cdMaterial.setValidators([Validators.required,
          Validators.maxLength(18)]);
          cdMaterial.updateValueAndValidity();
          fornecedorPermitido.setValidators(this.validarObrigatoriedadeCombo);
          fornecedorPermitido.updateValueAndValidity();
          this.txtForncedorObrigatorio.next('Selecione um fornecedor');
          this.temValorVazio.next(false);


        } else {
          cdMaterial.setValue('');
          cdMaterial.disable();
          cdMaterial.updateValueAndValidity();

          fornecedorPermitido.setValidators(null);
          fornecedorPermitido.updateValueAndValidity();
          this.txtForncedorObrigatorio.next('');
          this.temValorVazio.next(true);
        } break;
      }
      case TipoProcessoEnum.civelEstrategico: {
        if (isEnviaSapTrue) {
          cdMaterial.enable();
          cdMaterial.setValidators([Validators.required,
          Validators.maxLength(18),]);
          cdMaterial.updateValueAndValidity();
        } else {
          cdMaterial.setValue('');
          cdMaterial.disable();

          cdMaterial.updateValueAndValidity();
        } break;
      }
      case TipoProcessoEnum.juizadoEspecial: {
        if (isEnviaSapTrue) {
          cdMaterial.enable();
          cdMaterial.setValidators([Validators.required,
          Validators.maxLength(18)]);
          cdMaterial.updateValueAndValidity();
          fornecedorPermitido.setValidators(this.validarObrigatoriedadeCombo);
          fornecedorPermitido.updateValueAndValidity();
          this.txtForncedorObrigatorio.next('Selecione um fornecedor');
          this.temValorVazio.next(false);


        } else {
          cdMaterial.setValue('');
          cdMaterial.disable();

          cdMaterial.updateValueAndValidity();
          fornecedorPermitido.setValidators(null);
          fornecedorPermitido.updateValueAndValidity();
          this.txtForncedorObrigatorio.next('');
          this.temValorVazio.next(true);
        } break;

      }
      case TipoProcessoEnum.trabalhista: {
        if (isEnviaSapTrue) {
          cdMaterial.enable();
          cdMaterial.setValidators([Validators.required,
          Validators.maxLength(18),]);
          cdMaterial.updateValueAndValidity();
        } else {
          cdMaterial.setValue('');
          cdMaterial.disable();

          cdMaterial.updateValueAndValidity();
        } break;
      } case TipoProcessoEnum.PEX: {
        if (isEnviaSapTrue) {
          cdMaterial.enable();
          cdMaterial.setValidators([Validators.required,
          Validators.maxLength(18),]);
          cdMaterial.updateValueAndValidity();
        } else {
          cdMaterial.setValue('');
          cdMaterial.disable();

          cdMaterial.updateValueAndValidity();
        } break;
      }

    }
  }

  /**
    * Verifica se a combo está ou não preenchida em caso de obrigatóriedade,
    * ou seja, se o valor da combo é maior que -1.
    * @param c: é o formControl atual que será verificado.
    * (Esse valor não precisa ser colocado a mão,
    *  simplesmente adicionar a validação ao formulário como validator)
    */
  validarObrigatoriedadeCombo(c: FormControl) {
    return c.value ? null : { validarObrigatoriedadeCombo: false };
  }

}
