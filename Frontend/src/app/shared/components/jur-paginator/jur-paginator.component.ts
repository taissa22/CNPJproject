// tslint:disable component-class-suffix component-selector
// angular
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

// 3rd party
import { BehaviorSubject, merge } from 'rxjs';
import { distinctUntilChanged, map } from 'rxjs/operators';

// local imports
import { PageEvent } from './page-event';

/**
 * @deprecated Use '@sisjur/sisjur-paginator' instead.
 */
@Component({
  selector: 'jur-paginator',
  templateUrl: './jur-paginator.component.html',
  styleUrls: ['./jur-paginator.component.scss']
})
export class JurPaginator implements OnInit {
  private pageSize$: BehaviorSubject<number> = new BehaviorSubject<number>(8);
  /** Number of items to display on a page. Defaulted to 8. */
  get pageSize(): number {
    return this.pageSize$.value;
  }
  @Input()
  set pageSize(v: number) {
    this.pageSize$.next(v);
  }

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
  private pageSizeOptions$: BehaviorSubject<Array<number>> = new BehaviorSubject<Array<number>>([8, 15, 35, 50]);
  /** The set of provided page size options to display to the user. Defaulted to [8, 15, 35, 50]. */
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
