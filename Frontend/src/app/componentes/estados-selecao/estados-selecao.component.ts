import { AfterViewInit, Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Estados } from '@core/models';
import { Estado } from '@manutencao/models';
import { StaticInjector } from '@manutencao/static-injector';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TipoProcessoEnum } from '@shared/enums/tipo-processoEnum.enum';
import { DialogService } from '@shared/services/dialog.service';
import { element } from 'protractor';

@Component({
  selector: 'app-estados-selecao',
  templateUrl: './estados-selecao.component.html',
  styleUrls: ['./estados-selecao.component.scss']
})
export class EstadosSelecaoComponent implements AfterViewInit {
  tipoProcesso : number;
  estadosSelecionados: Array<{id: string, selecionado: boolean}> = [];

  estados: Array<Estado> = Estados.obterUfs().map((estado) => {
    return (
       new Estado(estado.id, estado.nome)
    )
  });

  ACFormControl : FormControl = new FormControl(false,[]);
  ALFormControl : FormControl = new FormControl(false,[]);
  AMFormControl : FormControl = new FormControl(false,[]);
  APFormControl : FormControl = new FormControl(false,[]);
  BAFormControl : FormControl = new FormControl(false,[]);
  CEFormControl : FormControl = new FormControl(false,[]);
  DFFormControl : FormControl = new FormControl(false,[]);
  ESFormControl : FormControl = new FormControl(false,[]);
  GOFormControl : FormControl = new FormControl(false,[]);
  MAFormControl : FormControl = new FormControl(false,[]);
  MGFormControl : FormControl = new FormControl(false,[]);
  MSFormControl : FormControl = new FormControl(false,[]);
  MTFormControl : FormControl = new FormControl(false,[]);
  PAFormControl : FormControl = new FormControl(false,[]);
  PBFormControl : FormControl = new FormControl(false,[]);
  PEFormControl : FormControl = new FormControl(false,[]);
  PIFormControl : FormControl = new FormControl(false,[]);
  PRFormControl : FormControl = new FormControl(false,[]);
  RJFormControl : FormControl = new FormControl(false,[]);
  RNFormControl : FormControl = new FormControl(false,[]);
  ROFormControl : FormControl = new FormControl(false,[]);
  RRFormControl : FormControl = new FormControl(false,[]);
  RSFormControl : FormControl = new FormControl(false,[]);
  SCFormControl : FormControl = new FormControl(false,[]);
  SEFormControl : FormControl = new FormControl(false,[]);
  SPFormControl : FormControl = new FormControl(false,[]);
  TOFormControl : FormControl = new FormControl(false,[]);
  TodosFormControl : FormControl = new FormControl(false,[]);


  constructor( private modal: NgbActiveModal,
               private dialogService: DialogService,) { }

  ngAfterViewInit(): void {


    
    if (this.estados){

      this.estados.forEach((estado) => {
        estado.selecionado = this.estadosSelecionados.find(estadoSelecionado => estadoSelecionado.id == estado.id && estadoSelecionado.selecionado == true) ? true : false
      });

      this.ACFormControl.setValue(this.verificaEstadoSelecionado(Estados.AC.id));
      this.ALFormControl.setValue(this.verificaEstadoSelecionado(Estados.AL.id));
      this.AMFormControl.setValue(this.verificaEstadoSelecionado(Estados.AM.id));
      this.APFormControl.setValue(this.verificaEstadoSelecionado(Estados.AP.id));
      this.BAFormControl.setValue(this.verificaEstadoSelecionado(Estados.BA.id));
      this.CEFormControl.setValue(this.verificaEstadoSelecionado(Estados.CE.id));
      this.DFFormControl.setValue(this.verificaEstadoSelecionado(Estados.DF.id));
      this.ESFormControl.setValue(this.verificaEstadoSelecionado(Estados.ES.id));
      this.GOFormControl.setValue(this.verificaEstadoSelecionado(Estados.GO.id));
      this.MAFormControl.setValue(this.verificaEstadoSelecionado(Estados.MA.id));
      this.MGFormControl.setValue(this.verificaEstadoSelecionado(Estados.MG.id));
      this.MSFormControl.setValue(this.verificaEstadoSelecionado(Estados.MS.id));
      this.MTFormControl.setValue(this.verificaEstadoSelecionado(Estados.MT.id));
      this.PAFormControl.setValue(this.verificaEstadoSelecionado(Estados.PA.id));
      this.PBFormControl.setValue(this.verificaEstadoSelecionado(Estados.PB.id));
      this.PEFormControl.setValue(this.verificaEstadoSelecionado(Estados.PE.id));
      this.PIFormControl.setValue(this.verificaEstadoSelecionado(Estados.PI.id));
      this.PRFormControl.setValue(this.verificaEstadoSelecionado(Estados.PR.id));
      this.RJFormControl.setValue(this.verificaEstadoSelecionado(Estados.RJ.id));
      this.RNFormControl.setValue(this.verificaEstadoSelecionado(Estados.RN.id));
      this.ROFormControl.setValue(this.verificaEstadoSelecionado(Estados.RO.id));
      this.RRFormControl.setValue(this.verificaEstadoSelecionado(Estados.RR.id));
      this.RSFormControl.setValue(this.verificaEstadoSelecionado(Estados.RS.id));
      this.SCFormControl.setValue(this.verificaEstadoSelecionado(Estados.SC.id));
      this.SEFormControl.setValue(this.verificaEstadoSelecionado(Estados.SE.id));
      this.SPFormControl.setValue(this.verificaEstadoSelecionado(Estados.SP.id));
      this.TOFormControl .setValue(this.verificaEstadoSelecionado(Estados.TO.id));
    }

  }

  atualizarRetorno(estadoId: string, selecionado: boolean){
    this.estados.forEach((estado) => {
       if (estado.id == estadoId) {
         estado.selecionado = selecionado;
       }
    })
  }

  save(){
   // this.estados.forEach((element) => { if (element.selecionado){{this.retorno.push( {id : element.id , selecionado : element.selecionado} )}}} );

    const listaEstados = this.estados.map(estado => {return({id : estado.id,selecionado : estado.selecionado})});
    this.modal.close(listaEstados);
  }

  close(){
    const listaEstados = this.estadosSelecionados.map(estado => {return({id : estado.id,selecionado : estado.selecionado})});
    this.modal.close(listaEstados);
  }

  verificaEstadoSelecionado(estadoId: string): boolean {
    
    return this.estados.find(estado => estado.id == estadoId).selecionado == true;
  }

  retornaEstadoSelecionado(estadoId: string): Estado {
    return this.estados.find(estado => estado.id == estadoId && estado.selecionado == true);
  }


  public static exibir(tipoProcesso : number, estadosSelecionados: Array<{id: string, selecionado: boolean}>): Promise<Array<{id: string, selecionado: boolean}>> {
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(EstadosSelecaoComponent, {windowClass: 'estados-modal', centered: true, size: 'md', backdrop: 'static' });
      modalRef.componentInstance.estadosSelecionados = estadosSelecionados;
      modalRef.componentInstance.tipoProcesso = tipoProcesso;
    return modalRef.result;
  }

}
