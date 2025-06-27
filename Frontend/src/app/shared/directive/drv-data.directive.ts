import { Directive, HostListener, ElementRef, Renderer, Renderer2, Input } from '@angular/core';

export enum KEY_CODE {
  RIGHT_ARROW = 39,
  LEFT_ARROW = 37
}
@Directive({
  // tslint:disable-next-line: directive-selector
  selector: '[DrvData]'
})
export class DrvDataDirective {

  // tslint:disable-next-line: variable-name
  @Input('DrvData') _data: Date;

  @HostListener('window:keyup', ['$event']) OnClick(event: KeyboardEvent) {
   // alert(this._data)

    this.dataValida(this._data);
    // tslint:disable-next-line: deprecation
    if (event.keyCode === KEY_CODE.RIGHT_ARROW) {
      // this.increment();
    }

    // tslint:disable-next-line: deprecation
    if (event.keyCode === KEY_CODE.LEFT_ARROW) {
      // this.decrement();
    }
  }


  dataValida(data: Date) {
    if (data == null) {
      return;
    }
    if (typeof(data) === 'object') {
      this.renderer.removeClass(this.el.nativeElement, 'erro');
      return null;
   }
    if (typeof(data) === 'string') {
       this.renderer.addClass(this.el.nativeElement, 'erro');
       return {erro: true, msg: 'Data invalida'};
    }
}
  constructor(private el: ElementRef, private renderer: Renderer2) { }

}
