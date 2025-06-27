// tslint:disable component-class-suffix component-selector
// angular
import { Component, EventEmitter, Input, Output } from '@angular/core';

// local imports
import { ColumnHeader } from '../column-header';
import { SortEvent } from '../sort-event';

/**
 * @deprecated Use '@sisjur/sisjur-table' instead.
 */
@Component({
  selector: 'jur-table-header, jur-thead',
  templateUrl: './jur-table-header.component.html',
  styleUrls: ['./jur-table-header.component.scss']
})
export class JurTableHeader {
  /** Columns definitions to display on a header. */
  @Input()
  columns: Array<ColumnHeader> = [];

  /** Current sorted column. Defaulted to null. */
  @Input()
  sortColumn: string | null = null;

  /** Current column sort direction. Defaulted to null. */
  @Input()
  sortDirection: 'asc' | 'desc' | null = null;

  /** Event emitted when a column has a sortColumn defined and is clicked. */
  @Output()
  readonly sort: EventEmitter<SortEvent | null> = new EventEmitter<SortEvent | null>();

  private raiseSortEvent(): void {
    if (this.sortDirection === null) {
      this.sort.next(null);
      return;
    }

    this.sort.next({
      columnName: this.sortColumn,
      direction: this.sortDirection
    });
  }

  onSort(columnName: string): void {
    if (this.sortColumn !== columnName) {
      this.sortColumn = columnName;
      this.sortDirection = 'asc';
      this.raiseSortEvent();
      return;
    }

    if (this.sortDirection === 'asc') {
      this.sortDirection = 'desc';
      this.raiseSortEvent();
      return;
    }

    this.sortColumn = null;
    this.sortDirection = null;
    this.raiseSortEvent();
  }
}
