import { Component, OnInit } from '@angular/core';
import { ManutencaoCategoriaPagamentoService } from './services/manutencao-categoria-pagamento.service';
import { TipoProcesso } from 'src/app/core/models/tipo-processo';
import { Subscription, BehaviorSubject } from 'rxjs';
import { take } from 'rxjs/operators';
import { BsModalService } from 'ngx-bootstrap';
import { ModalAdicionarDespesasJudiciaisComponent } from './modal-adicionar-despesas-judiciais/modal-adicionar-despesas-judiciais.component';
import { TipoLancamentoCategoriaPagamento } from '@shared/enums/tipo-lancamento-categoria-pagamento.enum';
import { ModalAdicionarGarantiasComponent } from './modal-adicionar-garantias/modal-adicionar-garantias.component';
import { ModalAdicionarHonorariosComponent } from './modal-adicionar-honorarios/modal-adicionar-honorarios.component';
import { ModalAdicionarPagamentosComponent } from './modal-adicionar-pagamentos/modal-adicionar-pagamentos.component';
import { ModalAdicionarRecuperacaoPagamentoComponent } from './modal-adicionar-recuperacao-pagamento/modal-adicionar-recuperacao-pagamento.component';
import { ModalAdicionarService } from './services/modal-adicionar.service';
import { OrdenacaoStatus } from '@shared/interfaces/ordenacao-status';
import { BotaoGridState } from '@shared/interfaces/botao-grid-state';
import { headerGrid } from './categoria-pagamento.constant';
import { DetalheResultadoService } from 'src/app/core/services/sap/detalhe-resultado.service';
import { ordenateHeader } from '@shared/utils';
import { ICategoriaPagamento } from '../interface/ICategoriaPagamento';
import { CategoriaPagamentoService } from '@core/services/sap/categoria-pagamento.service';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { role } from '../../sap.constants';

@Component({
  selector: 'app-manutencao-categoria-pagamento',
  templateUrl: './manutencao-categoria-pagamento.component.html',
  styleUrls: ['./manutencao-categoria-pagamento.component.scss']
})
export class ManutencaoCategoriaPagamentoComponent implements OnInit {
  breadcrumb: string;
  //#endregion

  constructor(private modalService: BsModalService,
    private service: ManutencaoCategoriaPagamentoService,
    private modalAdicionarService: ModalAdicionarService,
    private categoriaPagamentoService : CategoriaPagamentoService,
    private breadcrumbsService: BreadcrumbsService) { }

  get tipoProcessoSelecionado() {
    return this.service.tipoProcessoSelecionado;
  }

  comboTipoProcesso: TipoProcesso[] = [];
  comboCategoriaPagamentoEstrategico: any[];
  comboTipoLancamento = [];
  categoriasPagamento;
  exibirTable = false;
  exibir = false;
  tipoLancamento;
  tipoProcesso;
  desabilitarComboLancamento = true;
  codigo: number;
  tySubject = new BehaviorSubject<Array<OrdenacaoStatus>>([]);


  buscado;
  //#region Subscriptions
  comboTipoProcessoSubscription: Subscription;

  quantidadeItens;

  categoriasSubscription: Subscription;
  categoriaSelecionada: Subscription;

  categorias: ICategoriaPagamento[];
  categoriaObj: ICategoriaPagamento;



  categoria: ICategoriaPagamento[] = [];
  categoriaKeys = [];
  isNotFound = false;

  headerGrid = headerGrid;

  selecionado = new BehaviorSubject<boolean>(false);

  ngOnInit() {
    this.buscado = false;
    this.comboTipoProcessoSubscription = this.service.getTipoProcesso()
      .subscribe(comboboxItens => { this.comboTipoProcesso = comboboxItens; });


    this.service.currentValueComboTipoProcessoSubject.subscribe(
      item => {


          this.tipoProcesso = isNaN(item) ? null : item;
          if(!this.tipoLancamento)
          this.atualizarComboLancamento(item);


      }
    );

    this.service.currentValueComboLancamentoSubject.subscribe(lancamento =>{
      if(lancamento)
      this.tipoLancamento = lancamento
    }
      );


    this.categoriasSubscription = this.service.onChangeCategorias.subscribe(fornecedores => this.categorias = fornecedores);
    this.categoriaSelecionada = this.service.selectedCategoriasSubject.subscribe(forn => this.categoriaObj = forn[0]);

    this.modalAdicionarService.sucesso.subscribe(
      item => {
        if (item) {
          this.categoriaObj = null;
        }
      }
    );
  }

  async ngAfterViewInit(): Promise<void> {
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(role.menuCategoriaPagamento);
  }

  atualizarComboLancamento(tipoProcesso) {
    parseInt(tipoProcesso);
    const listaComboLancamento = this.service.comboboxTipoLancamentoSubject.value;
    let comboFiltrada;
    comboFiltrada = listaComboLancamento.filter(item => item.tipoProcesso.some(i => i == tipoProcesso));

    this.comboTipoLancamento = comboFiltrada;
    if (this.comboTipoLancamento.length > 0) {
      this.desabilitarComboLancamento = false;
    }
  }

  ngOnDestroy(): void {
    this.comboTipoProcessoSubscription.unsubscribe();
    this.service.currentValueComboLancamentoSubject.next(null);
    this.service.currentValueComboTipoProcessoSubject.next(null);
    this.exibir = false;
    this.desabilitarComboLancamento = true;
  }

  onClickBuscar() {
    this.service.clearAllData();
    this.refreshCategoriaPagamento();
    this.service.selectedCategoriasSubject.next([]);
    this.exibir = true;
    this.buscado = true;
    this.exibirTable = true;
  }

