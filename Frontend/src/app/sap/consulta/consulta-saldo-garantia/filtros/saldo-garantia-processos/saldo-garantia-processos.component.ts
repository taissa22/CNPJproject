import { Component, OnInit } from '@angular/core';
import { faTrashAlt } from '@fortawesome/free-solid-svg-icons';
import { List } from 'linqts';
import { FormControl, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { FiltroEndpointService } from 'src/app/sap/consultaLote/services/filtro-endpoint.service';
import { debounceTime, switchMap } from 'rxjs/operators';
import { ConsultaFiltroProcessoService } from '../../service/consulta-filtro-processo.service';
import { ConsultaTipoProcessoService } from '../../service/consulta-tipo-processo.service';
import { ConsultaSaldoGarantiaService } from '../../service/consulta-saldo-garantia.service';
import { TipoProcessoEnum } from '@shared/enums/tipo-processoEnum.enum';

@Component({
  selector: 'saldo-garantia-processos',
  templateUrl: './saldo-garantia-processos.component.html',
  styleUrls: ['./saldo-garantia-processos.component.scss']
})
export class SaldoGarantiaProcessosComponent implements OnInit {

  // constructor(private service: FiltroProcessoService) { }

  // TODO VIR AQUI CONSERTAR


  faTrashAlt = faTrashAlt;
  public info = 'Realize a busca no campo acima';
  enumTipoProcesso


  private processoLista: List<any> = new List();

  private processosPreSelecionados: List<any> = new List();

  public processo: any = null;

  public tipoProcessoSelecionado: number;

  processoId: FormControl;
  buscaForm: FormGroup;


  constructor(private service: ConsultaFiltroProcessoService,
              private formBuilder: FormBuilder,
              private filtroEndpointService: FiltroEndpointService,
              private consultaTipoProcessoService: ConsultaTipoProcessoService,
              private consultaSaldoGarantia: ConsultaSaldoGarantiaService

  ) {

    this.processoId = new FormControl(this.processoId,
      [Validators.minLength(3), Validators.required]);
    this.buscaForm = this.formBuilder.group({
      processoId: this.processoId
    });

  }

  ngOnInit() {
    this.busca();
    this.consultaTipoProcessoService.tipoProcessoTracker.
      subscribe(processo => this.tipoProcessoSelecionado = processo);
    this.enumTipoProcesso  = TipoProcessoEnum;
  }


  get processoIds() {
    return this.buscaForm.get('processoId');
  }


  public get processosSelecionados(): any[] {
    return this.service.processosSelecionados.ToArray();
  }

  public get processos(): Array<number> {
    return this.processoLista
      .Where(s => !this.service.processosSelecionados.Any(v =>
        v.id == s.id))
      .ToArray();
  }



  busca() {
    this.processoId.valueChanges.pipe(
      debounceTime(3000),
      switchMap((busca: any) => {
        busca = this.processoId.value.replace(/[^0-9\-\.]*/g, '');
        if (this.processoId.valid && busca) {
          return this.filtroEndpointService.buscarProcesso(busca,
            this.tipoProcessoSelecionado, 'consultaSaldoGarantia');
        } else {
          this.processo = null;
          return 's';

        }
      })
    ).subscribe(res => {
      if (res != 's') {
        this.info = 'Nenhum resultado encontrado';
        if (!this.service.processosSelecionados.
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
    if (!this.service.processosSelecionados.Any(v => v.id == processo.id)) {
      this.service.processosSelecionados.Add(processo);
      this.processo.forEach((item, index) => {
        if (item === processo) {
          this.processo.splice(index, 1);
        }
      });
      this.service.atualizarCount();

    }
  }

  public removeProcesso(processo: any) {
    if (this.service.processosSelecionados.Any(v => v.id == processo.id)) {
      this.service.processosSelecionados.Remove(processo);
      this.processo.push(processo);
      this.service.atualizarCount();
    }
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
