import { Component, OnInit } from '@angular/core';
import { DialogService } from './../../../shared/services/dialog.service';
import { take } from 'rxjs/operators';
import { HttpErrorResult } from './../../../core/http/http-error-result';
import { HelperAngular } from '@shared/helpers/helper-angular';
import { PexPorMediaService } from 'src/app/core/services/relatorios/pex-por-media/pex-por-media.service';
import { NgbDateParserFormatter, NgbDateNativeAdapter, NgbDateAdapter, NgbDate, NgbCalendar, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { NgbDateCustomParserFormatter } from '@shared/dateformat';
import { FormControl } from '@angular/forms';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { Permissoes } from '@permissoes';


@Component({
  selector: 'app-pex-por-media',
  templateUrl: './pex-por-media.component.html',
  styleUrls: ['./pex-por-media.component.scss'],
  providers: [{ provide: NgbDateAdapter, useClass: NgbDateNativeAdapter }, { provide: NgbDateParserFormatter, useClass: NgbDateCustomParserFormatter }]
})
export class PexPorMediaComponent implements OnInit {

  public tituloPagina = 'Fechamentos de Contingência PEX por Média';
  public caminhoPagina = '';

  public listaFechamento = [];
  public total;
  public listaArquivosFechamentoDownload = [];
  public dataFechamento = null;
  public empresas = null;
  public numeroMeses = null;
  public multDesvioPadrao = null;
  public percentualHaircut = null;
  public indAplicarHaircut = null;
  public nomeUsuario = null;
  public dataExecucao = null;
  public vermais = false;
  public datainicio = null;
  public datafim = null
  public popupDownload = false;
  dataInicial: Date = new Date();
  public dataInicialFormControl: FormControl = new FormControl;
  public dataFinalFormControl: FormControl = new FormControl;

  validandoDataInicio = {
    data: null,
    erro: false,
  };
  validandoDataFim = {
    data: null,
    erro: false,
  };

  constructor(
    private servicefechamento: PexPorMediaService, 
    private dialog: DialogService,
    private breadcrumbsService: BreadcrumbsService) {
    this.dataInicial.setDate(this.dataInicial.getDate()-30);
    this.dataInicialFormControl = new FormControl(new Date(this.dataInicial));
    this.dataFinalFormControl = new FormControl(new Date());
  }


  ngOnInit() {
    this.buscarListaFechamentos();
  }

  async ngAfterViewInit(): Promise<void> {
    this.caminhoPagina = await this.breadcrumbsService.nomeBreadcrumb(Permissoes.ACESSAR_RELATORIO_CONTINGENCIA_PEX_MEDIA);
  }

  validaData(data: any) {
    if (data.data === null || typeof (data.data) === 'string') {
      data.erro = false;
    }
  }

  listarFechamentoDiario(datainicio, datafim) {
    this.servicefechamento.buscarporFiltro(this.servicefechamento.paginacaoVerMais.value['pagina'], 10, datainicio, datafim).subscribe(res => {
      this.listaFechamento = res.data;
      this.total = res.total;
      if (this.total == this.listaFechamento.length)
        this.vermais = false;
      else this.vermais = true;
    });
  }


  buscarListaFechamentos() {
    this.servicefechamento.limpar();
    this.servicefechamento.consultarfechamentos()
      .pipe(take(1))
      .subscribe(
        fechamentos => {
          this.listaFechamento = fechamentos;
          if (this.listaFechamento == null || this.servicefechamento.paginacaoVerMais.value['total'] == this.listaFechamento.length) {
            this.vermais = false;
          } else {this.vermais = true};
        }
      );
  }

  buscarFechamentoPorData(d1, d2) {
    this.servicefechamento.limpar();
    if (d1._model === null || d2._model === null) {
      this.buscarListaFechamentos();
    }
    else if ((!this.validandoDataInicio.erro && !this.validandoDataFim.erro)) {
      this.datainicio = d1._model.day + '/' + d1._model.month + '/' + d1._model.year;
      this.datafim = d2._model.day + '/' + d2._model.month + '/' + d2._model.year;
      this.listarFechamentoDiario(this.datainicio, this.datafim);
    } else {
      return;
    }
  }

  verMais(d1, d2) {
    this.datainicio = d1._model === null || d2._model === null ? null : d1._model.day + '/' + d1._model.month + '/' + d1._model.year;
    this.datafim = d1._model === null || d2._model === null ? null : d2._model.day + '/' + d2._model.month + '/' + d2._model.year;

    this.servicefechamento.consultarMais(this.datainicio, this.datafim).subscribe(
      fechamentos => {
        this.listaFechamento.push.apply(this.listaFechamento, fechamentos)
        if (this.servicefechamento.paginacaoVerMais.value['total'] == this.listaFechamento.length)
          this.vermais = false;
        else this.vermais = true;
      }
    );
  }

  download(fechamentoId: number, dataFechamento: Date, dataGeracao: Date) {
    try {
      this.servicefechamento.downloadZip(fechamentoId, new Date(dataFechamento), new Date(dataGeracao));
    } catch (error) {
      this.dialog.err('Não foi possível baixar as informações', (error as HttpErrorResult).messages.join('\n'));
    }
  }
}
