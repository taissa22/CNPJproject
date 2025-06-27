import { Component, OnInit } from '@angular/core';
import { CriteriosGeraisFiltroService } from '../../services/CriteriosGeraisFiltro.service';
import { FormGroup } from '@angular/forms';
import { take, distinctUntilChanged, debounceTime } from 'rxjs/operators';

@Component({
  selector: 'criterioGerais-filtros',
  templateUrl: './criterioGerais.component.html',
  styleUrls: ['./criterioGerais.component.scss']
})
export class CriterioGeraisComponent implements OnInit {


  constructor(public service: CriteriosGeraisFiltroService) { }
  criteriosForm: FormGroup;

  rangeValido1 = true;
  rangeValido2 = true;

  ngOnInit() {
   
    //  this.atualizarForm()
    this.service.limparCriterios.subscribe(_ => this.atualizarForm())

  //  this.verificarAlteracaoForm()

  }

  atualizarForm() {
    // this.service.periodoPendenciaInicial = null;
    // this.service.periodoPendenciaFinal = null;
      this.criteriosForm = this.service.inicializarForm;
      this.verificarAlteracaoForm()
    // this.service.isRangeValido = true
    // this.service.somarValores(t)
    // console.log('rodou aqui')
    // console.log("🚀 ~ file: criterioGerais.component.ts ~ line 36 ~ CriterioGeraisComponent ~ atualizarForm ~ this.criteriosForm", this.criteriosForm)
  }

  verificarAlteracaoForm(){
    this.criteriosForm.valueChanges.pipe(distinctUntilChanged((prev, curr) => prev === curr)).subscribe(item => {
      this.service.somarValores(item);
      this.service.adicionarJson(item);
      this.service.isRangeValido =
        this.rangeValido1 && this.rangeValido2 ? true : false;

        // console.log(item)
    });
  }

}
