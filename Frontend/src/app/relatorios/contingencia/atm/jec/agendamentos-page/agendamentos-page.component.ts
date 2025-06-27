import { AfterViewInit, Component } from '@angular/core';
import { Permissoes } from '@permissoes';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { FechamentoAtmJecService } from '../../services/fechamento-atm-jec.service';
import { AgendamentoModalComponent } from '../agendamento-modal/agendamento-modal.component';

@Component({
  selector: 'app-agendamentos-page',
  templateUrl: './agendamentos-page.component.html',
  styleUrls: ['./agendamentos-page.component.scss']
})
export class AgendamentosPageComponent implements AfterViewInit {
  breadcrumb: string;
  constructor(
    private service: FechamentoAtmJecService,
    private breadcrumbsService: BreadcrumbsService
  ) { }

  private page: number = 0;
  hasMorePages: boolean = true;
  agendamentos: Array<Agendamento> = [];

  ngAfterViewInit(): void {
    this.refresh();
  }

  async refresh(): Promise<void> {
    this.agendamentos = [];
    this.page = 0;
    this.hasMorePages = true;
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(Permissoes.ACESSAR_ATM_JEC);
    await this.loadMore();
  }

  async loadMore(): Promise<void> {
    const result = await this.service.obterAgendamentos(this.page);

    console.log(result);

    result.data.forEach(x => this.agendamentos.push(x));
    this.page++;
    if (result.total == this.agendamentos.length) {
      this.hasMorePages = false;
    }
  }

  async novoAgendamento(): Promise<void> {
    await AgendamentoModalComponent.exibeModal();
    this.refresh();
  }

  downloadBase(agendamentoId: number) {
    this.service.exportarArquivoBase(agendamentoId);
  }

  downloadRelatorio(agendamentoId: number) {
    this.service.exportarArquivoRelatorio(agendamentoId);
  }
}
declare interface Fechamento {
  id: number;
  mesAnoFechamento: Date;
  dataFechamento: Date;
  numeroDeMeses: number;
  mensal: boolean;
}
declare interface Agendamento {
  id: number;
  mesAnoFechamento: Date;
  dataFechamento: Date;
  numeroDeMeses: number;
  codigoUsuario: string;
  nomeUsuario: string;
  dataAgendamento: Date;
  inicioDaExecucao: Date;
  fimDaExecucao: Date;
  status: number; // Status do agendamento (0 - Agendado, 1 - Processando, 2 - Finalizado, 3 - Erro)
  erro?: string;
}
