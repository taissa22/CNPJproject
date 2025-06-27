import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { SisjurPaginator } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.component';
import { AgendamentoRelatorioNegociacaoResponse } from '@relatorios/models/agendamento-relatorio-negociacao-response';
import { RelatorioNegociacaoService } from '@relatorios/services/relatorio-negociacao.service';
import { DialogService } from '@shared/services/dialog.service';
import { NegociacoesModalComponent } from './negociacoes-modal/negociacoes-modal.component';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { ErrorLib } from '@esocial/libs/error-lib';

@Component({
  selector: 'app-negociacoes',
  templateUrl: './negociacoes.component.html',
  styleUrls: ['./negociacoes.component.css']
})
export class NegociacoesComponent implements OnInit {

  @ViewChild(SisjurPaginator, { static: false }) paginator: SisjurPaginator;
  
  constructor(
    private service: RelatorioNegociacaoService,
    private dialog : DialogService,
    private breadcrumbsService: BreadcrumbsService
  ) { }

  async ngOnInit() {
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb('m_RelatorioNegociacoes');
    await this.obterAgendametnosAsync();
  }
  breadcrumb: string;

  agendamentos: Array<AgendamentoRelatorioNegociacaoResponse>;
  totalAgendamentos = 0;
  
  dataInicioAgendamentoFormControl = new FormControl();
  dataFimAgendamentoFormControl = new FormControl();
  
  buscaFormGroup: FormGroup = new FormGroup({
    dataInicioAgendamento : this.dataInicioAgendamentoFormControl,
    dataFimAgendamento : this.dataFimAgendamentoFormControl,
    page: new FormControl(0)
  });

  async obterAgendametnosAsync(){
    try {
      this.buscaFormGroup.get('page').setValue(this.iniciaValoresDaView() - 1);
      this.service.obterAgendamentosAsync(this.buscaFormGroup.value).then(x => {
        this.agendamentos = x.dadosAgendamento;
        this.totalAgendamentos = x.total;
      });
    } catch (error) {
      await this.dialog.err('Erro ao buscar', error);
      return;
    }
  }

  async excluir(cod: number): Promise<void> {
    const excluir: boolean = await this.dialog.confirm(
      'Excluir Agendamento',
      `Deseja excluir o Agendamento?`
    );

    if(excluir){
      try {
        await this.service.removerAgendamentoAsync(cod);
        await this.dialog.alert(
          'Exclusão realizada com sucesso',
          'Agendamento excluído!'
        );
        this.obterAgendametnosAsync();
      } catch (error) {
        await this.dialog.err('Erro ao excluir', error);
      return;
      }
    }

  }

  async exportarRetorno(codAgendExecRelNegociacao: number){
    try {
      return await this.service.downloadAgendamento(codAgendExecRelNegociacao);
    } catch (error) {
      await this.dialog.err(`Erro ao baixar o agendamento ${codAgendExecRelNegociacao}`, error);
    }
  }

  async incluir(): Promise<void> {
    const teveAlteracao: boolean = await NegociacoesModalComponent.exibeModalIncluir();

    if (teveAlteracao) {
      this.obterAgendametnosAsync();
    }
  }

  iniciaValoresDaView() {
    return this.paginator === undefined ? 1 : this.paginator.pageIndex + 1
  }

  colorStatus(status: string): string{
    switch (status) {
      case "Agendado":
        return '#3270A7'
      case "Processando":
        return '#FF8C00'
      case "Finalizado":
        return '#19A519'
      case "Erro":
        return '#F80000'
      default:
        return '';
    }
  }

}
