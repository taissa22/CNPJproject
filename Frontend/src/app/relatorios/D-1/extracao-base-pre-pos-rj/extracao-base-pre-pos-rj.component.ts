import { Component, OnInit, EventEmitter, Injectable, Output } from '@angular/core';

import { take } from 'rxjs/operators';
import { NgbDateCustomParserFormatter } from '@shared/dateformat';
import { ExtracaoBasePrePosRjService } from 'src/app/core/services/relatorios/extracao-base-pre-pos-rj/extracao-base-pre-pos-rj.service';
import { HelperAngular } from '@shared/helpers/helper-angular';
import { DownloadTiposProcesso } from '../../models/extracao-download-tipos-processos';
import { NgbDateParserFormatter, NgbDateNativeAdapter, NgbDateAdapter } from '@ng-bootstrap/ng-bootstrap';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { Permissoes } from '@permissoes';

@Component({
  selector: 'app-extracao-base-pre-pos-rj',
  templateUrl: './extracao-base-pre-pos-rj.component.html',
  styleUrls: ['./extracao-base-pre-pos-rj.component.scss'],
  providers: [{ provide: NgbDateAdapter, useClass: NgbDateNativeAdapter }, { provide: NgbDateParserFormatter, useClass: NgbDateCustomParserFormatter }]
})

export class ExtracaoBasePrePosRjComponent implements OnInit {

  public tituloPagina = 'Extração da base Pré/Pós RJ';
  public caminhoPagina = '';

  public listaExtracoesDiarias = [];
  public listaArquivosDownload = [];
  public listaTiposProcessos = DownloadTiposProcesso;
  public dataExtracao = null;
  public vermais = false;
  public popupDownload = false;
  public marcarTodosCheckbox = true;
  public naoExpurga: boolean;
  public idExtracao: number;

  validandoData = {
    data: null,
    erro: false,
  };

  constructor(
    private service: ExtracaoBasePrePosRjService,
    private messageService: HelperAngular,
    private breadcrumbsService: BreadcrumbsService
  ) { }

  ngOnInit() {
    this.buscarListaExtrações();
    this.marcarTodosCheckbox = true;

    setInterval(() => {
      if (this.validandoData.data != null) {
        this.validaData(this.validandoData, null);
      }
      if (this.validandoData.data == null) {
        this.validandoData.erro = false;
      }
    }, 2200);
  }

  async ngAfterViewInit(): Promise<void> {
    this.caminhoPagina = await this.breadcrumbsService.nomeBreadcrumb(Permissoes.ACESSAR_EXTRACAO_BASE_PRE_POS);
  }

  validaData(data: any, c2) {
    if(data.data === null || typeof (data.data) === 'string'){
      data.erro = false;
    }
    if (typeof (data.data) === 'object' && data.data != null) {
      let hoje = new Date();
      if(data.data.getFullYear() != hoje.getFullYear()){
        data.erro = true;
      }else {
        data.erro = false;
      }
    }
    if(c2 === 'INVALID'){
      data.erro = true;
    }
    console.log(data.erro)
  }

  buscarExtracaoPorData(event, c2) {
    if (event._model === null) {
      this.buscarListaExtrações();
    }
    else if(!this.validandoData.erro && c2 === 'VALID'){
        this.dataExtracao = event._model.year + '/' + event._model.month + '/' + event._model.day;
        this.listarExtracoesDiarias(this.dataExtracao);
    }else{
      return;
    }
  }

  listarExtracoesDiarias(dt) {
    this.service.listaExtracoesDiarias(1, 5, dt).subscribe(res => {
      this.listaExtracoesDiarias = res.data;
      if (this.listaExtracoesDiarias.length <= 5)
        this.vermais = false;
      else this.vermais = true;

      if (this.listaExtracoesDiarias.length === 0) {
        this.messageService.MsgBox2('Não existe base Pré/Pós RJ gerada para a data informada.', 'Nenhum resultado encontrado', 'warning', 'Ok');
        // (document.getElementById('inputData') as HTMLInputElement).value = "";
        // this.buscarListaExtrações();
      }
    });
  }

  buscarListaExtrações() {
    this.service.limpar();
    this.service.consultar()
      .pipe(take(1))
      .subscribe(
        agendamentos => {
          this.listaExtracoesDiarias = agendamentos
          if (this.service.paginacaoVerMais.value['total'] == this.listaExtracoesDiarias.length)
            this.vermais = false;
          else this.vermais = true;
        }
      );
  }

  comportamentoCheckboxMarcarTodos(checkbox) {
    if (this.marcarTodosCheckbox) {
      this.listaTiposProcessos.forEach(e => {
        e.checkbox = true;
      });
    } else {
      this.listaTiposProcessos.forEach(e => {
        e.checkbox = false;
      });
    }
  }

  comportamentoCheckbox(checkbox) {
    let count = 0;
    if (!checkbox && this.marcarTodosCheckbox) {
      this.marcarTodosCheckbox = false;
    }
    else {
      this.listaTiposProcessos.forEach(e => {
        if (e.checkbox) {
          count += 1;
          if (count === 7) {
            this.marcarTodosCheckbox = true;
          }
        }
      });
    }
  }

  naoExpurgaExtracao(idExtracao, naoExpurgar) {
    this.idExtracao = idExtracao;
    this.service.naoExpurgarExtracao(this.idExtracao, naoExpurgar).subscribe(res => {
    });
  }

  verMais() {
    this.service.consultarMais().subscribe(
      agendamentos => {
        this.listaExtracoesDiarias.push.apply(this.listaExtracoesDiarias, agendamentos)
        if (this.service.paginacaoVerMais.value['total'] == this.listaExtracoesDiarias.length)
          this.vermais = false;
        else this.vermais = true;
      }
    );
  }

  abrirPopupDownload(idExtracao: number) {
    this.popupDownload = true;
    this.idExtracao = idExtracao;
  }

  fecharPopupDownload() {
    this.marcarTodosCheckbox = true;
    this.comportamentoCheckboxMarcarTodos(this.marcarTodosCheckbox);
    this.popupDownload = false;
  }

  fazerDownload() {
    this.listaTiposProcessos.forEach(e => {
      if (e.checkbox)
        this.listaArquivosDownload.push(e.tipo);
    });
    this.downloadZip();
  }

  downloadZip() {
    this.service.downloadZip(this.idExtracao, this.listaArquivosDownload).subscribe(res => {
      let buffer = this.converterBase64ParaBuffer(res.data.arquivoZip);
      this.prepararDownload(buffer, res.data.nomeArquivoZip);
    });
    this.listaArquivosDownload = [];
  }

  prepararDownload(buffer, nomeDoArquivo) {
    const blob = new Blob([buffer]);

    if (navigator.msSaveBlob) {
      navigator.msSaveBlob(blob, nomeDoArquivo);
      return;
    }

    const link = document.createElement('a');

    if (link.download != undefined) {
      link.setAttribute('href', URL.createObjectURL(blob));
      link.setAttribute('download', nomeDoArquivo);
      link.style.visibility = 'hidden';
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
    }
  }

  converterBase64ParaBuffer(base64) {
    let binaryString = window.atob(base64);
    const bytes = new Uint8Array(binaryString.length);

    for (var indice = 0; indice < bytes.length; indice++) {
      bytes[indice] = binaryString.charCodeAt(indice);
    }
    return bytes;
  }
}
