import { ModalAdicionarOrgaoBBComponent } from './modal-adicionar-orgao-bb/modal-adicionar-orgao-bb.component';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { OrgaoBBService } from './services/orgao-bb.service';
import { ManutencaoCommonComponent } from '../../shared/components/manutencao-common-component';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { Subscription } from 'rxjs';
import { FiltroOrgaoServiceService } from './services/filtro-orgao-service.service';
import { ActivatedRoute } from '@angular/router';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { role } from '../../sap.constants';

@Component({
  selector: 'orgaoBB',
  templateUrl: './orgaoBB.component.html',
  styleUrls: ['./orgaoBB.component.scss']
})
export class OrgaoBBComponent extends ManutencaoCommonComponent implements OnInit, OnDestroy {
  breadcrumb: string;

  constructor(public service: OrgaoBBService,
    public filtroService: FiltroOrgaoServiceService,
    private modalService: BsModalService,
    private route: ActivatedRoute,
    private breadcrumbsService: BreadcrumbsService) {
    super(service);
  }

  config = {
    animated: false
  };

  public bsModalRef: BsModalRef;
  subscription: Subscription;
  orgaosBBSubscription: Subscription;
  public isNotFound = false;
  ngOnInit() {
    this.subscription = this.filtroService.orgaoBBSubject.subscribe(
      itens => {

        if (!itens || (itens.hasOwnProperty('data') && itens.data.length == 0)
          || Object.keys(itens).length <= 0) {
          this.isNotFound = true;
        } else {
          this.isNotFound = false;
          this.defineModel(itens.data);

        }
      }
    );
    this.orgaosBBSubscription = this.service.orgaosBB.subscribe(
      itens => {
        if (!itens || (itens.hasOwnProperty('data') && itens['data'].length == 0)) {
          this.isNotFound = true;
        } else {
          this.isNotFound = false;
          this.defineModel(itens['data']);

        }
      }
    );
    this.service.preencherValoresCombo(this.route);


  }

  async ngAfterViewInit(): Promise<void> {
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(role.menuInterfaceOrgaosBB);
  }

  setData(data: any[], isRefresh = false, colunasAExcluir?: any[]) {
    if (Array.isArray(data) && data.length > 0) {
      if (!isRefresh) {
        this.listaHeaders = Object.keys(data[0]).map(e => ({ campo: e, isActive: false, isAscendente: false }));
      }
      colunasAExcluir ? this.listaHeaders = this.listaHeaders.filter(item => !colunasAExcluir.includes(item.campo)) : '';
      this.data = data;
    } else {
      this.listaHeaders = [];
      this.data = [];
    }
  }

  excluirColuna = ['codigoBBTribunal', 'codigoBBComarca']

  defineModel(orgao) {
    this.setData(orgao, null, this.excluirColuna);
    this.setViewOrdenacao('nomeBBComarca');
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
    this.orgaosBBSubscription.unsubscribe();
  }

  adicionarOrgaoBB(_: any) {
    this.service.orgaoSelecionado = null;
    this.bsModalRef = this.modalService.show(ModalAdicionarOrgaoBBComponent, this.config);
  }

  editarOrgaoBB(orgão: any) {
    this.service.orgaoSelecionado = Object.assign({}, orgão);
    this.bsModalRef = this.modalService.show(ModalAdicionarOrgaoBBComponent, this.config);
  }
}

