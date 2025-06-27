import { BehaviorSubject, EMPTY, merge, Subject } from 'rxjs';
import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { JurPaginator } from '@shared/components/jur-paginator/jur-paginator.component';
import { JurTable } from '@shared/components/jur-table/jur-table.component';

import { catchError, switchMap } from 'rxjs/operators';
import { DialogService } from '@shared/services/dialog.service';
import { AbstractControl, FormControl, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';

import { Cotacao } from '@manutencao/models/cotacao.model';
import { ImportarCotacaoModalComponent } from '@manutencao/modals/importar-cotacao-modal/importar-cotacao-modal.component'
import { CotacaoModalComponent } from '@manutencao/modals/cotacao-modal/cotacao-modal.component';
import { CotacaoServiceMock } from '@manutencao/services/cotacao.service.mock';
import { CotacaoService } from '@manutencao/services/cotacao.service';
import { IndiceService } from '@manutencao/services/indice.service';
import { Indice } from '@manutencao/models/indice';
import { HttpErrorResult } from '@core/http';
import { OnChanges } from '@angular/core';
import { SortEvent } from '@shared/components/jur-table/sort-event';
import moment from 'moment';
import { CotacaoIndiceTrabalhista } from '@manutencao/models/cotacao-indice-trabalhista.model';
import { CotacaoIndiceTrabalhistaService } from '@manutencao/services/cotacao-indice-trabalhista.service';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { Permissoes } from '@permissoes';
@Component({
  selector: 'app-cotacao',
  templateUrl: './cotacao.component.html',
  styleUrls: ['./cotacao.component.scss'],
  providers: [{ provide: CotacaoServiceMock, useClass: CotacaoServiceMock }]
})
export class CotacaoComponent implements AfterViewInit {
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

  dadosDaBusca: FormGroup = new FormGroup({
    indice: this.indiceFormControl,
    mesAnoInicial: this.mesAnoInicialFormControl,
    mesAnoFinal: this.mesAnoFinalFormControl,
    dataDeCorrecao: this.dataDeCorrecaoFormControl,
    mesAnoInicialDistribuicao: this.mesAnoInicialDistribuicaoFormControl,
    mesAnoFinalDistribuicao: this.mesAnoFinalDistribuicaoFormControl
  });

  @ViewChild(JurTable, { static: false }) table: JurTable<any>;
  @ViewChild(JurPaginator, { static: false }) paginator: JurPaginator;
  breadcrumb: any;

  constructor(
    private service: CotacaoService,
    private serviceCotacaoIndiceTrabalhista: CotacaoIndiceTrabalhistaService,
    private indiceService: IndiceService,
    private dialog: DialogService,
    private breadcrumbsService: BreadcrumbsService
  ) { }

 
  async ngAfterViewInit() {
    await this.buscarIndices();
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(Permissoes.ACESSAR_COTACAO);
  }

  async buscarIndices() {
    this.listaIndices = await this.indiceService.obter();
    this.listaIndices.push(new Indice(-1, "ÍNDICE TRABALHISTA", "", "", false, false));
  }

  pesquisarCotacao() {

    if (this.indiceFormControl.value) this.acumulado = this.indiceFormControl.value.acumulado;

    this.contadorPesquisa++;
    if (this.contadorPesquisa == 1) return;

    this.definirIndiceColuna();

    if (this.indiceFormControl.value && this.indiceFormControl.value.id === -1) {
      this.buscarCotacoesIndicesTrabalhistas();
      
      return;
    }

    if (this.validaBuscaDeCotacoes()) return;

    let dados = this.iniciaValoresParaABusca();

    this.service
      .obterPaginado(
        dados.page,
        dados.sort,
        this.indiceFormControl.value.id ,
        dados.dataInicial,
        dados.dataFinal
      )
      .pipe(
        catchError(() => {
          this.dialog.err(
            'Informações não carregadas',
            'Não foi possível carregar as informações, tente novamente.<br>Se o erro persistir, contate o suporte'
          );
          return EMPTY;
        })
      ).subscribe(data => {
        this.dataSource = data.lista;
        this.total = data.total;
        this.houveBusca = true;
      });

  }

  definirIndiceColuna() {

    switch (this.indiceFormControl.value.codigoValorIndice) {
      case 'F':
        this.indiceColuna = 'Fator';
        break;
      case 'P':
        this.indiceColuna = 'Percentual';
        break;
      case 'V':
        this.indiceColuna = 'Valor';
        break;
      default:
        this.indiceColuna = '';
        break;
    }
    if (this.indiceFormControl.value.id === -1) {
      this.indiceColuna = 'CotacaoIndiceTrabalhista';
    }
    
  }

  // retorna true para invalido e false para valido 
  validaBuscaDeCotacoes() {

    if (!this.indiceFormControl.value) {
      this.dialog.err(
        'Desculpe, a busca não poderá ser realizada',
        'Selecione um índice e informe o período desejado.'
      );
      return true; 
    }

    const dataInicial = this.mesAnoInicialFormControl.value;
    const dataFinal = this.mesAnoFinalFormControl.value;

    if (dataInicial > dataFinal) {
      this.dialog.err(
        'Desculpe, a busca não poderá ser realizada',
        'O período inicial não pode ser maior do que o período final'
      );
      return true; 
    }

    if (isNaN(dataInicial.getTime()) || isNaN(dataFinal.getTime())) {
      this.dialog.err(
        'Desculpe, a busca não poderá ser realizada',
        'Período informado inválido'
      );
      return true;
    }
    return false;
  }

  async excluir(item: Cotacao): Promise<void> {
    const dataCotacao = new Date(item.dataCotacao);
    const dataFormatada = `${('0' + (dataCotacao.getMonth() + 1)).slice(
      -2
    )}/${dataCotacao.getFullYear()}`;
    const excluirCotacao: boolean = await this.dialog.confirm(
      'Excluir Cotação',
      `Deseja excluir a cotação<br><b>de ${dataFormatada}</b>?`
    );

    if (!excluirCotacao) {
      return;
    }

    try {
      await this.service.excluir(item.indice.id, item.dataCotacao);
      await this.dialog.alert(
        'Exclusão realizada com sucesso',
        'Cotação excluída!'
      );
      this.pesquisarCotacao()
    } catch (error) {
      await this.dialog.err(
        `Exclusão não realizada`,
        (error as HttpErrorResult).messages.join('\n')
      );
    }
  }

  async incluir(): Promise<void> {
    if(this.indiceFormControl.value.id < 0){
      const teveAlteracao: boolean =
      await ImportarCotacaoModalComponent.exibeModalDeIncluir(
        this.indiceFormControl.value
      );
    }else{
      const teveAlteracao: boolean =
      await CotacaoModalComponent.exibeModalDeIncluir(
        this.indiceFormControl.value
      );
    if (teveAlteracao) {
      this.pesquisarCotacao();
    }
    }
    
  }

  async alterar(item: Cotacao): Promise<void> {
    const teveAlteracao: boolean =
      await CotacaoModalComponent.exibeModalDeAlterar(
        item,
        this.indiceFormControl.value
      );
    if (teveAlteracao) {
      this.pesquisarCotacao()
    }
  }

  exportar(): void {
    let dados = this.iniciaValoresParaABusca();
    this.service.exportar(
      { column: dados.sort.column, direction: dados.sort.direction },
      this.indiceFormControl.value.id, dados.dataInicial, dados.dataFinal);
  }

 
  //#region ------------  Cotação Indice Trabalhista ---------------


  async buscarCotacoesIndicesTrabalhistas() {
    if (this.validaBuscaCotacoesIndicesTrabalhistas()) return;
    let dados = this.iniciaValoresParaABusca();
    let obj = await this.serviceCotacaoIndiceTrabalhista.obterPaginado(
      dados.dataCorrecaoTrabalhista,
      dados.dataInicioTrabalhista,
      dados.dataFimTrabalhista,
      dados.page.index,
      dados.page.size,
      dados.sort.column,
      dados.sort.direction
    );
      
    this.listaCotacoesIndicesTrabalhistas = obj.data;
    this.total = obj.total;

    this.houveBusca = true;
    

  }

  // retorna true para invalido e false para valido 
  validaBuscaCotacoesIndicesTrabalhistas(): boolean {
    if (this.dataDeCorrecaoFormControl.invalid || this.mesAnoInicialDistribuicaoFormControl.invalid || this.mesAnoFinalDistribuicaoFormControl.invalid) {
      this.dialog.err(
        'Desculpe, a busca não poderá ser realizada',
        'Entre com a data de correção e o período de distribuição para realizar a pesquisa!'
      );
      return true;
    }
    if (this.mesAnoInicialDistribuicaoFormControl.value > this.mesAnoFinalDistribuicaoFormControl.value) {
      this.dialog.err(
        'Desculpe, a busca não poderá ser realizada',
        'Data de distribuição inicial não pode ser maior do que a data de distribuição final'
      );
      return true; 
    }
    return false; 
  }

  async excluirCotacaoIndiceTrabalhista(item: CotacaoIndiceTrabalhista): Promise<void> {

    const excluirCotacao: boolean = await this.dialog.confirm(
      'Excluir Cotação',
      `Deseja excluir a cotação ?`
    );

    if (!excluirCotacao) {
      return;
    }

    try {
      await this.serviceCotacaoIndiceTrabalhista.excluir(item.dataCorrecao, item.dataBase);
      await this.dialog.alert(
        'Exclusão realizada com sucesso',
        'Cotação excluída!'
      );
      this.pesquisarCotacao()
    } catch (error) {
      await this.dialog.err(
        `Exclusão não realizada`,
        (error as HttpErrorResult).messages.join('\n')
      );
    }
  }

  exportarCotacaoIndiceTrabalhista(): void {
    let dados = this.iniciaValoresParaABusca();
    this.serviceCotacaoIndiceTrabalhista.exportar(
      dados.dataCorrecaoTrabalhista,
      dados.dataInicioTrabalhista,
      dados.dataFimTrabalhista, dados.sort.column, dados.sort.direction);
  }
  // --------Cotação Indice Trabalhista -----

  iniciaValoresParaABusca() {

    return {

      dataInicial: (() => {
        return `${this.mesAnoInicialFormControl.value.getMonth() + 1}/01/${this.mesAnoInicialFormControl.value.getFullYear()}`;
      })(),

      dataFinal: (() => {
        return `${this.mesAnoFinalFormControl.value.getMonth() + 1}/01/${this.mesAnoFinalFormControl.value.getFullYear()}`;
      })(),

      dataCorrecaoTrabalhista: (() => {
        let data: Date = this.dataDeCorrecaoFormControl.value
        return data.getFullYear() + "-" + (data.getMonth() + 1);
      })(),

      dataInicioTrabalhista: (() => {
        let data: Date = this.mesAnoInicialDistribuicaoFormControl.value;
        return data.getFullYear() + "-" + (data.getMonth() + 1);
      })(),

      dataFimTrabalhista: (() => {
        let data: Date = this.mesAnoFinalDistribuicaoFormControl.value;
        return data.getFullYear() + "-" + (data.getMonth() + 1);
      })(),

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

}
