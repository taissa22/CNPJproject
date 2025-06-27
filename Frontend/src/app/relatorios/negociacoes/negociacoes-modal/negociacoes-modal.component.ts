import { Component, OnInit } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { RelatorioNegociacaoService } from '@relatorios/services/relatorio-negociacao.service';
import { StaticInjector } from '@shared/static-injector';
import { DialogService } from '@shared/services/dialog.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-negociacoes-modal',
  templateUrl: './negociacoes-modal.component.html',
  styleUrls: ['./negociacoes-modal.component.scss']
})
export class NegociacoesModalComponent implements OnInit {

  constructor(
    private service: RelatorioNegociacaoService,
    private dialog: DialogService,
    private modal: NgbActiveModal,
    private datePipe: DatePipe
  ) { }

  ngOnInit() {

    this.obterDiaSemana();
    this.obterDiaMes();
    this.obterPeriodoSemanal();
    this.obterPeriodoMensal();

  }

  indProcessoCcFormControl = new FormControl(false);
  indProcessoJecFormControl = new FormControl(false);
  indProcessoProconFormControl = new FormControl(false);
  periodicidadeExecucaoFormControl = new FormControl(1);
  datProxExecFormControl = new FormControl(null);
  diaDaSemanaFormControl = new FormControl(null);
  indUltimoDiaMesFormControl = new FormControl(false);
  diaDoMesFormControl = new FormControl(null);
  datInicioNegociacaoFormControl = new FormControl(null);
  datFimNegociacaoFormControl = new FormControl(null);
  periodoSemanalFormControl = new FormControl(null);
  periodoMensalFormControl = new FormControl(null);
  indNegociacoesAtivasFormControl = new FormControl(false);
  
  formGroup: FormGroup = new FormGroup({
    indProcessoCc : this.indProcessoCcFormControl,
    indProcessoJec : this.indProcessoJecFormControl,
    indProcessoProcon : this.indProcessoProconFormControl,
    periodicidadeExecucao : this.periodicidadeExecucaoFormControl,
    datProxExec : this.datProxExecFormControl,
    diaDaSemana : this.diaDaSemanaFormControl,
    indUltimoDiaMes : this.indUltimoDiaMesFormControl,
    diaDoMes : this.diaDoMesFormControl,
    datInicioNegociacao : this.datInicioNegociacaoFormControl,
    datFimNegociacao : this.datFimNegociacaoFormControl,
    periodoSemanal : this.periodoSemanalFormControl,
    periodoMensal : this.periodoMensalFormControl,
    indNegociacoesAtivas : this.indNegociacoesAtivasFormControl,
  });

  diaSemanaList = [];
  diaMesList = [];
  periodoSemanalList = [];
  periodoMensalList = [];

  execucaoImediataDataAtual = new Date(new Date().getFullYear(), new Date().getMonth(), new Date().getDate());

