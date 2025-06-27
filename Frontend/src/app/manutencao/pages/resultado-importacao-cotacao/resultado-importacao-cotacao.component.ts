import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { JurPaginator } from '@shared/components/jur-paginator/jur-paginator.component';
import { JurTable } from '@shared/components/jur-table/jur-table.component';

import { DialogService } from '@shared/services/dialog.service';
import { AbstractControl, FormControl, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';

import { Cotacao } from '@manutencao/models/cotacao.model';

import { CotacaoServiceMock } from '@manutencao/services/cotacao.service.mock';
import { CotacaoService } from '@manutencao/services/cotacao.service';
import { IndiceService } from '@manutencao/services/indice.service';
import { Indice } from '@manutencao/models/indice';
import moment from 'moment';
import { CotacaoIndiceTrabalhista } from '@manutencao/models/cotacao-indice-trabalhista.model';
import { CotacaoIndiceTrabalhistaService } from '@manutencao/services/cotacao-indice-trabalhista.service';
import { catchError } from 'rxjs/operators';
import { EMPTY } from 'rxjs';
@Component({
  selector: 'app-resultado-importacao-cotacao',
  templateUrl: './resultado-importacao-cotacao.component.html',
  styleUrls: ['./resultado-importacao-cotacao.component.scss'],
  providers: [{ provide: CotacaoServiceMock, useClass: CotacaoServiceMock }]
})
export class ResultadoImportacaoCotacaoComponent implements AfterViewInit {
  listaIndices: Array<Indice> = [];
  dataSource: Array<Cotacao> = [];
  listaCotacoesIndicesTrabalhistas: Array<CotacaoIndiceTrabalhista> = [];
  total: number = 0;
  dataValidaAgendamento: Date;
  inicioPeriodo: Date;
  fimPeriodo: Date;
  houveBusca: boolean = false;
  indiceColuna: string = '';
  contadorPesquisa: number = 0;
  acumulado = false;

  indiceFormControl: FormControl = new FormControl(null);
  mesAnoInicialFormControl: FormControl = new FormControl(moment().toDate());
  mesAnoFinalFormControl: FormControl = new FormControl(moment().toDate());

  dataDeCorrecaoFormControl: FormControl = new FormControl(moment().toDate(), Validators.required);
  mesAnoInicialDistribuicaoFormControl: FormControl = new FormControl(moment().add(-43, 'years').month(0).toDate(), Validators.required);
  mesAnoFinalDistribuicaoFormControl: FormControl = new FormControl(moment().toDate(), Validators.required);

  @ViewChild(JurTable, { static: false }) table: JurTable<any>;
  @ViewChild(JurPaginator, { static: false }) paginator: JurPaginator;

  constructor(
    private service: CotacaoService,
    private serviceCotacaoIndiceTrabalhista: CotacaoIndiceTrabalhistaService,
    private indiceService: IndiceService,
    private dialog: DialogService
  ) { }
    ngAfterViewInit(): void {
        this.pesquisarCotacao();
    }
    iniciaValoresParaABusca() {

        return {
    
          page: (() => {
            return {
              index: this.paginator === undefined ? 1 : this.paginator.pageIndex + 1,
              size: this.paginator === undefined ? 8 : this.paginator.pageSize,
            }
          })(),
    
          sort: (() => {
    
            let sortColumn: any =
              this.table === undefined || !this.table.sortColumn
                ? (this.indiceColuna == 'CotacaoIndiceTrabalhista' ? "dataCorrecao" : "dataCotacao")
                : this.table.sortColumn;
    
            return {
              column: sortColumn,
              direction: this.table === undefined || !this.table.sortDirection ? "desc" : this.table.sortDirection
            }
          })()
        }
      }
  
      async pesquisarCotacao() {
        let dados = this.iniciaValoresParaABusca();
        this.serviceCotacaoIndiceTrabalhista
        let obj = await this.serviceCotacaoIndiceTrabalhista.obterPaginadoTemCotacaoTrab(
             dados.page.index,
            dados.page.size,
            dados.sort.column,
            dados.sort.direction
          );
          console.log(obj.data);
          this.listaCotacoesIndicesTrabalhistas = obj.data;
          this.total = obj.total;
          this.houveBusca = true;
    }
 
 
    importar(): void {
    this.serviceCotacaoIndiceTrabalhista.AplicarImportacao()
    .then(f => {
      this.dialog.info(
        'Importação realizada com sucesso.',
        ''
      );
      window.history.go(-1)
    }).catch(err => {})
  }

 
 


}
