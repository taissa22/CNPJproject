import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { FormGroup, FormControl, Validators, Form } from '@angular/forms';
import { TipoProcessoEnum } from '@shared/enums/tipo-processoEnum.enum';
import { HelperAngular } from '@shared/helpers/helper-angular';
import { ModalAdicionarService } from './modal-adicionar.service';

@Injectable({
  providedIn: 'root'
})
export class ModalAdicionarPagamentoService {

  constructor(private helpAngular: HelperAngular,
    private modalAdicionarService: ModalAdicionarService) { }

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
    const fornecedorPermitido = registerForm.get('fornecedoresPermitidos');
    const influenciarCont = registerForm.get('influenciaContingenciaMedia');
    const justificativa = registerForm.get('justificativa');
    const pagamentoA = registerForm.get('pagamentoA');

    switch (tipoProcesso) {

      case TipoProcessoEnum.civelConsumidor: {
        pagamentoA.enable();
        if (garantiaSelect.value == 1 || !garantiaSelect.value || garantiaSelect.value == null) {
          materialSap.enable();
          isEnviaSap.enable();
          grupocorrecaoSele.enable();
         

          if (isEnviaSap.value) {
            materialSap.setValidators([Validators.required,
            Validators.maxLength(18)]);
            materialSap.updateValueAndValidity();
            fornecedorPermitido.setValidators(this.validarObrigatoriedadeCombo);
            fornecedorPermitido.updateValueAndValidity();
            this.txtForncedorObrigatorio.next('Selecione um fornecedor');
            this.temValorVazio.next(false);
          } else {

            materialSap.disable()
            materialSap.setValue("");
            materialSap.updateValueAndValidity();
            fornecedorPermitido.setValidators(null);
            fornecedorPermitido.updateValueAndValidity();
            this.txtForncedorObrigatorio.next('');
            this.temValorVazio.next(true);
          }


        } else {
          materialSap.setValue('');
          isEnviaSap.setValue(false);
          materialSap.disable();
          isEnviaSap.disable();
          grupocorrecaoSele.disable();
          fornecedorPermitido.setValidators(null);
          fornecedorPermitido.updateValueAndValidity();
          this.txtForncedorObrigatorio.next('');
          this.temValorVazio.next(true);
       
        }
        if (influenciarCont.value) {
          justificativa.setValue('');
          justificativa.disable();
        } else {
          justificativa.enable();
        }
        break;
      }
      case TipoProcessoEnum.civelEstrategico: {
        if (garantiaSelect.value == 1 || !garantiaSelect.value || garantiaSelect.value == null) {
          materialSap.enable();
          isEnviaSap.enable();
          grupocorrecaoSele.enable();
          pagamentoA.enable();

          if (isEnviaSap.value) {
            materialSap.setValidators([Validators.required,
            Validators.maxLength(18)]);
            materialSap.updateValueAndValidity();
          } else {
            materialSap.disable()
            materialSap.setValue(""); materialSap.updateValueAndValidity();
          }

        }
        else {
          materialSap.setValue('');
          isEnviaSap.setValue(false);
          materialSap.disable();
          isEnviaSap.disable();
          grupocorrecaoSele.disable();
          pagamentoA.disable();
        }
        break;
      }
      case TipoProcessoEnum.juizadoEspecial: {
        if (garantiaSelect.value == 1 || !garantiaSelect.value) {
          materialSap.enable();
          isEnviaSap.enable();
          grupocorrecaoSele.enable();

          if (isEnviaSap.value) {
            materialSap.setValidators([Validators.required, Validators.maxLength(18)
            ]);
            materialSap.updateValueAndValidity();
            fornecedorPermitido.setValidators(this.validarObrigatoriedadeCombo);
            fornecedorPermitido.updateValueAndValidity();
            this.txtForncedorObrigatorio.next('Selecione um fornecedor');
            this.temValorVazio.next(false);
          } else {
            materialSap.disable()
            materialSap.setValue(""); materialSap.updateValueAndValidity();
            fornecedorPermitido.setValidators(null);
            fornecedorPermitido.updateValueAndValidity();
            this.txtForncedorObrigatorio.next('');
            this.temValorVazio.next(true);
          }


        } else {
          materialSap.setValue('');
          isEnviaSap.setValue(false);
          materialSap.disable();
          isEnviaSap.disable();
          grupocorrecaoSele.disable();
          fornecedorPermitido.setValidators(null);
          fornecedorPermitido.updateValueAndValidity();
          this.txtForncedorObrigatorio.next('');
          this.temValorVazio.next(true);
        }
        if (influenciarCont.value) {
          justificativa.setValue('');
          justificativa.disable();
        } else {
          justificativa.enable();
        }
        break;

      }
      case TipoProcessoEnum.trabalhista: {
        if (garantiaSelect.value == 1 || !garantiaSelect.value || garantiaSelect.value == null) {
          materialSap.enable();
          isEnviaSap.enable();
          grupocorrecaoSele.enable();


          if (isEnviaSap.value) {
            materialSap.setValidators([Validators.required, Validators.maxLength(18)
            ]);
            materialSap.updateValueAndValidity();
          } else {
            materialSap.disable()
            materialSap.setValue(""); materialSap.updateValueAndValidity();
          }

        }
        else {
          materialSap.setValue('');
          isEnviaSap.setValue(false);
          materialSap.disable();
          isEnviaSap.disable();
          grupocorrecaoSele.disable();
        }
        break;
      }
      case TipoProcessoEnum.PEX: {
        if (garantiaSelect.value == 1 || !garantiaSelect.value || garantiaSelect.value == null) {
          materialSap.enable();
          isEnviaSap.enable();
          grupocorrecaoSele.enable();


          if (isEnviaSap.value) {
            materialSap.setValidators([Validators.required, Validators.maxLength(18)
            ]);
            materialSap.updateValueAndValidity();
          } else {
            materialSap.disable()
            materialSap.setValue(""); materialSap.updateValueAndValidity();
          }
        }
        else {
          materialSap.setValue('');
          isEnviaSap.setValue(false);
          materialSap.disable();
          isEnviaSap.disable();
          grupocorrecaoSele.disable();
        }

        if (justificativa.value){
          justificativa.setValidators(this.validarObrigatoriedadeCombo);
        }

        if (influenciarCont.value) { 
          justificativa.setValue('');
          justificativa.disable();
        } else {
          justificativa.enable();
        }
        break;

      }
    }
  }

  validarObrigatoriedadeCombo(c: FormControl) {
    return c.value ? null : { validarObrigatoriedadeCombo: false };
  }

  salvar(registerForm: FormGroup, tipoProcesso) {
    const justificativa = registerForm.get('justificativa').value;
    const influencia = registerForm.get('influenciaContingenciaMedia').value;


    if ((!justificativa && !influencia)
      && (tipoProcesso == TipoProcessoEnum.civelConsumidor
        || tipoProcesso == TipoProcessoEnum.juizadoEspecial || tipoProcesso == TipoProcessoEnum.PEX)) {

      //TODO colocar aqui a função para salvar
      this.helpAngular.MsgBox2(`Não foi informada nenhuma justificativa para que a categoria
    não influencie na contingência. Deseja prosseguir?`, 'Atenção!', 'warning', 'Sim', 'Não')
        .then(result => {
          if (result.value) {
            this.modalAdicionarService.salvarAlteracao(registerForm);
          }
        }
        )
    } else {
      this.modalAdicionarService.salvarAlteracao(registerForm);
    }


  }
}
