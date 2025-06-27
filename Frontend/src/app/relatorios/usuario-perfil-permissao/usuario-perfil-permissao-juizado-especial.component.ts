import { formatDate } from '@angular/common';
import { AfterViewInit, Component } from '@angular/core';
import { UsuarioPerfilPermissaoModel } from '../models/usuario-perfil-permissao';
import { CorStatusAgendamento } from '../models/cor-status-agendamento';
import { UsuarioPerfilPermissaoService } from '../services/usuario-perfil-permissao.service';
import { DialogService } from '@shared/services/dialog.service';
import { HttpErrorResult } from '@core/http';
@Component({
  selector: 'app-usuario-perfil-permissao-juizado-especial.component',
  templateUrl: './usuario-perfil-permissao-juizado-especial.component.html',
  styleUrls: ['./usuario-perfil-permissao-juizado-especial.component.scss']
})

export class UsuarioPerfilPermissaoComponent implements AfterViewInit {

  listaAgendamentos = [];
 
  paginaAtual: number = 1;
  totalRegistros: number = 0;

  vermais = true;
  popupAgendamento = false;

  usuarioPerfilPermissao: UsuarioPerfilPermissaoModel;
  required = false;

  constructor(
    private service: UsuarioPerfilPermissaoService,
    private dialogService: DialogService

  ) { }

  async ngAfterViewInit() {
    await this.atualizarLista();
    
  }

  async atualizarLista() {
    this.limparLista();

    const resultado = await this.service.obterPaginado(this.paginaAtual);
    
    this.totalRegistros = resultado.total;
    this.listaAgendamentos = [].concat(this.listaAgendamentos, resultado.data);

    this.vermais = this.listaAgendamentos.length < this.totalRegistros;
  }

  retornaCorStatusAgendamento(status: number) {
    if (status)
      return CorStatusAgendamento[status]
  }
  
  async verMais() {
    this.paginaAtual += 1;

    const resultado = await this.service.obterPaginado(this.paginaAtual);
    
    this.totalRegistros = resultado.total;
    this.listaAgendamentos = [].concat(this.listaAgendamentos, resultado.data);

    this.vermais = this.listaAgendamentos.length < this.totalRegistros;   
  }
  
  abrirPopupAgendamento() { 
    this.atualizarLista();
   
    this.required = false;   
    this.popupAgendamento = false;
  }

  fecharPopupAgendamento() {
    this.popupAgendamento = false;
  }

  async fazerAgendamento() {
    this.usuarioPerfilPermissao = new UsuarioPerfilPermissaoModel();    
    this.usuarioPerfilPermissao.dataSolicitacao = formatDate(new Date(), 'dd/MM/yyyy HH:mm:ss', 'pt_BR');
    this.usuarioPerfilPermissao.status = "0";

    try {
      await this.service.agendar(this.usuarioPerfilPermissao);      
      await this.dialogService.info('Agendamento realizado com sucesso');
      this.fecharPopupAgendamento();
      this.atualizarLista();        
    } catch (error) {
      await this.dialogService.err('Falha ao agendar', (error as HttpErrorResult).messages.join('\n'))
    }
  }

  async deletarSolicitacoesAgendadas(id: number) {
    
    const excluir = await this.dialogService.confirm('Excluir Agendamento','Deseja excluir o agendamento?');
    if (excluir) {
      await this.service.removerSolicitacoesAgendadas(id);
      this.atualizarLista();
    }
  }

  async download(id: number) {
    try {
      await this.service.downloadArquivos(id);
    } catch (error) {
       await this.dialogService.err('Falha ao baixar o arquivo', (error as HttpErrorResult).messages.join('\n'))
    }    
  }

  limparLista(): void {
    this.listaAgendamentos = [];
    this.paginaAtual = 1;
  }

}
