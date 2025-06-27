import { getEntreValores } from '@shared/utils';
import { UserService } from '@core/services';
import { IVara } from './../interface/iVara';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { ProcessosTrabalhistaService } from './processosTrabalhista.service';
import { FiltroJson } from '../interface/FiltroJson';
import { Combobox } from '@shared/interfaces/combobox';
import { DualListModel } from 'src/app/core/models/dual-list.model';
import { Filter } from '@shared/interfaces/Filter';
import { FilterOperatorEnum } from '@shared/enums/FilterOperator.enum';
import { SearchCommand } from '@shared/interfaces/SearchCommand';
import { paginaInicial, quantidadePaginainicial } from 'src/app/app.constants';
import { AgendaAudienciaService } from 'src/app/core/services/processos/AgendaAudiencia/AgendaAudiencia.service';
import { IAgendaProcessoResultadoPesquisar } from '@shared/interfaces/IAgendaProcessoPesquisar';
import { Router } from '@angular/router';
import { HelperAngular } from './../../../../shared/helpers/helper-angular';
import { defaultSortOrder } from '../trabalhista-filtros.constants';
import { pluck } from 'rxjs/operators';
import { getNomesDuallistSelecionados } from '../../../../shared/utils';

@Injectable({
  providedIn: 'root'
})
export class TrabalhistaFiltrosService {

  json: FiltroJson = this.limparFiltro();

  listaAdvogadosCompleta: any;

  buscaInvalida = false;

  manter = false;

  varaIds: string[]
  varaNomes: IVara[]
  allEscritorio = false
  allEscritorioAcompanhante = false
  allAdvogado = false
  allAdvogadoAcompanhante = false
  allPreposto = false
  allPrepostoAcompanhante = false
  allEstado = false
  allComarca = false
  allEmpresaGrupo = false



  // listaAdvogado: any[]
  // listaAdvogadoAcompanhante: any[]

  criterios = {
    estrategico: "",
    periodoPendenciaCalculo: "",
    dataAudiencia: "",
    classificacaoHierarquica: "",
    idsAdvogados: [],
    idsAdvogadosAcompanhantes: [],
    codComarca: [],
    idsEmpresaGrupo: [],
    siglaEstado: [],
    idsPreposto: [],
    idsPrepostoAcompanhante: [],
    idsEscritoriosAudiencias: [],
    considerarFiltro: "",
    idsEscritoriosAcompanhantes: [],
    vara: [],
    codProcesso: [],
    classificacaoClosing : "",
  }

  ordemCriterios: String[] = [
    'estrategico',
    'periodoPendenciaCalculo',
    'dataAudiencia',
    'classificacaoHierarquica',
    'idsAdvogados',
    'idsAdvogadosAcompanhantes',
    'codComarca',
    'idsEmpresaGrupo',
    'idsEscritoriosAudiencias',
    'considerarFiltro',
    'idsEscritoriosAcompanhantes',
    'siglaEstado',
    'idsPreposto',
    'idsPrepostoAcompanhante',
    'codProcesso',
    'vara',
    'classificacaoClosing'
  ]

  listaEscritorios: Array<DualListModel> = [];
  listaAdvogados: Array<DualListModel> = [];
  listaComarca: Array<DualListModel> = [];
  listaEmpresaGrupo: Array<DualListModel> = [];
  listaEstado: Array<DualListModel> = [];
  listaPreposto: Array<DualListModel> = [];



  constructor(private processoFiltroService: ProcessosTrabalhistaService,
    private router: Router,
    private apiService: AgendaAudienciaService,
    private messageService: HelperAngular,
    private userService: UserService) { }

  // listaAdvogados: DualListModel[]

