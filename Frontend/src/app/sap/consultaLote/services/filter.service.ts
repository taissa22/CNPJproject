import { LoteFiltroDTO } from '@shared/interfaces/lote-filtro-dto';
import { SapService } from 'src/app/core/services/sap/sap.service';
import { DualListTitulo } from './../../../core/models/dual-list-titulo';
import { take } from 'rxjs/operators';
import { Injectable, Output, EventEmitter } from '@angular/core';
import { List } from 'linqts';
import { BehaviorSubject, Observable } from 'rxjs';
import { TipoProcessoService } from '../../../core/services/sap/tipo-processo.service';
import { DualListModel } from 'src/app/core/models/dual-list.model';
import { ApiService } from 'src/app/core';
import {  FormGroup, FormControl } from '@angular/forms';
import { CriteriosGeraisService } from './criterios-gerais.service';

import { ResultadoService } from 'src/app/core/services/sap/resultado.service';
import swal from 'sweetalert2';
import { money, TiposProcessosMapped, TiposProcessosCivelPluralMapped } from '@shared/utils';
import { formatDate } from '@angular/common';



export interface Exportacao {
  criterio: string,
  valor: any[]
}

@Injectable({
  providedIn: 'root'
})


export class FilterService {
  @Output() limparFiltros = new BehaviorSubject<boolean>(false);

  @Output() limparContadores = new EventEmitter<boolean>();

  constructor(private tipoProcessoService: TipoProcessoService,
    private http: ApiService, private resultadoService: ResultadoService,
  ) { }

  // LISTA DA EMPRESA
  public listaEmpresa: Array<DualListModel> = [];
  public listaEscritorio: Array<DualListModel> = [];
  public listaCentroCusto: Array<DualListModel> = [];
  public listaFornecedor: Array<DualListModel> = [];
  public listaTipodeLancamento: Array<DualListModel> = [];
  public listaStatusPagamento: Array<DualListModel> = [];
  public listaCategoriaPagamento: Array<DualListTitulo> = [];
   

  public criteriosGeraisValid: boolean

  public manterDados = false;
  public manterProcessos : boolean;

  //selecionados
  // public empresasSelecionadas: Array<number> = [];
  // public fornecedoresSelecionados: Array<number> = [];
  // public escritoriosSelecionados: Array<number> = [];
  // public centrosSelecionados: Array<number> = [];
  // public lancamentossSelecionados: Array<number> = [];
  // public statusPagamentosSelecionados: Array<number> = [];
  public sapsSelecionados: List<number> = new List();
  public guiasSelecionadas: List<number> = new List();
  public processosSelecionados: List<any> = new List();
  public contaJudicialsSelecionadas: List<any> = new List();
  public dataInicioLote = new BehaviorSubject<Date>(null);
  public dataFimLote = new BehaviorSubject<Date>(null);
  public dataInicioPedido = new BehaviorSubject<Date>(null);
  public dataFimPedido = new BehaviorSubject<Date>(null);
  public auxEscritorios = [] //variavel para guardar os dados do escritorio no caso do envio completo(null)
  public auxEmpresaGrupo = []
  public auxFornecedor = []
  public auxCentroCusto = []
  public auxTipoLancamento = []
  public auxStatusPagamento = []
  public auxIdCategoriaDePagamentos = [];
  public filtro: LoteFiltroDTO = {
    dataCriacaoPedidoMaior: null,
    dataCriacaoPedidoMenor: null,
    dataCriacaoMaior: null,
    dataCriacaoMenor: null,
    tipoProcesso: null,
    statusContabil: null,
    statusProcesso: null,
    dataCancelamentoLoteInicio: null,
    dataCancelamentoLoteFim: null,
    dataErroProcessamentoInicio: null,
    dataErroProcessamentoFim: null,
    dataRecebimentoFiscalInicio: null,
    dataRecebimentoFiscalFim: null,
    dataPagamentoPedidoInicio: null,
    dataPagamentoPedidoFim: null,
    dataEnvioEscritorioInicio: null,
    dataEnvioEscritorioFim: null,
    valorTotalLoteInicio: null,
    valorTotalLoteFim: null,
    idsProcessos: [],
    idsEmpresasGrupo: [],
    idsEscritorios: [],
    idsFornecedores: [],
    idsCentroCustos: [],
    idsTipoLancamentos: [],
    idsCategoriasPagamentos: [],
    idsStatusPagamentos: [],
    idsNumerosGuia: [],
    idsNumerosLote: [],
    idsPedidosSAP: [],
    numeroContaJudicial: [],
    id: 0,
    pagina: 1,
    quantidade: 5,
    total: 0,
  };

