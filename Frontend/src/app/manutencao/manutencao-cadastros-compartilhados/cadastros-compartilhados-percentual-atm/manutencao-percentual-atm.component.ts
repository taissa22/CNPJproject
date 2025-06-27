import { PercentualAtmDTO } from '@shared/interfaces/percentual-atm-dto';
import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { PercentualAtmService } from '@manutencao/services/percentual-atm.service';
import { DialogService } from '@shared/services/dialog.service';
import { ColunaGenerica } from '@manutencao/models/coluna-generica.model';
import { HttpErrorResult } from '@core/http';
import { ModalImportarPercentualAtmComponent } from './modal-importar-percentual-atm/modal-importar-percentual-atm.component';
import { FormControl } from '@angular/forms';
import { BsLocaleService } from 'ngx-bootstrap';
import { Permissoes, PermissoesService } from '@permissoes';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';

@Component({
  selector: 'app-manutencao-percentual-atm',
  templateUrl: './manutencao-percentual-atm.component.html',
  styleUrls: ['./manutencao-percentual-atm.component.scss']
})
export class ManutencaoPercentualAtmComponent implements OnInit {
  colunas: Array<ColunaGenerica> = [
    new ColunaGenerica('UF', 'uf', true, '30%', 'estadoId'),
    new ColunaGenerica('% ATM ', 'percentual', true, '70%', 'percentual')
  ];

  pagina: number = 1;
  totalDeRegistrosPorPagina: number = 8;
  ordenacaoColuna: 'uf' | 'percentual' | '' = 'uf';
  ordenacaoDirecao: 'asc' | 'desc';
  percentuaisAtm: Array<PercentualAtmDTO> = [];
  registros: Array<PercentualAtmDTO> = [];
  totalDeRegistros: number = 0;
  dataVigenciaFormControl: FormControl = new FormControl(new Date());
  datasVigencia: Date[] = [];
  dataIncluida: Date = null;
  tipoProcesso: TipoProcesso | undefined = undefined;
  private tiposProcessosDisponiveis: Array<TipoProcesso> = [
    {
      id: 1,
      descricao: 'Cível Consumidor',
      permissao: 'f_PercentualAtmCC',
      exportName: 'CC'
    },
    {
      id: 7,
      descricao: 'Juizado Especial',
      permissao: 'f_PercentualAtmJEC',
      exportName: 'JEC'
    },
    {
      id: 18,
      descricao: 'PEX',
      permissao: 'f_PercentualAtmPEX',
      exportName: 'PEX'
    }
  ];
  tiposProcesso: Array<TipoProcesso> = [];
  breadcrumb: string;

  constructor(
    private service: PercentualAtmService,
    private dialog: DialogService,
    private localeService: BsLocaleService,
    private permisoesService: PermissoesService,
    private changeDetectorRef: ChangeDetectorRef,
    private breadcrumbsService: BreadcrumbsService
  ) {
    this.localeService.use('pt-BR');
  }

  ngOnInit() {
    this.tiposProcessosDisponiveis.forEach(x => {
      if (this.permisoesService.temPermissaoPara(x.permissao)) {
        this.tiposProcesso.push(x);
      }
    });
    
    this.obter();
  }

  async ngAfterViewInit(): Promise<void> {
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(Permissoes.ACESSAR_PERCENTUAL_ATM);
  }

  get obterResultados(): Array<object> {
    const view = [];
    if (this.registros.length > 0) {
      this.registros.forEach((p: PercentualAtmDTO) => {
        view.push({
          uf: p.estadoId + ' - ' + p.nomeEstado,
          percentual: p.percentual.toFixed(2).toString().replace('.', ',')
        });
      });
    }
    return view;
  }

  async alteraTipoProcesso(tipo: TipoProcesso) { console.log(tipo);
    this.tipoProcesso = tipo;
    this.carregarInformacoes();
  }

  async carregarInformacoes() {
    await this.carregarComboVigencias(this.tipoProcesso.id);
    this.obter();
  }

  async obter(): Promise<void> {
    try {
      const result = await this.service.obterPaginado(
        this.pagina,
        this.totalDeRegistrosPorPagina,
        this.formataDataServidor(this.dataVigenciaFormControl.value),
        this.tipoProcesso.id,
        this.ordenacaoColuna,
        this.ordenacaoDirecao
      );
      this.totalDeRegistros = result.total;
      this.registros = result.data;
    } catch (error) {
      this.dialog.err(
        'Não foi possível carregar as informações',
        (error as HttpErrorResult).messages.join('\n')
      );
    }
  }

  formataDataServidor(data: Date) {
    return data.toISOString().substr(0, 10);
  }

  async carregarComboVigencias(codTipoProcesso: number): Promise<void> {
    try {
      const result = await this.service.ObterComboVigencias(codTipoProcesso);
      await (this.datasVigencia = result);
      if (this.datasVigencia.length > 0) {
        if (this.dataIncluida === null) {
          this.dataVigenciaFormControl = new FormControl(this.datasVigencia[0]);
        } else {
          for (const data of this.datasVigencia) {
            if (data.toString() === this.dataIncluida.toString()){
              this.dataVigenciaFormControl = new FormControl(data);
            }
          }
          this.dataIncluida = null;
        }
      } else {
        this.dataVigenciaFormControl = new FormControl(new Date());
      }
    } catch (error) {
      console.error(error);
      this.dialog.err(
        'Não foi possível carregar as informações',
        (error as HttpErrorResult).messages.join('\n')
      );
    }
  }

  async exportar(): Promise<void> {
    try {
      await this.service.exportar(
        this.ordenacaoColuna,
        this.formataDataServidor(this.dataVigenciaFormControl.value),
        this.tipoProcesso.id,
        this.tipoProcesso.exportName,
        this.ordenacaoDirecao
      );
    } catch (error) {
      console.error(error);
      this.dialog.err(
        'Não foi possível exportar as informações',
        (error as HttpErrorResult).messages.join('\n')
      );
    }
  }

  async abrirModalUpload(): Promise<void> {
    try {
      let modalRef = ModalImportarPercentualAtmComponent.exibeModal(
        this.tipoProcesso.id
      );
      modalRef.componentInstance.enviarData.subscribe(res => {
        this.dataIncluida = new Date(res);
        this.carregarInformacoes();
      });
    } catch (error) {
      console.error(error);
    }
  }
}
declare interface TipoProcesso {
  descricao: string;
  id: number;
  permissao: string;
  exportName: string;
}
