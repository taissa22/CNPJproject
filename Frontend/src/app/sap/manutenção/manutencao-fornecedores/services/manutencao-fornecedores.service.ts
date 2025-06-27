import { HelperAngular } from './../../../../shared/helpers/helper-angular';
import { CadastrarFornecedorDTO } from '@shared/interfaces/cadastrar-fornecedor-dto';
import { Injectable, Output, EventEmitter } from '@angular/core';
import { Fornecedor } from '@shared/interfaces/fornecedor';
import { BehaviorSubject, throwError } from 'rxjs';
import { FornecedorFiltroDTO } from '@shared/interfaces/fornecedor-filtro-dto';
import { FornecedorService } from 'src/app/core/services/sap/fornecedor.service';
import { take } from 'rxjs/operators';
import { FornecedorFiltroLabelDTO } from '@shared/interfaces/fornecedor-filtro-label-dto';
import { DownloadService } from 'src/app/core/services/sap/download.service';
import { OrdenacaoStatus } from '@shared/interfaces/ordenacao-status';
import swal from 'sweetalert2';
import { EmpresasSapDTO } from '@shared/interfaces/empresas-sap-dto';
import { FornecedorEditarDto } from '@shared/interfaces/fornecedor-editar-dto';


@Injectable({
  providedIn: 'root'
})
export class ManutencaoFornecedoresService {



  //mofal fornecedores



  //#region Subjects

  public fecharModal = new BehaviorSubject<boolean>(false)

  //#region onChangeEmitters
  public onChangeFornecedores = new BehaviorSubject<Fornecedor[]>([]);
  public onChangeFiltros = new BehaviorSubject<FornecedorFiltroDTO>({
    codigoEscritorio: 0,
    codigoTipoFornecedor: 0,
    codigoProfissional: 0,
    codigoBanco: 0,
    nomeFornecedor: '',
    codigoFornecedorSAP: '',
    nomeTipoFornecedor: '',
    nomeEscritorio: '',
    nomeProfissional: '',
    nomeBanco: '',
    pagina: 1,
    quantidade: 8,
    total: 0,
    ordenacao: '',
    ascendente: true
  });
  //#endregion

  //#region Behavior Trackers
  public paginaSubject = new BehaviorSubject<number>(1);
  public quantidadeSubject = new BehaviorSubject<number>(8);
  public totalSubject = new BehaviorSubject<number>(0);
  public ordenacaoSubject = new BehaviorSubject<string>('');
  public ascendenteSubject = new BehaviorSubject<boolean>(true);
  public minimoItensSubject = new BehaviorSubject<number>(0);
  public maximoItensSubject = new BehaviorSubject<number>(0);
  public headerSubject = new BehaviorSubject<string>('');
  public ordenacaoActivitySubject = new BehaviorSubject<Array<OrdenacaoStatus>>([]); //TODO: verificar necessidade
  public selectedFornecedoresSubject = new BehaviorSubject<Fornecedor[]>([]);
  public isDeleteCompleted = new BehaviorSubject<boolean>(false);

  //#endregion


  //#endregion Subjects

  public banco: any[]
  public escritorios: any[]
  public profissional: any[]

  //#region Var declaration
  //#endregion

  constructor(private fornecedorService: FornecedorService,
    private downloadService: DownloadService,
    private msgService: HelperAngular) { }

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
   * Atualiza os fornecedores e informa à todos os inscritos.
   * @param fornecedores Lista de fornecedores
   */
  private updateFornecedores(fornecedores: Fornecedor[]) {
    this.onChangeFornecedores.next(fornecedores);
  }


  private clearFornecedores() {
    this.onChangeFornecedores.next([]);
  }

  /**
   * Seleciona ou desseleciona um fornecedor na grid.
   * @param fornecedor Objeto fornecedor
   * @param isSelected Se deve estar selecionado ou não.
   */
  public updateFornecedorSelection(fornecedor, isSelected) {
    const fornecedorIndex = this.onChangeFornecedores.value.findIndex(e => e.id == fornecedor.id);
    let fornecedores = this.onChangeFornecedores.value;
    fornecedores.forEach(fornecedor => fornecedor['selected'] = false);
    if (fornecedorIndex == -1) {
      throw Error('Fornecedor não encontrado durante a seleção.');
    }
    fornecedores[fornecedorIndex]['selected'] = isSelected;
    this.updateSelectedFornecedores(fornecedores.filter(e => e['selected']));
    this.updateFornecedores(fornecedores);
  }


