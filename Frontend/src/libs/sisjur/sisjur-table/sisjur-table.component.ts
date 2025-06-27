import {
  ChangeDetectionStrategy,
  Component,
  ContentChild,
  ContentChildren,
  Directive,
  EventEmitter,
  Input,
  Output,
  QueryList,
  TemplateRef,
  ViewChild
} from '@angular/core';

@Component({
  selector: 'sisjur-table-header-cell',
  template: '<ng-template #cell><ng-content></ng-content></ng-template>',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class SisjurTableHeaderCell {
  @ViewChild('cell', { static: true })
  templateRef: TemplateRef<unknown>;

  @Input() width?: number;
  @Input() sort?: string;
}

@Directive({
  selector: 'sisjur-table-header'
})
export class SisjurTableHeader {
  @ContentChildren(SisjurTableHeaderCell)
  cells: QueryList<SisjurTableHeaderCell>;

  @Input() height? : string = '40px';
}

@Component({
  selector: 'sisjur-table-row-cell',
  template: '<ng-template #cell><ng-content></ng-content></ng-template>',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class SisjurTableRowCell {
  @ViewChild('cell', { static: true })
  templateRef: TemplateRef<unknown>;

  @Input() width?: number;
  @Input() ellipsis?: boolean = false;
  @Input() title?: string = '';
}

@Directive({
  selector: 'sisjur-table-row'
})
export class SisjurTableRow {
  @ContentChildren(SisjurTableRowCell)
  cells: QueryList<SisjurTableRowCell>;

  @Input() heightAuto?: boolean;
  @Input() background?: string = '';
}

@Component({
  selector: 'sisjur-table',
  templateUrl: './sisjur-table.component.html',
  styleUrls: ['./sisjur-table.component.scss']
})
export class SisjurTable {
  @ContentChild(SisjurTableHeader, { static: false }) header: SisjurTableHeader;
  @ContentChildren(SisjurTableRow) rows: QueryList<SisjurTableRow>;

  @Input()
  bodyRowsLimit?: number;
  @Input()
  emptyMessage: string = 'Nenhuma informação';
  @Input()
  sortColumn?: string;
  @Input()
  sortDirection: 'asc' | 'desc' = 'asc';
  @Output()
  readonly sort: EventEmitter<Sort> = new EventEmitter<Sort>();
  private onSort(): void {
    this.sort.next({
      direction: this.sortDirection,
      column: this.sortColumn
    });
  }
  sortBy(column?: string): void {
    if (!column) {
      return;
    }
    if (this.sortColumn !== column) {
      this.sortColumn = column;
      this.sortDirection = 'asc';
      this.onSort();
      return;
    }
    if (this.sortDirection === 'asc') {
      this.sortDirection = 'desc';
      this.onSort();
      return;
    }
    this.sortColumn = undefined;
    this.sortDirection = 'asc';
    this.onSort();
  }



}

export declare interface Sort {
  readonly column?: string;
  readonly direction: 'asc' | 'desc';
}
