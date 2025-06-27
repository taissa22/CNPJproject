import { Component, OnInit } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FormControl, FormGroup } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { DialogService } from '@shared/services/dialog.service';
import { CargaDeCompromissosService } from 'src/app/pagamentos/services/carga-de-compromissos.service';
import { StaticInjector } from 'src/app/pagamentos/static-injector';

@Component({
  selector: 'app-agendamento-carga-compromisso-modal',
  templateUrl: './agendamento-carga-compromisso-modal.component.html',
  styleUrls: ['./agendamento-carga-compromisso-modal.component.scss']
})
export class AgendamentoCargaCompromissoModalComponent implements OnInit {

  constructor(
    private datePipe: DatePipe,
    public activeModal: NgbActiveModal,
    private dialog : DialogService,
    private service: CargaDeCompromissosService
  ) { }

  async ngOnInit() {
    await this.gerarDiasMes();
    await this.calcularDias();
  }

  static exibeModal(): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      AgendamentoCargaCompromissoModalComponent,
      { centered: true, backdrop: 'static',size: 'sm', windowClass:"modal-agend-compromisso"}
    );
    return modalRef.result;
  }

  diasDoMes = [];
  mesesItems = [];
  dataAtual = new Date(new Date().getFullYear(), new Date().getMonth(), new Date().getDate());
  
  periodicidadeExecucaoFormControl: FormControl = new FormControl(0);
  dataEspecificaFormControl: FormControl = new FormControl(new Date(this.dataAtual));
  tipoDeProcessoFormControl: FormControl = new FormControl();
  nomeArquivoFormControl: FormControl = new FormControl();

  CCForm = new FormGroup({
    tipoProcesso: this.tipoDeProcessoFormControl,
    configExec: this.periodicidadeExecucaoFormControl,
    nomArquivoBase: this.nomeArquivoFormControl,
    datAgendamento: this.dataEspecificaFormControl
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
  
  async redefinirListagem(): Promise<void> {
    this.activeModal.close(true);
  }

  async agendar(){
    if(await this.validarAgendamento()){
      await this.checkExecucao();
      return this.dialog.err('Erro ao salvar', 'Não foi possivel salvar o agendamento, verique os dados e tente novamente.')
    }

    try {
      this.dataEspecificaFormControl.setValue(new Date(this.dataEspecificaFormControl.value));
      await this.service.incluirAgendamento(this.CCForm.value);
      await this.dialog.alert('Agendamento concluído.', "Cálculo agendado com sucesso.");
      this.activeModal.close(true);
    } catch (error) {
      this.dialog.err('Erro ao salvar', error.error);
    }
  }

  async validarAgendamento(): Promise<boolean> {
    let hasError = false;
    const dataEspecifica = this.periodicidadeExecucaoFormControl.value == 1 && this.dataEspecificaFormControl.value != "Invalid Date" ? this.datePipe.transform(this.dataEspecificaFormControl.value, 'yyyy-MM-ddTHH:mm:ss.SSSZ') : null;
    
    this.CCForm.patchValue({
      periodicidadeExecucao: Number(this.periodicidadeExecucaoFormControl.value), 
      datEspecifica : dataEspecifica
    });
    
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
