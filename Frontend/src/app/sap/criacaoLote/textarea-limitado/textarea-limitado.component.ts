import { Component, OnInit, Input, Output, EventEmitter, OnDestroy } from '@angular/core';
import { TextareaLimitadoService } from '../services/textarea-limitado.service';


@Component({
  // tslint:disable-next-line: component-selector
  selector: 'textarea-limitado',
  templateUrl: './textarea-limitado.component.html',
  styleUrls: ['./textarea-limitado.component.css']
})
export class TextareaLimitadoComponent implements OnInit, OnDestroy {
  //#region Necessario colocar uma LABEL, dizer se o usuario poderá
  // redimensionar e o limite de caracteres
  @Input() labelTextArea: string;
  @Input() isResize = false;
  @Input() limitCaracteres;
  @Output() textoCompleto = new EventEmitter<string>();
  texto: string;

//#endregion

  constructor(private service: TextareaLimitadoService) { }

  ngOnInit() {
    this.service.onTextChange.subscribe(texto => this.texto = texto);
   }

  ngOnDestroy() {
    this.textoCompleto = null;

  }



  retornarTexto() {
    this.service.updateTexto(this.texto);
    this.textoCompleto.emit(this.texto);
  }

}
