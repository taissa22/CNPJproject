import { Component, ElementRef, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { NgbDateAdapter, NgbDateNativeAdapter, NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import { NgbDateCustomParserFormatter } from '@shared/dateformat';
import { IntrucoesParaCargaModalComponent } from '../modals/intrucoes-para-carga-modal/intrucoes-para-carga-modal.component';
import { AgendamentoLogProcessoService } from '../services/agendamento-log-processo.service';
import moment from 'moment';
import * as lodash from 'lodash';
import { DialogService } from '@shared/services/dialog.service';
import { HttpErrorResult } from '@core/http';
import { TipoLogEnum } from '../enuns/tipoLog.enum';
import { OperacoesEnum } from '../enuns/operacoes.enum';
@Component({
  selector: 'agendar-relatorio',
  templateUrl: './agendar-relatorio.component.html',
  styleUrls: ['./agendar-relatorio.component.scss'],
  providers: [{ provide: NgbDateAdapter, useClass: NgbDateNativeAdapter }, { provide: NgbDateParserFormatter, useClass: NgbDateCustomParserFormatter }]
})
export class AgendarRelatorioComponent implements OnInit {

  formGroup: FormGroup;
  dataInicialFormControl: FormControl;
  public comboLogs: any[] = [];
  public comboOperacoes = [];
  nomeDoArquivo: string = "";
  arquivo: File = null; 
  tamanhoMaximoArquivosAnexos: number ;
  mensagemTamanhoMaximoArquivoAnexo: number;
  @Output() atualizarLista = new EventEmitter();
  @ViewChild('inputCsv', { static: false }) inputCsv: ElementRef;
  constructor(public service: AgendamentoLogProcessoService, private dialog: DialogService) { }

  async ngOnInit() {
    this.listarEnuns();
    this.criarForm();
    this.obterTamanhoMaximoArquivoAnexo();
  }

  listarEnuns() {
    for (let log in TipoLogEnum) {
      this.comboLogs.push({ id: parseInt(TipoLogEnum[log]), texto: log });
    }
    for (let op in OperacoesEnum) {
      this.comboOperacoes.push({ id: OperacoesEnum[op], texto: op });
    }
  }

  criarForm() {
    this.formGroup = new FormGroup({
      Operacao: new FormControl("T"),
      TipoLog: new FormControl(0),
      DataIni: new FormControl(new Date()),
      DataFim: new FormControl(new Date())
    })
    this.nomeDoArquivo = "";
    this.arquivo = null;
    if(this.inputCsv && this.inputCsv.nativeElement) this.inputCsv.nativeElement.value = '';
  }

  carregarArquivo() {
    const arquivo: File = this.inputCsv.nativeElement.files;
    if (arquivo && arquivo[0]) {
      this.nomeDoArquivo = arquivo[0].name;
      this.arquivo = arquivo[0];
      return false;
    }
    this.arquivo = null;
  }


  async salvar() { 
      this.geraObjParaOBackEnd(obj => {
        if (!this.validaObjParaOBackEnd(obj)) return false;
        this.service.criar(obj).then(() => {
          this.dialog.alert("Salvo com sucesso");
          this.atualizarLista.emit();
          this.criarForm(); 
        }).catch(error => { 
          if (error.error.includes('registros para carga.')) {
             this.dialog.err('Desculpe, o upload do arquivo não poderá ser realizado', error.error);
            return;
          }
           this.dialog.err(`Inclusão não realizada`, error.error);
        })
      }); 
  }


  geraObjParaOBackEnd(retorno: Function) {
    let form: any = lodash.cloneDeep(this.formGroup.value);
    form.DataIni = form.DataIni ? moment(form.DataIni).format(moment.HTML5_FMT.DATE) : "";
    form.DataFim = form.DataFim ? moment(form.DataFim).format(moment.HTML5_FMT.DATE) : "";
    this.converterArquivoParaBase64(this.arquivo, (arquivo) => {
      form.ArquivoBase64 = arquivo;
      retorno(form);
    });
  }

  converterArquivoParaBase64(arquivo: File, retorno: Function = new Function()) {
    if(this.arquivo == null) {
      retorno(null);
      return false;
    }
    var reader = new FileReader();
    reader.readAsDataURL(arquivo);
    reader.onload = function () {
      retorno(reader.result);
    };
    reader.onerror = function (error) {
      retorno(null);
    };
  }

  validaObjParaOBackEnd(obj) {

    if (!obj.DataIni) {
      this.dialog.err(`A data inicial deve ser preenchida.`);
      return false;
    }

    if (!obj.DataFim) {
      this.dialog.err(`A data final deve ser preenchida.`);
      return false;
    }

    if ( moment(obj.DataFim).format(moment.HTML5_FMT.DATE) < moment(obj.DataIni).format(moment.HTML5_FMT.DATE) ) {
      this.dialog.err(
        'A data inicial não pode ser maior que a data final'
      );  
      return false;
    }

    if (this.arquivo == null) {
      this.dialog.err(`Nenhum arquivo foi selecionado.`);
      return false;
    }
    const indexExtensaoDoArquivo = this.arquivo.name.lastIndexOf('.');
    if (this.arquivo.name.slice(indexExtensaoDoArquivo) != '.csv') {
      this.dialog.err('Desculpe, o upload do arquivo não poderá ser realizado', 'O arquivo importado não é CSV.');
      return false;
    }
   
    if(this.arquivo.size  > this.tamanhoMaximoArquivosAnexos){
      this.dialog.err('Desculpe, o upload do arquivo não poderá ser realizado', 'O tamanho do arquivo importado ultrapassa o limite de '+this.mensagemTamanhoMaximoArquivoAnexo+'MB.');
      return false;
    }

    return true;
  }


  obterTamanhoMaximoArquivoAnexo(){ 
    try {
      this.service.obterTamanhoMaximoArquivoAnexo().then(resultado =>{ 
        this.tamanhoMaximoArquivosAnexos = parseFloat(((resultado.dscConteudoParametro * 1024) * 1024).toFixed(2)); 
        this.mensagemTamanhoMaximoArquivoAnexo = resultado.dscConteudoParametro;
      });
    } catch (error) {
        this.dialog.err('Operação não realizada', (error as HttpErrorResult).messages.join('\n'));
    }
  }
  abreInstrucoesParaCarga() {
    IntrucoesParaCargaModalComponent.exibeModal();
  }

    
  mascaraDataKeyUp(formControl, id :string){  
    if(typeof(formControl.value) != "string") return false; 
    let el:any =  document.getElementById(id);
    if(formControl.value.length == 2){ 
      el.value = formControl.value+"/";
    }
    else if(formControl.value.length == 5){
      el.value = formControl.value+"/";
    } 
  }

  resetarFormControlInvalido(formControl) { 
    if (formControl.invalid) {
      formControl.reset();
    }
  }
}
