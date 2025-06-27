import { Component, OnInit } from '@angular/core';
import { RelatorioMovimentacaoCCService } from '@relatorios/contingencia/movimentacoes/services/relatorio-movimentacao-cc.service';
import { HelperAngular } from '@shared/helpers/helper-angular';
import { DialogService } from '@shared/services/dialog.service';
import { AgendamentoRelatorioMovimentacaoCCModalComponent } from '../agendamento-relatorio-movimentacao-cc-modal/agendamento-relatorio-movimentacao-cc-modal.component';
import { DatePipe } from '@angular/common';
import { TrabalhistaResultadoService } from 'src/app/agenda/agendaDeAudiencias/trabalhista-filtros/trabalhista-resultado/services/trabalhistaResultado.service';
import { MovimentacaoCC } from '../entidades/movimentacao-cc';

@Component({
  selector: 'app-relatorio-movimentacao-cc',
  templateUrl: './relatorio-movimentacao-cc.component.html',
  styleUrls: ['./relatorio-movimentacao-cc.component.scss']
})
export class RelatorioMovimentacaoCCComponent implements OnInit {
  constructor(
    private service: RelatorioMovimentacaoCCService,
    private dialogService: DialogService,
    private messageService: HelperAngular,
    private datePipe: DatePipe,
    public servicePaginacao: TrabalhistaResultadoService
  ) { }

  movimentos: MovimentacaoCC[];
  totalmovimentos: number;

  dataExecucaoIni: Date;
  dataExecucaoFim: Date;

  pagina: number;
  quantidadePagina = 5;

  ngOnInit(): void {
    this.pagina = 1;
    this.refresh();
  }

  async refresh(): Promise<void> {
    this.dataExecucaoIni = null;
    this.dataExecucaoFim = null;
    this.pagina = 1;
    this.obterAgendamentos(this.dataExecucaoIni, this.dataExecucaoFim, this.pagina - 1);
  }

  async obterAgendamentos(dataInicial?, dataFinal?, page?) {
    
    dataInicial = this.datePipe.transform(dataInicial, 'yyyy-MM-dd')
    dataFinal = this.datePipe.transform(dataFinal, 'yyyy-MM-dd')
    if(page == 0){
      this.servicePaginacao.paginatorService.setPage(1);
      this.pagina = 1;  
    }
    let result = await this.service.obterAgendamentos(dataInicial, dataFinal, page);
    return this.movimentos = result.data, this.totalmovimentos = result.total;
  }

  async novoAgendamento(): Promise<void> {
    await AgendamentoRelatorioMovimentacaoCCModalComponent.exibeModal();
    this.refresh();
  }

  async removerAgendamento(agendamentoId: number): Promise<void> {
    try
    {
      this.messageService.MsgBox2('Confirma a exclusão deste agendamento?', 'Excluir Agendamento', 'question', 'Sim', 'Não')
      .then(resposta =>
        {
          if (resposta.value)
          {
            this.service.excluirAgendamento(agendamentoId)
            .subscribe(() => {
              this.refresh();
            });
          }
        });
    }
    catch (error)
    {
      if (error && error.error)
      {
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


