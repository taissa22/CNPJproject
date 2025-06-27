import { Component, OnInit } from '@angular/core';
import { faTrashAlt } from '@fortawesome/free-solid-svg-icons';
import { List } from 'linqts';
import { FormControl, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { GuiaService } from '../../consultaLote/services/guia.service';
import { debounceTime, switchMap } from 'rxjs/operators';
import { NumeroGuiaService } from './services/numero-guia.service';

@Component({
  selector: 'app-numero-guia',
  templateUrl: './numero-guia.component.html',
  styleUrls: ['./numero-guia.component.scss']
})
export class NumeroGuiaComponent implements OnInit {

  faTrashAlt = faTrashAlt;
  public info = 'Realize a busca no campo acima';

  private guiaLista: List<number> = new List();

  private guiasPreSelecionados: List<number> = new List();

  public guia: number = null;

  guiaId: FormControl;
  guiaForm: FormGroup;


  constructor(private service: NumeroGuiaService, private guiaService: GuiaService,
    private formBuilder: FormBuilder) {



    this.guiaId = new FormControl(this.guiaId, [Validators.minLength(3), Validators.required]);
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

    return this.service.guiasSelecionadas.ToArray();
  }

  public get guias(): Array<number> {
    return this.guiaLista
      .Where(s => !this.service.guiasSelecionadas.Contains(s))
      .ToArray();
  }



  buscaGuias() {

    this.guiaId.valueChanges.subscribe(valor => {
      if (valor.length > 1) {

      }
    });

    this.guiaId.valueChanges.pipe(
      debounceTime(3000),
      switchMap((busca: number) => {


        busca = this.guiaId.value.replace(/[^0-9]*/g, '');

        if (this.guiaId.valid && busca) {

          return this.guiaService.buscaGuias(busca, this.service.tipoProcesso);

        } else {
          this.guia = null;
          this.info = 'Realize a busca no campo acima';
          return 's';
        }

      })
    ).subscribe(res => {
      if (res != 's') {
        this.info = 'Nenhum resultado encontrado';
        if (!this.service.guiasSelecionadas.Contains(parseInt(this.guiaId.value))) {
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
    if (!this.service.guiasSelecionadas.Contains(guia)) {
      this.service.guiasSelecionadas.Add(guia);
      this.guia = null;
      this.service.atualizarContagem();
    }
  }

  public removeGuia(guia: string) {
    if (this.service.guiasSelecionadas.Contains(parseInt(guia))) {
      this.service.guiasSelecionadas.Remove(parseInt(guia));
      this.guia = parseInt(guia);
      this.service.atualizarContagem();
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