  public tipoProcessoTracker = new BehaviorSubject<number>(null);
  public tipoProcessoAtual: number;



  LimparTipoProcesso() {
    this.tipoProcessoAtual = null;
    this.tipoProcessoTracker.next(this.tipoProcessoAtual);
  }

  updateTipoProcesso(tipoProcesso: number) {
    this.tipoProcessoAtual = tipoProcesso;
    this.tipoProcessoTracker.next(this.tipoProcessoAtual);
  }

  get nomeProcesso() {
    let r;
    TiposProcessosMapped.filter(i => i.idTipo === this.tipoProcessoTracker.value).map(n => r = n.nome); return r;
  }

  get nomeProcessoPlural() {
    let nomeProcesso;
    TiposProcessosCivelPluralMapped.filter(i => i.idTipo === this.tipoProcessoTracker.value).map(n => nomeProcesso = n.nome);
    return nomeProcesso;
  }

  public limpar() {

    if (!this.manterDados) {

      this.sapsSelecionados = new List();
      this.guiasSelecionadas = new List();
      this.processosSelecionados = new List();
      this.contaJudicialsSelecionadas = new List();
      this.dataInicioLote = new BehaviorSubject<Date>(null);
      this.dataFimLote = new BehaviorSubject<Date>(null);
      this.dataInicioPedido = new BehaviorSubject<Date>(null);
      this.dataFimPedido = new BehaviorSubject<Date>(null);
      this.auxCentroCusto = [];
      this.auxEmpresaGrupo = [];
      this.auxEscritorios = [];
      this.auxFornecedor = [];
      this.auxIdCategoriaDePagamentos = [];
      this.auxStatusPagamento = [];
      this.auxTipoLancamento = [];
      this.updateTipoProcesso(null);
      this.filtro = {
        dataCriacaoPedidoMaior: null,
        dataCriacaoPedidoMenor: null,
        dataCriacaoMaior: null,
        dataCriacaoMenor: null,
        tipoProcesso: null,
        statusContabil: null,
        statusProcesso: null,
        dataCancelamentoLoteInicio: null,
        dataCancelamentoLoteFim: null,
        dataErroProcessamentoInicio: null,
        dataErroProcessamentoFim: null,
        dataRecebimentoFiscalInicio: null,
        dataRecebimentoFiscalFim: null,
        dataPagamentoPedidoInicio: null,
        dataPagamentoPedidoFim: null,
        dataEnvioEscritorioInicio: null,
        dataEnvioEscritorioFim: null,
        valorTotalLoteInicio: null,
        valorTotalLoteFim: null,
        idsProcessos: [],
        idsEmpresasGrupo: [],
        idsEscritorios: [],
        idsFornecedores: [],
        idsCentroCustos: [],
        idsTipoLancamentos: [],
        idsCategoriasPagamentos: [],
        idsStatusPagamentos: [],
        idsNumerosGuia: [],
        idsNumerosLote: [],
        idsPedidosSAP: [],
        numeroContaJudicial: [],
        id: 0,
        pagina: 1,
        quantidade: 5,
        total: 0,
      };
      this.limparListarAoChamarNovamente();
      this.limparFiltros.next(true);
      this.limparContadores.next(true);
      this.LimparTipoProcesso();

    }

    this.manterDados = false;
  }

