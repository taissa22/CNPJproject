import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { Component, Input, OnInit, EventEmitter, Output } from '@angular/core';
import { SortDirectionEnum } from '@shared/enums/SortDirectionEnum.enum';
import { SortOrderView } from '@shared/interfaces/Sort/SortOrderView';
import { BsModalRef } from 'ngx-bootstrap';

@Component({
  selector: 'app-ordenacao-drag-drop',
  templateUrl: './ordenacao-drag-drop.component.html',
  styleUrls: ['./ordenacao-drag-drop.component.scss']
})
export class OrdenacaoDragDropComponent implements OnInit {

  constructor(public bsModalRef: BsModalRef) { }

  @Input() title;

  @Input() values: Array<SortOrderView> = [];

  @Output() orderedValues = new EventEmitter<Array<SortOrderView>>();


  ngOnInit() {
  }

  drop(event: CdkDragDrop<SortOrderView[]>) {
    moveItemInArray(this.values, event.previousIndex, event.currentIndex);
  }

  save() {
    this.orderedValues.emit(this.values);
    this.bsModalRef.hide();
  }

}
