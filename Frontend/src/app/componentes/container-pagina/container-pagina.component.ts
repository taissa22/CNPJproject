import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { Location } from '@angular/common';

/** @description Container total da página do sisjur
 * @input titulo titulo da pagina
 * @input caminho caminho o menu que fica após o titulo
 * @input textoRota texto do botão para voltar
 * @input rota rota de voltar
 * @input texto frase informativa
 * @input hasAdicionar verifica se possui botão de adicionar
 * @output adicionarClicked emit o evento que foi clicado o botão de add
 * @output voltou emit o evento que voltou a pagina
 */
@Component({
  selector: 'app-container-pagina',
  templateUrl: './container-pagina.component.html',
  styleUrls: ['./container-pagina.component.scss'],
  providers: [Location]
})
export class ContainerPaginaComponent implements OnInit {

  constructor() { }
  private textoBotao$: string = 'Incluir';
  @Input() set textoBotao(v: string) {
    this.textoBotao$ = v;
  }
  get textoBotao(): string {
    if (this.textoBotao$) {
      return this.textoBotao$;
    }
    return 'Incluir';
  }
  @Input() titulo: string;
  @Input() caminho: string;
  @Input() textoRota: string;
  @Input() rota: string;
  @Input() texto: string;
  @Input() hasAdicionar: boolean;
  @Output() adicionarClicked = new EventEmitter<any>();
  @Output() voltou = new EventEmitter<any>();

  ngOnInit() { }

  navigateToRota() {
    window.history.back();

  }

  clickedBtnVoltar() {
    this.voltou.emit(true);
  }

  clickedBtnAdicionar() {
    this.adicionarClicked.emit(true);
  }

}
