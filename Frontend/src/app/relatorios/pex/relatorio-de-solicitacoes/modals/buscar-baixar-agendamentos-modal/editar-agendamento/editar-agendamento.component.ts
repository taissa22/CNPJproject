import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { DiasSemanaEnum } from '@relatorios/pex/relatorio-de-solicitacoes/enuns/DiasSemanaRelatorio.enum';
import { TipoExecucaoRelatorioEnum } from '@relatorios/pex/relatorio-de-solicitacoes/enuns/TipoExecucaoRelatorio.enum';
import { AgendamentoRelatorioSolicitacoesModel } from '@relatorios/pex/relatorio-de-solicitacoes/models/agendamento-relatorio-de-solicitacoes.model';
import { ModeloModel } from '@relatorios/pex/relatorio-de-solicitacoes/models/modelo.model';
import { RelatorioDeSolicitacoesService } from '@relatorios/services/relatorios-de-solicitacoes.service';
import { DialogService } from '@shared/services/dialog.service';
import moment from 'moment';
import * as _ from 'lodash';
@Component({
  selector: 'editar-agendamento-relatorio',
  templateUrl: './editar-agendamento.component.html',
  styleUrls: ['./editar-agendamento.component.scss']
})
export class EditarAgendamentoComponent implements OnInit {

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

  @Input() agendamentoModel: any = null;
  @Output() atualizarAgendamentos =  new EventEmitter();

  constructor(private modal: NgbActiveModal,
    private formBuilder: FormBuilder,
    private dialog: DialogService,
    public relatoriosService: RelatorioDeSolicitacoesService) { }

  ngOnInit() {
    this.listarTiposDeExecucao();
    this.listarDiasDaSemana();
    this.listarDiasUteis();
    this.preencherFormAgendamento()
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
        (DiasSemanaEnum[diasEnumList[i]].toString() != 'Nenhum')) this.diasSemana.push(DiasSemanaEnum[diasEnumList[i]])
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
    if (moment(form.dataFim).toDate() > moment(form.dataIni).add(1, 'years').toDate() || moment(form.dataExecucao).toDate() > moment(new Date()).add(1, 'years').toDate()) {
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
    if (moment(form.dataFim).format(moment.HTML5_FMT.DATE) < moment(form.dataIni).format(moment.HTML5_FMT.DATE)) {
      this.dialog.err(
        'A data inicial não pode ser maior que a data final'
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

  limparCamposDinamicos() {
    this.diaSemana.setValue("Segunda-feira");
    this.diaMes.setValue(1);
    this.dataExecucao.setValue("");
    this.dataIni.setValue("");
    this.dataFim.setValue("");
    this.somenteEmDiasUteis.setValue(false);
    this.desabilitaHabilitaFinaisSemana();
  }

  // ---------------- 

  async salvar() {
    if (!this.validoParaSalvar()) return false;
    let agendamento =  this.preencherModelAgendamento(); 
    try {
      await this.relatoriosService.editarAgendamentoRelSolicitacao(agendamento).then();
      await this.dialog.alert('Editado com sucesso');
      this.atualizarAgendamentos.emit(agendamento.idAgendamento);
    } catch (error) {
      await this.dialog.err(error.error);
    }
  }

  preencherModelAgendamento() {
    let form = this.formAgendamento.value;
    let agendamentoModel = _.cloneDeep(this.agendamentoModel);
    delete agendamentoModel.openEdit;
    delete agendamentoModel.openDetalhes;
    delete agendamentoModel.openHistorico;
    agendamentoModel.descricao = form.descricao;
    agendamentoModel.nomeDoRelatorio = form.nomeRelatorio;
    agendamentoModel.tipoExecucao = form.tipoExecucao ? parseInt(TipoExecucaoRelatorioEnum[form.tipoExecucao]) : 1;
    agendamentoModel.diaSemana = form.diaSemana ? parseInt(DiasSemanaEnum[form.diaSemana]) : 1;
    agendamentoModel.diaMes = form.diaMes;
    agendamentoModel.dataProxExecucao = form.dataExecucao ? moment(form.dataExecucao).format(moment.HTML5_FMT.DATE) : "";
    agendamentoModel.dataIniAgendamento = form.dataIni ? moment(form.dataIni).format(moment.HTML5_FMT.DATE) : "";
    agendamentoModel.dataFimAgendamento = form.dataFim ? moment(form.dataFim).format(moment.HTML5_FMT.DATE) : "";
    agendamentoModel.somenteEmDiasUteis = form.somenteEmDiasUteis;
    if(agendamentoModel.nomeArquivoGerado == null) agendamentoModel.nomeArquivoGerado = "";  
    return agendamentoModel;
  }

  preencherFormAgendamento() {
    this.nomeRelatorio.setValue(this.agendamentoModel.nomeDoRelatorio);
    this.descricao.setValue(this.agendamentoModel.descricao);
    this.tipoExecucao.setValue(this.agendamentoModel.tipoExecucao); 
    this.diaSemana.setValue(this.agendamentoModel.diaSemana);
    this.diaMes.setValue(this.agendamentoModel.diaMes);

    if (this.agendamentoModel.dataProxExecucao) {
      let arr = this.agendamentoModel.dataProxExecucao.split("-");
      this.dataExecucao.setValue(new Date(parseInt(arr[0]), parseInt(arr[1]) - 1, parseInt(arr[2])));
    }

    if (this.agendamentoModel.dataIniAgendamento) {
      let arr = this.agendamentoModel.dataIniAgendamento.split("-");
      this.dataIni.setValue(new Date(parseInt(arr[0]), parseInt(arr[1]) - 1, parseInt(arr[2])));
    }

    if (this.agendamentoModel.dataFimAgendamento) {
      let arr = this.agendamentoModel.dataFimAgendamento.split("-");
      this.dataFim.setValue(new Date(parseInt(arr[0]), parseInt(arr[1]) - 1, parseInt(arr[2])));
    }

    this.somenteEmDiasUteis.setValue(this.agendamentoModel.somenteEmDiasUteis == "S")
  }
}
