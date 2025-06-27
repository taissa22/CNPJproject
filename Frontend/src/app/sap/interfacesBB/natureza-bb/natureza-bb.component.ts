import { Component, OnInit, OnDestroy } from '@angular/core';
import { ManutencaoCommonComponent } from '../../shared/components/manutencao-common-component';
import { BsModalService, BsModalRef } from 'ngx-bootstrap';
import { Subscription } from 'rxjs';
import { CrudNaturezaBBService } from './services/crud-naturezaBB.service';
import { NaturezaBBService } from './services/naturezaBB.service';
import { ModalAdicionarNaturezaBBComponent } from './modal-adicionar-naturezaBB/modal-adicionar-naturezaBB.component';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { role } from '../../sap.constants';

@Component({
  selector: 'app-natureza-bb',
  templateUrl: './natureza-bb.component.html',
  styleUrls: ['./natureza-bb.component.scss']
})
export class NaturezaBbComponent extends ManutencaoCommonComponent implements OnInit, OnDestroy {
  breadcrumb: string;

  constructor(private modalService: BsModalService,
    public service: NaturezaBBService,
    private crudService: CrudNaturezaBBService,
    private breadcrumbsService: BreadcrumbsService) {
    super(service);
  }

  description: { descricao: string } = { descricao: null };

  public bsModalRef: BsModalRef;
  subscription: Subscription;

  headerSemBusca = [ 'Código','Natureza Ação BB', 'Descrição'];

  ngOnInit() {
    this.defineModel({descricao: '', ordenacao: 'descricao', ascendente: true });
    this.subscription = this.crudService.naturezaBB.subscribe(data => this.setData(data));
  }

  async ngAfterViewInit(): Promise<void> {
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(role.menuInterfaceNaturezacaoAcoesBB);
  }
  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }


  adicionarNatureza(e) {
    this.crudService.addNatureza();
    this.bsModalRef = this.modalService.show(ModalAdicionarNaturezaBBComponent);
  }

  editarNatureza(Natureza) {
    this.crudService.editarNatureza(Natureza);
    this.bsModalRef = this.modalService.show(ModalAdicionarNaturezaBBComponent);
  }

  pesquisar(description: string = null) {
    this.description = { descricao: description.trim() };
    this.refresh({ descricao: description.trim(), ordenacao: 'descricao', ascendente: true });
  }
}
