import { Directive, ElementRef, HostListener, Input } from '@angular/core';

@Directive({
  // tslint:disable-next-line: directive-selector
  selector: 'input[numbersOnly]'
})
export class NumberDirective {

  // tslint:disable-next-line: variable-name
  constructor(private _el: ElementRef) { }

  @HostListener('input', ['$event']) onInputChange(event) {
    const initalValue = this._el.nativeElement.value;
    this._el.nativeElement.value = initalValue.replace(/[^0-9]*/g, '') ;
    if ( initalValue !== this._el.nativeElement.value) {

      event.stopPropagation();
    }
  }
  // @HostListener('paste', ['$event'])
  // onPaste(event: ClipboardEvent) {
  //   event.preventDefault();
  //   const pastedInput = event.clipboardData
  //     .getData('text/plain')
  //     .replace(/((?!\d).)/g, '')
  //   document.execCommand('insertText', false, pastedInput);
  // }

  @HostListener('drop', ['$event'])
  onDrop(event: DragEvent) {
    event.preventDefault();
    const textData = event.dataTransfer
      .getData('text')
      .replace(/((?!\d).)/g, '')
    this._el.nativeElement.focus();
    document.execCommand('insertText', false, textData);
  }

}
