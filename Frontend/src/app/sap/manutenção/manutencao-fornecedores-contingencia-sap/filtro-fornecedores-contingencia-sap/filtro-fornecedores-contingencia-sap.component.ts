import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { FiltroFornecedoresContingenciaSapService } from '../services/filtro-fornecedores-contingencia-sap.service';

@Component({
  selector: 'filtro-fornecedores-contingencia-sap',
  templateUrl: './filtro-fornecedores-contingencia-sap.component.html',
  styleUrls: ['./filtro-fornecedores-contingencia-sap.component.scss']
})
export class FiltroFornecedoresContingenciaSapComponent implements OnInit {

  constructor(private service: FiltroFornecedoresContingenciaSapService) { }

  form: FormGroup;
  ngOnInit() {
    this.form = this.service.inicializarForm();
  }

  buscar() {
    this.service.filtrar();
  }
}
