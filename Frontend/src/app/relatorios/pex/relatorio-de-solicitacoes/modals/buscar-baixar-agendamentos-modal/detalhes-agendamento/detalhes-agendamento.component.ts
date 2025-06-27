import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'detalhes-agendamento',
  templateUrl: './detalhes-agendamento.component.html',
  styleUrls: ['./detalhes-agendamento.component.scss']
})
export class DetalhesAgendamentoComponent implements OnInit {

  @Input() agendamento:any;

  constructor() { }

  ngOnInit() {
  }

}
