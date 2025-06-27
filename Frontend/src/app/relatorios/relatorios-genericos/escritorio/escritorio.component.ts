import { Component, OnInit, EventEmitter, Output, AfterContentInit, DoCheck, OnDestroy } from '@angular/core';
import { RelatorioService } from 'src/app/core/services/relatorio.service';
import { DualListModel } from 'src/app/core/models/dual-list.model';
import { TypeofExpr } from '@angular/compiler';
import { Subscription } from 'rxjs/internal/Subscription';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-escritorio',
  templateUrl: './escritorio.component.html',
  styleUrls: ['./escritorio.component.scss']
})
export class EscritorioComponent implements OnInit, OnDestroy {


  listasEscritorios: Array<DualListModel> = [];
  inscricao: Subscription;
  // @Output()itensSelecionados: EventEmitter<number> = new EventEmitter<number>();

  constructor(
    private reatorioService: RelatorioService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  mostraSelecionados(event: Array<DualListModel>) {
    const listass = event.map(teste => {
      return teste.id;
    });
    this.reatorioService.AddlistaEscritorios(listass);
    this.reatorioService.atualizaCountEscritorio(event.length);
  }
  ngOnInit() {
    this.inscricao = this.route.data.subscribe(info => {
      this.listasEscritorios = info.listaEscritorio;
    });

  }

  ngOnDestroy() {
    this.inscricao.unsubscribe();

  }

}
