import { Directive, ElementRef, HostListener, Renderer } from '@angular/core';

@Directive({
  // tslint:disable-next-line: directive-selector
  selector: '[textOverflow]'
})
export class TextOverflowDirective {

  // tslint:disable-next-line: deprecation
  constructor(private elementRef: ElementRef, private renderer: Renderer) {

  }

  @HostListener('mouseenter') over() {

    if (this.isEllipsisActive(this.elementRef.nativeElement)) {
      this.renderer.setElementAttribute(
        this.elementRef.nativeElement,
        'title', this.elementRef.nativeElement.innerText
      );
    }
  }

  private isEllipsisActive(element) {
    return element.offsetWidth < element.scrollWidth;
  }
}
