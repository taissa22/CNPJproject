import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { ErrorLib } from '@esocial/libs/error-lib';
import { ContribuicaoService } from '@esocial/services/formulario/subgrupos/contribuicao.service';
import { ImpostoService } from '@esocial/services/formulario/subgrupos/imposto.service';
import { PeriodoBaseService } from '@esocial/services/formulario/subgrupos/periodo-base.service';
import { PeriodoService } from '@esocial/services/formulario/subgrupos/periodo.service';
import { RemuneracaoService } from '@esocial/services/formulario/subgrupos/remuneracao.service';
import { StaticInjector } from '@esocial/static-injector';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DialogService } from '@shared/services/dialog.service';

@Component({
  selector: 'app-carregar-bloco-v1-2',
  templateUrl: './carregar-bloco_v1_2.component.html',
  styleUrls: ['./carregar-bloco_v1_2.component.scss']
})
export class CarregarBloco_v1_2_Component implements OnInit {
  @ViewChild('arquivo', { static: false }) arquivo: ElementRef;

  constructor(
    private dialogService: DialogService,
    private modal: NgbActiveModal,
    private periodoService: PeriodoService,
    private periodoBaseService: PeriodoBaseService,
    private calcContribService: ContribuicaoService,
    private impostoService: ImpostoService,
    private remuneracaoService: RemuneracaoService
  ) { }

  ngOnInit() {
    this.ParametroTamanhoMaximo = 10 * 1024 * 1024;
    this.ParametroTamanhoMaximoMsg = 10;
  }

  idFormulario: number;
  idContent: number;
  typeForm: string;
  arquivocsv: File = null;
  ParametroTamanhoMaximo: number;
  ParametroTamanhoMaximoMsg: number;

  nomeArquivoFormControl: FormControl = new FormControl(null);
  opcaoCargaFormControl: FormControl = new FormControl('1');
  tipoCargaFormControl: FormControl = new FormControl('1');
  formGroup: FormGroup = new FormGroup({
    arquivo: this.nomeArquivoFormControl,
    opcaoCarga: this.opcaoCargaFormControl
  });

  static exibeModal(idFormulario: number, typeForm: string, idContent?: number): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      CarregarBloco_v1_2_Component,
      {
        windowClass: 'modal-carregar-bloco',
        centered: true,
        size: 'md',
        backdrop: 'static'
      }
    );
    modalRef.componentInstance.idFormulario = idFormulario;
    modalRef.componentInstance.idContent = idContent;
    modalRef.componentInstance.typeForm = typeForm;

    return modalRef.result;
  }

  aoAdicionarArquivo() {
    const arquivo: File = this.arquivo.nativeElement.files[0];

    if (!arquivo) return;

    if (this.obterExtensaoDoArquivo(arquivo.name) !== '.csv') {
      return this.dialogService.err('A carga não poderá ser realizada!', 'O arquivo selecionado não é um arquivo .csv.');
    }

    if (arquivo.size > this.ParametroTamanhoMaximo) {
      return this.dialogService.err('A carga não poderá ser realizada!', `O arquivo selecionado ultrapassa o limite permitido de ${this.ParametroTamanhoMaximoMsg}MB.`);
    }

    this.nomeArquivoFormControl.setValue(arquivo.name);
    this.arquivocsv = arquivo;
  }

  async incluirAnexo() {
    let salvar = true;
    this.validaTipoPlanilha();
    if (this.opcaoCargaFormControl.value == '2') {
      const carga = await this.dialogService.confirm('Carga em Bloco', 'Todos os registros existentes serão apagados e os novos serão carregados. Deseja continuar com a carga?')
      if (!carga) salvar = false;
    }

    if (salvar) {
      try {
        const res = this.typeForm == "periodo" ?
          await this.periodoService.incluirCarregarBlocoAsync(this.idFormulario, this.idContent, this.arquivocsv, this.opcaoCargaFormControl.value) :
          this.typeForm == "periodo-base" ?
            await this.periodoBaseService.incluirCarregarBlocoAsync(this.idFormulario, this.arquivocsv, this.opcaoCargaFormControl.value) :
            this.typeForm == "info-cr-irrf" ?
              await this.impostoService.incluirCarregarBlocoAsync(this.idFormulario, this.arquivocsv, this.opcaoCargaFormControl.value) :
              this.typeForm == "remuneracao" ?
                await this.remuneracaoService.incluirCarregarBlocoAsync(this.idFormulario, this.idContent, this.arquivocsv, this.opcaoCargaFormControl.value) :
                await this.calcContribService.incluirCarregarBlocoAsync(this.idFormulario, this.arquivocsv, this.opcaoCargaFormControl.value, this.tipoCargaFormControl.value);

        if (res) {
          await this.dialogService.alert("Carga realizada com sucesso!", "O arquivo foi anexado com sucesso!")
        } else {
          await this.dialogService.err("A carga não poderá ser realizada!", "Falha ao anexar arquivo, tente novamente mais tarde.")
        }
        return this.modal.close(res);
      } catch (error) {
        const menssage = ErrorLib.ConverteMensagemErroImportacao(error);
        await this.dialogService.err("A carga não poderá ser realizada!", menssage);
        return this.modal.close(false);
      }
    }

  }

  obterExtensaoDoArquivo(nomeArquivo): string {
    const indexExtensaoDoArquivo = nomeArquivo.lastIndexOf('.');
    return nomeArquivo.slice(indexExtensaoDoArquivo);
  }

  close(): void {
    this.modal.close(false);
  }

  validaTipoPlanilha(){
    if (this.tipoCargaFormControl.value == '2') {
      this.opcaoCargaFormControl.setValue("2");
      this.opcaoCargaFormControl.updateValueAndValidity();
    }
  }

  async teste() {
    console.log(this.opcaoCargaFormControl.value)
    if (this.opcaoCargaFormControl.value == '2') {
      const carga = await this.dialogService.confirm('Carga em Bloco', 'Todos os registros existentes serão apagados e os novos serão carregados. Deseja continuar com a carga?')
      if (!carga) {
        this.modal.close();
      }
    }
  }

}
