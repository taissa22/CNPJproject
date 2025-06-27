import { Component, OnInit, OnDestroy } from '@angular/core';
import { BsModalService, BsModalRef } from 'ngx-bootstrap';
import { Subscription } from 'rxjs';
import { ManutencaoCommonComponent } from '../../shared/components/manutencao-common-component';
import { TribunalBBService } from './services/tribunalBBService.service';
import { ModalTribunaisBbComponent } from './modal-tribunais-bb/modal-tribunais-bb.component';
import { CrudTribunalBbService } from './services/crud-tribunal-bb.service';
import { take } from 'rxjs/operators';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { role } from '../../sap.constants';

@Component({
  selector: 'tribunaisBB',
  templateUrl: './tribunaisBB.component.html',
  styleUrls: ['./tribunaisBB.component.scss']
})
export class TribunaisBBComponent extends ManutencaoCommonComponent implements OnInit {
  breadcrumb: string;

  constructor(private modalService: BsModalService,
    public service: TribunalBBService,
    private crudService: CrudTribunalBbService,
    private breadcrumbsService: BreadcrumbsService) {
    super(service);
  }
  public bsModalRef: BsModalRef;
  subscription: Subscription;

  headerSemBusca = [
  'Código',
      'Tribunal BB',
     'Instância Designada',
   'Descrição do Tribunal' ]

  description: { descricao: string } = { descricao: null };

  ngOnInit() {
    this.headersToRemove = ['descricaoIndicadorInstancia'];
    this.crudService.dadosConsulta.subscribe(response => this.setData(response));
    this.defineModel({ ordenacao: 'descricao', ascendente: true });
  }

  async ngAfterViewInit(): Promise<void> {
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(role.menuInterfaceTribunaisBB);
  }

  pesquisar(description: string = null) {
    this.description = { descricao: description };
    this.refresh({ descricao: description, ordenacao: 'descricao', ascendente: true });
  }


  adicionarTribunal(e) {
    this.crudService.addTribunal();
    this.bsModalRef = this.modalService.show(ModalTribunaisBbComponent);
  }

  editarTribunal(tribunal) {
    this.crudService.editarTribunal(tribunal);
    this.bsModalRef = this.modalService.show(ModalTribunaisBbComponent);
  }



}
