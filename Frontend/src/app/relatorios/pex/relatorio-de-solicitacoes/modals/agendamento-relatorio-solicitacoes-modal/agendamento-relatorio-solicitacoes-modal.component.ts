import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { NgbActiveModal, NgbDateAdapter, NgbDateNativeAdapter, NgbDateParserFormatter, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { NgbDateCustomParserFormatter } from '@shared/dateformat';
import { connectableObservableDescriptor } from 'rxjs/internal/observable/ConnectableObservable';
import { DiasSemanaEnum } from '../../enuns/DiasSemanaRelatorio.enum';
import { TipoExecucaoRelatorioEnum } from '../../enuns/TipoExecucaoRelatorio.enum';
import { AgendamentoRelatorioSolicitacoesModel } from '../../models/agendamento-relatorio-de-solicitacoes.model';
import moment from 'moment';
import { DialogService } from '@shared/services/dialog.service';
import { DIR_DOCUMENT } from '@angular/cdk/bidi';
import { RelatorioDeSolicitacoesService } from '@relatorios/services/relatorios-de-solicitacoes.service';
import { HttpErrorResult } from '@core/http';
import { ModeloModel } from '../../models/modelo.model';
import { StaticInjector } from '../../static-injector';
@Component({
  selector: 'app-agendamento-relatorio-solicitacoes-modal',
  templateUrl: './agendamento-relatorio-solicitacoes-modal.component.html',
  styleUrls: ['./agendamento-relatorio-solicitacoes-modal.component.scss'],
  providers: [{ provide: NgbDateAdapter, useClass: NgbDateNativeAdapter }, { provide: NgbDateParserFormatter, useClass: NgbDateCustomParserFormatter }]
})
export class AgendamentoRelatorioSolicitacoesModalComponent implements OnInit {


  opcoesExecucao: string[] = [];
  diasSemana: string[] = [];
  diasMes = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31];
  
  descricao: FormControl = new FormControl("");
  nomeRelatorio: FormControl = new FormControl("");
  tipoExecucao: FormControl = new FormControl("Hoje");
  diaSemana: FormControl = new FormControl("Segunda-feira");
  diaMes: FormControl = new FormControl(1);
  dataExecucao: FormControl = new FormControl("");
  dataIni: FormControl = new FormControl("");
  dataFim: FormControl = new FormControl("");
  somenteEmDiasUteis: FormControl = new FormControl(false);

  formAgendamento: FormGroup = new FormGroup({
    nomeRelatorio: this.nomeRelatorio,
    descricao: this.descricao,
    tipoExecucao: this.tipoExecucao,
    diaSemana: this.diaSemana,
    diaMes: this.diaMes,
    dataExecucao: this.dataExecucao,
    dataIni: this.dataIni,
    dataFim: this.dataFim,
    somenteEmDiasUteis: this.somenteEmDiasUteis
  });

  agendamentoModel: AgendamentoRelatorioSolicitacoesModel = null;
  modeloModel = new ModeloModel();
  constructor(private modal: NgbActiveModal,
    private formBuilder: FormBuilder,
    private dialog: DialogService,
    public relatoriosService: RelatorioDeSolicitacoesService) { }

  ngOnInit() {
    this.listarTiposDeExecucao();
    this.listarDiasDaSemana();
  } 

  // --------- listagem ------

  listarTiposDeExecucao() {
    let optEnumList = Object.keys(TipoExecucaoRelatorioEnum);
    for (let i = 0; i < optEnumList.length; i++) {
      if (optEnumList.length / 2 >= i + 1) this.opcoesExecucao.push(TipoExecucaoRelatorioEnum[optEnumList[i]])
    } 
  }
  listarDiasDaSemana() {
    this.diasSemana = [];
    let diasEnumList = Object.keys(DiasSemanaEnum);
    for (let i = 0; i < diasEnumList.length; i++) {
      if (diasEnumList.length / 2 >= i + 1 &&
      (DiasSemanaEnum[diasEnumList[i]].toString() != 'Nenhum'))this.diasSemana.push(DiasSemanaEnum[diasEnumList[i]])
    }
  }

  listarDiasUteis() {
    this.diasSemana = [];
    let diasEnumList = Object.keys(DiasSemanaEnum);
    for (let i = 0; i < diasEnumList.length; i++) {
      if (diasEnumList.length / 2 >= i + 1 &&
      (DiasSemanaEnum[diasEnumList[i]].toString() != 'Nenhum' &&
       DiasSemanaEnum[diasEnumList[i]].toString() != 'Domingo' && 
       DiasSemanaEnum[diasEnumList[i]].toString() != 'Sábado')) this.diasSemana.push(DiasSemanaEnum[diasEnumList[i]]);
    }
  }
  // ---------------------------

  // ----- validacoes -------
  validoParaSalvar() {
    let valido = true;
    let form = this.formAgendamento.value;
    if (moment(form.dataFim).toDate() > moment(form.dataIni).add(1, 'years').toDate() || moment(form.dataExecucao).toDate() > moment( new Date()).add(1, 'years').toDate()) {
      this.dialog.err(
        'O agendamento tem período limite para daqui 1 ano'
      );
      valido = false;
    }
    
    if (moment(form.dataIni).format(moment.HTML5_FMT.DATE) < moment(Date.now()).format(moment.HTML5_FMT.DATE) || moment(form.dataFim).format(moment.HTML5_FMT.DATE) < moment(Date.now()).format(moment.HTML5_FMT.DATE) || moment(form.dataExecucao).format(moment.HTML5_FMT.DATE) < moment(Date.now()).format(moment.HTML5_FMT.DATE)) {
      this.dialog.err(
        'O agendamento deve ser configurado para uma data futura'
      );
      valido = false;
    }
    if ( moment(form.dataFim).format(moment.HTML5_FMT.DATE) < moment(form.dataIni).format(moment.HTML5_FMT.DATE) ) {
      this.dialog.err(
        'A data final não pode ser maior que a data inicial'
      );
      valido = false;
    }
    if (!this.nomeRelatorio.value || !this.nomeRelatorio.value.replace(/\s/g, '').length) {
      this.dialog.err(
        'Preencha o nome do relatório'
      );
      valido = false;
    }
    if (
      TipoExecucaoRelatorioEnum[this.tipoExecucao.value].toString() == TipoExecucaoRelatorioEnum['Semanalmente'].toString()
      && (!this.diaSemana.value || !this.diaSemana.value.replace(/\s/g, '').length)) {
      this.dialog.err(
        'Preencha o dia da semana'
      );
      valido = false;
    }
    if (
      TipoExecucaoRelatorioEnum[this.tipoExecucao.value].toString() == TipoExecucaoRelatorioEnum['Mensalmente'].toString()
      && !this.diaMes.value) {
      this.dialog.err(
        'Preencha o dia do mês'
      );
      valido = false;
    }
    if (
      TipoExecucaoRelatorioEnum[this.tipoExecucao.value].toString() == TipoExecucaoRelatorioEnum['Na data'].toString()
      && !this.dataExecucao.value) {
      this.dialog.err(
        'Preencha a data'
      );
      valido = false;
    }
    return valido;
  }

  resetarDatasInvalidas() {
    if (this.dataIni.invalid) {
      this.dataIni.reset();
    }
    if (this.dataFim.invalid) {
      this.dataFim.reset();
    }
    if (this.dataExecucao.invalid) {
      this.dataExecucao.reset();
    }
  }

  desabilitaHabilitaFinaisSemana() {
    if (this.somenteEmDiasUteis.value) {
      this.listarDiasUteis();
    }
    else {
      this.listarDiasDaSemana();
    }
  }

  desabilitaCheckDiasUteis() {
    if (this.diaSemana.value == 'Domingo' || this.diaSemana.value == 'Sábado') {
      this.somenteEmDiasUteis.disable();
    }
    else {
      this.somenteEmDiasUteis.enable();
    }
  }

  limparCamposDinamicos(){
    this.diaSemana.setValue("Segunda-feira");
    this.diaMes.setValue(1);
    this.dataExecucao.setValue("");
    this.dataIni.setValue("");
    this.dataFim.setValue("");
    this.somenteEmDiasUteis.setValue(false);
    this.desabilitaHabilitaFinaisSemana();
  }

  // ---------------- 

  // ----------- modal ---
  static exibeModal(obj: AgendamentoRelatorioSolicitacoesModel): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      AgendamentoRelatorioSolicitacoesModalComponent,
      { centered: true, backdrop: 'static', size: 'lg' }
    );
    modalRef.componentInstance.agendamentoModel = obj;
    return modalRef.result;
  }

  close(): void {
    this.modal.close();
  }
  // ------------


  async salvar() { 
    if (!this.validoParaSalvar()) return false; 
    try {
      await  this.relatoriosService.salvarAgendamentoRelSolicitacao(this.preencherModelAgendamento()).then(); 
      await this.dialog.alert('Agendado com sucesso'); 
      this.relatoriosService.salvarModelo$.next(this.modeloModel);
      this.close();
    } catch (error) {

      await this.dialog.err( error.error);
    } 
  }


  preencherModelAgendamento() {
    let form = this.formAgendamento.value;
    this.agendamentoModel.descricao = form.descricao;
    this.agendamentoModel.nomeDoRelatorio = form.nomeRelatorio;
    this.agendamentoModel.tipoExecucao = form.tipoExecucao ? parseInt(TipoExecucaoRelatorioEnum[form.tipoExecucao]) : 1;
    this.agendamentoModel.diaSemana = form.diaSemana ? parseInt(DiasSemanaEnum[form.diaSemana]) : 1;
    this.agendamentoModel.diaMes = form.diaMes;
    this.agendamentoModel.dataProxExecucao = form.dataExecucao? moment(form.dataExecucao).format(moment.HTML5_FMT.DATE) : "";
    this.agendamentoModel.dataIniAgendamento = form.dataIni ? moment(form.dataIni).format(moment.HTML5_FMT.DATE) : "";
    this.agendamentoModel.dataFimAgendamento = form.dataFim ? moment(form.dataFim).format(moment.HTML5_FMT.DATE) : "";
    this.agendamentoModel.somenteEmDiasUteis = form.somenteEmDiasUteis;
    this.preencherModelModelo(form);
    return this.agendamentoModel;
  }

  preencherModelModelo(form:any){
    this.modeloModel.nome = form.nomeRelatorio;
    this.modeloModel.descricao = form.Descricao;
  }



}
