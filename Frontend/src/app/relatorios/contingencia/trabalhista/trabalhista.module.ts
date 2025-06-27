import { Injector, LOCALE_ID, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TrabalhistaRoutingModule } from './trabalhista-routing.module';

import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule } from '@angular/forms';
import { ComponentesModule } from 'src/app/componentes/componentes.module';
import { SharedModule } from '@shared/shared.module';
import { RoleGuardService } from '@core/services/Roles.guard.ts.service';

import { ProvisaoTrabalhistaPorMediaPageComponent } from './provisao-trabalhista-por-media/provisao-trabalhista-por-media-page/provisao-trabalhista-por-media-page.component';
import { EstoqueDePedidosPageComponent } from './estoque-de-pedidos/estoque-de-pedidos-page/estoque-de-pedidos-page.component';
import { FechamentoComponent } from './fechamento/fechamento.component';

import { DetalheProvisaoTrabalhistaPedidoPOComponent } from './DetalheProvisãoTrabalhistaPedido/DetalheProvisaoTrabalhistaPedidoPO/DetalheProvisaoTrabalhistaPedidoPO.component';
import { DetalheProvisaoTrabalhistaPedidoPRComponent } from './DetalheProvisãoTrabalhistaPedido/DetalheProvisaoTrabalhistaPedidoPR/DetalheProvisaoTrabalhistaPedidoPR.component';
import { RelatorioMovimentacaoTrabalhistaComponent } from './relatorio-movimentacao-trabalhista/relatorio-movimentacao-trabalhista.component';
import { AgendamentoRelatorioMovimentacaoTrabalhistaComponent } from './agendamento-relatorio-movimentacao-trabalhista/agendamento-relatorio-movimentacao-trabalhista.component';
import { StaticInjector } from './static-injector';
import { TabsModule } from 'ngx-bootstrap';
@NgModule({
  imports: [
    CommonModule,
    TrabalhistaRoutingModule,

    NgbModule,
    FormsModule,
    ComponentesModule,
    SharedModule,
    TabsModule.forRoot(),
  ],
  declarations: [
    ProvisaoTrabalhistaPorMediaPageComponent,
    EstoqueDePedidosPageComponent,
    FechamentoComponent,
    DetalheProvisaoTrabalhistaPedidoPRComponent,
    DetalheProvisaoTrabalhistaPedidoPOComponent,
    RelatorioMovimentacaoTrabalhistaComponent,
    AgendamentoRelatorioMovimentacaoTrabalhistaComponent
  ],
  entryComponents: [AgendamentoRelatorioMovimentacaoTrabalhistaComponent],
  providers: [RoleGuardService, { provide: LOCALE_ID, useValue: 'pt' }]
})
export class TrabalhistaModule {
  constructor(private injector: Injector) {
    StaticInjector.setInjectorInstance(this.injector);
  }
}