  public limparFiltroSemProcesso() {

    if (!this.manterDados) {

      this.sapsSelecionados = new List();
      this.guiasSelecionadas = new List();
      this.processosSelecionados = new List();
      this.contaJudicialsSelecionadas = new List();
      this.dataInicioLote = new BehaviorSubject<Date>(null);
      this.dataFimLote = new BehaviorSubject<Date>(null);
      this.dataInicioPedido = new BehaviorSubject<Date>(null);
      this.dataFimPedido = new BehaviorSubject<Date>(null);
      this.auxCentroCusto = [];
      this.auxEmpresaGrupo = [];
      this.auxEscritorios = [];
      this.auxFornecedor = [];
      this.auxIdCategoriaDePagamentos = [];
      this.auxStatusPagamento = [];
      this.auxTipoLancamento = [];
      this.filtro = {
        dataCriacaoPedidoMaior: null,
        dataCriacaoPedidoMenor: null,
        dataCriacaoMaior: null,
        dataCriacaoMenor: null,
        tipoProcesso: null,
        statusContabil: null,
        statusProcesso: null,
        dataCancelamentoLoteInicio: null,
        dataCancelamentoLoteFim: null,
        dataErroProcessamentoInicio: null,
        dataErroProcessamentoFim: null,
        dataRecebimentoFiscalInicio: null,
        dataRecebimentoFiscalFim: null,
        dataPagamentoPedidoInicio: null,
        dataPagamentoPedidoFim: null,
        dataEnvioEscritorioInicio: null,
        dataEnvioEscritorioFim: null,
        valorTotalLoteInicio: null,
        valorTotalLoteFim: null,
        idsProcessos: [],
        idsEmpresasGrupo: [],
        idsEscritorios: [],
        idsFornecedores: [],
        idsCentroCustos: [],
        idsTipoLancamentos: [],
        idsCategoriasPagamentos: [],
        idsStatusPagamentos: [],
        idsNumerosGuia: [],
        idsNumerosLote: [],
        idsPedidosSAP: [],
        numeroContaJudicial: [],
        id: 0,
        pagina: 1,
        quantidade: 5,
        total: 0,
      };
      //this.limparListarAoChamarNovamente();
      this.reiniciarListas()
      this.limparFiltros.next(true);
      this.limparContadores.next(true);


    }

    this.manterDados = false;
  }

  reiniciarListas() {
    this.listaEmpresa.map(obj => obj.selecionado = false);
    this.listaEscritorio.map(obj => obj.selecionado = false);
    this.listaTipodeLancamento.map(obj => obj.selecionado = false);
    this.listaCentroCusto.map(obj => obj.selecionado = false);
    this.listaFornecedor.map(obj => obj.selecionado = false);
    this.listaStatusPagamento.map(obj => obj.selecionado = false);
    this.listaCategoriaPagamento.map(obj => obj.selecionado = false);
  }



  limparListarAoChamarNovamente() {
    this.listaEmpresa = [];
    this.listaEscritorio = [];
    this.listaTipodeLancamento = [];
    this.listaCentroCusto = [];
    this.listaFornecedor = [];
    this.listaStatusPagamento = [];
    this.listaCategoriaPagamento = [];
  }


  format(date, h, m, s, ms): Date {
    if (date) {
      return new Date(date.year, date.month - 1, date.day, h, m, s, ms);
    } else {
      return null;
    }
  }

  private openModalSemResultado() {

    const swalButton = swal.mixin({
      customClass: {
        confirmButton: 'btn btn-primary'
      },
      buttonsStyling: false
    });
    swalButton.fire({
      html:
        'Nenhum resultado foi encontrado',
      showCloseButton: true,
      cancelButtonText:
        'OK '
    });
  }

