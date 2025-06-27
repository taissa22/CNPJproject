import {
  Component,
  OnInit,
  ElementRef,
  QueryList,
  ContentChildren,
  Renderer,
  Input,
  AfterContentInit,
  ViewChild
} from '@angular/core';

@Component({
  selector: 'app-tabela',
  templateUrl: './tabela.component.html',
  styleUrls: ['./tabela.component.scss']
})
export class TabelaComponent implements OnInit, AfterContentInit {
  @ContentChildren('col') cols: QueryList<ElementRef>;
  @ContentChildren('row') rows: QueryList<ElementRef>;
  @ViewChild('container', { read: ElementRef, static: true }) container: ElementRef;
  @Input() tableStyle = { };
  @Input() headerStyle = { };
  @Input() dataStyle = { };
  @Input() height = 1000;

  // tslint:disable-next-line: deprecation
  constructor(private renderer: Renderer) { }

  ngOnInit() {
    function tableFixHead(e) {
      const el = e.target;
      const sT = el.scrollTop;
      el.querySelectorAll('thead th').forEach(th =>
        th.style.transform = `translateY(${sT}px)`
      );
    }

    document.querySelectorAll('.tableFixHead').forEach(el =>
      el.addEventListener('scroll', tableFixHead)
    );
  }

  ngAfterContentInit(): void {
    this.cols.forEach(col => {
      this.renderer.setElementClass(col.nativeElement, 'nowrap', true);
    });
    this.renderer.setElementProperty(this.container.nativeElement,
                                     'height',
                                     this.height);
  }
}
