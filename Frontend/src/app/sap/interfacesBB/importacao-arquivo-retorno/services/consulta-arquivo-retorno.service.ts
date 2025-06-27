import { formatDate } from '@angular/common';
import { Injectable, ValueProvider } from '@angular/core';
import { Router } from '@angular/router';
import { CriteriosSelecaoFiltro } from '@shared/enums/criteriosSelecaoFiltro.enum';
import { HelperAngular } from '@shared/helpers/helper-angular';
import { ConsultaArquivoRetornoFiltroDTO } from '@shared/interfaces/consulta-arquivo-retorno-filtro-dto';
import { BehaviorSubject, Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { FiltroModel } from 'src/app/core/models/filtro.model';
import { BBResumoProcessamentoService } from 'src/app/core/services/sap/bbresumo-processamento.service';
import { DownloadService } from 'src/app/core/services/sap/download.service';
import { UploadService } from 'src/app/core/services/upload.service';
import { CriteriosImportacaoArquivoRetornoService } from 'src/app/sap/filtros/criterios-importacao-arquivo-retorno/services/criterios-importacao-arquivo-retorno.service';
import { NumeroContaJudicialService } from 'src/app/sap/filtros/numero-conta-judicial/services/numero-conta-judicial.service';
import { NumeroGuiaService } from 'src/app/sap/filtros/numero-guia/services/numero-guia.service';
import { ProcessosCCService } from 'src/app/sap/filtros/processos-cc/services/processos-cc.service';
import { ProcessosJECServiceService } from 'src/app/sap/filtros/processos-juizado-especial/services/processosJECService.service';
import { ProcessosPexService } from 'src/app/sap/filtros/processos-pex/services/processos-pex.service';
import { environment } from 'src/environments/environment';
import { ResultadoArquivoRetornoService } from './resultado-arquivo-retorno.service';
import { APIResponse } from 'src/app/sap/shared/interfaces/apiresponse';
import { quantidadePaginainicial } from 'src/app/app.constants';

@Injectable({
  providedIn: 'root'
})
export class ConsultaArquivoRetornoService {
  public manterDados = false;

  consultaDTO = {
    numeroRemessaMenor: null,
    numeroRemessaMaior: null,
    numerosContasJudiciais: [],
    idsProcessosCC: null,
    idsProcessosPEX: null,
    idsProcessosJEC: null,
    idsNumerosGuia: null,
    dataRemessaMenor: null,
    dataRemessaMaior: null,
    valorGuiaInicio: null,
    valorGuiaFim: null,
    pagina: 1,
    quantidade: quantidadePaginainicial,
    total: 0
  };


  constructor(private numeroGuiaService: NumeroGuiaService,
    private criteriosGuiaService: CriteriosImportacaoArquivoRetornoService,
    private numeroContaJudicialService: NumeroContaJudicialService,
    private processoCCService: ProcessosCCService,
    private processoPexService: ProcessosPexService,
    private processoJecService: ProcessosJECServiceService,
    private service: BBResumoProcessamentoService,
    private resultadoService: ResultadoArquivoRetornoService,
    private uploadService: UploadService,
    private router: Router,
    private messageService: HelperAngular) {
  }
  contagemMudou: any;

  listaNomes = [
    {
      id: 1,
      titulo: CriteriosSelecaoFiltro.criterioGerais,
      linkMenu: 'criteriosGeraisGuia',
      selecionado: false,
      ativo: true,
      marcado: false,
      tituloPadrao: CriteriosSelecaoFiltro.criterioGerais
    },
    {
      id: 2,
      titulo: CriteriosSelecaoFiltro.numeroGuia,
      linkMenu: 'numeroGuiaGuia',
      selecionado: false,
      ativo: false,
      marcado: false,
      tituloPadrao: CriteriosSelecaoFiltro.numeroGuia
    },
    {
      id: 3,
      titulo: CriteriosSelecaoFiltro.numeroContaJudicial,
      linkMenu: 'numeroContaJudicialGuia',
      selecionado: false,
      ativo: false,
      marcado: false,
      tituloPadrao: CriteriosSelecaoFiltro.numeroContaJudicial
    },
    {
      id: 4,
      titulo: CriteriosSelecaoFiltro.numeroProcessoJE,
      linkMenu: 'numeroProcessoJECGuia',
      selecionado: false,
      ativo: false,
      marcado: false,
      tituloPadrao: CriteriosSelecaoFiltro.numeroProcessoJE
    },
    {
      id: 5,
      titulo: CriteriosSelecaoFiltro.numeroProcessoCC,
      linkMenu: 'numeroProcessoCCGuia',
      selecionado: false,
      ativo: false,
      marcado: false,
      tituloPadrao: CriteriosSelecaoFiltro.numeroProcessoCC
    },
    {
      id: 6,
      titulo: CriteriosSelecaoFiltro.numeroProcessoPex,
      linkMenu: 'numeroProcessoPexGuia',
      selecionado: false,
      ativo: false,
      marcado: false,
      tituloPadrao: CriteriosSelecaoFiltro.numeroProcessoPex
    },
  ];

  public currentCount = 0;

  limparDTO() {

    if (!this.manterDados) {
      this.iniciarSelecionado();
      this.consultaDTO = {
        numeroRemessaMenor: null,
        numeroRemessaMaior: null,
        numerosContasJudiciais: [],
        idsProcessosCC: null,
        idsProcessosPEX: null,
        idsProcessosJEC: null,
        idsNumerosGuia: null,
        dataRemessaMenor: null,
        dataRemessaMaior: null,
        valorGuiaInicio: null,
        valorGuiaFim: null,
        pagina: 1,
        quantidade: quantidadePaginainicial,
        total: 0
      };
      this.manterDados = false;
    }

  }





  get listaCriterioSelecao(): FiltroModel[] {
    const lista = this.listaNomes;
    return lista;
  }

  get contagemGuiaMudou$(): BehaviorSubject<number> {
    return this.numeroGuiaService.contagem;
  }

  get contagemCriteriosMudou$(): BehaviorSubject<number> {
    return this.criteriosGuiaService.contador;
  }

  get contagemContaJudicialMudou$(): BehaviorSubject<number> {
    return this.numeroContaJudicialService.contagemSubject;
  }

  get contagemProcessosCCMudou$(): BehaviorSubject<number> {
    return this.processoCCService.contagemSubject;
  }

  get contagemProcessosPexMudou$(): BehaviorSubject<number> {
    return this.processoPexService.contagemSubject;
  }
  get contagemProcessosJEC$(): BehaviorSubject<number> {
    return this.processoJecService.contagemSubject;
  }

  atualizarContagem(counter: number, id: number): void {
    if (!id) {
      this.listaCriterioSelecao.map(item => {
        item.titulo = item.tituloPadrao;
        item.selecionado = false;
      });
    } else {
      this.listaCriterioSelecao.map(item => {
        if (item.id === id) {
          item.selecionado = true;
          if (counter === 0) {
            item.titulo = item.tituloPadrao;
            item.selecionado = false;
          } else {
            item.titulo = item.tituloPadrao + ' (' + counter + ') ';
            item.selecionado = true;
          }
        }
      });
    }
  }


  enviarDados() {

    const getIdArray = array => array.map(e => e['id']);
    const getNumeroContaJudicialArray = array => array.map(e => e["Número da Conta Judicial"]);
    this.consultaDTO = {
      ...this.filtrosUtilizados$.value,
      numeroRemessaMenor: this.criteriosGuiaService.numeroRemessaInicio,
      numeroRemessaMaior: this.criteriosGuiaService.numeroRemessaFim,
      numerosContasJudiciais: getNumeroContaJudicialArray(this.numeroContaJudicialService.listaContasSubject.value),
      idsProcessosCC: getIdArray(this.processoCCService.listaProcessosSubject.value),
      idsProcessosPEX: getIdArray(this.processoPexService.listaProcessosSubject.value),
      idsProcessosJEC: getIdArray(this.processoJecService.listaProcessosSubject.value),
      idsNumerosGuia: this.numeroGuiaService.guiasSelecionadas.ToArray(),
      dataRemessaMenor: this.criteriosGuiaService.data.dataInicio ?
        formatDate(this.criteriosGuiaService.data.dataInicio, 'yyyy/MM/dd', 'pt_BR') : null,
      dataRemessaMaior: this.criteriosGuiaService.data.dataFim ?
        formatDate(this.criteriosGuiaService.data.dataFim, 'yyyy/MM/dd', 'pt_BR') : null,
      valorGuiaInicio: this.criteriosGuiaService.intervaloValoresGuiaInicio,
      valorGuiaFim: this.criteriosGuiaService.intervaloValoresGuiaFim
    };

    return this.consultaDTO;
  }

  filtrosUtilizados$ = new BehaviorSubject<ConsultaArquivoRetornoFiltroDTO>({
    pagina: 1,
    quantidade: 5,
    total: 0
  });




  consultarArquivoRetorno() {
    this.filtrosUtilizados$.next(this.enviarDados());
    this.service.consultarArquivoRetorno(this.enviarDados()).pipe(take(1))
      .subscribe(response => {
        const filtros = this.enviarDados();
        this.resultadoService.resultadoSubject.next(response['data']);
        filtros.total = response['total'];
        this.resultadoService.filtrosUtilizadosSubject$.next(filtros);
        if (response['sucesso'] && response['data'].length > 0) {
          this.router.navigate(['sap/interfaceBB/importacao/resultado'])
        } else if (response['sucesso'] && (response['data'].length == 0
          || !response.data)) {
          this.messageService.MsgBox2('Nenhum resultado encontrado', 'Atenção!', 'warning', 'Ok')
        }
        else {
          this.messageService.MsgBox2(response['mensagem'], 'Ops!', 'warning', 'Ok');
        }
      }
      );
  }

  upload(files: File[]): Observable<APIResponse> {
    const backEndURL = '/BBResumoProcessamento/Upload';
    return this.uploadService.uploadMultiFile(backEndURL, files);
  }

  iniciarSelecionado() {
    this.listaNomes.forEach(element => {

      element.ativo = false;
      if(element.id === 1){
        element.ativo = true;
      }


    });
  }
}

