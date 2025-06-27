import { StaticInjector } from '@shared/static-injector';
import { NgbActiveModal, NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-instrucoes-carga-modal',
  templateUrl: './instrucoes-carga-modal.component.html',
  styleUrls: ['./instrucoes-carga-modal.component.scss']
})
export class InstrucoesCargaModalComponent implements OnInit {

  @Input() titulo: string = 'Siga as instruções abaixo para a carga de comprovantes';
  tamanhoMaximoArquivosAnexos: number;

  constructor(public modal: NgbActiveModal) { }

  ngOnInit() {
  }

  // tslint:disable-next-line: member-ordering
  public static async exibeModal(tamanhoMaximoArquivosAnexos: number): Promise<void> {
    const modalService: NgbModal = StaticInjector.Instance.get(NgbModal);
    const modalRef = modalService.open(InstrucoesCargaModalComponent, { centered: true, backdrop: 'static' });
    modalRef.componentInstance.tamanhoMaximoArquivosAnexos = tamanhoMaximoArquivosAnexos;
    await modalRef.result;
  }

  cancelar() {
    this.modal.dismiss();
  }

}
