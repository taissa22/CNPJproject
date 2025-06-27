import { Component, OnInit } from '@angular/core';
import {  FiltroEndpointService } from '../../services/filtro-endpoint.service';
import { faTrashAlt } from '@fortawesome/free-solid-svg-icons';
import { List } from 'linqts';
import { FormControl, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { FilterService } from '../../services/filter.service';
import { SapService } from 'src/app/core/services/sap/sap.service';
import { TipoProcessoService } from 'src/app/core/services/sap/tipo-processo.service';
import { debounceTime, switchMap } from 'rxjs/operators';
import { ListaFiltroRadioConsultaLoteEnum } from '@shared/enums/lista-filtro-radio-consulta-lote.enum';

@Component({
  selector: 'filtro-processos',
  templateUrl: './filtro-processos.component.html',
  styleUrls: ['./filtro-processos.component.scss']
})
export class FiltroProcessosComponent implements OnInit {

  // constructor(private service: FiltroProcessoService) { }




  faTrashAlt = faTrashAlt;
  public info = 'Realize a busca no campo acima';

  private processoLista: List<any> = new List();

  private processosPreSelecionados: List<any> = new List();

  public processo: any[] = null;

  public tipoProcessoSelecionado: number;

  processoId: FormControl;
  buscaForm: FormGroup;


  constructor(private filterService: FilterService,
              private formBuilder: FormBuilder,
              private sapService: SapService,
              private tipoProcessoService: TipoProcessoService,
              private service: FiltroEndpointService,

  ) {

    this.processoId = new FormControl(this.processoId,
      [Validators.minLength(3), Validators.required]);
    this.buscaForm = this.formBuilder.group({
      processoId: this.processoId
    });

  }

  ngOnInit() {
    this.busca();
    this.filterService.tipoProcessoTracker.
    subscribe(processo => this.tipoProcessoSelecionado = processo);
  }


  get processoIds() {
    return this.buscaForm.get('processoId');
  }


  public get processosSelecionados(): any[] {
    return this.filterService.processosSelecionados.ToArray();
  }

  public get processos(): Array<number> {
    return this.processoLista
      .Where(s => !this.filterService.processosSelecionados.Any(v =>
        v.id == s.id))
      .ToArray();
  }



  busca() {
    this.processoId.valueChanges.pipe(
      debounceTime(3000),
      switchMap((busca: any) => {
        busca = this.processoId.value.replace(/[^0-9\-\.]*/g, '');
        if (this.processoId.valid && busca) {
          return this.service.buscarProcesso(busca,
            this.tipoProcessoSelecionado, 'consultaSaldoGarantia');
        } else {
          this.processo = null;
          return 's';

        }
      })
    ).subscribe(res => {
      if (res != 's') {
        this.info = 'Nenhum resultado encontrado';
        if (!this.filterService.processosSelecionados.
          Any(v => v.numeroProcesso == this.processoId.value)
        ) {
          this.processo = res.data;
        }
      } else {
        this.info = 'Realize a busca no campo acima';
      }
    });
  }

  //#region filtro

  public adicionarProcessosSelecionados() {
    this.processosPreSelecionados.ForEach(processo => this.adicionaProcessos(processo));
    this.processosPreSelecionados = new List();
  }

  private adicionaProcessos(processo: any) {
    if (!this.filterService.processosSelecionados.Any(v => v.id == processo.id)) {
      this.filterService.processosSelecionados.Add(processo);
      this.processo.forEach((item, index) => {
        if (item.id === processo.id) {
          this.processo.splice(index, 1);
        }
      });
      this.sapService.atualizaCount(this.filterService.processosSelecionados.ToArray().length,
      ListaFiltroRadioConsultaLoteEnum.processos);

    }
  }

  public removeProcesso(processo: any) {
    if (this.filterService.processosSelecionados.Any(v => v.id == processo.id)) {
      this.filterService.processosSelecionados.Remove(processo);
      this.processo.push(processo);
      this.sapService.atualizaCount(this.filterService.processosSelecionados.ToArray().length,
      ListaFiltroRadioConsultaLoteEnum.processos);    }
  }


  public preSelecionado(processo: any): boolean {
    let a;
    this.processosPreSelecionados.ForEach(
      item => {
        if (item.id == processo.id) {
          a = item;
        }
      }
    );
    return a;
  }

  public preAdicionarProcesso(processo: any) {
    if (!this.processosPreSelecionados.Any(v => v.id == processo.id)) {
      this.processosPreSelecionados.Add(processo);
    }
  }



}
