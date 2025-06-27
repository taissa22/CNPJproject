import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap';
import { FormGroup } from '@angular/forms';
import { EmpresasCrudService } from '../services/empresas-crud.service';
import { distinctUntilChanged } from 'rxjs/operators';
import { Subscription } from 'rxjs';
import { textosValidacaoFormulario } from '@shared/utils';


@Component({
  selector: 'app-modal-adicionar-empresas-sap',
  templateUrl: './modal-adicionar-empresas-sap.component.html',
  styleUrls: ['./modal-adicionar-empresas-sap.component.scss']
})
export class ModalAdicionarEmpresasSapComponent implements OnInit {

  constructor(public bsModalRef: BsModalRef,
    private service: EmpresasCrudService
    ) { }

    titulo =  'Incluir';
    registerForm: FormGroup;
    subscription: Subscription;
    ngOnInit() {


      this.registerForm = this.service.inicializarForm();

      this.service.fecharModal.subscribe(item => {
        if (item) {
          this.bsModalRef.hide();
        }
      });

      this.service.valoresEdicaoFormulario.pipe(distinctUntilChanged()).subscribe(
        item => {
          if (item) {
            this.titulo = 'Editar';
            this.registerForm.patchValue(item);
          } else {
            this.titulo = 'Incluir';
          }
        }
      );
    }



    validacaoTextos(nomeControl: string, nomeCampo: string, isFeminino: boolean) {
      return textosValidacaoFormulario(this.registerForm, nomeControl, nomeCampo, isFeminino);
    }

    salvarAlteracao() {
      this.service.salvar(this.registerForm);
    }

}