  static exibeModalIncluir(): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      NegociacoesModalComponent,
      {
        windowClass: 'modal-negociacoes',
        centered: true,
        size: 'lg',
        backdrop: 'static'
      }
    );
    return modalRef.result;
  }

  close(): void {
    this.modal.close(false);
  }

  async agendar(){
    this.validarTipoProcesso();
    this.validarCamposPeriodicidade();

    let dataEspecificaValida = this.validarDataEspecifica();
    if(!dataEspecificaValida){
      return await this.dialog.err(
        `Inclusão não realizada`,
        'Campo data específica deve ser válido.'
      );
    }
    
    if(this.periodicidadeExecucaoFormControl.value == 1 || this.periodicidadeExecucaoFormControl.value == 2){
      if(!(this.datInicioNegociacaoFormControl.value && this.datFimNegociacaoFormControl.value)){
        return await this.dialog.err(
          `Inclusão não realizada`,
          'Período obrigatório.'
        );
      }

      let maiorQue1Ano = this.validaDiferencaMaisDeUmAno(this.datInicioNegociacaoFormControl.value, this.datFimNegociacaoFormControl.value);
      if(maiorQue1Ano){
        return await this.dialog.err(
          `Inclusão não realizada`,
          'Permitido informar apenas um período de no máximo 1 ano.'
        );
      }
      
      let dataInicialMaior = this.dataInicialMaior();
      if(dataInicialMaior){
        return await this.dialog.err(
          `Inclusão não realizada`,
          'A data do período inicial não pode ser maior do que a data de execução.'
        );
      }

    }
    setTimeout(() => {
      if(this.formGroup.valid)
        this.salvar()
    }, 100);
  }

  async salvar(){
    try {
      await this.service.incluirAgendamentosAsync(this.formGroup.value);
      await this.dialog.alert(
        'Inclusão realizada com sucesso!',
        'Agendamento incluído com sucesso.'
      );
      this.modal.close(true);
    } catch (error) {
      await this.dialog.err(
        'Inclusão não realizada',
        error
      );
    }
  }

  obterDiaSemana(){
    return this.diaSemanaList = [
      {id: 1, descricao:'Domingo'},
      {id: 2, descricao:'Segunda-feira'},
      {id: 3, descricao:'Terça-feira'},
      {id: 4, descricao:'Quarta-feira'},
      {id: 5, descricao:'Quinta-feira'},
      {id: 6, descricao:'Sexta-feira'},
      {id: 7, descricao:'Sábado'}
    ]
  }
  
  obterDiaMes(){
    for (let i = 1; i <= 30; i++) {
      this.diaMesList.push({ id: i, descricao: i.toString() });
    }
  }

  ultimoDiaMes(){
    if(!this.indUltimoDiaMesFormControl.value)
      return this.diaDoMesFormControl.setValue(null);
  }

  obterPeriodoSemanal(){
    return this.periodoSemanalList = [
      {id: 1, descricao: 'Última semana'},
      {id: 2, descricao: 'Último mês'}
    ]
  }
  
  obterPeriodoMensal(){
    return this.periodoMensalList = [
      {id: 1, descricao: 'Último mês'},
      {id: 2, descricao: 'Últimos 6 meses'},
      {id: 3, descricao: 'Último ano'}
    ]
  }


  limparValoresPeriodicidade(value){
    switch (value) {
      case '1':
        this.datProxExecFormControl.setValue(null);
        this.diaDaSemanaFormControl.setValue(null);
        this.indUltimoDiaMesFormControl.setValue(false);
        this.diaDoMesFormControl.setValue(null);
        this.periodoSemanalFormControl.setValue(null);
        this.periodoMensalFormControl.setValue(null);
      break;
      
      case '2':
        this.diaDaSemanaFormControl.setValue(null);
        this.indUltimoDiaMesFormControl.setValue(false);
        this.diaDoMesFormControl.setValue(null);
        this.periodoSemanalFormControl.setValue(null);
        this.periodoMensalFormControl.setValue(null);
      break;
      
      case '3':
        this.datProxExecFormControl.setValue(null);
        this.indUltimoDiaMesFormControl.setValue(false);
        this.diaDoMesFormControl.setValue(null);
        this.periodoMensalFormControl.setValue(null);    
        this.datInicioNegociacaoFormControl.setValue(null);
        this.datFimNegociacaoFormControl.setValue(null);
      break;
      
      case '4':
        this.datProxExecFormControl.setValue(null);
        this.diaDaSemanaFormControl.setValue(null);
        this.periodoMensalFormControl.setValue(null);
        this.datInicioNegociacaoFormControl.setValue(null);
        this.datFimNegociacaoFormControl.setValue(null);
      break;
    
      default:
      break;
    }
  }

  validarTipoProcesso(){
    setTimeout(() => {
      if( !this.indProcessoCcFormControl.value && !this.indProcessoJecFormControl.value && !this.indProcessoProconFormControl.value){
        this.indProcessoCcFormControl.setValidators(Validators.requiredTrue);
        this.indProcessoCcFormControl.updateValueAndValidity();
        this.indProcessoCcFormControl.markAsTouched();
        
        this.indProcessoJecFormControl.setValidators(Validators.requiredTrue);
        this.indProcessoJecFormControl.updateValueAndValidity();
        this.indProcessoJecFormControl.markAsTouched();
        
        this.indProcessoProconFormControl.setValidators(Validators.requiredTrue);
        this.indProcessoProconFormControl.updateValueAndValidity();
        this.indProcessoProconFormControl.markAsTouched();
      }else{
        this.indProcessoCcFormControl.clearValidators();
        this.indProcessoCcFormControl.updateValueAndValidity();
        this.indProcessoCcFormControl.markAsTouched();
        
        this.indProcessoJecFormControl.clearValidators();
        this.indProcessoJecFormControl.updateValueAndValidity();
        this.indProcessoJecFormControl.markAsTouched();
        
        this.indProcessoProconFormControl.clearValidators();
        this.indProcessoProconFormControl.updateValueAndValidity();
        this.indProcessoProconFormControl.markAsTouched();
      }
    }, 100);
  }
  
  validarCamposPeriodicidade(){
    setTimeout(() => {
      this.diaDaSemanaFormControl.clearValidators();
      this.diaDaSemanaFormControl.updateValueAndValidity();
      this.diaDaSemanaFormControl.markAsTouched();
      this.periodoSemanalFormControl.clearValidators();
      this.periodoSemanalFormControl.updateValueAndValidity();
      this.periodoSemanalFormControl.markAsTouched();
      
      this.diaDoMesFormControl.clearValidators();
      this.diaDoMesFormControl.updateValueAndValidity();
      this.diaDoMesFormControl.markAsTouched();
      this.periodoMensalFormControl.clearValidators();
      this.periodoMensalFormControl.updateValueAndValidity();
      this.periodoMensalFormControl.markAsTouched();
      
      
      switch (this.periodicidadeExecucaoFormControl.value) {
        case '3':
          this.diaDaSemanaFormControl.setValidators(Validators.required);
          this.diaDaSemanaFormControl.updateValueAndValidity();
          this.diaDaSemanaFormControl.markAsTouched();
          
          this.periodoSemanalFormControl.setValidators(Validators.required);
          this.periodoSemanalFormControl.updateValueAndValidity();
          this.periodoSemanalFormControl.markAsTouched();
          break;

        case '4':
          if(!this.indUltimoDiaMesFormControl.value){
            this.diaDoMesFormControl.setValidators(Validators.required);
            this.diaDoMesFormControl.updateValueAndValidity();
            this.diaDoMesFormControl.markAsTouched();
          }
          this.periodoMensalFormControl.setValidators(Validators.required);
          this.periodoMensalFormControl.updateValueAndValidity();
          this.periodoMensalFormControl.markAsTouched();
        break;

        default:
        break;

      }
    }, 100);
  }

  validarDataEspecifica(){
    if(this.periodicidadeExecucaoFormControl.value == 2 && (this.datProxExecFormControl.value == null || this.datProxExecFormControl.value == 'Invalid Date' || this.datProxExecFormControl.value < this.execucaoImediataDataAtual))
      return false;
    return true;
  }

  validaDiferencaMaisDeUmAno(dataInicio: Date, dataFim: Date) {
    const diferencaMilissegundos = Math.abs(dataFim.getTime() - dataInicio.getTime());

    // Convertendo a diferença para anos
    var umAnoEmMilissegundos = 1000 * 60 * 60 * 24 * 365; // 1 ano em milissegundos
    var diferencaAnos = diferencaMilissegundos / umAnoEmMilissegundos;

    return diferencaAnos > 1;
  }

  dataInicialMaior(){
    if(this.periodicidadeExecucaoFormControl.value == 1)
      return this.datePipe.transform(this.datInicioNegociacaoFormControl.value, 'dd/MM/yyyy') > this.datePipe.transform(this.execucaoImediataDataAtual, 'dd/MM/yyyy')
    
    if(this.periodicidadeExecucaoFormControl.value == 2)
      return this.datInicioNegociacaoFormControl.value > this.datProxExecFormControl.value
  }

  desabilitaTooltip(control: FormControl):boolean{
    return control.valid || control.untouched || control.disabled
  }
}
