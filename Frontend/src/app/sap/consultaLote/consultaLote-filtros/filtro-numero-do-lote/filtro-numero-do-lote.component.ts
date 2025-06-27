import { FilterService } from './../../services/filter.service';
import { ConsultaNumeroLoteService } from './../../services/consulta-numero-lote.service';
import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { debounceTime, switchMap } from 'rxjs/operators';
import { renameProperty } from '@shared/utils';

@Component({
  selector: 'app-filtro-numero-do-lote',
  templateUrl: './filtro-numero-do-lote.component.html',
  styleUrls: ['./filtro-numero-do-lote.component.css']
})
export class FiltroNumeroDoLoteComponent implements OnInit {

  constructor(public service: ConsultaNumeroLoteService,
    private filterService: FilterService) { }

  numeroProcessoSubscription: Subscription;
  tipoProcesso: any;
  form: FormGroup;
  dadosEncontrados = [];
  //valoresCombo = valoresCombo;
  // valorComboSelecionado = this.service.valorComboSelecionado$.value;
  //hablitarBotao = false;

  ngOnInit() {
    this.form = new FormGroup({
      loteId: new FormControl(null, [Validators.required,
      Validators.maxLength(50),
      Validators.required])
    });

    this.filterService.tipoProcessoTracker.subscribe(processo => this.tipoProcesso = processo)

    this.dadosEncontrados = this.service.listaLotes$.value;

    this.numeroProcessoSubscription = this.form.get('loteId').valueChanges.pipe(
      debounceTime(3000),
      switchMap(valor => {
        if (this.form.get('loteId').valid) {

          return this.service.buscarProcesso(valor, this.tipoProcesso);
        }
        return [];
      })).subscribe(res => {
        if (res.sucesso && res.data != null) {
          this.onBuscaRealizada(res['data']);
        } else { return []; }
      });

    // this.numeroProcessoSubscription.add(
    //   this.service.valorComboSelecionado$.subscribe(val =>
    //     this.valorComboSelecionado = val
    //   )
    // );
  }


  ngOnDestroy() {
    this.numeroProcessoSubscription.unsubscribe();
  }

  onTypeSelect(e) {
    // this.service.valorComboSelecionado$.next(e);
    this.form.reset();
  }

  onAdicionar(lista: any[]) {

    this.service.updateProcesoss(lista);
  }

  onBuscaRealizada(data: any) {
    console.log("FiltroNumeroDoLoteComponent -> onBuscaRealizada -> data", data)

    if (data != 0) {
      const oldDadosEncontrados = this.dadosEncontrados;
      const oldAdicionados = oldDadosEncontrados.filter(e => e.hasOwnProperty('adicionado') && e['adicionado'])
      let dados = [];
      dados.push({ numeroLote: data })

      // Uma nova busca poderia replicar os dados, verifica se o array ja contem o numero da conta
      const newData = dados.filter(e => !oldAdicionados.some(old => old['Numero do Lote'] == e['numeroLote']));
      this.dadosEncontrados = [...oldAdicionados, ...newData];
      this.dadosEncontrados = this.dadosEncontrados.map(e => renameProperty(e, this.service.listaHeaderTabelaObj));
    }
  }

}
