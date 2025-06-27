import { Component, Input, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap';
import { ValorParteListas } from './IPartes';



@Component({
  selector: 'app-partes-modal',
  templateUrl: './partes-modal.component.html',
  styleUrls: ['./partes-modal.component.scss']
})
export class PartesModalComponent implements OnInit {

  constructor(public bsModalRef: BsModalRef ) { }

  @Input() partes: ValorParteListas = { autores: [], reus: []};

  @Input() titulo = 'Partes do Processo';
  @Input() subTitulo = 'Consulta dos dados dos autores e réus vinculados ao processo.';
  @Input() tituloAutor = 'Autores';
  @Input() tituloReu = 'Réus';

  ngOnInit() {
  }

}
