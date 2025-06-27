import { Component, OnInit } from '@angular/core';
import { Validators, FormGroup, FormBuilder } from '@angular/forms';
import { BehaviorSubject } from 'rxjs';
import { BsModalRef } from 'ngx-bootstrap';
import { HelperAngular } from '@shared/helpers/helper-angular';
import { ManutencaoFormasPagamentoService } from '../services/manutencao-formas-pagamento.service';
import { FormasPagamentoService } from 'src/app/core/services/sap/formas-pagamento.service';
import swal from 'sweetalert2';
import { distinctUntilChanged, take } from 'rxjs/operators';
import {  textosValidacaoFormulario } from '@shared/utils';
import { FormaPagamentoCrudService } from './../services/FormaPagamentoCrudService.service';
import { Subscription } from 'rxjs/internal/Subscription';

@Component({
  selector: 'app-modal-formas-pagamento',
  templateUrl: './modal-formas-pagamento.component.html',
  styleUrls: ['./modal-formas-pagamento.component.scss']
})
export class ModalFormasPagamentoComponent implements OnInit {

  constructor(public bsModalRef: BsModalRef,
    private service: FormaPagamentoCrudService
    ) { }

  // titulo = 'Incluir Forma de Pagamento';
  // registerForm: FormGroup;
  // modoSalvar = 'Cadastrar';
  // formaPagamento;
  // isTrue;

  // public fecharModal = new BehaviorSubject<boolean>(false);

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