  limparFiltro(): FiltroJson {

    if (!this.manter) {
      this.varaIds = [];
      this.varaNomes = [];
      this._listaFiltro = this._listaFiltroDefault;
      this.processoFiltroService.limpar();
      return this.json = {
        estrategico: "3",
        periodoPendenciaCalculoFim: null,
        periodoPendenciaCalculoInicio: null,
        dataAudienciaInicio: null,
        dataAudienciaFim: null,
        classificacaoHierarquica: "4",
        advogadoAudiencia: [],
        advogadoAcompanhante: [],
        codComarca: [],
        empresaGrupo: [],
        siglaEstado: [],
        preposto: [],
        prepostoAcompanhante: [],
        escritorioAudiencia: [],
        escritorioAcompanhante: [],
        vara: [],
        codProcesso: [],
        codEscritorioAudiencia: "1",
        tipoEscritorio: null,
        classificacaoClosing: "4"
      }
    }
    this.manter = true;
  }

  btnLimparFiltro() {
    this.manter = false;
    this.varaIds = [];
    this.varaNomes = [];
    // this._listaFiltro = this._listaFiltroDefault;
    this.processoFiltroService.limpar();
    this.json = {
      estrategico: "3",
      periodoPendenciaCalculoFim: null,
      periodoPendenciaCalculoInicio: null,
      dataAudienciaInicio: null,
      dataAudienciaFim: null,
      classificacaoHierarquica: "4",
      advogadoAudiencia: [],
      advogadoAcompanhante: [],
      codComarca: [],
      empresaGrupo: [],
      siglaEstado: [],
      preposto: [],
      prepostoAcompanhante: [],
      escritorioAudiencia: [],
      escritorioAcompanhante: [],
      vara: [],
      codProcesso: [],
      codEscritorioAudiencia: "1",
      tipoEscritorio: "1",
      classificacaoClosing: "4"
    }
  }

  transformDataToIsoString(dateInicio: Date, dateFim: Date) {
    return dateInicio
      && dateInicio.toISOString().substr(0, 10)
      + ';'
      + dateFim.toISOString().substr(0, 10);
  }

