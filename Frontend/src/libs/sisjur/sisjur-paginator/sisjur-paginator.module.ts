import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { FlexLayoutModule } from '@angular/flex-layout';
import { NgSelectModule } from '@ng-select/ng-select';

import { SisjurPaginator } from './sisjur-paginator.component';

@NgModule({
  imports: [CommonModule, FlexLayoutModule, NgSelectModule, FormsModule],
  declarations: [SisjurPaginator],
  exports: [SisjurPaginator]
})
export class SisjurPaginatorModule {}
