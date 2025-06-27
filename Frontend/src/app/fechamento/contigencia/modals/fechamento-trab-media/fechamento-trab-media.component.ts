import { AfterViewInit, Component, EventEmitter, OnInit, Output } from '@angular/core';
import { HelperAngular } from '@shared/helpers/helper-angular';
import { BsModalRef } from 'ngx-bootstrap';
import { FechamentoDto } from 'src/app/fechamento/models/contigencia/agendar-solicitar-fechamento-contingencia/fechamento-dto.model';
import { AgendarSolicitarFechamentoContingenciaService } from 'src/app/fechamento/services/agendar-solicitar-fechamento-contingencia.service';

@Component({
  selector: 'app-fechamento-trab-media',
  templateUrl: './fechamento-trab-media.component.html',
  styleUrls: ['./fechamento-trab-media.component.scss']
})
export class FechamentoTrabMediaComponent implements OnInit, AfterViewInit {

  @Output() onCancel = new EventEmitter();
  @Output() onConfirm = new EventEmitter();

  constructor(
    private messageService: HelperAngular,
    private service: AgendarSolicitarFechamentoContingenciaService
  ) { }

  ngOnInit() {
    this.execucaoImediata = true;
    this.execucaoImediataFechamentoMensal = false;
    this.execucaoImediataGerarBaseDados = false;
    this.execucaoImediataAgendAutRelMov = false;

    this.dataEspecifica = false;
    this.dataEspecificaFechamentoMensal = false;
    this.dataEspecificaGerarBaseDados = false;
    this.dataEspecificaAgendAutRelMov = true;

    this.diaria = false;
    this.diariaExecutarPreviaDiaUtil = false;
    this.diariaAgendAutRelMov = true;

    this.semanal = false;
    this.semanalAgendAutRelMov = true;

    this.mensal = false;
    this.mensalUltimoDiaMes = false;
    this.mensalFechamentoMensal = false
    this.mensalGerarBaseDados = false;
    this.mensalAgendAutRelMov = true;

    this.obterEmpresas();
    this.obterEmpresasGrupo();
    this.obterValoresTrabalhista();
    this.novovalor(this.editou, this.fechamentoEditado);
    this.calcularDias();
    this.calcularMeses();
  }

  ngAfterViewInit() {
    setTimeout(() => {
      this.edicaoDados(this.editou)
    }, 600)
  }

  id: number;
  editou: boolean;
  fechamentoEditado: any;

  fechamentoDto = new FechamentoDto;

  empresas = []
  listaEmpresas = []
  qtdEmpresasMarcadas: number;
  empresasTooltip = [];
  grupamentoEmpresas = [];
  diasDoMes = [];
  qtdNumeroMeses = [];

  //#region  FECHAMENTO
  fechamentoNumeroMeses: number;
  fechamentoNaoAplicarOutilier: boolean = true;
  fechamentoAplicarOutilier: boolean = false;
  fechamentoDesvioPadraoCheck: boolean = true;
  fechamentoDesvioPadrao: number;
  fechamentoAplicarExclusaoOutlierCheck: boolean = false;
  fechamentoPercAplicarExclusaoOutlier: string

  //#endregion

  //#region  EXECUÇÃO IMEDIATA
  execucaoImediata: boolean;
  execucaoImediataDiaContabil = new Date().getDate();
  execucaoImediataDataPrevia = new Date(new Date().getFullYear(), new Date().getMonth(), this.execucaoImediataDiaContabil - 1);
  execucaoImediataMesContabil = this.execucaoImediataDataPrevia.getMonth();
  execucaoImediataAnoContabil = this.execucaoImediataDataPrevia.getFullYear();
  execucaoImediataDataAtual = new Date(new Date().getFullYear(), new Date().getMonth(), this.execucaoImediataDiaContabil);
  execucaoImediataFechamentoMensal: boolean;
  execucaoImediataGerarBaseDados: boolean;
  execucaoImediataParcial: boolean;
  execucaoImediataAgendAutRelMov: boolean;

  //#endregion

