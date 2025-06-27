import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { TipoProcesso } from 'src/app/core/models/tipo-processo';
import { take } from 'rxjs/operators';
import { CategoriaPagamentoService } from 'src/app/core/services/sap/categoria-pagamento.service';
import { OrdenacaoStatus } from '@shared/interfaces/ordenacao-status';
import { CategoriaPagemntoFiltroDto } from '@shared/interfaces/categoria-pagemnto-filtro-dto';
import { DownloadService } from 'src/app/core/services/sap/download.service';
import { removerAcentos } from '@shared/utils';
import { listaTipoProcesso, fornecedoresPermitidos, nomeExportacao, mapearIndicadoresTipoProcessoParaNumero } from '../categoria-pagamento.constant';
import { TipoProcessoService } from 'src/app/core/services/sap/tipo-processo.service';
import { ICategoriaPagamento } from '../../interface/ICategoriaPagamento';

@Injectable({
  providedIn: 'root'
})
export class ManutencaoCategoriaPagamentoService {

  //#region Subjects
  ordenacao = 'Descrição da Categoria de Pagamento';
  public fecharModal = new BehaviorSubject<boolean>(false);

  public onChangeCategorias = new BehaviorSubject<ICategoriaPagamento[]>([]);
  public onChangeFiltros = new BehaviorSubject<CategoriaPagemntoFiltroDto>({
    tipoProcesso: null,
    tipoLancamento: null,
    ordenacao: '',
    ascendente: true
  });

  //#region funcionalidades da grid
  public ordenacaoSubject = new BehaviorSubject<string>('');
  public ascendenteSubject = new BehaviorSubject<boolean>(true);
  public headerSubject = new BehaviorSubject<string>('');
  public ordenacaoActivitySubject = new BehaviorSubject<Array<OrdenacaoStatus>>([]); // TODO: verificar necessidade
  public selectedCategoriasSubject = new BehaviorSubject<ICategoriaPagamento[]>([]);
  public isDeleteCompleted = new BehaviorSubject<boolean>(false);
  public comboboxTipoProcessoSubject = new BehaviorSubject<TipoProcesso[]>([]);
  public comboboxTipoLancamentoSubject = new BehaviorSubject(listaTipoProcesso);
  public buscarLancamentosSubject = new BehaviorSubject([]);
  public currentValueComboLancamentoSubject = new BehaviorSubject<number>(null);
  public currentValueComboTipoProcessoSubject = new BehaviorSubject<any>(null);
  public parametrosBuscaSubject = new BehaviorSubject({
    tipoProcesso: null,
    tipoLancamento: null
  });
  //#endregion
  //#endregion

  constructor(private tipoProcessoService: TipoProcessoService,
    private downloadService: DownloadService,
  private categoriaPagamentoService : CategoriaPagamentoService) { }

  //#region API CALLERS
  getTipoProcesso() {
    this.tipoProcessoService.getTiposProcesso('manutencaoCategoriaPagamento')
      .pipe(take(1))
      .subscribe(response => this.setComboTipoProcesso(response));

    return this.comboboxTipoProcessoSubject;
  }

  //#region get
  get listaFornecedoresPermitidos() {
    return fornecedoresPermitidos;
  }

  get tipoProcessoSelecionado() {
    return this.currentValueComboTipoProcessoSubject.value;
  }

  /**
  * Retorna o nome do tipo de processo sem acento e sem espaço.
  */

  get nomeTipoProcessoSemEspaco() {
    const tipoProcesso = this.currentValueComboTipoProcessoSubject.value;
    let nome;
    this.comboboxTipoProcessoSubject.value.filter(i => i.id == tipoProcesso)
      .map(n => nome = n.descricao);
    nome = nome.replace(/\s/g, '');
    nome = removerAcentos(nome);
    return nome;

  }

  /**
   * Retorna o nome do tipo de lançamento sem o acento e sem o espaço.
   */
  get nomeTipoLancamentoSemEspaco() {
    const lancamento = this.currentValueComboLancamentoSubject.value;
    let nome;
    this.comboboxTipoLancamentoSubject.value.filter(i => i.id == lancamento)
      .map(n => nome = n.descricao);
    nome = nome.replace(/\s/g, '');
    nome = removerAcentos(nome);
    return nome;
  }

