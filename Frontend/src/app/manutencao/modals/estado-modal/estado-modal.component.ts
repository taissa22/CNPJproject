import { DecimalPipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { HttpErrorResult } from '@core/http';
import { Estado } from '@manutencao/models/estado.model';
import { EstadoService } from '@manutencao/services/estado.service';
import { StaticInjector } from '@manutencao/static-injector';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DialogService } from '@shared/services/dialog.service';
import { toUpperFirstLetter } from '@shared/utils';
import { conformToMask } from 'angular2-text-mask';

@Component({
  selector: 'app-estado-modal',
  templateUrl: './estado-modal.component.html',
  styleUrls: ['./estado-modal.component.scss']
})
export class EstadoModalComponent implements OnInit {

  estado: Estado = null;
  IdFormControl:FormControl = new FormControl(null,Validators.required);
  nomeFormControl:FormControl = new FormControl(null,[Validators.required,Validators.maxLength(30)]);
  valorJurosFormControl:FormControl = new FormControl(0.0,Validators.required);
  formGroup:FormGroup = new FormGroup({
    id : this.IdFormControl,
    nome: this.nomeFormControl,
    valorJuros: this.valorJurosFormControl
  })
  mascara = [/[0-9]/, /[0-9]/, /[0-9]/,',', /[0-9]/, /[0-9]/, /[0-9]/, /[0-9]/, /[0-9]/];
  constructor(
    private service: EstadoService,
    private dialogService: DialogService,
    private modal: NgbActiveModal) { }

  ngOnInit() {
    this.iniciarForm();
  }

  iniciarForm(){
    this.IdFormControl.setValue(this.estado.id);
    this.IdFormControl.disable();
    this.nomeFormControl.setValue(this.estado.nome);
    let pipeNumber = new DecimalPipe('pt-BR')
    let vj = pipeNumber.transform(this.estado.valorJuros, '3.5');
    let valorJuros = conformToMask(
      vj,
      this.mascara,
      {guide: false}
    )
    this.valorJurosFormControl.setValue(valorJuros.conformedValue);
  }

  async salvar(){
    try {
      if (! await this.formValido()) return;

      await this.atualizar();

      await this.dialogService.alert(`Alteração realizada com sucesso`);
      this.modal.close(true);

    } catch (error) {
      await this.dialogService.err(`Desculpe, não foi possivel a Alteração`, (error as HttpErrorResult).messages.join('\n'));
    }
  }


  private async formValido() {

    if (this.nomeFormControl.value && !this.nomeFormControl.value.replace(/\s/g, '').length) {
      await this.dialogService.err(`Desculpe, não foi possivel a alteração`, `O campo nome não pode conter apenas espaços.`);
      return false;
    }
    if (this.nomeFormControl.value &&  this.nomeFormControl.value.length > 30) {
      await this.dialogService.err(`Desculpe, não foi possivel a alteração`, `O campo nome não pode conter mais de 30 caracteres.`);
      return false;
    }
    if (this.IdFormControl.value && !this.IdFormControl.value.replace(/\s/g, '').length) {
      await this.dialogService.err(`Desculpe, não foi possivel a alteração`, `O campo sigla não pode conter apenas espaços.`);
      return false;
    }
    if (this.estado.valorJuros != parseFloat(this.valorJurosFormControl.value.replace(",","."))) {
      await this.dialogService.err(`Atenção`, `A mudança da taxa de juros acarretará no calculo dos valores para todos os processos.`);
      return true;
    }
 
    return true;
  }

  
  async atualizar() {
    let obj :any =  this.formGroup.value;
     obj.id = this.IdFormControl.value;
     obj.valorJuros = parseFloat(obj.valorJuros.replace(",","."));
     await this.service.atualizar(this.formGroup.value);
  }

  desabilitaTooltip(formControl: FormControl): boolean {
    return formControl.untouched || formControl.valid;
  }

  static exibeModalDeAlterar(estado: Estado): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(EstadoModalComponent, { centered: true, size: 'sm', backdrop: 'static' });
    modalRef.componentInstance.estado = estado;
    return modalRef.result;
  }
  
  close(): void {
    this.modal.close(false);
  }
}
