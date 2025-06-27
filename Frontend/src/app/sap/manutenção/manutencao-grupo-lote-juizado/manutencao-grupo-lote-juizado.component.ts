import { ModalAdicionarGrupoLoteJuizadoComponent } from './modal-adicionar-grupo-lote-juizado/modal-adicionar-grupo-lote-juizado.component';
import { GrupoLoteJuizadoService } from './services/grupo-lote-juizado.service';
import { Component, OnInit } from '@angular/core';
import { BsModalService, BsModalRef } from 'ngx-bootstrap';
import { NaturezaBBService } from '../../interfacesBB/natureza-bb/services/naturezaBB.service';
import { CrudNaturezaBBService } from '../../interfacesBB/natureza-bb/services/crud-naturezaBB.service';
import { Subscription } from 'rxjs';
import { ModalAdicionarNaturezaBBComponent } from '../../interfacesBB/natureza-bb/modal-adicionar-naturezaBB/modal-adicionar-naturezaBB.component';
import { ManutencaoCommonComponent } from '../../shared/components/manutencao-common-component';
import { CrudGrupoLoteJuizadoService } from './services/crud-grupo-lote-juizado.service';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { role } from '../../sap.constants';

@Component({
  selector: 'app-manutencao-grupo-lote-juizado',
  templateUrl: './manutencao-grupo-lote-juizado.component.html',
  styleUrls: ['./manutencao-grupo-lote-juizado.component.scss']
})
export class ManutencaoGrupoLoteJuizadoComponent extends ManutencaoCommonComponent implements OnInit {
  breadcrumb: string;

  constructor(private modalService: BsModalService,
    public service: GrupoLoteJuizadoService,
    private crudService: CrudGrupoLoteJuizadoService,
    private breadcrumbsService: BreadcrumbsService) {
    super(service);
  }

  public bsModalRef: BsModalRef;
  subscription: Subscription;

  description: { descricao: string } = { descricao: null };

  ngOnInit() {
    this.defineModel({ ordenacao: 'descricao', ascendente: true });
    this.subscription = this.crudService.naturezaBB.subscribe(data => this.setData(data));
  }

  async ngAfterViewInit(): Promise<void> {
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(role.menuManutencaoGrupoLoteJuizado);
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
  pesquisar(description: string = null) {
    this.description = { descricao: description };

    this.refresh({ descricao: description, ordenacao: 'descricao', ascendente: true });
  }

  adicionarNatureza(e) {
    this.crudService.addGrupoLoteJuizado();
    this.bsModalRef = this.modalService.show(ModalAdicionarGrupoLoteJuizadoComponent);
  }

  editarNatureza(grupoLoteJuizado) {
    this.crudService.editarGrupoLoteJuizado(grupoLoteJuizado);
    this.bsModalRef = this.modalService.show(ModalAdicionarGrupoLoteJuizadoComponent);
  }

  // excluirNatureza(Natureza) {
  //   this.crudService.excluir(Natureza);
  // }


}
