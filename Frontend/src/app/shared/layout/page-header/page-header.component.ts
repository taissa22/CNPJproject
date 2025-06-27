import { Component, Input, Output, EventEmitter } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-page-header',
  templateUrl: './page-header.component.html',
  styleUrls: ['./page-header.component.scss']
})
export class PageHeaderComponent {
  @Input()
  public titulo: string;

  @Input()
  public caminho: string;

  @Input()
  public rotaVoltar: Array<string>;

  @Input()
  public exibeBotaoIncluir: boolean;

  @Output() aoClicarNovoRegistro = new EventEmitter();

  novo() {
    this.aoClicarNovoRegistro.emit();
  }
}
