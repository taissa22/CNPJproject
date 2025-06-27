import { Component, OnInit, EventEmitter, Output, Input } from '@angular/core';

@Component({
  selector: 'app-table-options',
  templateUrl: './table-options.component.html',
  styleUrls: ['./table-options.component.scss']
})
export class TableOptionsComponent implements OnInit {

  @Input() quantidadeItens = 8;
  @Output() valueChange = new EventEmitter<number>();

  constructor() { }

  ngOnInit() { }

  onChange() {
    this.valueChange.emit(this.quantidadeItens);
  }
}