  get convertJsonToFilterFormat(): Array<Filter> {

    let escritorio = this.getusuario()

    if (escritorio.ehEscritorio) {

      if (this.json.escritorioAudiencia.length == 0 && this.json.escritorioAcompanhante.length == 0) {
        this.listaEscritorios.filter(dados => this.json.escritorioAudiencia = [...this.json.escritorioAudiencia, dados.id])
      }
      // if(this.json.escritorioAcompanhante.length == 0){
      //   this.listaEscritoriosAcompanhantes.filter(dados => this.json.escritorioAcompanhante = [...this.json.escritorioAcompanhante, dados.id])
      // }
      // if(this.json.advogadoAudiencia.length == 0){
      //   this.listaAdvogados.filter(dados => this.json.advogadoAudiencia = [...this.json.advogadoAudiencia, dados.id])
      // }
      // if(this.json.advogadoAcompanhante.length == 0){
      //   this.listaAdvogadoAcompanhante.filter(dados => this.json.advogadoAcompanhante = [...this.json.advogadoAcompanhante, dados.id])
      // }

    }


    var convertedFilter: Array<Filter> = [
      { fieldName: 'estrategico', value: this.json.estrategico, filterOperator: FilterOperatorEnum.EqualsTo },
      { fieldName: 'periodoPendenciaCalculo', value: this.transformDataToIsoString(this.json.periodoPendenciaCalculoInicio, this.json.periodoPendenciaCalculoFim), filterOperator: FilterOperatorEnum.Between },
      { fieldName: 'dataAudiencia', value: this.transformDataToIsoString(this.json.dataAudienciaInicio, this.json.dataAudienciaFim), filterOperator: FilterOperatorEnum.Between },
      { fieldName: 'classificacaoHierarquica', value: this.json.classificacaoHierarquica, filterOperator: FilterOperatorEnum.EqualsTo },
      { fieldName: 'advogadoAudiencia', value: this.json.advogadoAudiencia.map((item) => item['id']).join(';'), value2: this.json.advogadoAudiencia.map((item) => item['codigoChave']).join(';'),  filterOperator: FilterOperatorEnum.In },
      { fieldName: 'advogadoAcompanhante', value: this.json.advogadoAcompanhante.map((item) => item['id']).join(';'), value2:this.json.advogadoAcompanhante.map((item) => item['codigoChave']).join(';') , filterOperator: FilterOperatorEnum.In },
      { fieldName: 'codComarca', value: this.json.codComarca.join(';'), filterOperator: FilterOperatorEnum.In },
      { fieldName: 'empresaGrupo', value: this.json.empresaGrupo.join(';'), filterOperator: FilterOperatorEnum.In },
      { fieldName: 'siglaEstado', value: this.json.siglaEstado.join(';'), filterOperator: FilterOperatorEnum.In },
      { fieldName: 'preposto', value: this.json.preposto.join(';'), filterOperator: FilterOperatorEnum.In },
      { fieldName: 'prepostoAcompanhante', value: this.json.prepostoAcompanhante.join(';'), filterOperator: FilterOperatorEnum.In },
      {
        fieldName: 'escritorioAudiencia', value: this.json.escritorioAudiencia.join(';'),
        filterOperator: this.json.tipoEscritorio == "1" ? FilterOperatorEnum.In_Escritorio_Audiencia : FilterOperatorEnum.In_Escritorio_Processo
      },
      { fieldName: 'escritorioAcompanhante', value: this.json.escritorioAcompanhante.join(';'), filterOperator: FilterOperatorEnum.In },
      { fieldName: 'vara', value: this.json.vara.join(';'), filterOperator: FilterOperatorEnum.EqualsTo },
      { fieldName: 'codProcesso', value: this.processoFiltroService.listaProcessos$.value.map(item => item['Codigo Interno']).join(';'), filterOperator: FilterOperatorEnum.In },
      { fieldName: 'classificacaoClosing', value: this.json.classificacaoClosing, filterOperator: FilterOperatorEnum.EqualsTo },
    ];


    this.preencherCriterios()

    return convertedFilter.filter(item => item.value != "" && item.value != null);

  }

