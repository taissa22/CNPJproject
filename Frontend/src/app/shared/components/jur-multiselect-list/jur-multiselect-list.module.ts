import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { JurMultiselectListComponent } from './jur-multiselect-list.component';



@NgModule({
  declarations: [
    JurMultiselectListComponent
  ],
  imports: [
    CommonModule
  ],
  exports:[
    JurMultiselectListComponent
  ]
})
export class JurMultiselectListModule { }