  /**
   * Atualiza a quantidade de itens por página atual.
   * @param quantidade Quantidade de itens por página
   */
  public updateQuantidade(quantidade: number) {
    this.quantidadeSubject.next(quantidade);
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

  private updateSelectedFornecedores(fornecedores: Fornecedor[]) {
    this.selectedFornecedoresSubject.next(fornecedores);
  }


  /**
   * Atualiza os filtros da busca.
   * @param filtros filtros utilizados
   */
  private updateFiltros(filtros: FornecedorFiltroDTO) {
    this.onChangeFiltros.next(filtros);
  }


  /**
   * Atualiza a página atual
   * @param pagina página atual
   */
  public updatePagina(pagina: number) {
    this.paginaSubject.next(pagina);
  }

  /**
   * Atualiza o total de itens da grid
   * @param total Quantidade total vinda da API
   */
  public updateTotal(total: number) {
    this.totalSubject.next(total);
  }

  /**
   * Atualiza a quantidade de itens da página atual.
   */
  public updateItemCount() {
    const currentPage = this.paginaSubject.value;

    // Valor máximo de itens na página
    const currentItemMax = currentPage * this.quantidadeSubject.value;

    // Valor mínimo de itens na página
    const currentItemMin = currentItemMax - this.quantidadeSubject.value;

    this.minimoItensSubject.next(currentItemMin);
    this.maximoItensSubject.next(currentItemMax);
  }

  /**
   * Resetta os BehaviorSubjects
   */
  public clearAllData() {
    this.updateFiltros({
      codigoEscritorio: 0,
      codigoTipoFornecedor: 0,
      codigoProfissional: 0,
      codigoBanco: 0,
      nomeFornecedor: '',
      codigoFornecedorSAP: '',
      nomeTipoFornecedor: '',
      nomeEscritorio: '',
      nomeProfissional: '',
      nomeBanco: '',
      pagina: 1,
      quantidade: this.quantidadeSubject.value,
      total: 0,
      ordenacao: '',
      ascendente: true
    });
    this.updateFornecedores([]);
    this.updateOrdenacao('', true);
    this.updatePagina(1);
  //  this.updateQuantidade(8);
    this.updateTotal(0);
  }
  //#endregion

  //#region API Getters
  /**
   * Contata a API de fornecedores e envia a lista de fornecedores
   * à todos os inscritos. (Assíncrono)
   *
   * @param json: JSON à ser enviado para a API.
   */
  public getFornecedores(dadosFiltros: FornecedorFiltroLabelDTO) {
    this.updateFiltros({
      ...dadosFiltros,
      pagina: this.paginaSubject.value,
      quantidade: this.quantidadeSubject.value,
      total: this.totalSubject.value,
      ordenacao: this.ordenacaoSubject.value,
      ascendente: this.ascendenteSubject.value
    });



    this.fornecedorService.getFornecedores(this.onChangeFiltros.value)
      .pipe(take(1))
      .subscribe(response => {
        const fornecedores = response['data'];
        fornecedores.forEach(fornecedor => fornecedor.selected = false);
        this.updateTotal(response['total']);
        this.updateFornecedores(fornecedores);
      });
    return this.onChangeFornecedores.asObservable();
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
    this.selectedFornecedoresSubject.next([]);
  }

  //#region Exportação de fornecedores
  exportarFornecedor() {
    const filtros = this.onChangeFiltros.value;
    this.downloadService.baixarExportacaoFornecedores(filtros);
  }

  public cadastrarFornecedores(cadFornecedor: CadastrarFornecedorDTO) {

    //this.fecharModal.next(true);

    this.fornecedorService.cadastrarFornecedor(cadFornecedor).pipe(take(1)).subscribe(response => {
      if (response.sucesso) {
        this.getFornecedores(this.onChangeFiltros.value).pipe(take(1)).subscribe();
        this.fecharModal.next(true);
      } else if (response.data.confirmacaoEnvio) {
        swal.fire({
          title: 'Atenção!',
          html: response.mensagem,
          icon: 'question',
          confirmButtonColor: '#6F62B2',
          confirmButtonText: 'Sim',
          cancelButtonText: 'Não',
          cancelButtonColor: '#9597a6',
          showCancelButton: true,
          showConfirmButton: true
        }).then((result) => {
          if (result.value) {
            cadFornecedor.criarCodigoFornecedorSAP = true;
            this.cadastrarFornecedores(cadFornecedor);
            this.fecharModal.next(true);
          }
        });
      } else {
        this.msgService.MsgBox(response.mensagem, 'Ops!');
        this.fecharModal.next(false);
      }
    })

  }

  editarFornecedor(json: FornecedorEditarDto) {
    //const func = () => 'a'
    this.fornecedorService.editarFornecedor(json).pipe(take(1)).subscribe(response => {
      if (response.sucesso) {
        this.getFornecedores(this.onChangeFiltros.value).pipe(take(1)).subscribe();
        this.fecharModal.next(true);

      } else if (response.data.confirmacaoEnvio) {
        swal.fire({
          title: 'Atenção!',
          html: response.mensagem,
          icon: 'question',
          confirmButtonColor: '#6F62B2',
          confirmButtonText: 'Sim',
          cancelButtonText: 'Não',
          cancelButtonColor: '#9597a6',
          showCancelButton: true,
          showConfirmButton: true
        }).then((result) => {
          if (result.value) {
            json.criarCodigoFornecedorSAP = true;
            this.editarFornecedor(json);
            this.fecharModal.next(true);
          }
        });
      } else {
        this.msgService.MsgBox(response.mensagem, 'Ops!');
        this.fecharModal.next(false);
      }
    })
  }


}
  //#endreg/ion

