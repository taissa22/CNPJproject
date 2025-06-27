import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Component, OnInit, Input } from '@angular/core';
import { StaticInjector } from '../../static-injector';

@Component({
  selector: 'app-instrucoes-migracao-pedidos-sap',
  templateUrl: './instrucoes-migracao-pedidos-sap.component.html',
  styleUrls: ['./instrucoes-migracao-pedidos-sap.component.scss']
})
export class InstrucoesMigracaoPedidosSapComponent implements OnInit {

  @Input() titulo: string = 'Siga as instruções abaixo para a carga de documentos';

  tamanhoMaximoArquivoAnexo: number;
  

  constructor(public modal: NgbActiveModal) { }

  ngOnInit() {
  }

  // tslint:disable-next-line: member-ordering
  public static async exibeModal(tamanhoMaximoArquivoAnexo: number): Promise<void> {
    const modalService: NgbModal = StaticInjector.Instance.get(NgbModal);
    const modalRef = modalService.open(InstrucoesMigracaoPedidosSapComponent, { centered: true, backdrop: 'static' });
    modalRef.componentInstance.tamanhoMaximoArquivoAnexo = tamanhoMaximoArquivoAnexo;
    await modalRef.result;
  }

  cancelar() {
    this.modal.close();
  }

}

