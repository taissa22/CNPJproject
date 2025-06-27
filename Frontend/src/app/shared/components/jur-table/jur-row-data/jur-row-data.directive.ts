import { Directive, OnInit } from '@angular/core';
import { TemplateRef, ViewContainerRef } from '@angular/core';

/**
 * @deprecated Use '@sisjur/sisjur-table' instead.
 */
@Directive({ selector: '[jurRowData], [rowData]' })
export class JurRowData implements OnInit {
  constructor(
    public templateRef: TemplateRef<any>,
    private viewContainerRef: ViewContainerRef
  ) {}

  ngOnInit(): void {
    this.viewContainerRef.clear();
    this.viewContainerRef.createEmbeddedView(this.templateRef, {
      $implicit: {}
    });
  }
}
