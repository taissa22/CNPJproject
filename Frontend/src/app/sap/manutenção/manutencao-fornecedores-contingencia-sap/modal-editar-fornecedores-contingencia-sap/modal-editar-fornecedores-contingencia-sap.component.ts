import { Component, OnInit } from '@angular/core';
import { CrudFornecedoresContingenciaSapService } from '../services/crud-fornecedores-contingencia-sap.service';
import { FormGroup } from '@angular/forms';
import { NgbDateAdapter, NgbDateNativeAdapter, NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import { NgbDateCustomParserFormatter } from '@shared/dateformat';
import { BsModalRef } from 'ngx-bootstrap';
import { FornecedoresContingenciaSapService } from '../services/fornecedores-contingencia-sap.service';

@Component({
  selector: 'app-modal-editar-fornecedores-contingencia-sap',
  templateUrl: './modal-editar-fornecedores-contingencia-sap.component.html',
  styleUrls: ['./modal-editar-fornecedores-contingencia-sap.component.scss'],
  providers: [{ provide: NgbDateAdapter, useClass: NgbDateNativeAdapter }, { provide: NgbDateParserFormatter, useClass: NgbDateCustomParserFormatter }]

})
export class ModalEditarFornecedoresContingenciaSapComponent implements OnInit {

  constructor(private service: CrudFornecedoresContingenciaSapService,
    private fornecedorContignenciaService: FornecedoresContingenciaSapService,
    public bsModalRef: BsModalRef) { }

  form: FormGroup;
  ngOnInit() {
    this.form = this.service.inicializarForm();

    this.fornecedorContignenciaService.fornecedoresSelecionado.subscribe(item =>{
      item.statusFornecedor = '' + item.statusFornecedor
      item.dataVencimentoCartaFianca = new Date(item.dataVencimentoCartaFianca)
          this.form.patchValue(item)}
    )

    this.service.fecharModal.subscribe(item =>
    {
      if (item) {
        this.bsModalRef.hide();
      }
      })
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

  salvarAlteracao() {
    this.service.salvarFornecedor(this.form)
  }


}
