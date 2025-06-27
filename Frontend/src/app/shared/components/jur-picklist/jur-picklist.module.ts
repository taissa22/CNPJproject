import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { JurPicklistComponent } from './jur-picklist.component';


@NgModule({
  declarations: [
    JurPicklistComponent
  ],
  imports: [
    CommonModule
  ],
  exports:[
    JurPicklistComponent
  ]
})
export class JurPicklistModule { }
