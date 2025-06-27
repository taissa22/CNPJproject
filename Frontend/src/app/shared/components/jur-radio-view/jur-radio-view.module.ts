import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { JurRadioViewCardComponent, JurRadioViewComponent, JurRadioViewItemComponent } from './jur-radio-view.component';
import { JurRadioSymbolComponent } from './jur-radio-symbol/jur-radio-symbol.component';
 
@NgModule({ 
  imports: [
    CommonModule 
  ],
  declarations: [
    JurRadioViewItemComponent,
    JurRadioViewComponent,
    JurRadioSymbolComponent,
    JurRadioViewCardComponent
  ],
  exports:[
    JurRadioViewItemComponent,
    JurRadioViewComponent,
    JurRadioSymbolComponent,
    JurRadioViewCardComponent
  ]
})
export class JurRadioViewModule { }