  private nomeStatus(statusId) {
    if (statusId == 1) {
      return 'Ativo';
    } else if (statusId == 2) {
      return 'Inativo';
    } else {
      return 'Ativo e Inativo';
    }
  }


  get valoresExportacao() {
    let dataExport: Exportacao[] = [{ criterio: '', valor: [] }];


    dataExport = [
      {
        criterio: 'Tipo de Processo',
        valor: [this.nomeProcesso]
      },
      {
        criterio: 'Status Contábil',
        valor: [this.nomeStatusContabil]
      },
      {
        criterio: 'Status Processo',
        valor: [this.nomeStatusProcesso]
      },
      {
        criterio: 'Data Criação Lote',
        valor: this.dataCriacaoMenor ? [this.dataCriacaoMenor] : []
      },
      {
        criterio: 'Data Criação Pedido',
        valor: this.dataCriacaoPedidoMenor ? [this.dataCriacaoPedidoMenor] : []
      },
      {
        criterio: 'Data Cancelamento Lote',
        valor: this.dataCancelamentoLoteInicio ? [this.dataCancelamentoLoteInicio] : []
      },
      {
        criterio: 'Data Erro Processamento',
        valor: this.dataErroProcessamentoInicio ? [this.dataErroProcessamentoInicio] : []
      },
      {
        criterio: 'Data Recebimento Fiscal',
        valor: this.dataRecebimentoFiscalInicio ? [this.dataRecebimentoFiscalInicio] : []
      },
      {
        criterio: 'Data Pagamento Pedido',
        valor: this.dataPagamentoPedidoInicio ? [this.dataPagamentoPedidoInicio] : []
      },
      {
        criterio: 'Data de Envio para o Esritório',
        valor: this.dataEnvioEscritorioInicio ? [this.dataEnvioEscritorioInicio] : []
      },
      {
        criterio: 'Valor Total do Lote',
        valor: this.valorTotalLoteInicio ? [this.valorTotalLoteInicio] : []
      },];

    dataExport.push({
      criterio: 'Processos',
      valor: this.filtro.idsProcessos ? [this.filtro.idsProcessos] : null
    });
    dataExport.push({
      criterio: 'Empresas do Grupo',
      valor: this.nomeidsEmpresasGrupo
    });
    dataExport.push({
      criterio: 'Escritórios',
      valor: this.nomeidsEscritorios
    });
    dataExport.push({
      criterio: 'Fornecedores',
      valor: this.nomeidsFornecedores
    });
    dataExport.push({
      criterio: 'Centros de Custo',
      valor: this.nomeidsCentroCustos
    });
    dataExport.push({
      criterio: 'Tipos de Lançamento',
      valor: this.nomeidsTipoLancamentos
    });
    dataExport.push({
      criterio: 'Categorias de Pagamento',
      valor: this.nomeidsCategoriasPagamentos
    });
    dataExport.push({
      criterio: 'Status de Pagamento',
      valor: this.nomeidsStatusPagamentos
    });
    dataExport.push({
      criterio: 'Pedido SAP',
      valor: this.filtro.idsPedidosSAP ? [this.filtro.idsPedidosSAP] : null
    });
    dataExport.push({
      criterio: 'Número da Guia', 
      valor: this.filtro.idsNumerosGuia ? [this.filtro.idsNumerosGuia] : null
    });
    dataExport.push({
      criterio: 'Número do Lote',
      valor: this.filtro.idsNumerosLote ? [this.filtro.idsNumerosLote] : null
    });

    dataExport = dataExport.filter(item => item.valor.some(valor => valor.length > 0));

    return dataExport;
    
  }

  get nomeStatusContabil() {
    return this.nomeStatus(this.filtro.statusContabil);
  }

  get nomeStatusProcesso() {
    return this.nomeStatus(this.filtro.statusProcesso);
  }


