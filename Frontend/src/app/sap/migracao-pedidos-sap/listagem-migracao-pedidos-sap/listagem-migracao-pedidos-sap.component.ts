//import { CargaDeDocumentosService } from './../../services/carga-de-documentos.service';
import { Component, OnInit, Input, Output, EventEmitter, DoCheck } from '@angular/core';
import { DialogService } from '@shared/services/dialog.service';
import { StatusAgendamento } from '../../../pagamentos/models/status-agendamento.enum';
//import { Documento } from '../../models/documento.model';

import { MigracaoPedidos } from '../models/migracao-pedidos-sap';
import { TipoArquivo } from '../models/tipo-arquivo';
import { MigracaoPedidosSapServiceService } from '../services/migracao-pedidos-sap-service.service';

@Component({
  selector: 'app-listagem-migracao-pedidos-sap',
  templateUrl: './listagem-migracao-pedidos-sap.component.html',
  styleUrls: ['./listagem-migracao-pedidos-sap.component.scss']
})
export class ListagemMigracaoPedidosSapComponent implements OnInit, DoCheck {

  @Input() migracaoPedidos: Array<MigracaoPedidos> = [];
  @Input() totalAgendamentos: number = 0;
  @Output() obterAgendamentos: EventEmitter<void> = new EventEmitter();
  @Output() excluir: EventEmitter<number | string> = new EventEmitter();

  quantidadeAgendamentos: number = 0;

  statusAgendamento = StatusAgendamento;

  constructor(
    private service: MigracaoPedidosSapServiceService,
    private dialog: DialogService
  ) { }

  ngOnInit() {

    console.log(this.migracaoPedidos)
  }

  ngDoCheck() {
    this.quantidadeAgendamentos = this.migracaoPedidos.length;
  }

  obterMaisAgendamentos(): void {
    try {
      this.obterAgendamentos.emit();
    } catch {
      this.dialog.err('Ocorreu um erro interno', 'Tente novamente mais tarde.');
    }
  }

  async excluirAgendamento(id: number | string): Promise<void> {
    try {
      const result = await this.dialog.confirm('Excluir Agendamento', 'Deseja excluir este agendamento?');

      if (result) {
          this.excluir.emit(id);
      }
    } catch {
      this.dialog.err('Ocorreu um erro interno', 'Tente novamente mais tarde.');
    }
  }

  downloadArquivosCarregados(id: number | string): string {
    try {
      return this.service.downloadArquivoURL(TipoArquivo.ARQUIVO_CARREGADO, id);
    } catch {
      this.dialog.err('Ocorreu um erro interno', 'Tente novamente mais tarde.');
    }
  }

  downloadResultaCarga(id: number | string): string {
    try{
      return this.service.downloadArquivoURL(TipoArquivo.RESULTADO_MIGRACAO, id);
    } catch {
      this.dialog.err('Ocorreu um erro interno', 'Tente novamente mais tarde.');
    }
  }

}