  preencherCriterios() {
    switch (this.json.estrategico) {
      case "1":
        this.criterios.estrategico = 'Sim'
        break;

      case "2":
        this.criterios.estrategico = 'Não'
        break;

      default:
        this.criterios.estrategico = 'Ambos'
        break;
    }

    switch (this.json.classificacaoClosing) {
      case "1":
        this.criterios.classificacaoClosing = 'Pré'
        break;

      case "2":
        this.criterios.classificacaoClosing = 'Pós'
        break;
      
      case "3":
        this.criterios.classificacaoClosing = 'Híbrido'
        break;

      default:
        this.criterios.classificacaoClosing = 'Indiferente'
        break;
    }

    this.criterios.periodoPendenciaCalculo = getEntreValores(this.json.periodoPendenciaCalculoInicio, this.json.periodoPendenciaCalculoFim)
    this.criterios.dataAudiencia = getEntreValores(this.json.dataAudienciaInicio, this.json.dataAudienciaFim)

    switch (this.json.classificacaoHierarquica) {
      case "1":
        this.criterios.classificacaoHierarquica = 'Único'
        break;

      case "2":
        this.criterios.classificacaoHierarquica = 'Primário'
        break;

      case "3":
        this.criterios.classificacaoHierarquica = 'Secundário'
        break;

      default:
        this.criterios.classificacaoHierarquica = 'Todos'
        break;
    }


    this.criterios.idsAdvogados = getNomesDuallistSelecionados(this.listaAdvogados, this.json.advogadoAudiencia, false, this.allAdvogado, true)
    this.criterios.idsAdvogadosAcompanhantes = getNomesDuallistSelecionados(this.listaAdvogados, this.json.advogadoAcompanhante, false, this.allAdvogadoAcompanhante, true)
    this.criterios.idsEscritoriosAudiencias = getNomesDuallistSelecionados(this.listaEscritorios, this.json.escritorioAudiencia, false, (!this.getusuario().ehEscritorio && this.allEscritorio))
    this.criterios.idsEscritoriosAcompanhantes = getNomesDuallistSelecionados(this.listaEscritorios, this.json.escritorioAcompanhante, false, this.allEscritorioAcompanhante)

    if (this.criterios.idsEscritoriosAudiencias) {
      switch (this.json.tipoEscritorio) {
        case "1":
          this.criterios.considerarFiltro = 'Escritório da Audiência'
          break;
  
        case "2":
          this.criterios.considerarFiltro = 'Escritório do Processo'
          break;
        default:
          break;
      }
    }

    this.criterios.codComarca = getNomesDuallistSelecionados(this.listaComarca, this.json.codComarca, false, this.allComarca)
    this.criterios.idsEmpresaGrupo = getNomesDuallistSelecionados(this.listaEmpresaGrupo, this.json.empresaGrupo, false , this.allEmpresaGrupo)
    this.criterios.siglaEstado = getNomesDuallistSelecionados(this.listaEstado, this.json.siglaEstado, false, this.allEstado)
    this.criterios.idsPreposto = getNomesDuallistSelecionados(this.listaPreposto, this.json.preposto, false, this.allPreposto)
    this.criterios.idsPrepostoAcompanhante = getNomesDuallistSelecionados(this.listaPreposto, this.json.prepostoAcompanhante, false, this.allPrepostoAcompanhante)

    this.criterios.codProcesso = this.processoFiltroService.listaProcessos$.value.map(item => 'Número de Processo: ' + item['N° do Processo'] + ' Código Interno: ' + item['Codigo Interno']);

    this.criterios.vara = this.varaNomes.map(item => 'Estado: ' + item['estado'] + ' Comarca: ' + item['comarca'] + ' Vara: ' + item['vara'] + ' Tipo Vara: ' + item['tipoVara'])


  }

  valoresTabela$ = new BehaviorSubject<IAgendaProcessoResultadoPesquisar>(
    { dado: [], totalElementos: 0 });

  buscar() {
    var json: SearchCommand = {
      filters: this.convertJsonToFilterFormat,
      sortOrders: defaultSortOrder.map(item => {
        return ({
          direction: item.direction,
          property: item.property
        })
      }),
      pageNumber: paginaInicial,
      pageSize: quantidadePaginainicial,
      isExportMethod: false
    }
    this.apiService.pesquisar(json).subscribe(
      item => {
        if (item.dado.length > 0) {
          this.valoresTabela$.next(item);
          this.router.navigate(['/agenda/agendaAudienncia/trabalhista-resultado'])
        }
        else {
          this.messageService.MsgBox2('Não foi encontrada nenhuma audiência agendada.',
            'Atenção!', 'warning', 'Ok');
        }
      }

    );
  };

