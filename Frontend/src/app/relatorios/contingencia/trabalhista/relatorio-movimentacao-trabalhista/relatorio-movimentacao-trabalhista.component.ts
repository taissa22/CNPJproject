import { Component, OnInit } from '@angular/core';
import { HelperAngular } from '@shared/helpers/helper-angular';
import { DialogService } from '@shared/services/dialog.service';
import { AgendamentoRelatorioMovimentacaoTrabalhistaComponent } from '../agendamento-relatorio-movimentacao-trabalhista/agendamento-relatorio-movimentacao-trabalhista.component';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { Permissoes } from '@permissoes';
import { RelatorioMovimentacaoTrabalhistaService } from '../services/relatorio-movimentacao-trabalhista.service';
import { RelatorioMovimentacaoTrabalhistaAgendamento } from '@relatorios/models/relatorio-movimentacao-trabalhista-agendamento';
import { DatePipe } from '@angular/common';
import { TrabalhistaResultadoService } from 'src/app/agenda/agendaDeAudiencias/trabalhista-filtros/trabalhista-resultado/services/trabalhistaResultado.service';


@Component({
  selector: 'app-relatorio-movimentacao-trabalhista',
  templateUrl: './relatorio-movimentacao-trabalhista.component.html',
  styleUrls: ['./relatorio-movimentacao-trabalhista.component.scss']
})
export class RelatorioMovimentacaoTrabalhistaComponent implements OnInit {

  constructor(
    private service: RelatorioMovimentacaoTrabalhistaService,
    private dialogService: DialogService,
    private messageService: HelperAngular,
    private breadcrumbService: BreadcrumbsService,
    private datePipe: DatePipe,
    public servicePaginacao: TrabalhistaResultadoService,
  ) { }

  agendamentos: Array<RelatorioMovimentacaoTrabalhistaAgendamento> = [];
  totalAgendamentos: number;
  breadcrumb: string;

  public listaExtracoesDiarias = [];
  public listaArquivosFechamentoDownload = [];
  public listaTiposProcessos = '';

  dataExecucaoIni: Date;
  dataExecucaoFim: Date;

  pagina: number;
  quantidadePagina = 5;

  ngOnInit(): void {
    this.pagina = 1;
    this.obterAgendamentos(this.dataExecucaoIni, this.dataExecucaoFim, this.pagina - 1);
  }

  async ngAfterViewInit(): Promise<void> {
    this.breadcrumb = await this.breadcrumbService.nomeBreadcrumb(Permissoes.ACESSAR_MOVIMENTACAO_TRABALHISTA);
  }

  async obterAgendamentos(dataInicial?, dataFinal?, page?) {
    
    dataInicial = this.datePipe.transform(dataInicial, 'yyyy-MM-dd')
    dataFinal = this.datePipe.transform(dataFinal, 'yyyy-MM-dd')
    if(page == 0){
      this.servicePaginacao.paginatorService.setPage(1);
      this.pagina = 1;  
    }
    let result = await this.service.obterAgendamentos(dataInicial, dataFinal, page);
    return this.agendamentos = result.data, this.totalAgendamentos = result.total;
  }

  async refresh(): Promise<void> {
    this.dataExecucaoIni = null;
    this.dataExecucaoFim = null;
    this.pagina = 1;
    this.obterAgendamentos(this.dataExecucaoIni, this.dataExecucaoFim, this.pagina - 1);
  }

  async novoAgendamento(): Promise<void> {
    await AgendamentoRelatorioMovimentacaoTrabalhistaComponent.exibeModal();
    this.refresh();
  }

  async removerAgendamento(agendamentoId: number): Promise<void> {
    try {
      this.messageService.MsgBox2('Confirma a exclusão deste agendamento?', 'Excluir Agendamento', 'question', 'Sim', 'Não')
        .then(resposta => {
          if (resposta.value) {
            this.service.excluirAgendamento(agendamentoId)
              .subscribe(() => {
                this.refresh();
              });
          }
        });
    }
    catch (error) {
      if (error && error.error) {
        await this.dialogService.err('Erro.', error.error);
        return;
      }

      await this.dialogService.err('remover não realizado.');
    }
  }

  downloadRelatorio(agendamentoId: number) {
    this.service.exportarArquivoRelatorio(agendamentoId);
  }

  async atualizarPaginaInicial(event) {
    this.servicePaginacao.paginatorService.setPage(event);
    this.pagina = event;
    this.obterAgendamentos(this.dataExecucaoIni, this.dataExecucaoFim, this.pagina - 1);
  }
}
