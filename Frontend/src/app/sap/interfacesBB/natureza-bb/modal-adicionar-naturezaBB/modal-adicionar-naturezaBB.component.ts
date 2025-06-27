import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap';
import { CrudTribunalBbService } from '../../tribunaisBB/services/crud-tribunal-bb.service';
import { OrgaoBBService } from '../../orgaoBB/services/orgao-bb.service';
import { FormGroup } from '@angular/forms';
import { Subscription } from 'rxjs';
import { instanciaDesignada } from '../../tribunaisBB/tribunalBB.constant';
import { distinctUntilChanged } from 'rxjs/operators';
import { textosValidacaoFormulario } from '@shared/utils';
import { CrudNaturezaBBService } from '../services/crud-naturezaBB.service';

@Component({
  selector: 'app-modal-adicionar-naturezaBB',
  templateUrl: './modal-adicionar-naturezaBB.component.html',
  styleUrls: ['./modal-adicionar-naturezaBB.component.scss']
})
export class ModalAdicionarNaturezaBBComponent implements OnInit {


  titulo =  'Incluir';
  constructor(public bsModalRef: BsModalRef, private service: CrudNaturezaBBService) { }

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
