import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FlexLayoutModule } from '@angular/flex-layout';

import {
  SisjurTable,
  SisjurTableHeader,
  SisjurTableHeaderCell,
  SisjurTableRow,
  SisjurTableRowCell
} from './sisjur-table.component';

@NgModule({
  imports: [CommonModule, FlexLayoutModule],
  declarations: [
    SisjurTable,
    SisjurTableHeader,
    SisjurTableHeaderCell,
    SisjurTableRow,
    SisjurTableRowCell
  ],
  exports: [
    SisjurTable,
    SisjurTableHeader,
    SisjurTableHeaderCell,
    SisjurTableRow,
    SisjurTableRowCell
  ]
})
export class SisjurTableModule {}
