import { Component } from '@angular/core';

// angular
import { AfterViewInit, ViewChild } from '@angular/core';

// 3rd party
import { BehaviorSubject, EMPTY, merge } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';

// core & shared imports
import { JurTable } from '@shared/components/jur-table/jur-table.component';
import { JurPaginator } from '@shared/components/jur-paginator/jur-paginator.component';
import { DialogService } from '@shared/services/dialog.service';
import { Page } from '@shared/types/page';
import { Sort } from '@shared/types/sort';

// local imports
//import { TipoDePrazo } from '@manutencao/models/tipo-de-prazo';
import { JurosVigenciasCiveis } from '@manutencao/models/juros-vigencias-civeis.model';
//import { JurosVigenciasCiveisServiceMock } from '@manutencao/services/juros-vigencias-civeis.service.mock';
//import { TipoDePrazoService } from '@manutencao/services/tipo-de-prazo.service';
//import { TipoDePrazoModalComponent } from '@manutencao/modals/tipo-de-prazo-modal/tipo-de-prazo-modal.component';
import { TiposProcesso } from '@manutencao/models/tipos-de-processos';
import { HttpErrorResult } from '@core/http';
import { JurosVigenciasCiveisService } from '@manutencao/services/juros-vigencias-civeis.service';
import { JurosVigenciasCiveisModalComponent } from '@manutencao/modals/juros-vigencias-civeis-modal/juros-vigencias-civeis-modal.component';
import { FormControl, FormGroup } from '@angular/forms';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { Permissoes } from '@permissoes';
//import { TipoDePrazoServiceMock } from '@manutencao/services/tipo-de-prazo.service.mock';


@Component({

  selector: 'app-juros-vigencias-civeis',
  templateUrl: './juros-vigencias-civeis.component.html',
  styleUrls: ['./juros-vigencias-civeis.component.scss'],
  // providers: [
  //   { provide: JurosVigenciasCiveisService, useClass: JurosVigenciasCiveisService }
  // ]

})
export class JurosVigenciasCiveisComponent implements AfterViewInit {
  breadcrumb: string;

  constructor(
    private service: JurosVigenciasCiveisService,
    private dialog: DialogService,
    private breadcrumbsService: BreadcrumbsService
  ) { }
  
  dataSource: Array<JurosVigenciasCiveis> = [];
  total: number = 0;
  sort: Sort;
  dataValida = true;

  private today: Date = new Date();  

  tipoProcessoFormControl: FormControl = new FormControl(null);
  dataInicialFormControl: FormControl = new FormControl(this.getInitialDate());
  dataFinalFormControl: FormControl = new FormControl(this.today);

  form: FormGroup = new FormGroup({
    tipoProcesso: this.tipoProcessoFormControl,
    dataInicial : this.dataInicialFormControl,
    dataFinal : this.dataFinalFormControl
  });

  @ViewChild(JurTable, { static: false }) table: JurTable<JurosVigenciasCiveis>;
  @ViewChild(JurPaginator, { static: false }) paginator: JurPaginator;

  private efetuarBusca$: BehaviorSubject<boolean> = new BehaviorSubject(false);
  set efetuarBusca(v: boolean) {
    this.efetuarBusca$.next(v);
  }
  get efetuarBusca(): boolean {
    return this.efetuarBusca$.value;
  }

  tiposProcesso: TiposProcesso[] = [];

  private dadosParaExportacao : { tipoProcessoId: number, dataInicial: Date, dataFinal: Date };

  async ngAfterViewInit(): Promise<void> {
    this.tiposProcesso = await this.service.getTiposDeProcesso();

    if (this.tiposProcesso.length == 1)
    {
      this.tipoProcessoFormControl.setValue(this.tiposProcesso[0].id);
    }
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(Permissoes.ACESSAR_JUROS);
  }

