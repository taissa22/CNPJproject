import { Subscription } from 'rxjs';
import { ordenateHeader } from '@shared/utils';
import { FornecedorEditarDto } from './../../../shared/interfaces/fornecedor-editar-dto';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { FornecedoresContingenciaSapService, fornecedoresHeaderOrdenada } from './services/fornecedores-contingencia-sap.service';
import { ManutencaoCommonComponent } from '../../shared/components/manutencao-common-component';
import { BsModalService, BsModalRef } from 'ngx-bootstrap';
import { FiltroFornecedoresContingenciaSapService } from './services/filtro-fornecedores-contingencia-sap.service';
import { ModalEditarFornecedoresContingenciaSapComponent } from './modal-editar-fornecedores-contingencia-sap/modal-editar-fornecedores-contingencia-sap.component';
import { CrudFornecedoresContingenciaSapService } from './services/crud-fornecedores-contingencia-sap.service';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { role } from '../../sap.constants';

@Component({
  selector: 'manutencao-fornecedores-contingencia-sap',
  templateUrl: './manutencao-fornecedores-contingencia-sap.component.html',
  styleUrls: ['./manutencao-fornecedores-contingencia-sap.component.scss']
})
export class ManutencaoFornecedoresContingenciaSapComponent
  extends ManutencaoCommonComponent implements OnInit, OnDestroy {
  breadcrumb: string;

  constructor(public service: FornecedoresContingenciaSapService,
    private modalService: BsModalService,
    public filtroService: FiltroFornecedoresContingenciaSapService,
    private crudService: CrudFornecedoresContingenciaSapService,
    private breadcrumbsService: BreadcrumbsService) {
    super(service);
  }
  public bsModalRef: BsModalRef;
  dataObj;
  // ----------------------------------- ATENÇÃO ------------------------------------!!!
  // Essa tela de manutenção somente possui a Edição, a adição desse fornecedor é
  // feita diretamente no banco, não possuindo CLI interativa do usuário.
  // É necessário que o fornecedor possua o tipo de fornecedor '4' para aparecer na grid.

  isNotFound = false;

  filtroSubscription: Subscription;
  filtroCrudSubscription: Subscription;
  ngOnInit() {

    this.filtroSubscription = this.filtroService.fornecedoresSubject.subscribe(itens => {
      if ((!itens || (itens.hasOwnProperty('data') && itens.data.length == 0)
        || itens && Object.keys(itens).length <= 0) && this.filtroService.buscar.value) {
        this.isNotFound = true;
        this.data = []
      }
      else {
        this.isNotFound = false;
        this.defineModel(itens.data);
      }
    });

    this.filtroCrudSubscription = this.crudService.fornecedorSubject.subscribe(itens => {
      if ((!itens || (itens.hasOwnProperty('data') && itens['data'].length == 0)
        || itens && Object.keys(itens).length <= 0) && this.filtroService.buscar.value) {
        this.isNotFound = true;
        this.data = []
      } else {
        this.isNotFound = false;
        this.defineModel(itens['data']);
      }
    });
  }

  async ngAfterViewInit(): Promise<void> {
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(role.menuManutencaoFornecedoresContingencia);
  }

  ngOnDestroy() {
    this.filtroSubscription.unsubscribe();
    this.filtroCrudSubscription.unsubscribe();
    this.filtroService.buscar.next(false);
  }

  editarFornecedores(fornecedor: any) {
    this.service.fornecedoresSelecionado.next(Object.assign({}, fornecedor));
    this.bsModalRef = this.modalService.show(ModalEditarFornecedoresContingenciaSapComponent);
  }

  defineModel(data) {
    this.setData(data, false);
  }

  setData(data: any[], isRefresh = false) {
    if (Array.isArray(data) && data.length > 0) {
        if (!isRefresh) {
            this.listaHeaders = ordenateHeader(Object.keys(data[0]), fornecedoresHeaderOrdenada)
                                      .filter(e => !this.headersToRemove.includes(e))
            .map(e => ({ campo: e, isActive: false, isAscendente: false }));




        }
        this.data = data;
    } else {
        this.listaHeaders = [];
        this.data = [];
    }
}

  onRowClick(index) {
    const estadoAnterior = this.data[index].selected;
    this.data.forEach(e => e.selected = false);
    this.data[index].selected = !estadoAnterior;

    if (this.data[index].selected) {
      this.dataObj = this.data[index];
    } else {
      this.dataObj = null;
    }

  }

}
