import { Injectable } from '@angular/core';
import { ApiService } from '..';
import { IResultado } from '@shared/interfaces/resultado';
import { Router, ActivatedRoute } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { TiposProcessosMapped } from '@shared/utils';


@Injectable({
  providedIn: 'root'
})

export class CriacaoService {
  constructor( private api: ApiService, private router: Router) {

  }

  private  tipoProcesso = new BehaviorSubject<number>(null);

  dataInicio: Date;
  dataFim: Date;
  valorInicial: number;
  valorFinal: number;

  return;

  get tipoProcessoSelecionado() {
    return this.tipoProcesso.value;
  }

  set tipoProcessoSelecionado(v) {
    this.tipoProcesso.next(v);
  }

  get nomeProcesso() {
    let r;
    TiposProcessosMapped.filter(i => i.idTipo === this.tipoProcessoSelecionado).map(n => r = n.nome);
    return r;
  }


  getFiltro() {
    return {
      dataInicio: this.dataInicio,
      dataFim: this.dataFim,
      tipoProcesso: this.tipoProcessoSelecionado,
      valorInicial: this.valorInicial,
      valorFinal: this.valorFinal
    };
 }

  limparDados() {
    this.dataInicio = null;
    this.dataFim = null;
    this.valorInicial = null;
    this.valorFinal = null;
  }

  getLancamentoLotes(json) {
    return this.api.post('/Lotes/RecuperarLancamentosLote', json).subscribe(val => this.return = val);
}

goToResultado() {
  this.router.navigate(['sap/lote/criar/resultado']);

}

}