  get dataCriacaoMenor() {
    if (this.filtro.dataCriacaoMenor) {
      return this.getEntreValoresString(new Date(this.resultadoService.filtros.dataCriacaoMenor),
        new Date(this.resultadoService.filtros.dataCriacaoMaior));
    }
  }

  get dataCancelamentoLoteInicio() {
    if (this.resultadoService.filtros.dataCancelamentoLoteInicio) {
      return this.getEntreValoresString(new Date(this.resultadoService.filtros.dataCancelamentoLoteInicio),
        new Date(this.resultadoService.filtros.dataCancelamentoLoteFinal));
    }
  }

  get dataErroProcessamentoInicio() {
    if (this.resultadoService.filtros.dataErroProcessamentoInicio) {
      return this.getEntreValoresString(new Date(this.resultadoService.filtros.dataErroProcessamentoInicio),
        new Date(this.resultadoService.filtros.dataErroProcessamentoFinal));
    }
  }

  get dataRecebimentoFiscalInicio() {
    if (this.resultadoService.filtros.dataRecebimentoFiscalInicio) {
      return this.getEntreValoresString(new Date(this.resultadoService.filtros.dataRecebimentoFiscalInicio),
        new Date(this.resultadoService.filtros.dataRecebimentoFiscalFim));
    }
  }

  get dataRecebimentoFiscalFim() {
    if (this.resultadoService.filtros.dataRecebimentoFiscalInicio) {
      return this.getEntreValoresString(new Date(this.resultadoService.filtros.dataRecebimentoFiscalInicio),
        new Date(this.resultadoService.filtros.dataRecebimentoFiscalFim));
    }
  }

  get dataPagamentoPedidoInicio() {
    if (this.resultadoService.filtros.dataPagamentoPedidoInicio) {
      return this.getEntreValoresString(new Date(this.resultadoService.filtros.dataPagamentoPedidoInicio),
        new Date(this.resultadoService.filtros.dataPagamentoPedidoFim));
    }
  }

  get dataEnvioEscritorioInicio() {
    if (this.resultadoService.filtros.dataEnvioEscritorioInicio) {
      return this.getEntreValoresString(new Date(this.resultadoService.filtros.dataEnvioEscritorioInicio),
        new Date(this.resultadoService.filtros.dataEnvioEscritorioInicio));
    }
  }

  get dataCriacaoPedidoMenor() {
    if (this.resultadoService.filtros.dataCriacaoPedidoMenor) {
      return this.getEntreValoresString(new Date(this.resultadoService.filtros.dataCriacaoPedidoMenor),
        new Date(this.resultadoService.filtros.dataCriacaoPedidoMaior));
    }
  }

  get valorTotalLoteInicio() {
    if (this.resultadoService.filtros.valorTotalLoteInicio) {
      return this.getEntreValoresString(null, null, this.resultadoService.filtros.valorTotalLoteInicio,
        this.resultadoService.filtros.valorTotalLoteFim);
    }
  }

  get nomeidsEmpresasGrupo() {

    if (this.auxEmpresaGrupo) {
      return this.getNomesDuallistSelecionados('listaEmpresa',
        this.auxEmpresaGrupo, false);
    }
  }
  get nomeidsEscritorios() {
    if (this.auxEscritorios) {
      return this.getNomesDuallistSelecionados('listaEscritorio',
        this.auxEscritorios, false);
    }
  }
  get nomeidsCentroCustos() {
    if (this.auxCentroCusto) {
      return this.getNomesDuallistSelecionados('listaCentroCusto',
        this.auxCentroCusto, false);
    }
  }
  get nomeidsTipoLancamentos() {
    if (this.auxTipoLancamento) {
      return this.getNomesDuallistSelecionados('listaTipodeLancamento',
        this.auxTipoLancamento, false);
    }
  }
  get nomeidsStatusPagamentos() {
    if (this.auxStatusPagamento) {
      return this.getNomesDuallistSelecionados('listaStatusPagamento',
        this.auxStatusPagamento, false);
    }
  }
  get nomeidsCategoriasPagamentos() {
    if (this.auxIdCategoriaDePagamentos) {
      return this.getNomesDuallistSelecionados('listaCategoriaPagamento',
        this.auxIdCategoriaDePagamentos, true);
    }
  }
  get nomeidsFornecedores() {
    if (this.resultadoService.filtros.idsFornecedores) {
      return this.getNomesDuallistSelecionados('listaFornecedor',
        this.resultadoService.filtros.idsFornecedores, false);
    }
  }



