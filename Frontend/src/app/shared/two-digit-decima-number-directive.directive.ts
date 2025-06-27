import { Directive, ElementRef, HostListener  } from '@angular/core';
import { DialogService } from '@shared/services/dialog.service';

@Directive({
  selector: '[appTwoDigitDecimaNumberDirective]'
})
export class TwoDigitDecimaNumberDirectiveDirective {

  // Aceita números decimais e valores negativos
  private regex: RegExp = new RegExp(/^99$|^0(,\d{1,2})?$|^[1-9]?[0-9](,\d{0,2})?$/g);
  // Permite códigos de chave para eventos especiais. Refletir:
  private specialKeys: Array<string> = ['Backspace', 'Tab', 'End', 'Home', 'ArrowLeft', 'ArrowRight', 'Del', 'Delete'];

  constructor(private el: ElementRef, private dialogService: DialogService) {}
  @HostListener('keypress', ['$event'])

  onKeyPress(event: KeyboardEvent) {
    // Allow Backspace, tab, end, and home keys
    if (this.specialKeys.indexOf(event.key) !== -1) {
      return;
    }

    const start = this.el.nativeElement.selectionStart;
    const end = this.el.nativeElement.selectionEnd;

    if (start != end){
      const value = this.el.nativeElement.value;
      const newValue  = value.substr(0, start) + value.substr(end);
      this.el.nativeElement.value = newValue;
    }

    let current: string = this.el.nativeElement.value;
    const position = this.el.nativeElement.selectionStart;
    const next: string = [current.slice(0, position), event.key == 'Decimal' ? ',' : event.key, current.slice(position)].join('');
    if (/^[0-9,]+$/.test(event.key) == false || (next && !String(next).match(this.regex))) {
      if(Number(next) >= 100)
      {
        this.el.nativeElement.value = '';
        this.dialogService.err('Atenção', 'O percentual de responsabilidade Oi tem que ser maior do que 0 e menor do que 100 para processos híbridos.');
      }
      
      event.preventDefault();
    }
  }

}
