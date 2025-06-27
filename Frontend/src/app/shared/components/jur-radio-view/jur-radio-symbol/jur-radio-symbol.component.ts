import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'jur-radio-symbol',
  templateUrl: './jur-radio-symbol.component.html',
  styleUrls: ['./jur-radio-symbol.component.scss']
})
export class JurRadioSymbolComponent implements OnInit {
  @Input() primeiro = false;
  @Input() ultimo = false;
  @Input() selecionado = false;
  constructor() { }

  ngOnInit() {
  }

}
