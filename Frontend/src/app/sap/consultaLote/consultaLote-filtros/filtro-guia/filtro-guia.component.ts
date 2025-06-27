import { Component, OnInit } from '@angular/core';
import { faTrashAlt } from '@fortawesome/free-solid-svg-icons';

import { List } from 'linqts';
import { FormControl, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { debounceTime } from 'rxjs/operators';
import { switchMap } from 'rxjs/operators';
import { SapService } from 'src/app/core/services/sap/sap.service';
import { TipoProcessoService } from 'src/app/core/services/sap/tipo-processo.service';
import { FilterService } from '../../services/filter.service';
import { GuiaService } from '../../services/guia.service';
import { ListaFiltroRadioConsultaLoteEnum } from '@shared/enums/lista-filtro-radio-consulta-lote.enum';


const QUERY_DELAY = 3000;

@Component({
  // tslint:disable-next-line: component-selector
  selector: 'filtro-guia',
  templateUrl: './filtro-guia.component.html',
  styleUrls: ['./filtro-guia.component.scss']
})
export class FiltroGuiaComponent implements OnInit {

  faTrashAlt = faTrashAlt;
  public info = 'Realize a busca no campo acima';

  private guiaLista: List<number> = new List();

  private guiasPreSelecionados: List<number> = new List();

  public guia: number = null;

  guiaId: FormControl;
  guiaForm: FormGroup;


  constructor(private filterService: FilterService, private guiaService: GuiaService,
    private formBuilder: FormBuilder, private sapService: SapService, private tipoProcessoService: TipoProcessoService) {



    this.guiaId = new FormControl(this.guiaId, [Validators.required, Validators.required]);
    this.guiaForm = this.formBuilder.group({
      guiaId: this.guiaId
    });

  }

  ngOnInit() {
    this.buscaGuias();

  }




  get guiaIds() {
    return this.guiaForm.get('guiaId');
  }


  public get guiasSelecionados(): number[] {

    return this.filterService.guiasSelecionadas.ToArray();
  }

  public get guias(): Array<number> {
    return this.guiaLista
      .Where(s => !this.filterService.guiasSelecionadas.Contains(s))
      .ToArray();
  }



  buscaGuias() {

    this.guiaId.valueChanges.subscribe(valor => {
      if (valor.length > 1) {

      }
    })
    this.guiaId.valueChanges.pipe(
      debounceTime(3000),
      switchMap((busca: number) => {


        busca = this.guiaId.value.replace(/[^0-9]*/g, '');

        if (this.guiaId.valid && busca) {

          return this.guiaService.buscaGuias(busca, this.filterService.tipoProcessoTracker.value);

        } else {
          this.guia = null;
          this.info = 'Realize a busca no campo acima';
          return 's'
        }

      })
    ).subscribe(res => {
      if (res != 's') {
        this.info = 'Nenhum resultado encontrado';
        if (!this.filterService.guiasSelecionadas.Contains(parseInt(this.guiaId.value))) {
          this.guia = res.data;
        }
      } else {
        this.info = 'Realize a busca no campo acima';
      }
    });



  }


  //#region filtro

  public adicionarGuiasSelecionados() {
    this.guiasPreSelecionados.ForEach(guia => this.adicionaGuia(guia));
    this.guiasPreSelecionados = new List();
  }

  private adicionaGuia(guia: number) {
    if (!this.filterService.guiasSelecionadas.Contains(guia)) {
      this.filterService.guiasSelecionadas.Add(guia);
      this.guia = null;
      this.sapService
        .atualizaCount(this.filterService.guiasSelecionadas.ToArray().length,
          ListaFiltroRadioConsultaLoteEnum.numeroGuia);

    }
  }

  public removeGuia(guia: string) {
    if (this.filterService.guiasSelecionadas.Contains(parseInt(guia))) {
      this.filterService.guiasSelecionadas.Remove(parseInt(guia));
      this.guia = parseInt(guia);
      this.sapService.atualizaCount(this.filterService.guiasSelecionadas.ToArray().length, 11);
    }
  }


  public preSelecionado(guia: string): boolean {

    return this.guiasPreSelecionados.Contains(parseInt(guia));
  }

  public preAdicionarGuia(guia: string) {
    if (!this.guiasPreSelecionados.Contains(parseInt(guia))) {
      this.guiasPreSelecionados.Add(parseInt(guia));
    }
  }



}
