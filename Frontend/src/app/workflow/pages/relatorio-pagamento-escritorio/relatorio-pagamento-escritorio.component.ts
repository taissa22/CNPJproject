import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { DialogService } from '@shared/services/dialog.service';
import { TrabalhistaResultadoService } from 'src/app/agenda/agendaDeAudiencias/trabalhista-filtros/trabalhista-resultado/services/trabalhistaResultado.service';
import { RelatorioPagamentoEscritorioResponse } from '../../model/relatorio-pagamento-escritorio-response';
import { Permissoes } from '@permissoes';
import { RelatorioPagamentoEscritorioModalComponent } from '../../modal/relatorio-pagamento-escritorio-modal/relatorio-pagamento-escritorio-modal.component';
import { RelatorioPagamentoEscritorioService } from '../../services/relatorio-pagamento-escritorio.service';

@Component({
  selector: 'app-relatorio-pagamento-escritorio',
  templateUrl: './relatorio-pagamento-escritorio.component.html',
  styleUrls: ['./relatorio-pagamento-escritorio.component.scss']
})
export class RelatorioPagamentoEscritorioComponent implements OnInit {

  constructor(
    private service: RelatorioPagamentoEscritorioService,
    private dialog: DialogService,
    private datePipe: DatePipe,
    private breadcrumbsService: BreadcrumbsService,
    public servicePaginacao: TrabalhistaResultadoService
  ) {}
  breadcrumb: string;

  agendamentos: Array<RelatorioPagamentoEscritorioResponse>;

  dataExecucaoIni: Date;
  dataExecucaoFim: Date;

  pagina: number;
  quantidadePagina = 5;
  totalRegistro: number;

  async ngOnInit() {
    this.pagina = 1;
    await this.refresh();
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(
      Permissoes.ACESSAR_RELATORIO_PAGAMENTO_ESCRITORIO
    );
  }

  async refresh(): Promise<void> {
    this.pagina = 1;
    await this.obterAgendamentos(
      this.pagina,
      this.dataExecucaoIni,
      this.dataExecucaoFim
    );
  }

  async obterAgendamentos(page, dataInicial?, dataFinal?) {
    dataInicial = this.datePipe.transform(dataInicial, 'yyyy-MM-dd');
    dataFinal = this.datePipe.transform(dataFinal, 'yyyy-MM-dd');
    try {
      await this.service
        .obterAgendamentos(page - 1, dataInicial, dataFinal)
        .then(x => {
          this.agendamentos = x.agendamentos;
          this.totalRegistro = x.total;
        });
    } catch (error) {
      await this.dialog.err(
        'Erro ao buscar.',
        'Não foi possível realizar a busca dos agendamentos.'
      );
    }
  }

  async novoAgendamento(): Promise<void> {
    const changes = await RelatorioPagamentoEscritorioModalComponent.exibeModal();
    if(changes)
      this.refresh();
  }

  async removerAgendamento(codAgendamento: number): Promise<void> {
    let confirm = await this.dialog.confirm(
      'Excluir agendamento.',
      'Confirma a exclusão deste agendamento?'
    );
    if (confirm) {
      try {
        await this.service.excluirAgendamento(codAgendamento);
        await this.dialog.alert(
          'Agendamento Excluído.',
          'Agendamento excluído com sucesso'
        );
        await this.obterAgendamentos(this.pagina);
      } catch (error) {
        await this.dialog.err(
          'Erro ao excluir',
          'Não foi possível realizar a exclusão do agendamento.'
        );
      }
    }
  }

  async downloadRelatorio(agendamentoId: number) {
    try {
      await this.service.exportarArquivoRelatorio(agendamentoId);
    } catch (error) {
      await this.dialog.err(
        'Erro no download',
        'Não foi possível realizar o download do relatório.'
      );
    }
  }

  async atualizarPaginaInicial(event) {
    await this.servicePaginacao.paginatorService.setPage(event);
    this.pagina = event;
    await this.obterAgendamentos(
      this.pagina,
      this.dataExecucaoIni,
      this.dataExecucaoFim
    );
  }

}
