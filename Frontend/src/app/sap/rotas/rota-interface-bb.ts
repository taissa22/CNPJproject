import { ComarcaTribunaisResolverGuard } from '../guards/comarcaTribunaisResolverGuard';
import { OrgaoBBComponent } from '../interfacesBB/orgaoBB/orgaoBB.component';
import { Routes } from '@angular/router';
import { role } from '../sap.constants';
import { RoleGuardService } from '../../core/services/Roles.guard.ts.service';
import { TribunaisBBComponent } from '../interfacesBB/tribunaisBB/tribunaisBB.component';
import { ModalidadeProdutoBbComponent } from '../interfacesBB/modalidade-produto-bb/modalidade-produto-bb.component';
import { NaturezaBbComponent } from '../interfacesBB/natureza-bb/natureza-bb.component';
import { CriteriosImportacaoArquivoRetornoComponent } from '../filtros/criterios-importacao-arquivo-retorno/criterios-importacao-arquivo-retorno.component';
import { NumeroGuiaComponent } from '../filtros/numero-guia/numero-guia.component';
import { NumeroContaJudicialComponent } from '../filtros/numero-conta-judicial/numero-conta-judicial.component';
import { ProcessosPexComponent } from '../filtros/processos-pex/processos-pex.component';
import { ProcessosJuizadoEspecialComponent } from '../filtros/processos-juizado-especial/processos-juizado-especial.component';
import { ProcessosCCComponent } from '../filtros/processos-cc/processos-cc.component';
import { ImportacaoArquivoRetornoResultadoGuard } from '../guards/importacao-arquivo-retorno-resultado.guard';
import { ConsultaArquivoRetornoComponent } from '../interfacesBB/importacao-arquivo-retorno/consulta-arquivo-retorno/consulta-arquivo-retorno.component';
import { ResultadoArquivoRetornoComponent } from '../interfacesBB/importacao-arquivo-retorno/resultado-arquivo-retorno/resultado-arquivo-retorno.component';
import { DetalheGuiaImportacaoArquivoRetornoComponent } from '../interfacesBB/importacao-arquivo-retorno/resultado-arquivo-retorno/detalhe-guia-importacao-arquivo-retorno/detalhe-guia-importacao-arquivo.component';

export const interfaceBBRota: Routes =[ {
  path: 'interfaceBB/orgao',
  resolve: { combo: ComarcaTribunaisResolverGuard },
  component: OrgaoBBComponent,
  data: { role: role.menuInterfaceOrgaosBB },
  canActivate : [RoleGuardService]
},
{
  path: 'interfaceBB/tribunal',
  component: TribunaisBBComponent,
  data: { role: role.menuInterfaceTribunaisBB },
  canActivate : [RoleGuardService]
},
{
  path: 'interfaceBB/modalidade',
  component: ModalidadeProdutoBbComponent,
  data: { role: role.menuInterfaceModalidadeProdutoBB },
  canActivate : [RoleGuardService]

},
{
  path: 'interfaceBB/natureza',
  component: NaturezaBbComponent,
  data: { role: role.menuInterfaceNaturezacaoAcoesBB },
  canActivate : [RoleGuardService]
},
{
  path: 'interfaceBB/importacao/consulta',
  component: ConsultaArquivoRetornoComponent,
  children: [
    {
      path: 'criteriosGeraisGuia', component: CriteriosImportacaoArquivoRetornoComponent
    },
    {
      path: 'numeroGuiaGuia', component: NumeroGuiaComponent
    },
    {
      path: 'numeroContaJudicialGuia', component: NumeroContaJudicialComponent
    },
    {
      path: 'numeroProcessoPexGuia', component: ProcessosPexComponent
    },
    {
      path:'numeroProcessoJECGuia', component: ProcessosJuizadoEspecialComponent
    },
    {
      path: 'numeroProcessoCCGuia', component: ProcessosCCComponent
    }
    ]
  },
  {
    path: 'interfaceBB/importacao/resultado',
    component: ResultadoArquivoRetornoComponent,

    canActivate: [
      ImportacaoArquivoRetornoResultadoGuard,
      // RoleGuardService
    ]
  },
   {
    path: 'interfaceBB/importacao/resultado/guiasOk',
    component:DetalheGuiaImportacaoArquivoRetornoComponent
  }
]
