import { Component, OnInit } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DialogService } from '@shared/services/dialog.service';
import { StaticInjector } from '../static-injector';
import { RelatorioMovimentacaoJecService } from '../services/relatorio-movimentacao-jec.service';
import { DatePipe } from '@angular/common';
import { TrabalhistaResultadoService } from 'src/app/agenda/agendaDeAudiencias/trabalhista-filtros/trabalhista-resultado/services/trabalhistaResultado.service';
import { MovimentacaoJec } from '../entities/movimentacao-jec';
import { FechamentoJec } from '../entities/fechamento-jec';


@Component({
  selector: 'app-agendamento-relatorio-movimentacao-jec-modal',
  templateUrl: './agendamento-relatorio-movimentacao-jec-modal.component.html',
  styleUrls: ['./agendamento-relatorio-movimentacao-jec-modal.component.scss']
})
export class AgendamentoRelatorioMovimentacaoJecModalComponent implements OnInit {

  fechamentoNovo = new MovimentacaoJec;
  fechamentosInicial: FechamentoJec[];
  fechamentosFinal: FechamentoJec[];

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
    private service: RelatorioMovimentacaoJecService,
    private modal: NgbActiveModal,
    private dialogService: DialogService,
    private datePipe: DatePipe,
    public servicePaginacao: TrabalhistaResultadoService,
  ) { }

  async ngOnInit(): Promise<void> {
    this.paginaFechamentoInicial = 1;
    this.paginaFechamentoFinal = 1;
    this.checkedInicial = false;
    this.checkedFinal = false;
    this.obterFechamentosInicial();
    this.obterFechamentosFinal();
  }

  close(): void {
    this.modal.close(false);
  }

  async agendar(): Promise<void> {
    try {
      await this.service.agendar(this.fechamentoNovo);
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

  static exibeModal(): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      AgendamentoRelatorioMovimentacaoJecModalComponent,
      { centered: true, backdrop: 'static', size: 'lg', windowClass: "modal-agend-movimentacao-jec" }
    );
    return modalRef.result;
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

  selecionarFechamentoInicial(data): void {
    this.FechamentoIni = data;
    this.fechamentoNovo.idBaseMovIni = data.id;
    this.fechamentoNovo.iniDataFechamento = data.dataFechamento;
    this.fechamentoNovo.iniNumMesesMediaHistorica = data.numeroMeses;
    this.fechamentoNovo.iniIndMensal = data.indicaFechamentoMensal == 'S' ? "S" : "N";
    this.fechamentoNovo.iniPercentualHaircut = data.percHaircut == null ? 0 : data.percHaircut;
    this.fechamentoNovo.iniValCorteOutliers = data.valorCorteOutliers;
    this.fechamentoNovo.iniIndFechamentoParcial = data.indicaFechamentoParcial;
    this.fechamentoNovo.iniEmpresas = data.empresas;

    this.datFechamentoIni = this.fechamentoNovo.iniDataFechamento;
  }

  selecionarFechamentoFinal(data): void {
    this.FechamentoFim = data;
    this.fechamentoNovo.idBaseMovFim = data.id;
    this.fechamentoNovo.fimDataFechamento = data.dataFechamento;
    this.fechamentoNovo.fimNumMesesMediaHistorica = data.numeroMeses;
    this.fechamentoNovo.fimIndMensal = data.indicaFechamentoMensal == 'S' ? "S" : "N";
    this.fechamentoNovo.fimPercentualHaircut = data.percHaircut == null ? 0 : data.percHaircut;
    this.fechamentoNovo.fimValCorteOutliers = data.valorCorteOutliers;
    this.fechamentoNovo.fimIndFechamentoParcial = data.indicaFechamentoParcial;
    this.fechamentoNovo.fimEmpresas = data.empresas;

    this.datFechamentoFim = this.fechamentoNovo.fimDataFechamento;
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