  //#region DATA ESPECIFICA
  dataEspecifica: boolean;
  dataEspecificaDiaContabil = new Date().getDate();
  dataEspecificaDataPrevia = new Date(new Date().getFullYear(), new Date().getMonth(), this.dataEspecificaDiaContabil - 1);
  dataEspecificaMesContabil = this.dataEspecificaDataPrevia.getMonth();
  dataEspecificaAnoContabil = this.dataEspecificaDataPrevia.getFullYear();
  dataEspecificaDataMinima = new Date(new Date().getFullYear(), new Date().getMonth(), new Date().getDate() + 1);
  dataEspecificaExecutarData: Date;
  dataEspecificaFechamentoMensal: boolean;
  dataEspecificaGerarBaseDados: boolean;
  dataEspecificaParcial: boolean;
  dataEspecificaAgendAutRelMov: boolean;
  //#endregion

  //#region DIARIA
  diaria: boolean;
  diariaPreviaInicio: Date;
  diariaPreviaFim: Date;
  diariaExecutarPreviaDiaUtil: boolean;
  diariaAgendAutRelMov: boolean;
  //#endregion

  //#region  SEMANAL
  semanal: boolean;
  semanalDiaPrevia = "";
  semanalAgendAutRelMov: boolean;
  //#endregion

  //#region MENSAL
  mensal: boolean;
  mensalUltimoDiaMes: boolean;
  mensalFechamentoMensal: boolean;
  mensalGerarBaseDados: boolean;
  mensalDiaPrevia = "";
  mensalAgendAutRelMov: boolean;
  //#endregion

  calcularDias() {
    let dias = ['1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11', '12', '13', '14', '15', '16', '17', '18', '19', '20', '21', '22', '23', '24', '25', '26', '27', '28']
    let ultimoDia = (new Date(new Date().getFullYear(), new Date().getMonth() + 1, 0)).getDate();

    ultimoDia == 29 ? dias.push('29') : ultimoDia == 30 ? dias.push('29', '30') : ultimoDia == 31 ? dias.push('29', '30', '31') : dias;

    this.diasDoMes = dias;
  }

  fechamentoCheck(item) {
    this.fechamentoNaoAplicarOutilier = false;
    this.fechamentoAplicarOutilier = false;
    switch (item) {
      case 'fechamentoNaoAplicarOutilier':
        this.fechamentoNaoAplicarOutilier = true;
        break
      case 'fechamentoAplicarOutilier':
        this.fechamentoAplicarOutilier = true;
        this.fechamentoDesvioPadraoCheck = true;
        this.fechamentoAplicarExclusaoOutlierCheck = false;
        break
    }
  }

  outlierCheck(item) {
    this.fechamentoDesvioPadraoCheck = false;
    this.fechamentoAplicarExclusaoOutlierCheck = false;
    switch (item) {
      case 'fechamentoDesvioPadraoCheck':
        this.fechamentoDesvioPadraoCheck = true;
        break
      case 'fechamentoAplicarExclusaoOutlierCheck':
        this.fechamentoAplicarExclusaoOutlierCheck = true;
        break
    }
  }

  tipoExecucao(item) {
    this.execucaoImediata = false;
    this.dataEspecifica = false;
    this.diaria = false;
    this.semanal = false;
    this.mensal = false;
    (<HTMLInputElement>document.getElementById(item)).checked = true;

    switch (item) {
      case 'execucaoImediata':
        this.execucaoImediata = true;
        break
      case 'dataEspecifica':
        this.dataEspecifica = true;
        break
      case 'diaria':
        this.diaria = true;
        break
      case 'semanal':
        this.semanal = true;
        break
      case 'mensal':
        this.mensal = true;
        break
    }
  }

  checkValue(name, value, type) {
    switch (name) {
      case 'fechamentoDesvioPadrao':
        type == 'focus' ? value == 0 ? this.fechamentoDesvioPadrao = null : this.fechamentoDesvioPadrao : '';
        type == 'blur' ? value == '' || value == null ? this.fechamentoDesvioPadrao = 0 : this.fechamentoDesvioPadrao : '';
        break;
      case 'fechamentoPercAplicarExclusaoOutlier':
        type == 'focus' ? value == '0,00' ? this.fechamentoPercAplicarExclusaoOutlier = null : this.fechamentoPercAplicarExclusaoOutlier : '';
        type == 'blur' ? value == '' || value == null ? this.fechamentoPercAplicarExclusaoOutlier = '0,00' : this.fechamentoPercAplicarExclusaoOutlier : '';
    }
  }

