import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { CriacaoService } from 'src/app/sap/criacaoLote/criacao.service';
import { BehaviorSubject } from 'rxjs';
import { LoteCriacaoBorderoDto } from '@shared/interfaces/lote-criacao-bordero-dto';
 
/** @description é um container que expande a medida que é clicado mostrando seu conteudo filho.
 * @input id - id do container que foi clicado
 * @input isOpen - Verifica se está aberto
 * @deprecated estilo - recebe um ngStyle como parametro - OLD
 * @deprecated bordaExist - Verifica se a borda completa existe - OLD
 * @deprecated bordaBot - verifica se existe somente bord-bottom - OLD
 * @output openSignal - envia um sinal informando que o container foi aberto
 * @output clicked - envia o id do container aberto
 * @example Esse componente deve ser chamado no html.
 * <app-container-expansivel-new [isOpen]="item.isOpen" [id]="item.codigoEmpresaCentralizadora"
                (openSignal)="onOpen($event, item.codigoEmpresaCentralizadora, item)" [ngStyle]="{color: '#554d80'}">
 * <div class="contentEsquerda"> Sou um valor no container fechado do lado esquerdo
 * </div>
 * <div class="contentEsquerda"> Sou um valor no container fechado do lado direito
 * </div>
 * <div class="contentConteudo"> Sou o conteudo que será mostrado ao expandir o container
 * </div>
 * </app-container-expansivel-new>
 *  @example Alterar o scss do componente pelo componente pai -NEW.
 * :host ::ng-deep .container-vertical{
 * color: blue
 * }
 *
 */
@Component({
  selector: 'app-container-expansivel-new',
  templateUrl: './container-expansivel-new.component.html',
  styleUrls: ['./container-expansivel-new.component.scss']
})
export class ContainerExpansivelNewComponent implements OnInit {
  @Input() id: number;
  @Input() isOpen = false;
  @Input('ngStyle') estilo = { color: '#554d80' };
  @Output() openSignal = new EventEmitter<boolean>();
  @Output() clicked = new EventEmitter<number>();
  @Input() bordaExist = true;
  @Input() bordaBot: false;
 
  /** @description usado para informar o estilo da borda - OLD */
  public estiloBorda = {};
  /** @description usado para informar o estilo da borda bottom - OLD */
  public estiloBordaBot = {};
 
  /** @description usado para verificar se ja existe um container aberto e fecha-lo para
   * somente ter 1 container aberto por vez
   */
  idSelecionado: any = null;
 
  constructor(private criacaoService: CriacaoService) { }
 
  ngOnInit() {
    if (this.bordaExist) {
      this.estiloBorda = {
        border: '1px solid #dbdbdb',
 
      };
    }
    if (this.bordaBot) {
      this.estiloBordaBot = {
        'border-bottom': '1px solid #dbdbdb',
 
      };
    }
  }
 
  /** abrir um fechar um componente. Verifica se o id do componente aberto é o mesmo do que está
   * tentando abrir, se for deve fechar.
   */
  open(id) {
 
    if (this.idSelecionado === id) {
      this.idSelecionado = null;
      this.isOpen = false;
    } else {
      this.idSelecionado = id;
      this.isOpen = true;
    }
    this.openSignal.emit(this.isOpen);
    this.clicked.emit(this.id);
    this.zerarListaBorderos();
  }
 
  zerarListaBorderos() {
    this.criacaoService.borderosSubject = new BehaviorSubject<LoteCriacaoBorderoDto[]>([]);
  }
 
}