import { Component, OnDestroy, OnInit } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { ModalAdicionarEmpresasSapComponent } from './modal-adicionar-empresas-sap/modal-adicionar-empresas-sap.component';
import { ManutencaoEmpresasSAPService } from './services/manutencao-empresas-sap.service';
import { EmpresasCrudService } from './services/empresas-crud.service';
import { Subscription } from 'rxjs/internal/Subscription';
import { ManutencaoCommonComponent } from '../../shared/components/manutencao-common-component';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { role } from '../../sap.constants';
@Component({
  selector: 'app-manutencao-empresas-sap',
  templateUrl: './manutencao-empresas-sap.component.html',
  styleUrls: ['./manutencao-empresas-sap.component.scss']
})
export class ManutencaoEmpresasSapComponent extends ManutencaoCommonComponent implements OnInit, OnDestroy {
  breadcrumb: string;

  constructor(private modalService: BsModalService,
    public service: ManutencaoEmpresasSAPService,
    private crudService: EmpresasCrudService,
    private breadcrumbsService: BreadcrumbsService
  ) {
    super(service);
  }

  public bsModalRef: BsModalRef;

  /** Coloca a coluna padrão da tabela */
  headerSemBusca = ['Código', 'Sigla', 'Nome', 'Envio SAP', 'Ativa', 'Organização de Compras'];


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
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(role.menuManutencaoEmpresaSap);
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
    this.bsModalRef = this.modalService.show(ModalAdicionarEmpresasSapComponent);
  }

  editar(valor) {
    this.crudService.editar(valor);
    this.bsModalRef = this.modalService.show(ModalAdicionarEmpresasSapComponent);
  }

}