  calcularMeses() {
    for (let i = 0; i < 100; i++) {
      this.qtdNumeroMeses.push(i)
    }
  }

  obterEmpresas() {
    this.service.obterEmpresas().subscribe(result => {
      this.empresas = result
    });
  }

  obterEmpresasGrupo() {
    this.service.obterEmpresasGrupos().subscribe(result => {
     this.empresasTooltip = result;
    });
  }

  empCentSelecionada(id: number){
    this.grupamentoEmpresas = this.empresasTooltip.filter(x => x.codigoEmp == id)
  }

  obterValoresTrabalhista() {
    this.service.obterValoresTrabalhista().subscribe(result => {
      this.fechamentoDesvioPadrao = result.valAjustePadrao.toString().replace('.', ',')
      this.fechamentoPercAplicarExclusaoOutlier = result.valPercProOut.toString().replace('.', ',')
    });
  }

  salvarFechamento() {
    let fechamentoSubmit = Object.assign({})

    this.listaEmpresas = [];
    let empresa = document.querySelectorAll("input[name='empresas']:checked")
    for (let i = 0; i < empresa.length; i++) {
      this.listaEmpresas.push(empresa[i].textContent)
    }

    fechamentoSubmit.indAtivo = "S";
    fechamentoSubmit.empresa = this.listaEmpresas.toString();
    fechamentoSubmit.codTipoFechamento = 7;
    fechamentoSubmit.numeroDeMeses = this.fechamentoNumeroMeses;

    if (this.fechamentoAplicarOutilier) {
      if (this.fechamentoDesvioPadraoCheck) {
        fechamentoSubmit.valAjusteDesvioPadrao = this.fechamentoDesvioPadrao;
      }
      if (this.fechamentoAplicarExclusaoOutlierCheck) {
        fechamentoSubmit.valPercentProcOutliers = this.fechamentoPercAplicarExclusaoOutlier.replace(',', '.');
      }
    }

    if (this.execucaoImediata) {
      if (this.execucaoImediataDataPrevia == null) { this.messageService.MsgBox2('Data da prévia não informada.', 'Não foi possível fazer o agendamento', 'warning', 'Ok'); return; }
      fechamentoSubmit.periodicidadeExecucao = 0
      fechamentoSubmit.indExecucaoImediata = "S";
      fechamentoSubmit.mesContabil = this.execucaoImediataMesContabil
      fechamentoSubmit.anoContabil = this.execucaoImediataAnoContabil
      fechamentoSubmit.dataPrevia = this.execucaoImediataDataPrevia.toLocaleDateString("pt-BR").split('/').reverse().join('-');
      fechamentoSubmit.indFechamentoMensal = this.execucaoImediataFechamentoMensal ? "S" : "N";
      fechamentoSubmit.indGerarBaseDadosFec = this.execucaoImediataGerarBaseDados ? "S" : "N";
      fechamentoSubmit.indAgendRelMovAutomatico = this.execucaoImediataAgendAutRelMov ? "S" : "N";
      // fechamentoSubmit.execucaoImediataParcial = this.execucaoImediataDataPrevia.toLocaleDateString("pt-BR").split('/').reverse().join('-') == new Date().toLocaleDateString("pt-BR").split('/').reverse().join('-');
    }

    if (this.dataEspecifica) {
      if (this.dataEspecificaExecutarData.toString() == 'Invalid Date') { this.messageService.MsgBox2('Executar na data não informada.', 'Não foi possível fazer o agendamento', 'warning', 'Ok'); return; }
      if (this.dataEspecificaDataPrevia == null) { this.messageService.MsgBox2('Data da prévia não informada.', 'Não foi possível fazer o agendamento', 'warning', 'Ok'); return; }
      fechamentoSubmit.periodicidadeExecucao = 1;
      fechamentoSubmit.dataEspecifica = this.dataEspecificaExecutarData.toLocaleDateString("pt-BR").split('/').reverse().join('-');
      fechamentoSubmit.mesContabil = this.dataEspecificaMesContabil;
      fechamentoSubmit.anoContabil = this.dataEspecificaAnoContabil;
      fechamentoSubmit.dataPrevia = this.dataEspecificaDataPrevia.toLocaleDateString("pt-BR").split('/').reverse().join('-');
      fechamentoSubmit.indFechamentoMensal = this.dataEspecificaFechamentoMensal ? "S" : "N";
      fechamentoSubmit.indGerarBaseDadosFec = this.dataEspecificaGerarBaseDados ? "S" : "N";
      fechamentoSubmit.indAgendRelMovAutomatico = this.dataEspecificaAgendAutRelMov ? "S" : "N";
      // fechamentoSubmit.dataEspecificaParcial = this.dataEspecificaDataPrevia.toLocaleDateString("pt-BR").split('/').reverse().join('-') == this.dataEspecificaExecutarData.toLocaleDateString("pt-BR").split('/').reverse().join('-');
    }

    if (this.diaria) {
      if (this.diariaPreviaInicio == undefined) { this.messageService.MsgBox2('Data inicial da prévia não informada.', 'Não foi possível fazer o agendamento', 'warning', 'Ok'); return; }
      if (this.diariaPreviaFim == undefined) { this.messageService.MsgBox2('Data final da prévia não informada.', 'Não foi possível fazer o agendamento', 'warning', 'Ok'); return; }
      fechamentoSubmit.periodicidadeExecucao = 2;
      fechamentoSubmit.dataDiariaIni = this.diariaPreviaInicio.toLocaleDateString("pt-BR").split('/').reverse().join('-');
      fechamentoSubmit.dataDiariaFim = this.diariaPreviaFim.toLocaleDateString("pt-BR").split('/').reverse().join('-');
      fechamentoSubmit.indSomenteDiaUtil = this.diariaExecutarPreviaDiaUtil ? "S" : "N";
      fechamentoSubmit.indAgendRelMovAutomatico = this.diariaAgendAutRelMov ? "S" : "N";
    }

    if (this.semanal) {
      if (this.semanalDiaPrevia == "") { this.messageService.MsgBox2('Data da prévia não informada.', 'Não foi possível fazer o agendamento', 'warning', 'Ok'); return; }
      fechamentoSubmit.periodicidadeExecucao = 3;
      fechamentoSubmit.diaDaSemana = parseInt(this.semanalDiaPrevia);
      fechamentoSubmit.indAgendRelMovAutomatico = this.semanalAgendAutRelMov ? "S" : "N";
    }

    if (this.mensal) {
      fechamentoSubmit.periodicidadeExecucao = 4;
      fechamentoSubmit.indUltimoDiaDoMes = this.mensalUltimoDiaMes ? "S" : "N";
      fechamentoSubmit.indFechamentoMensal = this.mensalFechamentoMensal ? "S" : "N";
      fechamentoSubmit.indGerarBaseDadosFec = this.mensalGerarBaseDados ? "S" : "N";
      fechamentoSubmit.indAgendRelMovAutomatico = this.mensalAgendAutRelMov ? "S" : "N";
      fechamentoSubmit.diaDoMes = this.mensalUltimoDiaMes == true ? ((new Date(new Date().getFullYear(), new Date().getMonth() + 1, 0)).getDate()).toString() : this.mensalDiaPrevia;
      if (fechamentoSubmit.mensalDiaPrevia == "") { this.messageService.MsgBox2('Data da prévia não informada.', 'Não foi possível fazer o agendamento', 'warning', 'Ok'); return; }
    }

    if (this.qtdEmpresasMarcadas == 0) { this.messageService.MsgBox2('Nenhuma empresa centralizadora selecionada.', 'Não foi possível fazer o agendamento', 'warning', 'Ok'); return; }
    if (this.fechamentoNumeroMeses == null) { this.messageService.MsgBox2('Nº de meses não informado.', 'Não foi possível fazer o agendamento', 'warning', 'Ok'); return; }

    this.fechamentoDto = fechamentoSubmit;

    if (!this.editou) {
      this.service.incluirAgendamento(this.fechamentoDto).subscribe(res => {
        this.messageService.MsgBox2('', 'Agendamento realizado com sucesso', 'success', 'Ok').then(resposta => {
          if (resposta.value) {
            this.confirm();
          }
        });
      }, err => {
        if (!err.error.includes("exception")) {
          this.messageService.MsgBox2('', err.error, 'warning', 'Ok');
        } else {
          this.messageService.MsgBox2('', 'Não foi possível fazer o agendamento', 'warning', 'Ok');
        }
      });
    }
    if (this.editou) {
      this.service.editarAgendamento(this.id, this.fechamentoDto).subscribe(res => {
        this.messageService.MsgBox2('', 'Agendamento atualizado com sucesso', 'success', 'Ok').then(resposta => {
          if (resposta.value) {
            this.confirm();
          }
        });
      }, err => {
        if (!err.error.includes("exception")) {
          this.messageService.MsgBox2('', err.error, 'warning', 'Ok');
        } else {
          this.messageService.MsgBox2('', 'Não foi possível atualizar o agendamento', 'warning', 'Ok');
        }
      });
    }
  }

