import { AfterViewInit, Component, OnInit } from '@angular/core';

import { FechamentosProvisaoTrabalhistaService } from '../../services/fechamentos-provisao-trabalhista.service';

import { environment } from '@environment';

import { Permissoes, PermissoesService } from '@permissoes';
import { DualListModel } from '@core/models/dual-list.model';

import { BehaviorSubject, EMPTY, merge } from 'rxjs';
import {
  catchError,
  distinctUntilChanged,
  map,
  switchMap
} from 'rxjs/operators';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';

@Component({
  selector: 'app-provisao-trabalhista-por-media-page',
  templateUrl: './provisao-trabalhista-por-media-page.component.html',
  styleUrls: ['./provisao-trabalhista-por-media-page.component.scss']
})
export class ProvisaoTrabalhistaPorMediaPageComponent
  implements OnInit, AfterViewInit {
  readonly exibeSemExclusao: boolean = false;
  readonly exibeExclusaoPorDesvioPadrao: boolean = false;
  readonly exibeExclusaoPorPercentual: boolean = false;
  readonly tipoOutlierUnico: boolean = true;

  constructor(
    permissoesService: PermissoesService,
    private fechamentosProvisaoTrabalhistaService: FechamentosProvisaoTrabalhistaService,
    private breadcrumbsService: BreadcrumbsService
  ) {
    this.exibeSemExclusao = permissoesService.temPermissaoPara(
      Permissoes.FILTRAR_PROVISAO_TRABALHISTA_OUTLIER_SEM_EXCLUSAO
    );
    this.exibeExclusaoPorPercentual = permissoesService.temPermissaoPara(
      Permissoes.FILTRAR_PROVISAO_TRABALHISTA_OUTLIER_POR_PERCENTUAL
    );
    this.exibeExclusaoPorDesvioPadrao = permissoesService.temPermissaoPara(
      Permissoes.FILTRAR_PROVISAO_TRABALHISTA_OUTLIER_POR_DESVIO_PADRAO
    );

    if (
      (this.exibeSemExclusao && this.exibeExclusaoPorPercentual) ||
      (this.exibeSemExclusao && this.exibeExclusaoPorDesvioPadrao) ||
      (this.exibeExclusaoPorPercentual && this.exibeExclusaoPorDesvioPadrao)
    ) {
      this.tipoOutlierUnico = false;
    }
  }

  private readonly semExclusao$ = new BehaviorSubject(false);
  get semExclusao(): boolean {
    return this.semExclusao$.value;
  }
  set semExclusao(value: boolean) {
    this.semExclusao$.next(value);
  }

  private readonly exclusaoPorDesvioPadrao$ = new BehaviorSubject(false);
  get exclusaoPorDesvioPadrao(): boolean {
    return this.exclusaoPorDesvioPadrao$.value;
  }
  set exclusaoPorDesvioPadrao(value: boolean) {
    this.exclusaoPorDesvioPadrao$.next(value);
  }

  private readonly exclusaoPorPercentual$ = new BehaviorSubject(false);
  get exclusaoPorPercentual(): boolean {
    return this.exclusaoPorPercentual$.value;
  }
  set exclusaoPorPercentual(value: boolean) {
    this.exclusaoPorPercentual$.next(value);
  }

  fechamentos: Fechamento[] = [];
  fechamentosAnteriores: Fechamento[] = [];

  fechamentoAtual: Fechamento;
  fechamentoAnterior: Fechamento;
  private fechamentoDetalhado: FechamentoDetalhado;

  empresasCentralizadoras: DualListModel[] = [];
  empresasCentralizadorasSelecionadas: DualListModel[] = [];
  breadcrumb: string;

  ngOnInit(): void {
    merge(
      this.semExclusao$,
      this.exclusaoPorDesvioPadrao$,
      this.exclusaoPorPercentual$
    )
      .pipe(
        map(_ => {
          return {
            semExclusao: this.semExclusao,
            desvioPadrao: this.exclusaoPorDesvioPadrao,
            percentual: this.exclusaoPorPercentual
          };
        })
      )
      .pipe(
        distinctUntilChanged(
          (x, y) =>
            x.semExclusao === y.semExclusao &&
            x.desvioPadrao === y.desvioPadrao &&
            x.percentual == y.percentual
        ),
        switchMap(value => {
          return this.fechamentosProvisaoTrabalhistaService
            .obterTodos(value.semExclusao, value.percentual, value.desvioPadrao)
            .pipe(catchError(() => EMPTY));
        })
      )
      .subscribe(fechamentos => {
        this.fechamentos = fechamentos;
        this.fechamentoAtual = null;

        this.fechamentosAnteriores = [];
        this.fechamentoAnterior = null;

        this.empresasCentralizadoras = [];
        this.empresasCentralizadorasSelecionadas = [];
      });
  }

  async ngAfterViewInit() {
    setTimeout(() => {
      this.clear();
    }, 0);
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(Permissoes.ACESSAR_PROVISAO_TRABALHISTA);
  }

  clear(): void {
    this.semExclusao = false;
    this.exclusaoPorDesvioPadrao = false;
    this.exclusaoPorPercentual = false;

    if (this.exibeSemExclusao) {
      this.semExclusao = true;
      return;
    }

    if (this.exibeExclusaoPorDesvioPadrao) {
      this.exclusaoPorDesvioPadrao = true;
      return;
    }

    this.exclusaoPorPercentual = this.exibeExclusaoPorPercentual;
  }

  async carregarFechamentoDetalhado(): Promise<void> {
    this.fechamentoDetalhado = await this.fechamentosProvisaoTrabalhistaService.obterFechamentoDetalhado(
      this.fechamentoAtual.id
    );
    this.empresasCentralizadoras = this.fechamentoDetalhado.centralizadoras.map(
      x => {
        return {
          id: x.codigo,
          label: x.nome
        };
      }
    );
    this.empresasCentralizadorasSelecionadas = [];
    this.fechamentoAnterior = null;
    this.fechamentosAnteriores = [];
  }

  async carregarFechamentosAnteriores(
    centralizadoras: DualListModel[]
  ): Promise<void> {
    this.empresasCentralizadorasSelecionadas = centralizadoras;
    this.fechamentoAnterior = null;
    this.fechamentosAnteriores = [];
    if (centralizadoras.length === 0) {
      return;
    }

    this.fechamentosAnteriores = await this.fechamentosProvisaoTrabalhistaService
      .obterAnteriores(
        this.fechamentoAtual.id,
        centralizadoras.map(x => x.id),
        this.semExclusao,
        this.exclusaoPorPercentual,
        this.exclusaoPorDesvioPadrao
      )
      .toPromise();
  }

  paginaResult(): void {
    const url: string =
      `${environment.s1_url}/2.0/relatorios/fechamento/provisao/trabalhista/Relatorio.aspx?` +
      `CodigoEmpresaCentralizadora=${this.empresasCentralizadorasSelecionadas
        .map(x => x.id)
        .join(';')}&` +
      `DataFechamento=${this.getOADate(this.fechamentoAtual.dataFechamento)}&` +
      `Meses=${this.fechamentoAtual.numeroDeMeses}&` +
      `Outlier=${this.fechamentoAtual.tipoDeOutliers.id}&` +
      `DataFechamentoAnterior=${
        this.fechamentoAnterior
          ? this.getOADate(this.fechamentoAnterior.dataFechamento)
          : 0
      }&` +
      `MesesAnterior=${
        this.fechamentoAnterior ? this.fechamentoAnterior.numeroDeMeses : 0
      }&` +
      `OutlierAnterior=${
        this.fechamentoAnterior ? this.fechamentoAnterior.tipoDeOutliers.id : 0
      }&` +
      'FiltraCentralizadora=1';

    window.location.href = url;
  }

  private getOADate(date: Date): string {
    var epoch = new Date(1899, 11, 30);
    var nDate = new Date(date);
    nDate.setHours(0, 0, 0, 0);

    return Math.abs(
      Math.round((((<any>epoch) as any) - <any>nDate) / 8.64e7)
    ).toString();
  }
}

declare interface Fechamento {
  id: number;
  dataFechamento: Date;
  numeroDeMeses: number;
  tipoDeOutliers: {
    id: number;
    descricao: string;
  };
}

declare interface FechamentoDetalhado extends Fechamento {
  centralizadoras: Array<{ codigo: number; nome: string }>;
}
