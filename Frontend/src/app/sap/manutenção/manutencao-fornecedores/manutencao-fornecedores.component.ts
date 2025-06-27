import { FornecedorService } from 'src/app/core/services/sap/fornecedor.service';
import { ModalFornecedoresService } from './services/modal-fornecedores.service';
import { ManutencaoFornecedoresService } from './services/manutencao-fornecedores.service';
import { ActivatedRoute } from '@angular/router';
import { Subscription, BehaviorSubject } from 'rxjs';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Fornecedor } from '@shared/interfaces/fornecedor';
import { FornecedorFiltroLabelDTO } from '@shared/interfaces/fornecedor-filtro-label-dto';
import { BotaoGridState } from '@shared/interfaces/botao-grid-state';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { ModalAdicionarFornecedoresComponent } from './modal-adicionar-fornecedores/modal-adicionar-fornecedores.component';
import { take, map } from 'rxjs/operators';
import swal from 'sweetalert2';
import { HelperAngular } from '@shared/helpers/helper-angular';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { role } from '../../sap.constants';

@Component({
  selector: 'app-manutencao-fornecedores',
  templateUrl: './manutencao-fornecedores.component.html',
  styleUrls: ['./manutencao-fornecedores.component.scss']
})
export class ManutencaoFornecedoresComponent implements OnInit, OnDestroy {

  bsModalRef: BsModalRef;

  //#region Subscriptions
  subscription: Subscription;
  fornecedoresSubscription: Subscription;
  fornecedorSelecionado: Subscription;
  headerSemBusca = ["Nome do Fornecedor", 'Código', 'Código do Fornecedor SAP'
    , 'Tipo', 'Nome do Profissional', 'Banco', 'Escritório'];

  fornecedorObj: Fornecedor
  quantidadeItens;
  //#endregion

  //#region View variables
  fornecedores: Fornecedor[];
  fornecedoresKeys = [];
  isNotFound = false;
  pageMax = 0;
  itensMax: number;
  itensMin: number;
  itensTotal: number;
  breadcrumb: string;
  //#endregion


  constructor(private modalService: BsModalService,
    private route: ActivatedRoute,
    public service: ManutencaoFornecedoresService,
    private fornecedorService: FornecedorService,
    private modalFornecedoresService: ModalFornecedoresService,
    private msgService: HelperAngular,
    private breadcrumbsService: BreadcrumbsService) { }


  ngOnInit() {
    //this.fornecedorObj.selected = false;
    this.subscription = this.route.data.subscribe(info => {
      this.service.banco = info.banco;
      this.service.escritorios = info.escritorio;
      this.service.profissional = info.profissionais;
    });

    this.service.quantidadeSubject.subscribe(quantidadeAtual => {
      this.quantidadeItens = quantidadeAtual
    });

    this.fornecedoresSubscription = this.service.onChangeFornecedores.subscribe(fornecedores => this.fornecedores = fornecedores);
    this.fornecedorSelecionado = this.service.selectedFornecedoresSubject.subscribe(forn => this.fornecedorObj = forn[0])
  }

  async ngAfterViewInit(): Promise<void> {
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(role.menuManutencaoFornecedores);
  }

  /**
   * Atualiza a quantidade de itens da tabela.
   *
   * @param quantidade: Quantidade de itens da tabela
   */
  onChangeTableLength(quantidade: number) {
    this.service.updateQuantidade(quantidade);
    this.refreshFornecedores(this.service.onChangeFiltros.value);
  }

  /**
   * Checa se um botão está ativo
   * @param header Cabeçalho da tabela a qual o botão de ordenação faz parte. (usado como chave)
   */
  isButtonActive(header) {
    return this.service.isOrdenacaoActive(header);
  }

  /**
   * Reseta a p�gina atual para a p�gina zero
   */
  private resetCurrentPage() {
    this.service.updatePagina(1);
  }

  /**
   * Calcula o m�ximo de p�ginas permitido para o usu�rio acessar.
   */
  private calculateMaxPage() {
    this.pageMax = Math.ceil(this.service.totalSubject.value / this.service.quantidadeSubject.value);
  }

  onRowClick(fornecedor: Fornecedor) {
    if (fornecedor.selected)
      this.service.updateFornecedorSelection(fornecedor, false);
    else
      this.service.updateFornecedorSelection(fornecedor, true);
  }

