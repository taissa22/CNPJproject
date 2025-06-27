import { Directive, HostListener, ElementRef } from '@angular/core';

@Directive({
  selector: 'input[OnlyNumberNoStartWith0]'
})
export class OnlyNumberNoStartWith0Directive {
  constructor(private _el: ElementRef) { }

  @HostListener('input', ['$event']) onInputChange(event) {
    const initalValue = this._el.nativeElement.value;
    this._el.nativeElement.value = initalValue.replace(/[^0-9]*/g, '').replace(/^0|((?!\d).)/g, '').replace(/^0*/g, ''); ;
    if ( initalValue !== this._el.nativeElement.value) {

      event.stopPropagation();
    }
  }

  // @HostListener('paste', ['$event'])
  // onPaste(event: ClipboardEvent) {
  //   event.preventDefault();
  //   console.log(event)
  //   const pastedInput = event.clipboardData
  //     .getData('text/plain')
  //     .replace(/^0|((?!\d).)/g, '')
  //     .replace(/^0*/g, '');
  //   document.execCommand('insertText', false, pastedInput);
  // }

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
