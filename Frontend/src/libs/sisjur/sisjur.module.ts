import { NgModule } from '@angular/core';

import { SisjurTableModule } from './sisjur-table/sisjur-table.module';
import { SisjurPaginatorModule } from './sisjur-paginator/sisjur-paginator.module';
import { SisjurSelectablePanelModule } from './sisjur-selectable-panel/sisjur-selectable-panel.module';
import { SisjurProgressBarModule } from './sisjur-progress-bar/sisjur-progress-bar.module';

@NgModule({
  imports: [
    SisjurTableModule,
    SisjurPaginatorModule,
    SisjurSelectablePanelModule,
    SisjurProgressBarModule
  ],
  exports: [
    SisjurTableModule,
    SisjurPaginatorModule,
    SisjurSelectablePanelModule,
    SisjurProgressBarModule
  ]
})
export class SisjurModule { }
