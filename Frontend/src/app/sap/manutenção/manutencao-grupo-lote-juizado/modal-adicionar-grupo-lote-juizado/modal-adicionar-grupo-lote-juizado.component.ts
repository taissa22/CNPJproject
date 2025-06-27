import { CrudGrupoLoteJuizadoService } from './../services/crud-grupo-lote-juizado.service';
import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap';
import { FormGroup } from '@angular/forms';
import { Subscription } from 'rxjs';
import { distinctUntilChanged } from 'rxjs/operators';
import { textosValidacaoFormulario } from '@shared/utils';

@Component({
  selector: 'app-modal-adicionar-grupo-lote-juizado',
  templateUrl: './modal-adicionar-grupo-lote-juizado.component.html',
  styleUrls: ['./modal-adicionar-grupo-lote-juizado.component.scss']
})
export class ModalAdicionarGrupoLoteJuizadoComponent implements OnInit {


  titulo =  'Incluir';
  constructor(public bsModalRef: BsModalRef, private service: CrudGrupoLoteJuizadoService) { }

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
