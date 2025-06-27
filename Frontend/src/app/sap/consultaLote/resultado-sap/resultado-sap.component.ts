/** Este código foi batizado pela arte milenar.
 *
 *                                  |\    /|
                              ___| \,,/_/
                           ---__/ \/    \
                          __--/     (D)  \
                          _ -/    (_      \
                         // /       \_ /  -\
   __-------_____--___--/           / \_ O o)
  /                                 /   \__/
 /                                 /
||          )                   \_/\
||         /              _      /  |
| |      /--______      ___\    /\  :
| /   __-  - _/   ------    |  |   \ \
 |   -  -   /                | |     \ )
 |  |   -  |                 | )     | |
  | |    | |                 | |    | |
  | |    < |                 | |   |_/
  < |    /__\                <  \
  /__\                       /___\

  * Todo trecho de código aqui deve seguir o go horse. Caso contrário, o programa irá quebrar.
  */


import { formatDate } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { ItemExpansivel } from '@shared/interfaces/item-expansivel';
import { AngularCsv } from 'angular-csv-ext/dist/Angular-csv';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { Subscription } from 'rxjs';
import { InfoTooltip } from 'src/app/core/models/info-tooltip.model';
import { DetalheResultadoService } from 'src/app/core/services/sap/detalhe-resultado.service';
import { DownloadService } from 'src/app/core/services/sap/download.service';
import { ResultadoService } from 'src/app/core/services/sap/resultado.service';
import { TipoProcessoService } from 'src/app/core/services/sap/tipo-processo.service';
import { ItensMenuLog } from 'src/app/processos/models/menu/itens-menu-log';
import { PermissoesSapService } from '../../permissoes-sap.service';
import { FilterService } from '../services/filter.service';
import { ModalAlteracaoDataGuiaComponent } from './modal-alteracao-data-guia/modal-alteracao-data-guia.component';
import { ModalAlteracaoDataGuiaService } from './modal-alteracao-data-guia/service/modal-alteracao-data-guia.service';
import { ResultadoSapService } from './resultado-sap.service';

@Component({
  // tslint:disable-next-line: component-selector
  selector: 'resultado-sap',
  templateUrl: './resultado-sap.component.html',
  styleUrls: ['./resultado-sap.component.scss']
})
export class ResultadoSapComponent implements OnInit, OnDestroy {

  public listaResultados: ItemExpansivel[];
  public verMais = true;
  subscription: Subscription;
  public lote;
  public sinal;
  public parametros = {};
  public keysParametros = [];
  public infoTooltip: InfoTooltip;
  temPermissao;

  titulo = "Consulta, Controle e Acompanhamento de Lotes - " + this.nomeProcesso;
  constructor(private service: ResultadoService,
    private detalhamentoService: DetalheResultadoService,
    private permissoesSapService: PermissoesSapService,
    private downloadService: DownloadService,
    private resultadoSapService: ResultadoSapService,
    private tipoProcessoService: TipoProcessoService,
    private resultadoService: ResultadoService,
    private filterservice: FilterService,
    private modalService: BsModalService,
    private modalAlteracaoService: ModalAlteracaoDataGuiaService) { }



  ngOnInit() {
    this.verificarPermissao();
    this.infoTooltip = this.resultadoService.infoTooltip;
    this.subscription = this.service.listaResultados
      .subscribe(listaResultados => this.listaResultados = listaResultados);
    this.verMais = this.service.filtros.total !== this.listaResultados.length && this.service.filtros.total !== null;
    this.updateParametrosConsulta();
  }
  public bsModalRef: BsModalRef;

