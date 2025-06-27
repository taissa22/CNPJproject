import { Comprovante } from '../../models/comprovante.model';
import { Component, OnInit, DoCheck, Input, Output, EventEmitter } from '@angular/core';
import { CargaComprovantesService } from '../../services/carga-comprovantes.service';
import { TipoArquivo } from '../../models/tipo-arquivo.model';
import { StatusAgendamento } from '../../models/status-agendamento.enum';

@Component({
  selector: 'listagem-carga-de-comprovantes',
  templateUrl: './listagem-carga-de-comprovantes.component.html',
  styleUrls: ['./listagem-carga-de-comprovantes.component.scss']
})
export class ListagemCargaDeComprovantesComponent implements OnInit, DoCheck {

  @Input() comprovantes: Array<Comprovante> = [];
  @Output() obterAgendamentos: EventEmitter<void> = new EventEmitter();
  @Output() excluir: EventEmitter<number | string> = new EventEmitter();

  @Input() totalAgendamentos: number = 0;

  quantidadeAgendamentos: number = 0;
  statusAgendamento = StatusAgendamento;

  constructor(private service: CargaComprovantesService) { }

  ngOnInit() {
  }

  ngDoCheck() {
    this.quantidadeAgendamentos = this.comprovantes.length;
  }

  obterMaisAgendamentos() {
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