  //#endregion

  //#region Setters
  setComboTipoProcesso(tipoProcessoCombo: TipoProcesso[]) {
    this.comboboxTipoProcessoSubject.next(tipoProcessoCombo);
  }

  setBuscarLancamentos(buscarLancamentos) {
    this.buscarLancamentosSubject.next(buscarLancamentos);
  }

  setComboTipoLancamento(tipoLancamentoCombo: any[]) {
    this.comboboxTipoLancamentoSubject.next(tipoLancamentoCombo);
  }

  setCurrentValueComboLancamento(index) {
    this.currentValueComboLancamentoSubject.next(index);
  }

  get tipoLancamentoSelecionado() {
    return this.currentValueComboLancamentoSubject.value;
  }

  setCurrentValueComboTipoProcesso(index) {
    this.currentValueComboTipoProcessoSubject.next(index);
  }

  setParametrosBusca(params) {
    this.parametrosBuscaSubject.next(params);
  }
  //#endregion

  //#region Update Functions
  /**
   * Atualiza uma atividade do botão de ordenação
   * @param header Coluna do botão de ordenação
   * @param active Se deve estar ativo ou não
   */
  public updateOrdenacaoActivity(header: string, active: boolean) {
    const index = this.ordenacaoActivitySubject.value.findIndex(e => e.key == header);
    if (index == -1) {
      throw new Error('Header não encontrado na instância de ordenacaoActivitySubject');
    }
    const ordenacaoActivity = this.ordenacaoActivitySubject.value;
    ordenacaoActivity[index].isActive = active;
    this.ordenacaoActivitySubject.next(ordenacaoActivity);
  }

  public updateHeader(header: string) {
    this.headerSubject.next(header);
  }

  /**
   * Adiciona um novo item no subject "ordenacaoActivity"
   * @param obj Item à ser adicionado
   */
  public pushOrdenacaoActivity(obj: OrdenacaoStatus) {
    const currentOrdenacaoActivity = this.ordenacaoActivitySubject.value;
    currentOrdenacaoActivity.push(obj);
    this.ordenacaoActivitySubject.next(currentOrdenacaoActivity);
  }

  /**
   * Atualiza os categoriasPagamento e informa à todos os inscritos.
   * @param categoriasPagamento Lista de categoriasPagamento
   */
  private updateCategoriases(categoriasPagamento: ICategoriaPagamento[]) {
    this.onChangeCategorias.next(categoriasPagamento);
  }



  /**
   * Seleciona ou desseleciona um categoriaPagamento na grid.
   * @param categoriaPagamento Objeto categoriaPagamento
   * @param isSelected Se deve estar selecionado ou não.
   */
  public updateCategoriasSelection(categoriaPagamento, isSelected) {
    const CategoriasIndex = this.onChangeCategorias.value.findIndex(e => e.codigo == categoriaPagamento.id);
    const categoriasPagamento = this.onChangeCategorias.value;
    categoriasPagamento.forEach(categoriaPagamento => categoriaPagamento.selected = false);
    if (CategoriasIndex == -1) {
      throw Error('categoriaPagamento não encontrado durante a seleção.');
    }
    categoriasPagamento[CategoriasIndex].selected = isSelected;
    this.updateSelectedCategoriases(categoriasPagamento.filter(e => e.selected));
    this.updateCategoriases(categoriasPagamento);
  }

  /**
   * Atualiza a ordenação do campo
   * @param campo Campo à ser ordenado
   * @param isAscendente se deve ser em ordem crescente (true) ou decrescente
   */
  public updateOrdenacao(campo, isAscendente) {
    this.ordenacaoSubject.next(campo);
    this.ascendenteSubject.next(isAscendente);
  }

  private updateSelectedCategoriases(categoriasPagamento: ICategoriaPagamento[]) {
    this.selectedCategoriasSubject.next(categoriasPagamento);
  }


  /**
   * Atualiza os filtros da busca.
   * @param filtros filtros utilizados
   */
  private updateFiltros(filtros: CategoriaPagemntoFiltroDto) {
    this.onChangeFiltros.next(filtros);
  }