  /**
   * Atualiza a lista de fornecedores com os fornecedores fornecidos pelo service.
   *
   * @param filtros: Filtros � serem utilizados durante a busca
   */
  refreshFornecedores(filtros: FornecedorFiltroLabelDTO) {
    this.service.getFornecedores(filtros)
      .subscribe(fornecedores => {
        if (fornecedores && fornecedores.length > 0) {
          this.fornecedoresKeys = Object.keys(fornecedores[0]).filter(e => (e != 'codigoBanco' &&
            e != 'codigoTipoFornecedor' &&
            e != 'codigoEscritorio' &&
            e != 'codigoProfissional'));

          this.fornecedoresKeys.forEach(key => {
            this.service.pushOrdenacaoActivity({
              key: key,
              isActive: false
            })
          });

          // Removendo o selected da grid
          this.fornecedoresKeys.splice(this.fornecedoresKeys.findIndex(e => e == 'selected'), 1);

          this.isNotFound = false;
        } else {
          this.isNotFound = true;
        }
        this.calculateMaxPage();
        this.atualizaItemCount();
      });

  }

  onClickExcluir() {
    if(this.fornecedorObj && this.fornecedorObj.selected) {
      swal.fire({
        title: 'Excluir Fornecedor',
        html: `Deseja excluir o Fornecedor <br> <b> ${this.fornecedorObj.nomeFornecedor}</b>?`,
        icon: 'question',
        confirmButtonColor: '#6F62B2',
        confirmButtonText: 'Sim',
        cancelButtonText: 'Não',
        cancelButtonColor: '#9597a6',
        showCancelButton: true,
        showConfirmButton: true
      }).then((result) => {
        if (result.value) {
          const selectedFornecedores = this.service.selectedFornecedoresSubject.value;
          // TODO: refatorar
          selectedFornecedores.forEach(fornecedorSelecionado => this.fornecedorService.excluirFornecedor(fornecedorSelecionado.id)
            .pipe(take(1))
            .subscribe(response => {
              if (!response.sucesso) {
                this.msgService.MsgBox2(response.mensagem, 'Exclusão não permitida!',
                'warning', 'Ok');
              } else {
                this.service.deleteSelected();

                if (this.service.onChangeFornecedores.value.length <= 1) {
                  this.service.paginaSubject.next(this.service.paginaSubject.value - 1)
                }

                this.refreshFornecedores(this.service.onChangeFiltros.value);
              }
            }));
        }
      });
    }
  }

  private atualizaItemCount() {
    this.service.updateItemCount();
    this.itensMin = this.service.minimoItensSubject.value;
    this.itensMax = this.service.maximoItensSubject.value;
    this.itensTotal = this.service.totalSubject.value;
  }

  /**
   * Chamado após o clique no botão "Buscar".
   * @param filtros Filtros à serem utilizados durante a busca
   */
  onBuscar(filtros: FornecedorFiltroLabelDTO) {
    this.service.clearAllData();
    this.refreshFornecedores(filtros);
    this.service.selectedFornecedoresSubject.next([]);
  }

  private updateBotaoOrdenacao(header: string) {
    const previousHeader = this.service.headerSubject.value;
    if (previousHeader) {
      this.service.updateOrdenacaoActivity(previousHeader, false)
    }
    this.service.updateHeader(header);
    this.service.updateOrdenacaoActivity(header, true);
  }

  /**
   * Altera a ordem da tabela de acordo com os campos passados.
   * @param header Campo da tabela
   * @param ordenacao Objeto contendo se est� ativo e se � ascendente.
   */
  onChangeOrdenacao(header: string, ordenacao: BotaoGridState) {
    const { isActive, ordemCrescente } = ordenacao;
    this.updateBotaoOrdenacao(header);
    this.service.updateOrdenacao(header, ordemCrescente);
    this.refreshFornecedores(this.service.onChangeFiltros.value);
  }

  /**
   * Altera a p�gina atual para a p�gina clicada.
   * @param page P�gina clicada da tabela
   */
  onPageClick(page: number) {
    this.service.updatePagina(page);
    this.refreshFornecedores(this.service.onChangeFiltros.value);
  }

  /**
     * Exportar fornecedor com filtro na página.
     */
  exportarFornecedor() {
    this.service.exportarFornecedor();
  }

  /**
   * Abrir modal de fornecedores
   * @param
   */
  abrirModalFornecedores(editar: boolean) {
    // TODO: refatorar para service
    //this.modalFornecedoresService.editar = editar;
    this.modalFornecedoresService.editarFornecedor.next(editar);
    if (editar && this.fornecedorObj && this.fornecedorObj.selected) {
      this.bsModalRef = this.modalService.show(ModalAdicionarFornecedoresComponent);
    } else if (!editar) {
      this.bsModalRef = this.modalService.show(ModalAdicionarFornecedoresComponent);
    }

  }

  ngOnDestroy(): void {
    //Called once, before the instance is destroyed.
    //Add 'implements OnDestroy' to the class.

  }

}
