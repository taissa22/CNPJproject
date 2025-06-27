import { Directive, ElementRef, HostListener, Input } from '@angular/core';
import { NgControl } from '@angular/forms';

@Directive({
  // tslint:disable-next-line: directive-selector
  selector: 'input[numbersPontoHifenBarraOnly]'
})
export class NumberPontoHifenDirective {

  // tslint:disable-next-line: variable-name
  constructor(private _el: ElementRef) { }

  @HostListener('input', ['$event']) onInputChange(event) {
    const initalValue = this._el.nativeElement.value;
    this._el.nativeElement.value = initalValue.replace(/[^0-9\-\.\/]*/g, '');
    if ( initalValue !== this._el.nativeElement.value) {

      event.stopPropagation();
    }
  }

  @HostListener('drop', ['$event'])
  onDrop(event: DragEvent) {
    event.preventDefault();
    const textData = event.dataTransfer
      .getData('text')
      .replace(/^0|((?!\d).)/g, '')
      .replace(/^0*/g, '');
    this._el.nativeElement.focus();
    document.execCommand('insertText', false, textData);
  }

}
