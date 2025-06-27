import { FormControl } from '@angular/forms';
import { Comprovante } from './../../models/comprovante.model';
import { HttpErrorResult } from '@core/http/http-error-result';
import { isNullOrUndefined } from 'util';
import { DialogService } from './../../../shared/services/dialog.service';
import { InstrucoesCargaModalComponent } from './../instrucoes-carga-modal/instrucoes-carga-modal.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Component, OnInit, ViewChild, Output, EventEmitter, ElementRef } from '@angular/core';
import { CargaComprovantesService } from '../../services/carga-comprovantes.service';
import { TipoArquivo } from '../../models/tipo-arquivo.model';

@Component({
  // tslint:disable-next-line: component-selector
  selector: 'painel-carga-de-comprovantes',
  templateUrl: './painel-carga-de-comprovantes.component.html',
  styleUrls: ['./painel-carga-de-comprovantes.component.scss']
})
export class PainelCargaDeComprovantesComponent implements OnInit {

  @ViewChild('inputPdf', { static: false }) inputPdf: ElementRef;
  @ViewChild('inputXlsx', { static: false }) inputXlsx: ElementRef;

  nomeArquivoPdfFormControl: FormControl = new FormControl('');
  nomeArquivoXlsxFormControl: FormControl = new FormControl('');

  arquivoPdf: File;
  arquivoXlsx: File;

  codigoParametroTamanhoMaximo = 'TAM_MAX_CARGA_COMPROVANTE';
  tamanhoMaximoArquivosAnexos: number ;
  mensagemTamanhoMaximoArquivoAnexo: number;

  @Output() recarregarLista: EventEmitter<void> = new EventEmitter();

  constructor(private modalService: NgbModal, private dialog: DialogService, private service: CargaComprovantesService) { }

  async ngOnInit() {
    await this.obterTamanhoMaximoArquivoAnexo();
  }

  async obterTamanhoMaximoArquivoAnexo(): Promise<void> {
    try {
      const resultado = await this.service.obterTamanhoMaximoArquivoAnexo(this.codigoParametroTamanhoMaximo);
      this.tamanhoMaximoArquivosAnexos = parseFloat(((resultado.conteudo * 1024) * 1024).toFixed(2));
      this.mensagemTamanhoMaximoArquivoAnexo = resultado.conteudo;
    } catch (error) {
      // await this.dialog.showErr('Operação não realizada', (error as HttpErrorResult).messages.join('\n'));
    }
  }

  async abrirModal(): Promise<void> {
    try {
      await InstrucoesCargaModalComponent.exibeModal(this.mensagemTamanhoMaximoArquivoAnexo);
    } catch (error) {
      console.error(error);
    }
  }

  adicionarArquivoPdf(): void {
    const arquivo: File = this.inputPdf.nativeElement.files;

    if (this.obterExtensaoDoArquivo(arquivo[0].name) === '.pdf') {
      this.nomeArquivoPdfFormControl.setValue(arquivo[0].name);
    } else {
      this.dialog.showErr('Desculpe, o upload do arquivo não poderá ser realizado', 'O arquivo importado não é PDF.');
      return;
    }
    this.arquivoPdf = arquivo[0];
  }

  adicionarArquivoXlsx(): void {
    const arquivo: File = this.inputXlsx.nativeElement.files;
    if (this.obterExtensaoDoArquivo(arquivo[0].name) === '.xlsx') {
      this.nomeArquivoXlsxFormControl.setValue(arquivo[0].name);
    } else {
      this.dialog.showErr('Desculpe, o upload do arquivo não poderá ser realizado', 'O arquivo importado não é XLSX.');
      return;
    }
    this.arquivoXlsx = arquivo[0];
  }

  obterExtensaoDoArquivo(nomeArquivo): string {
    const indexExtensaoDoArquivo = nomeArquivo.lastIndexOf('.');
    return nomeArquivo.slice(indexExtensaoDoArquivo);
  }

  verificaArquivosSelecionados(): boolean {
    return isNullOrUndefined(this.arquivoPdf) || isNullOrUndefined(this.arquivoXlsx);
  }

  obterPaginado(): void {
    this.recarregarLista.emit();
  }

  limparCamposAoAgendar(): void {
    this.inputPdf.nativeElement.value = '';
    this.inputXlsx.nativeElement.value = '';

    this.arquivoPdf = undefined;
    this.arquivoXlsx = undefined;

    this.nomeArquivoPdfFormControl.setValue('');
    this.nomeArquivoXlsxFormControl.setValue('');
  }

  async criar(): Promise<void> {
    const tamanhoArquivosEhMaior = this.tamanhoMaximoArquivosAnexos < (this.arquivoXlsx.size + this.arquivoPdf.size);

    if (tamanhoArquivosEhMaior) {
      await this.dialog.showErr('Desculpe, o upload do arquivo não poderá ser realizado', `O tamanho dos arquivos importados ultrapassa o limite permitido de ${this.mensagemTamanhoMaximoArquivoAnexo}MB`);
      return;
    }
    if (isNullOrUndefined(this.arquivoPdf)) {
      await this.dialog.showErr('Desculpe, mas o arquivo PDF não foi preenchido', 'Nunhum arquivo PDF foi selecionado.');
      return;
    }
    if (isNullOrUndefined(this.arquivoXlsx)) {
      await this.dialog.showErr('Desculpe, mas o arquivo XLSX não foi preenchido', 'Nunhum arquivo XLSX foi selecionado.');
      return;
    }

    try {
      await this.service.criar({ arquivoComprovantes: this.arquivoPdf, arquivoBaseSAP: this.arquivoXlsx });
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
