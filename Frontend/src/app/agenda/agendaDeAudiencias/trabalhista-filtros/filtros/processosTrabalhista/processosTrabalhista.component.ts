import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { debounceTime, switchMap } from 'rxjs/operators';
import { renameProperty } from '@shared/utils';
import { ProcessosTrabalhistaService } from '../../services/processosTrabalhista.service';
import { Combobox } from '@shared/interfaces/combobox';


const valoresCombo: Combobox[] = [{ id: 1, descricao: 'Nº Processo' },
{ id: 2, descricao: 'Código Interno' }];

@Component({
  selector: 'processos-trab',
  templateUrl: './processosTrabalhista.component.html',
  styleUrls: ['./processosTrabalhista.component.scss']
})
export class ProcessosTrabalhistaComponent implements OnInit {

  constructor(public service: ProcessosTrabalhistaService) { }

  numeroProcessoSubscription: Subscription;

  form: FormGroup;
  dadosEncontrados = [];
  valoresCombo = valoresCombo;
  valorComboSelecionado = this.service.valorComboSelecionado$.value;
  //hablitarBotao = false;

  ngOnInit() {
    this.form = new FormGroup({
      numeroProcesso: new FormControl(null, [Validators.minLength(3),
      Validators.maxLength(50),
      Validators.required])
    });

    this.dadosEncontrados = this.service.listaProcessos$.value;

    this.numeroProcessoSubscription = this.form.get('numeroProcesso').valueChanges.pipe(
      debounceTime(3000),
      switchMap(valor => {
        if (this.form.get('numeroProcesso').valid) {
          return this.service.buscarProcesso(valor);
        }
        return [];
      })).subscribe(res => {
        if (res.sucesso && res.data != null) {
          this.onBuscaRealizada(res['data']);
        } else { return []; }
      });

    this.numeroProcessoSubscription.add(
      this.service.valorComboSelecionado$.subscribe(val =>
        this.valorComboSelecionado = val
      )
    );
  }


  ngOnDestroy() {
    this.numeroProcessoSubscription.unsubscribe();
  }

  onTypeSelect(e) {
    this.service.valorComboSelecionado$.next(e);
    this.form.reset();
  }

  onAdicionar(listaContas: any[]) {
    this.service.updateProcesoss(listaContas);
  }

  onBuscaRealizada(data) {
    const oldDadosEncontrados = this.dadosEncontrados;
    const oldAdicionados = oldDadosEncontrados.filter(e => e.hasOwnProperty('adicionado') && e['adicionado']);

    // Uma nova busca poderia replicar os dados, verifica se o array ja contem o numero da conta
    const newData = data.filter(e => !oldAdicionados.some(old => old["Codigo Interno"] == e["id"]));
    this.dadosEncontrados = [...oldAdicionados, ...newData];
    this.dadosEncontrados = this.dadosEncontrados.map(e => renameProperty(e, this.service.listaHeaderTabelaObj));
  }

}
