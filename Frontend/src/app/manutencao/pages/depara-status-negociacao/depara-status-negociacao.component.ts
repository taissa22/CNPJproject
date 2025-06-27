import { PermissoesService } from '@permissoes';
// angular
import { Component, ViewChild, OnInit } from '@angular/core';

// 3rd party
import { EMPTY } from 'rxjs';
import { catchError } from 'rxjs/operators';

// core & shared imports
import { DialogService } from '@shared/services/dialog.service';
import { Page } from '@shared/types/page';
import { SortOf } from '@shared/types/sort';

// local imports
import { FormControl } from '@angular/forms';
import { HttpErrorResult } from '@core/http';
import { SisjurTable } from '@libs/sisjur/sisjur-table/sisjur-table.component';
import { SisjurPaginator } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.component';
import { DeparaStatusNegociacaoService } from '@manutencao/services/depara-status-negociacao.service';
import { DeparaStatusNegociacaoModalComponent } from '@manutencao/modals/depara-status-negociacao-modal/depara-status-negociacao-modal.component';
import { DeparaStatusNegociacao } from '@manutencao/models/depara-status-negociacao.model';
import { DeparaStatusResponse } from '@manutencao/models/depara-status-response.model';
import { HelperAngular } from '@shared/helpers/helper-angular';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { Permissoes } from '@permissoes';
import { TiposProcesso } from '@manutencao/models/tipos-de-processos';

@Component({
  selector: 'app-depara-status-negociacao',
  templateUrl: './depara-status-negociacao.component.html',
  styleUrls: ['./depara-status-negociacao.component.scss']
})
export class DeparaStatusNegociacaoComponent implements OnInit {
  constructor(
    private service: DeparaStatusNegociacaoService,
    private dialog: DialogService,
    private messageService: HelperAngular,
    private breadcrumbsService: BreadcrumbsService,
    private permissoesService: PermissoesService,
  ) { }

  statusDePara: Array<DeparaStatusNegociacao> = [];
  statusAPP: Array<DeparaStatusResponse>;
  substatusAPP: Array<DeparaStatusResponse>;
  statusSisjur: Array<DeparaStatusResponse>;
  total: number = 0;
  breadcrumb: string;
  buscarDescricaoFormControl: FormControl = new FormControl(null);
  tiposProcesso: Array<TiposProcesso> = [];
  exibirTabela = false;
  tipoProcessoFormControl: FormControl = new FormControl(null);

  @ViewChild(SisjurTable, { static: false }) table: SisjurTable;
  @ViewChild(SisjurPaginator, { static: false }) paginator: SisjurPaginator;

  async ngOnInit() {
    this.ObterListaTipoProcesso();
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(Permissoes.ACESSAR_DEPARASTATUSNEGOCIACAO);
    
  }

  async obterStatusNegociacoesAppPrepostoEStatusSisjur(): Promise<void> {
    try {
      const result = await this.service.obterStatusNegociacoesAppPrepostoEStatusSisjur(this.tipoProcessoFormControl.value);
      this.statusAPP = result.statusAppPreposto.map((status: any) => { return { id: status.id, descricao: status.descricao } });
      this.substatusAPP = result.substatusAppPreposto.map((status: any) => { return { id: status.id, descricao: status.descricao } });;
      this.statusSisjur = result.statusSisjur.map((status: any) => { return { id: status.id, descricao: status.descricao } });
    }
    catch (error) {
      await this.dialog.err(`Informações não carregadas`, (error as HttpErrorResult).messages.join('\n'));
    }
  }

  ObterListaTipoProcesso() {
    const permissesTela = [
      { permissao: Permissoes.ACESSAR_DEPARASTATUSNEGOCIACAO, tipoProcesso: TiposProcesso.CIVEL_CONSUMIDOR, menu: 'CÍVEL CONSUMIDOR' },
      { permissao: Permissoes.ACESSAR_DEPARASTATUSNEGOCIACAO, tipoProcesso: TiposProcesso.JEC, menu: 'JUIZADO ESPECIAL CÍVEL' },
      { permissao: Permissoes.ACESSAR_DEPARASTATUSNEGOCIACAO, tipoProcesso: TiposProcesso.PROCON, menu: 'PROCON' }
    ];

    permissesTela.forEach((item) => {
      if (this.permissoesService.temPermissaoPara(item.permissao)) {
        this.tiposProcesso.push({ ...item.tipoProcesso, descricao: item.menu })
      }
    })
  }

