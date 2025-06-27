import { NgModule, LOCALE_ID } from '@angular/core';
import { registerLocaleData } from '@angular/common';
import localePt from '@angular/common/locales/pt';
import { FormsModule } from '@angular/forms';
import { NgbModalModule, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { InputMaskModule } from '@libs/racoon-mask/input-mask.module';
import { SharedModule } from '../shared';
import { ComponentesModule } from '../componentes/componentes.module';
import { RoleGuardService } from '../core/services/Roles.guard.ts.service';
import { ExtracaoBasePrePosRjComponent } from './D-1/extracao-base-pre-pos-rj/extracao-base-pre-pos-rj.component';
import { AlteracaoProcessoBlocoWebComponent } from './alteracao-processo-bloco-web/alteracao-processo-bloco-web.component';
import { PexPorMediaComponent } from './pex/pex-por-media/pex-por-media.component';
import { RelatoriosRoutingModule } from './relatorios-routing.module';
import { Injector } from '@angular/core';
import { RelatorioGenericaComponent } from './componentes/relatorio-generico.component/relatorio-generica.component';
import { StaticInjector } from '@shared/static-injector';
import { CCPorMediaComponent } from './civel-consumidor/cc-por-media/cc-por-media.component';
import { BsDatepickerModule } from 'ngx-bootstrap';
import { UsuarioPerfilPermissaoComponent } from './usuario-perfil-permissao/usuario-perfil-permissao-juizado-especial.component';
import { NegociacoesComponent } from './negociacoes/negociacoes.component';
import { SisjurPaginatorModule } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.module';
import { NegociacoesModalComponent } from './negociacoes/negociacoes-modal/negociacoes-modal.component';


registerLocaleData(localePt);

@NgModule({
  imports: [
    FormsModule,
    ComponentesModule,  
    NgbModule,
    SharedModule,
    RelatoriosRoutingModule,
    InputMaskModule,
    BsDatepickerModule.forRoot(),
    SisjurPaginatorModule
  ],
  declarations: [
    ExtracaoBasePrePosRjComponent,
    AlteracaoProcessoBlocoWebComponent,
    RelatorioGenericaComponent,
    CCPorMediaComponent,
    PexPorMediaComponent,
    UsuarioPerfilPermissaoComponent,
    NegociacoesComponent,
    NegociacoesModalComponent
  ],
  providers: [RoleGuardService, { provide: LOCALE_ID, useValue: 'pt' }],
  entryComponents: [
    NegociacoesModalComponent
  ]
})
export class RelatoriosModule {
  constructor(private injector: Injector) {
    StaticInjector.setInjectorInstance(this.injector);
  }
}