  marcarTodas(marcar?: boolean) {    
    let marcarTodas = (<HTMLInputElement>document.getElementById('marcarTodas'))

    if (marcar != undefined)
      marcar ? marcarTodas.checked = true : marcarTodas.checked = false;

    if (marcarTodas.checked) {
      let marcarEmpresa = document.querySelectorAll("input[name='empresas']:not(:checked)")
      let empresa = (<HTMLInputElement><unknown>document.querySelectorAll("input[name='empresas']:not(:checked)"))
      for (let i = 0; i < marcarEmpresa.length; i++) {
        empresa[i].checked = true
      }
      this.qtdEmpresasMarcadas = document.querySelectorAll("input[name='empresas']:checked").length
    }
    else {
      let marcarEmpresa = document.querySelectorAll("input[name='empresas']:checked")
      let empresa = (<HTMLInputElement><unknown>document.querySelectorAll("input[name='empresas']:checked"))

      for (let i = 0; i < marcarEmpresa.length; i++) {
        empresa[i].checked = false
      }
      this.qtdEmpresasMarcadas = document.querySelectorAll("input[name='empresas']:checked").length
    }
  }

  desmarcarTodas() {
    let marcarTodas = (<HTMLInputElement>document.getElementById('marcarTodas'))
    let empresa = document.querySelectorAll("input[name='empresas']:not(:checked)")
    marcarTodas.checked = empresa.length == 0
    this.qtdEmpresasMarcadas = document.querySelectorAll("input[name='empresas']:checked").length
  }

