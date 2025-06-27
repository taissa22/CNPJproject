import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { ParametizacaoClosing } from '@manutencao/models/parametrizacao.closing.model';
import { Permissoes, PermissoesService } from '@permissoes';
import { DialogService } from '@shared/services/dialog.service';
import { Router } from '@angular/router';
import { ManutencaoParametrizacaoClosingService } from '../services/manutencao-parametrizacao-closing.service';

@Component({
  selector: 'civel-consumidor',
  templateUrl: './civel-consumidor.component.html',
  styleUrls: ['./civel-consumidor.component.scss']
})
export class CivelConsumidorComponent implements OnInit {
  @Input() lista: ParametizacaoClosing[];
  tipoProcesso: number = 1;
  parametizacaoNova: ParametizacaoClosing;
  parametizacaoOriginal: ParametizacaoClosing;
  temPermissao: boolean = this.permissaoService.temPermissaoPara(
    Permissoes.ACESSAR_TAB_PARAMETRIZACAO_CLOSING_CC
  );
  teveAlteracao: boolean = false;

  classificacaoClosing: FormControl = new FormControl();
  classificacaoClosingFibra: FormControl = new FormControl();
  classificacaoHibrido: FormControl = new FormControl();
  classificacaoHibridoFibra: FormControl = new FormControl();
  percentualResponsabilidade: FormControl = new FormControl();

  form = new FormGroup({
    classificacaoClosing: this.classificacaoClosing,
    classificacaoClosingFibra: this.classificacaoClosingFibra,
    classificacaoHibrido: this.classificacaoHibrido,
    classificacaoHibridoFibra: this.classificacaoHibridoFibra,
    percentualResponsabilidade: this.percentualResponsabilidade,
  });

  constructor(
    private crudService: ManutencaoParametrizacaoClosingService,
    private dialogService: DialogService,
    private router: Router,
    private permissaoService: PermissoesService
  ) {}

  ngOnInit() {
    this.iniciaLista();
    this.iniciarValoresForm();
    if (!this.temPermissao) {
      this.form.disable();
    }
  }

  verificaDesabilitado() {
    return !(this.temPermissao && this.teveAlteracao);
  }

  iniciaLista() {
    this.parametizacaoOriginal = {
      ...this.lista.find(i => i.codTipoProcesso === this.tipoProcesso)
    };
    this.parametizacaoNova = {
      ...this.lista.find(i => i.codTipoProcesso === this.tipoProcesso)
    };

    if (
      !this.parametizacaoNova ||
      Object.keys(this.parametizacaoNova).length === 0
    ) {
      this.parametizacaoNova.classificaoClosing = 4;
      this.parametizacaoNova.indClosingHibrido = '';
      this.parametizacaoNova.percResponsabilidade = 0;
      this.parametizacaoNova.classificaoClosingClientCO = 4;
      this.parametizacaoNova.indClosingHibridoClientCO = '';
      this.parametizacaoNova.percResponsabilidadeClientCO = 0;
    }
  }

  iniciarValoresForm() {
    this.classificacaoClosing.setValue(
      this.parametizacaoNova.classificaoClosing
    );
    this.classificacaoClosingFibra.setValue(
      this.parametizacaoNova.classificaoClosingClientCO
    );
    this.classificacaoHibrido.setValue(
      this.parametizacaoNova.indClosingHibrido
    );
    this.classificacaoHibridoFibra.setValue(
      this.parametizacaoNova.indClosingHibridoClientCO
    );

    this.percentualResponsabilidade.setValue(
      this.parametizacaoNova.percResponsabilidade !== null
        ? this.parametizacaoNova.percResponsabilidade
        : null
    );

  }

  alterarLista() {
    const form = this.form.value;
    this.parametizacaoNova.indClosingHibrido = form.classificacaoHibrido;
    this.parametizacaoNova.indClosingHibridoClientCO =
      form.classificacaoHibridoFibra;
    this.parametizacaoNova.classificaoClosing = form.classificacaoClosing;
    this.parametizacaoNova.classificaoClosingClientCO =
      form.classificacaoClosingFibra;

    if (form.percentualResponsabilidade !== null) {
      this.parametizacaoNova.percResponsabilidade = parseFloat(
        typeof form.percentualResponsabilidade === 'string'
          ? form.percentualResponsabilidade.replace(',', '.')
          : form.percentualResponsabilidade
      );
    }

    this.teveAlteracao = this.existeAlteracao();
  }

  VerificaZero(event: any) {
    if (event.target.value == 0) {
      this.dialogService.err(
        `Atenção`,
        `O percentual de responsabilidade Oi tem que ser maior do que 0 e menor do que 100 para processos híbridos.`
      );
      event.target.value = '';
      this.teveAlteracao = false;
    }
  }

  resetaLista() {
    this.iniciaLista();
    this.iniciarValoresForm();
    this.teveAlteracao = false;
  }

  existeAlteracao() {
    return (
      JSON.stringify(this.parametizacaoOriginal) !==
      JSON.stringify(this.parametizacaoNova)
    );
  }

  buscar() {
    this.crudService.obter().subscribe(
      resultado => {
        this.lista = resultado;
        this.iniciaLista();
      },
      error => console.log(error)
    );
  }

  async salvar() {
    const maiorQueCemMenorQueZero =
      this.parametizacaoNova.percResponsabilidade > 100 ||
      this.parametizacaoNova.percResponsabilidade < 0;
    const nulo =
      this.parametizacaoNova.percResponsabilidade == null ||
      this.parametizacaoNova.percResponsabilidade == undefined ||
      isNaN(this.parametizacaoNova.percResponsabilidade);

    if (maiorQueCemMenorQueZero || nulo) {
      await this.dialogService.err(
        `Atenção`,
        `O percentual de responsabilidade Oi tem que ser maior do que 0 e menor do que 100 para processos híbridos.`
      );
      return;
    }

    await this.crudService.atualizar(this.parametizacaoNova).subscribe(
      () => {
        this.dialogService.alert(`Alteração realizada com sucesso`);
        this.buscar();
        this.teveAlteracao = false;
      },
      (errorResponse) => {
        if (errorResponse && errorResponse.error && errorResponse.error.code === 'ERR001') {
          this.dialogService.err(`A Operação não pode ser realizada`, errorResponse.error.message);
        } else {
          this.dialogService.err(`Erro`, `Não foi possível realizar a alteração`);
        }
      }
    );
  }

  cancelar() {
    this.resetaLista();

  }
}