  updateParametrosConsulta() {
    const filtros = { ...this.service.filtros };
    filtros.idsTipoLancamentos = this.filterservice.auxTipoLancamento;
    filtros.idsCentroCustos = this.filterservice.auxCentroCusto;
    filtros.idsFornecedores = this.filterservice.auxFornecedor;
    filtros.idsCategoriasPagamentos = this.filterservice.auxIdCategoriaDePagamentos;
    filtros.idsStatusPagamentos = this.filterservice.auxStatusPagamento;
    filtros.idsEmpresasGrupo = this.filterservice.auxEmpresaGrupo;
    filtros.idsEscritorios = this.filterservice.auxEscritorios;

    // filtros.idsNumerosLote = filtros.idsNumerosLote.join('\n')

    // Removendo campos desnecessários
    const toExclude = ['total', 'pagina', 'quantidade'];
    const excludeApos = ['dataCriacaoPedidoMaior', 'dataCriacaoMaior',
      'dataCancelamentoLoteFinal', 'dataCancelamentoLoteFinal',
      'dataErroProcessamentoFinal', 'dataRecebimentoFiscalFim',
      'dataPagamentoPedidoFinal', 'valorTotalLoteFim',
      'dataPagamentoPedidoFim'];

    toExclude.forEach(e => delete filtros[e]);

    this.parametros = this.deleteEmpty(filtros);





    if (this.parametros.hasOwnProperty('idsProcessos')) {
      let processosSelecionados = [];
      let processosSelecionadosNumero = [];

      this.filterservice.processosSelecionados.ForEach(element => {

        processosSelecionados = [...processosSelecionados, element.id];
        processosSelecionadosNumero = [...processosSelecionadosNumero, element.numeroProcesso];
        // processosSelecionados = processosSelecionados + element.numeroProcesso + ','
      });
      this.parametros['idsProcessos'] = processosSelecionadosNumero.join('\n');
      this.service.filtros.idsProcessos = processosSelecionados;
    }

    if (this.parametros.hasOwnProperty('idsNumerosLote')) {
      this.parametros['idsNumerosLote'] = filtros.idsNumerosLote.join('\n')
    }

    if (this.parametros.hasOwnProperty('tipoProcesso')) {
      this.parametros['tipoProcesso'] = this.filterservice.nomeProcesso;
    }

    if (this.parametros.hasOwnProperty('dataCriacaoMenor')) {
      this.parametros['dataCriacaoMenor'] = this.filterservice.dataCriacaoMenor;
    }

    if (this.parametros.hasOwnProperty('dataCancelamentoLoteInicio')) {
      this.parametros['dataCancelamentoLoteInicio'] = this.filterservice.dataCancelamentoLoteInicio;
    }

    if (this.parametros.hasOwnProperty('dataErroProcessamentoInicio')) {
      this.parametros['dataErroProcessamentoInicio'] = this.filterservice.dataErroProcessamentoInicio;
    }

    if (this.parametros.hasOwnProperty('dataRecebimentoFiscalInicio')) {
      this.parametros['dataRecebimentoFiscalInicio'] = this.filterservice.dataRecebimentoFiscalInicio;
    }

    if (this.parametros.hasOwnProperty('dataPagamentoPedidoInicio')) {
      this.parametros['dataPagamentoPedidoInicio'] = this.filterservice.dataPagamentoPedidoInicio;
    }

    if (this.parametros.hasOwnProperty('dataEnvioEscritorioInicio')) {
      this.parametros['dataEnvioEscritorioInicio'] = this.filterservice.dataEnvioEscritorioInicio;
    }

    if (this.parametros.hasOwnProperty('dataCriacaoPedidoMenor')) {
      this.parametros['dataCriacaoPedidoMenor'] = this.filterservice.dataCriacaoPedidoMenor;
    }

    if (this.parametros.hasOwnProperty('valorTotalLoteInicio')) {
      this.parametros['valorTotalLoteInicio'] = this.filterservice.valorTotalLoteInicio;
    }

    if (this.parametros.hasOwnProperty('idsEmpresasGrupo')) {
      this.parametros['idsEmpresasGrupo'] = this.filterservice.nomeidsEmpresasGrupo;
    }
    if (this.parametros.hasOwnProperty('idsEscritorios')) {
      this.parametros['idsEscritorios'] = this.filterservice.nomeidsEscritorios;
    }
    if (this.parametros.hasOwnProperty('idsCentroCustos')) {
      this.parametros['idsCentroCustos'] = this.filterservice.nomeidsCentroCustos;
    }
    if (this.parametros.hasOwnProperty('idsTipoLancamentos')) {
      this.parametros['idsTipoLancamentos'] = this.filterservice.nomeidsTipoLancamentos;
    }
    if (this.parametros.hasOwnProperty('idsStatusPagamentos')) {
      this.parametros['idsStatusPagamentos'] = this.filterservice.nomeidsStatusPagamentos;
    }
    if (this.parametros.hasOwnProperty('idsCategoriasPagamentos')) {
      this.parametros['idsCategoriasPagamentos'] = this.filterservice.nomeidsCategoriasPagamentos;
    }
    if (this.parametros.hasOwnProperty('idsFornecedores')) {
      this.parametros['idsFornecedores'] = this.filterservice.nomeidsFornecedores;
    }


    excludeApos.forEach(e => delete this.parametros[e]);

    this.keysParametros = this.ordenateHeader(Object.keys(this.parametros));
    this.keysParametros.forEach(key => {
      if (!this.parametros[key] ||
        (Array.isArray(this.parametros[key]) &&
          !(this.parametros[key].length > 0))) {
        delete this.parametros[key];
      }
    });
  }



