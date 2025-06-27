import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { debounceTime, switchMap } from 'rxjs/operators';
import { Subscription } from 'rxjs';
import { NumeroContaJudicialService } from './services/numero-conta-judicial.service';

@Component({
  selector: 'app-numero-conta-judicial',
  templateUrl: './numero-conta-judicial.component.html',
  styleUrls: ['./numero-conta-judicial.component.scss']
})
export class NumeroContaJudicialComponent implements OnInit, OnDestroy {

  form: FormGroup;
  numeroContaJudicialSubscription: Subscription;
  dadosEncontrados = [];
  constructor(private service: NumeroContaJudicialService) { }

  ngOnInit() {
    this.form = new FormGroup({
      numeroContaJudicial: new FormControl(null, [Validators.minLength(3),
                                                  Validators.maxLength(13),
                                                  Validators.required])
    });

    this.dadosEncontrados = this.service.listaContasSubject.value;

    this.numeroContaJudicialSubscription = this.form.get('numeroContaJudicial').valueChanges.pipe(
      debounceTime(3000),
      switchMap(valor => {
        if (this.form.get('numeroContaJudicial').valid) {
          return this.service.recuperarContaJudicial(valor)
        }
        return [];
      })).subscribe(res => {
        if(res['sucesso'] && res['data'] != null) {
          this.onBuscaRealizada(res['data']);
        }
        else this.onBuscaRealizada(null);
      });
  }

  ngOnDestroy() {
    this.numeroContaJudicialSubscription.unsubscribe();
  }

  onAdicionar(listaContas: any[]) {
    this.service.updateListaContas(listaContas);
  }

  onBuscaRealizada(data) {
    const oldDadosEncontrados = this.dadosEncontrados;
    const oldAdicionados = oldDadosEncontrados.filter(e => e.hasOwnProperty('adicionado') && e['adicionado']);

    // Uma nova busca poderia replicar os dados, verifica se o array ja contem o numero da conta
    const hasNumeroContaJudicial = oldAdicionados.some(item => item["Número da Conta Judicial"] == data);
    if (!hasNumeroContaJudicial && data != null)
      this.dadosEncontrados = [{"Número da Conta Judicial": data}, ...oldAdicionados];
    else if (!hasNumeroContaJudicial) this.dadosEncontrados = [...oldAdicionados];
  }
}