  opcaoExecucao(item: string, editar?) {
    let check = (document.getElementById(item) as HTMLInputElement)

    editar ? check.checked = true : ''

    switch (item) {
      case 'execucaoImediataFechamentoMensal':
        check.checked ? this.execucaoImediataFechamentoMensal = true : this.execucaoImediataFechamentoMensal = false;
        break
      case 'execucaoImediataGerarBaseDados':
        check.checked ? this.execucaoImediataGerarBaseDados = true : this.execucaoImediataGerarBaseDados = false;
        break
      case 'execucaoImediataAgendAutRelMov':
        check.checked ? this.execucaoImediataAgendAutRelMov = true : this.execucaoImediataAgendAutRelMov = false;
        break
      case 'dataEspecificaFechamentoMensal':
        check.checked ? this.dataEspecificaFechamentoMensal = true : this.dataEspecificaFechamentoMensal = false;
        break;
      case 'dataEspecificaGerarBaseDados':
        check.checked ? this.dataEspecificaGerarBaseDados = true : this.dataEspecificaGerarBaseDados = false;
        break;
      case 'dataEspecificaAgendAutRelMov':
        check.checked ? this.dataEspecificaAgendAutRelMov = true : this.dataEspecificaAgendAutRelMov = false;
        break
      case 'diariaExecutarPreviaDiaUtil':
        check.checked ? this.diariaExecutarPreviaDiaUtil = true : this.diariaExecutarPreviaDiaUtil = false;
        break;
      case 'diariaAgendAutRelMov':
        check.checked ? this.diariaAgendAutRelMov = true : this.diariaAgendAutRelMov = false;
        break
      case 'semanalAgendAutRelMov':
        check.checked ? this.semanalAgendAutRelMov = true : this.semanalAgendAutRelMov = false;
        break
      case 'mensalUltimoDiaMes':
        check.checked ? this.mensalUltimoDiaMes = true : this.mensalUltimoDiaMes = false;
        this.mensalUltimoDiaMes == true ? this.mensalDiaPrevia = '' : this.mensalDiaPrevia;
        break;
      case 'mensalFechamentoMensal':
        check.checked ? this.mensalFechamentoMensal = true : this.mensalFechamentoMensal = false;
      case 'mensalGerarBaseDados':
        check.checked ? this.mensalGerarBaseDados = true : this.mensalGerarBaseDados = false;
        break;
      case 'mensalAgendAutRelMov':
        check.checked ? this.mensalAgendAutRelMov = true : this.mensalAgendAutRelMov = false;
        break
    }
  }

