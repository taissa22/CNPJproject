import { Component, OnInit } from '@angular/core';
import { LoteService } from 'src/app/core/services/sap/lote.service';
import { PermissoesSapService } from 'src/app/sap/permissoes-sap.service';
import { FormGroup, FormControl } from '@angular/forms';
import { ConsultaCriterioCivelConsumidorService } from '../../service/consulta-criterio-civel-consumidor.service';
import { ConsultaTipoProcessoService } from '../../service/consulta-tipo-processo.service';
import { Subscription } from 'rxjs';
import { ConsultaSaldoGarantiaService } from '../../service/consulta-saldo-garantia.service';

@Component({
  selector: 'app-saldo-garantia-criterios-gerais',
  templateUrl: './saldo-garantia-criterios-gerais.component.html',
  styleUrls: ['./saldo-garantia-criterios-gerais.component.scss']
})
export class SaldoGarantiaCriteriosGeraisComponent implements OnInit {


  constructor(private service: ConsultaCriterioCivelConsumidorService,
    public loteService: LoteService,
    private tipoProcessoService: ConsultaTipoProcessoService,
    public permissoesSapService: PermissoesSapService,
    private consultaSaldoGarantiaService: ConsultaSaldoGarantiaService
  ) { }

  form = new FormGroup({
    migrados: new FormControl(this.service.selectMigrados),
    processo: new FormControl(this.service.selectProcesso),
    umBloqueio: new FormControl(this.service.umBloqueio),
    garantiaDeposito: new FormControl(this.service.garantiaDeposito),
    garantiaBloqueio: new FormControl(this.service.garantiaBloqueio),
    garantioOutros: new FormControl(this.service.garantioOutros),
    riscoProvavel: new FormControl(this.service.riscoProvavel),
    riscoPossivel: new FormControl(this.service.riscoPossivel),
    riscoRemoto: new FormControl(this.service.riscoRemoto),
    agencia: new FormControl(this.service.agencia),
    conta: new FormControl(this.service.conta),
  });

  limparSubscription = new Subscription();

  public campoExibir: any;
  public tipoSelecionado = false;
  public processoSelecionado: number;

  ngOnInit() {
    this.service.atualizarContador();
    this.resetBoxs();
    this.verificarTipoProcesso();
    this.changeRadioButton('migrados', this.form.value.migrados);
    this.changeRadioButton('processo', this.form.value.processo);
    this.changeCheckbox('umBloqueio', this.form.value.umBloqueio);
    this.changeCheckbox('garantiaDeposito', this.form.value.garantiaDeposito);
    this.changeCheckbox('garantiaBloqueio', this.form.value.garantiaBloqueio);
    this.changeCheckbox('garantioOutros', this.form.value.garantioOutros);
    this.changeCheckbox('riscoProvavel', this.form.value.riscoProvavel);
    this.changeCheckbox('riscoPossivel', this.form.value.riscoPossivel);
    this.changeCheckbox('riscoRemoto', this.form.value.riscoRemoto);
    this.service.agencia = this.form.value.agencia;
    this.service.conta = this.form.value.conta;
    this.service.verificarCheckpoints()

    this.form.valueChanges.subscribe(formResult => {
      this.changeRadioButton('migrados', formResult.migrados);
      this.changeRadioButton('processo', formResult.processo);
      this.changeCheckbox('umBloqueio', formResult.umBloqueio);
      this.changeCheckbox('garantiaDeposito', formResult.garantiaDeposito);
      this.changeCheckbox('garantiaBloqueio', formResult.garantiaBloqueio);
      this.changeCheckbox('garantioOutros', formResult.garantioOutros);
      this.changeCheckbox('riscoProvavel', formResult.riscoProvavel);
      this.changeCheckbox('riscoPossivel', formResult.riscoPossivel);
      this.changeCheckbox('riscoRemoto', formResult.riscoRemoto);
      this.service.agencia = formResult.agencia;
      this.service.conta = formResult.conta;
      this.service.verificarCheckpoints()
      this.service.adicionarDadosDTO()
    });

    this.limparSubscription = this.consultaSaldoGarantiaService.limparConsulta.subscribe(e => {
      this.resetBoxs();
    });
  }

  onDestroy() {
    this.tipoSelecionado = false;
    this.limparSubscription.unsubscribe();
  }

