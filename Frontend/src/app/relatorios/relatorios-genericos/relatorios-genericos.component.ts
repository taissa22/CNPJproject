import { Component, OnInit, AfterContentInit } from '@angular/core';
import { RelatorioService } from '../../core/services/relatorio.service';
import { ActivatedRoute, Router } from '@angular/router';
import { FiltroModel } from '../../core/models/filtro.model';

@Component({
  selector: 'app-relatorios-genericos',
  templateUrl: './relatorios-genericos.component.html'
})
export class RelatoriosGenericosComponent implements OnInit , AfterContentInit {
  constructor(
    private relatorioService: RelatorioService,
    private route: ActivatedRoute,
    private router: Router
    ) { }
  tipo = '';
  tituloRelatorio: string;
  caminhoPagina: string;
  listaFiltro: Array<FiltroModel> = [];


  ngAfterContentInit(): void {
  }

  ngOnInit() {
    this.iniciaServico();
    this.router.navigate(['criterios'], {relativeTo: this.route});
  }
  iniciaServico() {
    this.route.params.subscribe(params => {
      this.tipo = params.tipo;
      if (this.tipo === 'consumidor') {
        this.listaFiltro = this.relatorioService.getFiltroRelatorioCivel();
        this.tituloRelatorio = 'Relatório Genérico Civel Consumidor';
        this.caminhoPagina = 'Relatório > Genérico > Civel Consumidor';
       }
      if (this.tipo === 'estrategico') {
        this.listaFiltro = this.relatorioService.getFiltroRelatorioEstrategico();
        this.tituloRelatorio = 'Relatório Genérico Civel Estratégico';
        this.caminhoPagina = 'Relatório > Genérico > Civel Estratégico';

      }
      if (this.tipo === 'procon') {
        this.listaFiltro = this.relatorioService.getFiltroRelatorioProcon();
        this.tituloRelatorio = 'Relatório Genérico Procon';
        this.caminhoPagina = 'Relatório > Genérico > Procon';

      }
    });
  }

}
