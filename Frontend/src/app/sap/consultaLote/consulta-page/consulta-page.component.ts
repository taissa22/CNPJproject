import { ConsultaNumeroLoteService } from './../services/consulta-numero-lote.service';
/** Este código foi batizado pela arte milenar.
 *
 *                                  |\    /|
                              ___| \,,/_/
                           ---__/ \/    \
                          __--/     (D)  \
                          _ -/    (_      \
                         // /       \_ /  -\
   __-------_____--___--/           / \_ O o)
  /                                 /   \__/
 /                                 /
||          )                   \_/\
||         /              _      /  |
| |      /--______      ___\    /\  :
| /   __-  - _/   ------    |  |   \ \
 |   -  -   /                | |     \ )
 |  |   -  |                 | )     | |
  | |    | |                 | |    | |
  | |    < |                 | |   |_/
  < |    /__\                <  \
  /__\                       /___\

  * Todo trecho de código aqui deve seguir o go horse. Caso contrário, o programa irá quebrar.
  */

import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TipoProcessoEnum } from '@shared/enums/tipo-processoEnum.enum';
import { BehaviorSubject, Subscription } from 'rxjs';
import { FiltroModel } from 'src/app/core/models/filtro.model';
import { TipoProcesso } from 'src/app/core/models/tipo-processo';
import { ResultadoService } from 'src/app/core/services/sap/resultado.service';
import { SapService } from 'src/app/core/services/sap/sap.service';
import { TipoProcessoService } from 'src/app/core/services/sap/tipo-processo.service';
import swal from 'sweetalert2';
import { FilterService } from '../services/filter.service';
import { CriteriosGeraisService } from './../services/criterios-gerais.service';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { role } from '../../sap.constants';




@Component({
  // tslint:disable-next-line: component-selector
  selector: 'consulta-page',
  templateUrl: './consulta-page.component.html',
  styleUrls: ['./consulta-page.component.scss']
})

export class ConsultaPageComponent implements OnInit {  

  constructor(private tipoProcessoService: TipoProcessoService,
    private filterService: FilterService,
    private route: ActivatedRoute,
    private resultadoService: ResultadoService,
    private router: Router,
    private sapSerivce: SapService,
    private criteriosGeraisService: CriteriosGeraisService,
    private loteService: ConsultaNumeroLoteService,
    private breadcrumbsService: BreadcrumbsService) { }

  listaFiltro: Array<FiltroModel> = [];
  titulo = 'Consulta, Controle e Acompanhamento de Lotes ';
  caminhoPaginaSap: string;
  filtroSap = 'Selecione o tipo de processo e os critérios de seleção dos lotes a serem considerados';

  // FontAwesome
  public nomeProcesso = '';
  public tipoSelecionado = new BehaviorSubject<number>(null);
  public tiposProcesso: TipoProcesso[];
  public btnResponse: string;
  public resultado = false;
  public isDisabled = false;
  subscription: Subscription;

  ngOnInit() {
    
    this.isDisabled = false;
    //#region   Resolver do tipo de processo
    this.subscription = this.route.data.subscribe(info => {
      this.tiposProcesso = info.tipoProcesso;

    });
    //#endregion
    this.listaFiltro = this.sapSerivce.ListarFiltros().filter(item => item.id != 12);
    this.filterService.tipoProcessoTracker.subscribe(processo => {
      this.tipoSelecionado.next(processo)
      if (processo == TipoProcessoEnum.juizadoEspecial) {
        this.listaFiltro = this.sapSerivce.listarComTipoProcesso(processo);

      } else {
        this.listaFiltro = this.sapSerivce.listarComTipoProcesso(processo)
          .filter(item => item.id != 12);
      }

      if (processo) {
        this.isDisabled = true;
        this.listaFiltro.map(item => {
          item.ativo = false;
          if (item.id === 1) {
            item.ativo = true;
          }
        });
      } else {
        this.isDisabled = false;
        this.listaFiltro.map(item => {
          item.ativo = false;
        });
      }

      this.alterarTitulos();

    });



    this.tipoSelecionado.next(this.filterService.tipoProcessoTracker.value);

    if (this.tipoSelecionado.value > 0) {
      this.filtroSap = 'Selecione os critérios de seleção dos lotes a serem considerados';
    }

    this.goToInicio();

    this.filterService.limparFiltros.subscribe(limpar => {
      if (limpar) {
        this.criteriosGeraisService.limparDaos()
        this.filterService.limparFiltros.next(false);
        this.loteService.listaLotes$.next([])
      }
    })

    this.filterService.limpar();

    this.filterService.limparContadores.subscribe( _ => {
      this.sapSerivce.limparContadores()
    })

  }

  async ngAfterViewInit(): Promise<void> {
    this.caminhoPaginaSap = await this.breadcrumbsService.nomeBreadcrumb(role.menuConsultaControleAcompanhamentoLote);
  }

  onTypeSelect(tipoProcessoId) {


    this.sapSerivce.LimparDados();
    // tslint:disable-next-line: radix

    this.filterService.tipoProcessoTracker.next(parseInt(tipoProcessoId));
    this.filtroSap = 'Selecione os critérios de seleção dos lotes a serem considerados';
   

    this.goToInicio();

  }

  goToInicio() {
    this.router.navigate(['criteriosGeraisGuia'], { relativeTo: this.route });
    this.listaFiltro.map(item => {
      item.ativo = false;
      if (item.id === 1) {
        item.ativo = true;
      }
    });
  }

  format(date, h, m, s, ms): Date {
    if (date) {
      return new Date(date.year, date.month - 1, date.day, h, m, s, ms);
    } else {
      return null;
    }
  }

  filtroJson() {
    return {
      dataCriacaoPedidoMaior: this.filterService.dataFimPedido.getValue(),
      dataCriacaoPedidoMenor: this.filterService.dataInicioPedido.getValue(),
      dataCriacaoMaior: this.filterService.dataFimLote.getValue(),
      dataCriacaoMenor: this.filterService.dataInicioLote.getValue(),
      listaNumeroGuia: this.filterService.guiasSelecionadas.ToArray(),
      listaPedidoSAP: this.filterService.sapsSelecionados.ToArray(),
      tipoProcesso: this.filterService.tipoProcessoTracker.value,
      pagina: 1,
      quantidade: 5
    };
  }
  private openModalSemResultado() {

    const swalButton = swal.mixin({
      customClass: {
        confirmButton: 'btn btn-primary'
      },
      buttonsStyling: false
    });
    swalButton.fire({
      html:
        'Nenhum resultado foi encontrado',
      showCloseButton: true,
      cancelButtonText:
        'OK '
    });
  }


  onClickbtn(e: string) {
    this.filterService.chamarDTO();
  }

  alterarTitulos() {
    this.nomeProcesso = this.filterService.nomeProcessoPlural;

    if (this.nomeProcesso) {
      this.titulo = 'Consulta, Controle e Acompanhamento de Lotes - ' + this.nomeProcesso;

    }
  }

  limparFiltro(event:any) {
    this.filterService.limparFiltroSemProcesso();
    this.goToInicio()
  
  }

}
