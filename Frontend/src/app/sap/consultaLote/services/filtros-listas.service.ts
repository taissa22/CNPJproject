// import { Injectable } from '@angular/core';
// import { List } from 'linqts';
// import { DualListModel } from 'src/app/core/models/dual-list.model';
// import { Observable } from 'rxjs';
// import { ApiService } from 'src/app/core';
// import { TipoProcessoService } from 'src/app/core/services/sap/tipo-processo.service';

// @Injectable({
//   providedIn: 'root'
// })
// export class FiltrosListasService {


//   // LISTA DA EMPRESA
//   public listaEmpresa: Array<DualListModel> = [];
//   public listaEscritorio: Array<DualListModel> = [];
//   public listaCentroCusto: Array<DualListModel> = [];
//   public listaFornecedor: Array<DualListModel> = [];
//   public listaTipodeLancamento: Array<DualListModel> = [];
//   public listaStatusPagamento: Array<DualListModel> = [];

//   public empresasSelecionadas: Array<number> = [];
//   public fornecedoresSelecionados: Array<number> = [];
//   public escritoriosSelecionados: Array<number> = [];
//   public centrosSelecionados: Array<number> = [];
//   public lancamentossSelecionados: Array<number> = [];
//   public statusPagamentosSelecionados: Array<number> = [];
//   public sapsSelecionados: List<string> = new List();
//   public guiasSelecionadas: List<string> = new List();
//   public processosSelecionados: List<string> = new List();
//   public contaJudicialsSelecionadas: List<string> = new List();

//   constructor( private http: ApiService, private tipoProcessoService: TipoProcessoService) { }

//   getListaEmpresasGrupo(): Array<DualListModel> {
//     return this.listaEmpresa;
//   }
//   getListaEscritorio(): Array<DualListModel> {
//     return this.listaEscritorio;
//   }
//   getListaTipoLancamento(): Array<DualListModel> {
//     return this.listaTipodeLancamento;
//   }
//   getListaCentroCusto(): Array<DualListModel> {
//     return this.listaCentroCusto;
//   }
//   getListaFornecedor(): Array<DualListModel> {
//     return this.listaFornecedor;
//   }

//   getListaStatusPagamento(): Array<DualListModel> {
//     return this.listaStatusPagamento;
//   }

// /**
//    * Busca a lista de componente
//    * @param codigoTipoProcesso: codigo do tipo de processo selecionado
//    *
//    */

//   public GetListaComponet(codigoTipoProcesso: number): Observable<any> {

//    return this.http.get(`/FiltroConsulta/CarregarFiltros?CodigoTipoProcesso=` + codigoTipoProcesso)

//   }

//   addFiltros(resposta , lista , variavel: DualListModel[]) {
//     resposta.data[lista].map(val => {
//       variavel.push( {
//             id: val.id,
//             label: val.descricao,
//             selecionado: false,
//             marcado: false,
//             somenteLeitura: false
//           });
//     });

//   }



//   getFiltros(tipoProcesso) {
//     // this.limpaListas();
//     this.GetListaComponet(tipoProcesso).subscribe(
//       resposta => {
//         this.addFiltros(resposta, 'listaEmpresaDoGrupo', this.listaEmpresa);
//         this.addFiltros(resposta, 'listaCentroCusto', this.listaCentroCusto);
//         this.addFiltros(resposta, 'listaEscritorio', this.listaEscritorio);
//         this.addFiltros(resposta, 'listaFornecedor', this.listaFornecedor);
//         this.addFiltros(resposta, 'listaTipodeLancamento', this.listaTipodeLancamento);
//         this.addFiltros(resposta, 'listaStatusPagamento', this.listaStatusPagamento);
//       }
//     )
//   }

//   AddlistaEmpresas(lista: Array<number>) {
//     this.empresasSelecionadas = lista;
//   }

//   AddlistaEscritorios(lista: Array<number>) {
//     this.escritoriosSelecionados = lista;
//   }

//   AddlistaFornecedor(lista: Array<number>) {
//     this.fornecedoresSelecionados = lista;
//   }
//   AddlistaLancamento(lista: Array<number>) {
//     this.lancamentossSelecionados = lista;
//   }

//   AddlistaCusto(lista: Array<number>) {
//     this.centrosSelecionados = lista;
//   }

//   AddlistaStatusPagamentos(lista: Array<number>) {
//     this.statusPagamentosSelecionados = lista;
//   }

//   limparListarAoChamarNovamente() {

//     this.listaEmpresa = [];
//     this.listaEscritorio = [];
//     this.listaTipodeLancamento = [];
//     this.listaCentroCusto = [];
//     this.listaFornecedor = [];
//     this.listaStatusPagamento = [];
//   }

// }
