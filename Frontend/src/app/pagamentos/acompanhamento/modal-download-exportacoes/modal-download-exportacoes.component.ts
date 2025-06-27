import { AfterViewInit, Component, OnInit } from '@angular/core';
import { StaticInjector } from '@manutencao/static-injector';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AcompanhamentoService } from '../../services/acompanhamento.service';
import {  FormControl } from '@angular/forms';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-modal-download-exportacoes',
  templateUrl: './modal-download-exportacoes.component.html',
  styleUrls: ['./modal-download-exportacoes.component.scss']
})
export class ModalDownloadExportacoesComponent implements AfterViewInit {
  
  filtroUsuario = { nome: 'nome', condicao: 'igual', valor: '' }; 
  agendamentos: any[] = [];
    
  constructor(     
    public activeModal: NgbActiveModal,
    private datePipe: DatePipe,
    private acompanhamentoService: AcompanhamentoService
  ) {
   
  }

  
  dataSolicitacaoInicioFormControl = new FormControl();
  dataSolicitacaoFimFormControl = new FormControl();
  nomeFormControl = new FormControl(''); 
  campoPesquisaFormControl = new FormControl('');
  tipoPesquisaFormControl = new FormControl(1);

  ngAfterViewInit() { 
    this.buscarAgendamentos();
  }

  buscarAgendamentos() {
     const dataFim = this.datePipe.transform(this.dataSolicitacaoFimFormControl.value, 'yyyy/MM/dd');
     const dataIni =this.datePipe.transform(this.dataSolicitacaoFimFormControl.value, 'yyyy/MM/dd');
 
    this.acompanhamentoService
      .obterAgendamentos(this.nomeFormControl.value, dataIni, dataFim,this.campoPesquisaFormControl.value, this.tipoPesquisaFormControl.value)
      .subscribe(
        response => {
          this.agendamentos = response;
        },
        error => {
          console.error('Erro ao buscar agendamentos:', error);
        }
      );
  }

  obterStatusDescricao(status: number): string {
    switch (status) {
      case 1:
        return 'Agendado';
      case 2:
        return 'Processando';
      case 3:
        return 'Finalizado';       
      case 4:
        return 'Erro';
    } 
  }

 
  exportacaoAgendamento(exportacao: number) : string{
    
   const agendamentoId = exportacao;
   return this.acompanhamentoService.downloadAgendamento(agendamentoId);
   
  // this.acompanhamentoService.downloadAgendamento(agendamentoId).subscribe(
  //   (response) => {
  //     const blob = new Blob([response], {type: 'application/zip' });
  //     const url = window.URL.createObjectURL(blob);
  //     const a = document.createElement('a');
  //     a.href = url;
  //     a.download = `exportacao_${agendamentoId}.zip`;
  //     document.body.appendChild(a);
  //     a.click();
  //     window.URL.revokeObjectURL(url);
  //   },
  //   (error) => {
  //     console.error('Erro ao tentar baixar exportação:', error);
  //   }
  // )   

  }

  fecharModal() {
    this.activeModal.dismiss();
  }

  confirmarDownload() {
    this.activeModal.close('confirm');
  }

  formatDate(date: any): string | null {
    if (!date) {
      return null;
    }

    const validDate = new Date(date);
    if (isNaN(validDate.getTime())) {
      console.error('Data inválida:', date);
      return null;
    }

    return validDate.toISOString().split('T')[0];
  }
}