  private get _listaFiltroDefault() {
    return [
      {
        id: 1,
        titulo: 'Critérios Gerais',
        linkMenu: 'criteriosGuia',
        selecionado: false,
        ativo: true,
        marcado: false,
        tituloPadrao: 'Critérios Gerais'
      },
      {
        id: 2,
        titulo: 'Advogado',
        linkMenu: 'advogadoGuia',
        selecionado: false,
        ativo: false,
        marcado: false,
        tituloPadrao: 'Advogado'
      },
      {
        id: 3,
        titulo: 'Advogado Acompanhante',
        linkMenu: 'advogadoAcompanhanteGuia',
        selecionado: false,
        ativo: false,
        marcado: false,
        tituloPadrao: 'Advogado Acompanhante'
      },
      {
        id: 4,
        titulo: 'Comarca',
        linkMenu: 'comarcaGuia',
        selecionado: false,
        ativo: false,
        marcado: false,
        tituloPadrao: 'Comarca'
      },
      {
        id: 5,
        titulo: 'Empresa do Grupo',
        linkMenu: 'empresaDoGrupoGuia',
        selecionado: false,
        ativo: false,
        marcado: false,
        tituloPadrao: 'Empresa do Grupo'
      },
      {
        id: 6,
        titulo: 'Escritório',
        linkMenu: 'escritorioGuia',
        selecionado: false,
        ativo: false,
        marcado: false,
        tituloPadrao: 'Escritório'
      },
      {
        id: 7,
        titulo: 'Escritório Acompanhante',
        linkMenu: 'escritorioAcompanhanteGuia',
        selecionado: false,
        ativo: false,
        marcado: false,
        tituloPadrao: 'Escritório Acompanhante'
      },
      {
        id: 8,
        titulo: 'Estado',
        linkMenu: 'estadoGuia',
        selecionado: false,
        ativo: false,
        marcado: false,
        tituloPadrao: 'Estado'
      },
      {
        id: 9,
        titulo: 'Preposto',
        linkMenu: 'prepostoGuia',
        selecionado: false,
        ativo: false,
        marcado: false,
        tituloPadrao: 'Preposto'
      },
      {
        id: 10,
        titulo: 'Preposto Acompanhante',
        linkMenu: 'prepostoAcompanhanteGuia',
        selecionado: false,
        ativo: false,
        marcado: false,
        tituloPadrao: 'Preposto Acompanhante'
      },
      {
        id: 11,
        titulo: 'Processos',
        linkMenu: 'processosGuia',
        selecionado: false,
        ativo: false,
        marcado: false,
        tituloPadrao: 'Processos'
      },
      {
        id: 12,
        titulo: 'Vara',
        linkMenu: 'varaGuia',
        selecionado: false,
        ativo: false,
        marcado: false,
        tituloPadrao: 'Vara'
      }
    ]
  }

  private _listaFiltro = this._listaFiltroDefault;


  public ListarFiltros() {
    return this._listaFiltro;
  }

  get contagemProcessos$(): BehaviorSubject<number> {
    return this.processoFiltroService.contagemSubject$
  }

  /** Atualiza a contagem de selecionados no filtro
   * @param counter Valor da quantidade atual de selecionados em numero
   * @param id Id na listaFiltro do filtro ao qual deve ser modificado com o contador
   */
  atualizarContagem(counter: number, id: number): void {

    if (!id) {
      this._listaFiltro.map(item => {
        item.titulo = item.tituloPadrao;
        item.selecionado = false;
      });
    } else {
      this._listaFiltro.map(item => {
        if (item.id === id) {
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

  /** Filtros estavam vindo com ID e Descricao e precisamos de ID e Label */
  transformToDualList(filtro: Array<Combobox>): DualListModel[] {

    return filtro.map(item => {
      return {
        id: item.id,
        label: item.descricao.toUpperCase(),
        codigoChave: item.codigoInterno ? item.codigoInterno : null
      };
    });
  }

  // transformToDualListString(filtro: Array<{ id: string, descricao: string }>): Array<{ id: string, label: string }> {

  //   return filtro.map(item => {
  //     return {
  //       id: item.descricao.toUpperCase(),
  //       label: item.descricao.toUpperCase()
  //     };
  //   });
  // }

  atualizarContagemVara() {
    this.atualizarContagem(this.varaIds.length, 12);

  }

  setListaEscritorios(lista) {
    this.listaEscritorios = lista
  }

  setListaAdvogados(lista) {
    this.listaAdvogados = lista
  }

  setListaComarca(lista) {
    this.listaComarca = lista
  }

  setListaEmpresaGrupo(lista) {
    this.listaEmpresaGrupo = lista
  }

  setListaestado(lista) {
    this.listaEstado = lista
  }

  setListaPreposto(lista) {
    this.listaPreposto = lista
  }


  getusuario() {
    return this.userService.getCurrentUser();
  }

  getCriterios() {
    return this.criterios;
  }


}
