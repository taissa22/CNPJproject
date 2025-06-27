import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { CriteriosImportacaoArquivoRetornoService } from './services/criterios-importacao-arquivo-retorno.service';
import { distinctUntilChanged } from 'rxjs/operators';

@Component({
  selector: 'criterios-importacao-arquivo-retorno',
  templateUrl: './criterios-importacao-arquivo-retorno.component.html',
  styleUrls: ['./criterios-importacao-arquivo-retorno.component.scss']
})
export class CriteriosImportacaoArquivoRetornoComponent implements OnInit {

  constructor(public service: CriteriosImportacaoArquivoRetornoService) { }
  
  isDataRemessaValid = true;
  isNumeroRemessaValid = true;
  isValoresGuiaValid = true;
  @Output() validacao = new EventEmitter()

  form: FormGroup;
  ngOnInit() {
    this.form = this.service.inicializarFormulario();

    this.form.valueChanges.subscribe(item => this.service.atualizarContagem());
  }


  atualizarData(e) {
    this.service.dataRemessaFim = e;
    this.service.atualizarContagem();
  }

  atualizarRemessa(e) {
    this.service.numeroRemessaFim = e;
    this.service.atualizarContagem();
  }

  atualizarIntervalo(e) {
    this.service.intervaloValoresGuiaFim = e;
    this.service.atualizarContagem();
  }

  atualizarNumeroRemessaValido(event) {
    this.isNumeroRemessaValid = event;  
    this.emitirValidacao();
  }

  atualizarDataRemessaValido(event) {
    this.isDataRemessaValid = event;  
    this.emitirValidacao();
  }

  atualizarIntervaloGuiaValido(event) {
    this.isValoresGuiaValid = event;
    this.emitirValidacao();
  }

  emitirValidacao() {
    this.validacao.emit(this.isValoresGuiaValid && this.isNumeroRemessaValid && this.isDataRemessaValid)
    this.service.isValidSubject.next(this.isValoresGuiaValid && this.isNumeroRemessaValid && this.isDataRemessaValid);
  }
}
