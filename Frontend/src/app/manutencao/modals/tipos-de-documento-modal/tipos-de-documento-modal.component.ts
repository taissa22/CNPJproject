// angular
import { AfterContentInit, AfterViewInit, Component, DoCheck, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

// 3rd party
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';

// core & shared
import { StaticInjector } from '@manutencao/static-injector';
import { DialogService } from '@shared/services/dialog.service';


import { TipoDeDocumento } from '@manutencao/models/tipos-de-documento.model';
import { TipoDeDocumentoService } from '@manutencao/services/tipos-de-documento.service';
import { HttpErrorResult } from '@core/http';
@Component({
  selector: 'app-tipos-de-documento-modal',
  templateUrl: './tipos-de-documento-modal.component.html',
  styleUrls: ['./tipos-de-documento-modal.component.scss']

})
export class TiposDeDocumentoModalComponent implements AfterViewInit,OnInit {
  tipoDeDocumento: TipoDeDocumento;
  codTipoDeProcesso: number = 0;
  titulo: string;
  tiposDePrazo: Array<{ id: number, descricao: string }>;

  comboDeparaEstrategico : any;
  comboDeparaConsumidor : any;

  constructor(
    private modal: NgbActiveModal,
    private service: TipoDeDocumentoService,
    private dialogService: DialogService
  ) { }


  descricaoFormControl: FormControl = new FormControl('', [
    Validators.required,
    Validators.maxLength(50)
  ]);

  ativoFormControl: FormControl = new FormControl(true);
  cadastraProcessoFormControl = new FormControl(false);
  requerDataAudienciaFormControl = new FormControl(false);
  prioritarioFilaCadastroProcessoFormControl = new FormControl(false);
  utilizadoEmProtocoloFormControl = new FormControl(false);
  tipoDePrazoFormControl = new FormControl(null);
  documentoApuracaoFormControl = new FormControl(false);
  enviarAppPrepostoFormControl = new FormControl(false);
  comboDeparaEstrategicoFormControl = new FormControl();
  comboDeparaConsumidorFormControl = new FormControl();
  formGroup: FormGroup = new FormGroup({
    decricao : this.descricaoFormControl,
    ativo: this.ativoFormControl,
    cadastraProcesso: this.cadastraProcessoFormControl,
    requerDataAudiencia: this.requerDataAudienciaFormControl,
    prioritarioFilaCadastroProcesso:this.prioritarioFilaCadastroProcessoFormControl,
    utilizadoEmProtocolo : this.utilizadoEmProtocoloFormControl,
    tipoDePrazo : this.tipoDePrazoFormControl,
    documentoApuracao : this.documentoApuracaoFormControl,
    enviarAppPreposto : this.enviarAppPrepostoFormControl,
    comboDeparaEstrategico: this.comboDeparaEstrategicoFormControl,
	comboDeparaConsumidor: this.comboDeparaConsumidorFormControl
  });

  ngOnInit(): void {
    if(this.tipoDeDocumento) {
      this.codTipoDeProcesso = this.tipoDeDocumento.codTipoProcesso;
    }
    this.obterDeParaEstrategico();
    this.obterDeParaConsumidor();
  }

  ngAfterViewInit(): void {
    this.enviarAppPrepostoFormControl.disable();
    if (this.tipoDeDocumento) {
      this.descricaoFormControl.setValue(this.tipoDeDocumento.descricao);
      this.ativoFormControl.setValue(this.tipoDeDocumento.ativo);
      this.cadastraProcessoFormControl.setValue(this.tipoDeDocumento.cadastraProcesso);
      this.requerDataAudienciaFormControl.setValue(this.tipoDeDocumento.requerDataAudienciaPrazo);
      this.prioritarioFilaCadastroProcessoFormControl.setValue(this.tipoDeDocumento.prioritarioFilaCadastroProcesso);
      this.utilizadoEmProtocoloFormControl.setValue(this.tipoDeDocumento.utilizadoEmProtocolo);
      this.tipoDePrazoFormControl.setValue(this.tipoDeDocumento.tipoDePrazo ? this.tipoDeDocumento.tipoDePrazo.id : null);
      this.documentoApuracaoFormControl.setValue(this.tipoDeDocumento.indDocumentoApuracao);
      this.enviarAppPrepostoFormControl.setValue(this.tipoDeDocumento.indEnviarAppPreposto);
      this.comboDeparaEstrategicoFormControl.setValue(this.tipoDeDocumento.idMigracao);
	  this.comboDeparaConsumidorFormControl.setValue(this.tipoDeDocumento.idMigracao);

      if(this.tipoDeDocumento.indDocumentoApuracao){
         this.enviarAppPrepostoFormControl.enable();
      }
    }
  }

  close(): void {
    this.modal.close(false);
  }

  desabilitaTooltip(formControl: FormControl): boolean {
    return formControl.untouched || formControl.valid;
  }

  async save(): Promise<void> {
    const operacao = this.tipoDeDocumento ? 'Alteração' : 'Inclusão';
    try {
      if (!this.descricaoFormControl.value.replace(/\s/g, '').length) {
        await this.dialogService.err(`${operacao} não realizada`, `O campo descrição não pode contar apenas espaços.`);
        return;
      }

      if (this.tipoDeDocumento) {
        await this.service.alterar(
          this.tipoDeDocumento.codigo,
          this.descricaoFormControl.value,
          this.tipoDeDocumento.codTipoProcesso,
          this.ativoFormControl.value,
          this.cadastraProcessoFormControl.value,
          this.requerDataAudienciaFormControl.value,
          this.prioritarioFilaCadastroProcessoFormControl.value,
          this.utilizadoEmProtocoloFormControl.value,
          this.documentoApuracaoFormControl.value,
          this.enviarAppPrepostoFormControl.value,
          this.tipoDePrazoFormControl.value ? this.tipoDePrazoFormControl.value : null,
          this.comboDeparaEstrategicoFormControl.value ? this.comboDeparaEstrategicoFormControl.value : null,
		  this.comboDeparaConsumidorFormControl.value ? this.comboDeparaConsumidorFormControl.value : null,
        );
      } else {
        await this.service.incluir(
          this.descricaoFormControl.value,
          this.codTipoDeProcesso,
          this.ativoFormControl.value,
          this.cadastraProcessoFormControl.value,
          this.requerDataAudienciaFormControl.value,
          this.prioritarioFilaCadastroProcessoFormControl.value,
          this.utilizadoEmProtocoloFormControl.value,
          this.documentoApuracaoFormControl.value,
          this.enviarAppPrepostoFormControl.value,
          this.tipoDePrazoFormControl.value ? this.tipoDePrazoFormControl.value : null,
          this.comboDeparaEstrategicoFormControl.value ? this.comboDeparaEstrategicoFormControl.value : null,
		  this.comboDeparaConsumidorFormControl.value ? this.comboDeparaConsumidorFormControl.value : null
        );
      }
      await this.dialogService.alert(`${operacao} realizada com sucesso`);
      this.modal.close(true);
    } catch (error) {
      await this.dialogService.err(`${operacao} não realizada`, (error as HttpErrorResult).messages.join('\n'));
    }
  }

  //#region MODAL

  static exibeModalDeIncluir(codTipoDeProcesso: number, tiposDePrazo: Array<{ id: number, descricao: string }>): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(TiposDeDocumentoModalComponent, { centered: true, size: 'sm', backdrop: 'static' });
    modalRef.componentInstance.codTipoDeProcesso = codTipoDeProcesso;
    modalRef.componentInstance.tiposDePrazo = tiposDePrazo;
    modalRef.componentInstance.titulo = 'Incluir Tipo de Documento';
    return modalRef.result;
  }

  static exibeModalDeAlterar(tipoDeDocumento: TipoDeDocumento, tiposDePrazo: Array<{ id: number, descricao: string }>): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(TiposDeDocumentoModalComponent, { centered: true, size: 'sm', backdrop: 'static' });
    modalRef.componentInstance.tipoDeDocumento = tipoDeDocumento;
    modalRef.componentInstance.tiposDePrazo = tiposDePrazo;
    modalRef.componentInstance.titulo = 'Alterar Tipo de Documento';
    return modalRef.result;
  }

  //#endregion MODAL


  controlaDisabledEnviarAppPrepostoCheck() {
    if (!this.documentoApuracaoFormControl.value) {
      this.enviarAppPrepostoFormControl.disable();
      this.enviarAppPrepostoFormControl.setValue(false);
    } else {
      this.enviarAppPrepostoFormControl.enable();
    }
  }

  obterDeParaEstrategico(): void {
    this.service.ObterDescricaoDeParaCivelEstrategico().then((e) => {
      this.comboDeparaEstrategico = e.map(r => ({ idEstrategico: r.codigo, nome: r.descricao + (r.ativo ? '' : ' [INATIVO]') }));
      console.log(this.comboDeparaEstrategico )
    });
  }

  obterDeParaConsumidor(): void {
    this.service.ObterDescricaoDeParaCivelConsumidor().then((e) => {
      this.comboDeparaConsumidor = e.map(r => ({ idConsumidor: r.codigo, nome: r.descricao + (r.ativo ? '' : ' [INATIVO]') }));
      console.log(this.comboDeparaConsumidor )
    });
}
}
