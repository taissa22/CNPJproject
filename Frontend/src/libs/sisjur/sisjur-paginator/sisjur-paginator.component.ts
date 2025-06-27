// tslint:disable component-class-suffix component-selector
// angular
import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

// 3rd party
import { BehaviorSubject, merge } from 'rxjs';
import { distinctUntilChanged, map } from 'rxjs/operators';

/**
 * Component to provide navigation between paged information. Displays the size of the current
 * page, user-selectable options to change that size, what items are being shown, and
 * navigational button to go to the previous or next page.
 */
@Component({
  selector: 'sisjur-paginator',
  templateUrl: './sisjur-paginator.component.html',
  styleUrls: ['./sisjur-paginator.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class SisjurPaginator implements OnInit {
  private pageSize$: BehaviorSubject<number> = new BehaviorSubject<number>(8);
  /** Number of items to display on a page. Defaulted to 8. */
  get pageSize(): number {
    return this.pageSize$.value;
  }
  @Input()
  set pageSize(v: number) {
    this.pageSize$.next(v);
  }

  @Input() MostrarQtdPorPagina : boolean = true;
  @Input() QtdFixaPorPagina : number = 10;


  private length$: BehaviorSubject<number> = new BehaviorSubject<number>(0);
  /** The length of the total number of items that are being paginated. Defaulted to 0. */
  get length(): number {
    return this.length$.value;
  }
  @Input()
  set length(v: number) {
    this.length$.next(v);
  }

  // prettier-ignore
  private pageSizeOptions$: BehaviorSubject<Array<number>> = new BehaviorSubject<Array<number>>([8, 15, 30, 50]);
  /** The set of provided page size options to display to the user. Defaulted to [8, 15, 30, 50]. */
  get pageSizeOptions(): Array<number> {
    return this.pageSizeOptions$.value;
  }
  @Input()
  set pageSizeOptions(v: Array<number>) {
    this.pageSizeOptions$.next(v);
  }

  private pageIndex$: BehaviorSubject<number> = new BehaviorSubject<number>(0);
  /** The zero-based page index of the displayed list of items. Defaulted to 0. */
  get pageIndex(): number {
    return this.pageIndex$.value;
  }
  set pageIndex(v: number) {
    this.pageIndex$.next(v);
  }

  /** Event emitted when the paginator changes the page size or page index. */
  @Output()
  readonly page: EventEmitter<PageEvent> = new EventEmitter<PageEvent>();

  ngOnInit(): void {
    this.pageSizeOptions$
    .pipe(distinctUntilChanged())
    .subscribe(x => (this.pageSize = x[0]));
    
    if(!this.MostrarQtdPorPagina){
      this.pageSize = this.QtdFixaPorPagina;
    }

    merge(this.length$, this.pageSize$, this.pageSizeOptions$)
      .pipe(
        map(_ => {
          return {
            length: this.length,
            pageSize: this.pageSize,
            pageSizeOptions: this.pageSizeOptions
          };
        })
      )
      .pipe(
        distinctUntilChanged(
          (x, y) =>
            x.length === y.length &&
            x.pageSize === y.pageSize &&
            x.pageSizeOptions === y.pageSizeOptions
        )
      )
      .subscribe(_ => {
        this.pageIndex = 0;
      });

    merge(this.pageSize$, this.pageIndex$)
      .pipe(
        map(_ => {
          return {
            pageSize: this.pageSize,
            pageIndex: this.pageIndex
          };
        })
      )
      .pipe(
        distinctUntilChanged((x, y) => {
          return x.pageSize === y.pageSize && x.pageIndex === y.pageIndex;
        })
      )
      .subscribe(_ =>
        this.page.emit({
          length: this.length,
          pageIndex: this.pageIndex,
          pageSize: this.pageSize
        })
      );
  }

  /** Advances to the next page if it exists. */
  nextPage(): void {
    if (this.hasNextPage()) {
      this.pageIndex++;
    }
  }
  /** Move back to the previous page if it exists. */
  previousPage(): void {
    if (this.hasPreviousPage()) {
      this.pageIndex--;
    }
  }
  /** Move to the first page if not already there. */
  firstPage(): void {
    this.pageIndex = 0;
  }
  /** Move to the last page if not already there. */
  lastPage(): void {
    this.pageIndex = this.getNumberOfPages() - 1;
  }
  /** Whether there is a previous page. */
  hasPreviousPage(): boolean {
    return this.pageIndex > 0;
  }
  /** Whether there is a next page. */
  hasNextPage(): boolean {
    return this.pageIndex < this.getNumberOfPages() - 1;
  }
  /** Calculate the number of pages */
  getNumberOfPages(): number {
    return Math.ceil(this.length / this.pageSize);
  }
}

/**
 * Change event object that is emitted when the user selects a
 * different page size or navigates to another page.
 */
export declare class PageEvent {
  /** The current page index. */
  pageIndex: number;
  /** The current page size */
  pageSize: number;
  /** The current total number of items being paged */
  length: number;
}
