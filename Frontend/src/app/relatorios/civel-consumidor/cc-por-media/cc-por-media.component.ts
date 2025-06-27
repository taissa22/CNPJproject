import { enGbLocale } from 'ngx-bootstrap/locale';
import { DialogService } from './../../../shared/services/dialog.service';
import { NgbDateCustomParserFormatter } from './../../../shared/dateformat';
import { HttpErrorResult } from './../../../core/http/http-error-result';
import { CCPorMediaService } from './../../../core/services/relatorios/cc-por-media/cc-por-media.service';
import { FechamentoCCMediaModel } from './../../../core/models/fechamento-cc-media.model';
import { Component, OnInit } from '@angular/core';
import { NgbDateAdapter, NgbDateNativeAdapter, NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import { BsLocaleService, defineLocale } from 'ngx-bootstrap';
import { FormControl } from '@angular/forms';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { Permissoes } from '@permissoes';


@Component({
  selector: 'app-cc-por-media',
  templateUrl: './cc-por-media.component.html',
  styleUrls: ['./cc-por-media.component.scss'],
  providers: [{ provide: NgbDateAdapter, useClass: NgbDateNativeAdapter }, { provide: NgbDateParserFormatter, useClass: NgbDateCustomParserFormatter }]
})
export class CCPorMediaComponent implements OnInit {

  public tituloPagina = 'Fechamentos de Contingência Cível Consumidor por Média';
  public caminhoPagina = '';

  dataInicial: Date = new Date();
  pagina: number = 1;
  fechamentos: Array<FechamentoCCMediaModel> = [];
  totalDeRegistros: number = 0;
  dataInicialFormControl: FormControl;
  dataFinalFormControl: FormControl;

  constructor(
    private dialog: DialogService, 
    private servico: CCPorMediaService, 
    private configLocalizacao: BsLocaleService,
    private breadcrumbsService: BreadcrumbsService) {
    this.configLocalizacao.use('pt-BR');
    // enGbLocale.invalidDate = 'Data Inválida';
    // defineLocale('data-padrao-br', enGbLocale);
    // this.configLocalizacao.use('data-padrao-br');

    this.dataInicial.setDate(this.dataInicial.getDate()-30);
    this.dataInicialFormControl = new FormControl(new Date(this.dataInicial));
    this.dataFinalFormControl = new FormControl(new Date());
  }

  ngOnInit() {
    this.obter(false);
  }

  async ngAfterViewInit(): Promise<void> {
    this.caminhoPagina = await this.breadcrumbsService.nomeBreadcrumb(Permissoes.ACESSAR_RELATORIO_CONTINGENCIA_CC_MEDIA);
  }

  async obter(verMais: boolean): Promise<void> {
    if (this.dataInicialFormControl.value == null || this.dataFinalFormControl.value == null){
      return;
    }
    this.pagina = verMais ? this.pagina + 1 : 1

    try {
      const result = await this.servico
        .obterPaginado(this.dataInicialFormControl.value.toISOString(),
        this.dataFinalFormControl.value.toISOString(),
        this.pagina);

      if (verMais) {
        this.fechamentos.push(...result.data);
      }
      else{
        this.fechamentos = result.data;
      }
      this.totalDeRegistros = result.total;

    } catch (error) {
     this.dialog.err('Não foi possível carregar as informações', (error as HttpErrorResult).messages.join('\n'));
    }
  }

  download(id: number) {
    try {
      this.servico.download(id);
    } catch (error) {
      this.dialog.err('Não foi possível baixar as informações', (error as HttpErrorResult).messages.join('\n'));
    }
  }
}
