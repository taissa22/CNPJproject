import { AfterViewInit, Component, OnInit } from '@angular/core';

import { BehaviorSubject, EMPTY, of } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';

import { environment } from '@environment';
import { Permissoes, PermissoesService } from '@permissoes';

import { FechamentosProvisaoTrabalhistaService } from '../../services/fechamentos-provisao-trabalhista.service';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
@Component({
  selector: 'app-estoque-de-pedidos-page',
  templateUrl: './estoque-de-pedidos-page.component.html',
  styleUrls: ['./estoque-de-pedidos-page.component.scss']
})
export class EstoqueDePedidosPageComponent implements OnInit, AfterViewInit {
  breadcrumb: string;
  constructor(
    permissoesService: PermissoesService,
    private fechamentosProvisaoTrabalhistaService: FechamentosProvisaoTrabalhistaService,
    private breadcrumbsService: BreadcrumbsService
  ) {
    if (
      permissoesService.temPermissaoPara(
        Permissoes.FILTRAR_ESTOQUE_DE_PEDIDOS_OUTLIER_SEM_EXCLUSAO
      )
    ) {
      this.outliers.push({ id: 0, descricao: 'Sem exclusão de Outliers' });
    }

    if (
      permissoesService.temPermissaoPara(
        Permissoes.FILTRAR_ESTOQUE_DE_PEDIDOS_OUTLIER_POR_DESVIO_PADRAO
      )
    ) {
      this.outliers.push({
        id: 1,
        descricao: 'Com exclusão de Outliers por Desvio Padrão'
      });
    }

    if (
      permissoesService.temPermissaoPara(
        Permissoes.FILTRAR_ESTOQUE_DE_PEDIDOS_OUTLIER_POR_PERCENTUAL
      )
    ) {
      this.outliers.push({
        id: 2,
        descricao: 'Com exclusão de Outliers por Percentual'
      });
    }
  }

  readonly outliers: Array<{ id: number; descricao: string }> = [];
  private readonly outlierAtual$ = new BehaviorSubject<TipoDeOutlier>(null);
  get outlierAtual(): TipoDeOutlier {
    return this.outlierAtual$.value;
  }
  set outlierAtual(value: TipoDeOutlier) {
    this.outlierAtual$.next(value);
  }

  fechamentos: Fechamento[] = [];
  private readonly fechamentoAtual$ = new BehaviorSubject<Fechamento>(null);
  get fechamentoAtual(): Fechamento {
    return this.fechamentoAtual$.value;
  }
  set fechamentoAtual(value: Fechamento) {
    this.fechamentoAtual$.next(value);
  }

  centralizadoras: Array<Centralizadora> = [];
  private readonly centralizadorasSelecionadas$ = new BehaviorSubject<
    Array<Centralizadora>
  >([]);
  get centralizadorasSelecionadas(): Array<Centralizadora> {
    return this.centralizadorasSelecionadas$.value;
  }
  set centralizadorasSelecionadas(value: Array<Centralizadora>) {
    this.centralizadorasSelecionadas$.next(value);
  }

  ngOnInit(): void {
    this.outlierAtual$
      .pipe(
        switchMap(value => {
          if (!value) {
            return EMPTY;
          }
          return this.fechamentosProvisaoTrabalhistaService
            .obterTodos(value.id === 0, value.id === 2, value.id === 1)
            .pipe(catchError(() => EMPTY));
        })
      )
      .subscribe(fechamentos => {
        this.fechamentoAtual = null;
        this.fechamentos = fechamentos;
      });

    this.fechamentoAtual$
      .pipe(
        switchMap(value => {
          if (!value) {
            this.centralizadoras = [];
            return EMPTY;
          }
          return this.fechamentosProvisaoTrabalhistaService.obterFechamentoDetalhado(
            value.id
          );
        }),
        switchMap(value => of(value.centralizadoras))
      )
      .subscribe(value => (this.centralizadoras = value));

    this.centralizadorasSelecionadas$
      .pipe(
        switchMap(value => {
          if (value.length !== 1) {
            return of([]);
          }
          return this.fechamentosProvisaoTrabalhistaService.obterEmpresasDaCentralizadora(
            value[0].codigo
          );
        })
      )
      .subscribe(value => (this.empresasDoGrupo = value));
  }

  async ngAfterViewInit() {
    this.clear();
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(Permissoes.ACESSAR_ESTOQUE_DE_PEDIDOS);
  }

  clear(): void {
    if (this.outliers.length > 0) {
      this.outlierAtual = this.outliers[0];
    }
  }

  empresasDoGrupo: Array<EmpresaDoGrupo> = [];
  empresasDoGrupoSelecionadas: Array<EmpresaDoGrupo> = [];

  paginaResult(): void {
    let url: string =
      `${environment.s1_url}/2.0/relatorios/fechamento/provisao/trabalhista/RelatorioEfeitoEstoque.aspx?` +
      `CodigoEmpresaCentralizadora=${this.centralizadorasSelecionadas
        .map(x => x.codigo)
        .join(';')}&` +
      `DataFechamento=${this.getOADate(this.fechamentoAtual.dataFechamento)}&` +
      `Meses=${this.fechamentoAtual.numeroDeMeses}&` +
      `Outlier=${this.fechamentoAtual.tipoDeOutliers.id}&` +
      `CodigoEmpresaDoGrupo=`;

    if (this.empresasDoGrupoSelecionadas.length > 0) {
      url += this.empresasDoGrupoSelecionadas.map(x => x.id).join(';');
    }

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

declare interface TipoDeOutlier {
  id: number;
  descricao: string;
}

declare interface Fechamento {
  id: number;
  dataFechamento: Date;
  numeroDeMeses: number;
  tipoDeOutliers: TipoDeOutlier;
}

declare interface FechamentoDetalhado extends Fechamento {
  centralizadoras: Array<Centralizadora>;
}

declare interface Centralizadora {
  codigo: number;
  nome: string;
}

declare interface EmpresaDoGrupo {
  id: number;
  nome: string;
}
