import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { __importDefault } from 'tslib';
import { BotaoGridState } from '@shared/interfaces/botao-grid-state';

@Component({
  selector: 'app-botao-ordenacao',
  templateUrl: './botao-ordenacao.component.html',
  styleUrls: ['./botao-ordenacao.component.scss']
})
export class BotaoOrdenacaoComponent implements OnInit {

  @Input() isActive = false;
  @Output() stateChange = new EventEmitter<BotaoGridState>();
  @Input() disabled = false;

  public ordemCrescente = true;

  constructor() { }

  ngOnInit() { }

  onClick() {
    if (this.isActive) {
      this.ordemCrescente = !this.ordemCrescente;
    } else {
      this.isActive = true;
    }

    this.stateChange.emit({
      isActive: this.isActive,
      ordemCrescente: this.ordemCrescente
    });
  }
}
