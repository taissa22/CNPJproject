import { Component, OnInit, SimpleChanges, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-filtro-busca-padrao',
  templateUrl: './filtro-busca-padrao.component.html',
  styleUrls: ['./filtro-busca-padrao.component.css']
})
export class FiltroBuscaPadraoComponent implements OnInit {


  constructor() { }

  @Input() label: string = '';

  @Input() placeholder: string ='';

  @Input() posTop: string = '20px';
  
  @Input() posLeft: string = '6px';

  @Output() nomeBusca: EventEmitter<string> = new EventEmitter<string>();

  ngOnInit() {
  }

  buscar(){
      this.nomeBusca.emit(this.label)
  }

}
