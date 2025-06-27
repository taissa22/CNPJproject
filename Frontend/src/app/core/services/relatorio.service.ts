import { BehaviorSubject, ReplaySubject } from 'rxjs';
import { User, ApiService, JwtService } from '..';
import { distinctUntilChanged } from 'rxjs/operators';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { DualListModel } from '../models/dual-list.model';
import { FiltroModel } from '../models/filtro.model';
import { CriteriosModel } from '../models/criterios-model';
import { civelConsumidor } from '../models/end-points.relatorios';

export class RelatorioService {
  private currentUserSubject = new BehaviorSubject<User>({} as User);
  public currentUser = this.currentUserSubject.asObservable().pipe(distinctUntilChanged());

  private isAuthenticatedSubject = new ReplaySubject<boolean>(1);
  public isAuthenticated = this.isAuthenticatedSubject.asObservable();


  public tituloRelatorio = '';

  private tipoRelatorio = '';

  // tslint:disable-next-line: new-parens
  public criterios: CriteriosModel = new CriteriosModel;

  public caminhoPagina = '';
  // LISTA DO SERVICO
  public dualList: Array<DualListModel> = [];
  // LISTA DA EMPRESA
  public listaEmpresa: Array<DualListModel> = [];
  // LISTA FILTRO
  public getListaFiltro: any[] = [];

  constructor(
    private router: Router,
    private apiService: ApiService,
    private http: HttpClient,
    private jwtService: JwtService
  ) {

  }
  private ListarFiltros() {
    return [
      { id: 1, titulo: 'Critérios Gerais', linkMenu: 'criterios', selecionado: false, ativo: true },
      { id: 2, titulo: 'Empresa do Grupo ', linkMenu: 'empresa', selecionado: false, ativo: false },
      { id: 3, titulo: 'Escritório', linkMenu: 'escritorios', selecionado: false, ativo: false }
    ];
  }
  private limpaListas() {
    // tslint:disable-next-line: new-parens
    this.criterios = new CriteriosModel;
    this.getListaFiltro = [];
    this.listaEmpresa = [];
    this.dualList = [];

  }
  private GetListaComponet(path: string, Lista: string) {
    const component = this;
    this.http.get(`${environment.api_url}${path}`).subscribe((resposta: any) => {

      component[Lista] = resposta.data.map(val => {
        return {
          id: val.id,
          label: val.descricao,
          selecionado: false,
          marcado: false
        };
      });
    });
  }

  iniciarRelatorioServico(servico: string): Array<FiltroModel> {
    this.getListaFiltro = this.ListarFiltros();
    this.tipoRelatorio = servico;
    switch (servico) {
      case 'Consumidor': {
        this.tituloRelatorio = 'Relatório Genérico Civel Consumidor';
        this.caminhoPagina = 'Relatório > Genérico > Civel Consumidor';
        this.getFiltroRelatorioCivel();
        return this.getListaFiltro;
      }
      case 'Estrategico': {
        this.tituloRelatorio = 'Relatório Genérico Civel Estratégico';
        this.caminhoPagina = 'Relatório > Genérico > Civel Estratégico';
        this.getFiltroRelatorioEstrategico();
        return this.getListaFiltro;
      }
      case 'procon': {
        this.tituloRelatorio = 'Relatório Genérico Procon';
        this.caminhoPagina = 'Relatório > Genérico > Procon';
        this.getFiltroRelatorioProcon();
        return this.getListaFiltro;
      }
      default: {

        break;
      }
    }
  }

  atualizaCountCriterios(counter: number) {
    this.getListaFiltro.map(item => {
      if (item.id === 1) {
        item.titulo = 'Critérios Gerais (' + counter + ') ';
        item.selecionado = true;
        if (counter === 0) {
          item.titulo = 'Critérios Gerais';
          item.selecionado = false;
        }
      }
    });
  }

  atualizaCountEscritorio(counter: number) {
    this.getListaFiltro.map(item => {
      if (item.id === 3) {
        item.titulo = 'Escritório (' + counter + ') ';
        item.selecionado = true;
        if (counter === 0) {
          item.titulo = 'Escritório';
          item.selecionado = false;
        }
      }
    });
  }

  atualizaCountEmpresaGrupo(counter: number) {
    this.getListaFiltro.map(item => {
      if (item.id === 2) {
        item.titulo = 'Empresa do grupo (' + counter + ') ';
        item.selecionado = true;
        if (counter === 0) {
          item.titulo = 'Empresa do grupo';
          item.selecionado = false;
        }
      }
    });
  }

  getFiltroRelatorioProcon(): Array<FiltroModel> {
    this.limpaListas();
    this.GetListaComponet('/RelatorioGenerico/RecuperaEscritorioProcon', 'dualList');
    this.GetListaComponet('/RelatorioGenerico/recuperaEmpresadoGrupo', 'listaEmpresa');
    this.getListaFiltro = this.ListarFiltros();

    return this.getListaFiltro;
  }

  getFiltroRelatorioEstrategico(): Array<FiltroModel> {
    this.limpaListas();
    this.GetListaComponet('/RelatorioGenerico/recuperaEscritorioAreaCivelEstrategico', 'dualList');
    this.GetListaComponet('/RelatorioGenerico/recuperaEmpresadoGrupo', 'listaEmpresa');
    this.getListaFiltro = this.ListarFiltros();

    return this.getListaFiltro;
  }

  getFiltroRelatorioCivel(): Array<FiltroModel> {
    this.GetListaComponet('/RelatorioGenerico/recuperaEscritorioCivelConsumidor', 'dualList');
    this.GetListaComponet('/RelatorioGenerico/recuperaEmpresadoGrupo', 'listaEmpresa');
    this.limpaListas();
    this.getListaFiltro = this.ListarFiltros();
    return this.getListaFiltro;
  }

  getListaEscritorios(): Array<DualListModel> {
    return this.dualList;
  }
  getListaEmpresasGrupo(): Array<DualListModel> {
    return this.listaEmpresa;
  }
  AddlistaEscritorios(lista: Array<number>) {
    this.criterios.listaEscritorios = lista;
  }
  AddlistaEmpresas(lista: Array<number>) {
    this.criterios.listaEmpresas = lista;
  }
}
