import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { StaticInjector } from '@manutencao/static-injector';
import { DialogService } from '@shared/services/dialog.service';
import { TipoDeAudiencia } from '@manutencao/models/tipo-de-audiencia';
import { TipoDeAudienciaService } from '@manutencao/services/tipo-de-audiencia.service';
import { TipoProcesso } from '@core/models/tipo-processo';
import { HttpErrorResult } from '@core/http/http-error-result';
import { TiposProcesso } from '@manutencao/models/tipos-de-processos';

@Component({
  selector: 'app-tipo-de-audiencia-modal',
  templateUrl: './tipo-de-audiencia-modal.component.html',
  styleUrls: ['./tipo-de-audiencia-modal.component.scss'],

})
export class TipoDeAudienciaModalComponent implements OnInit {

  tipoDeProcessoFormControl: FormControl = new FormControl(null, [Validators.required]);
  ativoFormControl: FormControl = new FormControl(true, [Validators.required]);
  siglaFormControl: FormControl = new FormControl('', [Validators.required, Validators.maxLength(4)]);
  descricaoFormControl: FormControl = new FormControl('', [Validators.required, Validators.maxLength(100)]);
  comboDeparaEstrategicoFormControl: FormControl = new FormControl();
  comboDeparaConsumidorFormControl: FormControl = new FormControl();
  audienciaVirtualFormControl: FormControl = new FormControl(false);
  tipoDeAudiencia: TipoDeAudiencia;
  tiposDeProcesso: TiposProcesso[] = [];
  titulo: string;
  comboDeparaEstrategico: any;
  comboDeparaConsumidor: any;

  constructor(
    private modal: NgbActiveModal,
    private dialogService: DialogService,
    private service: TipoDeAudienciaService
  ) { }

  ngOnInit(): void {
    this.obterDeParaEstrategico();
    this.obterDeParaConsumidor();
  }

  ngAfterViewInit(): void {
    if (this.tipoDeAudiencia) {
      this.tipoDeProcessoFormControl.setValue(this.tipoDeAudiencia.tipoDeProcesso.id);
      this.ativoFormControl.setValue(this.tipoDeAudiencia.ativo);
      this.siglaFormControl.setValue(this.tipoDeAudiencia.sigla);
      this.descricaoFormControl.setValue(this.tipoDeAudiencia.descricao);
      this.comboDeparaEstrategicoFormControl.setValue(this.tipoDeAudiencia.idMigracao)
      this.comboDeparaConsumidorFormControl.setValue(this.tipoDeAudiencia.idMigracao)
      this.tipoDeProcessoFormControl.disable();
      this.audienciaVirtualFormControl.setValue(this.tipoDeAudiencia.linkVirtual);
    }
  }

  formGroup: FormGroup = new FormGroup({
    tipoDeProcesso: this.tipoDeProcessoFormControl,
    ativo: this.ativoFormControl,
    sigla: this.siglaFormControl,
    descricao: this.descricaoFormControl,
    comboDeparaEstrategico: this.comboDeparaEstrategicoFormControl,
    comboDeparaConsumidor: this.comboDeparaConsumidorFormControl
  });

  close(): void {
    this.modal.close(false);
  }

  async save(): Promise<void> {
    const operacao = this.tipoDeAudiencia ? 'Alteração' : 'Inclusão';
    if (!this.descricaoFormControl.value.replace(/\s/g, '').length) {
      await this.dialogService.err(`${operacao} não realizada`, `O campo descrição não pode contar apenas espaços.`);
      return;
    }
    try {
      if (this.tipoDeAudiencia) {
        await this.service.alterar(
          this.tipoDeAudiencia.codigoTipoAudiencia,
          this.descricaoFormControl.value,
          this.ativoFormControl.value,
          this.siglaFormControl.value,
          this.audienciaVirtualFormControl.value,
          this.comboDeparaEstrategicoFormControl.value ? this.comboDeparaEstrategicoFormControl.value : null,
          this.comboDeparaConsumidorFormControl.value ? this.comboDeparaConsumidorFormControl.value : null
        );
      } else {
        await this.service.incluir(
          this.descricaoFormControl.value,
          this.tipoDeProcessoFormControl.value,
          this.ativoFormControl.value,
          this.siglaFormControl.value,
          this.audienciaVirtualFormControl.value,
          this.comboDeparaEstrategicoFormControl.value ? this.comboDeparaEstrategicoFormControl.value : null,
          this.comboDeparaConsumidorFormControl.value ? this.comboDeparaConsumidorFormControl.value : null
        );
      }
      await this.dialogService.alert(`${operacao} realizada com sucesso`);
      this.modal.close(true);
    } catch (error) {
      if ((error as HttpErrorResult).messages.join('\n').includes('Já existe')) {
        this.dialogService.info(`Desculpe, não é possível fazer a ${operacao.toLowerCase()}`, (error as HttpErrorResult).messages.join('\n'));
      } else {
        this.dialogService.err(`${operacao} não realizada`, (error as HttpErrorResult).messages.join('\n'));
      }
      throw error;
    }
  }

  public static exibeModalDeIncluir(tipoProcesso: Array<TipoProcesso>): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(TipoDeAudienciaModalComponent, { centered: true, backdrop: 'static', size: 'sm' });
    modalRef.componentInstance.titulo = 'Incluir Tipo de Audiência';
    modalRef.componentInstance.tiposDeProcesso = tipoProcesso;
    return modalRef.result;
  }

  public static exibeModalDeAlterar(tipoDeAudiencia: TipoDeAudiencia, tipoProcesso: Array<TipoProcesso>): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(TipoDeAudienciaModalComponent, { centered: true, backdrop: 'static', size: 'sm' });
    modalRef.componentInstance.titulo = 'Alterar Tipo de Audiência';
    modalRef.componentInstance.tipoDeAudiencia = tipoDeAudiencia;
    modalRef.componentInstance.tiposDeProcesso = tipoProcesso;
    return modalRef.result;
  }

  obterDeParaEstrategico(): void {
    this.service.ObterDescricaoDeParaCivelEstrategico().then((e) => {
      this.comboDeparaEstrategico = e.map(r => ({ idEstrategico: r.codigoTipoAudiencia, nome: r.descricao + (r.ativo ? '' : ' [INATIVO]') }));
    });
  }

  obterDeParaConsumidor(): void {
    this.service.ObterDescricaoDeParaCivelConsumidor().then((e) => {
      this.comboDeparaConsumidor = e.map(r => ({ idConsumidor: r.codigoTipoAudiencia, nome: r.descricao + (r.ativo ? '' : ' [INATIVO]') }));
    });
  }

  exibeRequerLink(): boolean {
    if (this.tipoDeProcessoFormControl.value === 1 || this.tipoDeProcessoFormControl.value === 7 || this.tipoDeProcessoFormControl.value === 17)
      return true;
    return false
  }

}
