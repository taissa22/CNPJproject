import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core'

@Component({
  selector: 'app-card-container',
  templateUrl: './card-container.component.html',
  styleUrls: ['./card-container.component.scss']
})
export class CardContainerComponent implements OnInit {

  @Input() cardId
  @Input() btnClass = 'fas fa-chevron-right'

  @Output('clicked') clickEvent = new EventEmitter<number>()

  constructor() { }

  ngOnInit() { }

  onClick() {
    this.clickEvent.emit(this.cardId)
  }
}


