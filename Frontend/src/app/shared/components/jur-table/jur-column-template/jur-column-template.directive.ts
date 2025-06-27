// tslint:disable directive-class-suffix directive-selector
import { Directive, Input, OnInit } from '@angular/core';

/**
 * @deprecated Use '@sisjur/sisjur-table' instead.
 */
@Directive({ selector: 'jur-column-template, [jurColumnTemplate]' })
export class JurColumnTemplate implements OnInit {
  /** Name to be displayed on the column header. This attribute is also required. */
  @Input() headerName: string;
  /**
   * Represents the sorting for the column. When present, the column header
   * will display the arrows for sorting and rise the sort event of the table
   * when clicked. 'headerName' must be defined for this one to be used;
   */
  @Input() sortColumn?: string;
  /**
   * Attribute to be projected to cells representing the FxFlex attribute.
   * Responsible for sizing definitions of the cell on a row.
   * Defaulted to 'auto'.
   */
  @Input() flex: string | number | 'auto' = 'auto';

  ngOnInit(): void {
    if (this.sortColumn && !this.headerName) {
      throw new Error(
        `Attribute 'headerName' is required when using 'sortColumn' for 'JurColumnTemplate'`
      );
    }
  }
}
