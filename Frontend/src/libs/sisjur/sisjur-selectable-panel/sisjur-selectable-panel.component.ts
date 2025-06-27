import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'sisjur-selectable-panel',
  templateUrl: './sisjur-selectable-panel.component.html',
  styleUrls: ['./sisjur-selectable-panel.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class SisjurSelectablePanel<T> implements OnInit {
  @Input() header: string;

  @Input('multi-select') multiSelect: boolean = false;
  @Input('hide-select-all') hideSelectAll: boolean = false;
  @Input() checkbox: boolean = false;

  private items$ = new BehaviorSubject<Array<T>>([]);
  @Input()
  set items(value: Array<T>) {
    this.items$.next(value);
  }
  @Input() bindTo: string;

  displayItems: Array<displayItems<T>> = [];
  allSelected: boolean = false;

  @Output()
  readonly selectionChange = new EventEmitter<Array<T>>();

  ngOnInit(): void {
    this.items$.subscribe(x => {
      this.displayItems = [];

      let bindToFn = (item: T) => item;
      if (this.bindTo) {
        bindToFn = (item: T) => item[this.bindTo];
      }

      x.forEach(item =>
        this.displayItems.push({
          display: bindToFn(item),
          selected: false,
          value: item
        })
      );

      if (!this.multiSelect) {
        this.doSelectionChange();
        return;
      }

      this.allSelected = false;
      this.selectAllItems();
    });
  }

  selectItem(item: displayItems<T>): void {
    if (item.selected) {
      item.selected = false;
      this.allSelected = false;
      this.doSelectionChange();
      return;
    }

    if (this.multiSelect) {
      item.selected = true;
      this.allSelected = this.displayItems.findIndex(x => !x.selected) === -1;
      this.doSelectionChange();
      return;
    }

    const oldSelected = this.displayItems.find(x => x.selected);

    if (oldSelected) {
      oldSelected.selected = false;
    }

    item.selected = true;
    this.doSelectionChange();
  }

  selectAllItems(): void {
    if (this.displayItems.length === 0) {
      this.allSelected = false;
      this.doSelectionChange();
      return;
    }

    this.allSelected = !this.allSelected;
    this.displayItems.forEach(x => (x.selected = this.allSelected));
    this.doSelectionChange();
  }

  private doSelectionChange(): void {
    this.selectionChange.next(
      this.displayItems.filter(x => x.selected).map(x => x.value)
    );
  }
}

declare interface displayItems<T> {
  display: string | T;
  selected: boolean;
  value: T;
}
