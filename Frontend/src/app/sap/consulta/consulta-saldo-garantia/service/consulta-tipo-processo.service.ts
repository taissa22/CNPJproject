import { Injectable } from '@angular/core';
import { TiposProcessosMapped, TiposProcessosCivelPluralMapped } from '@shared/utils';
import { BehaviorSubject, Observable } from 'rxjs';
import { ApiService } from 'src/app/core';
import { TipoProcesso } from 'src/app/core/models/tipo-processo';
import { pluck } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ConsultaTipoProcessoService {

  public tipoProcessoTracker = new BehaviorSubject<number>(null);
  public tipoProcessoAtual: number;
  public tela = 'consultaSaldoGarantia';
  constructor(private apiService: ApiService) { }

  LimparTipoProcesso() {
    this.tipoProcessoAtual = null;
    this.tipoProcessoTracker.next(this.tipoProcessoAtual);
  }

  updateTipoProcesso(tipoProcesso: number) {
    this.tipoProcessoAtual = tipoProcesso;
    this.tipoProcessoTracker.next(this.tipoProcessoAtual);

  }

  getnomeProcesso() {
    let r;
    TiposProcessosMapped.filter(i => i.idTipo === this.tipoProcessoTracker.value).map(n => r = n.nome);
    return r;
  }

  get nomeProcessoPlural() {
    let nomeProcesso;
     TiposProcessosCivelPluralMapped.filter(i => i.idTipo === this.tipoProcessoTracker.value).map(n => nomeProcesso = n.nome);
    return nomeProcesso;
  }
}
