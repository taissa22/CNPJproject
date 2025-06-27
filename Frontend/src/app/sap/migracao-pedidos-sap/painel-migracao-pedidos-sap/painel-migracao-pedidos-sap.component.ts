import { ParametroJuridicoService } from './../../../core/services/parametroJuridico.service';
import { HttpErrorResult } from '@core/http/http-error-result';
import { isNullOrUndefined } from 'util';
import { DialogService } from '@shared/services/dialog.service';
import { FormControl } from '@angular/forms';
import { InstrucoesMigracaoPedidosSapComponent } from '../instrucoes-migracao-pedidos-sap/instrucoes-migracao-pedidos-sap.component';
import { Component, OnInit, ViewChild, ElementRef, EventEmitter, Output } from '@angular/core';
import { MigracaoPedidosSapComponent } from '../migracao-pedidos-sap.component';
//import { TipoArquivo } from '../../models/tipo-arquivo.model';
import { TipoArquivo } from '../../../pagamentos/models/tipo-arquivo.model';
import { MigracaoPedidosSapServiceService } from '../services/migracao-pedidos-sap-service.service';

@Component({
  selector: 'app-painel-migracao-pedidos-sap',
  templateUrl: './painel-migracao-pedidos-sap.component.html',
  styleUrls: ['./painel-migracao-pedidos-sap.component.scss']
})
export class PainelMigracaoPedidosSapComponent implements OnInit {

  @ViewChild('arquivo', { static: false }) arquivo: ElementRef;

  nomeArquivoFormControl: FormControl = new FormControl('');

  codigoParametroTamanhoMaximo = 'TAM_MAX_MIGRAR_PEDIDOS';

  arquivoCsv: File;
  tamanhoMaximoArquivoAnexo: number ;
  mensagemTamanhoMaximoArquivoAnexo: number;

  @Output() recarregarLista: EventEmitter<void> = new EventEmitter();

  constructor(
    private dialog: DialogService,
    private service: MigracaoPedidosSapServiceService,
    private parametroService: ParametroJuridicoService
    ) { }

  async ngOnInit() {
    await this.obterTamanhoMaximoArquivoAnexo();
  }

  async obterTamanhoMaximoArquivoAnexo(): Promise<void> {
    try {
      const resultado = await this.parametroService.obter(this.codigoParametroTamanhoMaximo);
      this.tamanhoMaximoArquivoAnexo = parseFloat(((resultado.conteudo * 1024) * 1024).toFixed(2));
      this.mensagemTamanhoMaximoArquivoAnexo = resultado.conteudo;
    } catch (error) {
      console.error(error);
      await this.dialog.err('Operação não realizada', (error as HttpErrorResult).messages.join('\n'));
    }
  }

  async abrirModal(): Promise<void> {
    try {
      await InstrucoesMigracaoPedidosSapComponent.exibeModal(this.mensagemTamanhoMaximoArquivoAnexo);
    } catch (error) {
      console.error(error);
    }
  }


  aoAdicionarArquivo() {
    const arquivo: File = this.arquivo.nativeElement.files;

    if (this.obterExtensaoDoArquivo(arquivo[0].name) === '.csv') {
      this.nomeArquivoFormControl.setValue(arquivo[0].name);
    } else {
      this.dialog.err('Desculpe, o upload do arquivo não poderá ser realizado', 'O arquivo importado não é CSV.');
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
    const tamanhoArquivosEhMaior = this.tamanhoMaximoArquivoAnexo < this.arquivoCsv.size;

    if (tamanhoArquivosEhMaior) {
      await this.dialog.err('Desculpe, o upload do arquivo não poderá ser realizado', `O arquivo CSV importado ultrapassa o limite permitido de ${this.mensagemTamanhoMaximoArquivoAnexo}MB.`);
      return;
    }
    if (isNullOrUndefined(this.arquivoCsv)) {
      await this.dialog.err('Desculpe, mas o arquivo CSV não foi preenchido', 'Nunhum arquivo CSV foi selecionado.');
      return;
    }

    try {
      await this.service.criar(this.arquivoCsv);
      this.obterPaginado();
      this.limparCamposAoAgendar();
    } catch (error) {
      console.error(error);
      await this.dialog.err('Desculpe, o upload do arquivo não poderá ser realizado', (error as HttpErrorResult).messages.join('\n'));
      return;
    }
  }

  downloadArquivoPadraoURL(): string {
    return this.service.downloadArquivoURL(TipoArquivo.ARQUIVO_PADRAO);
  }

}

