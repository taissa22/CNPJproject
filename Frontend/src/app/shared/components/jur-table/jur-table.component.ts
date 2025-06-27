// tslint:disable component-class-suffix component-selector
// angular
import { Component, AfterViewInit, ViewChild, OnInit } from '@angular/core';
import { EventEmitter, Output, Input } from '@angular/core';
import { ContentChildren, QueryList, TemplateRef } from '@angular/core';

// 3rd party
import { BehaviorSubject } from 'rxjs';

// local imports
import { JurColumnTemplate } from './jur-column-template/jur-column-template.directive';
import { JurRowData } from './jur-row-data/jur-row-data.directive';
import { JurTableHeader } from './jur-table-header/jur-table-header.component';
import { SortEvent } from './sort-event';

/**
 * @deprecated Use '@sisjur/sisjur-table' instead.
 */
@Component({
  selector: 'jur-table',
  templateUrl: './jur-table.component.html',
  styleUrls: ['./jur-table.component.scss']
})
export class JurTable<T> implements OnInit, AfterViewInit {
  // prettier-ignore
  private sortColumn$: BehaviorSubject<string | null> = new BehaviorSubject(null);
  /** Current sorted column. Defaulted to null. */
  get sortColumn(): string | null {
    return this.sortColumn$.value;
  }
  @Input()
  set sortColumn(v: string | null) {
    this.sortColumn$.next(v);
  }

  // prettier-ignore
  private sortDirection$: BehaviorSubject<'asc' | 'desc' | null> = new BehaviorSubject(null);
  /** Current column sort direction. Defaulted to null. */
  get sortDirection(): 'asc' | 'desc' | null {
    return this.sortDirection$.value;
  }
  @Input()
  set sortDirection(v: 'asc' | 'desc' | null) {
    this.sortDirection$.next(v);
  }

  @ViewChild(JurTableHeader, { static: true })
  private header: JurTableHeader;

  /** The table's source of data. */
  @Input()
  dataSource: Array<T> = [];

  /** Error message displayed when there are no rows.
   * Defaulted to 'Nenhuma informação'.
   */
  @Input()
  errorMessage: string = 'Nenhuma informação';

  @ContentChildren(JurColumnTemplate)
  private columnTemplates: QueryList<JurColumnTemplate>;

  @ContentChildren(JurRowData)
  private columnData: QueryList<JurRowData>;

  /** Event emitted when a column has a sortColumn defined and is clicked. */
  @Output()
  readonly sort: EventEmitter<SortEvent> = new EventEmitter<SortEvent>();

  ngOnInit(): void {
    this.sortColumn$.subscribe(sortColumn => {
      if (this.header && this.header.sortColumn !== sortColumn) {
        this.header.sortColumn = sortColumn;
      }
    });

    this.sortDirection$.subscribe(sortDirection => {
      if (this.header && this.header.sortDirection !== sortDirection) {
        this.header.sortDirection = sortDirection;
      }
    });
  }

  ngAfterViewInit(): void {
    this.header.sort.subscribe((e: SortEvent) => {
      if (e !== null) {
        this.sortColumn = e.columnName;
        this.sortDirection = e.direction;
      } else {
        this.sortColumn = null;
        this.sortDirection = null;
      }
      this.sort.next(e);
    });

    setTimeout(() => {
      this.header.columns = this.columnTemplates.map(x => ({
        columnName: x.headerName,
        sortColumn: x.sortColumn,
        flex: x.flex
      }));
    }, 0);
  }

  getCellTemplateRef$(i: number): TemplateRef<any> {
    return this.columnData.toArray()[i].templateRef;
  }
}
