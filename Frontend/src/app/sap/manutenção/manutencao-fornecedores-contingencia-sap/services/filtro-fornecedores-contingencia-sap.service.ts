import { Injectable } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { take } from 'rxjs/operators';
import { BehaviorSubject } from 'rxjs';
import { FornecedoresContingenciaSap } from '@shared/interfaces/fornecedores-contingencia-sap';
import { FornecedoresContingenciaSAPService } from 'src/app/core/services/sap/FornecedoresContingenciaSap.service';
import { FornecedoresContingenciaSapService } from './fornecedores-contingencia-sap.service';

@Injectable({
  providedIn: 'root'
})
export class FiltroFornecedoresContingenciaSapService {

  constructor(private fb: FormBuilder,
  private fornecedoresContingenciaTsService: FornecedoresContingenciaSapService) { }

  get valoresFiltro() {
    let valores = Object.assign({}, this.form.value);
    valores.cnpj = valores.cnpj ? valores.cnpj.replace(/[^0-9]*/g, '') : null;
    return valores;
  }

  form: FormGroup;

  fornecedoresSubject = new BehaviorSubject<any>({});

  buscar = new BehaviorSubject<any>(false);
  inicializarForm() {
    this.form = this.fb.group({
      nome: null,
      codigo: null,
      cnpj: null,
      statusFornecedor: '3'
    });
    return this.form;
  }

  filtrar() {
    if (this.form.valid) {
      this.buscar.next(true);
      this.fornecedoresContingenciaTsService.totalSubject.next(0);
      this.fornecedoresContingenciaTsService.consultarPorFiltros(
        this.valoresFiltro
      ).pipe(take(1))
        .subscribe(itens => {
          this.fornecedoresSubject.next(itens);
        });
    }
  }
}
