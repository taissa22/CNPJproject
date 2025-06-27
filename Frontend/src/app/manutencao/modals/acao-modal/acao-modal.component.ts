import { TipoProcesso } from 'src/app/core/models/tipo-processo';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
// 3rd party
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
// core & shared
import { StaticInjector } from '@manutencao/static-injector';
import { DialogService } from '@shared/services/dialog.service';
// local
import { HttpErrorResult } from '@core/http';
import { Acao } from '@manutencao/models/acao.model';
import { AcaoService } from '@manutencao/services/acao.service';
import { TiposProcesso } from '@manutencao/models/tipos-de-processos';
import { NaturezaBBResponse } from '@manutencao/models/natureza-bb-response.model';
import { AcaoCivelEstrategicoResponse } from '../../models/acao-civel-estrategico-response';
import { AcaoCivelConsumidorResponse } from '@manutencao/models/acao-civel-consumidor-response';

@Component({
  selector: 'acao-modal',
  templateUrl: './acao-modal.component.html',
  styleUrls: ['./acao-modal.component.scss']
})
export class AcaoModalComponent implements OnInit {

  Acao: Acao;
  naturezaAcaoBB: Array<NaturezaBBResponse>;
  acoesCivelEstrategico: Array<AcaoCivelEstrategicoResponse>;
  acoesCivelConsumidor: Array<AcaoCivelConsumidorResponse>;
  TiposProcesso: TiposProcesso;
  idAcao: number;

  constructor(
    private modal: NgbActiveModal,
    private dialogService: DialogService,
    private _serviceAcao: AcaoService,
  ) { }

  descricaoFormControl: FormControl = new FormControl('', [Validators.required, Validators.maxLength(50)]);
  naturezaAcaoBBFormControl: FormControl = new FormControl();
  requerEscritorioFormControl: FormControl = new FormControl();
  enviarAppPrepostoFormControl: FormControl = new FormControl();
  acoesCivelEstrategicoFormControl: FormControl = new FormControl();
  acoesCivelConsumidorFormControl: FormControl = new FormControl();
  ativoFormControl: FormControl = new FormControl();

  formGroup: FormGroup = new FormGroup({
    id: this.descricaoFormControl,
    descricao: this.descricaoFormControl,
    naturezaAcaoBB: this.naturezaAcaoBBFormControl,
    requerEscritorio: this.requerEscritorioFormControl,
    enviarAppPreposto: this.enviarAppPrepostoFormControl,
    acoesCivelEstrategico: this.acoesCivelEstrategicoFormControl,
    acoesCivelConsumidor: this.acoesCivelConsumidorFormControl,
    ativo : this.ativoFormControl
  });

  ngOnInit(): void {
    this.InicilizaForm();
  }

  InicilizaForm() {
    console.log(this.Acao);
    if (this.Acao) {
      this.idAcao = this.Acao.id;
      this.descricaoFormControl.setValue(this.Acao.descricao);
      this.naturezaAcaoBBFormControl.setValue(this.Acao.naturezaAcaoBBId);
      this.requerEscritorioFormControl.setValue(this.Acao.indRequerEscritorio == "NÃO" ? false : true);
      this.enviarAppPrepostoFormControl.setValue(this.Acao.enviarAppPreposto == "NÃO" ? false : true);
      this.acoesCivelEstrategicoFormControl.setValue(this.Acao.acaoCivelEstrategicoId);
      this.acoesCivelConsumidorFormControl.setValue(this.Acao.acaoCivelConsumidorId);
      this.ativoFormControl.setValue(this.Acao.ativo == "NÃO" ? false : true);
    }
    else
    {
      this.ativoFormControl.setValue(true);
    }
  }

  close(): void {
    this.modal.close(false);
  }

  public static exibeModalDeIncluir(tipoProcesso: any, naturezaBB: Array<NaturezaBBResponse>, acoesCivelEstrategico: Array<AcaoCivelEstrategicoResponse>, acoesCivelConsumidor: Array<AcaoCivelConsumidorResponse>): Promise<boolean> {
    // prettier-ignore
    const modalRef = StaticInjector.Instance.get(NgbModal).open(AcaoModalComponent, { windowClass: 'evento-modal', centered: true, size: 'lg', backdrop: 'static' });
    modalRef.componentInstance.TiposProcesso = TiposProcesso.porId(tipoProcesso);
    modalRef.componentInstance.naturezaAcaoBB = naturezaBB;
    modalRef.componentInstance.acoesCivelEstrategico = acoesCivelEstrategico;
    modalRef.componentInstance.acoesCivelConsumidor = acoesCivelConsumidor;
    return modalRef.result;
  }

  // tslint:disable-next-line: member-ordering
  public static exibeModalDeAlterar(tipoProcesso: any, acao: Acao, naturezaBB: Array<NaturezaBBResponse>,  acoesCivelEstrategico: Array<AcaoCivelEstrategicoResponse>, acoesCivelConsumidor: Array<AcaoCivelConsumidorResponse>): Promise<boolean> {
    // prettier-ignore
    const modalRef = StaticInjector.Instance.get(NgbModal).open(AcaoModalComponent, { windowClass: 'evento-modal', centered: true, size: 'lg', backdrop: 'static' });
    modalRef.componentInstance.TiposProcesso = TiposProcesso.porId(tipoProcesso);
    modalRef.componentInstance.Acao = acao;
    modalRef.componentInstance.naturezaAcaoBB = naturezaBB;
    modalRef.componentInstance.acoesCivelEstrategico = acoesCivelEstrategico;
    modalRef.componentInstance.acoesCivelConsumidor = acoesCivelConsumidor;
    return modalRef.result;
  }

  async save(): Promise<void> {
    const operacao = this.Acao ? 'Alteração' : 'Inclusão';
    try {
      if (this.Acao) {
        await this._serviceAcao.alterar({ ...this.formGroup.value, id: this.idAcao, tipoProcesso: this.TiposProcesso.id });
      } else {
        await this._serviceAcao.incluir({ ...this.formGroup.value, id: 0, tipoProcesso: this.TiposProcesso.id });
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

