import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

// 3rd party
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';

// core & shared
import { StaticInjector } from '@manutencao/static-injector';
import { DialogService } from '@shared/services/dialog.service';

// local
import { DeparaStatusNegociacao } from '@manutencao/models/depara-status-negociacao.model';
import { DeparaStatusNegociacaoService } from '@manutencao/services/depara-status-negociacao.service';
import { DeparaStatusResponse } from '@manutencao/models/depara-status-response.model';
import { HttpErrorResult } from '@core/http';
import { TiposProcesso } from '@manutencao/models/tipos-de-processos';

@Component({
  selector: 'app-depara-status-negociacao-modal',
  templateUrl: './depara-status-negociacao-modal.component.html',
  styleUrls: ['./depara-status-negociacao-modal.component.scss']
})
export class DeparaStatusNegociacaoModalComponent implements OnInit {
  deparaStatusNegociacao: DeparaStatusNegociacao;
  statusAPP: Array<DeparaStatusResponse>;
  substatusAPP: Array<DeparaStatusResponse>;
  statusSisjur: Array<DeparaStatusResponse>;
  usuarioAtualId: string;
  idDepara: number;
  TiposProcesso: TiposProcesso;
  tipoProcessoDescricao: string;

  constructor(
    private modal: NgbActiveModal,
    private dialogService: DialogService,
    private _serviceDeparaStatusNegociacao: DeparaStatusNegociacaoService
  ) {}

  statusAPPFormControl: FormControl = new FormControl(null, [
    Validators.required
  ]);
  substatusAPPFormControl: FormControl = new FormControl(null);

  statusSisjurFormControl: FormControl = new FormControl(null, [
    Validators.required
  ]);
  //criarNegociacaoAutomaticaFormControl: FormControl = new FormControl(false);

  formGroup: FormGroup = new FormGroup({
    statusAPPId: this.statusAPPFormControl,
    substatusAPPId: this.substatusAPPFormControl,
    statusSisjurId: this.statusSisjurFormControl
    //criaNegociacoes: this.criarNegociacaoAutomaticaFormControl
  });

  ngOnInit(): void {
    this.InicilizaForm();
  }

  InicilizaForm() {
    if (this.deparaStatusNegociacao) {
      this.idDepara = this.deparaStatusNegociacao.id;
      /*this.criarNegociacaoAutomaticaFormControl.setValue(
        this.deparaStatusNegociacao.criaNegociacoes == 'Sim' ? true : false
      );*/
      this.statusAPPFormControl.setValue(
        this.deparaStatusNegociacao.idStatusApp
      );
      this.substatusAPPFormControl.setValue(
        this.deparaStatusNegociacao.idSubStatusApp
      );
      this.statusSisjurFormControl.setValue(
        this.deparaStatusNegociacao.idStatusSisjur
      );
    }
  }

  close(): void {
    this.modal.close(false);
  }

  // tslint:disable-next-line: member-ordering
  public static exibeModalDeIncluir(
    statusAPP: Array<DeparaStatusResponse>,
    substatusAPP: Array<DeparaStatusResponse>,
    statusSisjur: Array<DeparaStatusResponse>,
    tipoProcesso: any
  ): Promise<boolean> {
    // prettier-ignore
    console.log('incluir')
    statusSisjur = statusSisjur.filter(function (obj) {
      return obj.descricao.indexOf('[INATIVO]') === -1;
    });
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      DeparaStatusNegociacaoModalComponent,
      {
        windowClass: 'modal-depara-status-negociacao',
        centered: true,
        size: 'sm',
        backdrop: 'static'
      }
    );
    modalRef.componentInstance.statusAPP = statusAPP;
    modalRef.componentInstance.substatusAPP = substatusAPP;
    modalRef.componentInstance.statusSisjur = statusSisjur;
    modalRef.componentInstance.TiposProcesso =
      TiposProcesso.porId(tipoProcesso);

    if (tipoProcesso == 1)
      modalRef.componentInstance.tipoProcessoDescricao = 'CÍVEL CONSUMIDOR';
    else if (tipoProcesso == 7)
      modalRef.componentInstance.tipoProcessoDescricao =
        'JUIZADO ESPECIAL CÍVEL';
    else if (tipoProcesso == 17)
      modalRef.componentInstance.tipoProcessoDescricao = 'PROCON';

    return modalRef.result;
  }

  // tslint:disable-next-line: member-ordering
  public static exibeModalDeAlterar(
    deparaStatusNegociacao: DeparaStatusNegociacao,
    statusAPP: Array<DeparaStatusResponse>,
    substatusAPP: Array<DeparaStatusResponse>,
    statusSisjur: Array<DeparaStatusResponse>,
    tipoProcesso: any
  ): Promise<boolean> {
    // prettier-ignore
    console.log('alterar');
    statusSisjur = statusSisjur.filter(function (obj) {
      return obj.descricao.indexOf('[INATIVO]') === -1;
    });
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      DeparaStatusNegociacaoModalComponent,
      {
        windowClass: 'modal-depara-status-negociacao',
        centered: true,
        size: 'sm',
        backdrop: 'static'
      }
    );
    modalRef.componentInstance.deparaStatusNegociacao = deparaStatusNegociacao;
    modalRef.componentInstance.statusAPP = statusAPP;
    modalRef.componentInstance.substatusAPP = substatusAPP;
    modalRef.componentInstance.TiposProcesso =
      TiposProcesso.porId(tipoProcesso);

    if (
      !statusSisjur.find(i => i.id === deparaStatusNegociacao.idStatusSisjur)
    ) {
      statusSisjur.push(
        new DeparaStatusResponse(
          deparaStatusNegociacao.idStatusSisjur,
          deparaStatusNegociacao.descricaoStatusSisjur.indexOf('[INATIVO]') !==
          -1
            ? deparaStatusNegociacao.descricaoStatusSisjur
            : deparaStatusNegociacao.descricaoStatusSisjur + '[INATIVO]'
        )
      );
    }

    modalRef.componentInstance.statusSisjur = statusSisjur.sort((a, b) =>
      a.descricao < b.descricao ? -1 : 1
    );

    if (tipoProcesso == TiposProcesso.CIVEL_CONSUMIDOR.id)
      modalRef.componentInstance.tipoProcessoDescricao = TiposProcesso.CIVEL_CONSUMIDOR.descricao.toUpperCase();
    else if (tipoProcesso == TiposProcesso.JEC.id)
      modalRef.componentInstance.tipoProcessoDescricao = TiposProcesso.JEC.descricao.toUpperCase();
    else if (tipoProcesso == TiposProcesso.PROCON.id)
      modalRef.componentInstance.tipoProcessoDescricao = TiposProcesso.PROCON.descricao.toUpperCase();

    return modalRef.result;
  }

  async save(): Promise<void> {
    const operacao = this.deparaStatusNegociacao ? 'Alteração' : 'Inclusão';
    try {
      if (this.deparaStatusNegociacao) {
        await this._serviceDeparaStatusNegociacao.alterar({
          ...this.formGroup.value,
          id: this.idDepara,
          tipoProcesso: this.TiposProcesso.id
        });
      } else {
        await this._serviceDeparaStatusNegociacao.incluir({
          ...this.formGroup.value,
          tipoProcesso: this.TiposProcesso.id
        });
      }
      await this.dialogService.alert(`${operacao} realizada com sucesso`);
      this.modal.close(true);
    } catch (ex) {
      await this.dialogService.err(
        `Desculpe, não foi possivel a ${operacao}`,
        ex.error
      );
    }
  }

  desabilitaTooltip(formControl: FormControl) {
    return formControl.untouched || formControl.valid;
  }
}
