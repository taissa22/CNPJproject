import { CargaDeDocumentosService } from './../../services/carga-de-documentos.service';
import { Component, OnInit, Input, Output, EventEmitter, DoCheck } from '@angular/core';
import { StatusAgendamento } from '../../models/status-agendamento.enum';
import { Documento } from '../../models/documento.model';
import { TipoArquivo } from '../../models/tipo-arquivo.model';

@Component({
  selector: 'listagem-carga-de-documentos',
  templateUrl: './listagem-carga-de-documentos.component.html',
  styleUrls: ['./listagem-carga-de-documentos.component.scss']
})
export class ListagemCargaDeDocumentosComponent implements OnInit, DoCheck {

  @Input() documentos: Array<Documento> = [];
  @Input() totalAgendamentos: number = 0;
  @Output() obterAgendamentos: EventEmitter<void> = new EventEmitter();
  @Output() excluir: EventEmitter<number | string> = new EventEmitter();

  quantidadeAgendamentos: number = 0;

  statusAgendamento = StatusAgendamento;

  constructor(private service: CargaDeDocumentosService) { }

  ngOnInit() {
  }

  ngDoCheck() {
    this.quantidadeAgendamentos = this.documentos.length;
  }

  obterMaisAgendamentos(): void {
    this.obterAgendamentos.emit();
  }

  excluirAgendamento(id: number | string): void {
    this.excluir.emit(id);
  }

  downloadArquivosCarregados(id: number | string): string {
    return this.service.downloadArquivoURL(TipoArquivo.ARQUIVO_CARREGADO, id);
  }

  downloadResultaCarga(id: number | string): string {
    return this.service.downloadArquivoURL(TipoArquivo.RESULTADO_CARGA, id);
  }

}
