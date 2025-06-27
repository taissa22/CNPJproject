import { BehaviorSubject } from 'rxjs';
import { Injectable } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { TrabalhistaFiltrosService } from './trabalhista-filtros.service';
import { ListaFiltroAgendaAudienciaTrabalhistaEnum } from '../interface/ListaFiltroAgendaAudienciaTrabalhista.enum';
import { formatarData } from './../../../../shared/utils';
import { getDate } from 'ngx-bootstrap/chronos/utils/date-getters';


interface ICriteriosForm {
  classificacaoHierarquica: string;
  periodoAudienciaFim: any;
  periodoAudienciaInicial: any;
  periodoPendenciaFinal: any;
  periodoPendenciaInicial: any;
  processo: string;
  classificacaoClosing: string;
}

@Injectable({
  providedIn: 'root'
})
export class CriteriosGeraisFiltroService {

  constructor(private fb: FormBuilder,
    private trabalhistaService: TrabalhistaFiltrosService) { }

  private criterioForm: FormGroup;

  limparCriterios = new BehaviorSubject<boolean>(false)

  get inicializarForm(): FormGroup {
    this.criterioForm = this.fb.group({
      processo: [this.trabalhistaService.json.estrategico ?
        this.trabalhistaService.json.estrategico : "3"],

      periodoPendenciaInicial: [
        this.trabalhistaService.json.periodoPendenciaCalculoInicio],

      periodoPendenciaFinal: [
        this.trabalhistaService.json.periodoPendenciaCalculoFim],

      periodoAudienciaInicial: [
        this.trabalhistaService.json.dataAudienciaInicio],

      periodoAudienciaFim: [
        this.trabalhistaService.json.dataAudienciaFim],

      classificacaoHierarquica: [this.trabalhistaService.json.classificacaoHierarquica ?
        this.trabalhistaService.json.classificacaoHierarquica :
        "4"],

      classificacaoClosing: [this.trabalhistaService.json.classificacaoClosing ?
        this.trabalhistaService.json.classificacaoClosing :
        "4"],
    });

    return this.criterioForm;
  }

  get periodoPendenciaInicial(): Date {
    return this.criterioForm.controls.periodoPendenciaInicial.value;
  }

  set periodoPendenciaInicial(value) {
    this.criterioForm.controls.periodoPendenciaInicial.setValue(value);
  }

  get periodoPendenciaFinal(): Date {

    return this.criterioForm.controls.periodoPendenciaFinal.value;
  }

  set periodoPendenciaFinal(value) {
    this.criterioForm.controls.periodoPendenciaFinal.setValue(value);
  }

  get periodoAudienciaInicial(): Date {
    return this.criterioForm.controls.periodoAudienciaInicial.value;
  }

  set periodoAudienciaInicial(value) {
    this.criterioForm.controls.periodoAudienciaInicial.setValue(value);
  }


  get periodoAudienciaFim(): Date {
    return this.criterioForm.controls.periodoAudienciaFim.value;
  }

  set periodoAudienciaFim(value) {
    this.criterioForm.controls.periodoAudienciaFim.setValue(value);
  }

  get classificacaoHierarquica(): string {
    return this.criterioForm.controls.classificacaoHierarquica.value;
  }

  set classificacaoHierarquica(value) {
    this.criterioForm.controls.classificacaoHierarquica.setValue(value);
  }

  get classificacaoClosing(): string {
    return this.criterioForm.controls.classificacaoClosing.value;
  }

  set classificacaoClosing(value) {
    this.criterioForm.controls.classificacaoClosing.setValue(value);
  }

  get processo(): string {
    return this.criterioForm.controls.processo.value;
  }

  set processo(value) {
    this.criterioForm.controls.processo.setValue(value);
  }

  /**Soma os valores do formulário para mostrar no radio button esquerdo */
  somarValores(itemsForm: ICriteriosForm) {
    let count = 0;
    itemsForm.processo != "3" && count++;
    itemsForm.periodoAudienciaInicial && count++;
    itemsForm.periodoPendenciaInicial && count++;
    itemsForm.classificacaoHierarquica != "4" && count++;
    itemsForm.classificacaoClosing != "4" && count++;
    this.trabalhistaService.atualizarContagem(count,
      ListaFiltroAgendaAudienciaTrabalhistaEnum.criteriosGerais);

  }

  set isRangeValido(value){
    this.trabalhistaService.buscaInvalida = value;
  }

  adicionarJson(json: any) {




    json.processo == "3" ? this.trabalhistaService.json.estrategico = null :
      this.trabalhistaService.json.estrategico = json.processo;

    json.classificacaoHierarquica == "4" ?
      this.trabalhistaService.json.classificacaoHierarquica = null :
      this.trabalhistaService.json.classificacaoHierarquica
      = json.classificacaoHierarquica;

    this.trabalhistaService.json.dataAudienciaInicio =
    json.periodoAudienciaInicial;
    this.trabalhistaService.json.dataAudienciaFim =
    json.periodoAudienciaFim;

    this.trabalhistaService.json.periodoPendenciaCalculoInicio=
    json.periodoPendenciaInicial;
    this.trabalhistaService.json.periodoPendenciaCalculoFim =
    json.periodoPendenciaFinal;

    json.classificacaoClosing == "4" ?
      this.trabalhistaService.json.classificacaoClosing = null :
      this.trabalhistaService.json.classificacaoClosing
      = json.classificacaoClosing;

  }


}

