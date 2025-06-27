import { Directive, HostListener, ElementRef } from '@angular/core';

@Directive({
  selector: 'input[StartWithSpace]'
})
export class NoStartWithSpaceDirective {

  constructor(private _el: ElementRef) { }

  @HostListener('keydown', ['$event'])
  onKeyDown(e: KeyboardEvent) {
    const navKeys = ['Backspace', 'Tab', 'ArrowLeft', 'ArrowRight', 'ArrowDown', 'ArrowUp',
                     'Escape', 'Delete', 'Enter', 'CapsLock'];

    if (navKeys.includes(e.key)) {
      return;
    }
    if ((!e.ctrlKey && !e.altKey && !e.metaKey ) &&
      (this._el.nativeElement.value == '' && e.keyCode == 32
      )) {
      e.preventDefault();
    }
  }




}
