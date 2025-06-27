import { DownloadService } from 'src/app/core/services/sap/download.service';
import { Subscription } from 'rxjs';
import { ManutencaoCommonComponent } from './../../shared/components/manutencao-common-component';
import { Component, OnInit } from '@angular/core';
import { BsModalService, BsModalRef } from 'ngx-bootstrap';
import { ManutencaoCentroCustoService } from './service/manutencao-centro-custo.service';

import { ModalAdicionarCentrosCustoComponent } from './modal-adicionar-centros-custo/modal-adicionar-centros-custo.component';
import { CentroCustoCrudService } from './service/centro-custo-crud.service';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { role } from '../../sap.constants';

@Component({
  selector: 'app-manutencao-centro-custo',
  templateUrl: './manutencao-centro-custo.component.html',
  styleUrls: ['./manutencao-centro-custo.component.scss']
})
export class ManutencaoCentroCustoComponent extends ManutencaoCommonComponent implements OnInit {
  breadcrumb: string;



  constructor(private modalService: BsModalService,
              public service: ManutencaoCentroCustoService,
              private crudService: CentroCustoCrudService,
              private downloadService: DownloadService,
              private breadcrumbsService: BreadcrumbsService) {
                super(service);
              }
  public bsModalRef: BsModalRef;


   /** Coloca a coluna padrão da tabela */
   headerSemBusca = ['Código', 'Descrição do Centro Custo', 'Centro Custo SAP', 'Ativo'];


   subscription: Subscription;


   description: { descricao: string } = { descricao: null };

   ngOnInit() {
    this.defineModel({ ordenacao: 'descricao', ascendente: true });
    this.subscription = this.crudService.valores.subscribe(data => {

      this.setData(data)
    }
    );
  }

  async ngAfterViewInit(): Promise<void> {
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(role.menuManutencaoCentroCusto);
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
    this.bsModalRef = this.modalService.show(ModalAdicionarCentrosCustoComponent);
  }

  editar(valor) {
    this.crudService.editar(valor);
    this.bsModalRef = this.modalService.show(ModalAdicionarCentrosCustoComponent);
  }




  // //#region Views
  // public centroCustoKeys: string[] = [];
  // public centrosCusto = [];
  // //#endregion

  // itensTotal: number;
  // itensMax: number;
  // itensMin: number;

  // //#region Conditional vars
  // public isNotFound = false;
  // //#endregion

  // titulo = 'Adicionar centroCustos do SAP';

  // pageMax = 0;

  // ngOnInit() {
  //   this.service.quantidadeSubject.next(8);
  //   this.refreshcentrosCusto()
  //     .subscribe(newcentrosCusto => {
  //       this.definecentrosCusto(newcentrosCusto)
  //     });
  // }

  // ngOnDestroy(): void {
  //   //Called once, before the instance is destroyed.
  //   //Add 'implements OnDestroy' to the class.
  //   this.service.ordenacaoActivitySubject.next([])
  //   this.service.ordenacaoSubject.next(null)
  // }

  // definecentrosCusto(newCentrosCusto) {
  //   if (newCentrosCusto && newCentrosCusto.length > 0) {
  //     this.centrosCusto = newCentrosCusto;
  //     this.centroCustoKeys = Object.keys(newCentrosCusto[0])
  //       .filter(item => item != 'selected');
  //     this.centroCustoKeys.forEach(key => {
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
  //   this.refreshcentrosCusto();
  // }

  // private updateBotaoOrdenacao(header: string) {
  //   const previousHeader = this.service.headerSubject.value;
  //   if (previousHeader) {
  //     this.service.updateOrdenacaoActivity(previousHeader, false);
  //   }
  //   this.service.updateHeader(header);
  //   this.service.updateOrdenacaoActivity(header, true);
  // }

  // refreshcentrosCusto() {
  //   return this.service.getCentroCusto();
  // }

  // adicionarcentroCustoSAP(event) {
  //   this.centroCustoCrud.addCategoria();
  //    this.bsModalRef = this.modalService.show(ModalAdicionarCentrosCustoComponent);
  // }

  // excluirCentroCusto(centroCusto) {

  //   this.centroCustoCrud.excluirCentroCustoSap(centroCusto);
  // }



  // editarCentroCusto(centroCusto) {
  //   this.service.selectedCentroCustoSubject.next(centroCusto);
  //   this.centroCustoCrud.editarCentroCusto(centroCusto);
  //   this.bsModalRef = this.modalService.show(ModalAdicionarCentrosCustoComponent);
  // }

  // exportarcentrosCusto() {
  //   this.downloadService.baixarExportacaoCentroCustoSap();
  // }

  // /**
  //  * Altera a p�gina atual para a p�gina clicada.
  //  * @param page P�gina clicada da tabela
  //  */
  // onPageClick(page: number) {
  //   this.service.updatePagina(page);
  //   this.refreshcentrosCusto();
  // }

  // /**
  //  * Atualiza a quantidade de itens da tabela.
  //  *
  //  * @param quantidade: Quantidade de itens da tabela
  //  */
  // onChangeTableLength(quantidade: number) {
  //   this.service.updateQuantidade(quantidade);
  //   this.resetCurrentPage();
  //   this.refreshcentrosCusto();
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
