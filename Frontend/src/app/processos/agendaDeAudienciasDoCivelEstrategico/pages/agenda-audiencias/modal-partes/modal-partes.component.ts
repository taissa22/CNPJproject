
import { map, filter, delay } from 'rxjs/operators';
import { isNullOrUndefined } from 'util';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Component, OnInit, Input, AfterContentInit, AfterViewInit, ChangeDetectionStrategy } from '@angular/core';
import { Processo } from '../../../models';
import { StaticInjector } from '@shared/static-injector';
import { PartesDoProcessoService } from '../services/partes-do-processo.service';

@Component({
  templateUrl: './modal-partes.component.html',
  styleUrls: ['./modal-partes.component.scss']
})
export class ModalPartesAgendaComponent implements OnInit {
  @Input() entidade: Processo;
  listaAutores: Array<any> = [];
  listaReus: Array<any> = [];

  mascara(valor) {
    if (isNullOrUndefined(valor)) {
      return;
    }

    if (valor.length <= 11) {
      return valor.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/g, '\$1.\$2.\$3\-\$4');
    } else {
      return valor.replace(/(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})/g, '\$1.\$2.\$3\/\$4\-\$5');
    }
  }

  constructor(public activeModal: NgbActiveModal, private partesDoProcessoService: PartesDoProcessoService) { }

  ngOnInit() {
    setTimeout(() => {
      if (this.entidade) {
        const partesDoProcesso = this.partesDoProcessoService.obterPartesPorProcesso(this.entidade.id);
        partesDoProcesso.then(async res =>
          this.listaAutores = res.filter(p => p.tipoParticipacaoId === 1)
            .map(a => ({ id: a.parteId, nome: a.parte.nome, documento: a.parte.cpf !== null ? a.parte.cpf : a.parte.cnpj })));
        partesDoProcesso.then(async res =>
          this.listaReus = res.filter(p => p.tipoParticipacaoId === 2)
            .map(r => ({ id: r.parteId, nome: r.parte.nome, documento: r.parte.cpf !== null ? r.parte.cpf : r.parte.cnpj })));
      }
    });
  }

  // tslint:disable-next-line: member-ordering
  public static async exibeModalDeConsultar(processo: Processo): Promise<void> {
    const modalService: NgbModal = StaticInjector.Instance.get(NgbModal);
    const modalRef = modalService.open(ModalPartesAgendaComponent, { centered: true, backdrop: 'static' });
    modalRef.componentInstance.entidade = processo;
    await modalRef.result;
  }
}
