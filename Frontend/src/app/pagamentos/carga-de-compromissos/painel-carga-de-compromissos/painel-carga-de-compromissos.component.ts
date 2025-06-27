import { HttpErrorResult } from '@core/http/http-error-result';
import { isNullOrUndefined } from 'util';
import { DialogService } from '@shared/services/dialog.service';
import { FormControl, FormGroup } from '@angular/forms';
import {
  Component,
  OnInit,
  ViewChild,
  ElementRef,
  EventEmitter,
  Output
} from '@angular/core';
import { TipoArquivo } from '../../models/tipo-arquivo.model';
import { TiposProcesso } from '@manutencao/models/tipos-de-processos';
import { TipoDeAudienciaService } from '@manutencao/services/tipo-de-audiencia.service';
import { CargaDeCompromissosService } from '../../services/carga-de-compromissos.service';
import { CompromissoRequest } from '../../models/request.compromisso';
import { DatePipe } from '@angular/common';
import { formatarData } from '@shared/utils';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'painel-carga-de-compromissos',
  templateUrl: './painel-carga-de-compromissos.component.html',
  styleUrls: ['./painel-carga-de-compromissos.component.scss']
})
export class PainelCargaDeCompromissosComponent implements OnInit {
  private datePipe: DatePipe;
  @ViewChild('arquivo', { static: false }) arquivo: ElementRef;

  tiposDeProcesso: TiposProcesso[] = [];
  codigoParametroTamanhoMaximo = 'TAM_MAX_CARGA_DOCUMENTOS';

  arquivoCsv: File;
  tamanhoMaximoArquivoAnexo: number;
  mensagemTamanhoMaximoArquivoAnexo: number;
  dataAtual = new Date(
    new Date().getFullYear(),
    new Date().getMonth(),
    new Date().getDate()
  );

  periodicidadeExecucaoFormControl: FormControl = new FormControl(0);
  dataEspecificaFormControl: FormControl = new FormControl(
    new Date(this.dataAtual)
  );
  tipoDeProcessoFormControl: FormControl = new FormControl();
  nomeArquivoFormControl: FormControl = new FormControl('');

  CCForm = new FormGroup({
    tipoProcesso: this.tipoDeProcessoFormControl,
    configExec: this.periodicidadeExecucaoFormControl,
    nomArquivoBase: this.nomeArquivoFormControl,
    datAgendamento: this.dataEspecificaFormControl
  });

  @Output() recarregarLista: EventEmitter<void> = new EventEmitter();

  arquivoSelecionado: boolean = false;
  tipoDeProcessoSelecionado: boolean = false;

  constructor(
    private dialog: DialogService,
    private service: CargaDeCompromissosService,
    private serviceTipoDeAudiencia: TipoDeAudienciaService,
    public activeModal: NgbActiveModal
  ) {}

  aoSelecionarTipoDeProcesso(event: any): void {
    if (event) {
      this.tipoDeProcessoSelecionado = true;
    }
  }

  async ngOnInit() {
    await this.obterTamanhoMaximoArquivoAnexo();
  }

  async ngAfterViewInit(): Promise<void> {
    const tpsProcessos = await this.serviceTipoDeAudiencia.getTiposDeProcesso();

    this.tiposDeProcesso = tpsProcessos.filter((processo: any) => {
      const allowedIds = [
        'Cível Consumidor',
        'Cível Estratégico',
        'Juizado Especial Cível',
        'Pex'
      ];
      return allowedIds.includes(processo.descricao);
    });
  }

  async obterTamanhoMaximoArquivoAnexo(): Promise<void> {
    try {
      const resultado = await this.service.obterTamanhoMaximoArquivoAnexo(
        this.codigoParametroTamanhoMaximo
      );
      this.tamanhoMaximoArquivoAnexo = parseFloat(
        (resultado.conteudo * 1024 * 1024).toFixed(2)
      );
      this.mensagemTamanhoMaximoArquivoAnexo = resultado.conteudo;
    } catch (error) {
      console.error(error);
      await this.dialog.showErr(
        'Operação não realizada',
        (error as HttpErrorResult).messages.join('\n')
      );
    }
  }

  aoAdicionarArquivo() {
    const arquivo: File = this.arquivo.nativeElement.files;

    if (this.obterExtensaoDoArquivo(arquivo[0].name) === '.csv') {
      if (arquivo) {
        this.arquivoSelecionado = true;
      }
      this.nomeArquivoFormControl.setValue(arquivo[0].name);
    } else {
      this.dialog.showErr(
        'Desculpe, o upload do arquivo não poderá ser realizado',
        'O arquivo importado não é CSV.'
      );
      return;
    }
    this.arquivoCsv = arquivo[0];
  }

  obterExtensaoDoArquivo(nomeArquivo): string {
    const indexExtensaoDoArquivo = nomeArquivo.lastIndexOf('.');
    return nomeArquivo.slice(indexExtensaoDoArquivo);
  }

  verificaArquivosSelecionados(): boolean {
    return isNullOrUndefined(this.arquivo);
  }

  obterPaginado(): void {
    this.recarregarLista.emit();
  }

  limparCamposAoAgendar(): void {
    this.arquivo.nativeElement.value = '';
    this.arquivoCsv = undefined;
    this.nomeArquivoFormControl.setValue('');
  }

  async criar(): Promise<void> {
    const tamanhoArquivosEhMaior =
      this.tamanhoMaximoArquivoAnexo < this.arquivoCsv.size;

    if (tamanhoArquivosEhMaior) {
      await this.dialog.showErr(
        'Desculpe, o upload do arquivo não poderá ser realizado',
        `O tamanho do arquivo importado ultrapassa o limite permitido de ${this.mensagemTamanhoMaximoArquivoAnexo}MB`
      );
      return;
    }
    if (isNullOrUndefined(this.arquivoCsv)) {
      await this.dialog.showErr(
        'Desculpe, mas o arquivo CSV não foi preenchido',
        'Nunhum arquivo CSV foi selecionado.'
      );
      return;
    }

    try {
      const request = new CompromissoRequest();
      request.tipoProcesso = this.tipoDeProcessoFormControl.value;
      request.configExec = this.periodicidadeExecucaoFormControl.value;
      request.nomArquivoBase = this.nomeArquivoFormControl.value;
      request.datAgendamento = formatarData(
        this.dataEspecificaFormControl.value
      );

      await this.service.criar(this.arquivoCsv, request);
      await this.obterPaginado();
      this.limparCamposAoAgendar();
    } catch (error) {
      console.error(error);
      await this.dialog.showErr(
        'Operação não realizada',
        (error as HttpErrorResult).messages.join('\n')
      );
      return;
    }
  }

  // downloadArquivoPadraoURL(): string {
  //   return this.service.downloadArquivoURL(TipoArquivo.ARQUIVO_PADRAO);
  // }
}