  getListaEmpresasGrupo(): Array<DualListModel> {
    return this.listaEmpresa;
  }
  getListaEscritorio(): Array<DualListModel> {
    return this.listaEscritorio;
  }
  getListaTipoLancamento(): Array<DualListModel> {
    return this.listaTipodeLancamento;
  }
  getListaCentroCusto(): Array<DualListModel> {
    this.listaCentroCusto.filter(item1 =>
      this.filtro.idsCentroCustos.includes(item1.id)).map(
        item => item.selecionado = true
      );
    return this.listaCentroCusto;
  }
  getListaFornecedor(): Array<DualListModel> {
    return this.listaFornecedor;
  }

  getListaStatusPagamento(): Array<DualListModel> {
    return this.listaStatusPagamento;
  }
  getListaCategoriaPagamento(): Array<DualListTitulo> {
    this.listaCategoriaPagamento.forEach(itens => {
      itens.dados.forEach(dados => {
        this.auxIdCategoriaDePagamentos.forEach(selecionados => {
          dados.id == selecionados ? dados.selecionado = true : true;
        })

      });
    })


    return this.listaCategoriaPagamento;
  }

  /**
     * Busca a lista de componente
     *
     * @param Lista: Nome da variavel da lista que receerá o valor do back
     * @param codigoTipoProcesso: codigo do tipo de processo selecionado
     * @param listaEndpoint: lista que está dentro do endpoint e será mapeada para a variavel
     */

  public GetListaComponet(codigoTipoProcesso: number): Observable<any> {

    return this.http.get(`/Lotes/CarregarFiltros?CodigoTipoProcesso=` + codigoTipoProcesso)

  }

  addFiltros(resposta, lista, variavel: DualListModel[]) {

    resposta.data[lista].map(val => {
      variavel.push({
        id: val.id,
        label: val.descricao,
        selecionado: false,
        marcado: false,
        somenteLeitura: false,
        ativo: val.ativo
      });
    });

  }

  addFiltrosTitulo(resposta, lista, variavel: DualListTitulo[]) {
    resposta.data[lista].map(val => {
      variavel.push({
        id: val.id,
        titulo: val.titulo,
        selecionado: false,
        marcado: false,
        somenteLeitura: true,

        dados: val.dados.map(dad => {
          let infoDados
          infoDados = {
            id: dad.id,
            descricao: dad.descricao,
            ativo: dad.ativo,
            selecionado: false,
            marcado: false,
            idPai: val.id

          }
          return infoDados
        }),
      })

    })
  }


  getFiltros(tipoProcesso) {
    this.GetListaComponet(tipoProcesso).subscribe(
      resposta => {
        this.limparListarAoChamarNovamente();
        this.addFiltros(resposta, 'listaEmpresaDoGrupo', this.listaEmpresa);
        this.addFiltros(resposta, 'listaCentroCusto', this.listaCentroCusto);
        this.addFiltros(resposta, 'listaEscritorio', this.listaEscritorio);
        this.addFiltros(resposta, 'listaFornecedor', this.listaFornecedor);
        this.addFiltros(resposta, 'listaTipodeLancamento', this.listaTipodeLancamento);
        this.addFiltros(resposta, 'listaStatusPagamento', this.listaStatusPagamento);
        this.addFiltrosTitulo(resposta, 'listaCategoriaPagamento', this.listaCategoriaPagamento);
      }
    )
  }


