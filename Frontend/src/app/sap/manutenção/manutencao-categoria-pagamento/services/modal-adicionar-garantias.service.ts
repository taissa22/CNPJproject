import { Injectable } from '@angular/core';
import { FormControl, FormGroup, Validators, Form } from '@angular/forms';
import { BehaviorSubject } from 'rxjs';
import { TipoProcessoEnum } from '@shared/enums/tipo-processoEnum.enum';

@Injectable({
  providedIn: 'root'
})
export class ModalAdicionarGarantiasService {

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
  verificarForm(registerForm: FormGroup, tipoProcesso) {
    const garantiaSelect = registerForm.get('codigoClasseGarantia');
    const isEnviaSap = registerForm.get('indicaEnvioSAP');
    const materialSap = registerForm.get('codigoMaterial');
    const grupocorrecaoSele = registerForm.get('codigoGrupoCorrecao');

    switch (tipoProcesso) {

      case TipoProcessoEnum.civelConsumidor: {

        if (garantiaSelect.value == 1) {
          materialSap.enable();
          isEnviaSap.enable();
          grupocorrecaoSele.enable();


          if (isEnviaSap.value) {
            materialSap.setValidators([Validators.required, Validators.maxLength(18)]);
            materialSap.updateValueAndValidity();
          } else {
            materialSap.disable();
            materialSap.setValue("");
            materialSap.updateValueAndValidity();
          }


        } else if (garantiaSelect.value == 3) {
          grupocorrecaoSele.enable();
        } else {
          materialSap.setValue('');
          isEnviaSap.setValue(false);
          materialSap.disable();
          isEnviaSap.disable();
          grupocorrecaoSele.setValue(null);
          grupocorrecaoSele.disable();
        }
        break;

      }
      case TipoProcessoEnum.civelEstrategico: {
        if (garantiaSelect.value == 1) {
          materialSap.enable();
          isEnviaSap.enable();

          if (isEnviaSap.value) {
            materialSap.setValidators([Validators.required, ,
            Validators.maxLength(18)]);
            materialSap.updateValueAndValidity();
          } else {
           materialSap.disable();
            materialSap.setValue("");
            materialSap.updateValueAndValidity();
          }
        } else {
          materialSap.setValue('');
          isEnviaSap.setValue(false);
          materialSap.disable();
          isEnviaSap.disable();

        }
        break;
      }
      case TipoProcessoEnum.juizadoEspecial: {
        if (garantiaSelect.value == 1) {
          materialSap.enable();
          isEnviaSap.enable();
          grupocorrecaoSele.enable();


          if (isEnviaSap.value) {
            materialSap.setValidators([Validators.required,,
            Validators.maxLength(18)]);
            materialSap.updateValueAndValidity();
          } else {
           materialSap.disable();
            materialSap.setValue("");
            materialSap.updateValueAndValidity();
          }


        } else if (garantiaSelect.value == 3) {
          grupocorrecaoSele.enable();
          materialSap.setValue('');
          isEnviaSap.setValue(false);
          materialSap.disable();
          isEnviaSap.disable();
        } else {
          materialSap.setValue('');
          isEnviaSap.setValue(false);
          materialSap.disable();
          isEnviaSap.disable();
          grupocorrecaoSele.setValue(null);
          grupocorrecaoSele.disable();
        }
        break;
      }
      case TipoProcessoEnum.trabalhista: {
        if (garantiaSelect.value == 1) {
          materialSap.enable();
          isEnviaSap.enable();
          grupocorrecaoSele.enable();


          if (isEnviaSap.value) {
            materialSap.setValidators([Validators.required,  ,
            Validators.maxLength(18)]);
            materialSap.updateValueAndValidity();
          } else {
           materialSap.disable();
            materialSap.setValue("");
            materialSap.updateValueAndValidity();
          }


        } else if (garantiaSelect.value == 3) {
          grupocorrecaoSele.enable();
        } else {
          materialSap.setValue('');
          isEnviaSap.setValue(false);
          materialSap.disable();
          grupocorrecaoSele.setValue(null);

          isEnviaSap.disable();
          grupocorrecaoSele.disable();
        }
        break;

      }
      case TipoProcessoEnum.PEX: {
        if (garantiaSelect.value == 1) {
          materialSap.enable();
          isEnviaSap.enable();
          grupocorrecaoSele.enable();


          if (isEnviaSap.value) {
            materialSap.setValidators([Validators.required ,
            Validators.maxLength(18)]);
            materialSap.updateValueAndValidity();
          } else {
           materialSap.disable();
            materialSap.setValue("");
            materialSap.updateValueAndValidity();
          }


        } else if (garantiaSelect.value == 3) {
          grupocorrecaoSele.enable();
        } else {
          materialSap.setValue('');
          isEnviaSap.setValue(false);
          materialSap.disable();
          isEnviaSap.disable();
          grupocorrecaoSele.setValue(null);
          grupocorrecaoSele.disable();
        }
        break;
        }
    }
  }

  validarObrigatoriedadeCombo(c: FormControl){
    return c.value ? null : { validarObrigatoriedadeCombo: false };
  }


  setDisable(registerForm: FormGroup) {
    registerForm.get('codigoMaterial').disable();
    registerForm.get('indicaEnvioSAP').disable();
    registerForm.get('codigoGrupoCorrecao').disable();
  }

  setObrigatoriedadeClasseGarantia(registerForm: FormGroup) {
    registerForm.get('codigoClasseGarantia').setValidators(this.validarObrigatoriedadeCombo);
    registerForm.get('codigoClasseGarantia').updateValueAndValidity();

    return registerForm;
  }

}
