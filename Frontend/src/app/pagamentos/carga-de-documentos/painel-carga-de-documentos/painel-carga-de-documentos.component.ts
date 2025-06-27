import { HttpErrorResult } from '@core/http/http-error-result';
import { CargaDeDocumentosService } from './../../services/carga-de-documentos.service';
import { isNullOrUndefined } from 'util';
import { DialogService } from '@shared/services/dialog.service';
import { FormControl } from '@angular/forms';
import { InstrucoesCargaDocumentosModalComponent } from './../instrucoes-carga-documentos-modal/instrucoes-carga-documentos-modal.component';
import { Component, OnInit, ViewChild, ElementRef, EventEmitter, Output } from '@angular/core';
import { TipoArquivo } from '../../models/tipo-arquivo.model';

@Component({
  selector: 'painel-carga-de-documentos',
  templateUrl: './painel-carga-de-documentos.component.html',
  styleUrls: ['./painel-carga-de-documentos.component.scss']
})
export class PainelCargaDeDocumentosComponent implements OnInit {

  @ViewChild('arquivo', { static: false }) arquivo: ElementRef;

  nomeArquivoFormControl: FormControl = new FormControl('');

  codigoParametroTamanhoMaximo = 'TAM_MAX_CARGA_DOCUMENTOS';

  arquivoCsv: File;
  tamanhoMaximoArquivoAnexo: number ;
  mensagemTamanhoMaximoArquivoAnexo: number;

  @Output() recarregarLista: EventEmitter<void> = new EventEmitter();

  constructor(private dialog: DialogService, private service: CargaDeDocumentosService) { }

  async ngOnInit() {
    await this.obterTamanhoMaximoArquivoAnexo();
  }

  async obterTamanhoMaximoArquivoAnexo(): Promise<void> {
    try {
      const resultado = await this.service.obterTamanhoMaximoArquivoAnexo(this.codigoParametroTamanhoMaximo);
      this.tamanhoMaximoArquivoAnexo = parseFloat(((resultado.conteudo * 1024) * 1024).toFixed(2));
      this.mensagemTamanhoMaximoArquivoAnexo = resultado.conteudo;
    } catch (error) {
      console.error(error);
      await this.dialog.showErr('Operação não realizada', (error as HttpErrorResult).messages.join('\n'));
    }
  }

  async abrirModal(): Promise<void> {
    try {
      await InstrucoesCargaDocumentosModalComponent.exibeModal(this.mensagemTamanhoMaximoArquivoAnexo);
    } catch (error) {
      console.error(error);
    }
  }

  aoAdicionarArquivo() {
    const arquivo: File = this.arquivo.nativeElement.files;

    if (this.obterExtensaoDoArquivo(arquivo[0].name) === '.csv') {
      this.nomeArquivoFormControl.setValue(arquivo[0].name);
    } else {
      this.dialog.showErr('Desculpe, o upload do arquivo não poderá ser realizado', 'O arquivo importado não é CSV.');
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
      await this.dialog.showErr('Desculpe, o upload do arquivo não poderá ser realizado', `O tamanho do arquivo importado ultrapassa o limite permitido de ${this.mensagemTamanhoMaximoArquivoAnexo}MB`);
      return;
    }
    if (isNullOrUndefined(this.arquivoCsv)) {
      await this.dialog.showErr('Desculpe, mas o arquivo CSV não foi preenchido', 'Nunhum arquivo CSV foi selecionado.');
      return;
    }

    try {
      await this.service.criar(this.arquivoCsv);
      await this.obterPaginado();
      this.limparCamposAoAgendar();
    } catch (error) {
      console.error(error);
      await this.dialog.showErr('Operação não realizada', (error as HttpErrorResult).messages.join('\n'));
      return;
    }
  }

  downloadArquivoPadraoURL(): string {
    return this.service.downloadArquivoURL(TipoArquivo.ARQUIVO_PADRAO);
  }

}
