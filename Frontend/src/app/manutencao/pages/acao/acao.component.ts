// angular
import { Component, ViewChild, AfterViewInit, OnInit } from '@angular/core';
// 3rd party
import { catchError, switchMap } from 'rxjs/operators';
// core & shared imports
import { DialogService } from '@shared/services/dialog.service';
import { Page } from '@shared/types/page';
import { Sort, SortOf } from '@shared/types/sort';
// local imports
import { TiposProcesso } from '@manutencao/models/tipos-de-processos';
import { HttpErrorResult } from '@core/http';
import { FormControl } from '@angular/forms';
import { Permissoes, PermissoesService } from '@permissoes';
import { SisjurTable } from '@libs/sisjur/sisjur-table/sisjur-table.component';
import { SisjurPaginator } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.component';
import { EMPTY } from 'rxjs';
import { Acao } from '@manutencao/models/acao.model';
import { AcaoService } from '@manutencao/services/acao.service';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { HelperAngular } from '@shared/helpers/helper-angular';
import { AcaoModalComponent } from '@manutencao/modals/acao-modal/acao-modal.component';
import { NaturezaBBResponse } from '@manutencao/models/natureza-bb-response.model';
import { AcaoCivelEstrategicoResponse } from '@manutencao/models/acao-civel-estrategico-response';
import { AcaoCivelConsumidorResponse } from '@manutencao/models/acao-civel-consumidor-response';

@Component({
  selector: 'app-acao',
  templateUrl: './acao.component.html',
  styleUrls: ['./acao.component.scss'],
})
export class AcaoComponent implements OnInit {

  constructor(
    private service: AcaoService,
    private dialog: DialogService,
    private permisoesService: PermissoesService,
    private breadcrumbsService: BreadcrumbsService,
    private messageService: HelperAngular,
  ) { }

  naturezaAcaoBB: Array<NaturezaBBResponse>;
  acaoCivelEstrategico: Array<AcaoCivelEstrategicoResponse>;
  exibirTabela = false;
  linhaSelecionada: number = -1;
  recarregar: boolean = false;
  dataSource: Array<Acao> = [];
  total: number = 0;
  sort: Sort;
  breadcrumb: string;
  tiposProcesso: Array<TiposProcesso> = [];
  tipoProcessoFormControl: FormControl = new FormControl(null);
  buscarDescricaoFormControl: FormControl = new FormControl(null);

  acaoCivelConsumidor: Array<AcaoCivelConsumidorResponse>;

  @ViewChild(SisjurTable, { static: false }) table: SisjurTable;
  @ViewChild(SisjurPaginator, { static: false }) paginator: SisjurPaginator;

  async ngOnInit(): Promise<void> {
    this.ObterListaTipoProcesso();
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(Permissoes.ACESSAR_ACAO);
  }

