import { Component, OnInit } from '@angular/core';
import { ProcessosCCService } from './services/processos-cc.service';
import { Subscription } from 'rxjs';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { debounceTime, switchMap } from 'rxjs/operators';
import { renameProperty } from '@shared/utils';

@Component({
  selector: 'app-processos-cc',
  templateUrl: './processos-cc.component.html',
  styleUrls: ['./processos-cc.component.scss']
})
export class ProcessosCCComponent implements OnInit {

  constructor(public service: ProcessosCCService) { }

  numeroProcessoSubscription: Subscription;

  form: FormGroup;
  dadosEncontrados = [];

  ngOnInit() {
    this.form = new FormGroup({
      numeroProcesso: new FormControl(null, [Validators.minLength(3),
      Validators.maxLength(50),
      Validators.required])
    });

    this.dadosEncontrados = this.service.listaProcessosSubject.value;

    this.numeroProcessoSubscription = this.form.get('numeroProcesso').valueChanges.pipe(
      debounceTime(3000),
      switchMap(valor => {
        if (this.form.get('numeroProcesso').valid) {
          return this.service.recuperarProcesso(valor);
        }
        return [];
      })).subscribe(res => {
        if (res.sucesso && res.data != null) {
          this.onBuscaRealizada(res['data']);
        } else { return []; }
      });
  }

  ngOnDestroy() {
    this.numeroProcessoSubscription.unsubscribe();
  }

  onAdicionar(listaContas: any[]) {
    this.service.updateProcesoss(listaContas);
  }

  onBuscaRealizada(data) {
    const oldDadosEncontrados = this.dadosEncontrados;
    const oldAdicionados = oldDadosEncontrados.filter(e => e.hasOwnProperty('adicionado') && e['adicionado']);

    // Uma nova busca poderia replicar os dados, verifica se o array ja contem o numero da conta
    const newData = data.filter(e => !oldAdicionados.some(old => old["id"] == e["id"]));
    this.dadosEncontrados = [...oldAdicionados, ...newData];
    this.dadosEncontrados = this.dadosEncontrados.map(e => renameProperty(e, this.service.listaHeaderTabelaObj));
  }
}
