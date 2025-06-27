// angular
import { AfterViewChecked, AfterViewInit, Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

// 3rd party
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';

// core & shared
import { StaticInjector } from '@manutencao/static-injector';
import { DialogService } from '@shared/services/dialog.service';


import { TipoDeProcedimento } from '@manutencao/models/tipo-de-procedimento';
import { HttpErrorResult } from '@core/http';
import { TipoDeProcedimentoService } from '@manutencao/services/tipo-de-procedimento.service';
@Component({
  selector: 'app-tipo-de-procedimento-modal',
  templateUrl: './tipo-de-procedimento-modal.component.html',
  styleUrls: ['./tipo-de-procedimento-modal.component.scss']
  
})
export class TipoDeProcedimentoModalComponent implements AfterViewInit {
  tipoDeProcedimento: TipoDeProcedimento;
  codTipoDeProcesso: number;
  titulo: string;

  listaTipoParticipacao : Array<{codigo: number, descricao: string}> = []; 

  constructor(
    private modal: NgbActiveModal,
    private service: TipoDeProcedimentoService,
    private dialogService: DialogService
  ) {}

  descricaoFormControl: FormControl = new FormControl('', [
    Validators.required,
    Validators.maxLength(50)
  ]);

  ativoFormControl: FormControl = new FormControl(true);
  tipoParticipacao1FormControl: FormControl = new FormControl(null);
  indOrgao1FormControl: FormControl = new FormControl(null); 
  tipoParticipacao2FormControl: FormControl = new FormControl(null);
  indOrgao2FormControl: FormControl = new FormControl(null); 
  indPoloPassivoUnicoFormControl: FormControl = new FormControl(null); 
  indProvisionadoFormControl: FormControl = new FormControl(null);

  //poloPassivoUltimoEstado = this.indPoloPassivoUnicoFormControl.value;

  exibirMensagemOrgao1 = false;
  exibirMensagemOrgao2 = false;
  exibirMensagemPoloPassivo = false;

  msgOrgao2a: string = 'Não pode ser selecionada a coluna órgão para o tipo de participação 1 e a  coluna órgão para o tipo de participação 2 simultaneamente.';
  
  msgOrgao2b: string = 'Não pode ser selecionada a coluna polo passivo único e a coluna órgão para o tipo de participação 2 simultaneamente.';


  formGroup: FormGroup = new FormGroup({
    descricao: this.descricaoFormControl,
    tipoParticipacao1: this.tipoParticipacao1FormControl,
    tipoParticipacao2: this.tipoParticipacao2FormControl
  }); 

  async ngAfterViewInit(): Promise<void> {
    this.listaTipoParticipacao = await this.service.ObterListaTipoParticipacao();
    if (this.tipoDeProcedimento) {
      this.codTipoDeProcesso = this.tipoDeProcedimento.tipoDeProcesso.id;
      this.descricaoFormControl.setValue(this.tipoDeProcedimento.descricao);
      this.ativoFormControl.setValue(this.tipoDeProcedimento.indAtivo);   
      this.tipoParticipacao1FormControl.setValue(this.tipoDeProcedimento.tipoParticipacao1 ? this.tipoDeProcedimento.tipoParticipacao1.codigo : null);
      this.indOrgao1FormControl.setValue(this.tipoDeProcedimento.indOrgao1); 
      this.tipoParticipacao2FormControl.setValue(this.tipoDeProcedimento.tipoParticipacao2 ? this.tipoDeProcedimento.tipoParticipacao2.codigo : null);
      this.indOrgao2FormControl.setValue(this.tipoDeProcedimento.indOrgao2);
      this.indPoloPassivoUnicoFormControl.setValue(this.tipoDeProcedimento.indPoloPassivoUnico);
      this.indProvisionadoFormControl.setValue(this.tipoDeProcedimento.indProvisionado);
    }

    if (this.codTipoDeProcesso == 3 || this.codTipoDeProcesso == 4 || this.codTipoDeProcesso == 6) {
      this.tipoParticipacao1FormControl.setValidators([Validators.required]);
      this.tipoParticipacao2FormControl.setValidators([Validators.required]);

      this.tipoParticipacao1FormControl.updateValueAndValidity();
      this.tipoParticipacao2FormControl.updateValueAndValidity();
    }
  }

  close(): void {
    this.modal.close(false);
  }

  desabilitaTooltip(formControl: FormControl): boolean {
    return formControl.untouched || formControl.valid;
  }

  async save(): Promise<void> {   
    const operacao = this.tipoDeProcedimento? 'Alteração': 'Inclusão';
    try {
      if (!this.descricaoFormControl.value.replace(/\s/g, '').length) {
        await this.dialogService.err(`${operacao} não realizada`, `O campo descrição não pode contar apenas espaços.`);
        return;
      }
      
      if (this.tipoDeProcedimento) {
        await this.service.alterar(
          this.tipoDeProcedimento.codigo, 
          this.descricaoFormControl.value,         
          this.ativoFormControl.value,
          this.tipoParticipacao1FormControl.value,
          this.indOrgao1FormControl.value,
          this.tipoParticipacao2FormControl.value,
          this.indOrgao2FormControl.value,          
          this.indPoloPassivoUnicoFormControl.value,
          this.indProvisionadoFormControl.value 
        );
      } else {   
        
        await this.service.incluir(
          this.descricaoFormControl.value,         
          this.ativoFormControl.value,
          this.tipoParticipacao1FormControl.value,
          this.indOrgao1FormControl.value,
          this.tipoParticipacao2FormControl.value,
          this.indOrgao2FormControl.value,         
          this.indPoloPassivoUnicoFormControl.value,
          this.indProvisionadoFormControl.value,
          this.codTipoDeProcesso          
          );
      }
      await this.dialogService.alert(`${operacao} realizada com sucesso`);
      this.modal.close(true);
    } catch (error) {
      await this.dialogService.err(`${operacao} não realizada`, (error as HttpErrorResult).messages.join('\n'));
    }
  }

  //#region MODAL

  static exibeModalDeIncluir(codTipoDeProcesso: number):Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(TipoDeProcedimentoModalComponent, { centered: true, size: 'sm', backdrop: 'static' });
    modalRef.componentInstance.codTipoDeProcesso = codTipoDeProcesso;
    modalRef.componentInstance.titulo = 'Incluir Tipo de Procedimento';
    return modalRef.result;
  }

  
  static exibeModalDeAlterar(
    tipoDeProcedimento: TipoDeProcedimento,
    codTipoDeProcesso: number
    ):Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(TipoDeProcedimentoModalComponent, { centered: true, size: 'sm', backdrop: 'static' });
    modalRef.componentInstance.tipoDeProcedimento = tipoDeProcedimento;
    modalRef.componentInstance.codTipoDeProcesso = codTipoDeProcesso;
    modalRef.componentInstance.titulo = 'Alterar Tipo de Procedimento';
    return modalRef.result;
  }

  exibirMensagem(checkbox): void{

    let elemento = checkbox;

    switch(elemento){

      case 'orgao1':
        if(this.indOrgao2FormControl.value===true){
          this.exibirMensagemOrgao1 = true;
        }
      break;

      case 'orgao2':
        if(this.indOrgao1FormControl.value===true || this.indPoloPassivoUnicoFormControl.value===true){
          this.exibirMensagemOrgao2 = true;
        }
      break;

      case 'poloPassivo':
        if(this.indOrgao2FormControl.value===true){
          this.exibirMensagemPoloPassivo = true;
        }
      break;

      case 'desligar':
        this.exibirMensagemOrgao1 = false;
        this.exibirMensagemOrgao2 = false;
        this.exibirMensagemPoloPassivo = false;
      break;

      default:
      break;
    }

  }

  //#endregion MODAL
}

