import { Component, OnInit, OnDestroy } from '@angular/core';
import { ManutencaoCommonComponent } from '../../shared/components/manutencao-common-component';
import { BsModalService, BsModalRef } from 'ngx-bootstrap';
import { TribunalBBService } from '../tribunaisBB/services/tribunalBBService.service';
import { CrudTribunalBbService } from '../tribunaisBB/services/crud-tribunal-bb.service';
import { Subscription } from 'rxjs';
import { ModalidadeProdutoBbService } from './services/modalidade-produto-bb.service';
import { CrudModalidadeBBService } from './services/crudModalidadeBB.service';
import { ModalAdicionarModalidadeBbComponent } from './modal-adicionar-modalidade-bb/modal-adicionar-modalidade-bb.component';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { role } from '../../sap.constants';

@Component({
  selector: 'app-modalidade-produto-bb',
  templateUrl: './modalidade-produto-bb.component.html',
  styleUrls: ['./modalidade-produto-bb.component.scss']
})
export class ModalidadeProdutoBbComponent extends ManutencaoCommonComponent implements OnInit, OnDestroy {
  breadcrumb: string;

  constructor(private modalService: BsModalService,
              public service: ModalidadeProdutoBbService,
              private crudService: CrudModalidadeBBService,
              private breadcrumbsService: BreadcrumbsService) {
    super(service);
  }

  public bsModalRef: BsModalRef;
  subscription: Subscription;
  headerSemBusca = [
    'Código',
    'Modalidade BB',
    'Descrição'
  ];


  ngOnInit() {
    this.defineModel({ ordenacao: 'descricao', ascendente: true });
    this.subscription = this.crudService.modalidadeBB.subscribe(
      data => this.setData(data)
    );
  }

  
async ngAfterViewInit(): Promise<void> {
	this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(role.menuInterfaceModalidadeProdutoBB);
}

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  adicionarModalidade(e) {
    this.crudService.addModalidade();
    this.bsModalRef = this.modalService.show(ModalAdicionarModalidadeBbComponent);
  }

  description: { descricao: string } = { descricao: null };

  pesquisar(description: string = null) {
    this.description = { descricao: description };
    this.refresh({ descricao: description, ordenacao: 'descricao', ascendente: true });
  }

  editarModalidade(modalidade) {
    this.crudService.editarModalidade(modalidade);
    this.bsModalRef = this.modalService.show(ModalAdicionarModalidadeBbComponent);
  }



}
