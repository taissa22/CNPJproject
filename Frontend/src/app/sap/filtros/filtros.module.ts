import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FiltroProcessosMultiComponent } from './filtro-processos-multi/filtro-processos-multi.component';
import { CriteriosImportacaoArquivoRetornoComponent } from './criterios-importacao-arquivo-retorno/criterios-importacao-arquivo-retorno.component';
import { NumeroGuiaComponent } from './numero-guia/numero-guia.component';
import { NumeroContaJudicialComponent } from './numero-conta-judicial/numero-conta-judicial.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '@shared/shared.module';
import { ComponentesModule } from 'src/app/componentes/componentes.module';
import { ProcessosPexComponent } from './processos-pex/processos-pex.component';
import { ProcessosJuizadoEspecialComponent } from './processos-juizado-especial/processos-juizado-especial.component';
import { ProcessosCCComponent } from './processos-cc/processos-cc.component';



@NgModule({
  declarations: [FiltroProcessosMultiComponent,
    CriteriosImportacaoArquivoRetornoComponent,
    NumeroGuiaComponent,
    NumeroContaJudicialComponent,
    ProcessosPexComponent,
    ProcessosCCComponent,
    ProcessosJuizadoEspecialComponent],
  exports: [FiltroProcessosMultiComponent,
    CriteriosImportacaoArquivoRetornoComponent,
    NumeroGuiaComponent,
    NumeroContaJudicialComponent,
    ProcessosPexComponent,
    ProcessosJuizadoEspecialComponent],
  imports: [
    CommonModule,
    FormsModule,
    SharedModule,
    ReactiveFormsModule,
    ComponentesModule
  ]
})
export class FiltrosModule { }
