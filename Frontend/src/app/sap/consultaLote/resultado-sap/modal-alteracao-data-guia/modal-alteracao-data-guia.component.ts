import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { distinctUntilChanged } from 'rxjs/operators';
import { invalid } from '@angular/compiler/src/render3/view/util';
import { NgbDateAdapter, NgbDateNativeAdapter, NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import { NgbDateCustomParserFormatter } from '@shared/dateformat';
import { textosValidacaoFormulario } from '@shared/utils';
import { ModalAlteracaoDataGuiaService } from './service/modal-alteracao-data-guia.service';

@Component({
  selector: 'modal-alteracao-data-guia',
  templateUrl: './modal-alteracao-data-guia.component.html',
  styleUrls: ['./modal-alteracao-data-guia.component.scss'],
  providers: [{ provide: NgbDateAdapter, useClass: NgbDateNativeAdapter }, { provide: NgbDateParserFormatter, useClass: NgbDateCustomParserFormatter }]

})
export class ModalAlteracaoDataGuiaComponent implements OnInit {

  constructor(private service: ModalAlteracaoDataGuiaService,
    public bsModalRef: BsModalRef) { }

  form: FormGroup
  ngOnInit() {
    this.form = this.service.inicializarForm();
    this.form.get('opcaoSelecionada').valueChanges.pipe(distinctUntilChanged())
      .subscribe(item => {
        if (item === "2") {
          this.form.get('dataGuia').enable();
          this.form.get('dataGuia').setValidators(Validators.required);
          this.form.get('dataGuia').updateValueAndValidity()
        } else {
          this.form.get('dataGuia').disable();
          this.form.get('dataGuia').reset();
          this.form.get('dataGuia').clearValidators();
          this.form.get('dataGuia').updateValueAndValidity();
        }
      });

    this.service.fechaModal.subscribe(item => {
      if (item) {
        this.bsModalRef.hide();
      }
    }
    );
  }

  validarData(dataGuia) {
    if (typeof (dataGuia.value) === 'string' && dataGuia.value != null) {
     dataGuia.setErrors({ invalid: true });
    }
    if (typeof (dataGuia.value) === 'object' && dataGuia.value != null) {
      if (dataGuia.value.getYear() < 0) {
        dataGuia.setErrors({ invalid: true });
      } else {
        dataGuia.setErrors(null);
      }
    }
  }

  validacaoTextos(
   nomeControl: string,
    nomeCampo: string, campoFeminino: boolean) {
    if (
      (this.form.invalid &&
      this.form.get(nomeControl).hasError('required')
        && this.clickou)
      ||
      (this.form.invalid &&
        this.form.get(nomeControl).hasError('required')
        && this.form.get(nomeControl).touched)
    ) {
      if (!campoFeminino) {
        return `O ${nomeCampo} é obrigatório!`;
      } else {
        return `A ${nomeCampo} é obrigatória!`;
      }
    }
  }

  clickou = false
  confirmarFormulario() {
    this.clickou = true;
    this.service.salvar(this.form);
  }
}
