import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap';
import { CentroCustoCrudService } from 'src/app/sap/manutenção/manutencao-centro-custo/service/centro-custo-crud.service';
import { FormGroup } from '@angular/forms';
import { distinctUntilChanged } from 'rxjs/operators';
import { textosValidacaoFormulario } from '@shared/utils';
import { ComarcaBBCrudService } from '../service/comarcaBBCrud.service';

@Component({
  selector: 'app-modal-adicionar-comarcaBB',
  templateUrl: './modal-adicionar-comarcaBB.component.html',
  styleUrls: ['./modal-adicionar-comarcaBB.component.scss']
})
export class ModalAdicionarComarcaBBComponent implements OnInit {

  titulo = 'Incluir';
  constructor(public bsModalRef: BsModalRef, private service: ComarcaBBCrudService) { }

  registerForm: FormGroup;
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