  isDesabilitadoInluir(): boolean {

    let lancamento;
    let tipoProcesso;
    this.service.currentValueComboLancamentoSubject.subscribe(
      item => lancamento = item
    );
    this.service.currentValueComboTipoProcessoSubject.subscribe(
      item => tipoProcesso = item
    );


    if ( !lancamento || !tipoProcesso) {
      return true;
    } else {
      return false;
    }

  }

  isDesabilitado(): boolean {
    let lancamento;
    let tipoProcesso;
    this.service.currentValueComboLancamentoSubject.subscribe(
      item => lancamento = item
    );
    this.service.currentValueComboTipoProcessoSubject.subscribe(
      item => tipoProcesso = item
    );


    if ( (!this.codigo && !lancamento && !tipoProcesso)
    || (!lancamento && tipoProcesso)

     ) {
      return true;
    } else {
      return false;
    }

  }


  onClickAdicionar(isEditar) {
    if (this.categoriaObj && isEditar || !isEditar) {
      const parametrosUtilizados = {
        tipoProcesso: this.service.currentValueComboTipoProcessoSubject.value,
        tipoLancamento: this.service.currentValueComboLancamentoSubject.value
      };
      if (!parametrosUtilizados.tipoProcesso || !parametrosUtilizados.tipoLancamento) {
        return; // TODO: Alterar com entrada do back
      }

      if (isEditar) {
        this.modalAdicionarService.editarCategoria(this.categoriaObj);

      } else {
        this.modalAdicionarService.addCategoria();
      }

      switch (parametrosUtilizados.tipoLancamento) {
        case TipoLancamentoCategoriaPagamento.despesasJudiciais: this.modalService.show(ModalAdicionarDespesasJudiciaisComponent);
          break;
        case TipoLancamentoCategoriaPagamento.garantias: this.modalService.show(ModalAdicionarGarantiasComponent);
          break;
        case TipoLancamentoCategoriaPagamento.honorários: this.modalService.show(ModalAdicionarHonorariosComponent);
          break;
        case TipoLancamentoCategoriaPagamento.pagamentos: this.modalService.show(ModalAdicionarPagamentosComponent);
          break;
        case TipoLancamentoCategoriaPagamento.recuperacaoPagamento: this.modalService.show(ModalAdicionarRecuperacaoPagamentoComponent);
      }


    }
  }

  onChangeComboLancamento(index) {
    this.tipoLancamento = index;
    this.service.setCurrentValueComboLancamento(parseInt(index));
  }

  onChangeComboTipoProcesso(index) {
    this.tipoLancamento = null;
    this.service.setCurrentValueComboLancamento(parseInt(this.tipoLancamento));
    this.service.setCurrentValueComboTipoProcesso(parseInt(index));
    this.exibirTable = false;
    this.buscado = false;    
  }



  isButtonActive(header) {
    return this.service.isOrdenacaoActive(header);
  }

  onChangeOrdenacao(header: string, ordenacao: BotaoGridState) {
    const { isActive, ordemCrescente } = ordenacao;
    this.updateBotaoOrdenacao(header);
    this.service.updateOrdenacao(header, ordemCrescente);
    this.refreshCategoriaPagamento();
  }

  private updateBotaoOrdenacao(header: string) {
    const previousHeader = this.service.headerSubject.value;
    if (previousHeader) {
      this.service.updateOrdenacaoActivity(previousHeader, false);
    }
    this.service.updateHeader(header);
    this.service.updateOrdenacaoActivity(header, true);
  }

  /**
   * Essa função verifica o qual o nome do tipo de processo atual e qual o tipo de lançamento
   * e retorna o nome da variável que é chamada no service e seus valores
   */
  procurarListaTipoProcessoTipoLancamento(): string[] {
    const lista = headerGrid;
    let valores = lista.map(item => item['listaGrid' + this.service.nomeTipoProcessoSemEspaco +
      this.service.nomeTipoLancamentoSemEspaco]).filter(dados => dados != undefined);
    //o array a cima vem com um array de default, por isso foi criado um novo forEach
    valores.forEach(item => item.forEach(a => valores.push(a)));
    return valores;
  }
  onRowClick(indexEmpresaSap) {
    const estadoAnterior = this.categoria[indexEmpresaSap].selected;
    this.categoria.forEach(e => e.selected = false);
    this.categoria[indexEmpresaSap].selected = !estadoAnterior;

    if (this.categoria[indexEmpresaSap].selected) {
      this.categoriaObj = this.categoria[indexEmpresaSap];
    } else {
      this.categoriaObj = null;
    }
    this.selecionado.next(this.categoria[indexEmpresaSap].selected);

  }
  onClickExportar() {
    this.service.exportar();
  }

  refreshCategoriaPagamento() {
    this.categoriaObj = null;
    this.service.codigo = this.codigo;
    this.service.getCategorias()
      .subscribe(newCategoria => {

        this.categoria = newCategoria;

        if ( newCategoria.length > 0) {
          this.service.popularComboQuandoFiltradoPeloCodigo(this.categoria[0]);
          this.categoriaKeys = Object.keys(newCategoria[0])
            .filter(item => item != 'selected' &&
              this.procurarListaTipoProcessoTipoLancamento()
            );

          this.categoriaKeys =
            ordenateHeader(this.categoriaKeys, this.procurarListaTipoProcessoTipoLancamento());

          this.categoriaKeys.forEach(key => {
            this.service.pushOrdenacaoActivity({
              key,
              isActive: false
            });
          });

          this.isNotFound = false;
        } else {
          this.isNotFound = true;
        }
      });
  }



  onclickExcluir() {
    if (this.categoriaObj) {
      this.modalAdicionarService.excluirCategoria(this.categoriaObj);
    }
  }



}
