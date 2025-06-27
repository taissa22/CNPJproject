import { Component, OnInit, OnDestroy } from '@angular/core';
import { ManutencaoFormasPagamentoService } from './services/manutencao-formas-pagamento.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { ModalFormasPagamentoComponent } from './modal-formas-pagamento/modal-formas-pagamento.component';
import { Subscription } from 'rxjs';
import { ManutencaoCommonComponent } from '../../shared/components/manutencao-common-component';
import { FormaPagamentoCrudService } from './services/FormaPagamentoCrudService.service';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { role } from '../../sap.constants';

@Component({
  selector: 'app-manutencao-formas-pagamento',
  templateUrl: './manutencao-formas-pagamento.component.html',
  styleUrls: ['./manutencao-formas-pagamento.component.scss']
})
export class ManutencaoFormasPagamentoComponent extends ManutencaoCommonComponent implements OnInit, OnDestroy{
  breadcrumb: string;

  constructor(public service: ManutencaoFormasPagamentoService,
              private modalService: BsModalService,
              private crudService: FormaPagamentoCrudService,
              private breadcrumbsService: BreadcrumbsService){
                super(service);
              }

  public bsModalRef: BsModalRef;


  subscription: Subscription;


  description: { descricao: string } = { descricao: null };

  headerSemBusca = ['Código', 'Descrição da Forma de Pagamento',
  'Requer Borderô', 'Restrita'];

  ngOnInit() {
    this.defineModel({ ordenacao: 'descricao', ascendente: true });
    this.subscription = this.crudService.valores.subscribe(data => {

      this.setData(data)
    }
    );
  }

  async ngAfterViewInit(): Promise<void> {
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(role.menuManutencaoFormaPagamento);
  }

  pesquisar(description: string = null) {
    this.description = { descricao: description };

    this.refresh({ descricao: description, ordenacao: 'descricao', ascendente: true });
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  adicionar() {
    this.crudService.adicionar();
    this.bsModalRef = this.modalService.show(ModalFormasPagamentoComponent);
  }

  editar(valor) {
    this.crudService.editar(valor);
    this.bsModalRef = this.modalService.show(ModalFormasPagamentoComponent);
  }

}