  private verificarTipoProcesso() {
    const valorAntigo = this.tipoProcessoService.tipoProcessoTracker.value;

    this.tipoProcessoService.tipoProcessoTracker.subscribe(item => {
      this.campoExibir = this.service.verificarCamposExibir(item);
      this.processoSelecionado = item;
      if (item) {
        this.tipoSelecionado = true;
      }

      else {
        this.tipoSelecionado = false;
      }

      if (item !== valorAntigo) {
        this.service.limparDados()
        this.resetBoxs();

      }
    });
  }

  resetBoxs() {
    this.form.reset({
      migrados: this.service.selectMigrados,
      processo: this.service.selectProcesso,
      umBloqueio: this.service.umBloqueio,
      garantiaDeposito: this.service.garantiaDeposito,
      garantiaBloqueio: this.service.garantiaBloqueio,
      garantioOutros: this.service.garantioOutros,
      riscoProvavel: this.service.riscoProvavel,
      riscoPossivel: this.service.riscoPossivel,
      riscoRemoto: this.service.riscoRemoto,
      agencia: this.service.agencia,
      conta: this.service.conta
    });
    this.txtConta = null;
    this.erroConta = false;
    this.txtAgencia = null;
    this.erroAgencia = null;
  }

  erroAgencia;
  txtAgencia;

  verificarAgencia() {

    if (this.service.validarAgencia(
      this.form.get('conta').value, this.form.get('agencia').value
    ) && this.service.validarAgencia(this.form.get('conta').value,
      this.form.get('agencia').value) != 'valid') {
      this.txtAgencia = this.service.validarAgencia(this.form.get('conta').value,
        this.form.get('agencia').value);
      this.erroAgencia = true;
    }

    else if (this.service.validarConta(
      this.form.get('conta').value,
      this.form.get('agencia').value
    ) == 'valid') {
      this.txtConta = null;
      this.erroConta = false;
      this.txtAgencia = null;
      this.erroAgencia = null;
    }

    else {
      this.txtAgencia = null;
      this.erroAgencia = false;
    }
  }

  erroConta;
  txtConta;

  verificarConta() {

    if (this.service.validarConta(this.form.get('conta').value,
      this.form.get('agencia').value) && this.service.validarConta(
        this.form.get('conta').value,
        this.form.get('agencia').value
      ) != 'valid') {
      this.txtConta = this.service.validarConta(
        this.form.get('conta').value,
        this.form.get('agencia').value
      );
      this.erroConta = true;
      this.verificarAgencia();
    }

    else if (this.service.validarConta(this.form.get('conta').value,
      this.form.get('agencia').value) == 'valid') {
      this.txtConta = null;
      this.erroConta = false;
      this.txtAgencia = null;
      this.erroAgencia = null;
      this.verificarAgencia();
    }

    else if (this.service.validarConta(this.form.get('conta').value,
      this.form.get('agencia').value)) {
      this.txtConta = null;
      this.erroConta = false;
      this.verificarAgencia();
    }

    else {
      this.verificarAgencia();
    }

  }

  /**
  * Envia os valores do data range para o componente e service
  * @param label Label do campo de data utilizado
  * @param isDataInicial true se for data inicial, false se for data final
  * @param data Valor da data
  */
  changeData(label: string, isDataInicial: boolean, data: Date) {
    this.service.setData(label, isDataInicial, data);
    this.service.adicionarDadosDTO()
  }

  /**
  * Envia os valores do data range para o componente e service
  * @param label Label do campo de data utilizado
  * @param isDataInicial true se for data inicial, false se for data final
  * @returns returna uma data
  */
  recuperarData(label: string, isDataInicial: boolean): Date {
    return this.service.getData(label, isDataInicial);
  }

  onRangeValido(nomeCampo: string, valid: boolean) {
    this.service.validar(nomeCampo, valid);
  }

  recuperarNumero(label: string, isInicio: boolean): string {
    return this.service.getNumero(label, isInicio);
  }

  salvarNumero(label: string, isInicio: boolean, numero: string) {
    this.service.setNumero(label, isInicio, numero);
    this.service.adicionarDadosDTO()
  }

  changeRadioButton(nome: string, opcao: string) {
    this.service.setRadioButton(nome, opcao);
  }

  changeCheckbox(nome: string, opcao: boolean) {
    this.service.setCheckbox(nome, opcao);
  }
}
