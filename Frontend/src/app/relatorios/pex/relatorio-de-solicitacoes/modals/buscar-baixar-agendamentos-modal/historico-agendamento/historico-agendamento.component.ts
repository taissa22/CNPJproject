import { Component, Input, OnInit } from '@angular/core';
import { RelatorioDeSolicitacoesService } from '@relatorios/services/relatorios-de-solicitacoes.service';

@Component({
  selector: 'historico-agendamento',
  templateUrl: './historico-agendamento.component.html',
  styleUrls: ['./historico-agendamento.component.scss']
})
export class HistoricoAgendamentoComponent implements OnInit {
  @Input() agendamento: any ;

  agendamentos: any[] = [];

  constructor(public service: RelatorioDeSolicitacoesService) { }

  ngOnInit() {
    this.service.listaHistoricoExecucaoAgendamento(this.agendamento.idAgendamento).then(resposta =>{
      this.agendamentos = resposta; 
      this.agendamentos.map(a => {
        if(a.datUltExecucao) a.datUltExecucao = a.datUltExecucao.split(" ")[0];  
      })
    })
  }

}
