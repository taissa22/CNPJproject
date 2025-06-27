// angular
import { Component, OnInit, ViewChild } from '@angular/core';

// 3rd party
import { BehaviorSubject, EMPTY, merge } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';

// core & shared imports
import { JurTable } from '@shared/components/jur-table/jur-table.component';
import { JurPaginator } from '@shared/components/jur-paginator/jur-paginator.component';
import { DialogService } from '@shared/services/dialog.service';
import { Page } from '@shared/types/page';
import { Sort, SortOf } from '@shared/types/sort';
import { Permissoes, PermissoesService } from '@permissoes';
import { TiposProcesso } from '@manutencao/services/tipos-de-processos';
// local imports
import { BaseDeCalculo } from '@manutencao/models/base-de-calculo';
import { IndicesVigenciasService } from '@manutencao/services/indices-vigencias.service';
import { IndicesVigencias } from '@manutencao/models/indice-vigencias';
import { HttpErrorResult } from '@core/http';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import moment from 'moment';
import { IndiceVigenciaModalComponent } from '@manutencao/modals/indice-vigencia-modal/indice-vigencia-modal.component';

@Component({
  selector: 'app-indice-vigencias',
  templateUrl: './indices_vigencias_civeis.component.html',
  styleUrls: ['./indices_vigencias_civeis.component.scss'],
  
})
export class IndicesVigenciasCiveisComponent implements  OnInit {
  constructor(
    private service: IndicesVigenciasService,
    private dialog: DialogService,
    private permisoesService: PermissoesService
  ) { }

  dataSource: Array<IndicesVigencias> = [];
  total: number = 0;
  tiposProcesso: TiposProcesso[] = [];
  tiposindice: any[] = [];
  sort: Sort;
  dataInicio = null
  dataFim = null
  esconderFiltros = true;
  dataValida = true;
  @ViewChild(JurTable, { static: true }) table: JurTable<BaseDeCalculo>;
  @ViewChild(JurPaginator, { static: true }) paginator: JurPaginator;

  private search$: BehaviorSubject<string> = new BehaviorSubject('');
  set search(v: string) {
    this.search$.next(v);
  }
  get search(): string {
    return this.search$.value;
  }
  datepickerDataInicio(dataEvent) {
    this.dataInicio = moment(dataEvent, "DD/MM/YYYY");
  }
  datepickerDataFim(dataEvent) {
   
    this.dataFim = moment(dataEvent, "DD/MM/YYYY");
  }
  
  tipoProcessoFormControl: FormControl = new FormControl(null, [
    Validators.required
  ]);
  tipoIndiceFormControl: FormControl = new FormControl(0, [
  ]);
  formGroup: FormGroup = new FormGroup({
    tipoProcesso: this.tipoProcessoFormControl,
    tipoindice: this.tipoIndiceFormControl

  });
  ngOnInit(): void {
    const permissesTela = [ {permissao: Permissoes.ACESSAR_INDICES_VIGENCIA_CIVEL_CONSUMIDOR, tipoProcesso: TiposProcesso.CIVEL_CONSUMIDOR, menu: 'Cível Consumidor'},
                            {permissao: Permissoes.ACESSAR_INDICES_VIGENCIA_CIVEL_ESTRATEGICO, tipoProcesso: TiposProcesso.CIVEL_ESTRATEGICO, menu: 'Cível Estratégico'},
                          ]


      permissesTela.forEach((item) => {
         if (this.permisoesService.temPermissaoPara(item.permissao)) {
           this.tiposProcesso.push({...item.tipoProcesso, descricao: item.menu })
         }
      })
   

}

 aoSelecionarTipoProcesso() {
  this.service.obterIndices(this.tipoProcessoFormControl.value).subscribe(data => {
    this.tiposindice.push({})
    this.tiposindice = data;
    this.esconderFiltros = false
    this.dataSource = []
    this.total = 0
    this.tipoIndiceFormControl.reset()
  });
}


  buscar(){
    merge(this.table.sort, this.paginator.page, this.search$)
      .pipe(
        switchMap(() => {
          const page: Page = {
            index: this.paginator.pageIndex,
            size: this.paginator.pageSize
          };
          let sort: Sort = {
            column: 'descricao',
            direction: 'asc'
          };
          if (this.table.sortDirection !== null) {
            sort = {
              column: this.table.sortColumn,
              direction: this.table.sortDirection
            };
          }

          this.sort = sort;
          return this.service.obterPaginado(page, sort,moment(this.dataInicio, "x").format("DD/MM/YYYY") ,moment(this.dataFim, "x").format("DD/MM/YYYY"),this.tipoProcessoFormControl.value == null ? 0 : this.tipoProcessoFormControl.value,null,this.tipoIndiceFormControl.value == null ? 0 : this.tipoIndiceFormControl.value).pipe(
            catchError(err => {
              
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
        console.log(this.dataSource)
      });
  }
 
  async incluir(): Promise<void> {
    
    const teveAlteracao: boolean = await IndiceVigenciaModalComponent.exibeModalDeIncluir(this.tipoProcessoFormControl.value);
    if (teveAlteracao) {
        this.buscar();
        this.aoSelecionarTipoProcesso();
    }
  }

  async excluir(item: IndicesVigencias): Promise<void> {
    const excluir: boolean = await this.dialog.confirm(
      'Excluir Índice',
      `Deseja excluir o Índice <br><b>${item.descricao}</b>?`
    );

    if (!excluir) {
      return;
    }

    try {
      await this.service.excluir(item.processoId,item.dataVigencia);
      await this.dialog.alert(
        'Exclusão realizada com sucesso',
        'Índice excluído!'
      );
      this.search$.next(this.search);
    } catch (error) {

      await this.dialog.err(`Exclusão não realizada`, (error as HttpErrorResult).messages.join('\n'));
    }
  }
  validarData(valid: boolean): void {
    this.dataValida =  valid 
  }
  exportar() {
    return this.service.exportar(moment(this.dataInicio, "x").format("DD/MM/YYYY") ,moment(this.dataFim, "x").format("DD/MM/YYYY"),this.tipoProcessoFormControl.value == null ? 0 : this.tipoProcessoFormControl.value,null,this.tipoIndiceFormControl.value == null ? 0 : this.tipoIndiceFormControl.value)

  }
}
