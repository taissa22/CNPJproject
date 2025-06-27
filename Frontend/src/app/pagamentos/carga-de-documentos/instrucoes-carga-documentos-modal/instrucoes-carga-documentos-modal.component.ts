import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { StaticInjector } from '@shared/static-injector';
import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-instrucoes-carga-documentos-modal',
  templateUrl: './instrucoes-carga-documentos-modal.component.html',
  styleUrls: ['./instrucoes-carga-documentos-modal.component.scss']
})
export class InstrucoesCargaDocumentosModalComponent implements OnInit {

  @Input() titulo: string = 'Siga as instruções abaixo para a carga de documentos';

  tamanhoMaximoArquivoAnexo: number;

  constructor(public modal: NgbActiveModal) { }

  ngOnInit() {
  }

  // tslint:disable-next-line: member-ordering
  public static async exibeModal(tamanhoMaximoArquivoAnexo: number): Promise<void> {
    const modalService: NgbModal = StaticInjector.Instance.get(NgbModal);
    const modalRef = modalService.open(InstrucoesCargaDocumentosModalComponent, { centered: true, backdrop: 'static' });
    modalRef.componentInstance.tamanhoMaximoArquivoAnexo = tamanhoMaximoArquivoAnexo;
    await modalRef.result;
  }

  cancelar() {
    this.modal.dismiss();
  }

}
