import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { HttpErrorResult } from '@core/http';
import {  Estados } from '@core/models';
import { SisjurPaginator } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.component';
import { SisjurTable } from '@libs/sisjur/sisjur-table/sisjur-table.component';
import { EstadoModalComponent } from '@manutencao/modals/estado-modal/estado-modal.component';
import { Estado } from '@manutencao/models/estado.model';
import { EstadoService } from '@manutencao/services/estado.service';
import { NgTypeToSearchTemplateDirective } from '@ng-select/ng-select/lib/ng-templates.directive';
import { DialogService } from '@shared/services/dialog.service';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { Permissoes } from '@permissoes';


@Component({
  selector: 'app-estados',
  templateUrl: './estados.component.html',
  styleUrls: ['./estados.component.scss']
})
export class EstadosComponent implements OnInit{
  estados: Array<Estado> = [];
  total: number = 0;
  estadosSelect : Array<Estado> = [] ;
  linhaSelecionada: number = -1;
  estadoFormControl: FormControl = new FormControl(null);
  breadcrumb: string;

  @ViewChild(SisjurTable, { static: true }) table: SisjurTable;
  @ViewChild(SisjurPaginator, { static: true }) paginator: SisjurPaginator;
  constructor(
    private service: EstadoService, 
    private dialog: DialogService,
    private breadcrumbsService: BreadcrumbsService) { }

  async ngOnInit(): Promise<void> {
    this.pesquisarTodos();
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(Permissoes.ACESSAR_ESTADO);
  }

  abrirMunicipios(i) {
    if (this.linhaSelecionada == i) {
      this.linhaSelecionada = -1;
      return;
    }
    this.linhaSelecionada = i;
  }
  
  pesquisar() {
    let view = this.iniciaValoresDaView(); 
    this.service.obterPaginado(
      view.pageIndex,
      view.pageSize,
      this.estadoFormControl.value ? this.estadoFormControl.value : "",
      view.sortColumn,
      view.sortDirection)
      .subscribe(estados => {
        this.estados = estados.data;
        this.total = estados.total;
      });
  }

  iniciaValoresDaView() {
    let sort: any = this.table === undefined || !this.table.sortColumn ? "sigla" : this.table.sortColumn;
    return {
      sortColumn: sort,
      sortDirection: this.table === undefined || !this.table.sortDirection ? "asc" : this.table.sortDirection,
      pageIndex: this.paginator === undefined ? 1 : this.paginator.pageIndex + 1,
      pageSize: this.paginator === undefined ? 8 : this.paginator.pageSize,
    }
  }

  pesquisarTodos(){
    this.service.obterTodos()
      .subscribe(estados => {
        this.estadosSelect = estados;
      });
  }

  exportar() {
    let sort: any = this.table.sortColumn;
    this.service.exportar(
      sort,
      this.table.sortDirection,
      this.estadoFormControl.value ? this.estadoFormControl.value : ""
    );
  }

  async alterar(estado) {
     const teveAlteracao: boolean = await EstadoModalComponent.exibeModalDeAlterar(estado);
    if (teveAlteracao) this.pesquisar();
  }

  async remover(estado: Estado) {

    const excluir: boolean = await this.dialog.confirm(
      'Excluir Estado',
      `Deseja excluir o Estado <br><b>${estado.nome}</b>?`
    );

    if (!excluir) {
      return;
    }

    try {
      await this.service.remover(estado.id);
      await this.dialog.alert(`Exclusão realizada com sucesso`, `Estado excluido!`);
      this.pesquisar(); this.pesquisarTodos();

    } catch (error) {
      if ((error as HttpErrorResult).messages.join().includes('relacionado')) {
        await this.dialog.info(`Exclusão não permitida`, (error as HttpErrorResult).messages.join('\n'));
        return;
      }
      await this.dialog.err(`Exclusão não realizada`, (error as HttpErrorResult).messages.join('\n'));

    }
  }

}
