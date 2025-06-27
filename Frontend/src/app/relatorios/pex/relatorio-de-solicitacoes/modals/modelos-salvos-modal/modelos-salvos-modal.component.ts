import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { RelatorioDeSolicitacoesService } from '@relatorios/services/relatorios-de-solicitacoes.service';
import { DialogService } from '@shared/services/dialog.service'; 
import { StaticInjector } from '../../static-injector';
import { ModeloModel } from '../../models/modelo.model';
import { SalvarComoModeloModalComponent } from '../salvar-como-modelo-modal/salvar-como-modelo-modal.component';

@Component({
  selector: 'app-modelos-salvos',
  templateUrl: './modelos-salvos-modal.component.html',
  styleUrls: ['./modelos-salvos-modal.component.scss']
})
export class ModelosSalvosModalComponent implements OnInit { 
  tipoDePesquisaToggle = "OrdemAlfabetica"; // opçoes "OrdemAlfabetica" ou "DataAtualizacao"
  modelos:ModeloModel[] = [];
  pesquisa = "";
  constructor(public modal: NgbActiveModal, public service : RelatorioDeSolicitacoesService, public dialog : DialogService) { }

  ngOnInit() { 
    this.buscarModelos();
  }
   
  buscarModelos(){
    this.modelos = [];
    this.service.ListarModelos(this.tipoDePesquisaToggle,this.pesquisa).then(modelos => {
      modelos.map(m => this.modelos.push(ModeloModel.fromObj(m))); 
    }); 
  }

  buscarModelosSkip(){ 
    this.service.ListarModelos(this.tipoDePesquisaToggle,this.pesquisa,this.modelos.length).then(modelos => {
      modelos.map(m => this.modelos.push(ModeloModel.fromObj(m))); 
    }); 
  }
  
  verModelo(modelo:ModeloModel){
    this.service.verModelo$.next(modelo);
    this.close();
  }

  async excluir(modelo:ModeloModel){  
    const excluirTipoDocumento: boolean = await this.dialog.confirm( 
      `Deseja excluir este modelo?`
    );

    if (!excluirTipoDocumento) {
      return;
    }
      try {
        await  this.service.deletarModelo(modelo.id).then(); 
        await this.dialog.alert('Modelo excluído com sucesso');  
        this.buscarModelos();
      } catch (error) { 
        await this.dialog.err( error.error);
      }  
  } 

  // ---- modal --
  static exibeModal(): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(ModelosSalvosModalComponent, { centered: true, size: 'lg', backdrop: 'static' });
     return modalRef.result;
  }
  
  close() {
    this.modal.close();
  }
  // ------
   
}


