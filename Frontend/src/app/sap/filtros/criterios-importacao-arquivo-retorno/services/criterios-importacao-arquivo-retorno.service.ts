import { ProcessosPexService } from 'src/app/sap/filtros/processos-pex/services/processos-pex.service';
import { ProcessosCCService } from 'src/app/sap/filtros/processos-cc/services/processos-cc.service';
import { ProcessosJECServiceService } from 'src/app/sap/filtros/processos-juizado-especial/services/processosJECService.service';
import { NumeroContaJudicialService } from './../../numero-conta-judicial/services/numero-conta-judicial.service';
import { NumeroGuiaService } from 'src/app/sap/filtros/numero-guia/services/numero-guia.service';
import { DataRange } from './../../../../shared/interfaces/data-range';
import { Injectable } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { BehaviorSubject } from 'rxjs';
import { NumeroRange } from '@shared/interfaces/numero-range';

@Injectable({
  providedIn: 'root'
})
export class CriteriosImportacaoArquivoRetornoService {

  constructor(private fb: FormBuilder,
    private numeroGuia: NumeroGuiaService,
    private numeroConta: NumeroContaJudicialService,
    private processosJac: ProcessosJECServiceService,
    private processosCC: ProcessosCCService,
    private processosPex: ProcessosPexService) { }

  private form: FormGroup;
  isValidSubject = new BehaviorSubject<boolean>(true);
  public contador = new BehaviorSubject<number>(0);

  public manterDados = false;

  data: DataRange = {
    dataInicio: null,
    dataFim: null,
  }
  numeroRemessa: NumeroRange = {
    numInicio: null,
    numFim: null
  }
  intervaloValores: NumeroRange = {
    numInicio: null,
    numFim: null
  }

  //#region inicializar o formulario
  inicializarFormulario() {
    this.form = this.fb.group({
      dataRemessa: this.fb.group(
        {
          dataInicio: this.data.dataInicio,
          dataFim: this.data.dataFim
        }),
      numeroRemessa: this.fb.group({
        numInicio: [this.numeroRemessa.numInicio, [Validators.maxLength(10)]],
        numFim: [this.numeroRemessa.numFim, [Validators.maxLength(10)]]
      }),
      intervaloValoresGuia: this.fb.group({
        numInicio: [this.intervaloValores.numInicio, [Validators.maxLength(15)]],
        numFim: [this.intervaloValores.numFim, [Validators.maxLength(15)]]
      })
    });
    return this.form;
  }
  //#endregion

  //#region get/Set para os valores do formulário, pois vem de um componente
  get dataRemessaInicio() {
    return this.data.dataInicio;
  }

  set dataRemessaInicio(dataInicio) {
    this.data.dataInicio = dataInicio;
  }

  get dataRemessaFim() {
    return this.data.dataFim;
  }

  set dataRemessaFim(dataFim) {
    this.data.dataFim = dataFim;
  }

  get numeroRemessaInicio() {
    if (this.numeroRemessa.numInicio) {
      return this.numeroRemessa.numInicio;
    }
  }

  set numeroRemessaInicio(numInicio) {
    this.numeroRemessa.numInicio = numInicio;
  }

  get numeroRemessaFim() {
    if (this.numeroRemessa.numFim) {
      return this.numeroRemessa.numFim;
    }
  }

  set numeroRemessaFim(numFim) {
    this.numeroRemessa.numFim = numFim;
  }

  get intervaloValoresGuiaInicio() {
    return this.intervaloValores.numInicio;
  }

  set intervaloValoresGuiaInicio(numInicio) {
    this.intervaloValores.numInicio = numInicio;
  }

  get intervaloValoresGuiaFim() {
    return this.intervaloValores.numFim;
  }

  set intervaloValoresGuiaFim(numFim) {
    this.intervaloValores.numFim = numFim;
  }
  //#endregion

  //#region contagem dos valores escolhidos na tela
  atualizarContagem() {
    let count = 0;
    if (this.data.dataInicio && this.data.dataFim) {
      count++;
    }
    if (this.numeroRemessaFim && this.numeroRemessaInicio) {
      count++;
    }
    if (this.intervaloValoresGuiaInicio && this.intervaloValoresGuiaFim) {
      count++;
    }

    this.contador.next(count);
  }
  //#endregion


  limparFormulario() {
    if (!this.manterDados) {
      this.numeroGuia.limpar();
      this.numeroConta.limpar();
      this.processosJac.limpar();
      this.processosCC.limpar();
      this.processosPex.limpar();
      if (this.form) {
        this.data = {
          dataInicio: null,
          dataFim: null,
        };
        this.numeroRemessa = {
          numInicio: null,
          numFim: null
        };
        this.intervaloValores = {
          numInicio: null,
          numFim: null
        };
        this.isValidSubject.next(true);
        this.contador.next(0)

      }
    }
    this.manterDados = false
  }
}
