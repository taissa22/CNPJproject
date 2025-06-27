import { Component, OnInit } from '@angular/core';
import { textosValidacaoFormulario } from '@shared/utils';
import { BsModalRef } from 'ngx-bootstrap';
import { ComarcaBBCrudService } from '../../comarcaBB/service/comarcaBBCrud.service';
import { OrgaoBBService } from '../../orgaoBB/services/orgao-bb.service';
import { FormGroup } from '@angular/forms';
import { Subscription } from 'rxjs';
import { distinctUntilChanged } from 'rxjs/operators';
import { CrudTribunalBbService } from '../services/crud-tribunal-bb.service';
import { instanciaDesignada } from '../tribunalBB.constant';

@Component({
  selector: 'app-modal-tribunais-bb',
  templateUrl: './modal-tribunais-bb.component.html',
  styleUrls: ['./modal-tribunais-bb.component.scss']
})
export class ModalTribunaisBbComponent implements OnInit {


  titulo =  'Incluir';
  constructor(public bsModalRef: BsModalRef, private service: CrudTribunalBbService,
              private orgaoService: OrgaoBBService) { }

  registerForm: FormGroup;
  subscription: Subscription;
  instanciaDesignada;
  ngOnInit() {

    this.instanciaDesignada = instanciaDesignada;

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

  getValorCombo() {
    return this.registerForm.get('indicadorInstancia').value;
  }

  setValorCombo(event) {
    this.registerForm.get('indicadorInstancia').setValue(event);
  }

}
