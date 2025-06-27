// angular
import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { List } from 'linqts';

// 3rd party
import { EMPTY } from 'rxjs';
import { catchError } from 'rxjs/operators';

// core & shared imports
import { JurTable } from '@shared/components/jur-table/jur-table.component';
import { JurPaginator } from '@shared/components/jur-paginator/jur-paginator.component';
import { DialogService } from '@shared/services/dialog.service';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';

// local imports
import { Comarca } from '@manutencao/models/comarca.model';
import { ColunaGenerica } from '@manutencao/models/coluna-generica.model';
import { ComarcaModalComponent } from '@manutencao/modals/comarca-modal/comarca-modal.component';
import { ComarcaService } from '@manutencao/services/comarca.service';
import { Estado, Estados } from '@core/models';
import { PadStartPipe } from '@shared/pipes/pad-start.pipe';
import { HttpErrorResult } from '@core/http';
import { FormControl } from '@angular/forms';
import { Permissoes } from '@permissoes';
@Component({
  selector: 'app-comarca',
  templateUrl: './comarca.component.html',
  styleUrls: ['./comarca.component.scss']
})
export class ComarcaComponent implements AfterViewInit {
  ordenacaoColuna: 'id' | 'nome' | 'estadoId' | 'comarcaBBId' = 'nome';
  ordenacaoDirecao: 'asc' | 'desc' | null = 'asc';
  comarcaSelecionada = -1;
  total: number = 5;
  comarcas: Array<Comarca> = [];
  breadcrumb: string;
  estadoId: string;
  estados: Array<Estados> = Estados.obterUfs();
  colunas: Array<ColunaGenerica> = [
    new ColunaGenerica('Código', 'id', true, '10%', 'codigo', {
      display: 'inline-block'
    }),
    new ColunaGenerica('Estado', 'estadoId', true, '10%', 'estado', {
      display: 'inline-block'
    }),
    new ColunaGenerica('Nome', 'nome', true, '30%', 'nome', {
      display: 'inline-block'
    }),
    new ColunaGenerica('Comarca BB', 'comarcaBB', true, '35%', 'comarcaBB', {
      display: 'inline-block'
    })
  ];

  textoPesquisadoFormControl = new FormControl(null);

  @ViewChild(JurTable, { static: true }) table: JurTable<Comarca>;
  @ViewChild(JurPaginator, { static: true }) paginator: JurPaginator;

  constructor(
    private dialog: DialogService,
    private serviceComarca: ComarcaService,
    private breadcrumbService: BreadcrumbsService,
  ) { }

  ngAfterViewInit(): void {
    this.pesquisar();
    this.nomeBreadcrumb();
  }

  onClearInputPesquisar(){
     if(!this.textoPesquisadoFormControl.value){
      this.pesquisar()
     }
  }

  async nomeBreadcrumb() {
    this.breadcrumb = await this.breadcrumbService.nomeBreadcrumb(Permissoes.ACESSAR_COMARCA);
  }
  pesquisarPorEstado(event: Estado) {
    this.estadoId = event === undefined ? '' : event.id;
    this.textoPesquisadoFormControl.setValue(null);
    this.pesquisar();
  }

  selecionarComarca(index: number): void {
    if (this.comarcaSelecionada == index) {
      this.comarcaSelecionada = -1;
      return;
    }
    this.comarcaSelecionada = index;
  }

  ordenar(data) {
    this.ordenacaoColuna = data;
    if (this.ordenacaoDirecao == 'asc') this.ordenacaoDirecao = 'desc';
    else if (this.ordenacaoDirecao == 'desc') {
      this.ordenacaoDirecao = null;
      this.ordenacaoColuna = null;
    }
    else this.ordenacaoDirecao = 'asc';
    this.pesquisar();
  }

  pesquisar() {
    this.serviceComarca
      .obterPaginado(
        this.paginator.pageIndex + 1,
        this.paginator.pageSize,
        this.estadoId != null ? this.estadoId : '',
        this.ordenacaoColuna != null ? this.ordenacaoColuna : 'nome',
        this.ordenacaoDirecao == null ? 'asc' : this.ordenacaoDirecao,
        this.textoPesquisadoFormControl.value
      )
      .pipe(
        catchError(() => {
          this.dialog.err(
            'Informações não carregadas',
            'Não foi possível carregar as informações, tente novamente.<br>Se o erro persistir, contate o suporte'
          );
          return EMPTY;
        })
      )
      .subscribe(data => {
        this.total = data.total;
        this.comarcas = data.data;
      });
  }

  async incluirComarca(): Promise<void> {
    const teveAlteracao: boolean = await ComarcaModalComponent.exibeModalDeIncluir();
    if (teveAlteracao) this.pesquisar();
  }

  async alterarComarca(registro: any): Promise<void> {
    const teveAlteracao: boolean = await ComarcaModalComponent.exibeModalDeAlterar(registro);
    if (teveAlteracao) this.pesquisar();
  }

  exportar() {
    this.serviceComarca.exportar(
      this.ordenacaoColuna,
      this.ordenacaoDirecao,
      this.estadoId != null ? this.estadoId : '',
      this.textoPesquisadoFormControl.value
    ).then();
  }

  async removerComarca(comarca: Comarca) {
    const excluirTipoDocumento: boolean = await this.dialog.confirm(
      'Excluir Comarca',
      `Deseja excluir a Comarca<br><b>${comarca.nome}</b>?`
    );

    if (!excluirTipoDocumento) {
      return;
    }

    try {

      await this.serviceComarca.remover(comarca.id);
      await this.dialog.alert(
        'Exclusão realizada com sucesso',
        'Comarca excluída!'
      );
      this.pesquisar();

    } catch (error) {
      if ((error as HttpErrorResult).messages.join().includes('relacionada')) {
        await this.dialog.info(`Exclusão não permitida`, (error as HttpErrorResult).messages.join('\n'));
        return;
      }
      await this.dialog.err(`Exclusão não realizada`, (error as HttpErrorResult).messages.join('\n'));
    }

  }

}