  selecionarTipoProcesso() {
    this.buscarDescricaoFormControl.setValue('');
    if (this.exibirTabela) this.buscarTabela();
    this.exibirTabela = true;
  }

  buscarTabela(): void {
    let sort: SortOf<any> = { column: this.iniciaValoresDaView().sortColumn, direction: this.iniciaValoresDaView().sortDirection };
    const page: Page = { index: this.iniciaValoresDaView().pageIndex, size: this.iniciaValoresDaView().pageSize };
    this.service.obter(sort, page, this.tipoProcessoFormControl.value, this.buscarDescricaoFormControl.value).pipe(
      catchError(err => {
        this.dialog.err(
          'Informações não carregadas',
          'Não foi possível carregar as informações, tente novamente.<br>Se o erro persistir, contate o suporte'
        );
        return EMPTY;
      })
    ).subscribe(data => {
      this.statusDePara = data.data;
      this.total = data.total;
      this.obterStatusNegociacoesAppPrepostoEStatusSisjur();
    });;
  }

  iniciaValoresDaView() {
    return {
      sortColumn: this.table === undefined || !this.table.sortColumn ? "id" : this.table.sortColumn,
      sortDirection: this.table === undefined || !this.table.sortDirection ? "asc" : this.table.sortDirection,
      pageIndex: this.paginator === undefined ? 1 : this.paginator.pageIndex + 1,
      pageSize: this.paginator === undefined ? 8 : this.paginator.pageSize,
    }
  }

  async incluir(): Promise<void> {
    const teveAlteracao: boolean = await DeparaStatusNegociacaoModalComponent.exibeModalDeIncluir(this.statusAPP, this.substatusAPP, this.statusSisjur, this.tipoProcessoFormControl.value);
    this.buscarDescricaoFormControl.setValue('');
    if (teveAlteracao) { this.buscarTabela() }
  }

  async alterar(item: DeparaStatusNegociacao): Promise<void> {
    const teveAlteracao: boolean = await DeparaStatusNegociacaoModalComponent.exibeModalDeAlterar(item, this.statusAPP, this.substatusAPP, this.statusSisjur, this.tipoProcessoFormControl.value);
    this.buscarDescricaoFormControl.setValue('');
    if (teveAlteracao) { this.buscarTabela() }
  }

  async excluir(item: DeparaStatusNegociacao): Promise<void> {
    try {
      this.messageService.MsgBox2('Deseja realmente excluir esse DE x PARA?', 'Excluir DE x PARA', 'question', 'Sim', 'Não')
        .then(resposta => {
          if (resposta.value) {
            this.service.excluir(item.id)
              .subscribe(() => {
                this.dialog.alert('Exclusão realizada com sucesso', 'DE x PARA excluído!');
                this.buscarDescricaoFormControl.setValue('');
                this.buscarTabela();
              });
          }
        });
    }
    catch (error) {
      if (error && error.error) {
        await this.dialog.err('Erro.', error.error);
        return;
      }

      await this.dialog.err('remover não realizado.');
    }
  }

  exportar() {
    let sort: SortOf<any>;
    sort = { column: 'descricao', direction: 'asc' };
    sort = { column: this.iniciaValoresDaView().sortColumn, direction: this.iniciaValoresDaView().sortDirection };
    const page: Page = { index: this.paginator.pageIndex, size: this.paginator.pageSize };
    this.service.exportar(sort, page,this.tipoProcessoFormControl.value, this.buscarDescricaoFormControl.value);
  }

  onClearInputPesquisar() {
    if (!this.buscarDescricaoFormControl.value) { this.buscarTabela() }
  }

}