  ObterListaTipoProcesso() {
    const permissesTela = [
      { permissao: Permissoes.F_TIPO_ACAO_CIVEL_CONSUMIDOR, tipoProcesso: TiposProcesso.CIVEL_CONSUMIDOR, menu: 'CÍVEL CONSUMIDOR' },
      { permissao: Permissoes.F_TIPO_ACAO_CIVEL_ESTRATEGICO, tipoProcesso: TiposProcesso.CIVEL_ESTRATEGICO, menu: 'CÍVEL ESTRÁTEGICO' },
      { permissao: Permissoes.F_TIPO_ACAO_CRIMINAL_JUDICIAL, tipoProcesso: TiposProcesso.CRIMINAL_JUDICIAL, menu: 'CRIMINAL JUDICIAL' },
      { permissao: Permissoes.F_TIPO_ACAO_JEC, tipoProcesso: TiposProcesso.JEC, menu: 'JUIZADO ESPECIAL CÍVEL' },
      { permissao: Permissoes.F_TIPO_ACAO_PEX, tipoProcesso: TiposProcesso.PEX, menu: 'PEX' },
      { permissao: Permissoes.F_TIPO_ACAO_PROCON, tipoProcesso: TiposProcesso.PROCON, menu: 'PROCON' },
      { permissao: Permissoes.F_TIPO_ACAO_TRABALHISTA, tipoProcesso: TiposProcesso.TRABALHISTA, menu: 'TRABALHISTA' },
      { permissao: Permissoes.F_TIPO_ACAO_TRIBUTARIO_JUDICIAL, tipoProcesso: TiposProcesso.TRIBUTARIO_JUDICIAL, menu: 'TRIBUTARIO JUDICIAL' }];
    permissesTela.forEach((item) => {
      if (this.permisoesService.temPermissaoPara(item.permissao)) {
        this.tiposProcesso.push({ ...item.tipoProcesso, descricao: item.menu })
      }
    })
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
      this.dataSource = data.data;
      this.total = data.total;
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

  selecionarTipoProcesso() {
    this.buscarDescricaoFormControl.setValue('');
    if (this.exibirTabela) this.buscarTabela();
    this.exibirTabela = true;
  }

  exportar() {
    let sort: SortOf<any> = { column: this.iniciaValoresDaView().sortColumn, direction: this.iniciaValoresDaView().sortDirection };
    this.service.exportar(sort, this.tipoProcessoFormControl.value, this.buscarDescricaoFormControl.value);
  }

  async incluir(): Promise<void> {
    if (this.tipoProcessoFormControl.value == 7 || this.tipoProcessoFormControl.value == 1 || this.tipoProcessoFormControl.value == 18) {
      const result = await this.service.ObterNaturezaAcaoBB();
      this.naturezaAcaoBB = result.naturezaBB.map((status: any) => { return { id: status.id, descricao: status.descricao } });
    }

    if (this.tipoProcessoFormControl.value == 1) {
      const result = await this.service.ObterAcoesCivelEstrategico();
      this.acaoCivelEstrategico = result.acaoCivelEstrategico.map((status: any) => { return { id: status.id, descricao: status.descricao } });
    }

    if (this.tipoProcessoFormControl.value == 9) {
      const result = await this.service.ObterAcoesCivelConsumidor();
      this.acaoCivelConsumidor = result.acaoCivelConsumidor.map((status: any) => { return { id: status.id, descricao: status.descricao } });
    }

    const teveAlteracao: boolean = await AcaoModalComponent.exibeModalDeIncluir(this.tipoProcessoFormControl.value,
                                                                                this.naturezaAcaoBB,
                                                                                this.acaoCivelEstrategico,
                                                                                this.acaoCivelConsumidor);

    if (teveAlteracao) {
      this.buscarDescricaoFormControl.setValue('');
      this.buscarTabela();
    }
  }

  async alterar(item: Acao): Promise<void> {
    if (this.tipoProcessoFormControl.value == 7 || this.tipoProcessoFormControl.value == 1 || this.tipoProcessoFormControl.value == 18) {
      const result = await this.service.ObterNaturezaAcaoBB();
      this.naturezaAcaoBB = result.naturezaBB.map((status: any) => { return { id: status.id, descricao: status.descricao } });
    }

    if (this.tipoProcessoFormControl.value == 1) {
      const result = await this.service.ObterAcoesCivelEstrategico();
      this.acaoCivelEstrategico = result.acaoCivelEstrategico.map((status: any) => { return { id: status.id, descricao: status.descricao } });
    }

    if (this.tipoProcessoFormControl.value == 9) {
      const result = await this.service.ObterAcoesCivelConsumidor();
      this.acaoCivelConsumidor = result.acaoCivelConsumidor.map((status: any) => { return { id: status.id, descricao: status.descricao } });
    }

    const teveAlteracao: boolean = await AcaoModalComponent.exibeModalDeAlterar(this.tipoProcessoFormControl.value,
                                                                                item,
                                                                                this.naturezaAcaoBB,
                                                                                this.acaoCivelEstrategico,
                                                                                this.acaoCivelConsumidor);

    if (teveAlteracao) {
      this.buscarDescricaoFormControl.setValue('');
      this.buscarTabela()
    }
  }

  async excluir(item: Acao) {
    const excluirAcao: boolean = await this.dialog.confirm('Excluir Ação', `Deseja realmente excluir essa ação?`);

    if (!excluirAcao) { return; }

    try {
      await this.service.excluir(item.id, this.tipoProcessoFormControl.value);
      await this.dialog.alert(
        'Exclusão realizada com sucesso',
        'Ação excluída!'
      );
      this.buscarDescricaoFormControl.setValue('');
      this.buscarTabela();
    } catch (error) {
      await this.dialog.info(`Exclusão não permitida`, error.error);
      return;
    }
  }

  onClearInputPesquisar() {
    if (!this.buscarDescricaoFormControl.value) { this.buscarTabela() }
  }

}



