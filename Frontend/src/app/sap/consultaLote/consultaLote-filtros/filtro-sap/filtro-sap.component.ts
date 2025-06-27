import { Component, OnInit } from '@angular/core';
import { FilterService } from '../../services/filter.service';
import { LoteService } from 'src/app/core/services/sap/lote.service';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { SapService } from 'src/app/core/services/sap/sap.service';
import { TipoProcessoService } from 'src/app/core/services/sap/tipo-processo.service';
import { faTrashAlt } from '@fortawesome/free-solid-svg-icons';
import { List } from 'linqts';
import { debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';
import { ListaFiltroRadioConsultaLoteEnum } from '@shared/enums/lista-filtro-radio-consulta-lote.enum';

const QUERY_DELAY = 3000;

@Component({
  // tslint:disable-next-line: component-selector
  selector: 'filtro-sap',
  templateUrl: './filtro-sap.component.html',
  styleUrls: ['./filtro-sap.component.scss']
})
export class FiltroSapComponent implements OnInit {
  constructor(private filterService: FilterService, private loteService: LoteService,
              private formBuilder: FormBuilder, private sapService: SapService, private tipoProcessoService: TipoProcessoService ) { }

  get loteId() { return this.loteForm.get('loteId'); }

  public get sapsSelecionados(): number[] {
    return this.filterService.sapsSelecionados.ToArray();
  }


  public get saps(): Array<number> {
    return this.sapLista
      .Where(s => !this.filterService.sapsSelecionados.Contains(s))
      .ToArray();
  }

  faTrashAlt = faTrashAlt;
  private sapLista: List<number> = new List();
  private sapsPreSelecionados: List<number> = new List();
  private lotesId: any;
  public lote: string = null;
  public carregando = false;
  public info = 'Realize a busca no campo acima';

  loteForm: FormGroup = this.formBuilder.group({
    loteId: new FormControl(this.lotesId, [Validators.minLength(3), Validators.required])
  });
  ngOnInit() {
    this.buscaLotes();
  }

  buscaLotes() {
    this.loteForm.valueChanges.pipe(
      debounceTime(3000),
      distinctUntilChanged(),
      switchMap(id => {
        id = this.loteId.value.replace(/[^0-9]*/g, '');
        if (this.loteId.valid) {
          return this.loteService.buscaLotes(id, this.filterService.tipoProcessoTracker.value);
        } else {
          this.lote = null;
          this.info = 'Realize a busca no campo acima';
          return this.loteService.buscaLotes('0', this.filterService.tipoProcessoTracker.value);
        }
      })
    ).subscribe(res => {
      if (!this.filterService.sapsSelecionados.Contains(parseInt(this.loteId.value))) {
        this.lote = res.data;
      }

      this.info = 'Nenhum resultado encontrado';
    });

  }


  public adicionarSapsSelecionados() {
    this.sapsPreSelecionados.ForEach(sap => this.adicionaSap(sap));
    this.sapsPreSelecionados = new List();
  }

  private adicionaSap(sap: number) {
    if (!this.filterService.sapsSelecionados.Contains(sap)) {
      this.filterService.sapsSelecionados.Add(sap);
      this.lote = null;
      this.sapService.atualizaCount(this.filterService.sapsSelecionados.ToArray().length,
        ListaFiltroRadioConsultaLoteEnum.pedidoSap);
    }
  }

  public removelote(lote: string) {
    if (this.filterService.sapsSelecionados.Contains(parseInt(lote))) {
      this.filterService.sapsSelecionados.Remove(parseInt(lote));
      this.lote = lote;
      this.sapService.atualizaCount(this.filterService.sapsSelecionados.ToArray().length,
        ListaFiltroRadioConsultaLoteEnum.pedidoSap);
    }

  }

  public preSelecionado(sap: string): boolean {
    return this.sapsPreSelecionados.Contains(parseInt(sap));
  }

  public preAdicionarSap(sap: string) {
    if (!this.sapsPreSelecionados.Contains(parseInt(sap))) {
      this.sapsPreSelecionados.Add(parseInt(sap));
    }
  }


  // public adicionarTodosSaps() {
  //   this.saps.forEach(sap => this.adicionaSap(sap));
  //   this.sapsPreSelecionados = new List();
  // }


}
