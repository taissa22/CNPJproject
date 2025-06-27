import { Component, OnInit, OnDestroy } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap';
import { ComarcaBBCrudService } from '../../comarcaBB/service/comarcaBBCrud.service';
import { FormGroup } from '@angular/forms';
import { distinctUntilChanged } from 'rxjs/operators';
import { textosValidacaoFormulario } from '@shared/utils';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { OrgaoBBService } from '../services/orgao-bb.service';

@Component({
  selector: 'app-modal-adicionar-orgao-bb',
  templateUrl: './modal-adicionar-orgao-bb.component.html',
  styleUrls: ['./modal-adicionar-orgao-bb.component.scss']
})
export class ModalAdicionarOrgaoBBComponent implements OnInit, OnDestroy {

  titulo = 'Incluir';
  constructor(public bsModalRef: BsModalRef, private service: OrgaoBBService,
    private orgaoService: OrgaoBBService) { }

  registerForm: FormGroup;
  subscription: Subscription;
  tribunal;
  comarca;


  ngOnInit() {

    this.subscription = this.orgaoService.comboValueComarcaSubject.subscribe(
      comarca => this.comarca = comarca
    );
    this.subscription = this.orgaoService.comboValueTribunalSubject.subscribe(
      tribunal => this.tribunal = tribunal
    );

    this.registerForm = this.service.inicializarForm();
    this.service.fecharModal.subscribe(item => {
      if (item) {
        this.bsModalRef.hide();
      }
    });


    if (this.service.orgaoSelecionado) {
      this.titulo = 'Editar';
      this.service.modoSalvar = 'Editar'
      this.registerForm.patchValue(this.service.orgaoSelecionado);
    } else {
      this.titulo = 'Incluir';
      this.service.modoSalvar = 'Cadastrar';
    }

  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  validacaoTextos(nomeControl: string, nomeCampo: string, isFeminino: boolean) {
    return textosValidacaoFormulario(this.registerForm, nomeControl, nomeCampo, isFeminino);
  }

  salvarAlteracao() {
    this.service.salvarOrgaoBB(this.registerForm);
  }

  setComboBox(evento: any, nomeCampo: any) {
    this.registerForm.get(nomeCampo).setValue(evento)

  }

  getComboBox(nomeCampo: string) {

    return this.registerForm.get(nomeCampo).value;

  }

}
