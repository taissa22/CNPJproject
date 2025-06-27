import { Component, OnInit } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DialogService } from '@shared/services/dialog.service';
import { StaticInjector } from '../static-injector';
import { RelatorioMovimentacaoTrabalhistaFechamento } from '@relatorios/models/relatorio-movimentacao-trabalhista-fechamento';
import { RelatorioMovimentacaoTrabalhistaService } from '../services/relatorio-movimentacao-trabalhista.service';
import { RelatorioMovimentacaoTrabalhistaAgendamento } from '@relatorios/models/relatorio-movimentacao-trabalhista-agendamento';
import { DatePipe } from '@angular/common';
import { TrabalhistaResultadoService } from 'src/app/agenda/agendaDeAudiencias/trabalhista-filtros/trabalhista-resultado/services/trabalhistaResultado.service';

@Component({
  selector: 'app-agendamento-relatorio-movimentacao-trabalhista',
  templateUrl: './agendamento-relatorio-movimentacao-trabalhista.component.html',
  styleUrls: ['./agendamento-relatorio-movimentacao-trabalhista.component.scss']
})
export class AgendamentoRelatorioMovimentacaoTrabalhistaComponent implements OnInit {

  agendamentoNovo = new RelatorioMovimentacaoTrabalhistaAgendamento;
  fechamentosInicial: RelatorioMovimentacaoTrabalhistaFechamento[];
  fechamentosFinal: RelatorioMovimentacaoTrabalhistaFechamento[];
  FechamentoIni: any;
  FechamentoFim: any;
  datFechamentoIni: any;
  datFechamentoFim: any;

  dataFechamentoInicialIni: Date;
  dataFechamentoInicialFim: Date;
  dataFechamentoFinalIni: Date;
  dataFechamentoFinalFim: Date;

  paginaFechamentoInicial: number;
  paginaFechamentoFinal: number
  quantidadePagina = 5;

  totalFechamentosInicial: number;
  totalFechamentosFinal: number;

  checkedInicial: boolean;
  checkedFinal: boolean;

  constructor(
    private service: RelatorioMovimentacaoTrabalhistaService,
    private modal: NgbActiveModal,
    private dialogService: DialogService,
    private datePipe: DatePipe,
    public servicePaginacao: TrabalhistaResultadoService,
  ) { }

  ngOnInit() {
    this.paginaFechamentoInicial = 1;
    this.paginaFechamentoFinal = 1;
    this.checkedInicial = false;
    this.checkedFinal = false;
    this.obterFechamentosInicial()
    this.obterFechamentosFinal()
  }

  async obterFechamentosInicial(dataInicial?, dataFinal?, page?, checked?) {
    
    dataInicial = this.datePipe.transform(dataInicial, 'yyyy-MM-dd')
    dataFinal = this.datePipe.transform(dataFinal, 'yyyy-MM-dd')
    if(page == 0){
      this.servicePaginacao.paginatorService.setPage(1);
      this.paginaFechamentoInicial = 1;  
    }
    var result = await this.service.obterFechamentos(dataInicial, dataFinal, page == undefined ? 0 : page, checked == undefined? false : checked)
    this.fechamentosInicial = result.data;
    this.totalFechamentosInicial = result.total;
    return;
  }

  async obterFechamentosFinal(dataInicial?, dataFinal?, page?, checked?) {
    dataInicial = this.datePipe.transform(dataInicial, 'yyyy-MM-dd')
    dataFinal = this.datePipe.transform(dataFinal, 'yyyy-MM-dd')
    if(page == 0){
      this.servicePaginacao.paginatorService.setPage(1);
      this.paginaFechamentoFinal = 1;  
    }
    var result = await this.service.obterFechamentos(dataInicial, dataFinal, page == undefined ? 0 : page, checked == undefined? false : checked)
    this.fechamentosFinal = result.data;
    this.totalFechamentosFinal = result.total;
    return;
  }

