import { Component, OnInit } from '@angular/core';
import { EstornoResultadoService } from '../services/estorno-resultado.service';
import { BsModalService, BsModalRef } from 'ngx-bootstrap';
import { ModalPartesComponent } from './modal-partes/modal-partes.component';
import { ModalPedidosComponent } from './modal-partes/modal-pedidos/modal-pedidos.component';
import { EstornoAbasService } from '../services/estorno-abas.service';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { role } from 'src/app/sap/sap.constants';

@Component({
  selector: 'app-estorno-resultado',
  templateUrl: './estorno-resultado.component.html',
  styleUrls: ['./estorno-resultado.component.scss']
})
export class EstornoResultadoComponent implements OnInit {

  processo;
    private bsModalRef: BsModalRef;
  breadcrumb: string;
    constructor(private service: EstornoResultadoService,
       private modalService: BsModalService,
       private abaService: EstornoAbasService,
       private breadcrumbsService: BreadcrumbsService) { }

  config = {
    backdrop: false,
    ignoreBackdropClick: true,
    class: 'modal-class'
  };

  variavel;

  ngOnInit(): void {
     this.service.getProcessoSelecionado().subscribe(item =>
      this.processo = item )

    this.variavel = this.processo.numeroProcesso + ' - ' +
    this.processo.uf + '-' + this.processo.nomeComarca + " - " +
    this.processo.vara + 'ª VARA ' + this.processo.nomeTipoVara +
    ' - ' + this.processo.classificacaoHierarquica +
    ' - Código Interno: '
    + this.processo.codigoProcesso;

    this.abaService.valoresAbas = this.processo.dadosLancamentosEstorno;
  }

  async ngAfterViewInit(): Promise<void> {
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(role.menuEstornoLancamento);
  }

  onClickAbrirModalPartes() {
    this.bsModalRef = this.modalService.show(ModalPartesComponent, this.config);
  }
  onClickAbrirModalPedido() {
    this.bsModalRef = this.modalService.show(ModalPedidosComponent, this.config);
  }
}
