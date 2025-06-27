import { AfterViewInit, Component, OnInit } from '@angular/core';
import { RelatorioAtmPexService } from '../../services/relatorio-atm-pex.service';
import { AgendamentoPexModalComponent } from '../agendamento-pex-modal/agendamento-pex-modal.component';
import { DialogService } from '@shared/services/dialog.service';
import { HelperAngular } from '@shared/helpers/helper-angular';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { Permissoes } from '@permissoes';
@Component({
  selector: 'app-relatorio-atm-pex',
  templateUrl: './relatorio-atm-pex.component.html',
  styleUrls: ['./relatorio-atm-pex.component.scss']
})
export class RelatorioAtmPexComponent implements AfterViewInit {

  constructor(
    private service: RelatorioAtmPexService,
    private dialogService: DialogService,
    private messageService: HelperAngular,
    private breadcrumbsService: BreadcrumbsService
  ) { }

  private page: number = 0;
  hasMorePages: boolean = true;
  agendamentos: Array<Agendamento> = [];

  public listaExtracoesDiarias = [];
  public listaArquivosFechamentoDownload = [];
  public listaTiposProcessos = '';

  fechamentos: Array<Fechamento> = [];
  indices: Array<any> = [];
  breadcrumb: string;

  async ngAfterViewInit(): Promise<void> {
    this.fechamentos = await this.service.obterFechamentos();
    this.indices = await this.service.obterIndices();
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(Permissoes.ACESSAR_ATM_PEX);
    this.refresh();
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
    this.page++;
    if (result.total == this.agendamentos.length) {
      this.hasMorePages = false;
    }
    console.log(this.agendamentos);
  }

  async novoAgendamento(): Promise<void> {
    await AgendamentoPexModalComponent.exibeModal(this.fechamentos, this.indices);
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

  downloadBase(agendamentoId: number) {
    this.service.exportarArquivoBase(agendamentoId);
  }

  downloadRelatorio(agendamentoId: number) {
    this.service.exportarArquivoRelatorio(agendamentoId);
  }
}

declare interface Agendamento {
  agendamentoId: number;
  codSolicFechamento: number;
  valDesvioPadrao: number;
  numeroDeMeses: number;
  dataFechamento: Date;
  dataAgendamento: Date;
  dataInicioExecucao: Date;
  dataFimExecucao: Date;
  fechamentoPexMediaResponse: Fechamento;
  nomeUsuario: string;
  status: number; // Status do agendamento (0 - Agendado, 1 - Processando, 2 - Finalizado, 3 - Erro)
  erro?: string;
}

declare interface Fechamento {
  id: number;
  codSolicFechamentoCont: number;
  multDesvioPadrao: number;
  dataFechamento: Date;
  numeroMeses: number;
  dataAgendamento: Date;
  dataExecucao: Date;
  empresas: string;
  indAplicarHaircut: string;
  nomeUsuario: string;
  percentualHaircut: number;
}
