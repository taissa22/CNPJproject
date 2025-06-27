import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { TipoProcesso } from 'src/app/core/models/tipo-processo';
import { TipoProcessoEnum } from '../../../shared/enums/tipo-processoEnum.enum';
import { CriacaoService } from '../criacao.service';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { role } from '../../sap.constants';



@Component({
  // tslint:disable-next-line: component-selector
  selector: 'criacao-lotes',
  templateUrl: './filtro-criacao-lotes.component.html',
  styleUrls: ['./filtro-criacao-lotes.component.css']
})
export class FiltrocriacaoLotesComponent implements OnInit {
  breadcrumb: string;

  constructor(
    private route: ActivatedRoute, 
    public criacaoService: CriacaoService,
    private breadcrumbsService: BreadcrumbsService) { }

  subscription: Subscription;

  public tipoSelecionado = null;

  public tiposProcesso: TipoProcesso[];
  public dataValida = true;
  public numeroValido = true;
  enabled = false;

  //#region texto a ser lançado para a tela principal
  criarSaptxt = `Selecione o tipo de processo dos lotes a serem considerados e clique no
  botão para que o sistema apresente os que estão disponíveis para criação.`;
 //#endregion

  ngOnInit() {
    !this.criacaoService.manterDados && (this.criacaoService.tipoProcessoSelecionado = null);
    this.criacaoService.limparDados();

    //#region   Resolver do tipo de processo
    this.subscription = this.route.data.subscribe(info => {
      this.tiposProcesso = info.tipoProcesso;
    });
    this.tipoSelecionado = this.criacaoService.tipoProcessoSelecionado;
    //#endregion
  }

  async ngAfterViewInit(): Promise<void> {
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(role.menuCriacaoLote);
  }

  onTypeSelect(event) {
    //#region transformar o evento em int para fazer comparação com o enum
    // tslint:disable-next-line: radix
    this.tipoSelecionado = parseInt(event);
    this.enabled = true;
    //#endregion
    this.criacaoService.limparDados();
  }



  //#region Valores do Datepicker no componente Data Range
  datepickerDataInicio(dataEvent) {
    this.criacaoService.dataInicio = dataEvent;
  }
  datepickerDataFim(dataEvent) {
    this.criacaoService.dataFim = dataEvent;
  }
  //#endregion

  podeDesativar() {
    if ((!this.dataValida || !this.numeroValido)) {
      this.enabled = false;
    } else {
      this.enabled = true;
    }
    return this.enabled;
  }

  paginaResult() {
    this.criacaoService.tipoProcessoSelecionado = this.tipoSelecionado;
    this.criacaoService.getEmpresaCentralizadora();
  }

  onRangeValido(e) {
    this.dataValida = e;
    this.podeDesativar();
  }

  onNumeroValido(e) {
    this.numeroValido = e;
    this.podeDesativar();
  }

}
