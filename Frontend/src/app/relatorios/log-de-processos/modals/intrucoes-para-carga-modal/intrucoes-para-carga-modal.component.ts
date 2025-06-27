import { Component, OnInit } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { StaticInjector } from '../../static-injector';

@Component({
  selector: 'app-intrucoes-para-carga-modal',
  templateUrl: './intrucoes-para-carga-modal.component.html',
  styleUrls: ['./intrucoes-para-carga-modal.component.scss']
})
export class IntrucoesParaCargaModalComponent implements OnInit {

  constructor(public modal: NgbActiveModal) { }

  ngOnInit() {
  }

  

  static exibeModal(): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(IntrucoesParaCargaModalComponent, { centered: true  });
    // modalRef.componentInstance.operacaoTitulo = 'Incluir';
    return modalRef.result;
  }

}
