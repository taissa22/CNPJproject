import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { faQuestionCircle } from '@fortawesome/free-solid-svg-icons';
import { Combobox } from '@shared/interfaces/combobox';


/**
*
* É um componente de combobox que espera 2 atributos: descricao e id.
  * @Input opcoes -> lista de opções da combo
  * @Input currentIndex -> é o index atual da combo, o index default é -1, ou seja,
  * quando não houver nada selecionado.
  * @Input titulo -> é o nome dafult da opção quando não houver valor selecionado.
  * @Input icone -> é um boolean que libera a utilização de um icone de question
  * @Input classe -> seleciona uma classe do componente
  * @Input label -> é a label que fará parte da combobox
  * @Input iswhite -> é um boolean que libera a possibilidade de voltar ao valor default
  * e não selecionar nenhuma opção
  * @Input isDisable -> é um boolean que verifica se a combo está ativa
  * @Input txtTooltip -> é uma string que aparecerá em cima de um icone informativo.
  * @return selectionChange -> indica qual item foi escolhido na combo
*/
@Component({
  // tslint:disable-next-line: component-selector
  selector: 'combo-box',
  templateUrl: './combo-box.component.html',
  styleUrls: ['./combo-box.component.scss']
})

export class ComboBoxComponent implements OnInit {
  faQuestionCircle = faQuestionCircle;

  // Combo Box retornando o ID e mostrando a descricaos
  @Input() opcoes: Combobox[];

  @Output() selectionChange = new EventEmitter<number>();

  @Input() currentIndex = null;

  @Input() titulo: string;

  @Input() icone = false;

  @Input() classe: string;

  @Input() label: string;

  @Input() isWhite = false;

  @Input() isDisabled = false;

  @Input() txtTooltip: string = '';

  @Input() semResultado: string = 'Usuário sem permissão.';

  @Input() todos: string = '';

  /** @description Verifica se possui foco na combo para visualizar as informações e não onerar a tela
   * quando não precisar ser mostrado (Melhora performance em listas longas).
   */
  hasFocus = false;

  constructor() { }


  ngOnInit(): void {



  }

  ngOnChange(): void {
    this.currentIndex = this.currentIndex

  }

  onSelect() {
    this.currentIndex === '' ? this.currentIndex = null : this.currentIndex;
    this.currentIndex == '0' ? this.currentIndex = 0 : this.currentIndex;
    this.selectionChange.emit(this.currentIndex);
  }


  get valorAtualCombo() {
    let valor
    if (this.opcoes)

      this.opcoes.forEach(item => {
        if (item.id == this.currentIndex) {
          valor = item;

        }
      });

    return valor;
  }
}