  confirm() {
    this.onConfirm.emit();
  }

  close() {
    this.onCancel.emit();
  }

  //#region EDITAR

  novovalor(editar?, item?) {
    if (editar != true) return;

    this.fechamentoNumeroMeses = item.numeroDeMeses
    this.fechamentoNaoAplicarOutilier = item.fechamentoNaoAplicarOutilier
    item.valAjusteDesvioPadrao == null && item.valPercentProcOutliers == null ? setTimeout(() => { this.marcarTipoExecucao('fechamentoNaoAplicarOutilier') }, 100) : '';

    if (item.valAjusteDesvioPadrao != null || item.valPercentProcOutliers != null) {
      this.fechamentoAplicarOutilier = true;
      this.marcarTipoExecucao('fechamentoAplicarOutilier');

      if (item.valAjusteDesvioPadrao != null) {
        this.fechamentoDesvioPadraoCheck = true;
        this.outlierCheck('fechamentoDesvioPadraoCheck');
        this.marcarTipoExecucao('fechamentoDesvioPadraoCheck');
        this.fechamentoDesvioPadrao = item.valAjusteDesvioPadrao;
      }

      if (item.valPercentProcOutliers != null) {
        this.fechamentoAplicarExclusaoOutlierCheck = true;
        this.outlierCheck('fechamentoAplicarExclusaoOutlierCheck');
        this.marcarTipoExecucao('fechamentoAplicarExclusaoOutlierCheck');
        this.fechamentoPercAplicarExclusaoOutlier = item.valPercentProcOutliers
      }
    }
  }

  marcarTipoExecucao(execucao) {
    setTimeout(() => {
      let marcar = (<HTMLInputElement>document.getElementById(execucao))
      marcar.checked = true;
    }, 100)
  }

  edicaoDados(edicao: boolean) {
    if (edicao) {
      this.marcarEmpresasSelecionadas()
      let execucao = this.fechamentoEditado.periodicidadeExecucao.toLowerCase()
      execucao = execucao == "execução imediata" ? 'execucaoImediata' : execucao == "data específica" ? 'dataEspecifica' : execucao
      this.tipoExecucao(execucao)
      this.preencherExecucao(execucao, this.fechamentoEditado)
      return;
    }
   // (document.getElementById('execucaoImediataAgendAutRelMov') as HTMLInputElement).checked = true
    this.marcarTodas(true)
  }

  marcarEmpresasSelecionadas() {
    if (this.fechamentoEditado.empresasCentralizadoras.length == this.empresas.length) {
      return this.marcarTodas(true)
    }
    let empresas = [];
    let empresa = document.querySelectorAll("input[name='empresas']")
    for (let i = 0; i < empresa.length; i++) {
      empresas.push({
        id: empresa[i].textContent,
        name: empresa[i]
      })
    }

    for (let i = 0; i < this.fechamentoEditado.empresasCentralizadoras.length; i++) {
      let empresaSelecionada = empresas.find(emp => emp.id == this.fechamentoEditado.empresasCentralizadoras[i].id)
      let marcar = (<HTMLInputElement><unknown>document.querySelectorAll(`input[id='${empresaSelecionada.name.id}']:not(:checked)`))
      marcar[0].checked = empresaSelecionada ? true : false;
    }
    this.desmarcarTodas()
  }

