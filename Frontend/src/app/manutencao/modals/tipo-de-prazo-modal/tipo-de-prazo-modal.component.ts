import { TipoDePrazo } from './../../models/tipo-de-prazo';
import { TipoDePrazoService } from '@manutencao/services/tipo-de-prazo.service';
import { AfterViewInit, Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { StaticInjector } from '@manutencao/static-injector';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TiposProcesso } from '@manutencao/models/tipos-de-processos';
import { HttpErrorResult } from '@core/http';
import { DialogService } from '@shared/services/dialog.service';

@Component({
  selector: 'app-tipo-de-prazo-modal',
  templateUrl: './tipo-de-prazo-modal.component.html',
  styleUrls: ['./tipo-de-prazo-modal.component.scss']
})
export class TipoDePrazoModalComponent implements AfterViewInit {

  constructor(
    private service: TipoDePrazoService,
    private modal: NgbActiveModal,
    private dialogService: DialogService) { }

  tiposDeProcesso: Array<TiposProcesso> = [];
  tipoProcessoSelecionado: TiposProcesso = null;
  ativarPrazoServico: boolean = false;
  ativarPrazoDocumento: boolean = false;
  tipoDePrazo: TipoDePrazo;
  comboDeparaEstrategico : any;
  comboDeparaConsumidor : any;

  ngOnInit(): void {
    
    this.obterDeParaEstrategico();
    this.obterDeParaConsumidor();
  }

  ngAfterViewInit() {
    this.tipoProcessoFormControl.disable();
    this.tipoProcessoFormControl.setValue(this.tipoProcessoSelecionado? this.tipoProcessoSelecionado: null);

    if (this.tipoDePrazo) {
        this.descricaoFormControl.setValue(this.tipoDePrazo.descricao);
        this.ativoFormControl.setValue(this.tipoDePrazo.ativo)
        this.prazoServicoFormControl.setValue(this.tipoDePrazo.prazoServico)
        this.prazoDocumentoFormControl.setValue(this.tipoDePrazo.prazoDocumento)
        this.comboDeparaEstrategicoFormControl.setValue(this.tipoDePrazo.idMigracao)
        this.comboDeparaConsumidorFormControl.setValue(this.tipoDePrazo.idMigracao)

    }

    this.verificaCheckbox(this.tipoProcessoFormControl.value, true);
  }

  tipoProcessoFormControl: FormControl = new FormControl(null, [
    Validators.required
  ]);

  descricaoFormControl: FormControl = new FormControl('', [
    Validators.required,
    Validators.maxLength(50)
  ]);
  ativoFormControl: FormControl = new FormControl(true, [Validators.required]);
  prazoServicoFormControl: FormControl = new FormControl(false, [Validators.required]);
  prazoDocumentoFormControl: FormControl = new FormControl(false, [Validators.required]);
  comboDeparaEstrategicoFormControl: FormControl = new FormControl();
  comboDeparaConsumidorFormControl: FormControl = new FormControl();

  formGroup: FormGroup = new FormGroup({
    tipoProcesso: this.tipoProcessoFormControl,
    descricao: this.descricaoFormControl,
    ativo: this.ativoFormControl,
    prazoServico: this.prazoServicoFormControl,
    prazoDocumento: this.prazoDocumentoFormControl,
    comboDeparaEstrategico: this.comboDeparaEstrategicoFormControl,
    comboDeparaConsumidor: this.comboDeparaConsumidorFormControl

  });


  static exibeModalDeIncluir(tiposDeProcesso: Array<TiposProcesso>, tipoProcessoSelecionado: TiposProcesso): Promise<boolean> {
    // prettier-ignore

    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(TipoDePrazoModalComponent, { centered: true, windowClass: 'modalSize', backdrop: 'static' });

    modalRef.componentInstance.tiposDeProcesso = tiposDeProcesso;
    modalRef.componentInstance.tipoProcessoSelecionado = tipoProcessoSelecionado;
    return modalRef.result;
  }

  static exibeModalDeAlterar(
    tipoDePrazo: TipoDePrazo,
    tiposDeProcesso: Array<TiposProcesso>,
    tipoProcessoSelecionado: TiposProcesso
  ): Promise<boolean> {
    // prettier-ignore
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(TipoDePrazoModalComponent, { centered: true, windowClass: 'modalSize', backdrop: 'static' });
    modalRef.componentInstance.tipoDePrazo = tipoDePrazo;
    modalRef.componentInstance.tiposDeProcesso = tiposDeProcesso;
    modalRef.componentInstance.tipoProcessoSelecionado = tipoProcessoSelecionado;
    return modalRef.result;
  }

  close(): void {
    this.modal.close(false);
  }

  selecionarTipoProcesso(processoSelecionado) {

    this.tipoProcessoFormControl.setValue(processoSelecionado)
    this.verificaCheckbox(processoSelecionado)

  }

  verificaCheckbox(procSelect: number, isEditing: boolean = false) {
    if (procSelect === 1 || procSelect === 7 || procSelect === 17) {

      this.ativarPrazoServico = true;
      this.ativarPrazoDocumento = false;
      if (!isEditing) {
        this.prazoDocumentoFormControl.setValue(false);
      }

    } else if (procSelect === 14 || procSelect === 15) {

      this.ativarPrazoDocumento = true;
      this.ativarPrazoServico = false;
      if (!isEditing) {
        this.prazoServicoFormControl.setValue(false);
      }

    } else {
      this.ativarPrazoServico = false;
      this.ativarPrazoDocumento = false;
      if (!isEditing) {
        this.prazoServicoFormControl.setValue(false);
        this.prazoDocumentoFormControl.setValue(false);
      }
    }
  }

  async salvar(): Promise<void> {
    const operacao = this.tipoDePrazo? 'Alteração': 'Inclusão';

    if (!this.descricaoFormControl.value.replace(/\s/g, '').length) {
      await this.dialogService.err(`${operacao} não realizada`, `O campo descrição não pode contar apenas espaços.`);
      return;
    }

    try {
      if (this.tipoDePrazo) {
        await this.service.alterar(
          this.tipoDePrazo.codigo,
          this.descricaoFormControl.value,
          this.ativoFormControl.value,
          this.prazoServicoFormControl.value,
          this.prazoDocumentoFormControl.value,
          this.comboDeparaEstrategicoFormControl.value ? this.comboDeparaEstrategicoFormControl.value : null,
		      this.comboDeparaConsumidorFormControl.value ? this.comboDeparaConsumidorFormControl.value : null,
        );
      } else {
        await this.service.incluir(
          this.descricaoFormControl.value,
          this.tipoProcessoFormControl.value,
          this.ativoFormControl.value,
          this.prazoServicoFormControl.value,
          this.prazoDocumentoFormControl.value,
          this.comboDeparaEstrategicoFormControl.value ? this.comboDeparaEstrategicoFormControl.value : null,
		      this.comboDeparaConsumidorFormControl.value ? this.comboDeparaConsumidorFormControl.value : null,
        );
      }

      await this.dialogService.alert(`${operacao} realizada com sucesso`);
      this.modal.close(true);
    } catch (error) {
      await this.dialogService.err(`${operacao} não realizada`, (error as HttpErrorResult).messages.join('\n'));
      //throw error;
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
