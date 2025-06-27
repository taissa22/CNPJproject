import {
  Component, OnInit, ChangeDetectionStrategy, Input
 } from '@angular/core';

@Component({
  selector: 'app-input-numero-processo',
  templateUrl: './input-numero-processo.component.html',
  styleUrls: ['./input-numero-processo.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class InputNumeroProcessoComponent implements OnInit {

  // tslint:disable-next-line: max-line-length
  mascaraNumeroProcesso = [/[0-9]/, /\d/, /\d/, /\d/, /\d/, /\d/, /\d/, '-', /\d/, /\d/, '.', /\d/, /\d/, /\d/, /\d/, '.', /\d/, '.', /\d/, /\d/, '.', /\d/, /\d/, /\d/, /\d/];
  @Input()
    numeroProcesso: string;
  @Input()
    titulo: string;

  constructor() { }

  ngOnInit() {
  }

}
