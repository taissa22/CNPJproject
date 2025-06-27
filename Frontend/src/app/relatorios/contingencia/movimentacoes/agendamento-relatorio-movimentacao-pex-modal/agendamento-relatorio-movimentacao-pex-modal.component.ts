import { Component, OnInit } from '@angular/core';
import { RelatorioMovimentacaoPexService } from '@relatorios/contingencia/movimentacoes/services/relatorio-movimentacao-pex.service';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { HelperAngular } from '@shared/helpers/helper-angular';
import { DialogService } from '@shared/services/dialog.service';
import { StaticInjector } from '../static-injector';

@Component({
  selector: 'app-agendamento-relatorio-movimentacao-pex-modal',
  templateUrl: './agendamento-relatorio-movimentacao-pex-modal.component.html',
  styleUrls: ['./agendamento-relatorio-movimentacao-pex-modal.component.scss']
})
export class AgendamentoRelatorioMovimentacaoPexModalComponent implements OnInit {

  fechamentos: Array<Fechamento> = [];
  fechamentoPexMediaIniCodSolic= null;
  fechamentoPexMediaFimCodSolic = null;
  datFechamentoIni: Date;
  datFechamentoFim: Date;

  constructor(
    private service: RelatorioMovimentacaoPexService,
    private modal: NgbActiveModal,
    private dialogService: DialogService,
    private messageService: HelperAngular
  ) {}



  async ngOnInit(): Promise<void> {
    this.fechamentos = await this.service.obterFechamentos();
    console.log(this.fechamentos);
  }

  close(): void {
    this.modal.close(false);
  }

  selecionarFechamentoInicial(id, data): void {
    this.fechamentoPexMediaIniCodSolic = id;
    this.datFechamentoIni = data;
  }

  selecionarFechamentoFinal(id, data): void {
    this.fechamentoPexMediaFimCodSolic = id;
    this.datFechamentoFim = data;
  }

  async agendar(): Promise<void> {
    const fechamentosSolicitados = {
      FechamentoPexMediaIniCodSolic: this.fechamentoPexMediaIniCodSolic,
      DatFechamentoIni: this.datFechamentoIni,
      FechamentoPexMediaFimCodSolic: this.fechamentoPexMediaFimCodSolic,
      DatFechamentoFim: this.datFechamentoFim
    }
    try {
      await this.service.agendar(fechamentosSolicitados);
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

    if (this.fechamentoPexMediaIniCodSolic === null || this.fechamentoPexMediaFimCodSolic === null) {
      await this.dialogService.info('Selecione os fechamentos inicial e final que irão compor o relatório!');
    }
    else if(this.datFechamentoIni >= this.datFechamentoFim){
      await this.dialogService.info('A data do fechamento inicial deve ser menor do que a data do fechamento final!');
    }
    else {
      this.agendar();
    }
  }

  static exibeModal(): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      AgendamentoRelatorioMovimentacaoPexModalComponent,
      { centered: true, backdrop: 'static',size: 'lg', windowClass:"modal-agend-movimentacao-pex"}
    );
    return modalRef.result;
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
  dataAgendamento : Date;
}
