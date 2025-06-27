import { Component, OnInit, OnChanges, OnDestroy } from '@angular/core';
import { NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import { NgbDateCustomParserFormatter } from '@shared/dateformat';
import { RelatorioService } from 'src/app/core/services/relatorio.service';
import { CriteriosModel } from 'src/app/core/models/criterios-model';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';


@Component({
  selector: 'app-criterios',
  templateUrl: './criterios.component.html',
  styleUrls: ['./criterios.component.scss'],
  providers: [{ provide: NgbDateParserFormatter, useClass: NgbDateCustomParserFormatter }]
})
export class CriteriosComponent implements OnInit, OnDestroy {

  // tslint:disable-next-line: variable-name
  // tslint:disable-next-line: no-inferrable-types
  formCriterios: FormGroup;
  constructor(
    private relatorioService: RelatorioService,
    private formBuilder: FormBuilder
  ) { }
  ngOnInit() {
    this.formCriterios = this.formBuilder.group(
      {
        statusProcesso: [this.relatorioService.criterios.statusProcesso],
        statusContabilProcesso: [this.relatorioService.criterios.statusContabilProcesso],
        inicioPeriodoAudiencia: [this.relatorioService.criterios.inicioPeriodoAudiencia],
        fimPeriodoAudiencia: [this.relatorioService.criterios.fimPeriodoAudiencia],
        inicioDataCadastro: [this.relatorioService.criterios.inicioDataCadastro],
        fimDataCadastro: [this.relatorioService.criterios.fimDataCadastro],
        inicioDataDistribuicao: [this.relatorioService.criterios.inicioDataDistribuicao],
        fimDataDistribuicao: [this.relatorioService.criterios.fimDataDistribuicao],
        inicioDataFinalizacaoContabil: [this.relatorioService.criterios.inicioDataFinalizacaoContabil],
        fimDataFinalizacaoContabil: [this.relatorioService.criterios.fimDataFinalizacaoContabil],
        inicioDataFinalizacaoEscritorio: [this.relatorioService.criterios.inicioDataFinalizacaoEscritorio],
        fimDataFinalizacaoEscritorio: [this.relatorioService.criterios.fimDataFinalizacaoEscritorio]
      }
    );
    this.atualizaCount();

  }
  atualizaCount() {
    this.formCriterios.valueChanges.subscribe(
      (selectedValue) => {
        this.relatorioService.criterios = selectedValue;
        const teste = Object.values(selectedValue)
          .filter(reducer => {
            if (reducer !== null) {
              return reducer;
            }
          });
        this.relatorioService.atualizaCountCriterios(teste.length);

      }
    );
  }
  ngOnDestroy(): void {


  }
}
