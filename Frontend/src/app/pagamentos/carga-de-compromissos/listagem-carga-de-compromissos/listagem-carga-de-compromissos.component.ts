import { Component, OnInit, Input, Output, EventEmitter, DoCheck, ViewChild, AfterViewInit } from '@angular/core';
import { StatusAgendamento } from '../../models/status-agendamento.enum';
import { Compromisso } from '../../models/compromisso.model';
import { CargaDeCompromissosService } from '../../services/carga-de-compromissos.service';
import { SisjurPaginator } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.component';
import { DialogService } from '@shared/services/dialog.service';

@Component({
  selector: 'listagem-carga-de-compromissos',
  templateUrl: './listagem-carga-de-compromissos.component.html',
  styleUrls: ['./listagem-carga-de-compromissos.component.scss']
})
export class ListagemCargaDeCompromissosComponent implements OnInit, DoCheck {

  @Input() compromissos: Array<Compromisso> = [];
  @Input() totalCompromissos: number = 0;
  @Input() quantidadeAgendamentos: number = 0;
  @Output() pageIndex: EventEmitter<number> = new EventEmitter();
  @Output() pageSize:EventEmitter<number> = new EventEmitter(); 
  @Output() obterAgendamentos: EventEmitter<void> = new EventEmitter();
  @Output() excluir: EventEmitter<number | string> = new EventEmitter();
  @ViewChild(SisjurPaginator, { static: false }) paginator: SisjurPaginator;
  
  statusAgendamento = StatusAgendamento;
  
  tamanhoPagina: number = 0;

  constructor(private service: CargaDeCompromissosService,
    private dialog: DialogService
  ) { }

  ngOnInit() {
    if (this.paginator !=null)
      this.paginator.pageSize = this.tamanhoPagina;

    this.iniciaValoresDaView();
  }
  
  iniciaValoresDaView() {
    return {
      pageIndex: this.paginator === undefined ? 1 : this.paginator.pageIndex + 1,
      pageSize: this.paginator === undefined ? 8 : this.paginator.pageSize,
    }
  }

  ngDoCheck() {
    if (this.compromissos)
      this.quantidadeAgendamentos = this.compromissos.length;
  }

  obterMaisAgendamentos(): void {  
    this.pageIndex.emit(this.iniciaValoresDaView().pageIndex);
    this.pageSize.emit(this.iniciaValoresDaView().pageSize);
    this.obterAgendamentos.emit();
  }

  async excluirAgendamento(id: number | string){
    const podeExcluir = await this.dialog.confirm('Excluir Agendamento', 'Deseja excluir o agendamento?');
    if (podeExcluir){
      this.excluir.emit(id);      
    }
  }

  downloadArquivosCarregados(id: number | string): string {
    return this.service.downloadArquivoURL(id,1);
  }

  downloadResultaCarga(id: number | string): string {
    return this.service.downloadArquivoURL(id,2);
  }

}