  popularComboQuandoFiltradoPeloCodigo(valores: ICategoriaPagamento){
    if(!this.currentValueComboLancamentoSubject.value
      && !this.currentValueComboTipoProcessoSubject.value){
        this.currentValueComboTipoProcessoSubject.next(mapearIndicadoresTipoProcessoParaNumero(
          {
           indicadorAdministrativo: valores.indicadorAdministrativo,
           indicadorCivelConsumidor: valores.indicadorCivelConsumidor,
           indicadorJuizado: valores.indicadorJuizado,
           indicadorCivelEstrategico: valores.indicadorCivelEstrategico,
           indicadorPex: valores.indicadorPex,
           indicadorProcon: valores.indicadorProcon,
           indicadorTrabalhista: valores.indicadorTrabalhista,
           ind_TributarioAdministrativo: valores.ind_TributarioAdministrativo,
           indicadorTributarioJudicial: valores.indicadorTributarioJudicial
          }
        ));
        this.currentValueComboLancamentoSubject.next(valores.tipoLancamento);
      }
  }



  /**
   * Resetta os BehaviorSubjects
   */
  public clearAllData() {
    this.updateFiltros({
      tipoProcesso: null,
      tipoLancamento: null,
      ordenacao: '',
      ascendente: true
    });
    this.updateCategoriases([]);
    this.updateOrdenacao('', true);

  }
  //#endregion

  codigo: number = null;

  //#region API Getters
  /**
   * Contata a API de categoriasPagamento e envia a lista de categoriasPagamento
   * à todos os inscritos. (Assíncrono)
   *
   * @param json: JSON à ser enviado para a API.
   */
  public getCategorias() {
    const parametrosBusca = {
      tipoLancamento: this.currentValueComboLancamentoSubject.value,
      tipoProcesso: this.currentValueComboTipoProcessoSubject.value,
      codigo: this.codigo
    };
    this.setParametrosBusca(parametrosBusca);

    this.updateFiltros({
      ...parametrosBusca,
      ordenacao: this.ordenacaoSubject.value,
      ascendente: this.ascendenteSubject.value
    });

    this.categoriaPagamentoService.buscarInformacoesCategoriaPagamento(this.onChangeFiltros.value)
      .pipe(take(1))
      .subscribe(response => {
        const categoriasPagamento: ICategoriaPagamento[] = response.data;

        categoriasPagamento.forEach(categoriaPagamento => {
          // tslint:disable-next-line: max-line-length
          categoriaPagamento.clgarCodigoClasseGarantia == 0 ? categoriaPagamento.clgarCodigoClasseGarantia = null : categoriaPagamento.clgarCodigoClasseGarantia;
          // tslint:disable-next-line: max-line-length
          categoriaPagamento.tipoFornecedorPermitido == 0 ? categoriaPagamento.tipoFornecedorPermitido = null : categoriaPagamento.tipoFornecedorPermitido;
          categoriaPagamento.codigoMaterialSAP == 0 ? categoriaPagamento.codigoMaterialSAP = null : categoriaPagamento.codigoMaterialSAP;
          categoriaPagamento.selected = false;
        });
        this.updateCategoriases(categoriasPagamento);
      });
    return this.onChangeCategorias.asObservable();
  }
  //#endregion

  public isOrdenacaoActive(header) {
    const activityObjectIndex = this.ordenacaoActivitySubject.value.findIndex(e => e.key == header);

    if (activityObjectIndex == -1) {
      return false;
    }
    const activityObject = this.ordenacaoActivitySubject.value[activityObjectIndex];
    if (activityObject) {
      return activityObject.isActive;
    }
    return false;
  }

  public deleteSelected(): boolean {
    this.clearSelected();
    return true;
  }

  private clearSelected() {
    this.selectedCategoriasSubject.next([]);
  }

  //#region Exportação de categoriasPagamento
  exportarCategorias() {
    // this.downloadService.baixarExportacaoCategoriases(filtros);
  }

  exportar() {
    this.downloadService.baixarExportacaoCategoriaPagamento(this.onChangeFiltros.value,
      nomeExportacao(this.tipoProcessoSelecionado, this.tipoLancamentoSelecionado));
  }
}