  getInitialDate(): Date {
    let yearAgo: Date = new Date();
    yearAgo.setFullYear(this.today.getFullYear()-1);

    return yearAgo;
  }

  buscarTabela()
  {
    merge(
      this.table.sort,
      this.paginator.page,
      this.efetuarBusca$,
    )
      .pipe(
        switchMap(() => {  
          const page: Page = {
            index: this.paginator.pageIndex,
            size: this.paginator.pageSize
          };
          let sort: Sort;      
          
          sort = {
            column: 'dataVigencia',
            direction: 'desc'
          };
          
          if (this.table.sortDirection !== null) {
            sort = {
              column: this.table.sortColumn,
              direction: this.table.sortDirection
            };
          } 

          this.sort = sort;          

          return this.service
          .obter(sort, page, this.tipoProcessoFormControl.value, this.dataInicialFormControl.value, this.dataFinalFormControl.value)
            .pipe(
              catchError(() => {
                this.dialog.err(
                  'Informações não carregadas',
                  'Não foi possível carregar as informações, tente novamente.<br>Se o erro persistir, contate o suporte'
                );
                return EMPTY;
              })
            );            
        })        
      )
      .subscribe(data => {
        this.dataSource = data.lista;
        this.total = data.total;

        this.dadosParaExportacao = {  tipoProcessoId: this.tipoProcessoFormControl.value, 
                                      dataInicial: this.dataInicialFormControl.value,
                                      dataFinal: this.dataFinalFormControl.value };
      });  

  }

  async incluir(): Promise<void> {
    const teveAlteracao: boolean = await JurosVigenciasCiveisModalComponent.exibeModalDeIncluir(this.tiposProcesso);
    if (teveAlteracao) {
      this.efetuarBusca = true;
      this.efetuarBusca$.next(this.efetuarBusca)
    }
  }

  async alterar(item: JurosVigenciasCiveis): Promise<void> {
    // prettier-ignore
    const teveAlteracao: boolean = await JurosVigenciasCiveisModalComponent.exibeModalDeAlterar(item, this.tiposProcesso);
    if (teveAlteracao) {
      this.efetuarBusca = true;
      this.efetuarBusca$.next(this.efetuarBusca)
    }
  }

  async excluir(item: JurosVigenciasCiveis): Promise<void> {
    const excluir: boolean = await this.dialog.confirm(
      'Excluir Vigência de Taxa de Juros',
      `Deseja excluir a vigência?`
    );

    if (!excluir) {
      return;
    }

    try {
      
      await this.service.excluir(item.tipoDeProcesso.id, item.dataVigencia);
      await this.dialog.alert(
        'Exclusão realizada com sucesso',
        'O registro foi excluído do sistema.'
      );
      this.efetuarBusca = true;
      this.efetuarBusca$.next(this.efetuarBusca)
    } catch (error) {
      if ((error as HttpErrorResult).messages.join('\n').includes('relacionado')) {
        this.dialog.info('Exclusão não permitida', (error as HttpErrorResult).messages.join('\n'));
      } else {
        this.dialog.err('Exclusão não realizada', (error as HttpErrorResult).messages.join('\n'));
        throw error;
      }
    }
  }

  buscar() {       
    this.efetuarBusca = true;    

    setTimeout(() => {
      this.buscarTabela();
      this.efetuarBusca$.next(this.efetuarBusca)  
    }, 300 );
  }

  exportar(): void {
    this.service.exportar(this.sort, this.dadosParaExportacao.tipoProcessoId, this.dadosParaExportacao.dataInicial, this.dadosParaExportacao.dataFinal);
  }

  pegarData(data: Date, isInicio: boolean) {
    isInicio ? this.dataInicialFormControl.setValue(data) : this.dataFinalFormControl.setValue(data);
  }

  validarData(valid: boolean): void {
    this.dataValida =  valid 
    && this.dataInicialFormControl.valid 
    && this.dataFinalFormControl.valid
  }

}

