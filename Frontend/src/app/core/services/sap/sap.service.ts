import { RelatorioLancamentoModel } from './../../models/relatorioLancamento.model';

import { Injectable } from '@angular/core';
import { TipoProcessoService } from './tipo-processo.service';
import { BehaviorSubject } from 'rxjs';
import { FiltroModel } from '../../models/filtro.model';
import { FilterService } from 'src/app/sap/consultaLote/services/filter.service';


@Injectable({
  providedIn: 'root'
})
export class SapService {

  constructor(
    private tipoProcessoService: TipoProcessoService,
    private filterService: FilterService) { }



  public getListaFiltro: FiltroModel[] = [];

  public getRelatorioLancamento: RelatorioLancamentoModel[] = [];

  public currentCount = 0;
  private formStatus = new BehaviorSubject<any>('VALID');

  //#region titulos que irão para a parte de filtro na consulta do lote
  private tituloCriterio = 'Critérios Gerais';
  private tituloProcesso = 'Processos';
  private tituloEmpresaGrupo = 'Empresa do Grupo';
  private tituloEscritorio = 'Escritório';
  private tituloFornecedor = 'Fornecedor';
  private tituloCentroCusto = 'Centro de Custo';
  private tituloTipoLancamento = 'Tipo de Lançamento';
  private tituloCategoriaPagamento = 'Categoria de Pagamento';
  private tituloStatusPagamento = 'Status de Pagamento';
  private tituloSap = 'Pedido SAP';
  private tituloGuia = 'Número da Guia';
  private tituloNumeroContaJudicial = 'Número da Conta Judicial';
  private tituloNumeroLote = 'Número do Lote';
  //#endregion

  //#region lista do filtro
  listaFiltro = [
    {
      id: 1,
      titulo: this.tituloCriterio,
      linkMenu: 'criteriosGeraisGuia',
      selecionado: false,
      ativo: this.filterService.tipoProcessoTracker.value ? true : false,
      marcado: false,
      tituloPadrao: this.tituloCriterio
    },
    {
      id: 2,
      titulo: this.tituloProcesso,
      linkMenu: 'processosGuia',
      selecionado: false,
      ativo: false,
      marcado: false,
      tituloPadrao: this.tituloProcesso
    }, {
      id: 3,
      titulo: this.tituloEmpresaGrupo,
      linkMenu: 'empresaGrupoGuia',
      selecionado: false,
      ativo: false,
      marcado: false,
      tituloPadrao: this.tituloEmpresaGrupo
    }, {
      id: 4,
      titulo: this.tituloEscritorio,
      linkMenu: 'escritorioGuia',
      selecionado: false,
      ativo: false,
      marcado: false,
      tituloPadrao: this.tituloEscritorio
    }, {
      id: 5,
      titulo: this.tituloFornecedor,
      linkMenu: 'fornecedorGuia',
      selecionado: false,
      ativo: false,
      marcado: false,
      tituloPadrao: this.tituloFornecedor
    }, {
      id: 6,
      titulo: this.tituloCentroCusto,
      linkMenu: 'centroCustoGuia',
      selecionado: false,
      ativo: false,
      marcado: false,
      tituloPadrao: this.tituloCentroCusto,
    }, {
      id: 7,
      titulo: this.tituloTipoLancamento,
      linkMenu: 'tipoLancamentoGuia',
      selecionado: false,
      ativo: false,
      marcado: false,
      tituloPadrao: this.tituloTipoLancamento,
    },
    {
      id: 8,
      titulo: this.tituloCategoriaPagamento,
      linkMenu: 'categoriaPagamentoGuia',
      selecionado: false,
      ativo: false,
      marcado: false,
      tituloPadrao: this.tituloCategoriaPagamento,
    },
    {
      id: 9,
      titulo: this.tituloStatusPagamento,
      linkMenu: 'statusPagamentoGuia',
      selecionado: false,
      ativo: false,
      marcado: false,
      tituloPadrao: this.tituloStatusPagamento,
    },
    {
      id: 10,
      titulo: this.tituloSap,
      linkMenu: 'pedidoSapGuia',
      selecionado: false,
      ativo: false,
      marcado: false,
      tituloPadrao: this.tituloSap,
    },
    {
      id: 11,
      titulo: this.tituloGuia,
      linkMenu: 'numeroGuiaSapGuia',
      selecionado: false,
      ativo: false,
      marcado: false,
      tituloPadrao: this.tituloGuia,
    },
    {
      id: 12,
      titulo: this.tituloNumeroContaJudicial,
      linkMenu: 'numeroContaJudicialGuia',
      selecionado: false,
      ativo: false,
      marcado: false,
      tituloPadrao: this.tituloNumeroContaJudicial,
    },
    {
      id: 13,
      titulo: this.tituloNumeroLote,
      linkMenu: 'filtroNumeroDoLote',
      selecionado: false,
      ativo: false,
      marcado: false,
      tituloPadrao: this.tituloNumeroLote,
    }
  ];

  //#endregion

  // Liberar o filtro dependendo se existe tipo de processo
  public ativado = new BehaviorSubject<boolean>(true);

  //#region  Lista dos nomes dos filtros a ser levados para a pagina de consulta e suas rotas


  public get formStatusValue() {
    return this.formStatus;
  }

  public set formStatusValue(v: any) {
    this.formStatus.next(v);
  }

  listarComTipoProcesso(tipoProcesso) {
    this.filterService.limparListarAoChamarNovamente();
    if (tipoProcesso) {
      this.filterService.getFiltros(tipoProcesso);
    }
    this.getListaFiltro = this.listaFiltro;
    return this.getListaFiltro;
  }

  public ListarFiltros() {
    this.getListaFiltro = this.listaFiltro;
    return this.getListaFiltro;
  }

  //#endregion





  get tipoProcessoSelecionado() {
    return this.filterService.tipoProcessoAtual;
  }

  limparContadores() {
    this.atualizaCount(0, null);
  }



  //#region  Countadores da tela de filtro de consulta
  /**
   * Atualiza a contagem na tela de filtro de consulta de lote
   *
   * @param counter: a contagem propriamente dita
   * @param id: o id que está na lista do getListaFiltro
   *
   */
  atualizaCount(counter: number, id: number) {
    //let tituloAux :string
    if (!id) {
      this.getListaFiltro.map(item => {
        item.titulo = item.tituloPadrao;
        item.selecionado = false;
      });
    } else {
      this.getListaFiltro.map(item => {
        if (item.id === id) {
          this.currentCount = counter;
          item.selecionado = true;
          if (counter === 0) {
            item.titulo = item.tituloPadrao;
            item.selecionado = false;
          } else {
            item.titulo = item.tituloPadrao + ' (' + counter + ') ';
            item.selecionado = true;
          }
        }
      });
    }


  }



  LimparDados() {
    this.filterService.limpar();
    this.limparContadores();

  }

  //#endregion


}
