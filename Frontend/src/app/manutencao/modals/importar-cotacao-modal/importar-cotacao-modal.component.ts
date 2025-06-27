// angular
import { AfterViewInit, Component, ViewChild, ElementRef, OnInit } from '@angular/core';
import { FormControl, FormGroup, FormBuilder, Validators, AbstractControl } from '@angular/forms';

// 3rd party
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';

// core & shared
import { StaticInjector } from '@manutencao/static-injector';
import { DialogService } from '@shared/services/dialog.service';

// local
import { Cotacao } from '@manutencao/models/cotacao.model';
import { CotacaoServiceMock } from '@manutencao/services/cotacao.service.mock';
import { CotacaoService } from '@manutencao/services/cotacao.service';
import { isNullOrUndefined } from 'util';
import { ParametroJuridicoService } from '@core/services/parametroJuridico.service';
import { CotacaoIndiceTrabalhistaService } from '@manutencao/services/cotacao-indice-trabalhista.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-importar-cotacao-modal',
  templateUrl: './importar-cotacao-modal.component.html',
  styleUrls: ['./importar-cotacao-modal.component.scss'],
  providers: [{ provide: CotacaoServiceMock, useClass: CotacaoServiceMock }]

})
export class ImportarCotacaoModalComponent implements AfterViewInit, OnInit {
  @ViewChild('arquivo', { static: false }) arquivo: ElementRef;


  cotacao: Cotacao;
  dataCotacao: Date;
  arquivoLido: string = '';
  indice: { id: number, descricao: string, codigoValorIndice: string };
  indiceColuna: string = '';
  ParametroTamanhoMaximo: number = 0;
  ParametroTamanhoMaximoMsg: number = 0;
  constructor(
    private modal: NgbActiveModal,
    private dialogService: DialogService,
    private parametroService: ParametroJuridicoService,
    private serviceCotacaoIndiceTrabalhista: CotacaoIndiceTrabalhistaService,

  ) { }

  today: Date = new Date();
  arquivoCsv: File = null;
  dataSelecionadaFormControl: FormControl = new FormControl(this.today,[Validators.required]);
  nomeArquivoFormControl: FormControl = new FormControl(null,[Validators.required]);
  formGroup: FormGroup = new FormGroup({
    dataSelecionada: this.dataSelecionadaFormControl,
    arquivo: this.nomeArquivoFormControl
  });

  async ngOnInit() {
    await this.parametroService.obter('TAM_MAX_ARQ_IMP_COTA_TRAB').then((response) => {
      if (response && response.conteudo > 0) {
        this.ParametroTamanhoMaximo = parseFloat(((response.conteudo * 1024) * 1024).toFixed(2));
        this.ParametroTamanhoMaximoMsg = parseInt(response.conteudo, 10)
      }
    });
  }
  save() {
    this.today = this.dataSelecionadaFormControl.value;

    this.serviceCotacaoIndiceTrabalhista.
      criarImportacao(this.arquivoCsv, this.today.toDateString())
      .then(e => {
        if (e['dados'] !== '') {
          this.dialogService.err(
            'Importação não pode ser realizada!',
            e['dados'].toString()
          );
          this.nomeArquivoFormControl.setValue('');
          this.formGroup.pristine;
          this.arquivo.nativeElement.value = ''
          return;
        } else {
          this.close();
          
          window.location.href = "/Juridico/n/#/manutencao/cotacao/resultado-importacao-cotacao"
        }
      }).catch(e => {
        this.dialogService.err('Não foi possível criar a importação', `Erro: ${e}`);
      })




  }
  ngAfterViewInit(): void {
    if (this.cotacao) {
      this.dataCotacao = new Date(this.cotacao.dataCotacao);
      this.dataSelecionadaFormControl.setValue(this.dataCotacao);
      this.dataSelecionadaFormControl.disable();
    }

  }

  close(): void {
    this.modal.close(false);
  }

  //#region MODAL

  // tslint:disable-next-line: member-ordering
  static exibeModalDeIncluir(indice: { id: number, descricao: string, codigoValorIndice: string }): Promise<boolean> {
    // prettier-ignore
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(ImportarCotacaoModalComponent, { centered: true, size: 'lg', backdrop: 'static' });
    modalRef.componentInstance.indice = indice;
    return modalRef.result;
  }

  aoAdicionarArquivo() {
    const arquivo: File = this.arquivo.nativeElement.files;
    this.arquivoCsv = null;

    if (!this.validarArquivoSelecionado(arquivo)) {
      this.dialogService.err(
        'Importação não pode ser realizada!',
        'O arquivo importado não contém dados.'
      );
      return;
    }
    if (this.obterExtensaoDoArquivo(arquivo[0].name) === '.csv') {
      this.nomeArquivoFormControl.setValue(arquivo[0].name);
      
      const tamanhoArquivosEhMaior = this.ParametroTamanhoMaximo < arquivo[0].size;

      if (tamanhoArquivosEhMaior) {
        this.dialogService.err('Importação não pode ser realizada!', `O arquivo CSV importado ultrapassa o limite permitido de ${this.ParametroTamanhoMaximoMsg}MB.`);
        return;
      }
    } else {
      this.dialogService.err(
        'Importação não pode ser realizada!',
        ' O Arquivo não é um csv.'
      );
      return;
    }

    this.arquivoCsv = arquivo[0];
  }

  desabilitaTooltip(formControl: FormControl) {
    return formControl.untouched || formControl.valid;
  }
  obterExtensaoDoArquivo(nomeArquivo): string {
    const indexExtensaoDoArquivo = nomeArquivo.lastIndexOf('.');
    return nomeArquivo.slice(indexExtensaoDoArquivo);
  }

  validarArquivoSelecionado(arquivo: File): boolean {
    var validado = true;
    if (arquivo[0].size <= 17) validado = false;
    else if (isNullOrUndefined(arquivo)) validado = false;
    return validado;
  }
  //#endregion MODAL

}
