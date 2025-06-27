import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { InputNumeroProcessoComponent } from './input-numero-processo/input-numero-processo.component';
import { TextMaskModule } from 'angular2-text-mask';



@NgModule({
  declarations: [InputNumeroProcessoComponent],
  imports: [
    CommonModule,
    TextMaskModule,
    FormsModule
  ],
  exports: [
    InputNumeroProcessoComponent
  ]
})
export class InputsModule { }
