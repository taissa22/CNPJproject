import { Subscription } from 'rxjs';
import { ManutencaoCommonComponent } from './../../shared/components/manutencao-common-component';
import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { BotaoGridState } from '@shared/interfaces/botao-grid-state';
import { ComarcaBBService } from './service/comarcaBB.service';
import { DownloadService } from 'src/app/core/services/sap/download.service';
import { ComarcaBBCrudService } from './service/comarcaBBCrud.service';
import { ModalAdicionarComarcaBBComponent } from './modal-adicionar-comarcaBB/modal-adicionar-comarcaBB.component';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { role } from '../../sap.constants';

@Component({
  selector: 'app-interfacesBB-comarcaBB',
  templateUrl: './interfacesBB-comarcaBB.component.html',
  styleUrls: ['./interfacesBB-comarcaBB.component.scss']
})
export class InterfacesBBComarcaBBComponent extends ManutencaoCommonComponent implements OnInit {
  breadcrumb: string;

  constructor(public service: ComarcaBBService,
    private downloadService: DownloadService,
    private crudService: ComarcaBBCrudService,
    private modalService: BsModalService,
    private breadcrumbsService: BreadcrumbsService
  ) {super(service); }

  public bsModalRef: BsModalRef;


  /** Coloca a coluna padrão da tabela */
  headerSemBusca = ['id', 'codigoBB', 'codigoEstado', 'descricao'];



  subscription: Subscription;


  description: { descricao: string } = { descricao: null };

  ngOnInit() {
   this.headersToRemove = ['confirmaCardastro'];
   this.ordemColunas = ['id', 'codigoBB', 'codigoEstado', 'descricao'];
   this.defineModel({ ordenacao: 'descricao', ascendente: true });



   this.subscription = this.crudService.valores.subscribe(data => {

     this.setData(data)
   }
   );
 }

 async ngAfterViewInit(): Promise<void> {
	//this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(Permissoes.ACESSAR_JUROS);
	this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(role.menuInterfaceComarcarBB);
}

 pesquisar(description: string = null) {
   this.description = { descricao: description };
   this.refresh({ descricao: description, ordenacao: 'descricao', ascendente: true });
 }

 ngOnDestroy(): void {
   this.subscription.unsubscribe();
 }

 adicionar(e) {
   this.crudService.adicionar();
   this.bsModalRef = this.modalService.show(ModalAdicionarComarcaBBComponent);
 }

 editar(valor) {
   this.crudService.editar(valor);
   this.bsModalRef = this.modalService.show(ModalAdicionarComarcaBBComponent);
 }







  // public bsModalRef: BsModalRef;
  // //#region Views
  // public comarcaBBKeys: string[] = [];
  // public comarcaBB = [];
  // //#endregion

  // itensTotal: number;
  // itensMax: number;
  // itensMin: number;

  // //#region Conditional vars
  // public isNotFound = false;
  // //#endregion

  // titulo = 'Adicionar comarcaBBs do SAP';

  // pageMax = 0;

  // ngOnInit() {
  //   this.service.quantidadeSubject.next(8);
  //   this.refreshComarcaBB()
  //     .subscribe(newcomarcaBB => {
  //       this.defineComarcaBB(newcomarcaBB);
  //     });
  // }
  // ngOnDestroy(): void {
  //   //Called once, before the instance is destroyed.
  //   //Add 'implements OnDestroy' to the class.
  //   this.service.ordenacaoActivitySubject.next([])
  //   this.service.ordenacaoSubject.next(null)

  // }


  // defineComarcaBB(newcomarcaBB) {
  //   if (newcomarcaBB && newcomarcaBB.length > 0) {
  //     this.comarcaBB = newcomarcaBB;
  //     this.comarcaBBKeys = ["id", "codigoBB", "codigoEstado", "descricao"];
  //     this.comarcaBBKeys.forEach(key => {
  //       this.service.pushOrdenacaoActivity({ key,
  //                                            isActive: false });
  //     });

  //     this.isNotFound = false;
  //   } else {
  //     this.isNotFound = true;
  //   }
  //   this.atualizaItemCount();
  //   this.calculateMaxPage();
  // }

  // /**
  //  * Calcula o m�ximo de p�ginas permitido para o usu�rio acessar.
  //  */
  // private calculateMaxPage() {
  //   this.pageMax = Math.ceil(this.service.totalSubject.value / this.service.quantidadeSubject.value);
  // }

  // isButtonActive(header) {
  //   return this.service.isOrdenacaoActive(header);
  // }

  // onChangeOrdenacao(header: string, ordenacao: BotaoGridState) {
  //   const { isActive, ordemCrescente } = ordenacao;
  //   this.updateBotaoOrdenacao(header);
  //   this.service.updateOrdenacao(header, ordemCrescente);
  //   this.refreshComarcaBB();
  // }

  // private updateBotaoOrdenacao(header: string) {
  //   const previousHeader = this.service.headerSubject.value;
  //   if (previousHeader) {
  //     this.service.updateOrdenacaoActivity(previousHeader, false);
  //   }
  //   this.service.updateHeader(header);
  //   this.service.updateOrdenacaoActivity(header, true);
  // }

  // refreshComarcaBB() {
  //   return this.service.getComarcaBB();
  // }

  // adicionarComarcaBB(event) {
  //   this.comarcaBBCrud.addComarca();
  //   this.bsModalRef = this.modalService.show(ModalAdicionarComarcaBBComponent);
  // }

  // excluirComarcaBB(comarcaBB) {

  //   this.comarcaBBCrud.excluirComarcaBB(comarcaBB);
  // }



  // editarComarcaBB(comarcaBB) {
  //   this.service.selectedComarcaBBSubject.next(comarcaBB);
  //   this.comarcaBBCrud.editarComarcaBB(comarcaBB);
  //   this.bsModalRef = this.modalService.show(ModalAdicionarComarcaBBComponent);
  // }

  // exportarcomarcaBB() {
  //   this.downloadService.baixarExportacaoComarcaBB();
  // }

  // /**
  //  * Altera a p�gina atual para a p�gina clicada.
  //  * @param page P�gina clicada da tabela
  //  */
  // onPageClick(page: number) {
  //   this.service.updatePagina(page);
  //   this.refreshComarcaBB();
  // }

  // /**
  //  * Atualiza a quantidade de itens da tabela.
  //  *
  //  * @param quantidade: Quantidade de itens da tabela
  //  */
  // onChangeTableLength(quantidade: number) {
  //   this.service.updateQuantidade(quantidade);
  //   this.resetCurrentPage();
  //   this.refreshComarcaBB();
  // }

  // /**
  //  * Reseta a p�gina atual para a p�gina zero
  //  */
  // private resetCurrentPage() {
  //   this.service.updatePagina(1);
  // }

  // private atualizaItemCount() {
  //   this.service.updateItemCount();
  //   this.itensMin = this.service.minimoItensSubject.value;
  //   this.itensMax = this.service.maximoItensSubject.value;
  //   this.itensTotal = this.service.totalSubject.value;
  // }
}
