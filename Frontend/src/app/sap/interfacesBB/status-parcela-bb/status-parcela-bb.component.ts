import { Component, OnDestroy, OnInit } from '@angular/core';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { BsModalService } from 'ngx-bootstrap';
import { Subscription } from 'rxjs';
import { role } from '../../sap.constants';
import { ManutencaoCommonComponent } from '../../shared/components/manutencao-common-component';
import { ModalAdicionarStatusParcelaComponent } from './modal-adicionar-status-parcela/modal-adicionar-status-parcela.component';
import { CrudStatusParcelaService } from './services/crud-status-parcela.service';
import { StatusParcelaBBService } from './services/status-parcela-bb.service';

@Component({
  selector: 'app-status-parcela-bb',
  templateUrl: './status-parcela-bb.component.html',
  styleUrls: ['./status-parcela-bb.component.scss']
})
export class StatusParcelaBBComponent extends ManutencaoCommonComponent implements OnInit, OnDestroy {
  breadcrumb: any;

  constructor(
    public service: StatusParcelaBBService,
    private modalService: BsModalService, 
    private crudService: CrudStatusParcelaService,
    private breadcrumbsService: BreadcrumbsService) {
    super(service);
  }

  description: { descricao: string } = { descricao: null };

  headerSemBusca = [  'Código',
  'Status Parcela BB',
 'Descrição'];

  dataSubscription: Subscription;

  ngOnInit() {
    this.defineModel({ descricao: '', ordenacao: 'descricao', ascendente: true });
    this.dataSubscription = this.crudService.statusParcela.subscribe(data => this.setData(data));
  }

  async ngAfterViewInit(): Promise<void> {
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(role.menuInterfaceStatusParcelaBB);
  }

  editarStatusParcelaBB(parcela) {
    this.crudService.editarParcela(parcela);
    this.modalService.show(ModalAdicionarStatusParcelaComponent)
  }

  adicionarStatusParcelaBB() {
    this.crudService.addParcela();
    this.modalService.show(ModalAdicionarStatusParcelaComponent);

  }
  ngOnDestroy(): void {
    this.dataSubscription.unsubscribe();
  }

  pesquisar(description: string = null) {
    this.description = { descricao: description.trim() };
    this.refresh({ descricao: description.trim() , ordenacao: 'descricao', ascendente: true });
  }

}
