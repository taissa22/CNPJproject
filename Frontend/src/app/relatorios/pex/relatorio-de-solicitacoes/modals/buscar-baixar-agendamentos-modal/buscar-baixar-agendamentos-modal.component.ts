import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms'; 
import { NgbActiveModal, NgbDateAdapter, NgbDateNativeAdapter, NgbDateParserFormatter, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { RelatorioDeSolicitacoesService } from '@relatorios/services/relatorios-de-solicitacoes.service';
import { NgbDateCustomParserFormatter } from '@shared/dateformat';
import { DialogService } from '@shared/services/dialog.service';
import { StaticInjector } from '../../static-injector';
import Swal from 'sweetalert2'; 
import { DiasSemanaEnum } from '../../enuns/DiasSemanaRelatorio.enum';
import { TipoExecucaoRelatorioEnum } from '../../enuns/TipoExecucaoRelatorio.enum';
import { AgendamentoRelatorioSolicitacoesModel } from '../../models/agendamento-relatorio-de-solicitacoes.model';
import { MostrarApenasNoPeriodoComponent } from './mostrar-apenas-no-periodo/mostrar-apenas-no-periodo.component';
@Component({
  selector: 'app-buscar-baixar-agendamentos-modal',
  templateUrl: './buscar-baixar-agendamentos-modal.component.html',
  styleUrls: ['./buscar-baixar-agendamentos-modal.component.scss'],
  providers: [{ provide: NgbDateAdapter, useClass: NgbDateNativeAdapter }, { provide: NgbDateParserFormatter, useClass: NgbDateCustomParserFormatter }]
})
export class BuscarBaixarAgendamentosModalComponent implements OnInit {
  tipoDePesquisaToggle = "DataExecucao"; // opções "DataExecucao", "Relatorio"
  filtrarPor = "JaExecutados"; // opções "JaExecutados", "ExecucoesFuturas", "MostarApenasNoPeriodo"
  filtroDataIni = ''
  filtroDataFim = ''
  agendamentos: any[] = []
  historioAgendamento: any[] = [];
  pesquisa:string =""; 
  constructor(public modal: NgbActiveModal, public dialog: DialogService, public service: RelatorioDeSolicitacoesService) {
 
  }

  ngOnInit() {  
    this.listarAgendamentos();
    this.service.pesquisarPelasDatas$.subscribe(datas => {
      this.filtroDataIni = datas.dataIni;
      this.filtroDataFim = datas.dataFim;
      this.listarAgendamentos(); 
    });
  }

  abrirModalDatas(){ 
    MostrarApenasNoPeriodoComponent.exibeModal(); 
  }

  // ------ listar ------

  
  listarAgendamentos(idAgendamentoEditar = 0){
    this.agendamentos = [];
    this.service.ListarAgendamentos(this.filtrarPor,this.pesquisa,this.tipoDePesquisaToggle,this.filtroDataIni,this.filtroDataFim).then(agendamentos => {
      this.preencherListaDeAgendamentos(agendamentos,idAgendamentoEditar);
    });
  }
  listarAgendamentosSkip(){ 
    this.service.ListarAgendamentos(this.filtrarPor,this.pesquisa,this.tipoDePesquisaToggle,this.filtroDataIni,this.filtroDataFim,this.agendamentos.length).then(agendamentos => {
      this.preencherListaDeAgendamentos(agendamentos);
    });
  }

  preencherListaDeAgendamentos(agendamentos:any[],idAgendamentoEditar = 0){
    agendamentos.map(a => { 
      let ag:any = AgendamentoRelatorioSolicitacoesModel.retornaObj(a);
      ag.tipoExecucao = a.tipoExecucao ? TipoExecucaoRelatorioEnum[a.tipoExecucao].toString() : ""; 
      ag.diaSemana = a.diaSemana ? parseInt(a.diaSemana) ? DiasSemanaEnum[a.diaSemana].toString(): "" : ""; 
      if(ag.datUltExecucao) ag.datUltExecucao = ag.datUltExecucao.split(" ")[0];  
      // if(idAgendamentoEditar == a.idAgendamento) a.openEdit = true; 
      this.agendamentos.push({
        openEdit: false,
        openDetalhes: false,
        openHistorico: false,
        ...AgendamentoRelatorioSolicitacoesModel.retornaObj(a)
      }); 
    });
    this.agendamentos.map(a => {
      a.tipoExecucao = a.tipoExecucao ? TipoExecucaoRelatorioEnum[a.tipoExecucao].toString() : "";  
      a.diaSemana = a.diaSemana ? parseInt(a.diaSemana) ? DiasSemanaEnum[a.diaSemana].toString(): "" : ""; 
      if(idAgendamentoEditar == a.idAgendamento) a.openEdit = true; 
    });
  }

  static exibeModal(): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(this, { centered: true, size: 'lg', backdrop: 'static' });
    // modalRef.componentInstance.comarca = comarca;
    return modalRef.result;
  }
  close() {
    this.modal.close();
  }
  salvar() {

  }
  desabilitaTooltip(formControl: FormControl): boolean {
    return formControl.untouched || formControl.valid;
  }
  async swallEditarAgendamento(agendamento) {
    Swal.fire({
      html: "<p style='text-align: left;'>Ao editar um Agendamento (data e descrição), todos os relatórios executados a partir dele vêm com as novas </br> configurações. Deseja editar mesmo assim?</p>",
      cancelButtonText: "Voltar",
      showCancelButton: true,
      cancelButtonColor: '#aaa',
      confirmButtonText: "Confirmar edição",
      showConfirmButton: true,
      confirmButtonColor: '#6F62B2',
      reverseButtons: true

    }).then(resposta => {
      if (resposta.value) {
        this.agendamentos.map(a => a.openEdit = false);
        this.agendamentos.map(a => a.openDetalhes = false);
        this.agendamentos.map(a => a.openHistorico = false);
        agendamento.openEdit = true;
      }
    });


  }
  abrirDetalhesAgendamento(agendamento) {
    this.agendamentos.map(a => a.openDetalhes = false);
    this.agendamentos.map(a => a.openEdit = false);
    this.agendamentos.map(a => a.openHistorico = false);
    agendamento.openDetalhes = true
  }
  abrirHistorico(agendamento) {
    this.agendamentos.map(a => a.openDetalhes = false);
    this.agendamentos.map(a => a.openEdit = false);
    this.agendamentos.map(a => a.openHistorico = false);
    agendamento.openHistorico = true
  }

  async exportar(agendamento: any){ 
    try{
     await this.service.exportar(agendamento.nomeArquivoGerado ? agendamento.nomeArquivoGerado : "").then();
    } catch (error) { 
      await this.dialog.err("Arquivo não existe");
    }
  }

  async exportarLista(){ 
    try{
     await this.service.exportarLista().then();
    } catch (error) { 
      await this.dialog.err("Arquivo não existe");
    }
  }
}