  carregarMais() {
    const filtroVerMais = this.service.filtros;
    this.service.getListaResultados(filtroVerMais).subscribe(lista => this.listaResultados.push.apply(this.listaResultados, lista));
    this.verMais = this.service.filtros.total !== this.listaResultados.length;
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  signalChange(object) {
    console.log(object);
    this.lote = object.id;
    this.detalhamentoService.setCurrentItem(object);
    
  }

  openchange(e) {
    this.detalhamentoService.setSinal(true);
    
  }

  exportar() {
    this.downloadService.baixarExportacaoLotes(this.service.filtros, this.filterservice.tipoProcessoTracker.value);
  }

  get nomeProcesso() {
    return this.filterservice.nomeProcesso;
  }

  private ordenateHeader(header) {
    const headerOrder = ['tipoProcesso', 'statusContabil', 'statusProcesso',
      'dataCriacaoMenor', 'dataCriacaoMaior',
      'dataCriacaoPedidoMenor', 'dataCriacaoPedidoMaior',
      'dataCancelamentoLoteInicio', 'dataCancelamentoLoteFim',
      'dataErroProcessamentoInicio', 'dataErroProcessamentoFim',
      'dataRecebimentoFiscalInicio', 'dataRecebimentoFiscalFim',
      'dataPagamentoPedidoInicio', 'dataPagamentoPedidoFim',
      'dataEnvioEscritorioInicio', 'dataEnvioEscritorioFim',
      'valorTotalLoteInicio', 'valorTotalLoteFim',
      'numeroContaJudicial', 'idsProcessos',
      'idsEmpresasGrupo', 'idsEscritorios',
      'idsFornecedores', 'idsCentroCustos',
      'idsTipoLancamentos', 'idsCategoriasPagamentos',
      'idsStatusPagamentos',
      'idsPedidosSAP', 'idsNumerosGuia', 'idsNumerosLote'];

    let newHeader = [];
    headerOrder.forEach(order => {
      if (header.includes(order)) {
        newHeader.push(order);
      }
    });
    return newHeader;
  }

  exportarFiltros() {
    const options = {
      fieldSeparator: ';',
      quoteStrings: '"',
      decimalSeparator: '.',
      showLabels: true,
      showTitle: false,
      useTextFile: false,
      useBom: true,
      useKeysAsHeaders: false,
      headers: ['Critério de Pesquisa', 'Valor']
    };
    // tslint:disable-next-line: no-unused-expression
    new AngularCsv(this.filterservice.valoresExportacao,
      'Criterio_Pesquisa_Consulta_Lotes_' +
      formatDate(new Date(), 'yyyyMMdd_HHmmss', 'pt_BR'), options);
  }

  cancelarLote(lote) {
    
    this.resultadoSapService.cancelarLote(lote,this.filterservice.filtro.tipoProcesso);
    this.listaResultados.forEach(item => item.isOpen = false);
   // this.listaResultados.find(item => item.id == resultado.id).isOpen = true;
  }
  isArray(obj: any) {
    return Array.isArray(obj);
  }


  private deleteEmpty(array) {
    Object.keys(array).forEach(key => {
      if (!array[key] || (Array.isArray(array[key]) &&
        !(array[key].length > 0))) {
        delete array[key];
      }
    })
    return array;
  }
  /**
   * Verifica se tem permissão para regerar arquivo BB
   */
  temPermissaoRegerarArquivoBB = false;

  verificarPermissao() {
    this.temPermissao = this.permissoesSapService.f_ExportarLotesCivelCons ||
      this.permissoesSapService.f_ExportarLotesEstrat ||
      this.permissoesSapService.f_ExportarLotesJuizado ||
      this.permissoesSapService.f_ExportarLotesPex ||
      this.permissoesSapService.f_ExportarLotesTrabalhista;

    this.temPermissaoRegerarArquivoBB = this.permissoesSapService.f_RegerarArquivoBBLotesCivelCons
      || this.permissoesSapService.f_RegerarArquivoBBLotesJuizado
      || this.permissoesSapService.f_RegerarArquivoBBLotesPEX

  }

  onClickRegerarArquivo(resultado) {
    this.bsModalRef = this.modalService.show(ModalAlteracaoDataGuiaComponent);
    this.modalAlteracaoService.resultadoSelecionado.next(resultado);
  }

  manterDados() {
    this.filterservice.manterDados = true;
  }
}