  getNomesDuallistSelecionados(lista, valores: any[], hastitulo: boolean): string[] {
    const componente = this;
    let teste = [];
    if (!hastitulo) {
      teste = componente[lista].filter(item => valores.includes(item.id))
        .map(nome => nome.label);
    }
    let list
    if (hastitulo) {
      componente[lista].forEach(item =>
        item.dados.forEach(dados => teste.push(dados))
      );

      teste = teste.filter(item => valores.includes(item.id)).map(nome => nome.descricao)
    }

    return teste;
  }

  getEntreValoresString(data1?: Date, data2?: Date,
    number1?: number, number2?: number) {
    if (data1 && data2) {
      let optionsLocale = { year: 'numeric', month: 'numeric', day: 'numeric' };
      return 'De ' + data1.toLocaleString('pt-BR', optionsLocale) + ' até ' +
        data2.toLocaleString('pt-BR', optionsLocale);
    }
    if (number1 && number2) {
      return money(number1) + ' Até' +
        money(number2);
    }
  }

  AddlistaEmpresas(lista: Array<number>) {
    this.filtro.idsEmpresasGrupo = lista;
    // this.empresasSelecionadas = lista;
  }

  AddlistaEscritorios(lista: Array<number>) {
    this.filtro.idsEscritorios = lista;
    // this.escritoriosSelecionados = lista;
  }

  AddlistaFornecedor(lista: Array<number>) {
    this.filtro.idsFornecedores = lista;
    // this.fornecedoresSelecionados = lista;
  }
  AddlistaLancamento(lista: Array<number>) {
    this.filtro.idsTipoLancamentos = lista;
    // this.lancamentossSelecionados = lista;
  }

  AddlistaCusto(lista: Array<number>) {
    this.filtro.idsCentroCustos = lista;
    // this.centrosSelecionados = lista;
  }

  AddlistaStatusPagamentos(lista: Array<number>) {
    this.filtro.idsStatusPagamentos = lista;
    // this.statusPagamentosSelecionados = lista;
  }

  addCategoriadePagamento() {
    this.auxIdCategoriaDePagamentos = []
    this.listaCategoriaPagamento.forEach(item => {

      item.dados.forEach(dados => {
        if (dados && dados.selecionado)
          this.auxIdCategoriaDePagamentos.push(dados.id);

      });
    });
    this.filtro.idsCategoriasPagamentos = this.auxIdCategoriaDePagamentos;
  }

  colocarNull() {
    this.filtro.idsCategoriasPagamentos = [];
  }



  chamarDTO() {
    //DEFAUT
    this.filtro.idsProcessos = [];
    this.filtro.idsPedidosSAP = [];
    this.filtro.idsNumerosGuia = [];
    this.filtro.numeroContaJudicial = [];
    //this.filtro.idsCategoriasPagamentos = []
    this.filtro.quantidade = 5;
    this.filtro.total = 0;
    this.filtro.pagina = 1;
    this.tipoProcessoTracker.pipe(take(1)).subscribe(processo => this.filtro.tipoProcesso = processo)

    this.processosSelecionados.ForEach(item => {
      if (item.id) {
        this.filtro.idsProcessos.push(item.id)
      }
    });

    this.sapsSelecionados.ForEach(item => {
      if (item) {
        this.filtro.idsPedidosSAP.push(item)
      }
    });

    this.guiasSelecionadas.ForEach(item => {
      if (item) {
        this.filtro.idsNumerosGuia.push(item)
      }
    });
    this.contaJudicialsSelecionadas.ForEach(item => {
      if (item) {
        this.filtro.numeroContaJudicial.push(item)
      }
    });

    if (this.criteriosGeraisValid) {
      this.resultadoService.getListaResultados(this.filtro).subscribe(data => {
        if (data.length > 0) {
          this.resultadoService.redirectToResultado(data);
        } else {
          this.openModalSemResultado();
        }
      });

    }

  }

}
