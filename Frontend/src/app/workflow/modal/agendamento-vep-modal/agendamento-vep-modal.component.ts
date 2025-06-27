import { Component, OnInit } from '@angular/core';
import { StaticInjector } from '../../static-injector';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FormControl, FormGroup } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { DialogService } from '@shared/services/dialog.service';
import { AgendamentoVepService } from '../../services/agendamento-vep.service';

@Component({
  selector: 'app-agendamento-vep-modal',
  templateUrl: './agendamento-vep-modal.component.html',
  styleUrls: ['./agendamento-vep-modal.component.scss']
})
export class AgendamentoVepModalComponent implements OnInit {

  constructor(
    private datePipe: DatePipe,
    public activeModal: NgbActiveModal,
    private dialog : DialogService,
    private service: AgendamentoVepService
  ) { }

  async ngOnInit() {
    await this.gerarDiasMes();
    await this.calcularDias();
  }

  static exibeModal(): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      AgendamentoVepModalComponent,
      { centered: true, backdrop: 'static',size: 'sm', windowClass:"modal-agend-vep"}
    );
    return modalRef.result;
  }

  diasDoMes = [];
  mesesItems = [];
  dataAtual = new Date(new Date().getFullYear(), new Date().getMonth(), new Date().getDate());
  
  numMesesFormControl: FormControl = new FormControl(6);
  periodicidadeExecucaoFormControl: FormControl = new FormControl(0);
  dataEspecificaFormControl: FormControl = new FormControl(new Date(this.dataAtual));
  diaExecucaoFormControl: FormControl = new FormControl();
  indUltimoDiaMesFormControl: FormControl = new FormControl();

  vepForm = new FormGroup({
    datEspecifica: this.dataEspecificaFormControl,
    periodicidadeExecucao: this.periodicidadeExecucaoFormControl,
    numMeses: this.numMesesFormControl,
    indUltimoDiaDoMes: this.indUltimoDiaMesFormControl,
    diaDoMes: this.diaExecucaoFormControl
  });

  async gerarDiasMes(){
    let contador = 1;
    while(contador <= 36){
      this.mesesItems.push(contador);
      contador++;
    }
  }

  async calcularDias() {
    let contador = 1;
    while(contador <= 30){
      this.diasDoMes.push(contador);
      contador++;
    }
  }

  async desabilitarSelectDias(){
    if(!this.indUltimoDiaMesFormControl.value){
      this.diaExecucaoFormControl.disable()
      this.diaExecucaoFormControl.setValue(null);
    } else{
      this.diaExecucaoFormControl.enable();
    }

  }

  async agendar(){
    if(await this.validarAgendamento()){
      await this.checkExecucao();
      return this.dialog.err('Erro ao salvar', 'Não foi possivel salvar o agendamento, verique os dados e tente novamente.')
    }

    this.vepForm.patchValue({indUltimoDiaDoMes: this.indUltimoDiaMesFormControl.value ? true : false});
    try {
      this.dataEspecificaFormControl.setValue(new Date(this.dataEspecificaFormControl.value));
      await this.service.incluirAgendamento(this.vepForm.value);
      await this.dialog.alert('Agendamento concluído.', "Cálculo agendado com sucesso.");
      this.activeModal.close(true);
    } catch (error) {
      this.dialog.err('Erro ao salvar', error.error);
    }
  }

  async validarAgendamento(): Promise<boolean> {
    let hasError = false;
    const dataEspecifica = this.periodicidadeExecucaoFormControl.value == 1 && this.dataEspecificaFormControl.value != "Invalid Date" ? this.datePipe.transform(this.dataEspecificaFormControl.value, 'yyyy-MM-ddTHH:mm:ss.SSSZ') : null;
    
    this.vepForm.patchValue({
      periodicidadeExecucao: Number(this.periodicidadeExecucaoFormControl.value), 
      datEspecifica : dataEspecifica,
      indUltimoDiaDoMes: this.indUltimoDiaMesFormControl.value ? true : false
    });
    
    if(this.periodicidadeExecucaoFormControl.value != 4){
      this.vepForm.patchValue({
          indUltimoDiaDoMes: null,
          diaDoMes: null
        });
    }

    this.diaExecucaoFormControl.enable();

    if(this.periodicidadeExecucaoFormControl.value == 4 && !this.indUltimoDiaMesFormControl.value && !this.diaExecucaoFormControl.value)
      hasError = true;

    if(this.periodicidadeExecucaoFormControl.value == 1 && !this.dataEspecificaFormControl.value)
      hasError = true;

    if(this.periodicidadeExecucaoFormControl.value == 1 && this.dataEspecificaFormControl.value < this.datePipe.transform(this.dataAtual, 'yyyy-MM-ddTHH:mm:ss.SSSZ'))
      hasError = true;

    return hasError;
  }

  async checkExecucao(){
    switch(this.periodicidadeExecucaoFormControl.value) {
      case 0:
        (<HTMLInputElement>(document.getElementById('imediata'))).checked = true
      break;
      case 1:
        (<HTMLInputElement>document.getElementById('especifica')).checked = true
      break;
      case 4:
        (<HTMLInputElement>document.getElementById('mensal')).checked = true
      break;
    }
  }
}
