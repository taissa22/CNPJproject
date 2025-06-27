import { Component, OnDestroy, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { Subject } from 'rxjs';
import { debounceTime } from 'rxjs/operators';

@Component({
  selector: 'app-search-dual-list',
  templateUrl: './search-dual-list.component.html',
  styleUrls:['./search-dual-list.component.scss']
})
export class SearchDualListComponent implements OnInit, OnDestroy {
  @Output() onTyping = new EventEmitter<string>();
  debounce: Subject<string> = new Subject<string>();

  @Input() label: string;
  @Input() class: string;

  ngOnInit() {
    this.debounce
    .pipe(debounceTime(300))
    .subscribe(filter=>this.onTyping.emit(filter));
  }

  ngOnDestroy(): void {
    this.debounce.unsubscribe();
  }
}



