import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { ParametizacaoClosing } from '@manutencao/models/parametrizacao.closing.model';
import { Permissoes, PermissoesService } from '@permissoes';
import { DialogService } from '@shared/services/dialog.service';
import { Escritorio } from 'src/app/processos/agendaDeAudienciasDoCivelEstrategico/models';
import { ManutencaoParametrizacaoClosingService } from '../services/manutencao-parametrizacao-closing.service';

@Component({
  selector: 'civel-estrategico',
  templateUrl: './civel-estrategico.component.html',
  styleUrls: ['./civel-estrategico.component.scss']
})
export class CivelEstrategicoComponent implements OnInit {
  @Input() lista: ParametizacaoClosing[];
  tipoProcesso: number = 9;
  parametizacaoNova: ParametizacaoClosing;
  parametizacaoOriginal: ParametizacaoClosing;
  temPermissao: boolean = this.permissaoService.temPermissaoPara(Permissoes.ACESSAR_TAB_PARAMETRIZACAO_CLOSING_CE);
  teveAlteracao: boolean = false;
  escritorios: Escritorio[];

  classificacaoHibrido: FormControl = new FormControl();
  classificacaoHibridoFibra: FormControl = new FormControl();
  escritorioSelected: FormControl = new FormControl();

  form = new FormGroup({
    classificacaoHibrido: this.classificacaoHibrido,
    classificacaoHibridoFibra: this.classificacaoHibridoFibra,
    escritorioSelected: this.escritorioSelected,
  });
  constructor(private crudService: ManutencaoParametrizacaoClosingService,
    private dialogService: DialogService,
    private router: Router,
    private permissaoService: PermissoesService) {}

  ngOnInit() {
    this.iniciaLista();
    this.iniciarValoresForm();
    this.temPermissao == false ? this.form.disable() : null;
    this.obterEscritorios();
  }

  verificaDesabilitado(){
    return (this.temPermissao && this.teveAlteracao) ? false : true;
  }

  iniciaLista() {
    this.parametizacaoOriginal = {...this.lista.find(i => i.codTipoProcesso == this.tipoProcesso)};
    this.parametizacaoNova = {...this.lista.find(i => i.codTipoProcesso == this.tipoProcesso)};
    if(this.parametizacaoNova == undefined || this.parametizacaoNova == null || Object.keys(this.parametizacaoNova).length === 0){
      this.parametizacaoNova.classificaoClosing = 4;
      this.parametizacaoNova.indClosingHibrido = '';
      this.parametizacaoNova.percResponsabilidade = 0;
      this.parametizacaoNova.idEscritorioPadrao = 0;
      this.parametizacaoNova.idEscritorioPadraoClientCO = 0;
    }
  }

  iniciarValoresForm() {
    this.classificacaoHibrido.setValue(this.parametizacaoNova.indClosingHibrido);
    this.classificacaoHibridoFibra.setValue(this.parametizacaoNova.indClosingHibridoClientCO);
    this.escritorioSelected.setValue(this.parametizacaoNova.idEscritorioPadrao);
  }

  obterEscritorios(){
    this.crudService.obterEscritorios()
      .subscribe(
          resultado => {
              this.escritorios = resultado;
              console.log(this.escritorios);
          },
          error => console.log(error)
      );
      console.log("escritorios");
  }

  alterarLista(){
    let form = this.form.value;
    this.parametizacaoNova.indClosingHibrido = form.classificacaoHibrido;
    this.parametizacaoNova.indClosingHibridoClientCO = form.classificacaoHibridoFibra;
    this.parametizacaoNova.idEscritorioPadrao = Number(form.escritorioSelected);

    this.teveAlteracao = this.existeAlteracao() ? true : false;
  }

  resetaLista(){
    this.iniciaLista();
    this.iniciarValoresForm();
    this.teveAlteracao = false;
  }

  existeAlteracao(){
    return JSON.stringify(this.parametizacaoOriginal) !== JSON.stringify(this.parametizacaoNova);
  }

  buscar() {
    this.crudService.obter()
        .subscribe(
            resultado => {
                this.lista = resultado;
                this.iniciaLista();
            },
            error => console.log(error)
        );
  }

  async salvar() {
    let maiorQueCemMenorQueZero = (this.parametizacaoNova.percResponsabilidade > 100 || this.parametizacaoNova.percResponsabilidade < 0);
    let nulo = (this.parametizacaoNova.percResponsabilidade == null || this.parametizacaoNova.percResponsabilidade == undefined || isNaN(this.parametizacaoNova.percResponsabilidade));

    if (maiorQueCemMenorQueZero || nulo) {
        await this.dialogService.err(`Atenção`, `O percentual de responsabilidade deve estar entre 0 e 100.`);
        return;
    }

    await this.crudService.atualizar(this.parametizacaoNova).subscribe(
        result => {
            this.dialogService.alert(`Alteração realizada com sucesso`);
            this.buscar();
            this.teveAlteracao = false;
        },
        erro => {
            this.dialogService.err(`Erro`, `Não foi possível realizar a alteração`);
        }
    );
  }

  cancelar() {
    this.resetaLista();
  }

}
