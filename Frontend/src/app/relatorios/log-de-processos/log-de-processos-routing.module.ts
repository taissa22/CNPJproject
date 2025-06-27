import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RoleGuardService } from '@core/services/Roles.guard.ts.service';
import { LogDeProcessosComponent } from './log-de-processos.component';


const routes: Routes = [{
  path: '',
  component: LogDeProcessosComponent,
  data: {
    role: 'm_RelatorioLogProcessos'
  },
  canActivate: [RoleGuardService]
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LogDeProcessosRoutingModule { }