  static exibeModal(): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      AgendamentoRelatorioMovimentacaoTrabalhistaComponent,
      { centered: true, backdrop: 'static', size: 'lg', windowClass: "modal-agend-movimentacao-trabalhista" }
    );
    return modalRef.result;
  }

  close(): void {
    this.modal.close(false);
  }

  selecionarFechamentoInicial(data): void {
    this.FechamentoIni = data;
    this.agendamentoNovo.iniCodTipoOutlier = data.codTipoOutlier;
    this.agendamentoNovo.iniDataFechamento = data.dataFechamento;
    this.agendamentoNovo.iniEmpresas = data.empresasGrupo;
    this.agendamentoNovo.iniIndMensal = data.indFechamentoMensal;
    this.agendamentoNovo.iniIndFechamentoParcial = data.indFechamentoParcial;
    this.agendamentoNovo.iniNumMesesMediaHistorica = data.numeroMeses;
    this.agendamentoNovo.iniValOutlier = data.valOutlier

    this.datFechamentoIni = this.agendamentoNovo.iniDataFechamento;
  }

  selecionarFechamentoFinal(data): void {
    this.FechamentoFim = data;
    this.agendamentoNovo.fimCodTipoOutlier = data.codTipoOutlier;
    this.agendamentoNovo.fimDataFechamento = data.dataFechamento;
    this.agendamentoNovo.fimEmpresas = data.empresasGrupo;
    this.agendamentoNovo.fimIndMensal = data.indFechamentoMensal;
    this.agendamentoNovo.fimIndFechamentoParcial = data.indFechamentoParcial;
    this.agendamentoNovo.fimNumMesesMediaHistorica = data.numeroMeses;
    this.agendamentoNovo.fimValOutlier = data.valOutlier

    this.datFechamentoFim = this.agendamentoNovo.fimDataFechamento;
  }

  async agendar(): Promise<void> {
    try {
      await this.service.agendar(this.agendamentoNovo);
      await this.dialogService.alert('Agendado com sucesso.');
      this.modal.close(true);
    } catch (error) {
      if (error && error.error) {
        await this.dialogService.err('Agendamento não realizado.', error.error);
        return;
      }
      await this.dialogService.err('Agendamento não realizado.');
    }
  }

  async verificaDatas(): Promise<void> {

    if (this.FechamentoIni === undefined || this.FechamentoFim === undefined) {
      await this.dialogService.info('Selecione um fechamento inicial e um final para compor o relatório!');
    }
    else if (this.datFechamentoIni >= this.datFechamentoFim) {
      await this.dialogService.info('A data do fechamento inicial deve ser menor do que a data do fechamento final!');
    }
    else {
      this.agendar();
    }
  }

  async atualizarPaginaInicial(event) {
    this.servicePaginacao.paginatorService.setPage(event);
    this.paginaFechamentoInicial = event;
    this.obterFechamentosInicial(this.dataFechamentoInicialIni, this.dataFechamentoInicialFim, this.paginaFechamentoInicial - 1);
  }

  async atualizarPaginaFinal(event) {
    this.servicePaginacao.paginatorService.setPage(event);
    this.paginaFechamentoFinal = event;
    this.obterFechamentosFinal(this.dataFechamentoFinalIni, this.dataFechamentoFinalFim, this.paginaFechamentoFinal - 1);
  }

  listarFechamentosMensaisInicial() {
    this.checkedInicial = !this.checkedInicial
    this.paginaFechamentoInicial = 1;
    let check = document.getElementById("fechamentoMensalInicialCheck") as HTMLInputElement;
    check.checked = this.checkedInicial;
    return this.obterFechamentosInicial(this.dataFechamentoInicialIni, this.dataFechamentoInicialFim, this.paginaFechamentoInicial - 1, this.checkedInicial);
  }

  listarFechamentosMensaisFinal() {
    this.paginaFechamentoFinal = 1;
    this.checkedFinal = !this.checkedFinal
    let check = document.getElementById("fechamentoMensalFinalCheck") as HTMLInputElement;
    check.checked = this.checkedFinal;
    return this.obterFechamentosFinal(this.dataFechamentoFinalIni, this.dataFechamentoFinalFim, this.paginaFechamentoFinal - 1, this.checkedFinal);
  }

  async limparFiltrosInicial() {
    this.dataFechamentoInicialIni = null;
    this.dataFechamentoInicialFim = null;
    this.paginaFechamentoInicial = 1;
    this.checkedInicial = false;
    let check = document.getElementById("fechamentoMensalInicialCheck") as HTMLInputElement;
    check.checked = false;
    await this.obterFechamentosInicial();
  }

  async limparFiltrosFinal() {
    this.dataFechamentoFinalIni = null;
    this.dataFechamentoFinalFim = null;
    this.paginaFechamentoFinal = 1;
    this.checkedFinal = false;
    let check = document.getElementById("fechamentoMensalFinalCheck") as HTMLInputElement;
    check.checked = false;
    await this.obterFechamentosFinal();
  }
}