import { AfterContentChecked, AfterViewInit, Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

// 3rd party
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';

// core & shared
import { StaticInjector } from '@manutencao/static-injector';
import { DialogService } from '@shared/services/dialog.service';

// local
import { TiposProcesso } from '@manutencao/models/tipos-de-processos';
import { HttpErrorResult } from '@core/http';
import { EventoService } from '@manutencao/services/evento.service';
import { Evento } from '@manutencao/models/evento.model';
import { DualListModel } from '@core/models/dual-list.model';
import { catchError } from 'rxjs/operators';
import { EMPTY } from 'rxjs';
import { EventoDisponivel } from '@manutencao/models/evento-disponivel.model';
import { deepEqual } from 'assert';

@Component({
  selector: 'app-evento-dependente-modal',
  templateUrl: './evento-dependente-modal.component.html',
  styleUrls: ['./evento-dependente-modal.component.scss']
})
export class EventoDependenteModalComponent  implements AfterViewInit{
  [x: string]: any;

constructor( 
    private modal: NgbActiveModal,
    private dialogService: DialogService,
    private service: EventoService
  ) {}

  datasource: Array<EventoDisponivel> = [];
  
  eventoId : number;

  listaEventos: DualListModel[] = [];
  listaInicial : DualListModel[] = [];
   
  close(): void {
    this.modal.close(false);
  }

  ngAfterViewInit(): void {
      this.buscar();
  }

  async buscar() {
    this.datasource = await this.service.obterDisponiveis(this.eventoId);    
    this.listaEventos = this.datasource.map(item => ({ 
                                                        id: item.id, 
                                                        label : item.label, 
                                                        selecionado : item.selecionado
                                                      }));

    this.listaInicial = this.listaEventos.map(item => ({...item}));
  }
  
  private  houveAlteracao() : Boolean {    
    return JSON.stringify(this.listaInicial) !== JSON.stringify(this.listaEventos);   
  }

   pegaDiferenca(): Array<DualListModel> {
    const diferenca: Array<DualListModel> = [];

     this.listaEventos.forEach((item, index) => {
      if (this.listaInicial[index].selecionado !== item.selecionado) {
        diferenca.push(item);
      }
    })

    return diferenca;
  }

  async save(){
    try{
      if (this.houveAlteracao()){
        await this.service.alterarDependentes(this.pegaDiferenca(),this.eventoId);       
      }
      await this.dialogService.alert(`Operacão realizada com sucesso`);
      this.modal.close(true);
    }  catch (error) {
      console.log(error);
      await this.dialogService.err(`Operacão não realizada`, (error as HttpErrorResult).messages.join('\n'));
    };     
  }
  
  // tslint:disable-next-line: member-ordering
  public static exibeModal(eventoId : number): Promise<boolean> {
    // prettier-ignore
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(EventoDependenteModalComponent, {windowClass: 'evento-dependente-modal', centered: true, size: 'lg', backdrop: 'static' });
    modalRef.componentInstance.eventoId  = eventoId;    
    return modalRef.result;
  }
 
}
