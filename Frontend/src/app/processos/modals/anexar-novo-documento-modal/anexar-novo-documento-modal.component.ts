import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ListarAnexosResponse } from '../../models/parametrizar-distribuicao-processos/listar-anexos-response';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { isNullOrUndefined } from 'util';
import { DialogService } from '@shared/services/dialog.service';
import { FormControl, FormGroup } from '@angular/forms';
import { ParametroJuridicoService } from '@core/services/parametroJuridico.service';
import { ParametrizarDistribuicaoProcessosService } from '../../services/parametrizar-distribuicao-processos.service';
import { StaticInjector } from '../../static-injector';

@Component({
  selector: 'app-anexar-novo-documento-modal',
  templateUrl: './anexar-novo-documento-modal.component.html',
  styleUrls: ['./anexar-novo-documento-modal.component.scss']
})
export class AnexarNovoDocumentoModalComponent implements OnInit {
  @ViewChild('arquivo', { static: false }) arquivo: ElementRef;

  constructor(
    private modal: NgbActiveModal,
    private dialogService: DialogService,
    private service: ParametrizarDistribuicaoProcessosService
  ) { }

  ngOnInit() {
    this.ParametroTamanhoMaximo = 10 * 1024 * 1024;
    this.ParametroTamanhoMaximoMsg = 10;
  }

  //#region VARIAVEIS

  codParamDistribuicao: number = 0;
  anexo: ListarAnexosResponse
  qtdComentario = 0

  arquivoZip: File = null;
  ParametroTamanhoMaximo: number;
  ParametroTamanhoMaximoMsg: number;

  comentarioFormControl: FormControl = new FormControl(null);
  nomeArquivoFormControl: FormControl = new FormControl(null);
  formGroup: FormGroup = new FormGroup({
    comentario: this.comentarioFormControl,
    arquivo: this.nomeArquivoFormControl
  });
  //#endregion

  static async exibeModalAnexarNovoDocumento(codParamDistribuicao: number): Promise<ListarAnexosResponse> {
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(AnexarNovoDocumentoModalComponent, { centered: true, backdrop: 'static', size: 'lg', backdropClass: 'modal-backdrop-novo-anexo' });

    modalRef.componentInstance.codParamDistribuicao = codParamDistribuicao;

    return await modalRef.result;
  }

  //#region FUNÇÕES

  aoAdicionarArquivo() {
    const arquivo: File = this.arquivo.nativeElement.files[0];

    if (!arquivo) return;

    if (this.obterExtensaoDoArquivo(arquivo.name) !== '.zip') {
      return this.dialogService.err('Importação não pode ser realizada!', 'O arquivo selecionado não é um arquivo .zip.');
    }

    if (arquivo.size > this.ParametroTamanhoMaximo) {
      return this.dialogService.err('Importação não pode ser realizada!', `O arquivo selecionado ultrapassa o limite permitido de ${this.ParametroTamanhoMaximoMsg}MB.`);
    }

    this.nomeArquivoFormControl.setValue(arquivo.name);
    this.arquivoZip = arquivo;
  }

  validarArquivoSelecionado(arquivo: File): boolean {
    var validado = true;
    if (arquivo[0].size <= 17) validado = false;
    else if (isNullOrUndefined(arquivo)) validado = false;
    return validado;
  }

  obterExtensaoDoArquivo(nomeArquivo): string {
    const indexExtensaoDoArquivo = nomeArquivo.lastIndexOf('.');
    return nomeArquivo.slice(indexExtensaoDoArquivo);
  }

  contador() {
    this.qtdComentario = this.comentarioFormControl.value.length
  }

  close(): void {
    this.modal.close(false);
  }

  //#endregion

  //#region MÉTODOS

  async incluirAnexo() {
    try {
      const res = await this.service.incluirAnexoAsync(this.codParamDistribuicao, this.comentarioFormControl.value, this.arquivoZip);
      await this.dialogService.alert("Inclusão realizada com sucesso!", "O arquivo foi anexado com sucesso!")
      this.anexo = res;
      this.modal.close(this.anexo);
    } catch (error) {
      return this.dialogService.err("Erro", error.error)
    }
  }

  //#endregion

}
