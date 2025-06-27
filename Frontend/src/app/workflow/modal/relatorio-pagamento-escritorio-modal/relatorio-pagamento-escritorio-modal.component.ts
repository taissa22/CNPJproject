import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DialogService } from '@shared/services/dialog.service';
import { StaticInjector } from '../../static-injector';
import { FormControl, FormGroup } from '@angular/forms';
import { RelatorioPagamentoEscritorioService } from '../../services/relatorio-pagamento-escritorio.service';

@Component({
  selector: 'app-relatorio-pagamento-escritorio-modal',
  templateUrl: './relatorio-pagamento-escritorio-modal.component.html',
  styleUrls: ['./relatorio-pagamento-escritorio-modal.component.scss']
})
export class RelatorioPagamentoEscritorioModalComponent implements OnInit {

  constructor(
    private datePipe: DatePipe,
    public activeModal: NgbActiveModal,
    private dialog : DialogService,
    private service: RelatorioPagamentoEscritorioService
  ) { }

  async ngOnInit() {
    await this.calcularDias();
  }

  static exibeModal(): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      RelatorioPagamentoEscritorioModalComponent,
      { centered: true, backdrop: 'static',size: 'sm', windowClass:"modal-agend-vep"}
    );
    return modalRef.result;
  }

  diasDoMes = [];
  dataAtual = new Date(new Date().getFullYear(), new Date().getMonth(), new Date().getDate());
  
  mesReferenciaFormControl: FormControl = new FormControl();
  periodicidadeExecucaoFormControl: FormControl = new FormControl(0);
  dataEspecificaFormControl: FormControl = new FormControl(new Date(this.dataAtual));
  diaExecucaoFormControl: FormControl = new FormControl();
  indUltimoDiaMesFormControl: FormControl = new FormControl();

  relForm = new FormGroup({
    datEspecifica: this.dataEspecificaFormControl,
    periodicidadeExecucao: this.periodicidadeExecucaoFormControl,
    mesReferencia: this.mesReferenciaFormControl,
    diaDoMes: this.diaExecucaoFormControl
  });

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
    console.log(this.relForm.value);
    try {
      this.dataEspecificaFormControl.setValue(new Date(this.dataEspecificaFormControl.value));
      this.mesReferenciaFormControl.setValue(new Date(this.mesReferenciaFormControl.value));
      await this.service.incluirAgendamento(this.relForm.value);
      await this.dialog.alert('Agendamento concluído.', "Relatório agendado com sucesso.");
      this.activeModal.close(true);
    } catch (error) {
      this.dialog.err('Erro ao salvar', error.error);
    }
  }

  async validarAgendamento(): Promise<boolean> {
    let hasError = false;
    const dataEspecifica = this.periodicidadeExecucaoFormControl.value == 1 && this.dataEspecificaFormControl.value != "Invalid Date" ? this.datePipe.transform(this.dataEspecificaFormControl.value, 'yyyy-MM-ddTHH:mm:ss.SSSZ') : null;
    const mesReferencia = this.mesReferenciaFormControl.value != "Invalid Date" ? this.datePipe.transform(this.mesReferenciaFormControl.value, 'yyyy-MM-ddTHH:mm:ss.SSSZ') : null;
    
    this.relForm.patchValue({
      periodicidadeExecucao: Number(this.periodicidadeExecucaoFormControl.value), 
      datEspecifica : dataEspecifica,
      mesReferencia : mesReferencia
    });
    
    if(this.periodicidadeExecucaoFormControl.value != 4){
      this.relForm.patchValue({
          diaDoMes: null
        });
    }

    this.diaExecucaoFormControl.enable();

    if(this.mesReferenciaFormControl.value == null)
      hasError = true;

    if(this.periodicidadeExecucaoFormControl.value == 4 && !this.diaExecucaoFormControl.value)
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
