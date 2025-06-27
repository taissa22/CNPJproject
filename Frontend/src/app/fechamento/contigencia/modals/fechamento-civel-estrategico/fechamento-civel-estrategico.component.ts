import { AfterViewInit, Component, EventEmitter, OnInit, Output } from '@angular/core';
import { HelperAngular } from '@shared/helpers/helper-angular';
import { BsModalRef } from 'ngx-bootstrap';
import { AgendarSolicitarFechamentoContingenciaService } from 'src/app/fechamento/services/agendar-solicitar-fechamento-contingencia.service';
import { FechamentoDto } from 'src/app/fechamento/models/contigencia/agendar-solicitar-fechamento-contingencia/fechamento-dto.model';

@Component({
  selector: 'app-fechamento-civel-estrategico',
  templateUrl: './fechamento-civel-estrategico.component.html',
  styleUrls: ['./fechamento-civel-estrategico.component.scss']
})
export class FechamentoCivelEstrategicoComponent implements OnInit, AfterViewInit {

  @Output() onCancel = new EventEmitter();
  @Output() onConfirm = new EventEmitter();

  constructor(
    private bsModalRef: BsModalRef,
    private messageService: HelperAngular,
    private service: AgendarSolicitarFechamentoContingenciaService
  ) { }

  ngOnInit() {
    this.execucaoImediata = true;
    this.execucaoImediataFechamentoMensal = false;

    this.dataEspecifica = false;
    this.dataEspecificaFechamentoMensal = false;

    this.diaria = false;
    this.diariaExecutarPreviaDiaUtil = false;

    this.semanal = false;

    this.mensal = false;
    this.mensalUltimoDiaMes = false;
    this.mensalFechamentoMensal = false

    this.calcularDias();    
    this.obterEmpresas();
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
  diasDoMes = [];
  fechamentoPercHaircut: string;

  //#region  EXECUÇÃO IMEDIATA
  execucaoImediata: boolean;
  execucaoImediataDiaContabil = new Date().getDate();
  execucaoImediataDataPrevia = new Date(new Date().getFullYear(), new Date().getMonth(), this.execucaoImediataDiaContabil - 1);
  execucaoImediataMesContabil = this.execucaoImediataDataPrevia.getMonth();
  execucaoImediataAnoContabil = this.execucaoImediataDataPrevia.getFullYear();
  execucaoImediataDataAtual = new Date(new Date().getFullYear(), new Date().getMonth(), this.execucaoImediataDiaContabil);
  execucaoImediataFechamentoMensal: boolean;
  execucaoImediataParcial: boolean;

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
  dataEspecificaParcial: boolean;
  //#endregion

  //#region DIARIA
  diaria: boolean;
  diariaPreviaInicio: Date;
  diariaPreviaFim: Date;
  diariaExecutarPreviaDiaUtil: boolean;

  //#endregion

  //#region  SEMANAL
  semanal: boolean;
  semanalDiaPrevia = "";
  //#endregion

  //#region MENSAL
  mensal: boolean;
  mensalUltimoDiaMes: boolean;
  mensalFechamentoMensal: boolean;
  mensalDiaPrevia = ""
  //#endregion

  calcularDias() {
    let dias = ['1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11', '12', '13', '14', '15', '16', '17', '18', '19', '20', '21', '22', '23', '24', '25', '26', '27', '28']
    let ultimoDia = (new Date(new Date().getFullYear(), new Date().getMonth() + 1, 0)).getDate();

    ultimoDia == 29 ? dias.push('29') : ultimoDia == 30 ? dias.push('29', '30') : ultimoDia == 31 ? dias.push('29', '30', '31') : dias;

    this.diasDoMes = dias;
  }

  blur(item) {
    item == '' ? this.fechamentoPercHaircut = '0' : this.fechamentoPercHaircut;
  }

  obterHaircutPadrao() {
    this.service.obterHaircutPadrao(6).subscribe(result => {
      this.fechamentoPercHaircut = result.toString().replace('.', ',')
    });
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

  obterEmpresas() {
    this.service.obterEmpresas().subscribe(result => {
      this.empresas = result
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
    fechamentoSubmit.codTipoFechamento = 6;

    if (this.execucaoImediata) {
      if (this.execucaoImediataDataPrevia == null) { this.messageService.MsgBox2('Data da prévia não informada.', 'Não foi possível fazer o agendamento', 'warning', 'Ok'); return; }
      fechamentoSubmit.periodicidadeExecucao = 0
      fechamentoSubmit.indExecucaoImediata = "S";
      fechamentoSubmit.mesContabil = this.execucaoImediataMesContabil
      fechamentoSubmit.anoContabil = this.execucaoImediataAnoContabil
      fechamentoSubmit.dataPrevia = this.execucaoImediataDataPrevia.toLocaleDateString("pt-BR").split('/').reverse().join('-');
      fechamentoSubmit.indFechamentoMensal = this.execucaoImediataFechamentoMensal ? "S" : "N";
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
      // fechamentoSubmit.dataEspecificaParcial = this.dataEspecificaDataPrevia.toLocaleDateString("pt-BR").split('/').reverse().join('-') == this.dataEspecificaExecutarData.toLocaleDateString("pt-BR").split('/').reverse().join('-');
    }

    if (this.diaria) {
      if (this.diariaPreviaInicio == undefined) { this.messageService.MsgBox2('Data inicial da prévia não informada.', 'Não foi possível fazer o agendamento', 'warning', 'Ok'); return; }
      if (this.diariaPreviaFim == undefined) { this.messageService.MsgBox2('Data final da prévia não informada.', 'Não foi possível fazer o agendamento', 'warning', 'Ok'); return; }
      fechamentoSubmit.periodicidadeExecucao = 2;
      fechamentoSubmit.dataDiariaIni = this.diariaPreviaInicio.toLocaleDateString("pt-BR").split('/').reverse().join('-');
      fechamentoSubmit.dataDiariaFim = this.diariaPreviaFim.toLocaleDateString("pt-BR").split('/').reverse().join('-');
      fechamentoSubmit.indSomenteDiaUtil = this.diariaExecutarPreviaDiaUtil ? "S" : "N";
    }

    if (this.semanal) {
      if (this.semanalDiaPrevia == "") { this.messageService.MsgBox2('Data da prévia não informada.', 'Não foi possível fazer o agendamento', 'warning', 'Ok'); return; }
      fechamentoSubmit.periodicidadeExecucao = 3;
      fechamentoSubmit.diaDaSemana = parseInt(this.semanalDiaPrevia);
    }

    if (this.mensal) {
      fechamentoSubmit.periodicidadeExecucao = 4;
      fechamentoSubmit.indUltimoDiaDoMes = this.mensalUltimoDiaMes ? "S" : "N";
      fechamentoSubmit.indFechamentoMensal = this.mensalFechamentoMensal ? "S" : "N";
      fechamentoSubmit.diaDoMes = this.mensalUltimoDiaMes == true ? ((new Date(new Date().getFullYear(), new Date().getMonth() + 1, 0)).getDate()).toString() : this.mensalDiaPrevia;
      if (fechamentoSubmit.mensalDiaPrevia == "") { this.messageService.MsgBox2('Data da prévia não informada.', 'Não foi possível fazer o agendamento', 'warning', 'Ok'); return; }
    }

    if (this.qtdEmpresasMarcadas == 0) { this.messageService.MsgBox2('Nenhuma empresa centralizadora selecionada.', 'Não foi possível fazer o agendamento', 'warning', 'Ok'); return; }

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
      case 'dataEspecificaFechamentoMensal':
        check.checked ? this.dataEspecificaFechamentoMensal = true : this.dataEspecificaFechamentoMensal = false;
        break;
      case 'diariaExecutarPreviaDiaUtil':
        check.checked ? this.diariaExecutarPreviaDiaUtil = true : this.diariaExecutarPreviaDiaUtil = false;
        break;
      case 'mensalUltimoDiaMes':
        check.checked ? this.mensalUltimoDiaMes = true : this.mensalUltimoDiaMes = false;
        this.mensalUltimoDiaMes == true ? this.mensalDiaPrevia = '' : this.mensalDiaPrevia;
        break;
      case 'mensalFechamentoMensal':
        check.checked ? this.mensalFechamentoMensal = true : this.mensalFechamentoMensal = false;
        break;
    }
  }

  confirm() {
    this.onConfirm.emit();
  }

  close() {
    this.onCancel.emit();
  }

  //#region EDITAR
  edicaoDados(edicao: boolean) {
    if (edicao) {
      this.marcarEmpresasSelecionadas()
      let execucao = this.fechamentoEditado.periodicidadeExecucao.toLowerCase()
      execucao = execucao == "execução imediata" ? 'execucaoImediata' : execucao == "data específica" ? 'dataEspecifica' : execucao
      this.tipoExecucao(execucao)
      this.preencherExecucao(execucao, this.fechamentoEditado)
      return;
    }
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
        break
      case 'diaria':
        this.diariaPreviaInicio = this.convertToDate(item.dataDiariaIniDI)
        this.diariaPreviaFim = this.convertToDate(item.dataDiariaFimDI)
        if (item.indicaSomenteDiaUtilDI == "S") {
          setTimeout(() => {
            this.opcaoExecucao('diariaExecutarPreviaDiaUtil', true);
          }, 200)
        }
        break
      case 'semanal':
        this.semanalDiaPrevia = item.diaDaSemana.toString();
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