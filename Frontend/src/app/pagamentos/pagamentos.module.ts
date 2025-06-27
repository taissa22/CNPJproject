import { ReactiveFormsModule } from '@angular/forms';
import { NgModule, Injector } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ComponentesModule } from './../componentes/componentes.module';

import { PagamentosRoutingModule } from './pagamentos-routing.module';
import { CargaDeComprovantesComponent } from './carga-de-comprovantes/carga-de-comprovantes.component';
import { PainelCargaDeComprovantesComponent } from './carga-de-comprovantes/painel-carga-de-comprovantes/painel-carga-de-comprovantes.component';
import { InstrucoesCargaModalComponent } from './carga-de-comprovantes/instrucoes-carga-modal/instrucoes-carga-modal.component';
import { ListagemCargaDeComprovantesComponent } from './carga-de-comprovantes/listagem-carga-de-comprovantes/listagem-carga-de-comprovantes.component';
import { CargaDeDocumentosComponent } from './carga-de-documentos/carga-de-documentos.component';
import { InstrucoesCargaDocumentosModalComponent } from './carga-de-documentos/instrucoes-carga-documentos-modal/instrucoes-carga-documentos-modal.component';
import { ListagemCargaDeDocumentosComponent } from './carga-de-documentos/listagem-carga-de-documentos/listagem-carga-de-documentos.component';
import { CargaDeCompromissosComponent } from './carga-de-compromissos/carga-de-compromissos.component';
import { AgendamentoCargaCompromissoModalComponent } from './carga-de-compromissos/modal/agendamento-carga-compromisso-modal/agendamento-carga-compromisso-modal.component';
import { StaticInjector } from './static-injector';
import { NgSelectModule } from '@ng-select/ng-select';
import { NgbTooltipModule } from '@ng-bootstrap/ng-bootstrap';
import { PainelCargaDeCompromissosComponent } from './carga-de-compromissos/painel-carga-de-compromissos/painel-carga-de-compromissos.component';
import { PainelCargaDeDocumentosComponent } from './carga-de-documentos/painel-carga-de-documentos/painel-carga-de-documentos.component';
import { ListagemCargaDeCompromissosComponent } from './carga-de-compromissos/listagem-carga-de-compromissos/listagem-carga-de-compromissos.component';
import { SisjurPaginatorModule } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.module';
import { AcompanhamentoComponent } from './acompanhamento/acompanhamento.component';
import { SharedModule } from '@shared/shared.module';
import { ModalDownloadExportacoesComponent } from './acompanhamento/modal-download-exportacoes/modal-download-exportacoes.component';
import { InputMaskModule } from '@libs/racoon-mask/input-mask.module';

@NgModule({
  declarations: [
    CargaDeComprovantesComponent, 
    PainelCargaDeComprovantesComponent, 
    InstrucoesCargaModalComponent,
    ListagemCargaDeComprovantesComponent, 
    CargaDeDocumentosComponent, 
    PainelCargaDeDocumentosComponent,
    PainelCargaDeCompromissosComponent, 
    ListagemCargaDeCompromissosComponent,
    InstrucoesCargaDocumentosModalComponent, 
    ListagemCargaDeDocumentosComponent,
    CargaDeCompromissosComponent, 
    AgendamentoCargaCompromissoModalComponent,
    AcompanhamentoComponent ,
    ModalDownloadExportacoesComponent 
    ],
  imports: [
    CommonModule,
    PagamentosRoutingModule,
    ComponentesModule,
    ReactiveFormsModule,
    NgSelectModule,
    NgbTooltipModule,
    SisjurPaginatorModule,
    SharedModule,
    InputMaskModule
  ],
  entryComponents: [InstrucoesCargaModalComponent, InstrucoesCargaDocumentosModalComponent, AgendamentoCargaCompromissoModalComponent, ModalDownloadExportacoesComponent]
})
export class PagamentosModule {
  constructor(private injector: Injector) {
    StaticInjector.setInjectorInstance(this.injector);
  }
}