  preencherExecucao(execucao: string, item) {
    switch (execucao) {
      case 'execucaoImediata':
        this.execucaoImediataDataPrevia = this.convertToDate(item.dataPreviaEi);
        this.execucaoImediataMesContabil = item.mesContabilEi;
        this.execucaoImediataAnoContabil = item.anoContabilEi;
        if (item.indFechamentoMensalEi == "S") {
          setTimeout(() => {
            this.opcaoExecucao('execucaoImediataFechamentoMensal', true);
          }, 200)
        }
        if (item.indGerarBaseDadosFecEi == "S") {
          setTimeout(() => {
            this.opcaoExecucao('execucaoImediataGerarBaseDados', true);
          }, 200)
        }
        if (item.indAgendRelMovAutomatico == "S") {
          setTimeout(() => {
            this.opcaoExecucao('execucaoImediataAgendAutRelMov', true);
          }, 200)
        } else {
          setTimeout(() => {
            this.execucaoImediataAgendAutRelMov = false;
            (<HTMLInputElement>document.getElementById('execucaoImediataAgendAutRelMov')).checked = false;
          }, 200)
        }
        break
      case 'dataEspecifica':
        this.dataEspecificaExecutarData = this.convertToDate(item.dataEspecifica);
        this.execucaoImediataMesContabil = item.mesContabilDE;
        this.execucaoImediataAnoContabil = item.anoContabilDE;
        this.dataEspecificaDataPrevia = this.convertToDate(item.dataPreviaDE);
        if (item.indicaFechamentoMensalDE == "S") {
          setTimeout(() => {
            this.opcaoExecucao('dataEspecificaFechamentoMensal', true);
          }, 200)
        }
        if (item.indGerarBaseDadosFecDE == "S") {
          setTimeout(() => {
            this.opcaoExecucao('dataEspecificaGerarBaseDados', true);
          }, 200)
        }
        if (item.indAgendRelMovAutomatico == "S") {
          setTimeout(() => {
            this.opcaoExecucao('dataEspecificaAgendAutRelMov', true);
          }, 200)
        } else {
          setTimeout(() => {
            this.dataEspecificaAgendAutRelMov = false;
            (<HTMLInputElement>document.getElementById('dataEspecificaAgendAutRelMov')).checked = false;
          }, 200)
        }
        break
      case 'diaria':
        this.diariaPreviaInicio = this.convertToDate(item.dataDiariaIniDI)
        this.diariaPreviaFim = this.convertToDate(item.dataDiariaFimDI)
        if (item.indicaSomenteDiaUtilDI == "S") {
          setTimeout(() => {
            this.opcaoExecucao('diariaExecutarPreviaDiaUtil', true);
          }, 200)
        };
        if (item.indAgendRelMovAutomatico == "S") {
          setTimeout(() => {
            this.opcaoExecucao('diariaAgendAutRelMov', true);
          }, 200)
        } else {
          setTimeout(() => {
            this.diariaAgendAutRelMov = false;
            (<HTMLInputElement>document.getElementById('diariaAgendAutRelMov')).checked = false;
          }, 200)
        }
        break
      case 'semanal':
        this.semanalDiaPrevia = item.diaDaSemana.toString();
        if (item.indAgendRelMovAutomatico == "S") {
          setTimeout(() => {
            this.opcaoExecucao('semanalAgendAutRelMov', true);
          }, 200)
        }
        else {
          setTimeout(() => {
            this.semanalAgendAutRelMov = false;
            (<HTMLInputElement>document.getElementById('semanalAgendAutRelMov')).checked = false;
          }, 200)
        }
        break
      case 'mensal':
        if (item.indUltimoDiaDoMes == "S") {
          setTimeout(() => {
            this.opcaoExecucao('mensalUltimoDiaMes', true);
          }, 200)
        }
        this.mensalDiaPrevia = item.diaDoMes.toString();
        if (item.indicaFechamentoMensalME == "S") {
          setTimeout(() => {
            this.opcaoExecucao('mensalFechamentoMensal', true);
          }, 200)
        }
        if (item.indGerarBaseDadosFecME == "S") {
          setTimeout(() => {
            this.opcaoExecucao('mensalGerarBaseDados', true);
          }, 200)
        }
        if (item.indAgendRelMovAutomatico == "S") {
          setTimeout(() => {
            this.opcaoExecucao('mensalAgendAutRelMov', true);
          }, 200)
        } else {
          setTimeout(() => {
            this.mensalAgendAutRelMov = false;
            (<HTMLInputElement>document.getElementById('mensalAgendAutRelMov')).checked = false;
          }, 200)
        }
        break
    }
  }

  convertToDate(date) {
    var dateParts = ""
    if (date.includes("/")) {
      dateParts = date.slice(0, 10).split("/");
      return new Date(dateParts[1] + '/' + dateParts[0] + '/' + dateParts[2])
    }
    dateParts = date.slice(0, 10).split("-");
    return new Date(dateParts[1] + '/' + dateParts[2] + '/' + dateParts[0])
  }
  //#endregion
}