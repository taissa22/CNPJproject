import { Component, OnInit } from '@angular/core';
import { Permissoes } from '@permissoes';
import { RelatorioMovimentacaoPexService } from '@relatorios/contingencia/movimentacoes/services/relatorio-movimentacao-pex.service';
import { HelperAngular } from '@shared/helpers/helper-angular';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { DialogService } from '@shared/services/dialog.service';
import { AgendamentoRelatorioMovimentacaoPexModalComponent } from '../agendamento-relatorio-movimentacao-pex-modal/agendamento-relatorio-movimentacao-pex-modal.component';

@Component({
  selector: 'app-relatorio-movimentacao-pex',
  templateUrl: './relatorio-movimentacao-pex.component.html',
  styleUrls: ['./relatorio-movimentacao-pex.component.scss']
})
export class RelatorioMovimentacaoPexComponent implements OnInit {
  constructor(
    private service: RelatorioMovimentacaoPexService,
    private dialogService: DialogService,
    private messageService: HelperAngular,
    private breadcrumbsService: BreadcrumbsService
  ) { }

  private page: number = 0;
  hasMorePages: boolean = true;
  agendamentos: Array<Agendamento> = [];
  breadcrumb: string;

  public listaExtracoesDiarias = [];
  public listaArquivosFechamentoDownload = [];
  // public listaTiposProcessos = DownloadTiposProcesso;
  public listaTiposProcessos = '';

  ngOnInit(): void {
    this.refresh();
  }

  async ngAfterViewInit(): Promise<void> {
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(Permissoes.ACESSAR_RELATORIO_MOVIMENTACAO_PEX);
  }

  async refresh(): Promise<void> {
    this.agendamentos = [];
    this.page = 0;
    this.hasMorePages = true;
    await this.loadMore();
  }

  async loadMore(): Promise<void> {
    const result = await this.service.obterAgendamentos(this.page);
    result.data.forEach(x => this.agendamentos.push(x));
    console.log(result);
    console.log(this.agendamentos);
    this.page++;
    if (result.total == this.agendamentos.length) {
      this.hasMorePages = false;
    }
  }

  async novoAgendamento(): Promise<void> {
    await AgendamentoRelatorioMovimentacaoPexModalComponent.exibeModal();
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
}

declare interface Fechamento {
  id: number;
  dataExecucao: Date;
  dataFechamento: Date;
  empresas: string;
  indAplicarHaircut: string;
  multDesvioPadrao: number;
  nomeUsuario: string;
  numeroMeses: number;
  percentualHaircut: number;
  dataAgendamento : Date
}

declare interface Agendamento {
  id: number;
  datAgendamento: Date;
  fechamentoPexMediaIniCodSolic: number;
  fechamentoPexMediaFimCodSolic: number;
  datFechamentoIni: Date;
  datFechamentoFim: Date;
  datInicioExecucao: Date;
  datFimExecucao: Date;
  fechamentoFim: Fechamento;
  fechamentoIni: Fechamento;
  status: number; // Status do agendamento (0 - Agendado, 1 - Processando, 2 - Finalizado, 3 - Erro)
  msgErro: string;
  usuarioId: number;
  usuarioNome: string;
}
