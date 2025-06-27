import { ImagemGuiaComponent } from './../importacao-arquivo-retorno/resultado-arquivo-retorno/detalhe-guia-importacao-arquivo-retorno/imagem-guia/imagem-guia.component';
import { Routes } from '@angular/router';
import { ComarcaTribunaisResolverGuard } from '../../guards/comarcaTribunaisResolverGuard';
import { OrgaoBBComponent } from '../orgaoBB/orgaoBB.component';
import { role } from '../../sap.constants';
import { RoleGuardService } from '../../../core/services/Roles.guard.ts.service';
import { TribunaisBBComponent } from '../tribunaisBB/tribunaisBB.component';
import { ModalidadeProdutoBbComponent } from '../modalidade-produto-bb/modalidade-produto-bb.component';
import { NaturezaBbComponent } from '../natureza-bb/natureza-bb.component';
import { ConsultaArquivoRetornoComponent } from '../importacao-arquivo-retorno/consulta-arquivo-retorno/consulta-arquivo-retorno.component';
import { CriteriosImportacaoArquivoRetornoComponent } from '../../filtros/criterios-importacao-arquivo-retorno/criterios-importacao-arquivo-retorno.component';
import { NumeroGuiaComponent } from '../../filtros/numero-guia/numero-guia.component';
import { NumeroContaJudicialComponent } from '../../filtros/numero-conta-judicial/numero-conta-judicial.component';
import { ProcessosPexComponent } from '../../filtros/processos-pex/processos-pex.component';
import { ProcessosJuizadoEspecialComponent } from '../../filtros/processos-juizado-especial/processos-juizado-especial.component';
import { ProcessosCCComponent } from '../../filtros/processos-cc/processos-cc.component';
import { InterfacesBBComarcaBBComponent } from '../comarcaBB/interfacesBB-comarcaBB.component';
import { StatusParcelaBBComponent } from '../status-parcela-bb/status-parcela-bb.component';
import { ResultadoArquivoRetornoComponent } from '../importacao-arquivo-retorno/resultado-arquivo-retorno/resultado-arquivo-retorno.component';
import { ImportacaoArquivoRetornoResultadoGuard } from '../../guards/importacao-arquivo-retorno-resultado.guard';
import { ResultadoGuard } from '../../guards/resultado.guard';
import { DetalheGuiaGuard } from '../../guards/detalhe-guia.guard';
import { DetalheGuiaImportacaoArquivoRetornoComponent } from '../importacao-arquivo-retorno/resultado-arquivo-retorno/detalhe-guia-importacao-arquivo-retorno/detalhe-guia-importacao-arquivo.component';
import { ImportarArquivoComponent } from '../importacao-arquivo-retorno/importar-arquivo/importar-arquivo.component';
import { ImportacaoArquivoRetornoGuard } from '../../guards/importacao-arquivo-retorno.guard';
import { ConsultaArquivoRetornoResolver } from '../importacao-arquivo-retorno/guards/resolver/consulta-arquivo-retorno-resolver.guards';


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
  data: { role: role.menuInterfaceImportacaoConsultaArquivoRetorno },
  resolve: {parametroJuridico: ConsultaArquivoRetornoResolver},
  canActivate : [RoleGuardService],
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
    path: 'interfaceBB/comarcaBB',
    component: InterfacesBBComarcaBBComponent,
    data: { role: role.menuInterfaceComarcarBB },
    canActivate : [RoleGuardService]
    },

  {
    path: 'interfaceBB/statusparcela',
    component: StatusParcelaBBComponent,
    data: { role: role.menuInterfaceStatusParcelaBB },
    canActivate : [RoleGuardService]
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
     component: DetalheGuiaImportacaoArquivoRetornoComponent,
     canActivate: [
      DetalheGuiaGuard
    ]
  },
  {
    path: 'interfaceBB/importacao/resultado/importar',
     component: ImportarArquivoComponent,
     canActivate:[
      ImportacaoArquivoRetornoGuard
     ]
  },
  {
    path: 'interfaceBB/importacao/resultado/detalhe/imagemGuia/:codigoProcesso/:codigoLancamento',
    component: ImagemGuiaComponent,

  }
]
