import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap';
import { ComarcaBBCrudService } from '../../comarcaBB/service/comarcaBBCrud.service';
import { FormGroup } from '@angular/forms';
import { distinctUntilChanged } from 'rxjs/operators';
import { textosValidacaoFormulario } from '@shared/utils';
import { CrudModalidadeBBService } from '../services/crudModalidadeBB.service';

@Component({
  selector: 'app-modal-adicionar-modalidade-bb',
  templateUrl: './modal-adicionar-modalidade-bb.component.html',
  styleUrls: ['./modal-adicionar-modalidade-bb.component.scss']
})
export class ModalAdicionarModalidadeBbComponent implements OnInit {

  titulo = 'Incluir';
  constructor(public bsModalRef: BsModalRef, private service: CrudModalidadeBBService) { }

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

