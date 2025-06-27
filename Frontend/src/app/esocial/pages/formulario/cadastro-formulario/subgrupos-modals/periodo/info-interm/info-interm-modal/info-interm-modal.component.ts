import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ErrorLib } from '@esocial/libs/error-lib';
import { InfoInterm } from '@esocial/models/subgrupos/v1_2/InfoInterm';
import { InfoIntermService } from '@esocial/services/formulario/subgrupos/infointerm.service';
import { StaticInjector } from '@esocial/static-injector';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DialogService } from '@shared/services/dialog.service';

@Component({
  selector: 'app-info-interm-modal',
  templateUrl: './info-interm-modal.component.html',
  styleUrls: ['./info-interm-modal.component.scss']
})
export class InfoIntermModalComponent implements OnInit {
  infoInterm: InfoInterm;

  titulo: string;
  formularioId: number;
  contratoId: number;
  codigoIdeperiodo: number;
  periodo: Date;

  consulta: boolean = false;

  hrs: string = '';
  valMax: number = 31;

  tooltipDia: string = 'Preencher com o dia do mês efetivamente trabalhado pelo empregado com contrato de trabalho intermitente.';
  tooltipHoraTrab: string = 'Preencher com as horas trabalhadas no dia pelo empregado com contrato de trabalho intermitente. Preenchimento obrigatório e exclusivo se a classificação  tributária em S-1000 = [22 - Segurado especial, inclusive quando for empregador doméstico].';


  infointermDiaFormControl: FormControl = new FormControl(null);
  infointermHrstrabFormControl: FormControl = new FormControl(null);
  idFormControl: FormControl = new FormControl(null);

  formGroup: FormGroup = new FormGroup({
    InfointermHrstrab: this.infointermHrstrabFormControl,
    InfointermDia: this.infointermDiaFormControl,
    idEsF2500Infointerm: this.idFormControl
  });

  constructor(
    private modal: NgbActiveModal,
    private service : InfoIntermService,
    private dialogService: DialogService
  ) { }

  ngOnInit() {
  }

  ngAfterViewInit() {
    this.iniciarForm();
  }

  iniciarForm() {
    if (this.infoInterm) {
      this.infointermDiaFormControl.setValue(
        this.infoInterm.infointermDia
        );

        this.infointermDiaFormControl.disable();

      this.infointermHrstrabFormControl.setValue(
        this.infoInterm.infointermHrstrab
      );

      this.idFormControl.setValue(
        this.infoInterm.idEsF2500Infointerm
        );
    }else{
      this.infointermDiaFormControl.enable();
    }
  }

  async salvar() {
    const operacao = this.infoInterm ? 'Alteração' : 'Inclusão';

    try {

      if (this.formGroup.invalid) {
        return false;
      }

      
      if (this.infointermHrstrabFormControl.value) {
        this.infointermHrstrabFormControl.setValue(this.infointermHrstrabFormControl.value.replace(':',''));
      }

      if (this.infoInterm) {
        let obj = this.formGroup.value;
        obj.InfointermDia = this.infointermDiaFormControl.value;
        await this.service.atualizar(this.formularioId, this.contratoId, this.codigoIdeperiodo, obj);        
      }else{
        await this.service.incluir(this.formularioId, this.contratoId, this.codigoIdeperiodo, this.formGroup.value);
      }

      await this.dialogService.alert('Operação realizada', `${operacao} realizada com sucesso`);
      this.modal.close(true);
      
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      await this.dialogService.err(
        `Desculpe, não foi possivel a ${operacao}`,
        mensagem
      );
    }
  }

  static exibirModalAlterar(
    formularioId: number,
    contratoId: number,
    infoInterm: InfoInterm,
    codigoIdeperiodo: number,
    consulta: boolean,
    periodo: Date
  ): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      InfoIntermModalComponent,
      { windowClass: 'modal-info-interm',centered: true, size: 'sm', backdrop: 'static'}
    );
    modalRef.componentInstance.infoInterm = infoInterm;
    modalRef.componentInstance.contratoId = contratoId;
    modalRef.componentInstance.formularioId = formularioId;
    modalRef.componentInstance.codigoIdeperiodo = codigoIdeperiodo;
    modalRef.componentInstance.periodo = periodo;
    modalRef.componentInstance.titulo = consulta ? 'Consultar' :'Alterar';
    return modalRef.result;
  }

  static exibirModalIncluir(
    formularioId: number,
    contratoId: number,
    codigoIdeperiodo: number,
    periodo: Date
  ): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      InfoIntermModalComponent,
      { windowClass: 'modal-info-interm',centered: true, size: 'sm', backdrop: 'static' }
    );
    modalRef.componentInstance.formularioId = formularioId;
    modalRef.componentInstance.contratoId = contratoId;
    modalRef.componentInstance.codigoIdeperiodo = codigoIdeperiodo;
    modalRef.componentInstance.periodo = periodo;
    modalRef.componentInstance.titulo = 'Incluir';
    return modalRef.result;
  }

  close(): void {
    this.modal.close(false);
  }

  adicionaValidators() {

    let anoMes =  this.periodo.getMonth(); 
    this.valMax = new Date(this.periodo.getFullYear(), this.periodo.getMonth() + 1, 0).getDate();

    this.infointermDiaFormControl.setValidators([Validators.max(this.valMax), Validators.min(0), Validators.required]);
    this.infointermDiaFormControl.updateValueAndValidity();
    this.infointermDiaFormControl.markAsTouched();
    if (this.infointermDiaFormControl.value && (this.infointermDiaFormControl.value > 0)) {
      this.infointermHrstrabFormControl.setValidators([Validators.required]);
      this.infointermHrstrabFormControl.updateValueAndValidity();
      this.infointermHrstrabFormControl.markAllAsTouched();
    }else{
      this.infointermHrstrabFormControl.clearValidators();
      this.infointermHrstrabFormControl.updateValueAndValidity();
      this.infointermHrstrabFormControl.markAllAsTouched();
    }


  }

  desabilitaTooltip(control: FormControl):boolean{
    return control.valid || control.untouched || control.disabled
  }

  removerNaoNumericos(control: FormControl) {
    control.setValue(control.value.replace(/[^0-9]/g, ''));
  }

}
