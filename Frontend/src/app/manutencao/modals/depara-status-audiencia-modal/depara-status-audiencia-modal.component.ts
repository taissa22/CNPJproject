import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

// 3rd party
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';

// core & shared
import { StaticInjector } from '@manutencao/static-injector';
import { DialogService } from '@shared/services/dialog.service';

// local
import { DeparaStatusAudiencia } from '@manutencao/models/depara-status-audiencia.model';
import { DeparaStatusAudienciaService } from '@manutencao/services/depara-status-audiencia.service';
import { DeparaStatusResponse } from '@manutencao/models/depara-status-response.model';
import { HttpErrorResult } from '@core/http';
import { TiposProcesso } from '@manutencao/models/tipos-de-processos';

@Component({
  selector: 'app-depara-status-audiencia-modal',
  templateUrl: './depara-status-audiencia-modal.component.html',
  styleUrls: ['./depara-status-audiencia-modal.component.scss']
})
export class DeparaStatusAudienciaModalComponent implements OnInit {

  deparaStatusAudiencia: DeparaStatusAudiencia;
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
    private _serviceDeparaStatusAudiencia: DeparaStatusAudienciaService,
  ) { }

  statusAPPFormControl: FormControl = new FormControl(null, [Validators.required]);
  substatusAPPFormControl: FormControl = new FormControl(null, [Validators.required]);
  statusSisjurFormControl: FormControl = new FormControl(null, [Validators.required]);
  criarAudienciaAutomaticaFormControl: FormControl = new FormControl(false);

  formGroup: FormGroup = new FormGroup({
    statusAPPId: this.statusAPPFormControl,
    substatusAPPId: this.substatusAPPFormControl,
    statusSisjurId: this.statusSisjurFormControl,
    criarAudienciaAutomatica: this.criarAudienciaAutomaticaFormControl,
  });

  ngOnInit(): void {
    this.InicilizaForm();
  }

  InicilizaForm() {
    if (this.deparaStatusAudiencia) {
      this.idDepara = this.deparaStatusAudiencia.id;
      this.criarAudienciaAutomaticaFormControl.setValue(this.deparaStatusAudiencia.criacaoAutomaticaNovaAudiencia == "Sim" ? true : false);
      this.statusAPPFormControl.setValue(this.deparaStatusAudiencia.idStatusApp);
      this.substatusAPPFormControl.setValue(this.deparaStatusAudiencia.idSubStatusApp);
      this.statusSisjurFormControl.setValue(this.deparaStatusAudiencia.idStatusSisjur);
    }
  }

  close(): void {
    this.modal.close(false);
  }

  // tslint:disable-next-line: member-ordering
  public static exibeModalDeIncluir(statusAPP: Array<DeparaStatusResponse>, substatusAPP: Array<DeparaStatusResponse>, statusSisjur: Array<DeparaStatusResponse>, tipoProcesso: any): Promise<boolean> {
    // prettier-ignore
    console.log('incluir');
    statusSisjur = statusSisjur.filter(function (obj) { return obj.descricao.indexOf("[INATIVO]") === -1; });
    const modalRef = StaticInjector.Instance.get(NgbModal).open(DeparaStatusAudienciaModalComponent, { windowClass: 'modal-depara-status-audiencia', centered: true, size: 'sm', backdrop: 'static' });
    modalRef.componentInstance.statusAPP = statusAPP;
    modalRef.componentInstance.substatusAPP = substatusAPP;
    modalRef.componentInstance.statusSisjur = statusSisjur;
    modalRef.componentInstance.TiposProcesso = TiposProcesso.porId(tipoProcesso);

    if (tipoProcesso == 1) modalRef.componentInstance.tipoProcessoDescricao = "CÍVEL CONSUMIDOR";
    else if (tipoProcesso == 7) modalRef.componentInstance.tipoProcessoDescricao = "JUIZADO ESPECIAL CÍVEL";
    else if (tipoProcesso == 17) modalRef.componentInstance.tipoProcessoDescricao = "PROCON";

    return modalRef.result;
  }

  // tslint:disable-next-line: member-ordering
  public static exibeModalDeAlterar(deparaStatusAudiencia: DeparaStatusAudiencia, statusAPP: Array<DeparaStatusResponse>, substatusAPP: Array<DeparaStatusResponse>, statusSisjur: Array<DeparaStatusResponse>, tipoProcesso: any): Promise<boolean> {
    // prettier-ignore
    console.log('alterar');
    statusSisjur = statusSisjur.filter(function (obj) { return obj.descricao.indexOf("[INATIVO]") === -1; });
    const modalRef = StaticInjector.Instance.get(NgbModal).open(DeparaStatusAudienciaModalComponent, { windowClass: 'modal-depara-status-audiencia', centered: true, size: 'sm', backdrop: 'static' });
    modalRef.componentInstance.deparaStatusAudiencia = deparaStatusAudiencia;
    modalRef.componentInstance.statusAPP = statusAPP;
    modalRef.componentInstance.substatusAPP = substatusAPP;
    modalRef.componentInstance.TiposProcesso = TiposProcesso.porId(tipoProcesso);

    if (!statusSisjur.find(i => i.id === deparaStatusAudiencia.idStatusSisjur)) {
      statusSisjur.push(new DeparaStatusResponse(
        deparaStatusAudiencia.idStatusSisjur,
        deparaStatusAudiencia.descricaoStatusSisjur.indexOf("[INATIVO]") !== -1 ? deparaStatusAudiencia.descricaoStatusSisjur : deparaStatusAudiencia.descricaoStatusSisjur + "[INATIVO]"));
    }

    modalRef.componentInstance.statusSisjur = statusSisjur.sort((a, b) => (a.descricao < b.descricao) ? -1 : 1);

    if (tipoProcesso == 1) modalRef.componentInstance.tipoProcessoDescricao = "CÍVEL CONSUMIDOR";
    else if (tipoProcesso == 7) modalRef.componentInstance.tipoProcessoDescricao = "JUIZADO ESPECIAL CÍVEL";
    else if (tipoProcesso == 17) modalRef.componentInstance.tipoProcessoDescricao = "PROCON";

    return modalRef.result;
  }

  async save(): Promise<void> {
    const operacao = this.deparaStatusAudiencia ? 'Alteração' : 'Inclusão';
    try {
      if (this.deparaStatusAudiencia) {
        await this._serviceDeparaStatusAudiencia.alterar({ ...this.formGroup.value, id: this.idDepara, tipoProcesso: this.TiposProcesso.id });
      } else {
        await this._serviceDeparaStatusAudiencia.incluir({ ...this.formGroup.value, tipoProcesso: this.TiposProcesso.id });
      }
      await this.dialogService.alert(`${operacao} realizada com sucesso`);
      this.modal.close(true);
    } catch (ex) {
      await this.dialogService.err(`Desculpe, não foi possivel a ${operacao}`, ex.error);
    };
  }

  desabilitaTooltip(formControl: FormControl) {
    return formControl.untouched || formControl.valid;
  }

}

