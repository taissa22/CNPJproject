import { Component, OnInit } from '@angular/core';
import { faTrashAlt } from '@fortawesome/free-solid-svg-icons';
import { List } from 'linqts';
import { FormControl, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { FilterService } from '../../services/filter.service';
import { SapService } from 'src/app/core/services/sap/sap.service';
import { debounceTime, switchMap } from 'rxjs/operators';
import { TipoProcessoService } from 'src/app/core/services/sap/tipo-processo.service';
import { FiltroEndpointService } from '../../services/filtro-endpoint.service';
import { ListaFiltroRadioConsultaLoteEnum } from '@shared/enums/lista-filtro-radio-consulta-lote.enum';

@Component({
  selector: 'filtro-numero-conta-judicial',
  templateUrl: './filtro-numero-conta-judicial.component.html',
  styleUrls: ['./filtro-numero-conta-judicial.component.scss']
})
export class FiltroContaJudicialsComponent implements OnInit {

  faTrashAlt = faTrashAlt;
  public info = 'Realize a busca no campo acima';

  private contaJudicialLista: List<number> = new List();

  private contaJudicialsPreSelecionadas: List<number> = new List();

  public contaJudicial: string = null;

  contaJudicialId: FormControl;
  buscaForm: FormGroup;


  constructor(private filterService: FilterService,
    private formBuilder: FormBuilder,
    private sapService: SapService,
    private tipoProcessoService: TipoProcessoService,
    private service: FiltroEndpointService

  ) {



    this.contaJudicialId = new FormControl(this.contaJudicialId, [Validators.minLength(3), Validators.required]);
    this.buscaForm = this.formBuilder.group({
      contaJudicialId: this.contaJudicialId
    });

  }

  ngOnInit() {
    this.busca();

  }




  get contaJudicialIds() {
    return this.buscaForm.get('contaJudicialId');
  }


  public get contaJudicialsSelecionadas(): number[] {
    return this.filterService.contaJudicialsSelecionadas.ToArray();
  }

  public get contaJudicials(): Array<number> {
    return this.contaJudicialLista
      .Where(s => !this.filterService.contaJudicialsSelecionadas.Contains(s))
      .ToArray();
  }



  busca() {


    this.contaJudicialId.valueChanges.pipe(
      debounceTime(3000),
      switchMap((busca: number) => {
        busca = this.contaJudicialId.value.replace(/[^0-9]*/g, '');
        if (this.contaJudicialId.valid && busca) {
          return this.service.buscarContaJudicial(busca, this.filterService.tipoProcessoTracker.value);
        } else {
          this.contaJudicial = null;
          this.info = 'Realize a busca no campo acima';
          return 's';
        }
      })
    ).subscribe(res => {
      if (res != 's') {
        if (!this.filterService.contaJudicialsSelecionadas.Contains(this.contaJudicialId.value)) {
          this.contaJudicial = res.data;
        }
      } else{
        this.info = 'Realize a busca no campo acima';
    }
    });

}


  //#region filtro

  public adicionarContaJudicialSelecionados() {
  this.contaJudicialsPreSelecionadas.ForEach(contaJudicial => this.adicionaContaJudicial(contaJudicial));
  this.contaJudicialsPreSelecionadas = new List();
}

  private adicionaContaJudicial(contaJudicial: number) {
  if (!this.filterService.contaJudicialsSelecionadas.Contains(contaJudicial)) {
    this.filterService.contaJudicialsSelecionadas.Add(contaJudicial);
    this.contaJudicial = null;
    this.sapService.atualizaCount(this.filterService.contaJudicialsSelecionadas.ToArray().length,
      ListaFiltroRadioConsultaLoteEnum.numeroContaJudicial);

  }
}

  public removeContaJudicial(contaJudicial: string) {
  if (this.filterService.contaJudicialsSelecionadas.Contains(parseInt(contaJudicial))) {
    this.filterService.contaJudicialsSelecionadas.Remove(parseInt(contaJudicial));
    this.contaJudicial = contaJudicial;
    this.sapService.atualizaCount(this.filterService.contaJudicialsSelecionadas.ToArray().length,
      ListaFiltroRadioConsultaLoteEnum.numeroContaJudicial);
  }
}


  public preSelecionado(contaJudicial: string): boolean {
  return this.contaJudicialsPreSelecionadas.Contains(parseInt(contaJudicial));
}

  public preAdicionarContaJudicial(contaJudicial: string) {
  if (!this.contaJudicialsPreSelecionadas.Contains(parseInt(contaJudicial))) {
    this.contaJudicialsPreSelecionadas.Add(parseInt(contaJudicial));
  }
}


}
