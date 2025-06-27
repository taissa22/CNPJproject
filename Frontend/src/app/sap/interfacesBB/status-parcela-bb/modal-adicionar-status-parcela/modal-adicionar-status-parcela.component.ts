import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap';
import { ComarcaBBCrudService } from '../../comarcaBB/service/comarcaBBCrud.service';
import { FormGroup } from '@angular/forms';
import { distinctUntilChanged } from 'rxjs/operators';
import { textosValidacaoFormulario } from '@shared/utils';
import { CrudStatusParcelaService } from '../services/crud-status-parcela.service';

@Component({
  selector: 'app-modal-adicionar-status-parcela',
  templateUrl: './modal-adicionar-status-parcela.component.html',
  styleUrls: ['./modal-adicionar-status-parcela.component.scss']
})
export class ModalAdicionarStatusParcelaComponent implements OnInit {

  titulo = 'Incluir';
  constructor(public bsModalRef: BsModalRef, private service: CrudStatusParcelaService) { }

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
    this.registerForm.get('codigoBB').valueChanges.pipe(distinctUntilChanged()).subscribe(item => {
      
      item = item + '';
      item = item.replace(/^0|^((?!\d).)/g, '')
      item = item.replace(/[^0-9]*/g, '');
      this.registerForm.get('codigoBB').setValue(item)
      this.registerForm.get('codigoBB').updateValueAndValidity()

    })
  }

  validacaoTextos(nomeControl: string, nomeCampo: string, isFeminino: boolean) {
    return textosValidacaoFormulario(this.registerForm, nomeControl, nomeCampo, isFeminino);
  }

  salvarAlteracao() {
    this.service.salvar(this.registerForm);
  }
}
