import { AfterViewInit, ChangeDetectionStrategy, Component, ContentChildren, Directive, Input, OnInit, QueryList, TemplateRef, ViewChild } from '@angular/core';



@Component({
  selector: 'jur-radio-card',
  template: '<ng-template #card><ng-content></ng-content></ng-template>',
  changeDetection: ChangeDetectionStrategy.OnPush
}) 
export class JurRadioViewCardComponent {
  @ViewChild('card', { static: true }) templateRef: TemplateRef<unknown>;  
  @Input() name:string = "";
}  

@Directive({
  selector: 'jur-radio-item'
})
export class JurRadioViewItemComponent { 
  @Input() descricao:string;  
  @Input() selecionado: boolean = false;
  @Input() name:string = "";
}  

@Component({
  selector: 'jur-radio-view',
  templateUrl: './jur-radio-view.component.html',
  styleUrls: ['./jur-radio-view.component.scss']
})
export class JurRadioViewComponent implements OnInit,AfterViewInit {
  @ContentChildren(JurRadioViewItemComponent) itens: QueryList<JurRadioViewItemComponent>;
  @ContentChildren(JurRadioViewCardComponent) cards: QueryList<JurRadioViewCardComponent>;
  public itemSelecionado: JurRadioViewItemComponent = new JurRadioViewItemComponent(); 
  @Input() height: string = "auto";
  constructor() { }

  ngAfterViewInit(): void {  
    if(this.itens) {
      this.itens.map(i => {
          if(i.selecionado) this.selecionarItem(i);
      })
    }
  }

  ngOnInit() { 
  }

  selecionarItem(item:JurRadioViewItemComponent){
    this.itens.map(item =>  item.selecionado = false);
    item.selecionado = true;
    this.itemSelecionado = item;
  }


}
