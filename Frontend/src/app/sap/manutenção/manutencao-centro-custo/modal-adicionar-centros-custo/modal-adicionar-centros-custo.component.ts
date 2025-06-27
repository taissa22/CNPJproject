import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap';
import { FormGroup } from '@angular/forms';
import { CentroCustoCrudService } from '../service/centro-custo-crud.service';
import { textosValidacaoFormulario } from '@shared/utils';
import { distinctUntilChanged } from 'rxjs/operators';


@Component({
  selector: 'app-modal-adicionar-centros-custo',
  templateUrl: './modal-adicionar-centros-custo.component.html',
  styleUrls: ['./modal-adicionar-centros-custo.component.scss']
})
export class ModalAdicionarCentrosCustoComponent implements OnInit {

  titulo = 'Incluir'
  constructor(public bsModalRef: BsModalRef, private service: CentroCustoCrudService) { }

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
